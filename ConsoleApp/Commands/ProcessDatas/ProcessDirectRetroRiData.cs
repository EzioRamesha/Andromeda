using BusinessObject;
using BusinessObject.Identity;
using BusinessObject.RiDatas;
using DataAccess.Entities.Identity;
using Services;
using Services.Identity;
using Services.RiDatas;
using Shared;
using Shared.DataAccess;
using Shared.Trails;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleApp.Commands.ProcessDatas
{
    public class ProcessDirectRetroRiData : Command
    {
        public RiDataBo RiDataBo { get; set; }

        public int DirectRetroConfigurationId { get; set; }

        public IList<DirectRetroConfigurationDetailBo> DirectRetroConfigurationDetailBos { get; set; }

        public PremiumSpreadTableDetailBo PremiumSpreadTableDetailBo { get; set; }

        public int FailedType { get; set; }

        public ProcessDirectRetroRiData(RiDataBo riDataBo, int directRetroConfigurationId)
        {
            RiDataBo = riDataBo;
            DirectRetroConfigurationId = directRetroConfigurationId;
            FailedType = 0;
        }

        public void Process()
        {
            try
            {
                //WriteStatusLogFile(string.Format("Computing RI Data: {0}", RiDataBo.Id));
                List<string> errors = ValidateRiData(RiDataBo);
                if (errors.Count > 0)
                {
                    //foreach (string error in errors)
                    //{
                    //    WriteStatusLogFile(error);
                    //}
                    FailedType = DirectRetroBo.FailedTypeValidation;
                    return;
                }

                DirectRetroConfigurationDetailBos = DirectRetroConfigurationDetailService.GetByRiDataParam(DirectRetroConfigurationId, RiDataBo.RiskPeriodStartDate, RiDataBo.IssueDatePol, RiDataBo.ReinsEffDatePol, false);
                if (DirectRetroConfigurationDetailBos.Count == 0)
                {
                    //WriteStatusLogFile("There are no Retro Parties found", true); 
                    FailedType = DirectRetroBo.FailedTypeNoRetroParty;
                    return;
                }
                else if (DirectRetroConfigurationDetailBos.Count > 3)
                {
                    //WriteStatusLogFile(string.Format("There are more than 3 Retro Parties found", true)); 
                    FailedType = DirectRetroBo.FailedTypeExceedRetroParty;
                    return;
                }

                // Clear Previous Direct Retro Value
                RiDataBo.ResetDirectRetroValue();

                int index = 1;
                double totalRetroAar = 0;
                double totalRetroReinsurancePremium = 0;
                double totalRetroDiscount = 0;
                double totalNetPremium = 0;
                double totalNoClaimBonus = 0;
                double totalDatabaseCommission = 0;
                foreach (var detailBo in DirectRetroConfigurationDetailBos)
                {
                    //WriteStatusLogFile(string.Format("Retro Party {0} found, Party: {1}, Share: {2}", index, detailBo.RetroPartyBo.Party, detailBo.ShareStr));

                    if (!detailBo.PremiumSpreadTableId.HasValue)
                    {
                        //WriteStatusLogFile("Premium Spread Detail not exist in Direct Retro Configuration", true);
                        FailedType = DirectRetroBo.FailedTypePremiumSpread;
                        index++;
                        continue;
                    }

                    PremiumSpreadTableDetailBo = PremiumSpreadTableDetailService.GetByPremiumSpreadTableIdByParam(detailBo.PremiumSpreadTableId.Value, RiDataBo.CedingPlanCode, RiDataBo.MlreBenefitCode, RiDataBo.InsuredAttainedAge, false);

                    if (PremiumSpreadTableDetailBo == null)
                    {
                        //WriteStatusLogFile("There are no Premium Spread Detail found with the params", true);
                        FailedType = DirectRetroBo.FailedTypePremiumSpreadDetail;
                        index++;
                        continue;
                    }
                    //WriteStatusLogFile(string.Format("Premium Spread Detail found, Premium Spread: {0}", PremiumSpreadTableDetailBo.PremiumSpreadStr));

                    string retroPartyField = string.Format("RetroParty{0}", index);
                    string retroShareField = string.Format("RetroShare{0}", index);
                    string retroPremiumSpreadField = string.Format("RetroPremiumSpread{0}", index);
                    string retroAarField = string.Format("RetroAar{0}", index);
                    string retroReinsurancePremiumField = string.Format("RetroReinsurancePremium{0}", index);
                    string retroDiscountField = string.Format("RetroDiscount{0}", index);
                    string retroNetPremiumField = string.Format("RetroNetPremium{0}", index);
                    string retroNoClaimBonusField = string.Format("RetroNoClaimBonus{0}", index);
                    string retroDatabaseCommissionField = string.Format("RetroDatabaseCommission{0}", index);

                    double share = detailBo.Share;
                    double computeShare = share / 100;

                    RiDataBo.SetPropertyValue(retroPartyField, detailBo.RetroPartyBo.Party);
                    //LogValueSet(retroPartyField, detailBo.RetroPartyBo.Party);

                    RiDataBo.SetPropertyValue(retroShareField, share);
                    //LogValueSet(retroShareField, share);

                    RiDataBo.SetPropertyValue(retroPremiumSpreadField, PremiumSpreadTableDetailBo.PremiumSpread);
                    //LogValueSet(retroPremiumSpreadField, PremiumSpreadTableDetailBo.PremiumSpread);

                    double aar = RiDataBo.Aar.Value * computeShare;
                    totalRetroAar += aar;
                    RiDataBo.SetPropertyValue(retroAarField, aar);
                    //LogValueSet(retroAarField, aar);

                    // Change to transaction premium below
                    //double reinsurancePremium = RiDataBo.GrossPremium.Value * computeShare * PremiumSpreadTableDetailBo.PremiumSpread;
                    //totalRetroReinsurancePremium += reinsurancePremium;
                    //RiDataBo.SetPropertyValue(retroReinsurancePremiumField, reinsurancePremium);
                    //LogValueSet(retroReinsurancePremiumField, reinsurancePremium);

                    var transactionPremiumValue = RiDataBo.TransactionPremium ?? 0;
                    double transactionPremium = transactionPremiumValue * computeShare * PremiumSpreadTableDetailBo.PremiumSpread;
                    totalRetroReinsurancePremium += transactionPremium;
                    RiDataBo.SetPropertyValue(retroReinsurancePremiumField, transactionPremium);
                    //LogValueSet(retroReinsurancePremiumField, transactionPremium);

                    // Change to transaction discount below
                    //var totalDiscountValue = RiDataBo.TotalDiscount ?? 0;
                    //double totalDiscount = totalDiscountValue * computeShare * PremiumSpreadTableDetailBo.PremiumSpread;
                    //totalRetroDiscount += totalDiscount;
                    //RiDataBo.SetPropertyValue(retroDiscountField, totalDiscount);
                    //LogValueSet(retroDiscountField, totalDiscount);

                    var transactionDiscountValue = RiDataBo.TransactionDiscount ?? 0;
                    double transactionDiscount = transactionDiscountValue * computeShare * PremiumSpreadTableDetailBo.PremiumSpread;
                    totalRetroDiscount += transactionDiscount;
                    RiDataBo.SetPropertyValue(retroDiscountField, transactionDiscount);
                    //LogValueSet(retroDiscountField, transactionDiscount);

                    // Change to calculate from (Retro Reinsurance Premium - Retro Discount)
                    //double netPremium = RiDataBo.NetPremium.Value * computeShare * PremiumSpreadTableDetailBo.PremiumSpread;
                    //totalNetPremium += netPremium;
                    //RiDataBo.SetPropertyValue(retroNetPremiumField, netPremium);
                    //LogValueSet(retroNetPremiumField, netPremium);

                    double netPremium = transactionPremium - transactionDiscount;
                    totalNetPremium += netPremium;
                    RiDataBo.SetPropertyValue(retroNetPremiumField, netPremium);
                    //LogValueSet(retroNetPremiumField, netPremium);

                    var noClaimBonusValue = RiDataBo.NoClaimBonus ?? 0;
                    double noClaimBonus = noClaimBonusValue * computeShare;
                    totalNoClaimBonus += noClaimBonus;
                    RiDataBo.SetPropertyValue(retroNoClaimBonusField, noClaimBonus);
                    //LogValueSet(retroNoClaimBonusField, noClaimBonus);

                    var databaseCommissionValue = RiDataBo.DatabaseCommision ?? 0;
                    double databaseCommission = databaseCommissionValue * computeShare;
                    totalDatabaseCommission += databaseCommission;
                    RiDataBo.SetPropertyValue(retroDatabaseCommissionField, databaseCommission);
                    //LogValueSet(retroDatabaseCommissionField, databaseCommission);

                    index++;
                }

                RiDataBo.TotalDirectRetroAar = totalRetroAar;
                //LogValueSet("TotalDirectRetroAar", totalRetroAar);
                RiDataBo.TotalDirectRetroGrossPremium = totalRetroReinsurancePremium;
                //LogValueSet("TotalDirectRetroGrossPremium", totalRetroReinsurancePremium);
                RiDataBo.TotalDirectRetroDiscount = totalRetroDiscount;
                //LogValueSet("TotalDirectRetroDiscount", totalRetroDiscount);
                RiDataBo.TotalDirectRetroNetPremium = totalNetPremium;
                //LogValueSet("TotalDirectRetroNetPremium", totalNetPremium);
                RiDataBo.TotalDirectRetroNoClaimBonus = totalNoClaimBonus;
                //LogValueSet("TotalDirectRetroNoClaimBonus", totalNoClaimBonus);
                RiDataBo.TotalDirectRetroDatabaseCommission = totalDatabaseCommission;
                //LogValueSet("TotalDirectRetroDatabaseCommission", totalDatabaseCommission);

                UpdateRiData(RiDataBo);
                //WriteStatusLogFile(string.Format("Completed Compute RI Data: {0}", RiDataBo.Id), true);
            }
            catch (Exception e)
            {
                PrintError(e.Message);
                throw new Exception(e.Message);
            }
        }

        public List<string> ValidateRiData(RiDataBo bo)
        {
            List<string> errors = new List<string>();

            List<int> required = new List<int>
            {
                StandardOutputBo.TypeTreatyCode,
                StandardOutputBo.TypeAar,
                //StandardOutputBo.TypeGrossPremium, // Removed - Update to transaction premium
                //StandardOutputBo.TypeNetPremium, // Removed - calculate from (Direct Retro Reinsurance Premium - Direct Retro Discount)
                //StandardOutputBo.TypeTotalDiscount, // Removed - Update to transaction discount
                //StandardOutputBo.TypeRiskPeriodStartDate, //Optional
                //StandardOutputBo.TypeRiskPeriodEndDate, // Only use start date to compare
                //StandardOutputBo.TypeIssueDatePol, //Optional
                //StandardOutputBo.TypeReinsEffDatePol, //Optional
                StandardOutputBo.TypeCedingPlanCode,
                StandardOutputBo.TypeMlreBenefitCode,
                //StandardOutputBo.TypeInsuredAttainedAge, //Optional
            };

            foreach (int type in required)
            {
                string property = StandardOutputBo.GetPropertyNameByType(type);
                object value = bo.GetPropertyValue(property);
                if (value == null)
                {
                    errors.Add(string.Format("{0} is empty", property));
                }
                else if (value is string @string && string.IsNullOrEmpty(@string))
                {
                    errors.Add(string.Format("{0} is empty", property));
                }
            }

            return errors;
        }

        public void UpdateRiData(RiDataBo bo)
        {
            TrailObject trail = new TrailObject();

            Result result = RiDataService.Update(ref bo, ref trail);
            UserTrailBo userTrailBo = new UserTrailBo(
                bo.Id,
                "Update RI Data",
                result,
                trail,
                User.DefaultSuperUserId
            );
            UserTrailService.Create(ref userTrailBo);
        }

        //public void WriteStatusLogFile(string message, bool nextLine = false)
        //{
        //    Message.Add(message);
        //    if (nextLine)
        //        Message.Add("");
        //}

        //public void LogValueSet(string property, dynamic value)
        //{
        //    WriteStatusLogFile(string.Format("{0}: {1}", property, value));
        //}
    }
}
