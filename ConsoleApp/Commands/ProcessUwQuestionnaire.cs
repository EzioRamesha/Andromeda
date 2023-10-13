using BusinessObject.Identity;
using BusinessObject.TreatyPricing;
using DataAccess.Entities.Identity;
using DataAccess.Entities.TreatyPricing;
using Newtonsoft.Json;
using Services.Identity;
using Services.TreatyPricing;
using Shared;
using Shared.ProcessFile;
using Shared.Trails;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;

namespace ConsoleApp.Commands
{
    public class ProcessUwQuestionnaire : Command
    {
        public List<Column> Columns { get; set; }

        public TreatyPricingUwQuestionnaireVersionFileBo QuestionnaireFileBo { get; set; }

        public List<TreatyPricingUwQuestionnaireVersionDetailBo> QuestionnaireBos { get; set; }

        public string FilePath { get; set; }

        public int FileType { get; set; }

        public int? AuthUserId { get; set; }

        public TextFile TextFile { get; set; }

        public Excel Excel { get; set; }

        public int Worksheet { get; set; }

        public List<string> Errors { get; set; }

        public const int FileTypeCsv = 1;
        public const int FileTypeExcel = 2;

        public ProcessUwQuestionnaire()
        {
            Title = "ProcessUwQuestionnaire";
            Description = "To read UW Questionnaire csv file and insert into database";
            Errors = new List<string> { };
            GetColumns();
        }

        public override bool Validate()
        {
            try
            {
                // nothing
            }
            catch (Exception e)
            {
                PrintError(e.Message);
                return false;
            }
            return base.Validate();

            //string filepath = CommandInput.Arguments[0];
            //if (!File.Exists(filepath))
            //{
            //    PrintError(string.Format(MessageBag.FileNotExists, filepath));
            //    return false;
            //}
            //else
            //{
            //    FilePath = filepath;
            //}

            //return base.Validate();
        }

