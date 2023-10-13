using BusinessObject;
using Shared.ProcessFile;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleApp.Commands.ProcessDatas.Exports
{
    public class ExportMfrs17ContractCode : ExportFile
    {
        public Mfrs17ContractCodeBo Mfrs17ContractCodeBo { get; set; }
        public IQueryable<Mfrs17ContractCodeBo> Query { get; set; }
        public override string PrefixFileName { get; set; } = "Mfrs17ContractCode";

        public override List<Column> GetColumns()
        {
            Columns = Mfrs17ContractCodeBo.GetColumns();
            return Columns;
        }

        public override object GetEntity()
        {
            return Mfrs17ContractCodeBo;
        }

        public override IQueryable<object> GetQuery()
        {
            return Query;
        }

        public override IEnumerable<object> GetQueryNext()
        {
            if (Query == null)
                return new List<Mfrs17ContractCodeBo> { };
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
            Mfrs17ContractCodeBo = null;
            if (entity is Mfrs17ContractCodeBo e)
                Mfrs17ContractCodeBo = e;
        }

        public override void SetQuery(IQueryable<object> query)
        {
            Query = null;
            if (query is IQueryable<Mfrs17ContractCodeBo> q)
                Query = q;
        }
    }
}
