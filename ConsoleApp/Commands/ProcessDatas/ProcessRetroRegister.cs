using BusinessObject;
using DataAccess.Entities;
using DataAccess.Entities.Identity;
using Services;
using Shared;
using Shared.ProcessFile;
using Shared.Trails;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace ConsoleApp.Commands.ProcessDatas
{
    public class ProcessRetroRegister : Command
    {
        public List<Column> Columns { get; set; }

        public string FilePath { get; set; }

        public HttpPostedFileBase PostedFile { get; set; }

        public int? AuthUserId { get; set; }

        public TextFile TextFile { get; set; }

        public List<string> Errors { get; set; }

        public ProcessRetroRegister()
        {
            Title = "ProcessRetroRegister";
            Description = "To read Retro Confirmation Date in Retro Register csv file and insert into database";
            Arguments = new string[]
            {
                "filePath",
            };
            Hide = true;
            Errors = new List<string> { };
            GetColumns();
        }

        public override bool Validate()
        {
            string filepath = CommandInput.Arguments[0];
            if (!File.Exists(filepath))
            {
                PrintError(string.Format(MessageBag.FileNotExists, filepath));
                return false;
            }
            else
            {
                FilePath = filepath;
            }

            return base.Validate();
        }

        public override void Run()
        {
            PrintStarting();

            Process();

            PrintEnding();
        }

        public void Process()
        {
            if (PostedFile != null)
            {
                TextFile = new TextFile(PostedFile.InputStream);
            }
            else if (File.Exists(FilePath))
            {
                TextFile = new TextFile(FilePath);
            }
            else
            {
                throw new Exception("No file can be read");
            }

            TrailObject trail;
            while (TextFile.GetNextRow() != null)
            {
                if (TextFile.RowIndex == 1)
                    continue; // Skip header row

                if (PrintCommitBuffer())
                {
                }
                SetProcessCount();

                bool error = false;

                var entity = new RetroRegister();
                RetroRegisterBo b = null;
                try
                {
                    b = SetData();

                    if (string.IsNullOrEmpty(b.RetroStatementNo))
                    {
                        SetProcessCount("Invoice Number Empty");
                        Errors.Add(string.Format("Invoice Number required at row {0}", TextFile.RowIndex));
                        error = true;
                    }
                    else
                    {
                        RetroRegisterBo retroBo = RetroRegisterService.FindByInvoiceReferenceIfrs4(b.RetroStatementNo);
                        if (retroBo == null)
                        {
                            SetProcessCount("Invoice Number Not Found");
                            Errors.Add(string.Format("The Invoice Number doesn't exists: {0} at row {1}", b.RetroStatementNo, TextFile.RowIndex));
                            error = true;
                        }
                    }

                    var retroConfirmationDate = TextFile.GetColValue(RetroRegisterBo.ColumnConfirmationDate);
                    if (!string.IsNullOrEmpty(retroConfirmationDate))
                    {
                        if (!ValidateDateTimeFormat(RetroRegisterBo.ColumnConfirmationDate, ref b))
                        {
                            error = true;
                        }
                    }
                    else
                    {
                        SetProcessCount("Retro Confirmation Date Empty");
                        Errors.Add(string.Format("Please enter the Retro Confirmation Date at row {0}", TextFile.RowIndex));
                        error = true;
                    }

                }
                catch (Exception e)
                {
                    SetProcessCount("Error");
                    Errors.Add(string.Format("Error Triggered: {0} at row {1}", e.Message, TextFile.RowIndex));
                    error = true;
                }

                if (error)
                {
                    continue;
                }

                trail = new TrailObject();
                RetroRegisterBo bDb = RetroRegisterService.Find(b.Id);
                if (bDb != null)
                {
                    bDb.RetroConfirmationDate = b.RetroConfirmationDate;
                    bDb.UpdatedById = b.UpdatedById;
                    RetroRegisterService.Update(ref bDb, ref trail);

                    var iDbs = RetroRegisterService.FindByInvoiceReferenceIfrs17(b.RetroStatementNo);
                    if (!iDbs.IsNullOrEmpty())
                    {
                        foreach (var irDb in iDbs)
                        {
                            var bo = irDb;
                            bo.RetroConfirmationDate = b.RetroConfirmationDate;
                            bo.UpdatedById = b.UpdatedById;
                            RetroRegisterService.Update(ref bo, ref trail);
                        }
                    }

                }
            }

            PrintProcessCount();

            TextFile.Close();

            foreach (string error in Errors)
            {
                PrintError(error);
            }
        }

        public RetroRegisterBo SetData()
        {
            var b = new RetroRegisterBo
            {
                Id = 0,
                RetroStatementNo = TextFile.GetColValue(RetroRegisterBo.ColumnInvoiceNo),
                //RetroConfirmationDate = TextFile.GetColValue(RetroRegisterBo.ColumnConfirmationDate),
                CreatedById = AuthUserId != null ? AuthUserId.Value : User.DefaultSuperUserId,
                UpdatedById = AuthUserId != null ? AuthUserId : User.DefaultSuperUserId,
            };

            string idStr = TextFile.GetColValue(RetroRegisterBo.ColumnId);
            if (!string.IsNullOrEmpty(idStr) && int.TryParse(idStr, out int id))
            {
                b.Id = id;
            }

            return b;
        }

        public List<Column> GetColumns()
        {
            Columns = RetroRegisterBo.GetColumns();
            return Columns;
        }

        public bool ValidateDateTimeFormat(int type, ref RetroRegisterBo tbcm)
        {
            string header = Columns.Where(m => m.ColIndex == type).Select(m => m.Header).FirstOrDefault();
            string property = Columns.Where(m => m.ColIndex == type).Select(q => q.Property).FirstOrDefault();

            bool valid = true;
            string value = TextFile.GetColValue(type);
            if (!string.IsNullOrEmpty(value))
            {
                if (Util.TryParseDateTime(value, out DateTime? datetime, out string error))
                {
                    tbcm.SetPropertyValue(property, datetime.Value);
                }
                else
                {
                    SetProcessCount(string.Format(header, "Error"));
                    Errors.Add(string.Format("The {0} format can not be read: {1} at row {2}", header, value, TextFile.RowIndex));
                    valid = false;
                }
            }
            return valid;
        }
    }

}