        public override void Run()
        {
            if (TreatyPricingUwQuestionnaireVersionFileService.CountByStatus(TreatyPricingUwQuestionnaireVersionFileBo.StatusSubmitForProcessing) == 0)
            {
                Log = false;
                PrintMessage(MessageBag.NoBatchPendingProcess);
                return;
            }

            PrintStarting();

            while (LoadQuestionnaireVersionBo() != null)
            {
                Errors = new List<string>();

                if (!File.Exists(FilePath))
                    throw new Exception(string.Format(MessageBag.FileNotExists, FilePath));

                DeleteQuestionnaires();
                UpdateFileStatus(TreatyPricingUwQuestionnaireVersionFileBo.StatusProcessing, "Processing Questionnaire Data File");

                QuestionnaireBos = new List<TreatyPricingUwQuestionnaireVersionDetailBo> { };

                if (FileType == FileTypeCsv)
                {
                    try
                    {
                        TextFile = new TextFile(FilePath);
                        while (TextFile.GetNextRow() != null)
                        {
                            if (TextFile.RowIndex == 1)
                                continue; // Skip header row

                            if (PrintCommitBuffer()) { }
                            SetProcessCount();

                            bool error = false;

                            var entity = new TreatyPricingUwQuestionnaireVersionDetail();
                            TreatyPricingUwQuestionnaireVersionDetailBo b = null;
                            try
                            {
                                b = SetData();

                                if (string.IsNullOrEmpty(b.UwQuestionnaireCategoryCode))
                                {
                                    SetProcessCount("Category Empty");
                                    Errors.Add(string.Format("The Category is required at row {0}", TextFile.RowIndex));
                                    error = true;
                                }
                                else
                                {
                                    UwQuestionnaireCategoryBo categoryBo = UwQuestionnaireCategoryService.FindByCode(b.UwQuestionnaireCategoryCode);
                                    if (categoryBo == null)
                                    {
                                        SetProcessCount("Category Not Found");
                                        Errors.Add(string.Format("The Category doesn't exists: {0} at row {1}", b.UwQuestionnaireCategoryCode, TextFile.RowIndex));
                                        error = true;
                                    }
                                    else
                                    {
                                        b.UwQuestionnaireCategoryId = categoryBo.Id;
                                        b.UwQuestionnaireCategoryBo = categoryBo;
                                    }
                                }

                                if (string.IsNullOrEmpty(b.Question))
                                {
                                    SetProcessCount("Question Empty");
                                    Errors.Add(string.Format("The Question is required at row {0}", TextFile.RowIndex));
                                    error = true;
                                }
                                else
                                {
                                    var maxLengthAttr = entity.GetAttributeFrom<MaxLengthAttribute>("Question");
                                    if (maxLengthAttr != null && b.Question.Length > maxLengthAttr.Length)
                                    {
                                        SetProcessCount("Question exceeded max length");
                                        Errors.Add(string.Format("Question length can not be more than {0} characters at row {1}", maxLengthAttr.Length, TextFile.RowIndex));
                                        error = true;
                                    }
                                }

                            }
                            catch (Exception e)
                            {
                                SetProcessCount("Error");
                                Errors.Add(string.Format("Error Triggered: {0} at row {1}", e.Message, TextFile.RowIndex));
                                error = true;
                            }

                            if (error)
                                continue;

                            QuestionnaireBos.Add(b);
                        }

                        PrintProcessCount();

                        TextFile.Close();

                        if (Errors.Count > 0)
                        {
                            foreach (string error in Errors)
                            {
                                PrintMessage(error, true, false);
                                //PrintError(error);
                            }
                            QuestionnaireFileBo.Errors = JsonConvert.SerializeObject(Errors);
                            UpdateFileStatus(TreatyPricingUwQuestionnaireVersionFileBo.StatusFailed, "Process Questionnaire Data File Failed");
                        }
                        else
                        {
                            Save();
                            UpdateFileStatus(TreatyPricingUwQuestionnaireVersionFileBo.StatusSuccess, "Process Questionnaire Data File Success");
                        }
                    }
                    catch (Exception e)
                    {
                        if (TextFile != null)
                            TextFile.Close();

                        SetProcessCount("Error");
                        PrintMessage(e.Message);
                        Errors.Add(e.Message);

                        QuestionnaireFileBo.Errors = JsonConvert.SerializeObject(Errors);
                        UpdateFileStatus(TreatyPricingUwQuestionnaireVersionFileBo.StatusFailed, "Process Questionnaire Data File Failed");
                    }
                }
                else
                {
                    Worksheet = 1;

                    try
                    {
                        Excel = new Excel(FilePath, true, Worksheet);

                        //do something
                        while (Excel.GetNextRow() != null)
                        {
                            if (Excel.RowIndex == 1)
                                continue; // Skip header row

                            if (PrintCommitBuffer()) { }
                            SetProcessCount();

                            bool error = false;

                            var entity = new TreatyPricingUwQuestionnaireVersionDetail();
                            TreatyPricingUwQuestionnaireVersionDetailBo b = null;
                            try
                            {
                                b = new TreatyPricingUwQuestionnaireVersionDetailBo
                                {
                                    UwQuestionnaireCategoryCode = (Excel.XWorkSheet.Cells[Excel.RowIndex, TreatyPricingUwQuestionnaireVersionDetailBo.ColumnCode] as Microsoft.Office.Interop.Excel.Range).Value,
                                    Question = (Excel.XWorkSheet.Cells[Excel.RowIndex, TreatyPricingUwQuestionnaireVersionDetailBo.ColumnQuestion] as Microsoft.Office.Interop.Excel.Range).Value,
                                };

                                if (string.IsNullOrEmpty(b.UwQuestionnaireCategoryCode))
                                {
                                    SetProcessCount("Category Empty");
                                    Errors.Add(string.Format("The Category is required at row {0}", Excel.RowIndex));
                                    error = true;
                                }
                                else
                                {
                                    UwQuestionnaireCategoryBo categoryBo = UwQuestionnaireCategoryService.FindByCode(b.UwQuestionnaireCategoryCode);
                                    if (categoryBo == null)
                                    {
                                        SetProcessCount("Category Not Found");
                                        Errors.Add(string.Format("The Category doesn't exists: {0} at row {1}", b.UwQuestionnaireCategoryCode, Excel.RowIndex));
                                        error = true;
                                    }
                                    else
                                    {
                                        b.UwQuestionnaireCategoryId = categoryBo.Id;
                                        b.UwQuestionnaireCategoryBo = categoryBo;
                                    }
                                }

                                if (string.IsNullOrEmpty(b.Question))
                                {
                                    SetProcessCount("Question Empty");
                                    Errors.Add(string.Format("The Question is required at row {0}", Excel.RowIndex));
                                    error = true;
                                }
                                else
                                {
                                    var maxLengthAttr = entity.GetAttributeFrom<MaxLengthAttribute>("Question");
                                    if (maxLengthAttr != null && b.Question.Length > maxLengthAttr.Length)
                                    {
                                        SetProcessCount("Question exceeded max length");
                                        Errors.Add(string.Format("Question length can not be more than {0} characters at row {1}", maxLengthAttr.Length, Excel.RowIndex));
                                        error = true;
                                    }
                                }

                            }
                            catch (Exception e)
                            {
                                SetProcessCount("Error");
                                Errors.Add(string.Format("Error Triggered: {0} at row {1}", e.Message, Excel.RowIndex));
                                error = true;
                            }

                            if (error)
                                continue;

                            QuestionnaireBos.Add(b);
                        }

                        PrintProcessCount();

                        Excel.Close();

                        if (Errors.Count > 0)
                        {
                            foreach (string error in Errors)
                            {
                                PrintMessage(error, true, false);
                                //PrintError(error);
                            }
                            QuestionnaireFileBo.Errors = JsonConvert.SerializeObject(Errors);
                            UpdateFileStatus(TreatyPricingUwQuestionnaireVersionFileBo.StatusFailed, "Process Questionnaire Data File Failed");
                        }
                        else
                        {
                            Save();
                            UpdateFileStatus(TreatyPricingUwQuestionnaireVersionFileBo.StatusSuccess, "Process Questionnaire Data File Success");
                        }
                    }
                    catch (Exception e)
                    {
                        if (Excel != null)
                            Excel.Close();

                        SetProcessCount("Error");
                        PrintMessage(e.Message);
                        Errors.Add(e.Message);

                        QuestionnaireFileBo.Errors = JsonConvert.SerializeObject(Errors);
                        UpdateFileStatus(TreatyPricingUwQuestionnaireVersionFileBo.StatusFailed, "Process Questionnaire Data File Failed");
                    }
                }
            }

            PrintEnding();
        }

