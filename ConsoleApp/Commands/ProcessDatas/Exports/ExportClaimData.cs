using BusinessObject.Claims;
using Shared;
using Shared.ProcessFile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp.Commands.ProcessDatas.Exports
{
    class ExportClaimData : ExportFile
    {
        public ClaimDataBo ClaimDataBo { get; set; }
        public IQueryable<ClaimDataBo> Query { get; set; }
        public override string PrefixFileName { get; set; } = "ClaimData";

        public override List<Column> GetColumns()
        {
            Columns = ClaimDataBo.GetColumns();
            return Columns;
        }

        public override string GetColumnValue(Column col, object entity)
        {
            object v = entity.GetPropertyValue(col.Property);
            switch (col.Property)
            {
                case "MappingStatus":
                    return ClaimDataBo.GetMappingStatusName(int.Parse(v.ToString()));
                case "PreComputationStatus":
                    return ClaimDataBo.GetPreComputationStatusName(int.Parse(v.ToString()));
                case "PreValidationStatus":
                    return ClaimDataBo.GetPreValidationStatusName(int.Parse(v.ToString()));
                default:
                    return Util.FormatExport(v);
            }
        }

        public override object GetEntity()
        {
            return ClaimDataBo;
        }

        public override IQueryable<object> GetQuery()
        {
            return Query;
        }

        public override IEnumerable<object> GetQueryNext()
        {
            if (Query == null)
                return new List<ClaimDataBo> { };
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
            ClaimDataBo = null;
            if (entity is ClaimDataBo e)
                ClaimDataBo = e;
        }

        public override void SetQuery(IQueryable<object> query)
        {
            Query = null;
            if (query is IQueryable<ClaimDataBo> q)
                Query = q;
        }
    }
}
