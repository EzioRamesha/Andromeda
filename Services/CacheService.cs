using BusinessObject;
using BusinessObject.Claims;
using BusinessObject.RiDatas;
using DataAccess.EntityFramework;
using Services.RiDatas;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace Services
{
    public class CacheService
    {
        public IList<SalutationBo> SalutationBos { get; set; }
        public IList<PickListDetailBo> InsuredGenderCodes { get; set; }
        public IList<PickListDetailBo> InsuredTobaccoUses { get; set; }
        public IList<PickListDetailBo> InsuredOccupationCodes { get; set; }
        public IList<PickListDetailBo> ReinsBasisCodes { get; set; }
        public IList<PickListDetailBo> PremiumFrequencyCodes { get; set; }
        public IList<PickListDetailBo> BusinessOrigins { get; set; }

        public IList<PickListDetailBo> StatementStatus { get; set; }
        public IList<PickListDetailBo> CurrencyCodes { get; set; }

        public IEnumerable<string> TreatyIdCodes { get; set; }
        public IEnumerable<string> TreatyCodes { get; set; }
        public IEnumerable<string> BenefitCodes { get; set; }
        public IEnumerable<string> RateTableCodes { get; set; }
        public IEnumerable<string> Mfrs17CellMappingCellNames { get; set; }
        public IEnumerable<string> Mfrs17CellMappingContractCodes { get; set; }
        public IEnumerable<string> Mfrs17CellMappingLoaCodes { get; set; }
        public IEnumerable<PickListDetailBo> RiDataDropDownDetailBos { get; set; }

        public IEnumerable<TreatyCodeBo> TreatyCodeBos { get; set; }

        public List<TreatyBenefitCodeMappingDetailBo> TreatyBenefitCodeMappingDetailBosForTreatyMapping { get; set; }

        public List<TreatyBenefitCodeMappingDetailBo> TreatyBenefitCodeMappingDetailBosForBenefitMapping { get; set; }

        public List<TreatyBenefitCodeMappingDetailBo> TreatyBenefitCodeMappingDetailBosForProductFeatureMapping { get; set; }

        public List<Mfrs17CellMappingDetailBo> Mfrs17CellMappingDetailBosForCellMapping { get; set; }

        public List<RateTableDetailBo> RateTableDetailBosForRateTableMapping { get; set; }

        public List<AnnuityFactorMappingBo> AnnuityFactorMappingBosForAnnuityFactorMapping { get; set; }

        public List<FacMasterListingDetailBo> FacMasterListingDetailBosForFacMapping { get; set; }

        public List<TreatyCodeBo> TreatyCodeBosForTreatyNumberTreatyTypeMapping { get; set; }

        private static object _listLock { get; set; }

        public const int MappingTypeTreaty = 1;
        public const int MappingTypeBenefit = 2;
        public const int MappingTypeProductFeature = 3;
        public const int MappingTypeCell = 4;
        public const int MappingTypeRateTable = 5;
        public const int MappingTypeAnnuityFactor = 6;
        public const int MappingTypeFac = 7;
        public const int MappingTypeTreatyNumberTreatyType = 8;

        public void Load()
        {
            SalutationBos = SalutationService.Get().OrderByDescending(q => q.Name.Length).ToList();
            InsuredGenderCodes = PickListDetailService.GetByPickListId(PickListBo.InsuredGenderCode);
            InsuredTobaccoUses = PickListDetailService.GetByPickListId(PickListBo.InsuredTobaccoUse);
            InsuredOccupationCodes = PickListDetailService.GetByPickListId(PickListBo.InsuredOccupationCode);
            ReinsBasisCodes = PickListDetailService.GetByPickListId(PickListBo.ReinsBasisCode);
            PremiumFrequencyCodes = PickListDetailService.GetByPickListId(PickListBo.PremiumFrequencyCode);
            BusinessOrigins = PickListDetailService.GetByPickListId(PickListBo.BusinessOrigin);

            TreatyBenefitCodeMappingDetailBosForTreatyMapping = new List<TreatyBenefitCodeMappingDetailBo>();
            TreatyBenefitCodeMappingDetailBosForBenefitMapping = new List<TreatyBenefitCodeMappingDetailBo>();
            TreatyBenefitCodeMappingDetailBosForProductFeatureMapping = new List<TreatyBenefitCodeMappingDetailBo>();

            Mfrs17CellMappingDetailBosForCellMapping = new List<Mfrs17CellMappingDetailBo>();
            RateTableDetailBosForRateTableMapping = new List<RateTableDetailBo>();
            AnnuityFactorMappingBosForAnnuityFactorMapping = new List<AnnuityFactorMappingBo>();
            FacMasterListingDetailBosForFacMapping = new List<FacMasterListingDetailBo>();
            TreatyCodeBosForTreatyNumberTreatyTypeMapping = new List<TreatyCodeBo>();

            _listLock = new object();
        }

        public void LoadForRiData(int cedantId)
        {
            LoadTreatyCodes(cedantId);
            LoadBenefitCodes();
            LoadRateTableCodes();
            LoadMfrs17CellMappingCellNames();
            LoadMfrs17CellMappingContractCodes();
            LoadMfrs17CellMappingLoaCodes();
            LoadRiDataDropDownDetailBos();
        }

        public void LoadForSoaData(int cedantId)
        {
            LoadTreatyIdCodes(cedantId);
            LoadTreatyCodes(cedantId);
            LoadTreatyCodeBosByCedantId(cedantId);
            StatementStatus = PickListDetailService.GetByPickListId(PickListBo.StatementStatus);
            CurrencyCodes = PickListDetailService.GetByPickListId(PickListBo.CurrencyCode);
        }

        public void LoadTreatyIdCodes(int cedantId)
        {
            TreatyIdCodes = TreatyService.GetTreatyIdCodesByCedantId(cedantId);
        }

        public void LoadTreatyCodes(int cedantId)
        {
            TreatyCodes = TreatyCodeService.GetTreatyCodesByCedantId(cedantId);
        }

        public void LoadTreatyCodesByTreatyId(int treatyId)
        {
            TreatyCodes = TreatyCodeService.GetTreatyCodesByTreatyId(treatyId);
        }

        public void LoadBenefitCodes()
        {
            BenefitCodes = BenefitService.GetBenefitCodes();
        }

        public void LoadRateTableCodes()
        {
            RateTableCodes = RateService.GetRateTableCodes();
        }

        public void LoadMfrs17CellMappingCellNames()
        {
            Mfrs17CellMappingCellNames = Mfrs17CellMappingService.GetCellNames();
        }

        public void LoadMfrs17CellMappingContractCodes()
        {
            Mfrs17CellMappingContractCodes = Mfrs17ContractCodeDetailService.GetContractCode();
        }

        public void LoadMfrs17CellMappingLoaCodes()
        {
            Mfrs17CellMappingLoaCodes = Mfrs17CellMappingService.GetLoaCodes();
        }

        public void LoadRiDataDropDownDetailBos()
        {
            RiDataDropDownDetailBos = RiDataService.GetDropDowns();
        }

        public void LoadTreatyCodeBosByCedantId(int cedantId)
        {
            TreatyCodeBos = TreatyCodeService.GetByCedantId(cedantId);
        }

        public void LoadTreatyCodeBosByTreatyId(int treatyId)
        {
            TreatyCodeBos = TreatyCodeService.GetByTreatyId(treatyId);
        }

        public int? GetId(IList<PickListDetailBo> pl)
        {
            if (pl.Count == 1)
                return pl[0].Id;
            return null;
        }

        public int? GetInsuredGenderCodesId(RiDataBo riDataBo)
        {
            if (string.IsNullOrEmpty(riDataBo.InsuredGenderCode))
                return null;
            var ids = InsuredGenderCodes.Where(q => !string.IsNullOrEmpty(q.Code) && q.Code.ToLower() == riDataBo.InsuredGenderCode.ToLower()).ToList();
            return GetId(ids);
        }

        public int? GetInsuredTobaccoUseId(RiDataBo riDataBo)
        {
            if (string.IsNullOrEmpty(riDataBo.InsuredTobaccoUse))
                return null;
            var ids = InsuredTobaccoUses.Where(q => !string.IsNullOrEmpty(q.Code) && q.Code.ToLower() == riDataBo.InsuredTobaccoUse.ToLower()).ToList();
            return GetId(ids);
        }

        public int? GetInsuredOccupationCodeId(RiDataBo riDataBo)
        {
            if (string.IsNullOrEmpty(riDataBo.InsuredOccupationCode))
                return null;
            var ids = InsuredOccupationCodes.Where(q => !string.IsNullOrEmpty(q.Code) && q.Code.ToLower() == riDataBo.InsuredOccupationCode.ToLower()).ToList();
            return GetId(ids);
        }

        public int? GetReinsBasisCodeId(RiDataBo riDataBo)
        {
            if (string.IsNullOrEmpty(riDataBo.ReinsBasisCode))
                return null;
            var ids = ReinsBasisCodes.Where(q => !string.IsNullOrEmpty(q.Code) && q.Code.ToLower() == riDataBo.ReinsBasisCode.ToLower()).ToList();
            return GetId(ids);
        }

        public int? GetReinsBasisCodeId(ClaimDataBo claimDataBo)
        {
            if (string.IsNullOrEmpty(claimDataBo.ReinsBasisCode))
                return null;
            var ids = ReinsBasisCodes.Where(q => !string.IsNullOrEmpty(q.Code) && q.Code.ToLower() == claimDataBo.ReinsBasisCode.ToLower()).ToList();
            return GetId(ids);
        }

        public int? GetPremiumFrequencyCodeId(RiDataBo riDataBo)
        {
            if (string.IsNullOrEmpty(riDataBo.PremiumFrequencyCode))
                return null;
            var ids = PremiumFrequencyCodes.Where(q => !string.IsNullOrEmpty(q.Code) && q.Code.ToLower() == riDataBo.PremiumFrequencyCode.ToLower()).ToList();
            return GetId(ids);
        }

        public void AddMappingBo(int type, object bo)
        {
            switch (type)
            {
                case MappingTypeTreaty:
                    var newBoTreaty = (TreatyBenefitCodeMappingDetailBo)bo;
                    lock (_listLock)
                    {
                        TreatyBenefitCodeMappingDetailBosForTreatyMapping.Add(newBoTreaty);
                    }
                    break;
                case MappingTypeBenefit:
                    var newBoBenefit = (TreatyBenefitCodeMappingDetailBo)bo;
                    lock (_listLock)
                    {
                        TreatyBenefitCodeMappingDetailBosForBenefitMapping.Add(newBoBenefit);
                    }
                    break;
                case MappingTypeProductFeature:
                    var newBoProductFeature = (TreatyBenefitCodeMappingDetailBo)bo;
                    lock (_listLock)
                    {
                        TreatyBenefitCodeMappingDetailBosForProductFeatureMapping.Add(newBoProductFeature);
                    }
                    break;
                case MappingTypeCell:
                    var newBoCell = (Mfrs17CellMappingDetailBo)bo;
                    lock (_listLock)
                    {
                        Mfrs17CellMappingDetailBosForCellMapping.Add(newBoCell);
                    }
                    break;
                case MappingTypeRateTable:
                    var newBoRateTable = (RateTableDetailBo)bo;
                    lock (_listLock)
                    {
                        RateTableDetailBosForRateTableMapping.Add(newBoRateTable);
                    }
                    break;
                case MappingTypeAnnuityFactor:
                    var newBoAnnuityFactor = (AnnuityFactorMappingBo)bo;
                    lock (_listLock)
                    {
                        AnnuityFactorMappingBosForAnnuityFactorMapping.Add(newBoAnnuityFactor);
                    }
                    break;
                case MappingTypeFac:
                    var newBoFac = (FacMasterListingDetailBo)bo;
                    lock (_listLock)
                    {
                        FacMasterListingDetailBosForFacMapping.Add(newBoFac);
                    }
                    break;
                case MappingTypeTreatyNumberTreatyType:
                    var newBoTreatyCode = (TreatyCodeBo)bo;
                    lock (_listLock)
                    {
                        TreatyCodeBosForTreatyNumberTreatyTypeMapping.Add(newBoTreatyCode);
                    }
                    break;
                default:
                    break;
            }
        }

        public TreatyBenefitCodeMappingDetailBo FindByTreatyParams(
            int cedantId,
            RiDataBo riData,
            DateTime? reportDate = null,
            bool groupById = false)
        {
            lock (_listLock)
            {
                var query = TreatyBenefitCodeMappingDetailBosForTreatyMapping
                    .Where(q => q.Type == TreatyBenefitCodeMappingDetailBo.CombinationTypeTreaty) // IMPORTANT
                    .Where(q => q.TreatyBenefitCodeMappingBo.CedantId == cedantId)
                    .Where(q => q.CedingPlanCode.Trim() == riData.CedingPlanCode.Trim())
                    .Where(q => q.CedingBenefitTypeCode.Trim() == riData.CedingBenefitTypeCode.Trim());

                if (riData.ReinsEffDatePol.HasValue)
                {
                    query = query
                        .Where(q =>
                            (q.TreatyBenefitCodeMappingBo.ReinsEffDatePolStartDate.HasValue && q.TreatyBenefitCodeMappingBo.ReinsEffDatePolEndDate.HasValue &&
                            (
                                q.TreatyBenefitCodeMappingBo.ReinsEffDatePolStartDate.Value.Date <= riData.ReinsEffDatePol.Value.Date
                                && q.TreatyBenefitCodeMappingBo.ReinsEffDatePolEndDate.Value.Date >= riData.ReinsEffDatePol.Value.Date
                            ))
                            ||
                            q.TreatyBenefitCodeMappingBo.ReinsEffDatePolStartDate == null && q.TreatyBenefitCodeMappingBo.ReinsEffDatePolEndDate == null
                        );
                }
                else
                {
                    query = query
                        .Where(q => q.TreatyBenefitCodeMappingBo.ReinsEffDatePolStartDate == null && q.TreatyBenefitCodeMappingBo.ReinsEffDatePolEndDate == null);
                }

                if (!string.IsNullOrEmpty(riData.CedingBenefitRiskCode))
                {
                    query = query
                        .Where(q =>
                            (!string.IsNullOrEmpty(q.CedingBenefitRiskCode) && q.CedingBenefitRiskCode.Trim() == riData.CedingBenefitRiskCode.Trim())
                            ||
                            q.CedingBenefitRiskCode == null
                        );
                }
                else
                {
                    query = query
                        .Where(q => q.CedingBenefitRiskCode == null);
                }

                if (!string.IsNullOrEmpty(riData.CedingTreatyCode))
                {
                    query = query
                        .Where(q =>
                            (!string.IsNullOrEmpty(q.CedingTreatyCode) && q.CedingTreatyCode.Trim() == riData.CedingTreatyCode.Trim())
                            ||
                            q.CedingTreatyCode == null
                        );
                }
                else
                {
                    query = query
                        .Where(q => q.CedingTreatyCode == null);
                }

                if (!string.IsNullOrEmpty(riData.CampaignCode))
                {
                    query = query
                        .Where(q =>
                            (!string.IsNullOrEmpty(q.CampaignCode) && q.CampaignCode.Trim() == riData.CampaignCode.Trim())
                            ||
                            q.CampaignCode == null
                        );
                }
                else
                {
                    query = query
                        .Where(q => q.CampaignCode == null);
                }

                int? reinsBasisCodeId = GetReinsBasisCodeId(riData);
                if (reinsBasisCodeId != null)
                {
                    query = query
                        .Where(q =>
                            (q.TreatyBenefitCodeMappingBo.ReinsBasisCodePickListDetailId != null && q.TreatyBenefitCodeMappingBo.ReinsBasisCodePickListDetailId == reinsBasisCodeId)
                            ||
                            q.TreatyBenefitCodeMappingBo.ReinsBasisCodePickListDetailId == null
                        );
                }
                else
                {
                    query = query
                        .Where(q => q.TreatyBenefitCodeMappingBo.ReinsBasisCodePickListDetailId == null);
                }

                if (riData.InsuredAttainedAge.HasValue)
                {
                    query = query
                        .Where(q =>
                            (
                                (
                                    q.TreatyBenefitCodeMappingBo.AttainedAgeFrom <= riData.InsuredAttainedAge && q.TreatyBenefitCodeMappingBo.AttainedAgeTo >= riData.InsuredAttainedAge
                                    ||
                                    q.TreatyBenefitCodeMappingBo.AttainedAgeFrom <= riData.InsuredAttainedAge && q.TreatyBenefitCodeMappingBo.AttainedAgeTo >= riData.InsuredAttainedAge
                                )
                                ||
                                (q.TreatyBenefitCodeMappingBo.AttainedAgeFrom == null && q.TreatyBenefitCodeMappingBo.AttainedAgeTo == null)
                            )
                        );
                }
                else
                {
                    query = query
                        .Where(q => q.TreatyBenefitCodeMappingBo.AttainedAgeFrom == null && q.TreatyBenefitCodeMappingBo.AttainedAgeTo == null);
                }

                if (reportDate != null)
                {
                    query = query
                        .Where(q =>
                            (q.TreatyBenefitCodeMappingBo.ReportingStartDate.HasValue && q.TreatyBenefitCodeMappingBo.ReportingEndDate.HasValue &&
                            (
                                q.TreatyBenefitCodeMappingBo.ReportingStartDate.Value.Date <= reportDate.Value.Date
                                && q.TreatyBenefitCodeMappingBo.ReportingEndDate.Value.Date >= reportDate.Value.Date
                                ||
                                q.TreatyBenefitCodeMappingBo.ReportingStartDate.Value.Date <= reportDate.Value.Date
                                && q.TreatyBenefitCodeMappingBo.ReportingEndDate.Value.Date >= reportDate.Value.Date
                            ))
                            ||
                            (q.TreatyBenefitCodeMappingBo.ReportingStartDate == null && q.TreatyBenefitCodeMappingBo.ReportingEndDate == null)
                        );
                }
                else
                {
                    query = query
                        .Where(q => q.TreatyBenefitCodeMappingBo.ReportingStartDate == null && q.TreatyBenefitCodeMappingBo.ReportingEndDate == null);
                }

                if (riData.UnderwriterRating.HasValue)
                {
                    query = query
                        .Where(q =>
                            (
                                q.TreatyBenefitCodeMappingBo.UnderwriterRatingFrom <= riData.UnderwriterRating && q.TreatyBenefitCodeMappingBo.UnderwriterRatingTo >= riData.UnderwriterRating
                                ||
                                q.TreatyBenefitCodeMappingBo.UnderwriterRatingFrom == null && q.TreatyBenefitCodeMappingBo.UnderwriterRatingTo == null
                            )
                        );
                }
                else
                {
                    query = query
                        .Where(q => q.TreatyBenefitCodeMappingBo.UnderwriterRatingFrom == null && q.TreatyBenefitCodeMappingBo.UnderwriterRatingTo == null);
                }

                if (riData.OriSumAssured.HasValue)
                {
                    query = query
                        .Where(q =>
                            (
                                q.TreatyBenefitCodeMappingBo.OriSumAssuredFrom <= riData.OriSumAssured && q.TreatyBenefitCodeMappingBo.OriSumAssuredTo >= riData.OriSumAssured
                                ||
                                q.TreatyBenefitCodeMappingBo.OriSumAssuredFrom == null && q.TreatyBenefitCodeMappingBo.OriSumAssuredTo == null
                            )
                        );
                }
                else
                {
                    query = query
                        .Where(q => q.TreatyBenefitCodeMappingBo.OriSumAssuredFrom == null && q.TreatyBenefitCodeMappingBo.OriSumAssuredTo == null);
                }

                if (riData.ReinsuranceIssueAge != null)
                {
                    query = query
                        .Where(q =>
                            (
                                (
                                    q.TreatyBenefitCodeMappingBo.ReinsuranceIssueAgeFrom <= riData.ReinsuranceIssueAge && q.TreatyBenefitCodeMappingBo.ReinsuranceIssueAgeTo >= riData.ReinsuranceIssueAge
                                    ||
                                    q.TreatyBenefitCodeMappingBo.ReinsuranceIssueAgeFrom <= riData.ReinsuranceIssueAge && q.TreatyBenefitCodeMappingBo.ReinsuranceIssueAgeTo >= riData.ReinsuranceIssueAge
                                )
                                ||
                                (q.TreatyBenefitCodeMappingBo.ReinsuranceIssueAgeFrom == null && q.TreatyBenefitCodeMappingBo.ReinsuranceIssueAgeTo == null)
                            )
                        );
                }
                else
                {
                    query = query
                        .Where(q => q.TreatyBenefitCodeMappingBo.ReinsuranceIssueAgeFrom == null && q.TreatyBenefitCodeMappingBo.ReinsuranceIssueAgeTo == null);
                }

                // NOTE: Group by should put at the end of query
                if (groupById)
                {
                    query = query.GroupBy(q => q.TreatyBenefitCodeMappingId).Select(q => q.FirstOrDefault());
                }

                return query.FirstOrDefault();
            }
        }

        public TreatyBenefitCodeMappingDetailBo FindByBenefitParams(
            int cedantId,
            RiDataBo riData,
            DateTime? reportDate = null,
            bool groupById = false)
        {
            lock (_listLock)
            {
                EFExecutionStrategy connectionStrategy = EFExecutionStrategy.GetNewInstance("CacheService");

                var query = connectionStrategy.Execute(() => TreatyBenefitCodeMappingDetailBosForBenefitMapping
                    .Where(q => q.Type == TreatyBenefitCodeMappingDetailBo.CombinationTypeBenefit) // IMPORTANT
                    .Where(q => q.TreatyBenefitCodeMappingBo.CedantId == cedantId)
                    .Where(q => q.CedingPlanCode.Trim() == riData.CedingPlanCode.Trim())
                    .Where(q => q.CedingBenefitTypeCode.Trim() == riData.CedingBenefitTypeCode.Trim()));

                if (riData.ReinsEffDatePol.HasValue)
                {
                    connectionStrategy.Reset();
                    query = connectionStrategy.Execute(() => query
                        .Where(q =>
                            (q.TreatyBenefitCodeMappingBo.ReinsEffDatePolStartDate.HasValue && q.TreatyBenefitCodeMappingBo.ReinsEffDatePolEndDate.HasValue &&
                            (
                                q.TreatyBenefitCodeMappingBo.ReinsEffDatePolStartDate.Value.Date <= riData.ReinsEffDatePol.Value.Date
                                && q.TreatyBenefitCodeMappingBo.ReinsEffDatePolEndDate.Value.Date >= riData.ReinsEffDatePol.Value.Date
                            ))
                            ||
                            q.TreatyBenefitCodeMappingBo.ReinsEffDatePolStartDate == null && q.TreatyBenefitCodeMappingBo.ReinsEffDatePolEndDate == null
                        ));
                }
                else
                {
                    connectionStrategy.Reset();
                    query = connectionStrategy.Execute(() => query
                        .Where(q => q.TreatyBenefitCodeMappingBo.ReinsEffDatePolStartDate == null && q.TreatyBenefitCodeMappingBo.ReinsEffDatePolEndDate == null));
                }

                if (!string.IsNullOrEmpty(riData.CedingBenefitRiskCode))
                {
                    connectionStrategy.Reset();
                    query = connectionStrategy.Execute(() => query
                        .Where(q =>
                            (!string.IsNullOrEmpty(q.CedingBenefitRiskCode) && q.CedingBenefitRiskCode.Trim() == riData.CedingBenefitRiskCode.Trim())
                            ||
                            q.CedingBenefitRiskCode == null
                        ));
                }
                else
                {
                    connectionStrategy.Reset();
                    query = connectionStrategy.Execute(() => query
                        .Where(q => q.CedingBenefitRiskCode == null));
                }

                if (riData.InsuredAttainedAge != null)
                {
                    connectionStrategy.Reset();
                    query = connectionStrategy.Execute(() => query
                        .Where(q =>
                            (
                                (
                                    q.TreatyBenefitCodeMappingBo.AttainedAgeFrom <= riData.InsuredAttainedAge && q.TreatyBenefitCodeMappingBo.AttainedAgeTo >= riData.InsuredAttainedAge
                                    ||
                                    q.TreatyBenefitCodeMappingBo.AttainedAgeFrom <= riData.InsuredAttainedAge && q.TreatyBenefitCodeMappingBo.AttainedAgeTo >= riData.InsuredAttainedAge
                                )
                                ||
                                (q.TreatyBenefitCodeMappingBo.AttainedAgeFrom == null && q.TreatyBenefitCodeMappingBo.AttainedAgeTo == null)
                            )
                        ));
                }
                else
                {
                    connectionStrategy.Reset();
                    query = connectionStrategy.Execute(() => query
                        .Where(q => q.TreatyBenefitCodeMappingBo.AttainedAgeFrom == null && q.TreatyBenefitCodeMappingBo.AttainedAgeTo == null));
                }

                if (reportDate != null)
                {
                    connectionStrategy.Reset();
                    query = connectionStrategy.Execute(() => query
                        .Where(q =>
                            (q.TreatyBenefitCodeMappingBo.ReportingStartDate.HasValue && q.TreatyBenefitCodeMappingBo.ReportingEndDate.HasValue &&
                            (
                                q.TreatyBenefitCodeMappingBo.ReportingStartDate.Value.Date <= reportDate.Value.Date
                                && q.TreatyBenefitCodeMappingBo.ReportingEndDate.Value.Date >= reportDate.Value.Date
                                ||
                                q.TreatyBenefitCodeMappingBo.ReportingStartDate.Value.Date <= reportDate.Value.Date
                                && q.TreatyBenefitCodeMappingBo.ReportingEndDate.Value.Date >= reportDate.Value.Date
                            ))
                            ||
                            (q.TreatyBenefitCodeMappingBo.ReportingStartDate == null && q.TreatyBenefitCodeMappingBo.ReportingEndDate == null)
                        ));
                }
                else
                {
                    connectionStrategy.Reset();
                    query = connectionStrategy.Execute(() => query
                        .Where(q => q.TreatyBenefitCodeMappingBo.ReportingStartDate == null && q.TreatyBenefitCodeMappingBo.ReportingEndDate == null));
                }

                if (!string.IsNullOrEmpty(riData.TreatyCode))
                {
                    connectionStrategy.Reset();
                    query = connectionStrategy.Execute(() => query
                        .Where(q => q.TreatyBenefitCodeMappingBo.TreatyCodeBo.Code.Trim() == riData.TreatyCode.Trim()));
                }
                else
                {
                    connectionStrategy.Reset();
                    query = connectionStrategy.Execute(() => query
                        .Where(q => q.TreatyBenefitCodeMappingBo.TreatyCode == null));
                }

                if (!string.IsNullOrEmpty(riData.CedingTreatyCode))
                {
                    connectionStrategy.Reset();
                    query = connectionStrategy.Execute(() => query
                        .Where(q =>
                            (!string.IsNullOrEmpty(q.CedingTreatyCode) && q.CedingTreatyCode.Trim() == riData.CedingTreatyCode.Trim())
                            ||
                            q.CedingTreatyCode == null
                        ));
                }
                else
                {
                    connectionStrategy.Reset();
                    query = connectionStrategy.Execute(() => query
                        .Where(q => q.CedingTreatyCode == null));
                }

                if (!string.IsNullOrEmpty(riData.CampaignCode))
                {
                    connectionStrategy.Reset();
                    query = connectionStrategy.Execute(() => query
                        .Where(q =>
                            (!string.IsNullOrEmpty(q.CampaignCode) && q.CampaignCode.Trim() == riData.CampaignCode.Trim())
                            ||
                            q.CampaignCode == null
                        ));
                }
                else
                {
                    connectionStrategy.Reset();
                    query = connectionStrategy.Execute(() => query
                        .Where(q => q.CampaignCode == null));
                }

                int? reinsBasisCodeId = GetReinsBasisCodeId(riData);
                if (reinsBasisCodeId != null)
                {
                    connectionStrategy.Reset();
                    query = connectionStrategy.Execute(() => query
                        .Where(q =>
                            (q.TreatyBenefitCodeMappingBo.ReinsBasisCodePickListDetailId != null && q.TreatyBenefitCodeMappingBo.ReinsBasisCodePickListDetailId == reinsBasisCodeId)
                            ||
                            q.TreatyBenefitCodeMappingBo.ReinsBasisCodePickListDetailId == null
                        ));
                }
                else
                {
                    connectionStrategy.Reset();
                    query = connectionStrategy.Execute(() => query
                        .Where(q => q.TreatyBenefitCodeMappingBo.ReinsBasisCodePickListDetailId == null));
                }

                if (riData.UnderwriterRating.HasValue)
                {
                    connectionStrategy.Reset();
                    query = connectionStrategy.Execute(() => query
                        .Where(q =>
                            (
                                q.TreatyBenefitCodeMappingBo.UnderwriterRatingFrom <= riData.UnderwriterRating && q.TreatyBenefitCodeMappingBo.UnderwriterRatingTo >= riData.UnderwriterRating
                                ||
                                q.TreatyBenefitCodeMappingBo.UnderwriterRatingFrom == null && q.TreatyBenefitCodeMappingBo.UnderwriterRatingTo == null
                            )
                        ));
                }
                else
                {
                    connectionStrategy.Reset();
                    query = connectionStrategy.Execute(() => query
                        .Where(q => q.TreatyBenefitCodeMappingBo.UnderwriterRatingFrom == null && q.TreatyBenefitCodeMappingBo.UnderwriterRatingTo == null));
                }

                if (riData.OriSumAssured.HasValue)
                {
                    connectionStrategy.Reset();
                    query = connectionStrategy.Execute(() => query
                        .Where(q =>
                            (
                                q.TreatyBenefitCodeMappingBo.OriSumAssuredFrom <= riData.OriSumAssured && q.TreatyBenefitCodeMappingBo.OriSumAssuredTo >= riData.OriSumAssured
                                ||
                                q.TreatyBenefitCodeMappingBo.OriSumAssuredFrom == null && q.TreatyBenefitCodeMappingBo.OriSumAssuredTo == null
                            )
                        ));
                }
                else
                {
                    connectionStrategy.Reset();
                    query = connectionStrategy.Execute(() => query
                        .Where(q => q.TreatyBenefitCodeMappingBo.UnderwriterRatingFrom == null && q.TreatyBenefitCodeMappingBo.OriSumAssuredTo == null));
                }

                if (riData.ReinsuranceIssueAge != null)
                {
                    connectionStrategy.Reset();
                    query = connectionStrategy.Execute(() => query
                        .Where(q =>
                            (
                                (
                                    q.TreatyBenefitCodeMappingBo.ReinsuranceIssueAgeFrom <= riData.ReinsuranceIssueAge && q.TreatyBenefitCodeMappingBo.ReinsuranceIssueAgeTo >= riData.ReinsuranceIssueAge
                                    ||
                                    q.TreatyBenefitCodeMappingBo.ReinsuranceIssueAgeFrom <= riData.ReinsuranceIssueAge && q.TreatyBenefitCodeMappingBo.ReinsuranceIssueAgeTo >= riData.ReinsuranceIssueAge
                                )
                                ||
                                (q.TreatyBenefitCodeMappingBo.ReinsuranceIssueAgeFrom == null && q.TreatyBenefitCodeMappingBo.ReinsuranceIssueAgeTo == null)
                            )
                        ));
                }
                else
                {
                    connectionStrategy.Reset();
                    query = connectionStrategy.Execute(() => query
                        .Where(q => q.TreatyBenefitCodeMappingBo.ReinsuranceIssueAgeFrom == null && q.TreatyBenefitCodeMappingBo.ReinsuranceIssueAgeTo == null));
                }

                // NOTE: Group by should put at the end of query
                if (groupById)
                {
                    connectionStrategy.Reset();
                    query = connectionStrategy.Execute(() => query.GroupBy(q => q.TreatyBenefitCodeMappingId).Select(q => q.FirstOrDefault()));
                }

                return query.FirstOrDefault();
            }
        }

        public TreatyBenefitCodeMappingDetailBo FindByProductFeatureParams(
            int cedantId,
            RiDataBo riData,
            DateTime? reportDate = null,
            bool groupById = false)
        {
            lock (_listLock)
            {
                var query = TreatyBenefitCodeMappingDetailBosForProductFeatureMapping
                    .Where(q => q.Type == TreatyBenefitCodeMappingDetailBo.CombinationTypeProductFeature) // IMPORTANT
                    .Where(q => q.TreatyBenefitCodeMappingBo.CedantId == cedantId)
                    .Where(q => q.CedingPlanCode.Trim() == riData.CedingPlanCode.Trim())
                    .Where(q => q.TreatyBenefitCodeMappingBo.TreatyCodeBo.Code.Trim() == riData.TreatyCode.Trim())
                    .Where(q => q.TreatyBenefitCodeMappingBo.BenefitBo.Code.Trim() == riData.MlreBenefitCode.Trim())
                    .Where(q => q.CedingBenefitTypeCode.Trim() == riData.CedingBenefitTypeCode.Trim());

                if (riData.ReinsEffDatePol.HasValue)
                {
                    query = query
                        .Where(q =>
                            (q.TreatyBenefitCodeMappingBo.ReinsEffDatePolStartDate.HasValue && q.TreatyBenefitCodeMappingBo.ReinsEffDatePolEndDate.HasValue &&
                            (
                                q.TreatyBenefitCodeMappingBo.ReinsEffDatePolStartDate.Value.Date <= riData.ReinsEffDatePol.Value.Date
                                && q.TreatyBenefitCodeMappingBo.ReinsEffDatePolEndDate.Value.Date >= riData.ReinsEffDatePol.Value.Date
                            ))
                            ||
                            q.TreatyBenefitCodeMappingBo.ReinsEffDatePolStartDate == null && q.TreatyBenefitCodeMappingBo.ReinsEffDatePolEndDate == null
                        );
                }
                else
                {
                    query = query
                        .Where(q => q.TreatyBenefitCodeMappingBo.ReinsEffDatePolStartDate == null && q.TreatyBenefitCodeMappingBo.ReinsEffDatePolEndDate == null);
                }

                if (riData.InsuredAttainedAge.HasValue)
                {
                    query = query
                        .Where(q =>
                            (
                                q.TreatyBenefitCodeMappingBo.AttainedAgeFrom <= riData.InsuredAttainedAge && q.TreatyBenefitCodeMappingBo.AttainedAgeTo >= riData.InsuredAttainedAge
                                ||
                                q.TreatyBenefitCodeMappingBo.AttainedAgeFrom == null && q.TreatyBenefitCodeMappingBo.AttainedAgeTo == null
                            )
                        );
                }
                else
                {
                    query = query
                        .Where(q => q.TreatyBenefitCodeMappingBo.AttainedAgeFrom == null && q.TreatyBenefitCodeMappingBo.AttainedAgeTo == null);
                }

                if (!string.IsNullOrEmpty(riData.CampaignCode))
                {
                    query = query
                        .Where(q =>
                            (!string.IsNullOrEmpty(q.CampaignCode) && q.CampaignCode.Trim() == riData.CampaignCode.Trim())
                            ||
                            q.CampaignCode == null
                        );
                }
                else
                {
                    query = query
                        .Where(q => q.CampaignCode == null);
                }

                if (riData.UnderwriterRating.HasValue)
                {
                    query = query
                        .Where(q =>
                            (
                                q.TreatyBenefitCodeMappingBo.UnderwriterRatingFrom <= riData.UnderwriterRating && q.TreatyBenefitCodeMappingBo.UnderwriterRatingTo >= riData.UnderwriterRating
                                ||
                                q.TreatyBenefitCodeMappingBo.UnderwriterRatingFrom == null && q.TreatyBenefitCodeMappingBo.UnderwriterRatingTo == null
                            )
                        );
                }
                else
                {
                    query = query
                        .Where(q => q.TreatyBenefitCodeMappingBo.UnderwriterRatingFrom == null && q.TreatyBenefitCodeMappingBo.UnderwriterRatingTo == null);
                }

                if (riData.OriSumAssured.HasValue)
                {
                    query = query
                        .Where(q =>
                            (
                                q.TreatyBenefitCodeMappingBo.OriSumAssuredFrom <= riData.OriSumAssured && q.TreatyBenefitCodeMappingBo.OriSumAssuredTo >= riData.OriSumAssured
                                ||
                                q.TreatyBenefitCodeMappingBo.OriSumAssuredFrom == null && q.TreatyBenefitCodeMappingBo.OriSumAssuredTo == null
                            )
                        );
                }
                else
                {
                    query = query
                        .Where(q => q.TreatyBenefitCodeMappingBo.UnderwriterRatingFrom == null && q.TreatyBenefitCodeMappingBo.OriSumAssuredTo == null);
                }

                if (riData.ReinsuranceIssueAge != null)
                {
                    query = query
                        .Where(q =>
                            (
                                (
                                    q.TreatyBenefitCodeMappingBo.ReinsuranceIssueAgeFrom <= riData.ReinsuranceIssueAge && q.TreatyBenefitCodeMappingBo.ReinsuranceIssueAgeTo >= riData.ReinsuranceIssueAge
                                    ||
                                    q.TreatyBenefitCodeMappingBo.ReinsuranceIssueAgeFrom <= riData.ReinsuranceIssueAge && q.TreatyBenefitCodeMappingBo.ReinsuranceIssueAgeTo >= riData.ReinsuranceIssueAge
                                )
                                ||
                                (q.TreatyBenefitCodeMappingBo.ReinsuranceIssueAgeFrom == null && q.TreatyBenefitCodeMappingBo.ReinsuranceIssueAgeTo == null)
                            )
                        );
                }
                else
                {
                    query = query
                        .Where(q => q.TreatyBenefitCodeMappingBo.ReinsuranceIssueAgeFrom == null && q.TreatyBenefitCodeMappingBo.ReinsuranceIssueAgeTo == null);
                }

                if (reportDate != null)
                {
                    query = query
                        .Where(q =>
                            (q.TreatyBenefitCodeMappingBo.ReportingStartDate.HasValue && q.TreatyBenefitCodeMappingBo.ReportingEndDate.HasValue &&
                            (
                                q.TreatyBenefitCodeMappingBo.ReportingStartDate.Value.Date <= reportDate.Value.Date
                                && q.TreatyBenefitCodeMappingBo.ReportingEndDate.Value.Date >= reportDate.Value.Date
                                ||
                                q.TreatyBenefitCodeMappingBo.ReportingStartDate.Value.Date <= reportDate.Value.Date
                                && q.TreatyBenefitCodeMappingBo.ReportingEndDate.Value.Date >= reportDate.Value.Date
                            ))
                            ||
                            (q.TreatyBenefitCodeMappingBo.ReportingStartDate == null && q.TreatyBenefitCodeMappingBo.ReportingEndDate == null)
                        );
                }
                else
                {
                    query = query
                        .Where(q => q.TreatyBenefitCodeMappingBo.ReportingStartDate == null && q.TreatyBenefitCodeMappingBo.ReportingEndDate == null);
                }

                if (!string.IsNullOrEmpty(riData.CedingBenefitRiskCode))
                {
                    query = query
                        .Where(q =>
                            (!string.IsNullOrEmpty(q.CedingBenefitRiskCode) && q.CedingBenefitRiskCode.Trim() == riData.CedingBenefitRiskCode.Trim())
                            ||
                            q.CedingBenefitRiskCode == null
                        );
                }
                else
                {
                    query = query
                        .Where(q => q.CedingBenefitRiskCode == null);
                }

                if (!string.IsNullOrEmpty(riData.CedingTreatyCode))
                {
                    query = query
                        .Where(q =>
                            (!string.IsNullOrEmpty(q.CedingTreatyCode) && q.CedingTreatyCode.Trim() == riData.CedingTreatyCode.Trim())
                            ||
                            q.CedingTreatyCode == null
                        );
                }
                else
                {
                    query = query
                        .Where(q => q.CedingTreatyCode == null);
                }

                int? reinsBasisCodeId = GetReinsBasisCodeId(riData);
                if (reinsBasisCodeId != null)
                {
                    query = query
                        .Where(q =>
                            (q.TreatyBenefitCodeMappingBo.ReinsBasisCodePickListDetailId != null && q.TreatyBenefitCodeMappingBo.ReinsBasisCodePickListDetailId == reinsBasisCodeId)
                            ||
                            q.TreatyBenefitCodeMappingBo.ReinsBasisCodePickListDetailId == null
                        );
                }
                else
                {
                    query = query
                        .Where(q => q.TreatyBenefitCodeMappingBo.ReinsBasisCodePickListDetailId == null);
                }

                // NOTE: Group by should put at the end of query
                if (groupById)
                {
                    query = query.GroupBy(q => q.TreatyBenefitCodeMappingId).Select(q => q.FirstOrDefault());
                }

                return query.FirstOrDefault();
            }
        }

        public Mfrs17CellMappingDetailBo FindByCellMappingParams(
            RiDataBo riData,
            bool groupById = false)
        {
            lock (_listLock)
            {
                int? reinsBasisCodeId = GetReinsBasisCodeId(riData);
                var query = Mfrs17CellMappingDetailBosForCellMapping
                    .Where(q => q.TreatyCode.Trim() == riData.TreatyCode.Trim())
                    .Where(q => q.Mfrs17CellMappingBo.ProfitCommPickListDetailId.HasValue && q.Mfrs17CellMappingBo.ProfitCommPickListDetailBo.Code.Trim() == riData.ProfitComm.Trim())
                    .Where(q => q.Mfrs17CellMappingBo.ReinsBasisCodePickListDetailId == reinsBasisCodeId);

                if (!string.IsNullOrEmpty(riData.CedingPlanCode))
                {
                    query = query
                        .Where(q => (q.CedingPlanCode != null && q.CedingPlanCode.Trim() == riData.CedingPlanCode.Trim()) || q.CedingPlanCode == null);
                }
                else
                {
                    query = query
                        .Where(q => q.CedingPlanCode == null);
                }

                if (!string.IsNullOrEmpty(riData.MlreBenefitCode))
                {
                    query = query
                        .Where(q => (q.BenefitCode != null && q.BenefitCode.Trim() == riData.MlreBenefitCode.Trim()) || q.BenefitCode == null);
                }
                else
                {
                    query = query
                        .Where(q => q.BenefitCode == null);
                }

                if (riData.ReinsEffDatePol.HasValue)
                {
                    query = query
                        .Where(q =>
                            (q.Mfrs17CellMappingBo.ReinsEffDatePolStartDate.HasValue && q.Mfrs17CellMappingBo.ReinsEffDatePolEndDate.HasValue &&
                            (
                                q.Mfrs17CellMappingBo.ReinsEffDatePolStartDate.Value.Date <= riData.ReinsEffDatePol.Value.Date
                                && q.Mfrs17CellMappingBo.ReinsEffDatePolEndDate.Value.Date >= riData.ReinsEffDatePol.Value.Date
                            ))
                            ||
                            q.Mfrs17CellMappingBo.ReinsEffDatePolStartDate == null && q.Mfrs17CellMappingBo.ReinsEffDatePolEndDate == null
                        );
                }
                else
                {
                    query = query
                        .Where(q => q.Mfrs17CellMappingBo.ReinsEffDatePolStartDate == null && q.Mfrs17CellMappingBo.ReinsEffDatePolEndDate == null);
                }

                if (!string.IsNullOrEmpty(riData.RateTable))
                {
                    query = query
                        .Where(q => (q.Mfrs17CellMappingBo.RateTable != null && q.Mfrs17CellMappingBo.RateTable.Trim() == riData.RateTable.Trim()) || q.Mfrs17CellMappingBo.RateTable == null);
                }
                else
                {
                    query = query
                        .Where(q => q.Mfrs17CellMappingBo.RateTable == null);
                }

                // NOTE: Group by should put at the end of query
                if (groupById)
                {
                    query = query.GroupBy(q => q.Mfrs17CellMappingId).Select(q => q.FirstOrDefault());
                }

                return query.FirstOrDefault();
            }
        }

        public RateTableDetailBo FindByRateTableMappingParams(
            RiDataBo riData,
            DateTime? reportDate,
            bool groupById = false)
        {
            lock (_listLock)
            {
                var query = RateTableDetailBosForRateTableMapping.Where(q => q.TreatyCode.Trim() == riData.TreatyCode.Trim());

                if (!string.IsNullOrEmpty(riData.CedingPlanCode))
                {
                    query = query
                        .Where(q => (!string.IsNullOrEmpty(q.CedingPlanCode) && q.CedingPlanCode.Trim() == riData.CedingPlanCode.Trim()) || q.CedingPlanCode == null);
                }
                else
                {
                    query = query
                        .Where(q => q.CedingPlanCode == null);
                }

                if (!string.IsNullOrEmpty(riData.CedingTreatyCode))
                {
                    query = query
                        .Where(q => (!string.IsNullOrEmpty(q.CedingTreatyCode) && q.CedingTreatyCode.Trim() == riData.CedingTreatyCode.Trim()) || q.CedingTreatyCode == null);
                }
                else
                {
                    query = query
                        .Where(q => q.CedingTreatyCode == null);
                }

                if (!string.IsNullOrEmpty(riData.CedingPlanCode2))
                {
                    query = query
                        .Where(q => (!string.IsNullOrEmpty(q.CedingPlanCode2) && q.CedingPlanCode2.Trim() == riData.CedingPlanCode2.Trim()) || q.CedingPlanCode2 == null);
                }
                else
                {
                    query = query
                        .Where(q => q.CedingPlanCode2 == null);
                }

                if (!string.IsNullOrEmpty(riData.CedingBenefitTypeCode))
                {
                    query = query
                        .Where(q => (!string.IsNullOrEmpty(q.CedingBenefitTypeCode) && q.CedingBenefitTypeCode.Trim() == riData.CedingBenefitTypeCode.Trim()) || q.CedingBenefitTypeCode == null);
                }
                else
                {
                    query = query
                        .Where(q => q.CedingBenefitTypeCode == null);
                }

                if (!string.IsNullOrEmpty(riData.CedingBenefitRiskCode))
                {
                    query = query
                        .Where(q => (!string.IsNullOrEmpty(q.CedingBenefitRiskCode) && q.CedingBenefitRiskCode.Trim() == riData.CedingBenefitRiskCode.Trim()) || q.CedingBenefitRiskCode == null);
                }
                else
                {
                    query = query
                        .Where(q => q.CedingBenefitRiskCode == null);
                }

                if (!string.IsNullOrEmpty(riData.GroupPolicyNumber))
                {
                    query = query
                        .Where(q => (!string.IsNullOrEmpty(q.GroupPolicyNumber) && q.GroupPolicyNumber.Trim() == riData.GroupPolicyNumber.Trim()) || q.GroupPolicyNumber == null);
                }
                else
                {
                    query = query
                        .Where(q => q.GroupPolicyNumber == null);
                }

                if (!string.IsNullOrEmpty(riData.MlreBenefitCode))
                {
                    query = query.Where(q => (q.RateTableBo.BenefitBo != null && q.RateTableBo.BenefitBo.Code.Trim() == riData.MlreBenefitCode.Trim()) || q.RateTableBo.BenefitBo == null);
                }
                else
                {
                    query = query.Where(q => q.RateTableBo.BenefitBo == null);
                }

                int? reinsBasisCodeId = GetReinsBasisCodeId(riData);
                if (reinsBasisCodeId != null)
                {
                    query = query
                        .Where(q => q.RateTableBo.ReinsBasisCodePickListDetailId == reinsBasisCodeId || q.RateTableBo.ReinsBasisCodePickListDetailId == null);
                }
                else
                {
                    query = query.Where(q => q.RateTableBo.ReinsBasisCodePickListDetailId == null);
                }

                int? premiumFrequencyCodeId = GetPremiumFrequencyCodeId(riData);
                if (premiumFrequencyCodeId != null)
                {
                    query = query.Where(q => q.RateTableBo.PremiumFrequencyCodePickListDetailId == premiumFrequencyCodeId || q.RateTableBo.PremiumFrequencyCodePickListDetailId == null);
                }
                else
                {
                    query = query.Where(q => q.RateTableBo.PremiumFrequencyCodePickListDetailId == null);
                }

                if (riData.InsuredAttainedAge != null)
                {
                    query = query
                        .Where(q =>
                            (
                                q.RateTableBo.AttainedAgeFrom <= riData.InsuredAttainedAge && q.RateTableBo.AttainedAgeTo >= riData.InsuredAttainedAge
                                ||
                                q.RateTableBo.AttainedAgeFrom <= riData.InsuredAttainedAge && q.RateTableBo.AttainedAgeTo >= riData.InsuredAttainedAge
                            )
                            || (q.RateTableBo.AttainedAgeFrom == null && q.RateTableBo.AttainedAgeTo == null)
                        );
                }
                else
                {
                    query = query
                        .Where(q => q.RateTableBo.AttainedAgeFrom == null && q.RateTableBo.AttainedAgeTo == null);
                }

                if (riData.OriSumAssured != null)
                {
                    query = query
                        .Where(q =>
                            (
                                q.RateTableBo.PolicyAmountFrom <= riData.OriSumAssured && q.RateTableBo.PolicyAmountTo >= riData.OriSumAssured
                                ||
                                q.RateTableBo.PolicyAmountFrom <= riData.OriSumAssured && q.RateTableBo.PolicyAmountTo >= riData.OriSumAssured
                            )
                            || (q.RateTableBo.PolicyAmountFrom == null && q.RateTableBo.PolicyAmountTo == null)
                        );
                }
                else
                {
                    query = query
                        .Where(q => q.RateTableBo.PolicyAmountFrom == null && q.RateTableBo.PolicyAmountTo == null);
                }

                if (riData.Aar != null)
                {
                    query = query
                        .Where(q =>
                            (
                                q.RateTableBo.AarFrom <= riData.Aar && q.RateTableBo.AarTo >= riData.Aar
                                ||
                                q.RateTableBo.AarFrom <= riData.Aar && q.RateTableBo.AarTo >= riData.Aar
                            )
                            || (q.RateTableBo.AarFrom == null && q.RateTableBo.AarTo == null)
                        );
                }
                else
                {
                    query = query
                        .Where(q => q.RateTableBo.AarFrom == null && q.RateTableBo.AarTo == null);
                }

                if (riData.ReinsEffDatePol.HasValue)
                {
                    query = query
                        .Where(q =>
                            (q.RateTableBo.ReinsEffDatePolStartDate.HasValue && q.RateTableBo.ReinsEffDatePolEndDate.HasValue &&
                            (
                                q.RateTableBo.ReinsEffDatePolStartDate.Value.Date <= riData.ReinsEffDatePol.Value.Date
                                && q.RateTableBo.ReinsEffDatePolEndDate.Value.Date >= riData.ReinsEffDatePol.Value.Date
                            ))
                            ||
                            q.RateTableBo.ReinsEffDatePolStartDate == null && q.RateTableBo.ReinsEffDatePolEndDate == null
                        );
                }
                else
                {
                    query = query
                        .Where(q => q.RateTableBo.ReinsEffDatePolStartDate == null && q.RateTableBo.ReinsEffDatePolEndDate == null);
                }

                if (reportDate != null)
                {
                    query = query
                        .Where(q =>
                            (q.RateTableBo.ReportingStartDate.HasValue && q.RateTableBo.ReportingEndDate.HasValue &&
                            (
                                q.RateTableBo.ReportingStartDate.Value.Date <= reportDate.Value.Date
                                && q.RateTableBo.ReportingEndDate.Value.Date >= reportDate.Value.Date
                                ||
                                q.RateTableBo.ReportingStartDate.Value.Date <= reportDate.Value.Date
                                && q.RateTableBo.ReportingEndDate.Value.Date >= reportDate.Value.Date
                            ))
                            ||
                            (q.RateTableBo.ReportingStartDate == null && q.RateTableBo.ReportingEndDate == null)
                        );
                }
                else
                {
                    query = query
                        .Where(q => q.RateTableBo.ReportingStartDate == null && q.RateTableBo.ReportingEndDate == null);
                }

                if (riData.PolicyTerm != null)
                {
                    query = query
                        .Where(q =>
                            (
                                q.RateTableBo.PolicyTermFrom <= riData.PolicyTerm && q.RateTableBo.PolicyTermTo >= riData.PolicyTerm
                                ||
                                q.RateTableBo.PolicyTermFrom <= riData.PolicyTerm && q.RateTableBo.PolicyTermTo >= riData.PolicyTerm
                            )
                            || (q.RateTableBo.PolicyTermFrom == null && q.RateTableBo.PolicyTermTo == null)
                        );
                }
                else
                {
                    query = query
                        .Where(q => q.RateTableBo.PolicyTermFrom == null && q.RateTableBo.PolicyTermTo == null);
                }

                if (riData.DurationMonth != null)
                {
                    query = query
                        .Where(q =>
                            (
                                q.RateTableBo.PolicyDurationFrom <= riData.DurationMonth && q.RateTableBo.PolicyDurationTo >= riData.DurationMonth
                                ||
                                q.RateTableBo.PolicyDurationFrom <= riData.DurationMonth && q.RateTableBo.PolicyDurationTo >= riData.DurationMonth
                            )
                            || (q.RateTableBo.PolicyDurationFrom == null && q.RateTableBo.PolicyDurationTo == null)
                        );
                }
                else
                {
                    query = query
                        .Where(q => q.RateTableBo.PolicyDurationFrom == null && q.RateTableBo.PolicyDurationTo == null);
                }

                // NOTE: Group by should put at the end of query
                if (groupById)
                {
                    query = query.GroupBy(q => q.RateTableId).Select(q => q.FirstOrDefault());
                }

                return query.FirstOrDefault();
            }
        }

        public AnnuityFactorMappingBo FindByAnnuityFactorMappingParams(
            RiDataBo riData,
            bool groupById = false)
        {
            lock (_listLock)
            {
                var query = AnnuityFactorMappingBosForAnnuityFactorMapping.Where(q => (!string.IsNullOrEmpty(q.CedingPlanCode) && q.CedingPlanCode.Trim() == riData.CedingPlanCode.Trim()) || q.CedingPlanCode == null);

                if (riData.ReinsEffDatePol.HasValue)
                {
                    query = query
                        .Where(q =>
                            (q.AnnuityFactorBo.ReinsEffDatePolStartDate.HasValue && q.AnnuityFactorBo.ReinsEffDatePolEndDate.HasValue &&
                            (
                                q.AnnuityFactorBo.ReinsEffDatePolStartDate.Value.Date <= riData.ReinsEffDatePol.Value.Date
                                && q.AnnuityFactorBo.ReinsEffDatePolEndDate.Value.Date >= riData.ReinsEffDatePol.Value.Date
                            ))
                            ||
                            q.AnnuityFactorBo.ReinsEffDatePolStartDate == null && q.AnnuityFactorBo.ReinsEffDatePolEndDate == null
                        );
                }
                else
                {
                    query = query
                        .Where(q => q.AnnuityFactorBo.ReinsEffDatePolStartDate == null && q.AnnuityFactorBo.ReinsEffDatePolEndDate == null);
                }

                // NOTE: Group by should put at the end of query
                if (groupById)
                {
                    query = query.GroupBy(q => q.AnnuityFactorId).Select(q => q.FirstOrDefault());
                }

                return query.FirstOrDefault();
            }
        }

        public FacMasterListingDetailBo FindByFacMappingParams(
            RiDataBo riData,
            bool groupById = false)
        {
            lock (_listLock)
            {
                var query = FacMasterListingDetailBosForFacMapping
                    .Where(q => q.FacMasterListingBo.InsuredName.Trim() == riData.InsuredName.Trim())
                    .Where(q => q.PolicyNumber.Trim() == riData.PolicyNumber.Trim())
                    .Where(q => q.BenefitCode.Trim() == riData.MlreBenefitCode.Trim());

                if (!string.IsNullOrEmpty(riData.CedingBenefitTypeCode))
                {
                    query = query
                        .Where(q =>
                            (!string.IsNullOrEmpty(q.CedingBenefitTypeCode) && q.CedingBenefitTypeCode.Trim() == riData.CedingBenefitTypeCode.Trim())
                            ||
                            q.CedingBenefitTypeCode == null
                        );
                }
                else
                {
                    query = query
                        .Where(q => q.CedingBenefitTypeCode == null);
                }

                // NOTE: Group by should put at the end of query
                if (groupById)
                {
                    query = query.GroupBy(q => q.FacMasterListingId).Select(q => q.FirstOrDefault());
                }

                return query.FirstOrDefault();
            }
        }

        public TreatyCodeBo FindByTreatyNumberTreatyTypeParams(string treatyCode)
        {
            lock (_listLock)
            {
                var query = TreatyCodeBosForTreatyNumberTreatyTypeMapping
                    .Where(q => q.Code.Trim() == treatyCode.Trim());

                return query.FirstOrDefault();
            }
        }
    }
}
