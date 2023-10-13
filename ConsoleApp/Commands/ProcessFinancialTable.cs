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
    public class ProcessFinancialTable : Command
    {
        public string FilePath { get; set; }

        public TreatyPricingFinancialTableVersionFileBo FinancialTableFileBo { get; set; }

        public int FinancialTableDetailId { get; set; }

        public Excel Excel { get; set; }

        public List<Column> Column { get; set; }

        public int LegendsRowCount { get; set; }

        public int RowCount { get; set; }

        public int ColumnCount { get; set; }

        public int Worksheet { get; set; }

        public List<string> Errors { get; set; }

        public List<FinancialTableLegends> Legends { get; set; }

        public List<FinancialTableMainData> MainData { get; set; }

        public List<TreatyPricingFinancialTableUploadLegendBo> LegendBos { get; set; }

        public List<TreatyPricingFinancialTableUploadBo> CellBos { get; set; }

        public int SumAssuredInterval { get; set; }

        public int UserId { get; set; }

        public const int WorksheetMain = 1;
        public const int WorksheetLegends = 2;

        public const int TypeAbbreviation = 1;
        public const int TypeDescription = 2;

        public ProcessFinancialTable()
        {
            Title = "ProcessFinancialTable";
            Description = "To process financial table data from excel file";
            Errors = new List<string> { };

            SumAssuredInterval = Util.GetConfigInteger("FinancialTableSumAssuredMultiples");
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
            if (TreatyPricingFinancialTableVersionFileService.CountByStatus(TreatyPricingFinancialTableVersionFileBo.StatusSubmitForProcessing) > 0)
            {
                foreach (var bo in TreatyPricingFinancialTableVersionFileService.GetByStatus(TreatyPricingFinancialTableVersionFileBo.StatusSubmitForProcessing))
                {
                    FilePath = bo.GetLocalPath();
                    FinancialTableFileBo = bo;
                    UserId = 1;
                    Errors = new List<string>();
                    Process();
                }
            }
            PrintEnding();
        }

        public void Process()
        {
            UpdateFileStatus(TreatyPricingFinancialTableVersionFileBo.StatusProcessing, "Processing Financial Table Data File");

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
                Legends = new List<FinancialTableLegends>();

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
                        Legends.Add(new FinancialTableLegends(abbreviationValue, descriptionValue));
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
                UpdateFileStatus(TreatyPricingFinancialTableVersionFileBo.StatusSuccess, "Process Financial Table Data File Success");
            }
            else
            {
                ProcessingFailed();
                return;
            }
        }

        public void ProcessSumAssuredRanges()
        {
            List<int> minimumSumAssuredList = new List<int>();
            List<int> maximumSumAssuredList = new List<int>();

            MainData = new List<FinancialTableMainData>();

            //Set row count
            try
            {
                RowCount = 1;

                Excel.RowIndex = 1;
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
                Excel.ColIndex = 1;
                Excel.RowIndex = 1;

                if (PrintCommitBuffer()) { }
                SetProcessCount("Process minimum sum assured");

                while (Excel.GetNextRow() != null && Excel.RowIndex <= RowCount)
                {
                    if (Excel.RowIndex == 1)
                        continue; // Skip header row

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
                Excel.ColIndex = 2;
                Excel.RowIndex = 1;

                if (PrintCommitBuffer()) { }
                SetProcessCount("Process maximum sum assured");

                while (Excel.GetNextRow() != null && Excel.RowIndex <= RowCount)
                {
                    if (Excel.RowIndex == 1)
                        continue; // Skip header row

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
                    Errors.Add(string.Format("Value \"{0}\" at Cell[{1},{2}] is not +1 from previous Sum Assured (To)", minimumSumAssuredList[i], i + 2, 1));
                    break;
                }

                if (maximumSumAssuredList[i] < 2000000000)
                {
                    if ((maximumSumAssuredList[i] - minimumSumAssuredList[i] + 1) % SumAssuredInterval != 0) // Check interval requirement
                    {
                        Errors.Add(string.Format("Range at Row[{0}] does not match interval required", i + 2));
                        break;
                    }
                }

                MainData.Add(new FinancialTableMainData(minimumSumAssuredList[i], maximumSumAssuredList[i], ""));
            }
        }

        public void ValidateRequirementsExist()
        {
            try
            {
                if (PrintCommitBuffer()) { }
                SetProcessCount("Process requirements exist validation");

                Excel.ColIndex = 3;
                Excel.RowIndex = 1;
                int i = 0;

                while (Excel.GetNextRow() != null && Excel.RowIndex <= RowCount)
                {
                    if (Excel.RowIndex == 1)
                        continue; // Skip header row

                    if ((Excel.XWorkSheet.Cells[Excel.RowIndex, Excel.ColIndex] as Microsoft.Office.Interop.Excel.Range).Value2 == null)
                    {
                        Errors.Add(string.Format("Financial Requirement value at Cell[{0},{1}] should not be empty", Excel.RowIndex, Excel.ColIndex));
                        //return;
                    }

                    bool cellValuePass = true;
                    string cellValue = (Excel.XWorkSheet.Cells[Excel.RowIndex, Excel.ColIndex] as Microsoft.Office.Interop.Excel.Range).Value;

                    List<string> cellValueList = cellValue.Split(',').ToList();

                    foreach (string value in cellValueList)
                    {
                        bool valueExists = false;

                        foreach (FinancialTableLegends legend in Legends)
                        {
                            if (value.Trim() == legend.Abbreviation.Trim())
                            {
                                valueExists = true;
                                break;
                            }
                        }

                        if (valueExists == false)
                        {
                            cellValuePass = false;
                            Errors.Add(string.Format("Value \"{0}\" at Cell[{1},{2}] does not exist in Legends sheet", value, Excel.RowIndex, Excel.ColIndex));
                            break;
                        }
                    }

                    if (cellValuePass == true && MainData.Count >= i + 1)
                    {
                        MainData[i].Code = cellValue;
                    }
                    i++;
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

            #region Insert Legends data
            if (PrintCommitBuffer()) { }
            SetProcessCount("Insert legends data into database");

            foreach (var legend in Legends)
            {
                var bo = new TreatyPricingFinancialTableUploadLegendBo
                {
                    TreatyPricingFinancialTableVersionDetailId = FinancialTableDetailId,
                    Code = legend.Abbreviation.Trim(),
                    Description = legend.Description,
                    CreatedById = UserId,
                    UpdatedById = UserId,
                };

                TreatyPricingFinancialTableUploadLegendService.Create(ref bo);
            }
            #endregion

            #region Insert Main sheet row data
            if (PrintCommitBuffer()) { }
            SetProcessCount("Insert main sheet data into database");

            foreach (var mainData in MainData)
            {
                var bo = new TreatyPricingFinancialTableUploadBo
                {
                    TreatyPricingFinancialTableVersionDetailId = FinancialTableDetailId,
                    MinimumSumAssured = mainData.MinimumSumAssured,
                    MaximumSumAssured = mainData.MaximumSumAssured,
                    Code = mainData.Code,
                    CreatedById = UserId,
                    UpdatedById = UserId,
                };

                TreatyPricingFinancialTableUploadService.Create(ref bo);
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
                    return;
                }
            }
        }

        public void ProcessingFailed()
        {
            DeleteFile();
            UpdateFileStatus(TreatyPricingFinancialTableVersionFileBo.StatusFailed, "Process Financial Table Data File Failed");
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
            var financialTableDataFile = FinancialTableFileBo;
            financialTableDataFile.Status = status;

            if (Errors.Count > 0)
            {
                financialTableDataFile.Errors = JsonConvert.SerializeObject(Errors);
            }

            var trail = new TrailObject();
            var result = TreatyPricingFinancialTableVersionFileService.Update(ref financialTableDataFile, ref trail);

            var userTrailBo = new UserTrailBo(
                financialTableDataFile.Id,
                description,
                result,
                trail,
                User.DefaultSuperUserId
            );
            UserTrailService.Create(ref userTrailBo);
        }

        public void SaveDetailTableData()
        {
            if (FinancialTableFileBo == null)
                return;

            var detailBo = new TreatyPricingFinancialTableVersionDetailBo
            {
                TreatyPricingFinancialTableVersionId = FinancialTableFileBo.TreatyPricingFinancialTableVersionId,
                DistributionTierPickListDetailId = FinancialTableFileBo.DistributionTierPickListDetailId,
                Description = FinancialTableFileBo.Description,
                CreatedById = UserId,
                UpdatedById = UserId,
            };

            TreatyPricingFinancialTableVersionDetailService.Create(ref detailBo);

            FinancialTableDetailId = detailBo.Id;
        }
    }

    public class FinancialTableLegends
    {
        public string Abbreviation { get; set; }

        public string Description { get; set; }

        public FinancialTableLegends(string abbreviation, string description)
        {
            Abbreviation = abbreviation;
            Description = description;
        }
    }

    public class FinancialTableMainData
    {
        public int MinimumSumAssured { get; set; }

        public int MaximumSumAssured { get; set; }

        public string Code { get; set; }

        public FinancialTableMainData(int minimumSumAssured, int maximumSumAssured, string code)
        {
            MinimumSumAssured = minimumSumAssured;
            MaximumSumAssured = maximumSumAssured;
            Code = code;
        }
    }
}
