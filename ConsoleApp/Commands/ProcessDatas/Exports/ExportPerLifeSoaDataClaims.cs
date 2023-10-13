using BusinessObject;
using BusinessObject.Retrocession;
using Shared;
using Shared.ProcessFile;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleApp.Commands.ProcessDatas.Exports
{
    public class ExportPerLifeSoaDataClaims : ExportFile
    {
        public PerLifeSoaDataBo PerLifeSoaDataBo { get; set; }
        public IList<PerLifeSoaDataBo> PerLifeSoaDataBos { get; set; }
        public IQueryable<PerLifeSoaDataBo> Query { get; set; }
        public override string PrefixFileName { get; set; } = "PerLifeSoa_Claims";
        public bool CategoryPendingClaim { get; set; } = false;

        public override List<Column> GetColumns()
        {
            Columns = PerLifeSoaDataBo.GetClaimColumns(CategoryPendingClaim);
            return Columns;
        }

        public override string GetColumnValue(Column col, object entity)
        {
            object v = entity.GetPropertyValue(col.Property);
            switch (col.Property)
            {
                case "ClaimStatus":
                    return v != null ? ClaimRegisterBo.GetStatusName(int.Parse(v.ToString())) : null;
                case "OffsetStatus":
                    return v != null ? ClaimRegisterBo.GetOffsetStatusName(int.Parse(v.ToString())) : null;
                default:
                    return Util.FormatExport(v);
            }
        }

        public override object GetEntity()
        {
            return PerLifeSoaDataBo;
        }

        public override IQueryable<object> GetQuery()
        {
            return Query;
        }

        public override IEnumerable<object> GetQueryNext()
        {
            if (Query == null)
                return new List<PerLifeSoaDataBo> { };
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
            PerLifeSoaDataBo = null;
            if (entity is PerLifeSoaDataBo e)
            {
                PerLifeSoaDataBo = e;
            }
        }

        public override void SetQuery(IQueryable<object> query)
        {
            Query = null;
            if (query is IQueryable<PerLifeSoaDataBo> q)
                Query = q;
        }
    }
}
