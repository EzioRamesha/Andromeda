using BusinessObject;
using BusinessObject.SoaDatas;
using DataAccess.EntityFramework;
using Services.SoaDatas;
using Shared;
using Shared.ProcessFile;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ConsoleApp.Commands
{
    public class GenerateSoaDataPostValidation : Command
    {
        public Excel SummaryPostValidationExcel { get; set; }

        public string Directory { get; set; }

        public string FileName { get; set; }

        public string FilePath { get; set; }

        public List<string> SheetNames { get; set; } = new List<string> { "MLReChecking", "CedantAmount", "DiscrepancyChecks", "Differences" };
        public int TotalSheet { get; set; } = 4;

        public int RowIndex { get; set; } = 1;
        public int Skip { get; set; } = 0;
        public int Take { get; set; } = 500;
        public int Total { get; set; } = 0;

        public int CurrentQuery { get; set; } = 1;

        public int SoaDataBatchId { get; set; }
        public int Type { get; set; }
        public bool OriginalCurrency { get; set; }
        public bool TypeDifferences { get; set; } = false;

        public SoaDataPostValidationBo SoaDataPostValidationBo { get; set; }
        public SoaDataPostValidationDifferenceBo SoaDataPostValidationDifferenceBo { get; set; }
        public IQueryable<SoaDataPostValidationBo> Query { get; set; }
        public IQueryable<SoaDataPostValidationBo> MlreCheckingQuery { get; set; }
        public IQueryable<SoaDataPostValidationBo> CedantAmtQuery { get; set; }
        public IQueryable<SoaDataPostValidationBo> DiscrepancyCheckQuery { get; set; }
        public IQueryable<SoaDataPostValidationDifferenceBo> DifferencesQuery { get; set; }

        public List<Column> Column { get; set; }

        public List<string> StringRows { get; set; }

        public List<string> Errors { get; set; }

        public GenerateSoaDataPostValidation()
        {
            Title = "GenerateSoaDataPostValidation";
            Description = "To export soa data post validation summary excel file";
            Errors = new List<string> { };
        }

        public override void Run()
        {
            PrintStarting();
            Process();
            PrintEnding();
        }

        public void Process()
        {
            try
            {
                FileInfo file = new FileInfo(FilePath);
                file.Directory.Create();
                SummaryPostValidationExcel = new Excel(FilePath, false);

                ProcessHtml();
            }
            catch (Exception e)
            {
                Errors.Add(e.Message);
                SummaryPostValidationExcel.Close();
                return;
            }

            PrintProcessCount();


            if (Errors.Count == 0)
            {
                SummaryPostValidationExcel.FilePath = FilePath;
                SummaryPostValidationExcel.Save();
            }
            else
            {
                foreach (string error in Errors)
                {
                    PrintError(error);
                }
                return;
            }
        }

        public void ProcessHtml()
        {
            int rowNumber = 1;

            foreach (string row in StringRows)
            {
                int columnNumber = 1;
                var cellValues = row.Split('|').ToList().Select(s => s.Trim());

                foreach (string cellValue in cellValues)
                {
                    //For medical table comparison legend listing
                    string trimmedCellValue = string.Join("\n", cellValue.Split('\n').Select(s => s.Trim()));

                    SummaryPostValidationExcel.XWorkSheet.Cells[rowNumber, columnNumber].Value = Util.FormatExport(trimmedCellValue);

                    columnNumber++;
                }

                rowNumber++;
            }

            SummaryPostValidationExcel.XWorkSheet.Columns.AutoFit();
        }

        #region current process to process using html data inteads of db.
        //public void Process()
        //{
        //    GenerateSoaDataPostValidationSummaryFile();

        //    SummaryPostValidationExcel.OpenTemplate();

        //    using (var db = new AppDbContext(false))
        //    {
        //        var query = db.SoaDataPostValidations.Select(SoaDataPostValidationService.Expression()).Where(q => q.SoaDataBatchId == SoaDataBatchId);

        //        if (OriginalCurrency)
        //            query = query.Where(q => !string.IsNullOrEmpty(q.CurrencyCode) && q.CurrencyCode != PickListDetailBo.CurrencyCodeMyr);
        //        else
        //            query = query.Where(q => string.IsNullOrEmpty(q.CurrencyCode) || q.CurrencyCode == PickListDetailBo.CurrencyCodeMyr);

        //        var queryDiff = db.SoaDataPostValidationDifferences.Select(SoaDataPostValidationDifferenceService.Expression()).Where(q => q.SoaDataBatchId == SoaDataBatchId);

        //        switch (Type)
        //        {
        //            case 1:
        //                MlreCheckingQuery = query.Where(q => q.Type == SoaDataPostValidationBo.TypeMlreShareMlreChecking);
        //                CedantAmtQuery = query.Where(q => q.Type == SoaDataPostValidationBo.TypeMlreShareCedantAmount);
        //                DiscrepancyCheckQuery = query.Where(q => q.Type == SoaDataPostValidationBo.TypeMlreShareDiscrepancyCheck);
        //                DifferencesQuery = queryDiff.Where(q => q.Type == SoaDataPostValidationDifferenceBo.TypeMlreShare);
        //                break;
        //            case 2:
        //                MlreCheckingQuery = query.Where(q => q.Type == SoaDataPostValidationBo.TypeLayer1ShareMlreChecking);
        //                CedantAmtQuery = query.Where(q => q.Type == SoaDataPostValidationBo.TypeLayer1ShareCedantAmount);
        //                DiscrepancyCheckQuery = query.Where(q => q.Type == SoaDataPostValidationBo.TypeLayer1ShareDiscrepancyCheck);
        //                DifferencesQuery = queryDiff.Where(q => q.Type == SoaDataPostValidationDifferenceBo.TypeLayer1Share);
        //                break;
        //            case 3:
        //                MlreCheckingQuery = query.Where(q => q.Type == SoaDataPostValidationBo.TypeRetakafulShareMlreChecking);
        //                CedantAmtQuery = query.Where(q => q.Type == SoaDataPostValidationBo.TypeRetakafulShareCedantAmount);
        //                DiscrepancyCheckQuery = query.Where(q => q.Type == SoaDataPostValidationBo.TypeRetakafulShareDiscrepancyCheck);
        //                DifferencesQuery = queryDiff.Where(q => q.Type == SoaDataPostValidationDifferenceBo.TypeRetakaful);
        //                break;
        //        }

        //        SummaryPostValidationExcel.XWorkBook = (SummaryPostValidationExcel.XApp.Workbooks.Add(System.Reflection.Missing.Value));
        //        SummaryPostValidationExcel.XWorkSheet = SummaryPostValidationExcel.XWorkBook.ActiveSheet;

        //        for (var i = TotalSheet; i > 0; i--)
        //        {
        //            CurrentQuery = i;
        //            GetQuery();

        //            SummaryPostValidationExcel.XWorkSheet = SummaryPostValidationExcel.XApp.Worksheets.Add();
        //            SummaryPostValidationExcel.XWorkSheet.Name = SheetNames[i - 1];

        //            GetColumns();
        //            RowIndex = 1;
        //            WriteHeaderLine();

        //            GetQueryTotal();
        //            for (Skip = 0; Skip < Total + Take; Skip += Take)
        //            {
        //                if (Skip >= Total)
        //                    break;

        //                var list = GetQueryNext();
        //                if (list == null)
        //                    return;

        //                foreach (var entity in list)
        //                {
        //                    SetEntity(entity);
        //                    WriteDataLine();
        //                }
        //            }
        //        }
        //    }

        //    SummaryPostValidationExcel.Save();
        //}

        //public void GenerateSoaDataPostValidationSummaryFile()
        //{
        //    var templateFilepath = Util.GetWebAppDocumentFilePath("SoaDataSummary_Template.xlsx");
        //    FileName = string.Format("SoaDataPostValidation{0}_{1}", (OriginalCurrency ? "ORI" : ""), SoaDataBatchId).AppendDateTimeFileName(".xlsx");
        //    Directory = Util.GetRetroStatementDownloadPath();
        //    FilePath = Path.Combine(Util.GetTemporaryPath(), FileName);
        //    SummaryPostValidationExcel = new Excel(templateFilepath, FilePath, 1);
        //}

        //public void WriteHeaderLine()
        //{
        //    if (SummaryPostValidationExcel == null)
        //        return;

        //    int colIndex;
        //    int index = 1;
        //    foreach (var col in Column)
        //    {
        //        colIndex = index;
        //        if (col.ColIndex.HasValue)
        //            colIndex = col.ColIndex.Value;

        //        SummaryPostValidationExcel.WriteCell(RowIndex, colIndex, col.Header);

        //        index++;
        //    }

        //    RowIndex++;
        //}

        //public object GetEntity()
        //{
        //    if (TypeDifferences)
        //        return SoaDataPostValidationDifferenceBo;
        //    else
        //        return SoaDataPostValidationBo;
        //}

        //public IQueryable<object> GetQuery()
        //{
        //    if (CurrentQuery == 4)
        //    {
        //        TypeDifferences = true;
        //        return DifferencesQuery;
        //    }

        //    switch (CurrentQuery)
        //    {
        //        case 1:
        //            Query = MlreCheckingQuery;
        //            break;
        //        case 2:
        //            Query = CedantAmtQuery;
        //            break;
        //        case 3:
        //            Query = DiscrepancyCheckQuery;
        //            break;
        //    }
        //    TypeDifferences = false;
        //    return Query;
        //}

        //public IEnumerable<object> GetQueryNext()
        //{
        //    if (TypeDifferences)
        //    {
        //        if (DifferencesQuery == null)
        //            return new List<SoaDataPostValidationDifferenceBo> { };
        //        return DifferencesQuery.OrderBy(q => q.TreatyCode).Skip(Skip).Take(Take).ToList();
        //    }
        //    else
        //    {
        //        if (Query == null)
        //            return new List<SoaDataPostValidationBo> { };
        //        return Query.OrderBy(q => q.TreatyCode).Skip(Skip).Take(Take).ToList();
        //    }
        //}

        //public int GetQueryTotal()
        //{
        //    if (TypeDifferences)
        //    {
        //        if (DifferencesQuery == null)
        //            Total = 0;
        //        else
        //            Total = DifferencesQuery.Count();
        //        return Total;
        //    }


        //    if (Query == null)
        //        Total = 0;
        //    else
        //        Total = Query.Count();
        //    return Total;
        //}

        //public void SetEntity(object entity)
        //{
        //    if (TypeDifferences)
        //    {
        //        SoaDataPostValidationDifferenceBo = null;
        //        if (entity is SoaDataPostValidationDifferenceBo e)
        //            SoaDataPostValidationDifferenceBo = e;
        //    }
        //    else
        //    {
        //        SoaDataPostValidationBo = null;
        //        if (entity is SoaDataPostValidationBo e)
        //            SoaDataPostValidationBo = e;
        //    }
        //}

        //public void SetQuery(IQueryable<object> query)
        //{
        //    Query = null;
        //    if (query is IQueryable<SoaDataPostValidationBo> q)
        //        Query = q;
        //}

        //public void WriteDataLine()
        //{
        //    if (SummaryPostValidationExcel == null)
        //        return;

        //    object entity = GetEntity();
        //    if (entity == null)
        //        return;

        //    int colIndex;
        //    int index = 1;
        //    foreach (var col in Column)
        //    {
        //        colIndex = index;
        //        if (col.ColIndex.HasValue)
        //            colIndex = col.ColIndex.Value;

        //        SummaryPostValidationExcel.WriteCell(RowIndex, colIndex, GetColumnValue(col, entity));

        //        index++;
        //    }

        //    RowIndex++;
        //}

        //public virtual string GetColumnValue(Column col, object entity)
        //{
        //    return Util.FormatExport(entity.GetPropertyValue(col.Property));
        //}

        //public List<Column> GetColumns()
        //{
        //    if (TypeDifferences)
        //        Column = SoaDataPostValidationDifferenceBo.GetColumns();
        //    else
        //        Column = SoaDataPostValidationBo.GetColumns();
        //    return Column;
        //}
        #endregion
    }
}
