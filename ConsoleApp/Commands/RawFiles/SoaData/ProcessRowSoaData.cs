using BusinessObject;
using BusinessObject.SoaDatas;
using Services.SoaDatas;
using Shared;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleApp.Commands.RawFiles.SoaData
{
    public class ProcessRowSoaData : Command
    {
        public ProcessSoaDataBatch ProcessSoaDataBatch { get; set; }
        public List<SoaDataBo> SoaDataBos { get; set; }

        public ProcessRowSoaData(ProcessSoaDataBatch batch, MappingSoaData mappingSoaData)
        {
            ProcessSoaDataBatch = batch;
            SoaDataBos = mappingSoaData.SoaDataBos;
        }

        public void Save()
        {
            if (ProcessSoaDataBatch.Test)
                return;
            if (SoaDataBos.IsNullOrEmpty())
                return;

            foreach (var soaDataBo in SoaDataBos)
            {
                var soaData = soaDataBo;
                soaData.GetTotalCommission();
                if (string.IsNullOrEmpty(soaData.CompanyName))
                    soaData.CompanyName = ProcessSoaDataBatch.CedantBo.Name;
                if (string.IsNullOrEmpty(soaData.BusinessCode))
                    soaData.BusinessCode = ProcessSoaDataBatch.TreatyBo.BusinessOriginPickListDetailBo?.Code;
                if (string.IsNullOrEmpty(soaData.TreatyType))
                {
                    var TreatyTypeCode = ProcessSoaDataBatch.CacheService.TreatyCodeBos.Where(q => q.Code == soaData.TreatyCode && q.TreatyId == ProcessSoaDataBatch.TreatyId).FirstOrDefault();
                    if (TreatyTypeCode != null)
                        soaData.TreatyType = TreatyTypeCode.TreatyTypePickListDetailBo?.Code;
                }
                if (string.IsNullOrEmpty(soaData.CurrencyCode))
                    soaData.CurrencyCode = ProcessSoaDataBatch.CurrencyCodePickListDetailBo?.Code;
                if (!soaData.CurrencyRate.HasValue)
                    soaData.CurrencyRate = ProcessSoaDataBatch.SoaDataBatchBo.CurrencyRate;
                soaData.SoaDataBatchId = ProcessSoaDataBatch.SoaDataFileBo.SoaDataBatchId;
                soaData.SoaDataFileId = ProcessSoaDataBatch.SoaDataFileBo.Id;
                soaData.CreatedById = ProcessSoaDataBatch.SoaDataFileBo.CreatedById;
                soaData.UpdatedById = ProcessSoaDataBatch.SoaDataFileBo.UpdatedById;
                soaData.GetGrossPremium();
                soaData.GetTotalCommission();
                soaData.GetNetTotalAmount();
                SoaDataService.Create(ref soaData);

                // MYR currency summary to be derive from foreign amount * currency rate
                if (!string.IsNullOrEmpty(soaDataBo.CurrencyCode) && soaDataBo.CurrencyCode != PickListDetailBo.CurrencyCodeMyr)
                {
                    var MYRbo = new SoaDataBo
                    {
                        BusinessCode = soaData.BusinessCode,
                        TreatyId = soaData.TreatyId,
                        TreatyCode = soaData.TreatyCode,
                        RiskMonth = soaData.RiskMonth,
                        SoaReceivedDate = soaData.SoaReceivedDate,
                        BordereauxReceivedDate = soaData.BordereauxReceivedDate,
                        CompanyName = soaData.CompanyName,
                        RiskQuarter = soaData.RiskQuarter,
                        TreatyType = soaData.TreatyType,
                        TreatyMode = soaData.TreatyMode,
                        CurrencyCode = PickListDetailBo.CurrencyCodeMyr,
                        CurrencyRate = soaData.CurrencyRate,
                        SoaDataBatchId = ProcessSoaDataBatch.SoaDataFileBo.SoaDataBatchId,
                        CreatedById = ProcessSoaDataBatch.SoaDataFileBo.CreatedById,
                        UpdatedById = ProcessSoaDataBatch.SoaDataFileBo.UpdatedById,
                    };
                    MYRbo.NbPremium = soaData.NbPremium * MYRbo.CurrencyRate;
                    MYRbo.RnPremium = soaData.RnPremium * MYRbo.CurrencyRate;
                    MYRbo.AltPremium = soaData.AltPremium * MYRbo.CurrencyRate;

                    MYRbo.TotalDiscount = soaData.TotalDiscount * MYRbo.CurrencyRate;
                    MYRbo.RiskPremium = soaData.RiskPremium * MYRbo.CurrencyRate;
                    MYRbo.NoClaimBonus = soaData.NoClaimBonus * MYRbo.CurrencyRate;
                    MYRbo.Levy = soaData.Levy * MYRbo.CurrencyRate;
                    MYRbo.ProfitComm = soaData.ProfitComm * MYRbo.CurrencyRate;
                    MYRbo.SurrenderValue = soaData.SurrenderValue * MYRbo.CurrencyRate;
                    MYRbo.ModcoReserveIncome = soaData.ModcoReserveIncome * MYRbo.CurrencyRate;
                    MYRbo.RiDeposit = soaData.RiDeposit * MYRbo.CurrencyRate;
                    MYRbo.DatabaseCommission = soaData.DatabaseCommission * MYRbo.CurrencyRate;
                    MYRbo.AdministrationContribution = soaData.AdministrationContribution * MYRbo.CurrencyRate;
                    MYRbo.ShareOfRiCommissionFromCompulsoryCession = soaData.ShareOfRiCommissionFromCompulsoryCession * MYRbo.CurrencyRate;
                    MYRbo.RecaptureFee = soaData.RecaptureFee * MYRbo.CurrencyRate;
                    MYRbo.CreditCardCharges = soaData.CreditCardCharges * MYRbo.CurrencyRate;
                    MYRbo.BrokerageFee = soaData.BrokerageFee * MYRbo.CurrencyRate;
                    MYRbo.Claim = soaData.Claim * MYRbo.CurrencyRate;
                    MYRbo.Gst = soaData.Gst * MYRbo.CurrencyRate;

                    MYRbo.GetGrossPremium();
                    MYRbo.GetTotalCommission();
                    MYRbo.GetNetTotalAmount();

                    SoaDataService.Create(ref MYRbo);
                }
            }
        }
    }
}
