using BusinessObject.Identity;
using BusinessObject.TreatyPricing;
using DataAccess.Entities.Identity;
using Newtonsoft.Json;
using Services.Identity;
using Services.TreatyPricing;
using Shared;
using Shared.ProcessFile;
using Shared.Trails;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ConsoleApp.Commands
{
    public class ProcessMedicalTable : Command
    {
        public string FilePath { get; set; }

        public TreatyPricingMedicalTableVersionFileBo MedicalTableFileBo { get; set; }

        public int MedicalTableDetailId { get; set; }

        public Excel Excel { get; set; }

        public List<Column> Column { get; set; }

        public int ColumnCount { get; set; }

        public int LegendsRowCount { get; set; }

        public int RowCount { get; set; }

        public int Worksheet { get; set; }

        public List<string> Errors { get; set; }

        public List<MedicalTableLegends> Legends { get; set; }

        public List<MedicalTableAgeRanges> AgeRanges { get; set; }

        public List<MedicalTableSumAssuredRanges> SumAssuredRanges { get; set; }

        public List<TreatyPricingMedicalTableUploadLegendBo> LegendBos { get; set; }

        public List<TreatyPricingMedicalTableUploadColumnBo> ColumnBos { get; set; }

        public List<TreatyPricingMedicalTableUploadRowBo> RowBos { get; set; }

        public List<TreatyPricingMedicalTableUploadCellBo> CellBos { get; set; }

        public int AgeInterval { get; set; }

        public int SumAssuredInterval { get; set; }

        public int UserId { get; set; }

        public const int WorksheetMain = 1;
        public const int WorksheetLegends = 2;

        public const int TypeAbbreviation = 1;
        public const int TypeDescription = 2;
        
        public ProcessMedicalTable()
        {
            Title = "ProcessMedicalTable";
            Description = "To process medical table data from excel file";
            Errors = new List<string> { };

            AgeInterval = Util.GetConfigInteger("MedicalTableAgeMultiples");
            SumAssuredInterval = Util.GetConfigInteger("MedicalTableSumAssuredMultiples");
        }

        public override bool Validate()
        {
            //if (!File.Exists(FilePath))
            //{
            //    PrintError(string.Format(MessageBag.FileNotExists, FilePath));
            //    return false;
            //}

            return base.Validate();
        }

        public override void Run()
        {
            PrintStarting();
            //Process();
            if (TreatyPricingMedicalTableVersionFileService.CountByStatus(TreatyPricingMedicalTableVersionFileBo.StatusSubmitForProcessing) > 0)
            {
                foreach (var bo in TreatyPricingMedicalTableVersionFileService.GetByStatus(TreatyPricingMedicalTableVersionFileBo.StatusSubmitForProcessing))
                {
                    FilePath = bo.GetLocalPath();
                    MedicalTableFileBo = bo;
                    UserId = 1;
                    Errors = new List<string>();
                    Process();
                }
            }
            PrintEnding();
        }

        public void Process()
        {
            UpdateFileStatus(TreatyPricingMedicalTableVersionFileBo.StatusProcessing, "Processing Medical Table Data File");

            ProcessLegends();
            ProcessMain();

            PrintProcessCount();

            foreach (string error in Errors)
            {
                PrintError(error);
            }
        }

        public void ProcessLegends()
        {
            Worksheet = WorksheetLegends;

            try
            {
                Excel = new Excel(FilePath, true, Worksheet);
            }
            catch (Exception e)
            {
                Errors.Add(e.Message);
                return;
            }

            //Set legends row count
            try
            {
                LegendsRowCount = 1;

                Excel.RowIndex = 1;
                Excel.ColIndex = 1;

                if (PrintCommitBuffer()) { }
                SetProcessCount("Get legends row count");

                while (Excel.GetNextRow() != null)
                {
                    if ((Excel.XWorkSheet.Cells[Excel.RowIndex, 1] as Microsoft.Office.Interop.Excel.Range).Value2 != null
                        && (Excel.XWorkSheet.Cells[Excel.RowIndex, 2] as Microsoft.Office.Interop.Excel.Range).Value2 != null)
                    {
                        var value1 = (Excel.XWorkSheet.Cells[Excel.RowIndex, 1] as Microsoft.Office.Interop.Excel.Range).Value;
                        var value2 = (Excel.XWorkSheet.Cells[Excel.RowIndex, 2] as Microsoft.Office.Interop.Excel.Range).Value;

                        if (value1.ToString().Trim() != "" && value2.ToString().Trim() != "")
                            LegendsRowCount++;
                    }
                }
            }
            catch (Exception e)
            {
                Errors.Add(e.Message);
                return;
            }

            try
            {
                Legends = new List<MedicalTableLegends>();

                Excel.RowIndex = 1;
                Excel.ColIndex = 1;

                if (PrintCommitBuffer()) { }
                SetProcessCount("Process legends");

                while (Excel.GetNextRow() != null && Excel.RowIndex <= LegendsRowCount)
                {
                    if (Excel.RowIndex == 1)
                        continue; // Skip header row

                    string abbreviationValue = (Excel.XWorkSheet.Cells[Excel.RowIndex, TypeAbbreviation] as Microsoft.Office.Interop.Excel.Range).Value;
                    string descriptionValue = (Excel.XWorkSheet.Cells[Excel.RowIndex, TypeDescription] as Microsoft.Office.Interop.Excel.Range).Value;

                    if (abbreviationValue.Length > 30)
                    {
                        Errors.Add(string.Format("Abbreviation should not be longer than 30 characters", Excel.RowIndex, TypeAbbreviation));
                        //return;
                    }

                    if (!string.IsNullOrEmpty(descriptionValue))
                    {
                        Legends.Add(new MedicalTableLegends(abbreviationValue, descriptionValue));
                    }
                }

                Excel.Close();
            }
            catch (Exception e)
            {
                Errors.Add(e.Message);
                ProcessingFailed();
                return;
            }
        }

        public void ProcessMain()
        {
            Worksheet = WorksheetMain;

            try
            {
                Excel = new Excel(FilePath, true, Worksheet);
            }
            catch (Exception e)
            {
                Errors.Add(e.Message);
                ProcessingFailed();
                return;
            }

            //if (Errors.Count == 0)
            //{
                ProcessAgeRanges();
            //}
            //else
            //{
            //    ProcessingFailed();
            //    return;
            //}

            //if (Errors.Count == 0)
            //{
                ProcessSumAssuredRanges();
            //}
            //else
            //{
            //    ProcessingFailed();
            //    return;
            //}

            //if (Errors.Count == 0)
            //{
                ValidateRequirementsExist();
            //}
            //else
            //{
            //    ProcessingFailed();
            //    return;
            //}

            if (Errors.Count == 0)
            {
                InsertData();
            }
            else
            {
                ProcessingFailed();
                return;
            }

            if (Errors.Count == 0)
            {
                DeleteFile();
                UpdateFileStatus(TreatyPricingMedicalTableVersionFileBo.StatusSuccess, "Process Medical Table Data File Success");
            }
            else
            {
                ProcessingFailed();
                return;
            }
        }

        public void ProcessAgeRanges()
        {
            List<int> minimumAgeList = new List<int>();
            List<int> maximumAgeList = new List<int>();

            AgeRanges = new List<MedicalTableAgeRanges>();

            //Set column count
            try
            {
                ColumnCount = 2;

                Excel.RowIndex = 1;
                Excel.ColIndex = 2;

                if (PrintCommitBuffer()) { }
                SetProcessCount("Get column count");

                while (Excel.GetNextCol() != null)
                {
                    if ((Excel.XWorkSheet.Cells[1, Excel.ColIndex] as Microsoft.Office.Interop.Excel.Range).Value2 != null
                        && (Excel.XWorkSheet.Cells[2, Excel.ColIndex] as Microsoft.Office.Interop.Excel.Range).Value2 != null
                        && (Excel.XWorkSheet.Cells[4, Excel.ColIndex] as Microsoft.Office.Interop.Excel.Range).Value2 != null)
                    {
                        var value1 = (Excel.XWorkSheet.Cells[1, Excel.ColIndex] as Microsoft.Office.Interop.Excel.Range).Value;
                        var value2 = (Excel.XWorkSheet.Cells[2, Excel.ColIndex] as Microsoft.Office.Interop.Excel.Range).Value;
                        var value3 = (Excel.XWorkSheet.Cells[4, Excel.ColIndex] as Microsoft.Office.Interop.Excel.Range).Value;

                        if (value1.ToString().Trim() != "" && value2.ToString().Trim() != "" && value3.ToString().Trim() != "")
                            ColumnCount++;
                    }
                }
            }
            catch (Exception e)
            {
                Errors.Add(e.Message);
                return;
            }

            try
            {
                Excel.RowIndex = 1;
                Excel.ColIndex = 2;

                if (PrintCommitBuffer()) { }
                SetProcessCount("Process minimum age");

                while (Excel.GetNextCol() != null && Excel.ColIndex <= ColumnCount)
                {
                    if ((Excel.XWorkSheet.Cells[Excel.RowIndex, Excel.ColIndex] as Microsoft.Office.Interop.Excel.Range).Value2 == null)
                    {
                        Errors.Add(string.Format("Minimum Age value at Cell[{0},{1}] should not be empty", Excel.RowIndex, Excel.ColIndex));
                        //return;
                    }

                    var minimumAge = (Excel.XWorkSheet.Cells[Excel.RowIndex, Excel.ColIndex] as Microsoft.Office.Interop.Excel.Range).Value;

                    ValidateIntegerDataType(minimumAge, Excel.RowIndex, Excel.ColIndex);

                    minimumAgeList.Add(Int32.Parse(minimumAge.ToString()));
                }
            }
            catch (Exception e)
            {
                Errors.Add(e.Message);
                return;
            }

            try
            {
                Excel.RowIndex = 2;
                Excel.ColIndex = 2;

                if (PrintCommitBuffer()) { }
                SetProcessCount("Process maximum age");

                while (Excel.GetNextCol() != null && Excel.ColIndex <= ColumnCount)
                {
                    if ((Excel.XWorkSheet.Cells[Excel.RowIndex, Excel.ColIndex] as Microsoft.Office.Interop.Excel.Range).Value2 == null)
                    {
                        Errors.Add(string.Format("Maximum Age value at Cell[{0},{1}] should not be empty", Excel.RowIndex, Excel.ColIndex));
                        //return;
                    }

                    var maximumAge = (Excel.XWorkSheet.Cells[Excel.RowIndex, Excel.ColIndex] as Microsoft.Office.Interop.Excel.Range).Value;

                    ValidateIntegerDataType(maximumAge, Excel.RowIndex, Excel.ColIndex);

                    maximumAgeList.Add(Int32.Parse(maximumAge.ToString()));
                }
            }
            catch (Exception e)
            {
                Errors.Add(e.Message);
                return;
            }

            for (int i = 0; i < minimumAgeList.Count; i++)
            {
                if (i > 0 && (minimumAgeList[i] != maximumAgeList[i - 1] + 1)) // Check age continuity
                {
                    Errors.Add(string.Format("Value \"{0}\" at Cell[{1},{2}] is not +1 from previous Age (To)", minimumAgeList[i], 1, i + 3));
                    break;
                }

                if ((maximumAgeList[i] - minimumAgeList[i] + 1) % AgeInterval != 0) // Check interval requirement
                {
                    Errors.Add(string.Format("Range at Column[{0}] does not match interval required", i + 3));
                    break;
                }

                AgeRanges.Add(new MedicalTableAgeRanges(minimumAgeList[i], maximumAgeList[i]));
            }
        }

        public void ProcessSumAssuredRanges()
        {
            List<int> minimumSumAssuredList = new List<int>();
            List<int> maximumSumAssuredList = new List<int>();

            SumAssuredRanges = new List<MedicalTableSumAssuredRanges>();

            //Set row count
            try
            {
                RowCount = 3;

                Excel.RowIndex = 3;
                Excel.ColIndex = 1;

                if (PrintCommitBuffer()) { }
                SetProcessCount("Get row count");

                while (Excel.GetNextRow() != null)
                {
                    if ((Excel.XWorkSheet.Cells[Excel.RowIndex, 1] as Microsoft.Office.Interop.Excel.Range).Value2 != null
                        && (Excel.XWorkSheet.Cells[Excel.RowIndex, 2] as Microsoft.Office.Interop.Excel.Range).Value2 != null
                        && (Excel.XWorkSheet.Cells[Excel.RowIndex, 3] as Microsoft.Office.Interop.Excel.Range).Value2 != null)
                    {
                        var value1 = (Excel.XWorkSheet.Cells[Excel.RowIndex, 1] as Microsoft.Office.Interop.Excel.Range).Value;
                        var value2 = (Excel.XWorkSheet.Cells[Excel.RowIndex, 2] as Microsoft.Office.Interop.Excel.Range).Value;
                        var value3 = (Excel.XWorkSheet.Cells[Excel.RowIndex, 3] as Microsoft.Office.Interop.Excel.Range).Value;

                        if (value1.ToString().Trim() != "" && value2.ToString().Trim() != "" && value3.ToString().Trim() != "")
                            RowCount++;
                    }
                }
            }
            catch (Exception e)
            {
                Errors.Add(e.Message);
                return;
            }

            try
            {
                Excel.RowIndex = 3;
                Excel.ColIndex = 1;

                if (PrintCommitBuffer()) { }
                SetProcessCount("Process minimum sum assured");

                while (Excel.GetNextRow() != null && Excel.RowIndex <= RowCount)
                {
                    if ((Excel.XWorkSheet.Cells[Excel.RowIndex, Excel.ColIndex] as Microsoft.Office.Interop.Excel.Range).Value2 == null)
                    {
                        Errors.Add(string.Format("Minimum Sum Assured value at Cell[{0},{1}] should not be empty", Excel.RowIndex, Excel.ColIndex));
                        //return;
                    }

                    var minimumSumAssured = (Excel.XWorkSheet.Cells[Excel.RowIndex, Excel.ColIndex] as Microsoft.Office.Interop.Excel.Range).Value;

                    ValidateIntegerDataType(minimumSumAssured, Excel.RowIndex, Excel.ColIndex);

                    minimumSumAssuredList.Add(Int32.Parse(minimumSumAssured.ToString()));
                }
            }
            catch (Exception e)
            {
                Errors.Add(e.Message);
                return;
            }

            try
            {
                Excel.RowIndex = 3;
                Excel.ColIndex = 2;

                if (PrintCommitBuffer()) { }
                SetProcessCount("Process maximum sum assured");

                while (Excel.GetNextRow() != null && Excel.RowIndex <= RowCount)
                {
                    if ((Excel.XWorkSheet.Cells[Excel.RowIndex, Excel.ColIndex] as Microsoft.Office.Interop.Excel.Range).Value2 == null)
                    {
                        Errors.Add(string.Format("Maximum Sum Assured value at Cell[{0},{1}] should not be empty", Excel.RowIndex, Excel.ColIndex));
                        //return;
                    }

                    var maximumSumAssured = (Excel.XWorkSheet.Cells[Excel.RowIndex, Excel.ColIndex] as Microsoft.Office.Interop.Excel.Range).Value;

                    if (maximumSumAssured.ToString().Trim().ToLower() == "max")
                    {
                        maximumSumAssuredList.Add(2000000000);
                    }
                    else
                    {
                        ValidateIntegerDataType(maximumSumAssured, Excel.RowIndex, Excel.ColIndex);
                        maximumSumAssuredList.Add(Int32.Parse(maximumSumAssured.ToString()));
                    }
                }
            }
            catch (Exception e)
            {
                Errors.Add(e.Message);
                return;
            }

            for (int i = 0; i < minimumSumAssuredList.Count; i++)
            {
                if (i > 0 && (minimumSumAssuredList[i] != maximumSumAssuredList[i - 1] + 1)) // Check sum assured continuity
                {
                    Errors.Add(string.Format("Value \"{0}\" at Cell[{1},{2}] is not +1 from previous Sum Assured (To)", minimumSumAssuredList[i], i + 4, 1));
                    break;
                }

                if (maximumSumAssuredList[i] < 2000000000)
                {
                    if ((maximumSumAssuredList[i] - minimumSumAssuredList[i] + 1) % SumAssuredInterval != 0) // Check interval requirement
                    {
                        Errors.Add(string.Format("Range at Row[{0}] does not match interval required", i + 4));
                        break;
                    }
                }

                SumAssuredRanges.Add(new MedicalTableSumAssuredRanges(minimumSumAssuredList[i], maximumSumAssuredList[i]));
            }
        }

        public void ValidateRequirementsExist()
        {
            try
            {
                if (PrintCommitBuffer()) { }
                SetProcessCount("Process requirements exist validation");

                Excel.RowIndex = 3;

                while (Excel.GetNextRow() != null && Excel.RowIndex <= RowCount)
                {
                    Excel.ColIndex = 2;

                    while (Excel.GetNextCol() != null && Excel.ColIndex <= ColumnCount)
                    {
                        if ((Excel.XWorkSheet.Cells[Excel.RowIndex, Excel.ColIndex] as Microsoft.Office.Interop.Excel.Range).Value2 == null)
                        {
                            Errors.Add(string.Format("Medical Requirement value at Cell[{0},{1}] should not be empty", Excel.RowIndex, Excel.ColIndex));
                            //return;
                        }

                        string cellValue = (Excel.XWorkSheet.Cells[Excel.RowIndex, Excel.ColIndex] as Microsoft.Office.Interop.Excel.Range).Value;

                        List<string> cellValueList = cellValue.Split(',').ToList();

                        foreach (string value in cellValueList)
                        {
                            bool valueExists = false;

                            foreach (MedicalTableLegends legend in Legends)
                            {
                                if (value.Trim() == legend.Abbreviation.Trim())
                                {
                                    valueExists = true;
                                    break;
                                }
                            }

                            if (valueExists == false)
                            {
                                Errors.Add(string.Format("Value \"{0}\" at Cell[{1},{2}] does not exist in Legends sheet", value, Excel.RowIndex, Excel.ColIndex));
                                break;
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Errors.Add(e.Message);
                return;
            }
        }

        public void InsertData()
        {
            SaveDetailTableData();
            List<int> InsertedRowIds = new List<int>();
            List<int> InsertedColumnIds = new List<int>();

            #region Insert Legends data
            if (PrintCommitBuffer()) { }
            SetProcessCount("Insert legends data into database");

            foreach (var legend in Legends)
            {
                var bo = new TreatyPricingMedicalTableUploadLegendBo
                {
                    TreatyPricingMedicalTableVersionDetailId = MedicalTableDetailId,
                    Code = legend.Abbreviation.Trim(),
                    Description = legend.Description,
                    CreatedById = UserId,
                    UpdatedById = UserId,
                };

                TreatyPricingMedicalTableUploadLegendService.Create(ref bo);
            }
            #endregion

            #region Insert Main sheet row data
            if (PrintCommitBuffer()) { }
            SetProcessCount("Insert row data into database");

            foreach (var sumAssured in SumAssuredRanges)
            {
                var bo = new TreatyPricingMedicalTableUploadRowBo
                {
                    TreatyPricingMedicalTableVersionDetailId = MedicalTableDetailId,
                    MinimumSumAssured = sumAssured.MinimumSumAssured,
                    MaximumSumAssured = sumAssured.MaximumSumAssured,
                    CreatedById = UserId,
                    UpdatedById = UserId,
                };

                TreatyPricingMedicalTableUploadRowService.Create(ref bo);
                InsertedRowIds.Add(bo.Id);
            }
            #endregion

            #region Insert Main sheet column data
            if (PrintCommitBuffer()) { }
            SetProcessCount("Insert column data into database");

            foreach (var age in AgeRanges)
            {
                var bo = new TreatyPricingMedicalTableUploadColumnBo
                {
                    TreatyPricingMedicalTableVersionDetailId = MedicalTableDetailId,
                    MinimumAge = age.MinimumAge,
                    MaximumAge = age.MaximumAge,
                    CreatedById = UserId,
                    UpdatedById = UserId,
                };

                TreatyPricingMedicalTableUploadColumnService.Create(ref bo);
                InsertedColumnIds.Add(bo.Id);
            }
            #endregion

            #region Insert Main sheet cells data
            try
            {
                if (PrintCommitBuffer()) { }
                SetProcessCount("Insert cell data into database");

                int rowListIndex = 0;
                int columnListIndex = 0;

                Excel.RowIndex = 3;

                while (Excel.GetNextRow() != null && Excel.RowIndex <= RowCount)
                {
                    Excel.ColIndex = 2;
                    columnListIndex = 0;

                    while (Excel.GetNextCol() != null && Excel.ColIndex <= ColumnCount)
                    {
                        string cellValue = (Excel.XWorkSheet.Cells[Excel.RowIndex, Excel.ColIndex] as Microsoft.Office.Interop.Excel.Range).Value;

                        var bo = new TreatyPricingMedicalTableUploadCellBo
                        {
                            TreatyPricingMedicalTableUploadColumnId = InsertedColumnIds[columnListIndex],
                            TreatyPricingMedicalTableUploadRowId = InsertedRowIds[rowListIndex],
                            Code = cellValue.Trim(),
                            CreatedById = UserId,
                            UpdatedById = UserId,
                        };

                        TreatyPricingMedicalTableUploadCellService.Create(ref bo);
                        columnListIndex++;
                    }
                    rowListIndex++;
                }
            }
            catch (Exception e)
            {
                Errors.Add(e.Message);
                return;
            }
            #endregion
        }

        public void ValidateIntegerDataType(dynamic cellValue, int rowIndex, int colIndex)
        {
            object value = cellValue;
            if ((cellValue is int) == false)
            {
                int? intValue = Util.GetParseInt(value.ToString());
                if (!intValue.HasValue)
                {
                    Errors.Add(string.Format("Value \"{0}\" is not numeric at Cell[{1},{2}]", value.ToString(), rowIndex, colIndex));
                    //return;
                }
            }
        }

        public void ProcessingFailed()
        {
            DeleteFile();
            UpdateFileStatus(TreatyPricingMedicalTableVersionFileBo.StatusFailed, "Process Medical Table Data File Failed");
        }

        public void DeleteFile()
        {
            try
            {
                Excel.Close();
                if (File.Exists(FilePath))
                    File.Delete(FilePath);
            }
            catch (Exception)
            {
            }
        }

        public void UpdateFileStatus(int status, string description)
        {
            var medicalTableDataFile = MedicalTableFileBo;
            medicalTableDataFile.Status = status;

            if (Errors.Count > 0)
            {
                medicalTableDataFile.Errors = JsonConvert.SerializeObject(Errors);
            }

            var trail = new TrailObject();
            var result = TreatyPricingMedicalTableVersionFileService.Update(ref medicalTableDataFile, ref trail);

            var userTrailBo = new UserTrailBo(
                medicalTableDataFile.Id,
                description,
                result,
                trail,
                User.DefaultSuperUserId
            );
            UserTrailService.Create(ref userTrailBo);
        }

        public void SaveDetailTableData()
        {
            if (MedicalTableFileBo == null)
                return;

            var detailBo = new TreatyPricingMedicalTableVersionDetailBo
            {
                TreatyPricingMedicalTableVersionId = MedicalTableFileBo.TreatyPricingMedicalTableVersionId,
                DistributionTierPickListDetailId = MedicalTableFileBo.DistributionTierPickListDetailId,
                Description = MedicalTableFileBo.Description,
                CreatedById = UserId,
                UpdatedById = UserId,
            };

            TreatyPricingMedicalTableVersionDetailService.Create(ref detailBo);

            MedicalTableDetailId = detailBo.Id;
        }
    }

    public class MedicalTableLegends
    {
        public string Abbreviation { get; set; }

        public string Description { get; set; }

        public MedicalTableLegends(string abbreviation, string description)
        {
            Abbreviation = abbreviation;
            Description = description;
        }
    }

    public class MedicalTableAgeRanges
    {
        public int MinimumAge { get; set; }

        public int MaximumAge { get; set; }

        public MedicalTableAgeRanges(int minimumAge, int maximumAge)
        {
            MinimumAge = minimumAge;
            MaximumAge = maximumAge;
        }
    }

    public class MedicalTableSumAssuredRanges
    {
        public int MinimumSumAssured { get; set; }

        public int MaximumSumAssured { get; set; }

        public MedicalTableSumAssuredRanges(int minimumSumAssured, int maximumSumAssured)
        {
            MinimumSumAssured = minimumSumAssured;
            MaximumSumAssured = maximumSumAssured;
        }
    }
}
