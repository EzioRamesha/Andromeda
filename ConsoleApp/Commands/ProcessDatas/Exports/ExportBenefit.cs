using BusinessObject;
using Services;
using Shared;
using Shared.ProcessFile;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleApp.Commands.ProcessDatas.Exports
{
    public class ExportBenefit : ExportFile
    {
        public BenefitBo BenefitBo { get; set; }
        public IList<BenefitDetailBo> BenefitDetailBos { get; set; }
        public IQueryable<BenefitBo> Query { get; set; }
        public override string PrefixFileName { get; set; } = "Benefit";

        public override List<Column> GetColumns()
        {
            Columns = BenefitBo.GetColumns();
            return Columns;
        }

        public override string GetColumnValue(Column col, object entity)
        {
            object v = entity.GetPropertyValue(col.Property);
            switch (col.ColIndex)
            {
                case BenefitBo.ColumnStatus:
                    return BenefitBo.GetStatusName(int.Parse(v.ToString()));
                case BenefitBo.ColumnMLReEventCode:
                    if (!BenefitDetailBos.IsNullOrEmpty())
                        return string.Join(",", BenefitDetailBos.Select(x => x.EventCodeBo.Code).ToList());
                    return null;
                case BenefitBo.ColumnClaimCode:
                    if (!BenefitDetailBos.IsNullOrEmpty())
                        return string.Join(",", BenefitDetailBos.Select(x => x.ClaimCodeBo.Code).ToList());
                    return null;
                case BenefitBo.ColumnValuationBenefitCode:
                case BenefitBo.ColumnBenefitCategoryCode:
                    return v != null ? PickListDetailService.Find(int.Parse(v.ToString())).Code : null;
                default:
                    return Util.FormatExport(v);
            }
        }

        public override object GetEntity()
        {
            return BenefitBo;
        }

        public override IQueryable<object> GetQuery()
        {
            return Query;
        }

        public override IEnumerable<object> GetQueryNext()
        {
            if (Query == null)
                return new List<BenefitBo> { };
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
            BenefitBo = null;
            BenefitDetailBos = new List<BenefitDetailBo> { };
            if (entity is BenefitBo e)
            {
                BenefitBo = e;
                BenefitDetailBos = BenefitDetailService.GetByBenefitId(e.Id);
            }
        }

        public override void SetQuery(IQueryable<object> query)
        {
            Query = null;
            if (query is IQueryable<BenefitBo> q)
                Query = q;
        }
    }
}
