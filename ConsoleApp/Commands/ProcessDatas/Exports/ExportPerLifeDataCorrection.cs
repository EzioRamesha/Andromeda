using BusinessObject.Retrocession;
using Shared;
using Shared.ProcessFile;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleApp.Commands.ProcessDatas.Exports
{
    public class ExportPerLifeDataCorrection : ExportFile
    {
        public PerLifeDataCorrectionBo PerLifeDataCorrectionBo { get; set; }
        public IQueryable<PerLifeDataCorrectionBo> Query { get; set; }
        public bool IsRetro { get; set; } = false;
        public override string PrefixFileName { get; set; } = "PerLifeDataCorrection";

        public override List<Column> GetColumns()
        {
            Columns = PerLifeDataCorrectionBo.GetColumns();
            return Columns;
        }

        public override string GetColumnValue(Column col, object entity)
        {
            object v = entity.GetPropertyValue(col.Property);
            switch (col.Type)
            {
                case PerLifeDataCorrectionBo.ColumnIsProceedToAggregate:
                    if (bool.TryParse(v.ToString(), out bool b))
                        return Util.BoolToString(b);
                    return "";
                default:
                    return Util.FormatExport(v);
            }
        }

        public override object GetEntity()
        {
            return PerLifeDataCorrectionBo;
        }

        public override IQueryable<object> GetQuery()
        {
            return Query;
        }

        public override IEnumerable<object> GetQueryNext()
        {
            if (Query == null)
                return new List<PerLifeDataCorrectionBo> { };
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
            PerLifeDataCorrectionBo = null;
            if (entity is PerLifeDataCorrectionBo e)
                PerLifeDataCorrectionBo = e;
        }

        public override void SetQuery(IQueryable<object> query)
        {
            Query = null;
            if (query is IQueryable<PerLifeDataCorrectionBo> q)
                Query = q;
        }
    }
}
