using BusinessObject.RiDatas;
using Services;
using Shared;
using Shared.ProcessFile;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleApp.Commands.ProcessDatas.Exports
{
    public class ExportRiDataWarehouseHistory : ExportFile
    {
        public RiDataWarehouseHistoryBo RiDataWarehouseHistoryBo { get; set; }
        public IQueryable<RiDataWarehouseHistoryBo> Query { get; set; }
        public override string PrefixFileName { get; set; } = "RiDataWarehouse";

        public override List<Column> GetColumns()
        {
            Columns = RiDataWarehouseHistoryBo.GetColumns();
            return Columns;
        }

        public override string GetColumnValue(Column col, object entity)
        {
            object v = entity.GetPropertyValue(col.Property);
            switch (col.Property)
            {
                case "RecordType":
                    return RiDataBatchBo.GetRecordTypeName(int.Parse(v != null ? v.ToString() : "0"));
                case "EndingPolicyStatus":
                    return PickListDetailService.Find(int.Parse(v != null ? v.ToString() : "0"))?.Code;
                default:
                    return Util.FormatExport(v);
            }
        }

        public override object GetEntity()
        {
            return RiDataWarehouseHistoryBo;
        }

        public override IQueryable<object> GetQuery()
        {
            return Query;
        }

        public override IEnumerable<object> GetQueryNext()
        {
            if (Query == null)
                return new List<RiDataWarehouseHistoryBo> { };

            if (!IsRangeQuery)
                return Query.OrderBy(q => q.Id).Skip(Skip).Take(Take).ToList();
            else
            {
                int maxId = MinId + Take;
                var query = Query.Where(q => q.Id >= MinId && q.Id < maxId).ToList();
                MinId = maxId;

                return query;
            }
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
            RiDataWarehouseHistoryBo = null;
            if (entity is RiDataWarehouseHistoryBo e)
                RiDataWarehouseHistoryBo = e;
        }

        public override void SetQuery(IQueryable<object> query)
        {
            Query = null;
            if (query is IQueryable<RiDataWarehouseHistoryBo> qrd)
                Query = qrd;
        }

        public override void Init()
        {
            if (Query == null)
                return;

            if (Total < Take)
                IsRangeQuery = false;
            else
            {
                IsRangeQuery = true;
                MinId = Query.Min(q => q.Id);
            }
        }
    }
}
