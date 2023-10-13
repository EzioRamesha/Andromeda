using BusinessObject.Identity;
using Shared.ProcessFile;
using System.Collections.Generic;

namespace BusinessObject
{
    public class PickListBo
    {
        public int Id { get; set; }

        public int DepartmentId { get; set; }

        public DepartmentBo DepartmentBo { get; set; }

        public int? StandardOutputId { get; set; }
        public StandardOutputBo StandardOutputBo { get; set; }

        public int? StandardClaimDataOutputId { get; set; }
        public StandardClaimDataOutputBo StandardClaimDataOutputBo { get; set; }

        public int? StandardSoaDataOutputId { get; set; }
        public StandardSoaDataOutputBo StandardSoaDataOutputIBo { get; set; }

        public string FieldName { get; set; }

        public bool IsEditable { get; set; } = true;

        public List<int> UsedByDepartments { get; set; }

        public int CreatedById { get; set; }

        public int? UpdatedById { get; set; }

        public const int ProductCategory = 1;
        public const int ProductType = 2;
        public const int DistributionChannel = 3;
        public const int CessionType = 4;
        public const int BusinessOrigin = 5;
        public const int BusinessType = 6;
        public const int ProductLine = 7;
        public const int RiArrangement = 8;
        public const int ReinsBasisCode = 9;
        public const int PremiumFrequencyCode = 10;
        public const int TransactionTypeCode = 11;
        public const int CedingBenefitTypeCode = 12;
        public const int InsuredGenderCode = 13;
        public const int InsuredTobaccoUse = 14;
        public const int InsuredGenderCode2nd = 15;
        public const int InsuredTobaccoUse2nd = 16;
        public const int InsuredOccupationCode = 17;
        public const int PolicyStatusCode = 18;
        public const int PolicyPaymentMethod = 19;
        public const int DependantIndicator = 20;
        public const int SpecialIndicator1 = 21;
        public const int SpecialIndicator2 = 22;
        public const int SpecialIndicator3 = 23;
        public const int FundsAccountingType = 24;
        public const int Mfrs17BasicRider = 25;
        public const int FundCode = 26;
        public const int LineOfBusiness = 27;
        public const int GstIndicator = 28;
        public const int StaffPlanIndicator = 29;
        public const int IndicatorJointLife = 30;
        public const int TerritoryOfIssueCode = 31;
        public const int CurrencyCode = 32;

        // Phase 2 new picklist
        public const int ValuationBenefitCode = 33;
        public const int CedingCompanyType = 34;
        public const int TreatyType = 35;
        public const int TreatyStatus = 36;
        //public const int CurrencyConversionRate = 37; // Not in use
        public const int CountryCode = 37;
        public const int StatementStatus = 38;
        public const int RecordType = 39;
        public const int ClaimTransactionType = 40;
        public const int InvoiceField = 41;
        public const int ProfitComm = 42;
        public const int BenefitCategory = 43;
        public const int RetroRegisterField = 44;

        // Phase 2a new picklist
        public const int ReinsuranceType = 45;
        public const int AgeBasis = 46;
        public const int RateGuarantee = 47;
        public const int UnearnedPremiumRefund = 48;
        public const int AgeDefinition = 49;
        public const int DistributionTier = 50;
        public const int UnderwritingMethod = 51;
        public const int TargetSegment = 52;
        public const int PayoutType = 53;
        public const int ArrangementReinsuranceType = 54;
        public const int RiskPatternSum = 55;
        public const int CampaignType = 56;
        public const int PricingTeam = 57;
        public const int PaymentRetrocessionairePremium = 58;
        public const int ArrangementRetrocessionnaireType = 59;
        public const int IndustryName = 60;
        public const int ReferredType = 61;
        public const int RequestType = 62;
        public const int PremiumType = 63;
        public const int TableType = 64;
        public const int Entitlement = 65;
        public const int ExceptionStatus = 66;
        public const int RetroPremiumFrequencyMode = 67;

        public const int ColumnDepartmentCode = 1;
        public const int ColumnFieldName = 2;
        public const int ColumnPickListDetailCode = 3;
        public const int ColumnPickListDetailDescription = 4;

