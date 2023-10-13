using BusinessObject.TreatyPricing;
using Services;
using Services.Identity;
using Services.TreatyPricing;
using Shared;
using Shared.ProcessFile;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleApp.Commands.ProcessDatas.Exports
{
    public class ExportGroupReferral : ExportFile
    {
        public TreatyPricingGroupReferralVersionBo TreatyPricingGroupReferralVersionBo { get; set; }
        public IQueryable<TreatyPricingGroupReferralVersionBo> Query { get; set; }
        public override string PrefixFileName { get; set; } = "GroupReferral";
        public bool IsTracking { get; set; } = false;

        public override bool IsTextFile { get; set; } = false;

        public override List<Column> GetColumns()
        {
            // Default
            Columns = TreatyPricingGroupReferralVersionBo.GetColumns();

            if (IsTracking)
                Columns = TreatyPricingGroupReferralVersionBo.GetTrackingColumns();
            
            return Columns;
        }

        public override string GetColumnValue(Column col, object entity)
        {
            object v = entity.GetPropertyValue(col.Property);
            switch (col.Property)
            {
                case "CedantId":
                    if (!IsTracking)
                        return v != null ? CedantService.Find(int.Parse(v.ToString()))?.Name : null;
                    else
                        return v != null ? CedantService.Find(int.Parse(v.ToString()))?.Code : null;
                case "InsuredGroupNameId":
                    return v != null ? InsuredGroupNameService.Find(int.Parse(v.ToString()))?.Name : null;
                case "IndustryNamePickListDetailId":
                case "ReferredTypePickListDetailId":
                case "RequestTypePickListDetailId":
                    return v != null ? PickListDetailService.Find(int.Parse(v.ToString()))?.Description : null;
                case "RiGroupSlipPersonInChargeId":
                case "GroupReferralPersonInChargeId":
                    return v != null ? UserService.Find(int.Parse(v.ToString()))?.FullName : null;
                case "RiGroupSlipStatus":
                    return v != null ? TreatyPricingGroupReferralBo.GetRiGroupSlipStatusName(int.Parse(v.ToString())) : null;
                case "TreatyPricingGroupMasterLetterId":
                    return v != null ? TreatyPricingGroupMasterLetterService.Find(int.Parse(v.ToString()))?.Code : null;
                case "GroupReferralStatus":
                    return TreatyPricingGroupReferralBo.GetStatusName(int.Parse(v.ToString()));
                case "WorkflowStatusId":
                    return TreatyPricingGroupReferralBo.GetWorkflowStatusName(int.Parse(v.ToString()));
                case "Version":
                    return string.Format("v{0}.0", v);
                case "ChecklistPendingUnderwriting":
                case "ChecklistPendingHealth":
                case "ChecklistPendingClaims":
                case "ChecklistPendingBD":
                case "ChecklistPendingCR":
                    return bool.Parse(v.ToString()) ? "Y" : "N";
                case "CommissionMarginDEA":
                    if (TreatyPricingGroupReferralVersionBenefitService.ExistByTreatyPricingGroupReferralVersionIdBenefitCode(int.Parse(v.ToString()), "DEA"))
                        return v != null ? TreatyPricingGroupReferralVersionBenefitService.GetBenefitCommissionMargin(int.Parse(v.ToString()), "DEA") : null;
                    else
                        return v != null ? TreatyPricingGroupReferralVersionBenefitService.GetBenefitCommissionMargin(int.Parse(v.ToString()), "DEA_N") : null;
                case "CommissionMarginMSE":
                    return v != null ? TreatyPricingGroupReferralVersionBenefitService.GetBenefitCommissionMargin(int.Parse(v.ToString()), "MSE") : null;
                case "ExpenseMarginDEA":
                    if (TreatyPricingGroupReferralVersionBenefitService.ExistByTreatyPricingGroupReferralVersionIdBenefitCode(int.Parse(v.ToString()), "DEA"))
                        return v != null ? TreatyPricingGroupReferralVersionBenefitService.GetBenefitExpenseMargin(int.Parse(v.ToString()), "DEA") : null;
                    else
                        return v != null ? TreatyPricingGroupReferralVersionBenefitService.GetBenefitExpenseMargin(int.Parse(v.ToString()), "DEA_N") : null;
                case "ExpenseMarginMSE":
                    return v != null ? TreatyPricingGroupReferralVersionBenefitService.GetBenefitExpenseMargin(int.Parse(v.ToString()), "MSE") : null;
                case "ProfitMarginDEA":
                    if (TreatyPricingGroupReferralVersionBenefitService.ExistByTreatyPricingGroupReferralVersionIdBenefitCode(int.Parse(v.ToString()), "DEA"))
                        return v != null ? TreatyPricingGroupReferralVersionBenefitService.GetBenefitProfitMargin(int.Parse(v.ToString()), "DEA") : null;
                    else
                        return v != null ? TreatyPricingGroupReferralVersionBenefitService.GetBenefitProfitMargin(int.Parse(v.ToString()), "DEA_N") : null;
                case "ProfitMarginMSE":
                    return v != null ? TreatyPricingGroupReferralVersionBenefitService.GetBenefitProfitMargin(int.Parse(v.ToString()), "MSE") : null;
                default:
                    return Util.FormatExport(v);
            }           
        }

        public override object GetEntity()
        {
            return TreatyPricingGroupReferralVersionBo;
        }

        public override IQueryable<object> GetQuery()
        {
            return Query;
        }

        public override IEnumerable<object> GetQueryNext()
        {
            if (Query == null)
                return new List<TreatyPricingGroupReferralVersionBo> { };
            return Query.OrderBy(q => q.CedantId).Skip(Skip).Take(Take).ToList();
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
            TreatyPricingGroupReferralVersionBo = null;
            if (entity is TreatyPricingGroupReferralVersionBo e)
                TreatyPricingGroupReferralVersionBo = e;            
        }

        public override void SetQuery(IQueryable<object> query)
        {
            Query = null;
            if (query is IQueryable<TreatyPricingGroupReferralVersionBo> q)
                Query = q;
        }
    }
}
