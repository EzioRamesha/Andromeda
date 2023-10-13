using BusinessObject.TreatyPricing;
using Services;
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
    public class ExportTreatyWorkflow : ExportFile
    {
        public TreatyPricingTreatyWorkflowBo TreatyPricingTreatyWorkflowBo { get; set; }
        public IQueryable<TreatyPricingTreatyWorkflowBo> Query { get; set; }
        public override string PrefixFileName { get; set; } = "TreatyPricingTreatyWorkflow";

        public override List<Column> GetColumns()
        {
            Columns = TreatyPricingTreatyWorkflowBo.GetColumns();
            return Columns;
        }

        public override string GetColumnValue(Column col, object entity)
        {
            object v = entity.GetPropertyValue(col.Property);
            switch (col.Property)
            {
                case "CoverageStatus":
                    if (v != null)
                    {
                        object coverageStatus = entity.GetPropertyValue("CoverageStatus");
                        return TreatyPricingTreatyWorkflowBo.GetCoverageStatusName(int.Parse(coverageStatus.ToString()));
                    }
                    else return "";
                case "DocumentStatus":
                    object documentStatus = entity.GetPropertyValue("DocumentStatus");
                    return TreatyPricingTreatyWorkflowBo.GetDocumentStatusName(int.Parse(documentStatus.ToString()));
                case "DraftingStatus":
                    object draftingStatus = entity.GetPropertyValue("DraftingStatus");
                    return TreatyPricingTreatyWorkflowBo.GetDraftingStatusName(int.Parse(draftingStatus.ToString()));
                case "DraftingStatusCategory":
                    object draftingStatusCategory = entity.GetPropertyValue("DraftingStatusCategory");
                    return TreatyPricingTreatyWorkflowBo.GetDraftingStatusCategoryName(int.Parse(draftingStatusCategory.ToString()));
                case "DocumentType":
                    object documentType = entity.GetPropertyValue("DocumentType");
                    return TreatyPricingTreatyWorkflowBo.GetDocumentTypeName(int.Parse(documentType.ToString()));
                case "ReinsuranceTypePickListDetailId":
                    return PickListDetailService.Find(int.Parse(v.ToString())).ToString();
                case "InwardRetroPartyDetailId":
                    if (v != null)
                    {
                        var inwardRetroPartyDetail = RetroPartyService.Find(int.Parse(v.ToString()));
                        return inwardRetroPartyDetail.Party + inwardRetroPartyDetail.Name;
                    }
                    else return "";
                case "CounterPartyDetailId":
                    if (v != null)
                    {
                        var counterPartyDetailId = CedantService.Find(int.Parse(v.ToString()));
                        return counterPartyDetailId.ToString();
                    }
                    else return "";
                default:
                    return Util.FormatExport(v);
            }
        }

        public override object GetEntity()
        {
            return TreatyPricingTreatyWorkflowBo;
        }

        public override IQueryable<object> GetQuery()
        {
            return Query;
        }

        public override IEnumerable<object> GetQueryNext()
        {
            if (Query == null)
                return new List<TreatyPricingTreatyWorkflowBo> { };
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
            TreatyPricingTreatyWorkflowBo = null;
            if (entity is TreatyPricingTreatyWorkflowBo e)
                TreatyPricingTreatyWorkflowBo = e;
        }

        public override void SetQuery(IQueryable<object> query)
        {
            Query = null;
            if (query is IQueryable<TreatyPricingTreatyWorkflowBo> qrd)
                Query = qrd;
        }
    }
}