        public static IList<PickListBo> GetPickLists()
        {
            return new List<PickListBo>() {
                GetStaticPickList(ProductCategory),
                GetStaticPickList(ProductType),
                GetStaticPickList(DistributionChannel),
                GetStaticPickList(CessionType),
                GetStaticPickList(BusinessOrigin),
                GetStaticPickList(BusinessType),
                GetStaticPickList(ProductLine),
                GetStaticPickList(RiArrangement),
                GetStaticPickList(ReinsBasisCode),
                GetStaticPickList(PremiumFrequencyCode),
                GetStaticPickList(TransactionTypeCode),
                GetStaticPickList(CedingBenefitTypeCode),
                GetStaticPickList(InsuredGenderCode),
                GetStaticPickList(InsuredTobaccoUse),
                GetStaticPickList(InsuredGenderCode2nd),
                GetStaticPickList(InsuredTobaccoUse2nd),
                GetStaticPickList(InsuredOccupationCode),
                GetStaticPickList(PolicyStatusCode),
                GetStaticPickList(PolicyPaymentMethod),
                GetStaticPickList(DependantIndicator),
                GetStaticPickList(SpecialIndicator1),
                GetStaticPickList(SpecialIndicator2),
                GetStaticPickList(SpecialIndicator3),
                GetStaticPickList(FundsAccountingType),
                GetStaticPickList(Mfrs17BasicRider),
                GetStaticPickList(FundCode),
                GetStaticPickList(LineOfBusiness),
                GetStaticPickList(GstIndicator),
                GetStaticPickList(StaffPlanIndicator),
                GetStaticPickList(IndicatorJointLife),
                GetStaticPickList(TerritoryOfIssueCode),
                GetStaticPickList(CurrencyCode),

                // Phase 2 new picklist
                GetStaticPickList(ValuationBenefitCode),
                GetStaticPickList(CedingCompanyType),
                GetStaticPickList(TreatyType),
                GetStaticPickList(TreatyStatus),
                GetStaticPickList(CountryCode),
                GetStaticPickList(StatementStatus),
                GetStaticPickList(RecordType),
                GetStaticPickList(ClaimTransactionType),
                GetStaticPickList(InvoiceField),
                GetStaticPickList(ProfitComm),
                GetStaticPickList(BenefitCategory),
                GetStaticPickList(RetroRegisterField),
                
                // Phase 2a new picklist
                GetStaticPickList(ReinsuranceType),
                GetStaticPickList(AgeBasis),
                GetStaticPickList(RateGuarantee),
                GetStaticPickList(UnearnedPremiumRefund),
                GetStaticPickList(AgeDefinition),
                GetStaticPickList(DistributionTier),
                GetStaticPickList(UnderwritingMethod),
                GetStaticPickList(TargetSegment),
                GetStaticPickList(PayoutType),
                GetStaticPickList(ArrangementReinsuranceType),
                GetStaticPickList(RiskPatternSum),
                GetStaticPickList(CampaignType),
                GetStaticPickList(PricingTeam),
                GetStaticPickList(PaymentRetrocessionairePremium),
                GetStaticPickList(ArrangementRetrocessionnaireType),
                GetStaticPickList(IndustryName),
                GetStaticPickList(ReferredType),
                GetStaticPickList(RequestType),
                GetStaticPickList(PremiumType),
                GetStaticPickList(TableType),
                GetStaticPickList(Entitlement),
                GetStaticPickList(ExceptionStatus),
                GetStaticPickList(RetroPremiumFrequencyMode),
            };
        }

        public static List<Column> GetColumns()
        {
            return new List<Column>
            {
                new Column
                {
                    Header = "Department",
                    ColIndex = ColumnDepartmentCode,
                    Property = "Department",
                },
                new Column
                {
                    Header = "Field Name",
                    ColIndex = ColumnFieldName,
                    Property = "FieldName",
                },
                new Column
                {
                    Header = "Code",
                    ColIndex = ColumnPickListDetailCode,
                    Property = "Code",
                },
                new Column
                {
                    Header = "Description",
                    ColIndex = ColumnPickListDetailDescription,
                    Property = "Description",
                },
            };
        }

