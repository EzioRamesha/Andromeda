using BusinessObject.SoaDatas;
using Services.Identity;
using Shared;
using Shared.ProcessFile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp.Commands.ProcessDatas.Exports
{
    public class ExportSoaDataCompiledSummary : ExportFile
    {
        public SoaDataCompiledSummaryBo SoaDataCompiledSummaryBo { get; set; }
        public IQueryable<SoaDataCompiledSummaryBo> Query { get; set; }

        public override string PrefixFileName { get; set; } = "SoaDataCompiledSummary";

        public override bool IsTextFile { get; set; } = false;

        public override List<Column> GetColumns()
        {
            Columns = SoaDataCompiledSummaryBo.GetColumns();
            return Columns;
        }

        public override object GetEntity()
        {
            return SoaDataCompiledSummaryBo;
        }

        public override IQueryable<object> GetQuery()
        {
            return Query;
        }

        public override IEnumerable<object> GetQueryNext()
        {
            if (Query == null)
                return new List<SoaDataCompiledSummaryBo> { };
            return Query.OrderBy(q => q.InvoiceType).Skip(Skip).Take(Take).ToList();
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
            SoaDataCompiledSummaryBo = null;
            if (entity is SoaDataCompiledSummaryBo e)
                SoaDataCompiledSummaryBo = e;
        }

        public override void SetQuery(IQueryable<object> query)
        {
            Query = null;
            if (query is IQueryable<SoaDataCompiledSummaryBo> q)
                Query = q;
        }

        public override string GetColumnValue(Column col, object entity)
        {
            object v = entity.GetPropertyValue(col.Property);
            switch (col.Property)
            {
                case "InvoiceType":
                    return SoaDataCompiledSummaryBo.GetInvoiceTypeName(int.Parse(v.ToString()));
                case "CreatedById":
                    return v != null ? UserService.Find(int.Parse(v.ToString()))?.UserName : null;
                default:
                    return Util.FormatExport(v);
            }
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

            if (GetQuery() == null)
                return;

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
