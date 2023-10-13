using BusinessObject;
using BusinessObject.SoaDatas;
using DataAccess.EntityFramework;
using Services.Identity;
using Services.SoaDatas;
using Shared;
using Shared.ProcessFile;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ConsoleApp.Commands
{
    public class GenerateSoaDataCompiledSummary : Command
    {
        public Excel SummaryCompiledExcel { get; set; }

        public string Directory { get; set; }

        public string FileName { get; set; }

        public string FilePath { get; set; }

        public List<string> SheetNames { get; set; } = new List<string> { "IFRS4", "IFRS17" };
        public int TotalSheet { get; set; } = 2;

        public int RowIndex { get; set; } = 1;
        public int Skip { get; set; } = 0;
        public int Take { get; set; } = 500;
        public int Total { get; set; } = 0;

        public int CurrentQuery { get; set; } = 1;

        public int SoaDataBatchId { get; set; }
        public bool TypeIFRS17 { get; set; } = false;

        public SoaDataCompiledSummaryBo SoaDataCompiledSummaryBo { get; set; }
        public IQueryable<SoaDataCompiledSummaryBo> Query { get; set; }
        public IQueryable<SoaDataCompiledSummaryBo> CompiledSummaryIfrs4Query { get; set; }
        public IQueryable<SoaDataCompiledSummaryBo> CompiledSummaryIfrs17Query { get; set; }

        public List<Column> Column { get; set; }

        public List<string> StringRows { get; set; }

        public List<string> Errors { get; set; }

        public GenerateSoaDataCompiledSummary()
        {
            Title = "GenerateSoaDataCompiledSummary";
            Description = "To export soa data compiled summary excel file";
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
                SummaryCompiledExcel = new Excel(FilePath, false);

                ProcessHtml();
            }
            catch (Exception e)
            {
                Errors.Add(e.Message);
                SummaryCompiledExcel.Close();
                return;
            }

            PrintProcessCount();


            if (Errors.Count == 0)
            {
                SummaryCompiledExcel.FilePath = FilePath;
                SummaryCompiledExcel.Save();
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

                    SummaryCompiledExcel.XWorkSheet.Cells[rowNumber, columnNumber].Value = Util.FormatExport(trimmedCellValue);

                    columnNumber++;
                }

                rowNumber++;
            }

            SummaryCompiledExcel.XWorkSheet.Columns.AutoFit();
        }

        #region current process to process using html data inteads of db.
        //public void Process()
        //{
        //    GenerateSoaDataValidationSummaryFile();

        //    SummaryCompiledExcel.OpenTemplate();

        //    using (var db = new AppDbContext(false))
        //    {
        //        CompiledSummaryIfrs4Query = db.SoaDataCompiledSummaries.Select(SoaDataCompiledSummaryService.Expression())
        //            .Where(q => q.SoaDataBatchId == SoaDataBatchId)
        //            .Where(q => q.ReportingType == SoaDataCompiledSummaryBo.ReportingTypeIFRS4);

        //        CompiledSummaryIfrs17Query = db.SoaDataCompiledSummaries.Select(SoaDataCompiledSummaryService.Expression())
        //            .Where(q => q.SoaDataBatchId == SoaDataBatchId)
        //            .Where(q => q.ReportingType == SoaDataCompiledSummaryBo.ReportingTypeIFRS17);

        //        SummaryCompiledExcel.XWorkBook = (SummaryCompiledExcel.XApp.Workbooks.Add(System.Reflection.Missing.Value));
        //        SummaryCompiledExcel.XWorkSheet = SummaryCompiledExcel.XWorkBook.ActiveSheet;

        //        for (var i = TotalSheet; i > 0; i--)
        //        {
        //            CurrentQuery = i;
        //            GetQuery();

        //            SummaryCompiledExcel.XWorkSheet = SummaryCompiledExcel.XApp.Worksheets.Add();
        //            SummaryCompiledExcel.XWorkSheet.Name = SheetNames[i - 1];

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

        //    SummaryCompiledExcel.Save();
        //}

        //public void GenerateSoaDataValidationSummaryFile()
        //{
        //    var templateFilepath = Util.GetWebAppDocumentFilePath("SoaDataSummary_Template.xlsx");
        //    FileName = string.Format("SoaDataCompiled_{0}", SoaDataBatchId).AppendDateTimeFileName(".xlsx");
        //    Directory = Util.GetRetroStatementDownloadPath();
        //    FilePath = Path.Combine(Util.GetTemporaryPath(), FileName);
        //    SummaryCompiledExcel = new Excel(templateFilepath, FilePath, 1);
        //}

        //public void WriteHeaderLine()
        //{
        //    if (SummaryCompiledExcel == null)
        //        return;

        //    int colIndex;
        //    int index = 1;
        //    foreach (var col in Column)
        //    {
        //        colIndex = index;
        //        if (col.ColIndex.HasValue)
        //            colIndex = col.ColIndex.Value;

        //        SummaryCompiledExcel.WriteCell(RowIndex, colIndex, col.Header);

        //        index++;
        //    }

        //    RowIndex++;
        //}

        //public object GetEntity()
        //{
        //    return SoaDataCompiledSummaryBo;
        //}

        //public IQueryable<object> GetQuery()
        //{
        //    switch (CurrentQuery)
        //    {
        //        case 1:
        //            Query = CompiledSummaryIfrs4Query;
        //            TypeIFRS17 = false;
        //            break;
        //        case 2:
        //            Query = CompiledSummaryIfrs17Query;
        //            TypeIFRS17 = true;
        //            break;
        //    }
        //    return Query;
        //}

        //public IEnumerable<object> GetQueryNext()
        //{
        //    if (Query == null)
        //        return new List<SoaDataCompiledSummaryBo> { };
        //    return Query.OrderBy(q => q.InvoiceType).Skip(Skip).Take(Take).ToList();
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
        //    SoaDataCompiledSummaryBo = null;
        //    if (entity is SoaDataCompiledSummaryBo e)
        //        SoaDataCompiledSummaryBo = e;
        //}

        //public void SetQuery(IQueryable<object> query)
        //{
        //    Query = null;
        //    if (query is IQueryable<SoaDataCompiledSummaryBo> q)
        //        Query = q;
        //}

        //public void WriteDataLine()
        //{
        //    if (SummaryCompiledExcel == null)
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

        //        SummaryCompiledExcel.WriteCell(RowIndex, colIndex, GetColumnValue(col, entity));

        //        index++;
        //    }

        //    RowIndex++;
        //}

        //public void ProcessData()
        //{
        //    if (SummaryCompiledExcel == null)
        //        return;

        //    if (GetQuery() == null)
        //        return;

        //    GetQueryTotal();
        //    for (Skip = 0; Skip < Total + Take; Skip += Take)
        //    {
        //        if (Skip >= Total)
        //            break;

        //        var list = GetQueryNext();
        //        if (list == null)
        //            return;

        //        foreach (var entity in list)
        //        {
        //            SetEntity(entity);
        //            WriteDataLine();
        //        }
        //    }
        //}

        //public string GetColumnValue(Column col, object entity)
        //{
        //    object v = entity.GetPropertyValue(col.Property);
        //    switch (col.Property)
        //    {
        //        case "InvoiceType":
        //            return SoaDataCompiledSummaryBo.GetInvoiceTypeName(int.Parse(v.ToString()));
        //        case "CreatedById":
        //            return v != null ? UserService.Find(int.Parse(v.ToString()))?.UserName : null;
        //        default:
        //            return Util.FormatExport(v);
        //    }
        //}

        //public List<Column> GetColumns()
        //{
        //    Column = SoaDataCompiledSummaryBo.GetColumns(TypeIFRS17);
        //    return Column;
        //}
        #endregion
    }
}
