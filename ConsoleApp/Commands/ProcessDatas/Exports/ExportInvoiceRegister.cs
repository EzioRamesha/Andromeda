using BusinessObject.InvoiceRegisters;
using BusinessObject.SoaDatas;
using Services;
using Services.Identity;
using Shared;
using Shared.ProcessFile;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleApp.Commands.ProcessDatas.Exports
{
    class ExportInvoiceRegister : ExportFile
    {
        public InvoiceRegisterBo InvoiceRegisterBo { get; set; }
        public IQueryable<InvoiceRegisterBo> Query { get; set; }
        public override string PrefixFileName { get; set; } = "InvoiceRegister";

        public override List<Column> GetColumns()
        {
            Columns = InvoiceRegisterBo.GetColumns();
            return Columns;
        }

        public override string GetColumnValue(Column col, object entity)
        {
            object v = entity.GetPropertyValue(col.Property);
            switch (col.Property)
            {
                case "Status":
                    return v != null ? SoaDataBatchBo.GetInvoiceStatusName(int.Parse(v.ToString())) : null;
                case "InvoiceType":
                    return InvoiceRegisterBo.GetInvoiceTypeName(int.Parse(v.ToString()));
                case "CedantId":
                    return v != null ? CedantService.Find(int.Parse(v.ToString()))?.Name : null;
                case "TreatyCodeId":
                    return v != null ? TreatyCodeService.Find(int.Parse(v.ToString()))?.Code : null;
                case "TreatyTypeId":
                case "LobId":
                    return v != null ? PickListDetailService.Find(int.Parse(v.ToString()))?.Code : null;
                case "PreparedById":
                    return v != null ? UserService.Find(int.Parse(v.ToString()))?.UserName : null;
                default:
                    return Util.FormatExport(v);
            }
        }

        public override object GetEntity()
        {
            return InvoiceRegisterBo;
        }

        public override IQueryable<object> GetQuery()
        {
            return Query;
        }

        public override IEnumerable<object> GetQueryNext()
        {
            if (Query == null)
                return new List<InvoiceRegisterBo> { };
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
            InvoiceRegisterBo = null;
            if (entity is InvoiceRegisterBo e)
                InvoiceRegisterBo = e;
        }

        public override void SetQuery(IQueryable<object> query)
        {
            Query = null;
            if (query is IQueryable<InvoiceRegisterBo> q)
                Query = q;
        }
    }
}
