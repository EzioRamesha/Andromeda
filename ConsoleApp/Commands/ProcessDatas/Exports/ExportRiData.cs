using BusinessObject.RiDatas;
using Shared;
using Shared.ProcessFile;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleApp.Commands.ProcessDatas.Exports
{
    public class ExportRiData : ExportFile
    {
        public RiDataBo RiDataBo { get; set; }
        public IQueryable<RiDataBo> Query { get; set; }
        public override string PrefixFileName { get; set; } = "RiData";

        public override List<Column> GetColumns()
        {
            Columns = RiDataBo.GetColumns();
            return Columns;
        }

        public override string GetColumnValue(Column col, object entity)
        {
            object v = entity.GetPropertyValue(col.Property);
            switch (col.Property)
            {
                case "PreComputation1Status":
                    return RiDataBo.GetPreComputation1StatusName(int.Parse(v.ToString()));
                case "PreComputation2Status":
                    return RiDataBo.GetPreComputation2StatusName(int.Parse(v.ToString()));
                case "PreValidationStatus":
                    return RiDataBo.GetPreValidationStatusName(int.Parse(v.ToString()));
                case "ConflictType":
                    return RiDataBo.GetConflictTypeName(int.Parse(v.ToString()));
                case "PostComputationStatus":
                    return RiDataBo.GetPostComputationStatusName(int.Parse(v.ToString()));
                case "PostValidationStatus":
                    return RiDataBo.GetPostValidationStatusName(int.Parse(v.ToString()));
                case "MappingStatus":
                    return RiDataBo.GetMappingStatusName(int.Parse(v.ToString()));
                case "FinaliseStatus":
                    return RiDataBo.GetFinaliseStatusName(int.Parse(v.ToString()));
                case "RecordType":
                    return RiDataBo.GetRecordTypeName(int.Parse(v.ToString()));
                default:
                    return Util.FormatExport(v);
            }
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