        public void DeleteQuestionnaires()
        {
            // DELETE ALL QUESTIONNAIRE BEFORE PROCESS
            PrintMessage("Deleting Questionnaire records...", true, false);
            TreatyPricingUwQuestionnaireVersionDetailService.DeleteAllByTreatyPricingUwQuestionnaireVersionId(QuestionnaireFileBo.TreatyPricingUwQuestionnaireVersionId);
            PrintMessage("Deleted Questionnaire records", true, false);
        }

        public TreatyPricingUwQuestionnaireVersionFileBo LoadQuestionnaireVersionBo()
        {
            QuestionnaireFileBo = TreatyPricingUwQuestionnaireVersionFileService.FindByStatus(TreatyPricingUwQuestionnaireVersionFileBo.StatusSubmitForProcessing);
            if (QuestionnaireFileBo != null)
            {
                FilePath = QuestionnaireFileBo.GetLocalPath();
                FileType = Path.GetExtension(QuestionnaireFileBo.FileName).Contains("csv") ? FileTypeCsv : FileTypeExcel;
            }

            return QuestionnaireFileBo;
        }

        public void UpdateFileStatus(int status, string description)
        {
            var questionnaireDataFile = QuestionnaireFileBo;
            questionnaireDataFile.Status = status;

            var trail = new TrailObject();
            var result = TreatyPricingUwQuestionnaireVersionFileService.Update(ref questionnaireDataFile, ref trail);

            var userTrailBo = new UserTrailBo(
                questionnaireDataFile.Id,
                description,
                result,
                trail,
                User.DefaultSuperUserId
            );
            UserTrailService.Create(ref userTrailBo);
        }

        public void Save()
        {
            if (QuestionnaireFileBo == null)
                return;
            if (!QuestionnaireBos.IsNullOrEmpty())
            {
                foreach(var questionnaireBo in QuestionnaireBos)
                {
                    questionnaireBo.TreatyPricingUwQuestionnaireVersionId = QuestionnaireFileBo.TreatyPricingUwQuestionnaireVersionId;
                    questionnaireBo.CreatedById = QuestionnaireFileBo.CreatedById;
                    questionnaireBo.UpdatedById = QuestionnaireFileBo.UpdatedById;
                    var bo = questionnaireBo;
                    TreatyPricingUwQuestionnaireVersionDetailService.Create(ref bo);
                }
            }
        }

        public TreatyPricingUwQuestionnaireVersionDetailBo SetData()
        {
            var b = new TreatyPricingUwQuestionnaireVersionDetailBo
            {
                UwQuestionnaireCategoryCode = TextFile.GetColValue(TreatyPricingUwQuestionnaireVersionDetailBo.ColumnCode),
                Question = TextFile.GetColValue(TreatyPricingUwQuestionnaireVersionDetailBo.ColumnQuestion),
            };
            return b;
        }

        public List<Column> GetColumns()
        {
            Columns = TreatyPricingUwQuestionnaireVersionDetailBo.GetColumns();
            return Columns;
        }
    }
}
