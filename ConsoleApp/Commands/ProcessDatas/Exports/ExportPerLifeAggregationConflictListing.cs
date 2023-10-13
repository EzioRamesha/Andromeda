using BusinessObject.Retrocession;
using BusinessObject.RiDatas;
using Shared;
using Shared.ProcessFile;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleApp.Commands.ProcessDatas.Exports
{
    public class ExportPerLifeAggregationConflictListing : ExportFile
    {
        public PerLifeAggregationDetailDataBo PerLifeAggregationDetailDataBo { get; set; }
        public IQueryable<PerLifeAggregationDetailDataBo> Query { get; set; }
        public override string PrefixFileName { get; set; } = "PerLifeAggregationConflictListing";

        public override List<Column> GetColumns()
        {
            Columns = PerLifeAggregationDetailDataBo.GetConflictListingColumns();
            return Columns;
        }

        public override object GetEntity()
        {
            return PerLifeAggregationDetailDataBo;
        }

        public override IQueryable<object> GetQuery()
        {
            return Query;
        }

        public override IEnumerable<object> GetQueryNext()
        {
            if (Query == null)
                return new List<PerLifeAggregationDetailDataBo> { };
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
            PerLifeAggregationDetailDataBo = null;
            if (entity is PerLifeAggregationDetailDataBo e)
                PerLifeAggregationDetailDataBo = e;
        }

        public override void SetQuery(IQueryable<object> query)
        {
            Query = null;
            if (query is IQueryable<PerLifeAggregationDetailDataBo> q)
                Query = q;
        }
    }
}
