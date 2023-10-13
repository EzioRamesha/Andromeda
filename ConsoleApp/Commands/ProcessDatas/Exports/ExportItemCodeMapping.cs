using BusinessObject;
using Shared;
using Shared.ProcessFile;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleApp.Commands.ProcessDatas.Exports
{
    public class ExportItemCodeMapping : ExportFile
    {
        public ItemCodeMappingBo ItemCodeMappingBo { get; set; }
        public IQueryable<ItemCodeMappingBo> Query { get; set; }
        public override string PrefixFileName { get; set; } = "ItemCodeMapping";

        public override List<Column> GetColumns()
        {
            Columns = ItemCodeMappingBo.GetColumns();
            return Columns;
        }

        public override string GetColumnValue(Column col, object entity)
        {
            object v = entity.GetPropertyValue(col.Property);
            switch (col.ColIndex)
            {
                case ItemCodeMappingBo.ColumnReportingType:
                    return ItemCodeBo.GetReportingTypeName(int.Parse(v.ToString()));
                default:
                    return Util.FormatExport(v);
            }
        }

        public override object GetEntity()
        {
            return ItemCodeMappingBo;
        }

        public override IQueryable<object> GetQuery()
        {
            return Query;
        }

        public override IEnumerable<object> GetQueryNext()
        {
            if (Query == null)
                return new List<ItemCodeMappingBo> { };
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
            ItemCodeMappingBo = null;
            if (entity is ItemCodeMappingBo e)
                ItemCodeMappingBo = e;
        }

        public override void SetQuery(IQueryable<object> query)
        {
            Query = null;
            if (query is IQueryable<ItemCodeMappingBo> q)
                Query = q;
        }
    }
}
