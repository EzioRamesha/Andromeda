using BusinessObject;
using Shared;
using Shared.ProcessFile;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleApp.Commands.ProcessDatas.Exports
{
    public class ExportRetroSummary : ExportFile
    {
        public RetroSummaryBo RetroSummaryBo { get; set; }
        public IQueryable<RetroSummaryBo> Query { get; set; }
        public override string PrefixFileName { get; set; } = "RetroSummary";

        public string SoaQuarter { get; set; }

        public ExportRetroSummary(string soaQuarter)
        {
            SoaQuarter = soaQuarter;
        }

        public override List<Column> GetColumns()
        {
            Columns = RetroSummaryBo.GetColumns();
            return Columns;
        }

        public override string GetColumnValue(Column col, object entity)
        {
            object v = entity.GetPropertyValue(col.Property);
            switch (col.Property)
            {
                case "SoaQuarter":
                    return SoaQuarter;
                default:
                    return Util.FormatExport(v);
            }
        }

        public override object GetEntity()
        {
            return RetroSummaryBo;
        }

        public override IQueryable<object> GetQuery()
        {
            return Query;
        }

        public override IEnumerable<object> GetQueryNext()
        {
            if (Query == null)
                return new List<RetroSummaryBo> { };
            return Query.Skip(Skip).Take(Take).ToList();
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
            RetroSummaryBo = null;
            if (entity is RetroSummaryBo e)
                RetroSummaryBo = e;
        }

        public override void SetQuery(IQueryable<object> query)
        {
            Query = null;
            if (query is IQueryable<RetroSummaryBo> q)
                Query = q;
        }
    }
}
