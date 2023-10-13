using BusinessObject.RiDatas;
using Shared.ProcessFile;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleApp.Commands.ProcessDatas.Exports
{
    public class ExportDirectRetroRiData : ExportFile
    {
        public RiDataBo RiDataBo { get; set; }
        public IQueryable<RiDataBo> Query { get; set; }
        public override string PrefixFileName { get; set; } = "DirectRetroRiData";

        public override List<Column> GetColumns()
        {
            Columns = RiDataBo.GetDirectRetroColumns();
            return Columns;
        }

        public override object GetEntity()
        {
            return RiDataBo;
        }

        public override IQueryable<object> GetQuery()
        {
            return Query;
        }

        public override IEnumerable<object> GetQueryNext()
        {
            if (Query == null)
                return new List<RiDataBo> { };
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
            RiDataBo = null;
            if (entity is RiDataBo e)
                RiDataBo = e;
        }

        public override void SetQuery(IQueryable<object> query)
        {
            Query = null;
            if (query is IQueryable<RiDataBo> q)
                Query = q;
        }
    }
}
