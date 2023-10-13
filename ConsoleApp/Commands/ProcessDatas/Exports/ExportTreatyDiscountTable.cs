using BusinessObject;
using Shared;
using Shared.ProcessFile;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleApp.Commands.ProcessDatas.Exports
{
    public class ExportTreatyDiscountTable : ExportFile
    {
        public TreatyDiscountTableBo TreatyDiscountTableBo { get; set; }
        public IQueryable<TreatyDiscountTableBo> Query { get; set; }
        public override string PrefixFileName { get; set; } = "TreatyDiscountTable";

        public override List<Column> GetColumns()
        {
            Columns = TreatyDiscountTableBo.GetColumns();
            return Columns;
        }

        public override string GetColumnValue(Column col, object entity)
        {
            object v = entity.GetPropertyValue(col.Property);

            switch (col.Property)
            {
                case "Type":
                    return TreatyDiscountTableBo.GetTypeName(int.Parse(v.ToString()));
                default:
                    return Util.FormatExport(v);
            }
        }

        public override object GetEntity()
        {
            return TreatyDiscountTableBo;
        }

        public override IQueryable<object> GetQuery()
        {
            return Query;
        }

        public override IEnumerable<object> GetQueryNext()
        {
            if (Query == null)
                return new List<TreatyDiscountTableBo> { };
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
            TreatyDiscountTableBo = null;
            if (entity is TreatyDiscountTableBo e)
                TreatyDiscountTableBo = e;
        }

        public override void SetQuery(IQueryable<object> query)
        {
            Query = null;
            if (query is IQueryable<TreatyDiscountTableBo> q)
                Query = q;
        }
    }
}
