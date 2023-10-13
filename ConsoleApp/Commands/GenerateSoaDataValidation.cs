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
    public class GenerateSoaDataValidation : Command
    {
        public Excel SummaryValidationExcel { get; set; }

        public string Directory { get; set; }

        public string FileName { get; set; }

        public string FilePath { get; set; }

        public List<string> SheetNames { get; set; } = new List<string> { "SoaData", "RiSummary", "Differences" };
        public int TotalSheet { get; set; } = 3;

        public int RowIndex { get; set; } = 1;
        public int Skip { get; set; } = 0;
        public int Take { get; set; } = 500;
        public int Total { get; set; } = 0;

        public int CurrentQuery { get; set; } = 1;

        public int SoaDataBatchId { get; set; }
        public int Type { get; set; }
        public bool OriginalCurrency { get; set; }
        public bool TypeRetakaful { get; set; } = false;

        public SoaDataRiDataSummaryBo SoaDataRiDataSummaryBo { get; set; }
        public IQueryable<SoaDataRiDataSummaryBo> Query { get; set; }
        public IQueryable<SoaDataRiDataSummaryBo> SoaDataQuery { get; set; }
        public IQueryable<SoaDataRiDataSummaryBo> RiSummaryQuery { get; set; }
        public IQueryable<SoaDataRiDataSummaryBo> DifferencesQuery { get; set; }

        public List<Column> Column { get; set; }

        public List<string> StringRows { get; set; }

        public List<string> Errors { get; set; }

        public GenerateSoaDataValidation()
        {
            Title = "GenerateSoaDataValidation";
            Description = "To export soa data validation excel file";
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
                SummaryValidationExcel = new Excel(FilePath, false);

                ProcessHtml();
            }
            catch (Exception e)
            {
                Errors.Add(e.Message);
                SummaryValidationExcel.Close();
                return;
            }

            PrintProcessCount();


            if (Errors.Count == 0)
            {
                SummaryValidationExcel.FilePath = FilePath;
                SummaryValidationExcel.Save();
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
                    //decimal value;
                    //if(decimal.TryParse(trimmedCellValue, out value))
                    //{
                    //    SummaryValidationExcel.XWorkSheet.Cells[rowNumber, columnNumber].Value = Util.FormatExport(trimmedCellValue); ;
                    //}
                    //else
                    //{

                    //}

                    SummaryValidationExcel.XWorkSheet.Cells[rowNumber, columnNumber].Value = Util.FormatExport(trimmedCellValue);


                    columnNumber++;
                }

                rowNumber++;
            }

            SummaryValidationExcel.XWorkSheet.Columns.AutoFit();
        }

        #region current process to process using html data inteads of db.
        //public void Process()
        //{
        //    GenerateSoaDataValidationSummaryFile();

        //    SummaryValidationExcel.OpenTemplate();

        //    using (var db = new AppDbContext(false))
        //    {
        //        var query = db.SoaDataRiDataSummaries.Select(SoaDataRiDataSummaryService.Expression()).Where(q => q.SoaDataBatchId == SoaDataBatchId);

        //        if (OriginalCurrency)
        //            query = query.Where(q => !string.IsNullOrEmpty(q.CurrencyCode) && q.CurrencyCode != PickListDetailBo.CurrencyCodeMyr);
        //        else
        //            query = query.Where(q => string.IsNullOrEmpty(q.CurrencyCode) || q.CurrencyCode == PickListDetailBo.CurrencyCodeMyr);

        //        switch (Type)
        //        {
        //            case 1:
        //                SoaDataQuery = query.Where(q => q.Type == SoaDataRiDataSummaryBo.TypeSoaData && q.BusinessCode != PickListDetailBo.BusinessOriginCodeServiceFee);
        //                RiSummaryQuery = query.Where(q => q.Type == SoaDataRiDataSummaryBo.TypeRiDataSummary && q.BusinessCode != PickListDetailBo.BusinessOriginCodeServiceFee);
        //                DifferencesQuery = query.Where(q => q.Type == SoaDataRiDataSummaryBo.TypeDifferences && q.BusinessCode != PickListDetailBo.BusinessOriginCodeServiceFee);
        //                break;
        //            case 2:
        //                SoaDataQuery = query.Where(q => q.Type == SoaDataRiDataSummaryBo.TypeSoaData && q.BusinessCode == PickListDetailBo.BusinessOriginCodeServiceFee);
        //                RiSummaryQuery = query.Where(q => q.Type == SoaDataRiDataSummaryBo.TypeRiDataSummary && q.BusinessCode == PickListDetailBo.BusinessOriginCodeServiceFee);
        //                DifferencesQuery = query.Where(q => q.Type == SoaDataRiDataSummaryBo.TypeDifferences && q.BusinessCode == PickListDetailBo.BusinessOriginCodeServiceFee);
        //                break;
        //        }

        //        SummaryValidationExcel.XWorkBook = (SummaryValidationExcel.XApp.Workbooks.Add(System.Reflection.Missing.Value));
        //        SummaryValidationExcel.XWorkSheet = SummaryValidationExcel.XWorkBook.ActiveSheet;

        //        for (var i = TotalSheet; i > 0; i--)
        //        {
        //            CurrentQuery = i;
        //            GetQuery();

        //            SummaryValidationExcel.XWorkSheet = SummaryValidationExcel.XApp.Worksheets.Add();
        //            SummaryValidationExcel.XWorkSheet.Name = SheetNames[i - 1];

        //            //GetColumns();
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

        //    SummaryValidationExcel.Save();
        //}

        //public void GenerateSoaDataValidationSummaryFile()
        //{
        //    Column = SoaDataRiDataSummaryBo.GetColumns(TypeRetakaful);

        //    var templateFilepath = Util.GetWebAppDocumentFilePath("SoaDataSummary_Template.xlsx");
        //    FileName = string.Format("SoaDataValidation{0}_{1}", (OriginalCurrency ? "ORI" : ""), SoaDataBatchId).AppendDateTimeFileName(".xlsx");
        //    Directory = Util.GetRetroStatementDownloadPath();
        //    FilePath = Path.Combine(Util.GetTemporaryPath(), FileName);
        //    SummaryValidationExcel = new Excel(templateFilepath, FilePath, 1);
        //}

        //public void WriteHeaderLine()
        //{
        //    if (SummaryValidationExcel == null)
        //        return;

        //    int colIndex;
        //    int index = 1;
        //    foreach (var col in Column)
        //    {
        //        colIndex = index;
        //        if (col.ColIndex.HasValue)
        //            colIndex = col.ColIndex.Value;

        //        SummaryValidationExcel.WriteCell(RowIndex, colIndex, col.Header);

        //        index++;
        //    }

        //    RowIndex++;
        //}

        //public object GetEntity()
        //{
        //    return SoaDataRiDataSummaryBo;
        //}

        //public IQueryable<object> GetQuery()
        //{
        //    switch (CurrentQuery)
        //    {
        //        case 1:
        //            Query = SoaDataQuery;
        //            break;
        //        case 2:
        //            Query = RiSummaryQuery;
        //            break;
        //        case 3:
        //            Query = DifferencesQuery;
        //            break;
        //    }
        //    return Query;
        //}

        //public IEnumerable<object> GetQueryNext()
        //{
        //    if (Query == null)
        //        return new List<SoaDataRiDataSummaryBo> { };
        //    return Query.OrderBy(q => q.TreatyCode).Skip(Skip).Take(Take).ToList();
        //}

        //public int GetQueryTotal()
        //{
        //    if (Query == null)
        //        Total = 0;
        //    else
        //        Total = Query.Count();
        //    return Total;
        //}

        //public void SetEntity(object entity)
        //{
        //    SoaDataRiDataSummaryBo = null;
        //    if (entity is SoaDataRiDataSummaryBo e)
        //        SoaDataRiDataSummaryBo = e;
        //}

        //public void SetQuery(IQueryable<object> query)
        //{
        //    Query = null;
        //    if (query is IQueryable<SoaDataRiDataSummaryBo> q)
        //        Query = q;
        //}

        //public void WriteDataLine()
        //{
        //    if (SummaryValidationExcel == null)
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

        //        SummaryValidationExcel.WriteCell(RowIndex, colIndex, GetColumnValue(col, entity));

        //        index++;
        //    }

        //    RowIndex++;
        //}

        //public virtual string GetColumnValue(Column col, object entity)
        //{
        //    return Util.FormatExport(entity.GetPropertyValue(col.Property));
        //}
        #endregion
    }
}
