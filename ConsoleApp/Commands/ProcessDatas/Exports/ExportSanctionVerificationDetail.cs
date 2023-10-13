using BusinessObject;
using BusinessObject.Sanctions;
using Services;
using Shared;
using Shared.ProcessFile;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleApp.Commands.ProcessDatas.Exports
{
    public class ExportSanctionVerificationDetail : ExportFile
    {
        public SanctionVerificationDetailBo SanctionVerificationDetailBo { get; set; }
        public ModuleBo ModuleBo { get; set; }
        public IQueryable<SanctionVerificationDetailBo> Query { get; set; }
        public override string PrefixFileName { get; set; } = "SanctionVerificationMatchedRecord";

        public override List<Column> GetColumns()
        {
            Columns = SanctionVerificationDetailBo.GetColumns();
            return Columns;
        }

        public override string GetColumnValue(Column col, object entity)
        {
            object v = entity.GetPropertyValue(col.Property);
            switch (col.Type)
            {
                case SanctionVerificationDetailBo.TypeModuleId:
                    return ModuleBo?.Name;
                case SanctionVerificationDetailBo.TypeIsWhitelist:
                    bool bl = bool.Parse(v.ToString());
                    return bl ? "Yes" : "No";
                case SanctionVerificationDetailBo.TypePreviousDecision:
                    int? decision = v != null ? Util.GetParseInt(v.ToString()) : 0;
                    return SanctionVerificationDetailBo.GetPreviousDecisionName(decision ?? 0);
                default:
                    return Util.FormatExport(v);
            }
        }

        public override object GetEntity()
        {
            return SanctionVerificationDetailBo;
        }

        public override IQueryable<object> GetQuery()
        {
            return Query;
        }

        public override IEnumerable<object> GetQueryNext()
        {
            if (Query == null)
                return new List<SanctionVerificationDetailBo> { };
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
            SanctionVerificationDetailBo = null;
            if (entity is SanctionVerificationDetailBo e)
            {
                SanctionVerificationDetailBo = e;
                ModuleBo = ModuleService.Find(SanctionVerificationDetailBo.ModuleId);
            }
        }

        public override void SetQuery(IQueryable<object> query)
        {
            Query = null;
            if (query is IQueryable<SanctionVerificationDetailBo> q)
                Query = q;
        }
    }
}
