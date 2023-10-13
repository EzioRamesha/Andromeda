using BusinessObject;
using BusinessObject.RiDatas;
using BusinessObject.SoaDatas;
using Shared.ProcessFile;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleApp.Commands.ProcessDatas.Exports
{
    public class ExportSoaData : ExportFile
    {
        public SoaDataBo SoaDataBo { get; set; }
        public IQueryable<SoaDataBo> Query { get; set; }
        public override string PrefixFileName { get; set; } = "SoaData";

        public override bool IsTextFile { get; set; } = false;

        public override List<Column> GetColumns()
        {
            Columns = SoaDataBo.GetColumns();
            return Columns;
        }

        public override object GetEntity()
        {
            return SoaDataBo;
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
            SoaDataBo = null;
            if (entity is SoaDataBo e)
                SoaDataBo = e;
        }

        public override void SetQuery(IQueryable<object> query)
        {
            Query = null;
            if (query is IQueryable<SoaDataBo> q)
                Query = q;
        }

        public override void WriteDataLine()
        {
            if (Excel == null)
                return;

            object entity = GetEntity();
            if (entity == null)
                return;

            int colIndex;
            int index = 1;
            foreach (var col in Columns)
            {
                colIndex = index;
                if (col.ColIndex.HasValue)
                    colIndex = col.ColIndex.Value;

                Excel.WriteCell(RowIndex, colIndex, GetColumnValue(col, entity));

                index++;
            }

            RowIndex++;
        }

        public void WriteTemplateDataLineFormat()
        {
            int colIndex;
            int index = 1;
            foreach (var col in Columns)
            {
                colIndex = index;
                if (col.ColIndex.HasValue)
                    colIndex = col.ColIndex.Value;

                switch (col.Property)
                {
                    case "SoaQuarter":
                    case "RiskQuarter":
                        Excel.SetQuarterValidation(RowIndex, colIndex, col.Property);
                        break;
                    default:

                        int? soType = StandardSoaDataOutputBo.GetTypeByProperty(col.Property);
                        if (soType != null)
                        {
                            int soDataType = StandardSoaDataOutputBo.GetDataTypeByType(soType.Value);
                            switch (soDataType)
                            {
                                case StandardOutputBo.DataTypeDate:
                                    Excel.SetNumberFormat(RowIndex, colIndex, Excel.FormatDate);
                                    break;
                                case StandardOutputBo.DataTypeAmount:
                                case StandardOutputBo.DataTypePercentage:
                                    Excel.SetNumberFormat(RowIndex, colIndex, Excel.FormatAmount);
                                    Excel.SetAmountValidation(RowIndex, colIndex);
                                    break;
                                case StandardOutputBo.DataTypeInteger:
                                    Excel.SetNumberFormat(RowIndex, colIndex, Excel.FormatInteger);
                                    Excel.SetIntegerValidation(RowIndex, colIndex);
                                    break;
                                default:
                                    Excel.SetNumberFormat(RowIndex, colIndex, Excel.FormatGeneral);
                                    break;
                            }
                        }
                        break;
                }
                index++;
            }

            RowIndex++;
        }
    }
}
