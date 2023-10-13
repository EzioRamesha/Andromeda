using BusinessObject.Retrocession;
using Shared;
using Shared.ProcessFile;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleApp.Commands.ProcessDatas.Exports
{
    public class ExportRetroBenefitRetentionLimit : ExportFile
    {
        public RetroBenefitRetentionLimitBo RetroBenefitRetentionLimitBo { get; set; }
        public IQueryable<RetroBenefitRetentionLimitBo> Query { get; set; }
        public override string PrefixFileName { get; set; } = "RetroBenefitRetentionLimit";
        public bool IsWithDetail { get; set; } = false;

        public override List<Column> GetColumns()
        {
            Columns = RetroBenefitRetentionLimitBo.GetColumns(IsWithDetail);
            return Columns;
        }

        public override object GetEntity()
        {
            return RetroBenefitRetentionLimitBo;
        }

        public override IQueryable<object> GetQuery()
        {
            return Query;
        }

        public override IEnumerable<object> GetQueryNext()
        {
            if (Query == null)
                return new List<RetroBenefitRetentionLimitBo> { };
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
            RetroBenefitRetentionLimitBo = null;
            if (entity is RetroBenefitRetentionLimitBo e)
                RetroBenefitRetentionLimitBo = e;
        }

        public override void SetQuery(IQueryable<object> query)
        {
            Query = null;
            if (query is IQueryable<RetroBenefitRetentionLimitBo> q)
                Query = q;
        }
    }
}
