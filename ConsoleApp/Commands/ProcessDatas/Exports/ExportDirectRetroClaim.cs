using BusinessObject;
using Shared;
using Shared.ProcessFile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp.Commands.ProcessDatas.Exports
{
    public class ExportDirectRetroClaim : ExportFile
    {
        public ClaimRegisterBo ClaimRegisterBo { get; set; }
        public IQueryable<ClaimRegisterBo> Query { get; set; }
        public override string PrefixFileName { get; set; } = "DirectRetroClaims";

        public override List<Column> GetColumns()
        {
            Columns = ClaimRegisterBo.GetDirectRetroColumns();
            return Columns;
        }

        public override string GetColumnValue(Column col, object entity)
        {
            object v = entity.GetPropertyValue(col.Property);
            switch (col.Property)
            {
                case "RetroRecovery1":
                case "RetroRecovery2":
                case "RetroRecovery3":
                    return v != null ? Util.DoubleToString(v, 2) : null;
                default:
                    return Util.FormatExport(v);
            }
        }

        public override object GetEntity()
        {
            return ClaimRegisterBo;
        }

        public override IQueryable<object> GetQuery()
        {
            return Query;
        }

        public override IEnumerable<object> GetQueryNext()
        {
            if (Query == null)
                return new List<ClaimRegisterBo> { };
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
            ClaimRegisterBo = null;
            if (entity is ClaimRegisterBo e)
                ClaimRegisterBo = e;
        }

        public override void SetQuery(IQueryable<object> query)
        {
            Query = null;
            if (query is IQueryable<ClaimRegisterBo> q)
                Query = q;
        }
    }
}
