using BusinessObject.TreatyPricing;
using Services;
using Services.Identity;
using Services.TreatyPricing;
using Shared;
using Shared.ProcessFile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp.Commands.ProcessDatas.Exports
{
    public class ExportQuotationWorkflow : ExportFile
    {
        public TreatyPricingQuotationWorkflowBo TreatyPricingQuotationWorkflowBo { get; set; }
        public IQueryable<TreatyPricingQuotationWorkflowBo> Query { get; set; }
        public override string PrefixFileName { get; set; } = "TreatyPricingQuotationWorkflow";

        public override List<Column> GetColumns()
        {
            Columns = TreatyPricingQuotationWorkflowBo.GetColumns();
            return Columns;
        }

        public override string GetColumnValue(Column col, object entity)
        {
            object v = entity.GetPropertyValue(col.Property);
            switch (col.Property)
            {
                case "Status":
                    return TreatyPricingQuotationWorkflowBo.GetStatusName(int.Parse(v.ToString()));
                case "PricingStatus":
                    return TreatyPricingQuotationWorkflowBo.GetPricingStatusName(int.Parse(v.ToString()));
                case "CedantId":
                    return CedantService.Find(int.Parse(v.ToString())).Code;
                case "ReinsuranceTypePickListDetailId":
                    return PickListDetailService.Find(int.Parse(v.ToString())).Code;
                case "PricingTeamPickListDetailId":
                    return PickListDetailService.Find(int.Parse(v.ToString())).Code;
                case "BDPersonInChargeId":
                    return v != null ? UserService.Find(int.Parse(v.ToString()))?.FullName : null;
                case "PersonInChargeId":
                    return v != null ? UserService.Find(int.Parse(v.ToString()))?.FullName : null;
                default:
                    return Util.FormatExport(v);
            }
        }

        public override object GetEntity()
        {
            return TreatyPricingQuotationWorkflowBo;
        }

        public override IQueryable<object> GetQuery()
        {
            return Query;
        }

        public override IEnumerable<object> GetQueryNext()
        {
            if (Query == null)
                return new List<TreatyPricingQuotationWorkflowBo> { };
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
            TreatyPricingQuotationWorkflowBo = null;
            if (entity is TreatyPricingQuotationWorkflowBo e)
                TreatyPricingQuotationWorkflowBo = e;
        }

        public override void SetQuery(IQueryable<object> query)
        {
            Query = null;
            if (query is IQueryable<TreatyPricingQuotationWorkflowBo> qrd)
                Query = qrd;
        }
    }
}
