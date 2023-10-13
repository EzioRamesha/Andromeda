using BusinessObject.Retrocession;
using BusinessObject.RiDatas;
using Shared;
using Shared.ProcessFile;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleApp.Commands.ProcessDatas.Exports
{
    public class ExportPerLifeAggregationDetailTreaty : ExportFile
    {
        public PerLifeAggregationDetailTreatyBo PerLifeAggregationDetailTreatyBo { get; set; }
        public bool IsExcludedRecordSummary { get; set; } = false;
        public bool IsRetroRecordSummary { get; set; } = false;
        public IQueryable<PerLifeAggregationDetailTreatyBo> Query { get; set; }
        public override string PrefixFileName { get; set; } = "PerLifeAggregationDetailTreaty";

        public override List<Column> GetColumns()
        {
            // Default
            Columns = PerLifeAggregationDetailTreatyBo.GetColumns();

            if (IsExcludedRecordSummary)
                Columns = PerLifeAggregationDetailTreatyBo.GetExcludedSummaryColumns();

            if (IsRetroRecordSummary)
                Columns = PerLifeAggregationDetailTreatyBo.GetRetroSummaryColumns();

            return Columns;
        }

        public override object GetEntity()
        {
            return PerLifeAggregationDetailTreatyBo;
        }

        public override IQueryable<object> GetQuery()
        {
            return Query;
        }

        public override IEnumerable<object> GetQueryNext()
        {
            if (Query == null)
                return new List<PerLifeAggregationDetailTreatyBo> { };

            if (IsExcludedRecordSummary || IsRetroRecordSummary)
            {
                return Query.OrderByDescending(q => q.RiskQuarter).ThenBy(q => q.TreatyCode).Skip(Skip).Take(Take).ToList();
            } else
            {
                return Query.OrderBy(q => q.Id).Skip(Skip).Take(Take).ToList();
            }
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
            PerLifeAggregationDetailTreatyBo = null;
            if (entity is PerLifeAggregationDetailTreatyBo e)
                PerLifeAggregationDetailTreatyBo = e;
        }

        public override void SetQuery(IQueryable<object> query)
        {
            Query = null;
            if (query is IQueryable<PerLifeAggregationDetailTreatyBo> q)
                Query = q;
        }
    }
}
