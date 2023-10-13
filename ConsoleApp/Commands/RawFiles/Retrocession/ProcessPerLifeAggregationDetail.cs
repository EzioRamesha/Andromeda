using BusinessObject;
using BusinessObject.Retrocession;
using BusinessObject.RiDatas;
using DataAccess.Entities.Identity;
using Newtonsoft.Json;
using Services;
using Services.Retrocession;
using Shared;
using Shared.DataAccess;
using System;
using System.Collections.Generic;

namespace ConsoleApp.Commands.RawFiles.Retrocession
{
    public class ProcessPerLifeAggregationDetail : Command
    {
        public ProcessPerLifeAggregation ProcessPerLifeAggregation { get; set; }

        public PerLifeAggregationMonthlyDataBo PerLifeAggregationMonthlyDataBo { get; set; }

        public RiDataWarehouseHistoryBo RiDataWarehouseHistoryBo { get; set; }

        public IList<RetroTreatyDetailBo> RetroTreatyDetailBos { get; set; }

        public RetroTreatyDetailBo RetroTreatyDetailBo { get; set; }

        public double? PremiumSpread { get; set; }

        public double? TreatyDiscount { get; set; }

        public StandardRetroOutputEval StandardRetroOutputEval { get; set; }

        public List<string> Errors { get; set; }

        public bool Success { get; set; } = true;

        public double? GstRate { get; set; }

        public ProcessPerLifeAggregationDetail(ProcessPerLifeAggregation processPerLifeAggregation, PerLifeAggregationMonthlyDataBo perLifeAggregationMonthlyDataBo)
        {
            ProcessPerLifeAggregation = processPerLifeAggregation;
            PerLifeAggregationMonthlyDataBo = perLifeAggregationMonthlyDataBo;
            RiDataWarehouseHistoryBo = perLifeAggregationMonthlyDataBo.PerLifeAggregationDetailDataBo.RiDataWarehouseHistoryBo;
        }

        public void ComputeRetroPeremium()
        {
            if (!LoadConfigurations())
                return;

            Errors = new List<string>();

            foreach (RetroTreatyDetailBo bo in RetroTreatyDetailBos)
            {
                Success = true;
                RetroTreatyDetailBo = bo;

                LoadTables();

                ComputeFormula(RetroTreatyDetailBo.GrossRetroPremium, "RetroGrossPremium");
                ComputeFormula(RetroTreatyDetailBo.TreatyDiscount, "RetroDiscount");
                ComputeFormula(RetroTreatyDetailBo.NetRetroPremium, "RetroNetPremium");

                if (Success)
                    CreateRetroData();
            }

            if (!Errors.IsNullOrEmpty())
            {
                Success = false;
            }
            UpdateMonthlyDataErrors();
        }

        public bool LoadConfigurations()
        {
            DateTime riskDate = new DateTime(PerLifeAggregationMonthlyDataBo.RiskYear, PerLifeAggregationMonthlyDataBo.RiskMonth, 1);
            RetroTreatyDetailBos = ProcessPerLifeAggregation.GetRetroTreatyDetails(RiDataWarehouseHistoryBo.ReinsBasisCode, RiDataWarehouseHistoryBo.TreatyCode, RiDataWarehouseHistoryBo.TreatyType,
                RiDataWarehouseHistoryBo.ReinsEffDatePol, riskDate);

            GstRate = ProcessPerLifeAggregation.GetGst(RiDataWarehouseHistoryBo.ReinsEffDatePol, riskDate);

            return !RetroTreatyDetailBos.IsNullOrEmpty();
        }

        public void LoadTables()
        {
            RetroTreatyDetailBo bo = RetroTreatyDetailBo;

            PremiumSpread = null;
            TreatyDiscount = null;

            if (bo.PremiumSpreadTableId.HasValue)
            {
                PremiumSpread = PremiumSpreadTableDetailService.GetPremiumSpreadByParams(bo.PremiumSpreadTableId.Value, RiDataWarehouseHistoryBo.CedingPlanCode, 
                    RiDataWarehouseHistoryBo.MlreBenefitCode, RiDataWarehouseHistoryBo.InsuredAttainedAge);
            }

            if (bo.TreatyDiscountTableId.HasValue)
            {
                TreatyDiscount = TreatyDiscountTableDetailService.GetDiscountByParams(bo.TreatyDiscountTableId.Value, RiDataWarehouseHistoryBo.CedingPlanCode,
                    RiDataWarehouseHistoryBo.MlreBenefitCode, PerLifeAggregationMonthlyDataBo.Aar);
            }

            StandardRetroOutputEval = new StandardRetroOutputEval()
            {
                PerLifeAggregationMonthlyDataBo = PerLifeAggregationMonthlyDataBo,
                MlreShare = bo.MlreShare,
                PremiumSpread = PremiumSpread,
                Discount = TreatyDiscount,
            };
        }

        public void ComputeFormula(string formula, string property)
        {
            if (string.IsNullOrEmpty(formula))
                return;

            try
            {
                StandardRetroOutputEval.Formula = formula;
                StandardRetroOutputEval.Errors = new List<string>();
                object value = StandardRetroOutputEval.EvalFormula();

                if (StandardRetroOutputEval.Errors.IsNullOrEmpty())
                {
                    if (Util.IsValidDouble(value, out double? val, out string error))
                    {
                        StandardRetroOutputEval.SetPropertyValue(property, val);
                    }
                    else
                    {
                        SetError(error);
                    }
                }
                else
                {
                    foreach(string error in StandardRetroOutputEval.Errors)
                    {
                        SetError(error);
                    }
                }
            }
            catch (Exception e)
            {
                SetError(e.ToString());
            }
        }

        public void CreateRetroData()
        {
            double? retroGst = null;
            if (GstRate.HasValue && StandardRetroOutputEval.RetroNetPremium.HasValue)
            {
                retroGst = StandardRetroOutputEval.RetroNetPremium * (GstRate / 100);
            }

            PerLifeAggregationMonthlyRetroDataBo bo = new PerLifeAggregationMonthlyRetroDataBo()
            {
                PerLifeAggregationMonthlyDataId = PerLifeAggregationMonthlyDataBo.Id,
                RetroParty = RetroTreatyDetailBo.RetroTreatyBo.RetroPartyBo?.Code,
                RetroGrossPremium = StandardRetroOutputEval.RetroGrossPremium,
                RetroDiscount = StandardRetroOutputEval.RetroDiscount,
                RetroNetPremium = StandardRetroOutputEval.RetroNetPremium,
                RetroGst = retroGst,
                MlreShare = RetroTreatyDetailBo.MlreShare,
                CreatedById = User.DefaultSuperUserId,
                UpdatedById = User.DefaultSuperUserId
            };

            Result result = PerLifeAggregationMonthlyRetroDataService.Create(ref bo);
            if (!result.Valid)
                Success = false;
        }

        public void UpdateMonthlyDataErrors()
        {
            string errors = Errors.IsNullOrEmpty() ? null : JsonConvert.SerializeObject(Errors);
            var bo = PerLifeAggregationMonthlyDataBo;

            if (bo.Errors == errors)
                return;

            bo.Errors = errors;
            bo.UpdatedById = User.DefaultSuperUserId;

            PerLifeAggregationMonthlyDataService.Update(ref bo);
        }

        public void SetError(string error)
        {
            Success = false;
            Errors.Add(string.Format("{0} for Retro Party {1}", error, RetroTreatyDetailBo.RetroTreatyBo.RetroPartyBo?.Code));
        } 
    }
}
