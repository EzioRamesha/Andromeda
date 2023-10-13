using BusinessObject;
using Shared.ProcessFile;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleApp.Commands.ProcessDatas.Exports
{
    public class ExportRateTable : ExportFile
    {
        public RateTableBo RateTableBo { get; set; }
        public IQueryable<RateTableBo> Query { get; set; }
        public override string PrefixFileName { get; set; } = "RateTableMapping";

        public override List<Column> GetColumns()
        {
            Columns = RateTableBo.GetColumns();
            return Columns;
        }

        public override object GetEntity()
        {
            return RateTableBo;
        }

        public override IQueryable<object> GetQuery()
        {
            return Query;
        }

        public override IEnumerable<object> GetQueryNext()
        {
            if (Query == null)
                return new List<RateTableBo> { };
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
            RateTableBo = null;
            if (entity is RateTableBo e)
                RateTableBo = e;
        }

        public override void SetQuery(IQueryable<object> query)
        {
            Query = null;
            if (query is IQueryable<RateTableBo> q)
                Query = q;
        }
    }
}
