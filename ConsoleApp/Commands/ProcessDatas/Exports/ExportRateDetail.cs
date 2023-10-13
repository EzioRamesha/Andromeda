using BusinessObject;
using Services;
using Shared;
using Shared.ProcessFile;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleApp.Commands.ProcessDatas.Exports
{
    public class ExportRateDetail : ExportFile
    {
        public RateDetailBo RateDetailBo { get; set; }
        public IQueryable<RateDetailBo> Query { get; set; }
        public override string PrefixFileName { get; set; } = "RateDetails";
        public List<int> ValuationRateFields { get; set; }

        public override List<Column> GetColumns()
        {
            Columns = RateDetailBo.GetColumns();
            return Columns;
        }

        public override string GetColumnValue(Column col, object entity)
        {
            object v = entity.GetPropertyValue(col.Property);

            switch (col.Property)
            {
                case "InsuredGenderCode":
                    if (ValuationRateFields.Contains(RateDetailBo.TypeInsuredGenderCode))
                        return v != null ? PickListDetailService.FindByPickListIdCode(PickListBo.InsuredGenderCode, v.ToString())?.Code : null;
                    else
                        return null;
                case "CedingTobaccoUse":
                    if (ValuationRateFields.Contains(RateDetailBo.TypeCedingTobaccoUse))
                        return v != null ? PickListDetailService.FindByPickListIdCode(PickListBo.InsuredTobaccoUse, v.ToString())?.Code : null;
                    else
                        return null;
                case "CedingOccupationCode":
                    if (ValuationRateFields.Contains(RateDetailBo.TypeCedingOccupationCode))
                        return v != null ? PickListDetailService.FindByPickListIdCode(PickListBo.InsuredOccupationCode, v.ToString())?.Code : null;
                    else
                        return null;
                case "AttainedAge":
                    if (ValuationRateFields.Contains(RateDetailBo.TypeAttainedAge))
                        return v != null ? Util.FormatExport(v) : null;
                    else
                        return null;
                case "IssueAge":
                    if (ValuationRateFields.Contains(RateDetailBo.TypeIssueAge))
                        return v != null ? Util.FormatExport(v) : null;
                    else
                        return null;
                case "PolicyTerm":
                    if (ValuationRateFields.Contains(RateDetailBo.TypePolicyTerm))
                        return v != null ? Util.FormatExport(v) : null;
                    else
                        return null;
                case "PolicyTermRemain":
                    if (ValuationRateFields.Contains(RateDetailBo.TypePolicyTermRemain))
                        return v != null ? Util.FormatExport(v) : null;
                    else
                        return null;
                default:
                    return Util.FormatExport(v);
            }
        }

        public override object GetEntity()
        {
            return RateDetailBo;
        }

        public override IQueryable<object> GetQuery()
        {
            return Query;
        }

        public override IEnumerable<object> GetQueryNext()
        {
            if (Query == null)
                return new List<RateDetailBo> { };
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
            RateDetailBo = null;
            if (entity is RateDetailBo e)
            {
                RateDetailBo = e;
                ValuationRateFields = RateBo.GetFieldsByValuationRate(RateService.Find(e.RateId).ValuationRate);
            }
        }

        public override void SetQuery(IQueryable<object> query)
        {
            Query = null;
            if (query is IQueryable<RateDetailBo> q)
                Query = q;
        }
    }
}
