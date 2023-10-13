using BusinessObject;
using Shared.ProcessFile;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleApp.Commands.ProcessDatas.Exports
{
    public class ExportMfrs17CellMapping : ExportFile
    {
        public Mfrs17CellMappingBo Mfrs17CellMappingBo { get; set; }
        public IQueryable<Mfrs17CellMappingBo> Query { get; set; }
        public override string PrefixFileName { get; set; } = "Mfrs17CellMapping";

        public override List<Column> GetColumns()
        {
            Columns = Mfrs17CellMappingBo.GetColumns();
            return Columns;
        }

        public override object GetEntity()
        {
            return Mfrs17CellMappingBo;
        }

        public override IQueryable<object> GetQuery()
        {
            return Query;
        }

        public override IEnumerable<object> GetQueryNext()
        {
            if (Query == null)
                return new List<Mfrs17CellMappingBo> { };
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
            Mfrs17CellMappingBo = null;
            if (entity is Mfrs17CellMappingBo e)
                Mfrs17CellMappingBo = e;
        }

        public override void SetQuery(IQueryable<object> query)
        {
            Query = null;
            if (query is IQueryable<Mfrs17CellMappingBo> q)
                Query = q;
        }
    }
}
