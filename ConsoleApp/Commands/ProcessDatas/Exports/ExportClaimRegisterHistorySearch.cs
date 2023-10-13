using BusinessObject;
using Services;
using Services.Identity;
using Shared;
using Shared.ProcessFile;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleApp.Commands.ProcessDatas.Exports
{
    class ExportClaimRegisterHistorySearch : ExportFile
    {
        public ClaimRegisterHistoryBo ClaimRegisterHistoryBo { get; set; }
        public IQueryable<ClaimRegisterHistoryBo> Query { get; set; }
        public override string PrefixFileName { get; set; } = "ClaimRegisterSearch";
        public bool IsWithAdjustmentDetails { get; set; } = false;

        public override List<Column> GetColumns()
        {
            Columns = ClaimRegisterBo.GetColumns(IsWithAdjustmentDetails);
            return Columns;
        }

        public override string GetColumnValue(Column col, object entity)
        {
            object v = entity.GetPropertyValue(col.Property);
            switch (col.Property)
            {
                case "ClaimStatus":
                    return ClaimRegisterBo.GetStatusName(int.Parse(v.ToString()));
                case "MappingStatus":
                    return ClaimRegisterBo.GetMappingStatusName(int.Parse(v.ToString()));
                case "ProcessingStatus":
                    return ClaimRegisterBo.GetProcessingStatusName(int.Parse(v.ToString()));
                case "DuplicationCheckStatus":
                    return ClaimRegisterBo.GetDuplicationCheckStatusName(int.Parse(v.ToString()));
                case "PostComputationStatus":
                    return ClaimRegisterBo.GetPostComputationStatusName(int.Parse(v.ToString()));
                case "PostValidationStatus":
                    return ClaimRegisterBo.GetPostValidationStatusName(int.Parse(v.ToString()));
                case "ProvisionStatus":
                    return ClaimRegisterBo.GetProvisionStatusName(int.Parse(v.ToString()));
                case "OffsetStatus":
                    return ClaimRegisterBo.GetOffsetStatusName(int.Parse(v.ToString()));
                case "PicClaimId":
                case "PicDaaId":
                    return v != null ? UserService.Find(int.Parse(v.ToString()))?.UserName : null;
                case "ClaimReasonId":
                    return v != null ? ClaimReasonService.Find(int.Parse(v.ToString()))?.Reason : null;
                case "TreatyCode":
                    if (col.Header == "BUSINESS ORIGIN")
                        return v != null ? TreatyCodeService.GetBusinessOriginByCode(v.ToString()) : null;
                    return Util.FormatExport(v);
                default:
                    return Util.FormatExport(v);
            }
        }

        public override object GetEntity()
        {
            return ClaimRegisterHistoryBo;
        }

        public override IQueryable<object> GetQuery()
        {
            return Query;
        }

        public override IEnumerable<object> GetQueryNext()
        {
            if (Query == null)
                return new List<ClaimRegisterHistoryBo> { };
            return Query.OrderBy(q => q.ClaimRegisterId).ThenBy(q => q.SortIndex).Skip(Skip).Take(Take).ToList();
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
            ClaimRegisterHistoryBo = null;
            if (entity is ClaimRegisterHistoryBo e)
                ClaimRegisterHistoryBo = e;
        }

        public override void SetQuery(IQueryable<object> query)
        {
            Query = null;
            if (query is IQueryable<ClaimRegisterHistoryBo> q)
                Query = q;
        }
    }
}
