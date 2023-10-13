using BusinessObject.SoaDatas;
using Shared.ProcessFile;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleApp.Commands.ProcessDatas.Exports
{
    public class ExportSoaDataPostValidation : ExportFile
    {
        public SoaDataPostValidationBo SoaDataPostValidationBo { get; set; }
        public SoaDataPostValidationDifferenceBo SoaDataPostValidationDifferenceBo { get; set; }
        public IQueryable<SoaDataPostValidationBo> Query { get; set; }
        public IQueryable<SoaDataPostValidationBo> MlreCheckingQuery { get; set; }
        public IQueryable<SoaDataPostValidationBo> CedantAmtQuery { get; set; }
        public IQueryable<SoaDataPostValidationBo> DiscrepancyCheckQuery { get; set; }
        public IQueryable<SoaDataPostValidationDifferenceBo> DifferencesQuery { get; set; }
        public override string PrefixFileName { get; set; } = "SoaDataPostValidation";

        public override bool IsTextFile { get; set; } = false;
        public bool TypeDifferences { get; set; } = false;
        public List<string> SheetNames { get; set; }
        public int TotalSheet { get; set; } = 4;
        public int CurrentQuery { get; set; } = 1;

        public override List<Column> GetColumns()
        {
            if (TypeDifferences)
                Columns = SoaDataPostValidationDifferenceBo.GetColumns();
            else
                Columns = SoaDataPostValidationBo.GetColumns();
            return Columns;
        }

        public override object GetEntity()
        {
            if (TypeDifferences)
                return SoaDataPostValidationDifferenceBo;
            else
                return SoaDataPostValidationBo;
        }

        public override IQueryable<object> GetQuery()
        {
            if (CurrentQuery == 4)
            {
                TypeDifferences = true;
                return DifferencesQuery;
            }

            switch (CurrentQuery)
            {
                case 1:
                    Query = MlreCheckingQuery;
                    break;
                case 2:
                    Query = CedantAmtQuery;
                    break;
                case 3:
                    Query = DiscrepancyCheckQuery;
                    break;
            }
            TypeDifferences = false;
            return Query;
        }

        public override IEnumerable<object> GetQueryNext()
        {
            if (TypeDifferences)
            {
                if (DifferencesQuery == null)
                    return new List<SoaDataPostValidationDifferenceBo> { };
                return DifferencesQuery.OrderBy(q => q.TreatyCode).Skip(Skip).Take(Take).ToList();
            }
            else
            {
                if (Query == null)
                    return new List<SoaDataPostValidationBo> { };
                return Query.OrderBy(q => q.TreatyCode).Skip(Skip).Take(Take).ToList();
            }
        }

        public override int GetQueryTotal()
        {
            if (TypeDifferences)
            {
                if (DifferencesQuery == null)
                    Total = 0;
                else
                    Total = DifferencesQuery.Count();
                return Total;
            }


            if (Query == null)
                Total = 0;
            else
                Total = Query.Count();
            return Total;
        }

        public override void SetEntity(object entity)
        {
            if (TypeDifferences)
            {
                SoaDataPostValidationDifferenceBo = null;
                if (entity is SoaDataPostValidationDifferenceBo e)
                    SoaDataPostValidationDifferenceBo = e;
            }
            else
            {
                SoaDataPostValidationBo = null;
                if (entity is SoaDataPostValidationBo e)
                    SoaDataPostValidationBo = e;
            }
        }

        public override void SetQuery(IQueryable<object> query)
        {
            Query = null;
            if (query is IQueryable<SoaDataPostValidationBo> q)
                Query = q;
        }

        public override void WriteDataLine()
        {
            if (Excel == null)
                return;

            object entity = GetEntity();
            if (entity == null)
                return;

            int colIndex;
            int index = 1;
            foreach (var col in Columns)
            {
                colIndex = index;
                if (col.ColIndex.HasValue)
                    colIndex = col.ColIndex.Value;

                Excel.WriteCell(RowIndex, colIndex, GetColumnValue(col, entity));

                index++;
            }

            RowIndex++;
        }
        public void ProcessData()
        {
            if (Excel == null)
                return;

            Excel.XWorkBook = (Excel.XApp.Workbooks.Add(System.Reflection.Missing.Value));
            Excel.XWorkSheet = Excel.XWorkBook.ActiveSheet;

            //xlWorkBook = xlApp.Workbooks.Add(misValue)             xlWorkSheet = xlWorkBook.Sheets.Add()

            for (var i = TotalSheet; i > 0; i--)
            {
                CurrentQuery = i;
                GetQuery();

                Excel.XWorkSheet = Excel.XApp.Worksheets.Add();
                Excel.XWorkSheet.Name = SheetNames[i - 1];

                GetColumns();
                RowIndex = 1;
                WriteHeaderLine();

                GetQueryTotal();
                for (Skip = 0; Skip < Total + Take; Skip += Take)
                {
                    if (Skip >= Total)
                        break;

                    var list = GetQueryNext();
                    if (list == null)
                        return;

                    foreach (var entity in list)
                    {
                        SetEntity(entity);
                        WriteDataLine();
                    }
                }
            }
            
        }
    }
}
