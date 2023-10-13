using BusinessObject.Retrocession;
using BusinessObject.RiDatas;
using Shared;
using Shared.ProcessFile;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleApp.Commands.ProcessDatas.Exports
{
    public class ExportPerLifeDuplicationCheck : ExportFile
    {
        public PerLifeDuplicationCheckBo PerLifeDuplicationCheckBo { get; set; }
        public IQueryable<PerLifeDuplicationCheckBo> Query { get; set; }
        public override string PrefixFileName { get; set; } = "PerLifeDuplicationCheck";

        public override List<Column> GetColumns()
        {
            Columns = PerLifeDuplicationCheckBo.GetColumns();
            return Columns;
        }

        public override string GetColumnValue(Column col, object entity)
        {
            object v = entity.GetPropertyValue(col.Property);
            switch (col.Property)
            {
                case "Inclusion":
                case "EnableReinsuranceBasisCodeCheck":
                    if (bool.TryParse(v.ToString(), out bool b))
                        return Util.BoolToString(b);
                    return "";
                default:
                    return Util.FormatExport(v);
            }
        }

        public override object GetEntity()
        {
            return PerLifeDuplicationCheckBo;
        }

        public override IQueryable<object> GetQuery()
        {
            return Query;
        }

        public override IEnumerable<object> GetQueryNext()
        {
            if (Query == null)
                return new List<PerLifeDuplicationCheckBo> { };
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
            PerLifeDuplicationCheckBo = null;
            if (entity is PerLifeDuplicationCheckBo e)
                PerLifeDuplicationCheckBo = e;
        }

        public override void SetQuery(IQueryable<object> query)
        {
            Query = null;
            if (query is IQueryable<PerLifeDuplicationCheckBo> q)
                Query = q;
        }
    }
}
