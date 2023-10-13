using BusinessObject;
using Services;
using Shared;
using Shared.ProcessFile;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleApp.Commands.ProcessDatas.Exports
{
    public class ExportPickList : ExportFile
    {
        public PickListBo PickListBo { get; set; }
        public IList<PickListDetailBo> PickListDetailBos { get; set; }
        public PickListDetailBo PickListDetailBo { get; set; }
        public IQueryable<PickListBo> Query { get; set; }
        public override string PrefixFileName { get; set; } = "PickList";

        public override List<Column> GetColumns()
        {
            Columns = PickListBo.GetColumns();
            return Columns;
        }

        public override string GetColumnValue(Column col, object entity)
        {
            object v = entity.GetPropertyValue(col.Property);
            switch (col.ColIndex)
            {
                case PickListBo.ColumnDepartmentCode:
                    return PickListDetailBo.PickListBo.DepartmentBo.Code;
                case PickListBo.ColumnFieldName:
                    return PickListDetailBo.PickListBo.FieldName;
                default:
                    return Util.FormatExport(v);
            }
        }

        public override object GetEntity()
        {
            return PickListDetailBo;
        }

        public override IQueryable<object> GetQuery()
        {
            return Query;
        }

        public override IEnumerable<object> GetQueryNext()
        {
            if (Query == null)
                return new List<PickListBo> { };
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
            PickListBo = null;
            PickListDetailBos = new List<PickListDetailBo> { };
            if (entity is PickListBo pickListBo)
            {
                PickListBo = pickListBo;
                PickListDetailBos = PickListDetailService.GetByPickListId(pickListBo.Id);
            }
        }

        public void SetEntityDetail(object entity)
        {
            PickListDetailBo = null;
            if (entity is PickListDetailBo pickListDetailBo)
            {
                PickListDetailBo = pickListDetailBo;
            }
        }

        public override void SetQuery(IQueryable<object> query)
        {
            Query = null;
            if (query is IQueryable<PickListBo> q)
                Query = q;
        }

        public override void ProcessNext()
        {
            var list = GetQueryNext();
            if (list == null)
                return;

            foreach (var entity in list)
            {
                SetEntity(entity);
                foreach (var entityDetail in PickListDetailBos)
                {
                    SetEntityDetail(entityDetail);
                    WriteDataLine();
                    Processed++;
                }
            }
        }
    }
}
