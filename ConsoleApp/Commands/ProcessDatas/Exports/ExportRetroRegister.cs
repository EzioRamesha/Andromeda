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
    class ExportRetroRegister : ExportFile
    {
        public RetroRegisterBo RetroRegisterBo { get; set; }
        public IQueryable<RetroRegisterBo> Query { get; set; }
        public override string PrefixFileName { get; set; } = "RetroRegister";

        public override List<Column> GetColumns()
        {
            Columns = RetroRegisterBo.GetColumns();
            return Columns;
        }

        public override string GetColumnValue(Column col, object entity)
        {
            object v = entity.GetPropertyValue(col.Property);
            switch (col.Property)
            {
                case "RetroStatementType":
                    return RetroRegisterBo.GetRetroTypeName(int.Parse(v.ToString()));
                case "CedantId":
                    return v != null ? CedantService.Find(int.Parse(v.ToString()))?.Name : null;
                case "TreatyCodeId":
                    return v != null ? TreatyCodeService.Find(int.Parse(v.ToString()))?.Code : null;
                case "RetroPartyId":
                    return v != null ? RetroPartyService.Find(int.Parse(v.ToString()))?.Code : null;
                case "PreparedById":
                    return v != null ? UserService.Find(int.Parse(v.ToString()))?.UserName : null;
                default:
                    return Util.FormatExport(v);
            }
        }

        public override object GetEntity()
        {
            return RetroRegisterBo;
        }

        public override IQueryable<object> GetQuery()
        {
            return Query;
        }

        public override IEnumerable<object> GetQueryNext()
        {
            if (Query == null)
                return new List<RetroRegisterBo> { };
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
            RetroRegisterBo = null;
            if (entity is RetroRegisterBo e)
                RetroRegisterBo = e;
        }

        public override void SetQuery(IQueryable<object> query)
        {
            Query = null;
            if (query is IQueryable<RetroRegisterBo> q)
                Query = q;
        }
    }
}
