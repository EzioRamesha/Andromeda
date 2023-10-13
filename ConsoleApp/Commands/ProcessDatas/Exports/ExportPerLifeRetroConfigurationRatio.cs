using BusinessObject.Retrocession;
using Shared;
using Shared.ProcessFile;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleApp.Commands.ProcessDatas.Exports
{
    public class ExportPerLifeRetroConfigurationRatio : ExportFile
    {
        public PerLifeRetroConfigurationRatioBo PerLifeRetroConfigurationRatioBo { get; set; }
        public IQueryable<PerLifeRetroConfigurationRatioBo> Query { get; set; }
        public override string PrefixFileName { get; set; } = "PerLifeRetroConfigurationRatio";

        public override List<Column> GetColumns()
        {
            Columns = PerLifeRetroConfigurationRatioBo.GetColumns();
            return Columns;
        }

        public override object GetEntity()
        {
            return PerLifeRetroConfigurationRatioBo;
        }

        public override IQueryable<object> GetQuery()
        {
            return Query;
        }

        public override IEnumerable<object> GetQueryNext()
        {
            if (Query == null)
                return new List<PerLifeRetroConfigurationRatioBo> { };
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
            PerLifeRetroConfigurationRatioBo = null;
            if (entity is PerLifeRetroConfigurationRatioBo e)
                PerLifeRetroConfigurationRatioBo = e;
        }

        public override void SetQuery(IQueryable<object> query)
        {
            Query = null;
            if (query is IQueryable<PerLifeRetroConfigurationRatioBo> q)
                Query = q;
        }
    }
}
