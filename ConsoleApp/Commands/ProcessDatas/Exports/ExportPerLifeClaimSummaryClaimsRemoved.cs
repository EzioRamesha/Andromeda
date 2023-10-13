using BusinessObject;
using BusinessObject.Retrocession;
using BusinessObject.SoaDatas;
using Services;
using Services.Identity;
using Shared;
using Shared.ProcessFile;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleApp.Commands.ProcessDatas.Exports
{
    class ExportPerLifeClaimSummaryClaimsRemoved : ExportFile
    {
        public PerLifeClaimRetroDataBo PerLifeClaimRetroDataBo { get; set; }
        public IQueryable<PerLifeClaimRetroDataBo> Query { get; set; }
        public override string PrefixFileName { get; set; } = "PerLifeClaimSummaryClaimsRemoved";

        public override List<Column> GetColumns()
        {
            Columns = PerLifeClaimRetroDataBo.GetColumns();
            return Columns;
        }

        public override string GetColumnValue(Column col, object entity)
        {
            object v = entity.GetPropertyValue(col.Property);
            switch (col.Property)
            {
                case "ClaimRegisterHistory.ClaimStatus":
                    if (v != null)
                    {
                        return ClaimRegisterBo.GetStatusName(int.Parse(v.ToString()));
                    }
                    else return "";
                case "ClaimRegisterHistory.ProvisionStatus":
                    if (v != null)
                    {
                        return ClaimRegisterBo.GetProvisionStatusName(int.Parse(v.ToString()));
                    }
                    else return "";
                case "ClaimRegisterHistory.OffsetStatus":
                    if (v != null)
                    {
                        return ClaimRegisterBo.GetOffsetStatusName(int.Parse(v.ToString()));
                    }
                    else return "";
                default:
                    return Util.FormatExport(v);
            }
        }

        public override object GetEntity()
        {

            return PerLifeClaimRetroDataBo;

        }

        public override IQueryable<object> GetQuery()
        {
            return Query;
        }

        public override IEnumerable<object> GetQueryNext()
        {
            if (Query == null)
                return new List<PerLifeClaimRetroDataBo> { };
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
            PerLifeClaimRetroDataBo = null;
            if (entity is PerLifeClaimRetroDataBo e)
                PerLifeClaimRetroDataBo = e;
        }

        public override void SetQuery(IQueryable<object> query)
        {
            Query = null;
            if (query is IQueryable<PerLifeClaimRetroDataBo> q)
                Query = q;
        }

    }
}
