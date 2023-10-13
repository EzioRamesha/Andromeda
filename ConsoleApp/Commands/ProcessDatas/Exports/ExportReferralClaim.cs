using BusinessObject;
using Services;
using Services.Identity;
using Shared;
using Shared.ProcessFile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp.Commands.ProcessDatas.Exports
{
    public class ExportReferralClaim : ExportFile
    {
        public ReferralClaimBo ReferralClaimBo { get; set; }
        public IQueryable<ReferralClaimBo> Query { get; set; }
        public override string PrefixFileName { get; set; } = "ReferralClaim";

        public override List<Column> GetColumns()
        {
            Columns = ReferralClaimBo.GetColumns();
            return Columns;
        }

        public override string GetColumnValue(Column col, object entity)
        {
            object v = entity.GetPropertyValue(col.Property);
            switch (col.Property)
            {
                case "Status":
                    return ReferralClaimBo.GetStatusName(int.Parse(v.ToString()));
                case "TurnAroundTime":
                    if (v == null)
                        return "";
                    TimeSpan ts = new TimeSpan(long.Parse(v.ToString()));
                    if (ts == null)
                        return "";
                    return string.Format("{0} Hours {1} Minutes", ((ts.Days * 24) + ts.Hours), ts.Minutes);
                case "DocTurnAroundTime":
                    if (v == null)
                        return "";
                    TimeSpan dts = new TimeSpan(long.Parse(v.ToString()));
                    if (dts == null)
                        return "";
                    return string.Format("{0} Hours {1} Minutes", ((dts.Days * 24) + dts.Hours), dts.Minutes);
                case "ReferralReasonId":
                case "DelayReasonId":
                case "RetroReferralReasonId":
                case "MlreReferralReasonId":
                    return v != null ? ClaimReasonService.Find(int.Parse(v.ToString()))?.Reason : "";
                case "ClaimCategoryId":
                    return v != null ? ClaimCategoryService.Find(int.Parse(v.ToString()))?.Category : "";
                case "RetroReviewedById":
                case "AssessedById":
                case "ReviewedById":
                case "AssignedById":
                case "PersonInChargeId":
                    return v != null ? UserService.Find(int.Parse(v.ToString()))?.FullName : "";
                case "ClaimsDecision":
                    return v != null ? ReferralClaimBo.GetClaimsDecisionName(int.Parse(v.ToString())) : "";
                default:
                    return Util.FormatExport(v);
            }
        }

        public override object GetEntity()
        {
            return ReferralClaimBo;
        }

        public override IQueryable<object> GetQuery()
        {
            return Query;
        }

        public override IEnumerable<object> GetQueryNext()
        {
            if (Query == null)
                return new List<ReferralClaimBo> { };
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
            ReferralClaimBo = null;
            if (entity is ReferralClaimBo e)
                ReferralClaimBo = e;
        }

        public override void SetQuery(IQueryable<object> query)
        {
            Query = null;
            if (query is IQueryable<ReferralClaimBo> qrd)
                Query = qrd;
        }
    }
}
