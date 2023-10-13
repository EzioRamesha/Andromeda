using BusinessObject.Retrocession;
using BusinessObject.RiDatas;
using Services.Retrocession;
using Shared;
using Shared.ProcessFile;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleApp.Commands.ProcessDatas.Exports
{
    public class ExportPerLifeAggregationMonthlyData : ExportFile
    {
        public PerLifeAggregationMonthlyDataBo PerLifeAggregationMonthlyDataBo { get; set; }
        public IQueryable<PerLifeAggregationMonthlyDataBo> Query { get; set; }
        public bool IsRetroDetail { get; set; } = false;
        public bool IsRetroRiData { get; set; } = false;
        public bool IsRetentionPremium { get; set; } = false;
        public List<string> RetroParties { get; set; }
        public override string PrefixFileName { get; set; } = "PerLifeAggregationMonthlyData";

        public override List<Column> GetColumns()
        {
            Columns = PerLifeAggregationMonthlyDataBo.GetColumns();

            if (IsRetroRiData)
                Columns = PerLifeAggregationMonthlyDataBo.GetRetroRiDataColumns();

            if (IsRetroDetail)
                Columns = PerLifeAggregationMonthlyDataBo.GetColumnsWithStandardRetroOutput();

            if (IsRetentionPremium)
                Columns = PerLifeAggregationMonthlyDataBo.GetRetentionPremiumColumns(RetroParties);

            return Columns;
        }

        public override string GetColumnValue(Column col, object entity)
        {
            object v = entity.GetPropertyValue(col.Property);

            if (col.Property.Contains("_GrossPremium"))
            {
                var arrProperty = Util.ToArraySplitTrim(col.Property, '_');
                var value = PerLifeAggregationMonthlyDataBo.PerLifeAggregationMonthlyRetroDataBos.Where(q => q.RetroParty == arrProperty[0]).Select(q => q.RetroGrossPremium).FirstOrDefault();
                return Util.DoubleToString(value);
            }
            else if (col.Property.Contains("_NetPremium"))
            {
                var arrProperty = Util.ToArraySplitTrim(col.Property, '_');
                var value = PerLifeAggregationMonthlyDataBo.PerLifeAggregationMonthlyRetroDataBos.Where(q => q.RetroParty == arrProperty[0]).Select(q => q.RetroNetPremium).FirstOrDefault();
                return Util.DoubleToString(value);
            }
            else if (col.Property.Contains("_RetroDiscount"))
            {
                var arrProperty = Util.ToArraySplitTrim(col.Property, '_');
                var value = PerLifeAggregationMonthlyDataBo.PerLifeAggregationMonthlyRetroDataBos.Where(q => q.RetroParty == arrProperty[0]).Select(q => q.RetroDiscount).FirstOrDefault();
                return Util.DoubleToString(value);
            }
            else
            {
                switch (col.Property)
                {
                    case "RetroIndicator":
                        if (v != null && bool.TryParse(v.ToString(), out bool b))
                        {
                            return Util.BoolToString(b);
                        }
                        return "";
                    default:
                        return Util.FormatExport(v);
                }
            }
        }

        public override object GetEntity()
        {
            return PerLifeAggregationMonthlyDataBo;
        }

        public override IQueryable<object> GetQuery()
        {
            return Query;
        }

        public override IEnumerable<object> GetQueryNext()
        {
            if (Query == null)
                return new List<PerLifeAggregationMonthlyDataBo> { };
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
            PerLifeAggregationMonthlyDataBo = null;
            if (entity is PerLifeAggregationMonthlyDataBo e)
                PerLifeAggregationMonthlyDataBo = e;

            if (PerLifeAggregationMonthlyDataBo != null && IsRetentionPremium)
                PerLifeAggregationMonthlyDataBo.PerLifeAggregationMonthlyRetroDataBos = PerLifeAggregationMonthlyRetroDataService.GetByPerLifeAggregationMonthlyDataId(PerLifeAggregationMonthlyDataBo.Id).ToList();
        }

        public override void SetQuery(IQueryable<object> query)
        {
            Query = null;
            if (query is IQueryable<PerLifeAggregationMonthlyDataBo> q)
                Query = q;
        }
    }
}
