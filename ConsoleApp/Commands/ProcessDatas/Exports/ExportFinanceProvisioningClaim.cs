using BusinessObject;
using BusinessObject.Identity;
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
    public class ExportFinanceProvisioningClaim : ExportFile
    {
        public ClaimRegisterBo ClaimRegisterBo { get; set; }
        public UserBo PicClaimBo { get; set; }
        public UserBo PicDaaBo { get; set; }
        public IQueryable<ClaimRegisterBo> Query { get; set; }
        public override string PrefixFileName { get; set; } = "FinanceProvisioningClaims";

        public override List<Column> GetColumns()
        {
            Columns = ClaimRegisterBo.GetFinanceProvisioningColumns();
            return Columns;
        }

        public override string GetColumnValue(Column col, object entity)
        {
            object v = entity.GetPropertyValue(col.Property);
            switch (col.Property)
            {
                case "HasRedFlag":
                case "IsReferralCase":
                    if (bool.TryParse(v.ToString(), out bool b))
                        return Util.BoolToString(b);
                    return "";
                case "PicClaimId":
                    return PicClaimBo?.FullName;
                case "PicDaaId":
                    return PicDaaBo?.FullName;
                case "ClaimStatus":
                    return ClaimRegisterBo.GetStatusName(int.Parse(v.ToString()));
                case "ProvisionStatus":
                    return ClaimRegisterBo.GetProvisionStatusName(int.Parse(v.ToString()));
                case "OffsetStatus":
                    return ClaimRegisterBo.GetOffsetStatusName(int.Parse(v.ToString()));
                default:
                    return Util.FormatExport(v);
            }
        }

        public override object GetEntity()
        {
            return ClaimRegisterBo;
        }

        public override IQueryable<object> GetQuery()
        {
            return Query;
        }

        public override IEnumerable<object> GetQueryNext()
        {
            if (Query == null)
                return new List<ClaimRegisterBo> { };
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
            ClaimRegisterBo = null;
            if (entity is ClaimRegisterBo e)
            {
                ClaimRegisterBo = e;
                PicClaimBo = UserService.Find(ClaimRegisterBo.PicClaimId);
                PicDaaBo = UserService.Find(ClaimRegisterBo.PicDaaId);
            }
        }

        public override void SetQuery(IQueryable<object> query)
        {
            Query = null;
            if (query is IQueryable<ClaimRegisterBo> q)
                Query = q;
        }
    }
}
