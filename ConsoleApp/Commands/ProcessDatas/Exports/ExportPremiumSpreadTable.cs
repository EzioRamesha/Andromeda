using BusinessObject;
using Shared;
using Shared.ProcessFile;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleApp.Commands.ProcessDatas.Exports
{
    public class ExportPremiumSpreadTable : ExportFile
    {
        public PremiumSpreadTableBo PremiumSpreadTableBo { get; set; }
        public IQueryable<PremiumSpreadTableBo> Query { get; set; }
        public override string PrefixFileName { get; set; } = "PremiumSpreadTable";

        public override List<Column> GetColumns()
        {
            Columns = PremiumSpreadTableBo.GetColumns();
            return Columns;
        }

        public override string GetColumnValue(Column col, object entity)
        {
            object v = entity.GetPropertyValue(col.Property);

            switch (col.Property)
            {
                case "Type":
                    return PremiumSpreadTableBo.GetTypeName(int.Parse(v.ToString()));
                default:
                    return Util.FormatExport(v);
            }
        }

        public override object GetEntity()
        {
            return PremiumSpreadTableBo;
        }

        public override IQueryable<object> GetQuery()
        {
            return Query;
        }

        public override IEnumerable<object> GetQueryNext()
        {
            if (Query == null)
                return new List<PremiumSpreadTableBo> { };
            return Query.OrderBy(q => q.Id).Skip(Skip).Take(Take).ToList();
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
            PremiumSpreadTableBo = null;
            if (entity is PremiumSpreadTableBo e)
                PremiumSpreadTableBo = e;
        }

        public override void SetQuery(IQueryable<object> query)
        {
            Query = null;
            if (query is IQueryable<PremiumSpreadTableBo> q)
                Query = q;
        }
    }
}
