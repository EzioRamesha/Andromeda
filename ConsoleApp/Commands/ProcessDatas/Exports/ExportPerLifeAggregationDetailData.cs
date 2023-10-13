using BusinessObject.Retrocession;
using BusinessObject.RiDatas;
using Shared;
using Shared.ProcessFile;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleApp.Commands.ProcessDatas.Exports
{
    public class ExportPerLifeAggregationDetailData : ExportFile
    {
        public PerLifeAggregationDetailDataBo PerLifeAggregationDetailDataBo { get; set; }
        public bool IsException { get; set; } = false;
        public bool IsExcludedRecord { get; set; } = false;
        public IQueryable<PerLifeAggregationDetailDataBo> Query { get; set; }
        public override string PrefixFileName { get; set; } = "PerLifeAggregationDetailData";

        public override List<Column> GetColumns()
        {
            // Default
            Columns = PerLifeAggregationDetailDataBo.GetColumns();

            if (IsException)
                Columns = PerLifeAggregationDetailDataBo.GetExceptionColumns();

            if (IsExcludedRecord)
                Columns = PerLifeAggregationDetailDataBo.GetExcludedRecordColumns();

            return Columns;
        }

        public override string GetColumnValue(Column col, object entity)
        {   
            object v = entity.GetPropertyValue(col.Property);

            if (IsException || IsExcludedRecord)
            {
                switch (col.Property)
                {
                    case "RecordType":
                        return RiDataBo.GetRecordTypeName(int.Parse(v.ToString()));
                    case "ExceptionType":
                        var vStr = v != null ? v.ToString() : "";
                        return PerLifeAggregationDetailDataBo.GetExceptionTypeName(Util.GetParseInt(vStr));
                    case "ProceedStatus":
                        return PerLifeAggregationDetailDataBo.GetProceedStatusName(Util.GetParseInt(v.ToString()));
                    default:
                        return Util.FormatExport(v);
                }
            }

            return Util.FormatExport(v);
        }

        public override object GetEntity()
        {
            return PerLifeAggregationDetailDataBo;
        }

        public override IQueryable<object> GetQuery()
        {
            return Query;
        }

        public override IEnumerable<object> GetQueryNext()
        {
            if (Query == null)
                return new List<PerLifeAggregationDetailDataBo> { };
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
            PerLifeAggregationDetailDataBo = null;
            if (entity is PerLifeAggregationDetailDataBo e)
                PerLifeAggregationDetailDataBo = e;
        }

        public override void SetQuery(IQueryable<object> query)
        {
            Query = null;
            if (query is IQueryable<PerLifeAggregationDetailDataBo> q)
                Query = q;
        }
    }
}
