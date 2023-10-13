using BusinessObject;
using BusinessObject.RiDatas;
using BusinessObject.SoaDatas;
using Services.RiDatas;
using Shared;
using Shared.ProcessFile;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;
using Row = Shared.ProcessFile.Row;

namespace ConsoleApp.Commands.RawFiles.SoaData
{
    public class MappingSoaData : Command
    {
        public const int ROW_TYPE_HEADER = 1;
        public const int ROW_TYPE_DATA = 2;

        public ProcessSoaDataBatch ProcessSoaDataBatch { get; set; }
        public LogSoaDataFile LogSoaDataFile { get; set; }
        public List<SoaDataBo> SoaDataBos { get; set; }

        public Row Row { get; set; }

        public MappingSoaData(ProcessSoaDataBatch batch)
        {
            ProcessSoaDataBatch = batch;
        }

        public MappingSoaData(ProcessSoaDataBatch batch, Row row) : this(batch)
        {
            Row = row;
        }

        public void MappingRow()
        {
            LogSoaDataFile = new LogSoaDataFile(ProcessSoaDataBatch.SoaDataFileBo);

            int rowIndex = Row.RowIndex;
            int rowType = ROW_TYPE_DATA;
            if (rowIndex == 1)
                rowType = ROW_TYPE_HEADER;

            SoaDataBos = new List<SoaDataBo>
            {
                new SoaDataBo()
            };

            foreach (Column col in Row.Columns)
            {
                switch (rowType)
                {
                    case ROW_TYPE_HEADER:
                        break;
                    case ROW_TYPE_DATA:
                        ProcessData(col);
                        break;
                }
            }
        }

        public bool ProcessData(Column col)
        {
            //LogRiDataFile.SwMappingDetail.Start();

            int? colIndex = col.ColIndex;
            object value = col.Value;
            object value2 = col.Value2;

            var mappingCols = ProcessSoaDataBatch.Columns.Where(q => q.ColIndex == colIndex).ToList();
            if (!mappingCols.IsNullOrEmpty())
            {
                foreach (var mappingCol in mappingCols)
                {
                    foreach (SoaDataBo soaData in SoaDataBos)
                    {
                        bool success = soaData.ProcessSoaData(mappingCol, value);
                        if (success)
                            ValidateData(mappingCol, soaData);
                        else
                            LogSoaDataFile.MappingErrorCount++;
                    }
                }
            }

            //LogRiDataFile.SwMappingDetail.Stop();
            return true;
        }

        public void ValidateData(Column mappingCol, SoaDataBo soaData)
        {
            //string code = StandardSoaDataOutputBo.GetCodeByType(mappingCol.Type);
            bool success = true;
            string message = "";
            string value = "";
            switch (mappingCol.Type)
            {
                case StandardSoaDataOutputBo.TypeTreatyId:
                    if (string.IsNullOrEmpty(soaData.TreatyId))
                    {
                        success = false;
                        break;
                    }

                    if (ProcessSoaDataBatch.CacheService.TreatyIdCodes.IsNullOrEmpty())
                        success = false;
                    else if (ProcessSoaDataBatch.CacheService.TreatyIdCodes.Where(q => q == soaData.TreatyId).ToList().IsNullOrEmpty())
                        success = false;

                    value = soaData.TreatyId;
                    break;
                case StandardSoaDataOutputBo.TypeTreatyCode:
                    if (string.IsNullOrEmpty(soaData.TreatyCode))
                    {
                        success = false;
                        break;
                    }

                    if (ProcessSoaDataBatch.CacheService.TreatyCodes.IsNullOrEmpty())
                        success = false;
                    else if (ProcessSoaDataBatch.CacheService.TreatyCodes.Where(q => q == soaData.TreatyCode).ToList().IsNullOrEmpty())
                        success = false;

                    value = soaData.TreatyCode;
                    break;
                case StandardSoaDataOutputBo.TypeTreatyMode:
                    if (string.IsNullOrEmpty(soaData.TreatyMode))
                    {
                        success = false;
                        break;
                    }
                    break;
                case StandardSoaDataOutputBo.TypeSoaQuarter:
                    if (string.IsNullOrEmpty(soaData.SoaQuarter))
                    {
                        success = false;
                        break;
                    }

                    var pattern = @"^\d{2}Q\d$";
                    var match = Regex.Match(soaData.SoaQuarter, pattern);
                    if (!match.Success)
                        success = false;

                    value = soaData.SoaQuarter;
                    break;
                case StandardSoaDataOutputBo.TypeRiskQuarter:
                    if (string.IsNullOrEmpty(soaData.RiskQuarter))
                    {
                        success = false;
                        break;
                    }

                    pattern = @"^\d{2}Q\d$";
                    match = Regex.Match(soaData.RiskQuarter, pattern);
                    if (!match.Success)
                        success = false;

                    value = soaData.RiskQuarter;
                    break;
                case StandardSoaDataOutputBo.TypeStatementStatus:
                    if (string.IsNullOrEmpty(soaData.StatementStatus))
                        break;

                    if (ProcessSoaDataBatch.CacheService.StatementStatus.IsNullOrEmpty())
                        success = false;
                    else if (ProcessSoaDataBatch.CacheService.StatementStatus.Where(q => q.Code == soaData.StatementStatus).ToList().IsNullOrEmpty())
                        success = false;

                    value = soaData.StatementStatus;
                    break;
                case StandardSoaDataOutputBo.TypeCurrencyCode:
                    if (string.IsNullOrEmpty(soaData.CurrencyCode))
                        break;

                    if (ProcessSoaDataBatch.CacheService.CurrencyCodes.IsNullOrEmpty())
                        success = false;
                    else if (ProcessSoaDataBatch.CacheService.CurrencyCodes.Where(q => q.Code == soaData.CurrencyCode).ToList().IsNullOrEmpty())
                        success = false;

                    value = soaData.CurrencyCode;
                    break;
                case StandardSoaDataOutputBo.TypeRemarks1:
                    int maxLength = 60;
                    if (!string.IsNullOrEmpty(soaData.Remarks1) && soaData.Remarks1.Length > maxLength)
                    {
                        success = false;
                        break;
                    }

                    value = soaData.Remarks1;
                    break;
                case StandardSoaDataOutputBo.TypeProfitComm:
                    if (ProcessSoaDataBatch.SoaDataBatchBo.IsProfitCommissionData && !soaData.ProfitComm.HasValue)
                    {
                        success = false;
                        break;
                    }
                    break;
            }

            if (!success)
            {
                success = false;
                soaData.MappingValidate = false;
                if (!string.IsNullOrEmpty(value))
                {
                    switch (mappingCol.Type)
                    {
                        case StandardSoaDataOutputBo.TypeSoaQuarter:
                        case StandardSoaDataOutputBo.TypeRiskQuarter:
                            soaData.SetError(mappingCol.Property, string.Format("Invalid Quarter Input: {0} ", value));
                            break;
                        default:
                            message = string.Format("{0} does not exists in database", value);
                            soaData.SetError(mappingCol.Property, string.Format("Mapping Error: {0}", message));
                            break;
                    }
                }
                else
                {
                    if (mappingCol.Type == StandardSoaDataOutputBo.TypeRemarks1)
                        soaData.SetError(mappingCol.Property, string.Format(MessageBag.MaxLength, mappingCol.Property, 60));
                    else
                        soaData.SetError(mappingCol.Property, "Field should not be empty");
                } 
                soaData.SetPropertyValue(mappingCol.Property, null);
                LogSoaDataFile.MappingErrorCount++;
            }
        }
    }
}
