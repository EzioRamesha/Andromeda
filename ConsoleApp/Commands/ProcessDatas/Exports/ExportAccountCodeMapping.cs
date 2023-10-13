using BusinessObject;
using Shared;
using Shared.ProcessFile;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleApp.Commands.ProcessDatas.Exports
{
    public class ExportAccountCodeMapping : ExportFile
    {
        public AccountCodeMappingBo AccountCodeMappingBo { get; set; }
        public IQueryable<AccountCodeMappingBo> Query { get; set; }
        public bool IsRetro { get; set; } = false;
        public override string PrefixFileName { get; set; } = "AccountCodeMapping";

        public override List<Column> GetColumns()
        {
            if (IsRetro)
                Columns = AccountCodeMappingBo.GetRetroColumns();
            else
                Columns = AccountCodeMappingBo.GetColumns();

            return Columns;
        }

        public override string GetColumnValue(Column col, object entity)
        {
            object v = entity.GetPropertyValue(col.Property);
            switch (col.Property)
            {
                case "ReportTypeName":
                    object reportType = entity.GetPropertyValue("ReportType");
                    return AccountCodeMappingBo.GetReportTypeName(int.Parse(reportType.ToString()));
                case "TypeName":
                    object type = entity.GetPropertyValue("Type");
                    return AccountCodeMappingBo.GetTypeName(int.Parse(type.ToString()));
                case "DebitCreditIndicatorPositive":
                    object indicatorPositive = entity.GetPropertyValue("DebitCreditIndicatorPositive");
                    if (indicatorPositive != null)
                        return AccountCodeMappingBo.GetDebitCreditIndicatorName(int.Parse(indicatorPositive.ToString()));
                    else
                        return null;
                case "DebitCreditIndicatorNegative":
                    object indicatorNegative = entity.GetPropertyValue("DebitCreditIndicatorNegative");
                    if (indicatorNegative != null)
                        return AccountCodeMappingBo.GetDebitCreditIndicatorName(int.Parse(indicatorNegative.ToString()));
                    else
                        return null;
                default:
                    return Util.FormatExport(v);
            }
        }

        public override object GetEntity()
        {
            return AccountCodeMappingBo;
        }

        public override IQueryable<object> GetQuery()
        {
            return Query;
        }

        public override IEnumerable<object> GetQueryNext()
        {
            if (Query == null)
                return new List<AccountCodeMappingBo> { };
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
            AccountCodeMappingBo = null;
            if (entity is AccountCodeMappingBo e)
                AccountCodeMappingBo = e;
        }

        public override void SetQuery(IQueryable<object> query)
        {
            Query = null;
            if (query is IQueryable<AccountCodeMappingBo> q)
                Query = q;
        }
    }
}