        public static PickListBo GetStaticPickList(int key)
        {
            switch (key)
            {
                case ProductCategory:
                    return new PickListBo
                    {
                        Id = key,
                        DepartmentId = DepartmentBo.DepartmentShared,
                        FieldName = "Product Category",
                        UsedByDepartments = new List<int>()
                        {
                            DepartmentBo.DepartmentTreatyPricing
                        }
                    };
                case ProductType:
                    return new PickListBo
                    {
                        Id = key,
                        DepartmentId = DepartmentBo.DepartmentShared,
                        FieldName = "Product Type",
                        UsedByDepartments = new List<int>()
                        {
                            DepartmentBo.DepartmentTreatyPricing
                        }
                    };
                case DistributionChannel:
                    return new PickListBo
                    {
                        Id = key,
                        DepartmentId = DepartmentBo.DepartmentShared,
                        FieldName = "Distribution Channel",
                        UsedByDepartments = new List<int>()
                        {
                            DepartmentBo.DepartmentTreatyPricing
                        }
                    };
                case CessionType:
                    return new PickListBo
                    {
                        Id = key,
                        DepartmentId = DepartmentBo.DepartmentShared,
                        FieldName = "Cession Type",
                        UsedByDepartments = new List<int>()
                        {
                            DepartmentBo.DepartmentTreatyPricing
                        }
                    };
                case BusinessOrigin:
                    return new PickListBo
                    {
                        Id = key,
                        DepartmentId = DepartmentBo.DepartmentDataAnalyticsAdministration,
                        FieldName = "Business Origin",
                        IsEditable = false,
                        UsedByDepartments = new List<int>()
                        {
                            DepartmentBo.DepartmentTreatyPricing
                        }
                    };
                case BusinessType:
                    return new PickListBo
                    {
                        Id = key,
                        DepartmentId = DepartmentBo.DepartmentShared,
                        FieldName = "Business Type",
                        UsedByDepartments = new List<int>()
                        {
                            DepartmentBo.DepartmentTreatyPricing,
                            DepartmentBo.DepartmentRetrocession
                        }
                    };
                case ProductLine:
                    return new PickListBo
                    {
                        Id = key,
                        DepartmentId = DepartmentBo.DepartmentShared,
                        FieldName = "Product Line",
                        UsedByDepartments = new List<int>()
                        {
                            DepartmentBo.DepartmentTreatyPricing
                        }
                    };
                case RiArrangement:
                    return new PickListBo
                    {
                        Id = key,
                        DepartmentId = DepartmentBo.DepartmentShared,
                        FieldName = "RI Arrangement",
                        UsedByDepartments = new List<int>()
                        {
                            DepartmentBo.DepartmentTreatyPricing
                        }
                    };
                case ReinsBasisCode:
                    return new PickListBo
                    {
                        Id = key,
                        DepartmentId = DepartmentBo.DepartmentDataAnalyticsAdministration,
                        StandardOutputId = StandardOutputBo.TypeReinsBasisCode,
                        StandardClaimDataOutputId = StandardClaimDataOutputBo.TypeReinsBasisCode,
                        FieldName = "Reins Basic Code",
                        UsedByDepartments = new List<int>()
                        {
                            DepartmentBo.DepartmentClaim,
                            DepartmentBo.DepartmentDataAnalyticsAdministration,
                            DepartmentBo.DepartmentRetrocession,
                            DepartmentBo.DepartmentValuation
                        }
                    };
                case PremiumFrequencyCode:
                    return new PickListBo
                    {
                        Id = key,
                        DepartmentId = DepartmentBo.DepartmentShared,
                        StandardOutputId = StandardOutputBo.TypePremiumFrequencyCode,
                        FieldName = "Premium Frequency Code",
                        IsEditable = false,
                        UsedByDepartments = new List<int>()
                        {
                            DepartmentBo.DepartmentValuation,
                            DepartmentBo.DepartmentDataAnalyticsAdministration
                        }
                    };
                case TransactionTypeCode:
                    return new PickListBo
                    {
                        Id = key,
                        DepartmentId = DepartmentBo.DepartmentDataAnalyticsAdministration,
                        StandardOutputId = StandardOutputBo.TypeTransactionTypeCode,
                        FieldName = "Transaction Type Code",
                        IsEditable = false,
                        UsedByDepartments = new List<int>()
                        {
                            DepartmentBo.DepartmentDataAnalyticsAdministration
                        }
                    };
                case CedingBenefitTypeCode:
                    return new PickListBo
                    {
                        Id = key,
                        DepartmentId = DepartmentBo.DepartmentDataAnalyticsAdministration,
                        StandardOutputId = StandardOutputBo.TypeCedingBenefitTypeCode,
                        StandardClaimDataOutputId = StandardClaimDataOutputBo.TypeCedingBenefitTypeCode,
                        FieldName = "Ceding Benefit Type Code",
                        UsedByDepartments = new List<int>()
                        {
                            DepartmentBo.DepartmentDataAnalyticsAdministration
                        }
                    };
                case InsuredGenderCode:
                    return new PickListBo
                    {
                        Id = key,
                        DepartmentId = DepartmentBo.DepartmentDataAnalyticsAdministration,
                        StandardOutputId = StandardOutputBo.TypeInsuredGenderCode,
                        StandardClaimDataOutputId = StandardClaimDataOutputBo.TypeInsuredGenderCode,
                        FieldName = "Insured Gender Code",
                        UsedByDepartments = new List<int>()
                        {
                            DepartmentBo.DepartmentDataAnalyticsAdministration,
                            DepartmentBo.DepartmentClaim,
                            DepartmentBo.DepartmentRetrocession
                        }
                    };
                case InsuredTobaccoUse:
                    return new PickListBo
                    {
                        Id = key,
                        DepartmentId = DepartmentBo.DepartmentDataAnalyticsAdministration,
                        StandardOutputId = StandardOutputBo.TypeInsuredTobaccoUse,
                        StandardClaimDataOutputId = StandardClaimDataOutputBo.TypeInsuredTobaccoUse,
                        FieldName = "Insured Tobacco Use",
                        UsedByDepartments = new List<int>()
                        {
                            DepartmentBo.DepartmentDataAnalyticsAdministration,
                            DepartmentBo.DepartmentClaim,
                        }
                    };
                case InsuredGenderCode2nd:
                    return new PickListBo
                    {
                        Id = key,
                        DepartmentId = DepartmentBo.DepartmentDataAnalyticsAdministration,
                        StandardOutputId = StandardOutputBo.TypeInsuredGenderCode2nd,
                        FieldName = "Insured Gender Code 2nd",
                        UsedByDepartments = new List<int>()
                        {
                            DepartmentBo.DepartmentDataAnalyticsAdministration,
                            DepartmentBo.DepartmentClaim,
                            DepartmentBo.DepartmentRetrocession
                        }
                    };
                case InsuredTobaccoUse2nd:
                    return new PickListBo
                    {
                        Id = key,
                        DepartmentId = DepartmentBo.DepartmentDataAnalyticsAdministration,
                        StandardOutputId = StandardOutputBo.TypeInsuredTobaccoUse2nd,
                        FieldName = "Insured Tobacco Use 2nd",
                        UsedByDepartments = new List<int>()
                        {
                            DepartmentBo.DepartmentDataAnalyticsAdministration,
                            DepartmentBo.DepartmentClaim
                        }
                    };
                case InsuredOccupationCode:
                    return new PickListBo
                    {
                        Id = key,
                        DepartmentId = DepartmentBo.DepartmentDataAnalyticsAdministration,
                        StandardOutputId = StandardOutputBo.TypeInsuredOccupationCode,
                        FieldName = "Insured Occupation Code",
                        UsedByDepartments = new List<int>()
                        {
                            DepartmentBo.DepartmentDataAnalyticsAdministration
                        }
                    };
                case PolicyStatusCode:
                    return new PickListBo
                    {
                        Id = key,
                        DepartmentId = DepartmentBo.DepartmentDataAnalyticsAdministration,
                        StandardOutputId = StandardOutputBo.TypePolicyStatusCode,
                        FieldName = "Policy Status Code",
                        IsEditable = false,
                        UsedByDepartments = new List<int>()
                        {
                            DepartmentBo.DepartmentDataAnalyticsAdministration
                        }
                    };
                case PolicyPaymentMethod:
                    return new PickListBo
                    {
                        Id = key,
                        DepartmentId = DepartmentBo.DepartmentDataAnalyticsAdministration,
                        StandardOutputId = StandardOutputBo.TypePolicyPaymentMethod,
                        FieldName = "Policy Payment Method",
                        UsedByDepartments = new List<int>()
                        {
                            DepartmentBo.DepartmentDataAnalyticsAdministration,
                            DepartmentBo.DepartmentValuation
                        }
                    };
                case DependantIndicator:
                    return new PickListBo
                    {
                        Id = key,
                        DepartmentId = DepartmentBo.DepartmentDataAnalyticsAdministration,
                        StandardOutputId = StandardOutputBo.TypeDependantIndicator,
                        FieldName = "Dependant Indicator",
                        UsedByDepartments = new List<int>()
                        {
                            DepartmentBo.DepartmentDataAnalyticsAdministration,
                            DepartmentBo.DepartmentValuation
                        }
                    };
                case SpecialIndicator1:
                    return new PickListBo
                    {
                        Id = key,
                        DepartmentId = DepartmentBo.DepartmentDataAnalyticsAdministration,
                        StandardOutputId = StandardOutputBo.TypeSpecialIndicator1,
                        FieldName = "Special Indicator 1",
                        UsedByDepartments = new List<int>()
                        {
                            DepartmentBo.DepartmentDataAnalyticsAdministration,
                            DepartmentBo.DepartmentValuation
                        }
                    };
                case SpecialIndicator2:
                    return new PickListBo
                    {
                        Id = key,
                        DepartmentId = DepartmentBo.DepartmentDataAnalyticsAdministration,
                        StandardOutputId = StandardOutputBo.TypeSpecialIndicator2,
                        FieldName = "Special Indicator 2",
                        UsedByDepartments = new List<int>()
                        {
                            DepartmentBo.DepartmentDataAnalyticsAdministration,
                            DepartmentBo.DepartmentValuation
                        }
                    };
                case SpecialIndicator3:
                    return new PickListBo
                    {
                        Id = key,
                        DepartmentId = DepartmentBo.DepartmentDataAnalyticsAdministration,
                        StandardOutputId = StandardOutputBo.TypeSpecialIndicator3,
                        FieldName = "Special Indicator 3",
                        UsedByDepartments = new List<int>()
                        {
                            DepartmentBo.DepartmentDataAnalyticsAdministration,
                            DepartmentBo.DepartmentValuation
                        }
                    };
                case FundsAccountingType:
                    return new PickListBo
                    {
                        Id = key,
                        DepartmentId = DepartmentBo.DepartmentDataAnalyticsAdministration,
                        StandardOutputId = StandardOutputBo.TypeFundsAccountingTypeCode,
                        StandardClaimDataOutputId = StandardClaimDataOutputBo.TypeFundsAccountingTypeCode,
                        FieldName = "Funds Accounting Type",
                        UsedByDepartments = new List<int>()
                        {
                            DepartmentBo.DepartmentDataAnalyticsAdministration,
                            DepartmentBo.DepartmentValuation,
                            DepartmentBo.DepartmentClaim,
                            DepartmentBo.DepartmentRetrocession
                        }
                    };
                case Mfrs17BasicRider:
                    return new PickListBo
                    {
                        Id = key,
                        DepartmentId = DepartmentBo.DepartmentDataAnalyticsAdministration,
                        StandardOutputId = StandardOutputBo.TypeMfrs17BasicRider,
                        FieldName = "MFRS17 Basic Rider",
                        UsedByDepartments = new List<int>()
                        {
                            DepartmentBo.DepartmentDataAnalyticsAdministration,
                            DepartmentBo.DepartmentValuation
                        }
                    };
                case FundCode:
                    return new PickListBo
                    {
                        Id = key,
                        DepartmentId = DepartmentBo.DepartmentDataAnalyticsAdministration,
                        StandardOutputId = StandardOutputBo.TypeFundCode,
                        FieldName = "Fund Code",
                        UsedByDepartments = new List<int>()
                        {
                            DepartmentBo.DepartmentDataAnalyticsAdministration
                        }
                    };
                case LineOfBusiness:
                    return new PickListBo
                    {
                        Id = key,
                        DepartmentId = DepartmentBo.DepartmentDataAnalyticsAdministration,
                        StandardOutputId = StandardOutputBo.TypeLineOfBusiness,
                        FieldName = "Line Of Business",
                        UsedByDepartments = new List<int>()
                        {
                            DepartmentBo.DepartmentDataAnalyticsAdministration,
                            DepartmentBo.DepartmentValuation
                        }
                    };
                case GstIndicator:
                    return new PickListBo
                    {
                        Id = key,
                        DepartmentId = DepartmentBo.DepartmentDataAnalyticsAdministration,
                        StandardOutputId = StandardOutputBo.TypeGstIndicator,
                        FieldName = "Gst Indicator",
                        UsedByDepartments = new List<int>()
                        {
                            DepartmentBo.DepartmentDataAnalyticsAdministration,
                            DepartmentBo.DepartmentValuation
                        }
                    };
                case StaffPlanIndicator:
                    return new PickListBo
                    {
                        Id = key,
                        DepartmentId = DepartmentBo.DepartmentDataAnalyticsAdministration,
                        StandardOutputId = StandardOutputBo.TypeStaffPlanIndicator,
                        FieldName = "Staff Plan Indicator",
                        UsedByDepartments = new List<int>()
                        {
                            DepartmentBo.DepartmentDataAnalyticsAdministration
                        }
                    };
                case IndicatorJointLife:
                    return new PickListBo
                    {
                        Id = key,
                        DepartmentId = DepartmentBo.DepartmentDataAnalyticsAdministration,
                        StandardOutputId = StandardOutputBo.TypeIndicatorJointLife,
                        FieldName = "Indicator Joint Life",
                        UsedByDepartments = new List<int>()
                        {
                            DepartmentBo.DepartmentDataAnalyticsAdministration,
                            DepartmentBo.DepartmentValuation
                        }
                    };
                case TerritoryOfIssueCode:
                    return new PickListBo
                    {
                        Id = key,
                        DepartmentId = DepartmentBo.DepartmentDataAnalyticsAdministration,
                        StandardOutputId = StandardOutputBo.TypeTerritoryOfIssueCode,
                        FieldName = "Territory Of Issue Code",
                        UsedByDepartments = new List<int>()
                        {
                            DepartmentBo.DepartmentDataAnalyticsAdministration,
                            DepartmentBo.DepartmentValuation
                        }
                    };
                case CurrencyCode:
                    return new PickListBo
                    {
                        Id = key,
                        DepartmentId = DepartmentBo.DepartmentDataAnalyticsAdministration,
                        StandardOutputId = StandardOutputBo.TypeCurrencyCode,
                        StandardClaimDataOutputId = StandardClaimDataOutputBo.TypeCurrencyCode,
                        StandardSoaDataOutputId = StandardSoaDataOutputBo.TypeCurrencyCode,
                        FieldName = "Currency Code",
                        UsedByDepartments = new List<int>()
                        {
                            DepartmentBo.DepartmentDataAnalyticsAdministration,
                            DepartmentBo.DepartmentValuation
                        }
                    };
                case ValuationBenefitCode:
                    return new PickListBo
                    {
                        Id = key,
                        DepartmentId = DepartmentBo.DepartmentValuation,
                        FieldName = "Valuation Benefit Code",
                        IsEditable = false,
                        UsedByDepartments = new List<int>()
                        {
                            DepartmentBo.DepartmentDataAnalyticsAdministration,
                            DepartmentBo.DepartmentValuation
                        }
                    };
                case CedingCompanyType:
                    return new PickListBo
                    {
                        Id = key,
                        DepartmentId = DepartmentBo.DepartmentDataAnalyticsAdministration,
                        FieldName = "Ceding Company Type",
                        UsedByDepartments = new List<int>()
                        {
                            DepartmentBo.DepartmentDataAnalyticsAdministration,
                            DepartmentBo.DepartmentTreatyPricing
                        }
                    };
                 case TreatyType:
                    return new PickListBo
                    {
                        Id = key,
                        DepartmentId = DepartmentBo.DepartmentDataAnalyticsAdministration,
                        StandardOutputId = StandardOutputBo.TypeTreatyType,
                        StandardClaimDataOutputId = StandardClaimDataOutputBo.TypeTreatyType,
                        FieldName = "Treaty Type",
                        UsedByDepartments = new List<int>()
                        {
                            DepartmentBo.DepartmentDataAnalyticsAdministration,
                            DepartmentBo.DepartmentTreatyPricing
                        }
                    };
                case TreatyStatus:
                    return new PickListBo
                    {
                        Id = key,
                        DepartmentId = DepartmentBo.DepartmentDataAnalyticsAdministration,
                        FieldName = "Treaty Status",
                        UsedByDepartments = new List<int>()
                        {
                            DepartmentBo.DepartmentDataAnalyticsAdministration,
                            DepartmentBo.DepartmentTreatyPricing
                        }
                    };
                case CountryCode:
                    return new PickListBo
                    {
                        Id = key,
                        DepartmentId = DepartmentBo.DepartmentDataAnalyticsAdministration,
                        FieldName = "Country Code",
                        UsedByDepartments = new List<int>()
                        {
                            DepartmentBo.DepartmentDataAnalyticsAdministration,
                            DepartmentBo.DepartmentRetrocession
                        }
                    };
                case StatementStatus:
                    return new PickListBo
                    {
                        Id = key,
                        DepartmentId = DepartmentBo.DepartmentDataAnalyticsAdministration,
                        StandardSoaDataOutputId = StandardSoaDataOutputBo.TypeStatementStatus,
                        FieldName = "Statement Status",
                        UsedByDepartments = new List<int>()
                        {
                            DepartmentBo.DepartmentDataAnalyticsAdministration
                        }
                    };
                case RecordType:
                    return new PickListBo
                    {
                        Id = key,
                        DepartmentId = DepartmentBo.DepartmentClaim,
                        StandardClaimDataOutputId = StandardClaimDataOutputBo.TypeRecordType,
                        FieldName = "Record Type",
                        IsEditable = false,
                        UsedByDepartments = new List<int>()
                        {
                            DepartmentBo.DepartmentClaim
                        }
                    };
                case ClaimTransactionType:
                    return new PickListBo
                    {
                        Id = key,
                        DepartmentId = DepartmentBo.DepartmentClaim,
                        StandardClaimDataOutputId = StandardClaimDataOutputBo.TypeClaimTransactionType,
                        FieldName = "Claim Transaction Type",
                        IsEditable = false,
                        UsedByDepartments = new List<int>()
                        {
                            DepartmentBo.DepartmentClaim,
                            DepartmentBo.DepartmentFinance
                        }
                    };
                case InvoiceField:
                    return new PickListBo
                    {
                        Id = key,
                        DepartmentId = DepartmentBo.DepartmentDataAnalyticsAdministration,
                        FieldName = "Invoice Field",
                        IsEditable = false,
                        UsedByDepartments = new List<int>()
                        {
                            DepartmentBo.DepartmentDataAnalyticsAdministration
                        }
                    };
                case ProfitComm:
                    return new PickListBo
                    {
                        Id = key,
                        DepartmentId = DepartmentBo.DepartmentDataAnalyticsAdministration,
                        StandardOutputId = StandardOutputBo.TypeProfitComm,
                        FieldName = "Profit Commission",
                        UsedByDepartments = new List<int>()
                        {
                            DepartmentBo.DepartmentDataAnalyticsAdministration,
                            DepartmentBo.DepartmentTreatyPricing
                        }
                    };
                case BenefitCategory:
                    return new PickListBo
                    {
                        Id = key,
                        DepartmentId = DepartmentBo.DepartmentValuation,
                        FieldName = "Benefit Category",
                        UsedByDepartments = new List<int>()
                        {
                            DepartmentBo.DepartmentTreatyPricing,
                            DepartmentBo.DepartmentValuation
                        }
                    };
                case RetroRegisterField:
                    return new PickListBo
                    {
                        Id = key,
                        DepartmentId = DepartmentBo.DepartmentDataAnalyticsAdministration,
                        FieldName = "Retro Register Field",
                        IsEditable = false,
                        UsedByDepartments = new List<int>()
                        {
                            DepartmentBo.DepartmentDataAnalyticsAdministration,
                            DepartmentBo.DepartmentRetrocession
                        }
                    };
                case ReinsuranceType:
                    return new PickListBo
                    {
                        Id = key,
                        DepartmentId = DepartmentBo.DepartmentBD,
                        FieldName = "Reinsurance Type",
                        //IsEditable = false, //To be uncomment
                        UsedByDepartments = new List<int>()
                        {
                            DepartmentBo.DepartmentBD,
                            DepartmentBo.DepartmentProductPricing
                        }
                    };
                case AgeBasis:
                    return new PickListBo
                    {
                        Id = key,
                        DepartmentId = DepartmentBo.DepartmentBD,
                        FieldName = "Age Basis",
                        //IsEditable = false, //To be uncomment
                        UsedByDepartments = new List<int>()
                        {
                            DepartmentBo.DepartmentBD,
                            DepartmentBo.DepartmentProductPricing
                        }
                    };
                case RateGuarantee:
                    return new PickListBo
                    {
                        Id = key,
                        DepartmentId = DepartmentBo.DepartmentBD,
                        FieldName = "Rate Guarantee",
                        //IsEditable = false, //To be uncomment
                        UsedByDepartments = new List<int>()
                        {
                            DepartmentBo.DepartmentBD,
                            DepartmentBo.DepartmentProductPricing
                        }
                    };
                case UnearnedPremiumRefund:
                    return new PickListBo
                    {
                        Id = key,
                        DepartmentId = DepartmentBo.DepartmentBD,
                        FieldName = "Unearned Premium Refund",
                        //IsEditable = false, //To be uncomment
                        UsedByDepartments = new List<int>()
                        {
                            DepartmentBo.DepartmentBD,
                            DepartmentBo.DepartmentProductPricing
                        }
                    };
                case AgeDefinition:
                    return new PickListBo
                    {
                        Id = key,
                        DepartmentId = DepartmentBo.DepartmentBD,
                        FieldName = "Age Definition",
                        //IsEditable = false, //To be uncomment
                        UsedByDepartments = new List<int>()
                        {
                            DepartmentBo.DepartmentBD,
                            DepartmentBo.DepartmentProductPricing
                        }
                    };
                case DistributionTier:
                    return new PickListBo
                    {
                        Id = key,
                        DepartmentId = DepartmentBo.DepartmentBD,
                        FieldName = "Distribution Tier",
                        //IsEditable = false, //To be uncomment
                        UsedByDepartments = new List<int>()
                        {
                            DepartmentBo.DepartmentBD,
                            DepartmentBo.DepartmentProductPricing
                        }
                    };
                case UnderwritingMethod:
                    return new PickListBo
                    {
                        Id = key,
                        DepartmentId = DepartmentBo.DepartmentBD,
                        FieldName = "Underwriting Method",
                        UsedByDepartments = new List<int>()
                        {
                            DepartmentBo.DepartmentBD,
                            DepartmentBo.DepartmentProductPricing
                        }
                    };
                case TargetSegment:
                    return new PickListBo
                    {
                        Id = key,
                        DepartmentId = DepartmentBo.DepartmentBD,
                        FieldName = "Target Segment",
                        UsedByDepartments = new List<int>()
                        {
                            DepartmentBo.DepartmentBD,
                            DepartmentBo.DepartmentProductPricing
                        }
                    };
                case PayoutType:
                    return new PickListBo
                    {
                        Id = key,
                        DepartmentId = DepartmentBo.DepartmentBD,
                        FieldName = "Payout Type",
                        UsedByDepartments = new List<int>()
                        {
                            DepartmentBo.DepartmentBD,
                            DepartmentBo.DepartmentProductPricing
                        }
                    };
                case ArrangementReinsuranceType:
                    return new PickListBo
                    {
                        Id = key,
                        DepartmentId = DepartmentBo.DepartmentBD,
                        FieldName = "Arrangement Reinsurance Type",
                        UsedByDepartments = new List<int>()
                        {
                            DepartmentBo.DepartmentBD,
                            DepartmentBo.DepartmentProductPricing
                        }
                    };
                case RiskPatternSum:
                    return new PickListBo
                    {
                        Id = key,
                        DepartmentId = DepartmentBo.DepartmentBD,
                        FieldName = "Risk Pattern Sum",
                        UsedByDepartments = new List<int>()
                        {
                            DepartmentBo.DepartmentBD,
                            DepartmentBo.DepartmentProductPricing
                        }
                    };
                case CampaignType:
                    return new PickListBo
                    {
                        Id = key,
                        DepartmentId = DepartmentBo.DepartmentBD,
                        FieldName = "Campaign Type",
                        UsedByDepartments = new List<int>()
                        {
                            DepartmentBo.DepartmentBD,
                            DepartmentBo.DepartmentProductPricing
                        }
                    };
                case PricingTeam:
                    return new PickListBo
                    {
                        Id = key,
                        DepartmentId = DepartmentBo.DepartmentBD,
                        FieldName = "Pricing Team",
                        IsEditable = false,
                        UsedByDepartments = new List<int>()
                        {
                            DepartmentBo.DepartmentBD,
                            DepartmentBo.DepartmentProductPricing
                        }
                    };
                case PaymentRetrocessionairePremium:
                    return new PickListBo
                    {
                        Id = key,
                        DepartmentId = DepartmentBo.DepartmentBD,
                        FieldName = "Payment of Retrocessionaire Premium",
                        UsedByDepartments = new List<int>()
                        {
                            DepartmentBo.DepartmentBD,
                            DepartmentBo.DepartmentProductPricing
                        }
                    };
                case ArrangementRetrocessionnaireType:
                    return new PickListBo
                    {
                        Id = key,
                        DepartmentId = DepartmentBo.DepartmentBD,
                        FieldName = "Retrocessionnaire Type of Arrangement",
                        UsedByDepartments = new List<int>()
                        {
                            DepartmentBo.DepartmentBD,
                            DepartmentBo.DepartmentProductPricing
                        }
                    };
                case IndustryName:
                    return new PickListBo
                    {
                        Id = key,
                        DepartmentId = DepartmentBo.DepartmentBD,
                        FieldName = "Industry Name",
                        UsedByDepartments = new List<int>()
                        {
                            DepartmentBo.DepartmentBD,
                            DepartmentBo.DepartmentProductPricing
                        }
                    };
                case ReferredType:
                    return new PickListBo
                    {
                        Id = key,
                        DepartmentId = DepartmentBo.DepartmentBD,
                        FieldName = "Referred Type",
                        UsedByDepartments = new List<int>()
                        {
                            DepartmentBo.DepartmentBD,
                            DepartmentBo.DepartmentProductPricing
                        }
                    };
                case RequestType:
                    return new PickListBo
                    {
                        Id = key,
                        DepartmentId = DepartmentBo.DepartmentBD,
                        FieldName = "Type of Request",
                        UsedByDepartments = new List<int>()
                        {
                            DepartmentBo.DepartmentBD,
                            DepartmentBo.DepartmentProductPricing
                        }
                    };
                case PremiumType:
                    return new PickListBo
                    {
                        Id = key,
                        DepartmentId = DepartmentBo.DepartmentBD,
                        FieldName = "Type of Premium",
                        UsedByDepartments = new List<int>()
                        {
                            DepartmentBo.DepartmentBD,
                            DepartmentBo.DepartmentProductPricing
                        }
                    };
                case TableType:
                    return new PickListBo
                    {
                        Id = key,
                        DepartmentId = DepartmentBo.DepartmentBD,
                        FieldName = "Table Type",
                        IsEditable = false,
                        UsedByDepartments = new List<int>()
                        {
                            DepartmentBo.DepartmentBD,
                            DepartmentBo.DepartmentProductPricing
                        }
                    };
                case Entitlement:
                    return new PickListBo
                    {
                        Id = key,
                        DepartmentId = DepartmentBo.DepartmentBD,
                        FieldName = "Entitlement",
                        UsedByDepartments = new List<int>()
                        {
                            DepartmentBo.DepartmentBD,
                            DepartmentBo.DepartmentProductPricing
                        }
                    };
                case ExceptionStatus:
                    return new PickListBo
                    {
                        Id = key,
                        DepartmentId = DepartmentBo.DepartmentShared,
                        FieldName = "Exception Status",
                        UsedByDepartments = new List<int>()
                        {
                            DepartmentBo.DepartmentBD,
                            DepartmentBo.DepartmentProductPricing
                        }
                    };
                case RetroPremiumFrequencyMode:
                    return new PickListBo
                    {
                        Id = key,
                        DepartmentId = DepartmentBo.DepartmentShared,
                        FieldName = "Retro Premium Frequency Mode",
                        UsedByDepartments = new List<int>()
                        {
                            DepartmentBo.DepartmentRetrocession
                        }
                    };
                default:
                    return null;
            }
        }
    }
}
