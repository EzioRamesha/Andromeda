using BusinessObject;
using Shared.ProcessFile;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleApp.Commands.ProcessDatas.Exports
{
    public class ExportProductFeatureMapping : ExportFile
    {
        public TreatyBenefitCodeMappingBo TreatyBenefitCodeMappingBo { get; set; }
        public IQueryable<TreatyBenefitCodeMappingBo> Query { get; set; }
        public override string PrefixFileName { get; set; } = "ProductFeatureMapping";

        public override List<Column> GetColumns()
        {
            Columns = TreatyBenefitCodeMappingBo.GetColumns();
            return Columns;
        }

        public override object GetEntity()
        {
            return TreatyBenefitCodeMappingBo;
        }

        public override IQueryable<object> GetQuery()
        {
            return Query;
        }

        public override IEnumerable<object> GetQueryNext()
        {
            if (Query == null)
                return new List<TreatyBenefitCodeMappingBo> { };
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
            TreatyBenefitCodeMappingBo = null;
            if (entity is TreatyBenefitCodeMappingBo e)
                TreatyBenefitCodeMappingBo = e;
        }

        public override void SetQuery(IQueryable<object> query)
        {
            Query = null;
            if (query is IQueryable<TreatyBenefitCodeMappingBo> q)
                Query = q;
        }
    }
}
