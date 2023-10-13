using BusinessObject.SoaDatas;
using Shared.ProcessFile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp.Commands.ProcessDatas.Exports
{
    public class ExportSoaDataValidation : ExportFile
    {
        public SoaDataBo SoaDataBo { get; set; } 
        public SoaDataRiDataSummaryBo SoaDataRiDataSummaryBo { get; set; }
        public IQueryable<SoaDataRiDataSummaryBo> Query { get; set; }
        public IQueryable<SoaDataRiDataSummaryBo> SoaDataQuery { get; set; }
        public IQueryable<SoaDataRiDataSummaryBo> RiSummaryQuery { get; set; }
        public IQueryable<SoaDataRiDataSummaryBo> DifferencesQuery { get; set; }

        public override string PrefixFileName { get; set; } = "SoaDataValidation";
        public override bool IsTextFile { get; set; } = false;

        public bool TypeRetakaful { get; set; } = false;
        public bool TypeRiDatas { get; set; } = false;
        public List<string> SheetNames { get; set; }
        public int TotalSheet { get; set; } = 3;
        public int CurrentQuery { get; set; } = 1;

        public override List<Column> GetColumns()
        {
            Columns = SoaDataRiDataSummaryBo.GetColumns(TypeRetakaful);
            return Columns;
        }

        public override object GetEntity()
        {
            return SoaDataRiDataSummaryBo;
        }

        public override IQueryable<object> GetQuery()
        {
            switch (CurrentQuery)
            {
                case 1:
                    Query = SoaDataQuery;
                    break;
                case 2:
                    Query = RiSummaryQuery;
                    break;
                case 3:
                    Query = DifferencesQuery;
                    break;
            }
            return Query;
        }

        public override IEnumerable<object> GetQueryNext()
        {
            if (Query == null)
                return new List<SoaDataRiDataSummaryBo> { };
            return Query.OrderBy(q => q.TreatyCode).Skip(Skip).Take(Take).ToList();
        }

        public override int GetQueryTotal()
        {
            if (Query == null)
                Total = 0;
            else
                Total = Query.Count();
            return Total;
        }

        public override void SetEntity(object entity)
        {
            SoaDataRiDataSummaryBo = null;
            if (entity is SoaDataRiDataSummaryBo e)
                SoaDataRiDataSummaryBo = e;
        }

        public override void SetQuery(IQueryable<object> query)
        {
            Query = null;
            if (query is IQueryable<SoaDataRiDataSummaryBo> q)
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
