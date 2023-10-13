using BusinessObject.RiDatas;
using Shared.ProcessFile;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleApp.Commands.ProcessDatas.Exports
{
    public class ExportRiDataCorrection : ExportFile
    {
        public RiDataCorrectionBo RiDataCorrectionBo { get; set; }
        public IQueryable<RiDataCorrectionBo> Query { get; set; }
        public override string PrefixFileName { get; set; } = "RiDataCorrection";

        public override List<Column> GetColumns()
        {
            Columns = RiDataCorrectionBo.GetColumns();
            return Columns;
        }

        public override object GetEntity()
        {
            return RiDataCorrectionBo;
        }

        public override IQueryable<object> GetQuery()
        {
            return Query;
        }

        public override IEnumerable<object> GetQueryNext()
        {
            if (Query == null)
                return new List<RiDataCorrectionBo> { };
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
            RiDataCorrectionBo = null;
            if (entity is RiDataCorrectionBo e)
                RiDataCorrectionBo = e;
        }

        public override void SetQuery(IQueryable<object> query)
        {
            Query = null;
            if (query is IQueryable<RiDataCorrectionBo> q)
                Query = q;
        }
    }
}
