using BusinessObject.Retrocession;
using BusinessObject.SoaDatas;
using Services;
using Services.Identity;
using Shared;
using Shared.ProcessFile;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleApp.Commands.ProcessDatas.Exports
{
    public class ExportValidDuplicationList : ExportFile
    {
        public ValidDuplicationListBo ValidDuplicationListBo { get; set; }
        public IQueryable<ValidDuplicationListBo> Query { get; set; }
        public override string PrefixFileName { get; set; } = "ValidDuplicationList";

        public override List<Column> GetColumns()
        {
            Columns = ValidDuplicationListBo.GetColumns();
            return Columns;
        }

        public override string GetColumnValue(Column col, object entity)
        {
            object v = entity.GetPropertyValue(col.Property);
            switch (col.Property)
            {
                default:
                    return Util.FormatExport(v);
            }
        }

        public override object GetEntity()
        {

            return ValidDuplicationListBo;

        }

        public override IQueryable<object> GetQuery()
        {
            return Query;
        }

        public override IEnumerable<object> GetQueryNext()
        {
            if (Query == null)
                return new List<ValidDuplicationListBo> { };
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
            ValidDuplicationListBo = null;
            if (entity is ValidDuplicationListBo e)
                ValidDuplicationListBo = e;
        }

        public override void SetQuery(IQueryable<object> query)
        {
            Query = null;
            if (query is IQueryable<ValidDuplicationListBo> q)
                Query = q;
        }
    }
}
