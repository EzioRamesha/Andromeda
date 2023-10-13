using BusinessObject.RiDatas;
using Services;
using Shared;
using Shared.ProcessFile;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleApp.Commands.ProcessDatas.Exports
{
    public class ExportRiDataWarehouse : ExportFile
    {
        public RiDataWarehouseBo RiDataWarehouseBo { get; set; }
        public IQueryable<RiDataWarehouseBo> Query { get; set; }
        public override string PrefixFileName { get; set; } = "RiDataWarehouse";
        public bool LinkReferral { get; set; } = false;

        public override List<Column> GetColumns()
        {
            Columns = RiDataWarehouseBo.GetColumns(LinkReferral);
            return Columns;
        }

        public override string GetColumnValue(Column col, object entity)
        {
            object v = entity.GetPropertyValue(col.Property);
            switch (col.Property)
            {
                case "RecordType":
                    return RiDataBatchBo.GetRecordTypeName(int.Parse(v != null ? v.ToString() : "0"));
                default:
                    return Util.FormatExport(v);
            }
        }

        public override object GetEntity()
        {
            return RiDataWarehouseBo;
        }

        public override IQueryable<object> GetQuery()
        {
            return Query;
        }

        public override IEnumerable<object> GetQueryNext()
        {
            if (Query == null)
                return new List<RiDataWarehouseBo> { };

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
            RiDataWarehouseBo = null;
            if (entity is RiDataWarehouseBo e)
                RiDataWarehouseBo = e;
        }

        public override void SetQuery(IQueryable<object> query)
        {
            Query = null;
            if (query is IQueryable<RiDataWarehouseBo> qrd)
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
