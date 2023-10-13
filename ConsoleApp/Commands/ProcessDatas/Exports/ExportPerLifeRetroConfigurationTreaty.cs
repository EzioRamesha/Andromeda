using BusinessObject.Retrocession;
using Shared;
using Shared.ProcessFile;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleApp.Commands.ProcessDatas.Exports
{
    public class ExportPerLifeRetroConfigurationTreaty : ExportFile
    {
        public PerLifeRetroConfigurationTreatyBo PerLifeRetroConfigurationTreatyBo { get; set; }
        public IQueryable<PerLifeRetroConfigurationTreatyBo> Query { get; set; }
        public override string PrefixFileName { get; set; } = "PerLifeRetroConfigurationTreaty";

        public override List<Column> GetColumns()
        {
            Columns = PerLifeRetroConfigurationTreatyBo.GetColumns();
            return Columns;
        }

        public override string GetColumnValue(Column col, object entity)
        {
            object v = entity.GetPropertyValue(col.Property);

            switch (col.Property)
            {
                case "IsToAggregate":
                    if (v != null && bool.TryParse(v.ToString(), out bool b))
                    {
                        return Util.BoolToString(b);
                    }
                    return "";
                default:
                    return Util.FormatExport(v);
            }
        }

        public override object GetEntity()
        {
            return PerLifeRetroConfigurationTreatyBo;
        }

        public override IQueryable<object> GetQuery()
        {
            return Query;
        }

        public override IEnumerable<object> GetQueryNext()
        {
            if (Query == null)
                return new List<PerLifeRetroConfigurationTreatyBo> { };
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
            PerLifeRetroConfigurationTreatyBo = null;
            if (entity is PerLifeRetroConfigurationTreatyBo e)
                PerLifeRetroConfigurationTreatyBo = e;
        }

        public override void SetQuery(IQueryable<object> query)
        {
            Query = null;
            if (query is IQueryable<PerLifeRetroConfigurationTreatyBo> q)
                Query = q;
        }
    }
}
