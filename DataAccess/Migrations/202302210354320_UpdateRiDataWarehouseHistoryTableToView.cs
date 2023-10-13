namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateRiDataWarehouseHistoryTableToView : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.RiDataWarehouseHistories", "EndingPolicyStatus", "dbo.PickListDetails");
            DropForeignKey("dbo.Mfrs17ReportingDetailRiDatas", "RiDataWarehouseHistoryId", "dbo.RiDataWarehouseHistories");
            DropForeignKey("dbo.RiDataWarehouseHistories", "CreatedById", "dbo.Users");
            DropForeignKey("dbo.RiDataWarehouseHistories", "CutOffId", "dbo.CutOff");
            DropForeignKey("dbo.RiDataWarehouseHistories", "RiDataWarehouseId", "dbo.RiDataWarehouse");
            DropForeignKey("dbo.PerLifeAggregationDetailData", "RiDataWarehouseHistoryId", "dbo.RiDataWarehouseHistories");
            DropIndex("dbo.RiDataWarehouseHistories", new[] { "AarShare2" });
            DropIndex("dbo.RiDataWarehouseHistories", new[] { "AarCap2" });
            DropIndex("dbo.RiDataWarehouseHistories", new[] { "WakalahFeePercentage" });
            DropIndex("dbo.RiDataWarehouseHistories", new[] { "TreatyNumber" });
            //AddForeignKey("dbo.Mfrs17ReportingDetailRiDatas", "RiDataWarehouseHistoryId", "dbo.RiDataWarehouseHistories", "Id", cascadeDelete: true);
            AddForeignKey("dbo.RiDataWarehouseHistories", "CreatedById", "dbo.Users", "Id", cascadeDelete: true);
            AddForeignKey("dbo.RiDataWarehouseHistories", "CutOffId", "dbo.CutOff", "Id", cascadeDelete: true);
            AddForeignKey("dbo.RiDataWarehouseHistories", "RiDataWarehouseId", "dbo.RiDataWarehouse", "Id", cascadeDelete: true);
            //AddForeignKey("dbo.PerLifeAggregationDetailData", "RiDataWarehouseHistoryId", "dbo.RiDataWarehouseHistories", "Id", cascadeDelete: true);

            DropTable("dbo.RiDataWarehouseHistories");

            Sql("CREATE OR ALTER VIEW [dbo].[RiDataWarehouseHistories] AS SELECT 1 AS CutOffId, Id AS RiDataWarehouseId, * FROM RiDataWarehouse WHERE Id = 0");
        }
        
        public override void Down()
        {
            Sql("DROP VIEW [dbo].[RiDataWarehouseHistories]");

            CreateTable(
                "dbo.RiDataWarehouseHistories",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    CutOffId = c.Int(nullable: false),
                    RiDataWarehouseId = c.Long(nullable: false),
                    EndingPolicyStatus = c.Int(),
                    RecordType = c.Int(nullable: false),
                    Quarter = c.String(maxLength: 64),
                    TreatyCode = c.String(maxLength: 35),
                    ReinsBasisCode = c.String(maxLength: 30),
                    FundsAccountingTypeCode = c.String(maxLength: 30),
                    PremiumFrequencyCode = c.String(maxLength: 10),
                    ReportPeriodMonth = c.Int(),
                    ReportPeriodYear = c.Int(),
                    RiskPeriodMonth = c.Int(),
                    RiskPeriodYear = c.Int(),
                    TransactionTypeCode = c.String(maxLength: 2),
                    PolicyNumber = c.String(maxLength: 150),
                    IssueDatePol = c.DateTime(precision: 7, storeType: "datetime2"),
                    IssueDateBen = c.DateTime(precision: 7, storeType: "datetime2"),
                    ReinsEffDatePol = c.DateTime(precision: 7, storeType: "datetime2"),
                    ReinsEffDateBen = c.DateTime(precision: 7, storeType: "datetime2"),
                    CedingPlanCode = c.String(maxLength: 30),
                    CedingBenefitTypeCode = c.String(maxLength: 30),
                    CedingBenefitRiskCode = c.String(maxLength: 50),
                    MlreBenefitCode = c.String(maxLength: 10),
                    OriSumAssured = c.Double(),
                    CurrSumAssured = c.Double(),
                    AmountCededB4MlreShare = c.Double(),
                    RetentionAmount = c.Double(),
                    AarOri = c.Double(),
                    Aar = c.Double(),
                    AarSpecial1 = c.Double(),
                    AarSpecial2 = c.Double(),
                    AarSpecial3 = c.Double(),
                    InsuredName = c.String(maxLength: 128),
                    InsuredGenderCode = c.String(maxLength: 1),
                    InsuredTobaccoUse = c.String(maxLength: 1),
                    InsuredDateOfBirth = c.DateTime(precision: 7, storeType: "datetime2"),
                    InsuredOccupationCode = c.String(maxLength: 5),
                    InsuredRegisterNo = c.String(maxLength: 30),
                    InsuredAttainedAge = c.Int(),
                    InsuredNewIcNumber = c.String(maxLength: 15),
                    InsuredOldIcNumber = c.String(maxLength: 15),
                    InsuredName2nd = c.String(maxLength: 128),
                    InsuredGenderCode2nd = c.String(maxLength: 1),
                    InsuredTobaccoUse2nd = c.String(maxLength: 1),
                    InsuredDateOfBirth2nd = c.DateTime(precision: 7, storeType: "datetime2"),
                    InsuredAttainedAge2nd = c.Int(),
                    InsuredNewIcNumber2nd = c.String(maxLength: 15),
                    InsuredOldIcNumber2nd = c.String(maxLength: 15),
                    ReinsuranceIssueAge = c.Int(),
                    ReinsuranceIssueAge2nd = c.Int(),
                    PolicyTerm = c.Double(),
                    PolicyExpiryDate = c.DateTime(precision: 7, storeType: "datetime2"),
                    DurationYear = c.Double(),
                    DurationDay = c.Double(),
                    DurationMonth = c.Double(),
                    PremiumCalType = c.String(maxLength: 5),
                    CedantRiRate = c.Double(),
                    RateTable = c.String(maxLength: 128),
                    AgeRatedUp = c.Int(),
                    DiscountRate = c.Double(),
                    LoadingType = c.String(maxLength: 15),
                    UnderwriterRating = c.Double(),
                    UnderwriterRatingUnit = c.Double(),
                    UnderwriterRatingTerm = c.Int(),
                    UnderwriterRating2 = c.Double(),
                    UnderwriterRatingUnit2 = c.Double(),
                    UnderwriterRatingTerm2 = c.Int(),
                    UnderwriterRating3 = c.Double(),
                    UnderwriterRatingUnit3 = c.Double(),
                    UnderwriterRatingTerm3 = c.Int(),
                    FlatExtraAmount = c.Double(),
                    FlatExtraUnit = c.Double(),
                    FlatExtraTerm = c.Int(),
                    FlatExtraAmount2 = c.Double(),
                    FlatExtraTerm2 = c.Int(),
                    StandardPremium = c.Double(),
                    SubstandardPremium = c.Double(),
                    FlatExtraPremium = c.Double(),
                    GrossPremium = c.Double(),
                    StandardDiscount = c.Double(),
                    SubstandardDiscount = c.Double(),
                    VitalityDiscount = c.Double(),
                    TotalDiscount = c.Double(),
                    NetPremium = c.Double(),
                    AnnualRiPrem = c.Double(),
                    RiCovPeriod = c.Double(),
                    AdjBeginDate = c.DateTime(precision: 7, storeType: "datetime2"),
                    AdjEndDate = c.DateTime(precision: 7, storeType: "datetime2"),
                    PolicyNumberOld = c.String(maxLength: 150),
                    PolicyStatusCode = c.String(maxLength: 20),
                    PolicyGrossPremium = c.Double(),
                    PolicyStandardPremium = c.Double(),
                    PolicySubstandardPremium = c.Double(),
                    PolicyTermRemain = c.Double(),
                    PolicyAmountDeath = c.Double(),
                    PolicyReserve = c.Double(),
                    PolicyPaymentMethod = c.String(maxLength: 10),
                    PolicyLifeNumber = c.Int(),
                    FundCode = c.String(maxLength: 25),
                    LineOfBusiness = c.String(maxLength: 5),
                    ApLoading = c.Double(),
                    LoanInterestRate = c.Double(),
                    DefermentPeriod = c.Int(),
                    RiderNumber = c.Int(),
                    CampaignCode = c.String(maxLength: 10),
                    Nationality = c.String(maxLength: 20),
                    TerritoryOfIssueCode = c.String(maxLength: 20),
                    CurrencyCode = c.String(maxLength: 3),
                    StaffPlanIndicator = c.String(maxLength: 1),
                    CedingTreatyCode = c.String(maxLength: 30),
                    CedingPlanCodeOld = c.String(maxLength: 30),
                    CedingBasicPlanCode = c.String(maxLength: 30),
                    CedantSar = c.Double(),
                    CedantReinsurerCode = c.String(maxLength: 10),
                    AmountCededB4MlreShare2 = c.Double(),
                    CessionCode = c.String(maxLength: 10),
                    CedantRemark = c.String(maxLength: 255),
                    GroupPolicyNumber = c.String(maxLength: 30),
                    GroupPolicyName = c.String(maxLength: 128),
                    NoOfEmployee = c.Int(),
                    PolicyTotalLive = c.Int(),
                    GroupSubsidiaryName = c.String(maxLength: 128),
                    GroupSubsidiaryNo = c.String(maxLength: 30),
                    GroupEmployeeBasicSalary = c.Double(),
                    GroupEmployeeJobType = c.String(maxLength: 10),
                    GroupEmployeeJobCode = c.String(maxLength: 10),
                    GroupEmployeeBasicSalaryRevise = c.Double(),
                    GroupEmployeeBasicSalaryMultiplier = c.Double(),
                    CedingPlanCode2 = c.String(maxLength: 30),
                    DependantIndicator = c.String(maxLength: 2),
                    GhsRoomBoard = c.Int(),
                    PolicyAmountSubstandard = c.Double(),
                    Layer1RiShare = c.Double(),
                    Layer1InsuredAttainedAge = c.Int(),
                    Layer1InsuredAttainedAge2nd = c.Int(),
                    Layer1StandardPremium = c.Double(),
                    Layer1SubstandardPremium = c.Double(),
                    Layer1GrossPremium = c.Double(),
                    Layer1StandardDiscount = c.Double(),
                    Layer1SubstandardDiscount = c.Double(),
                    Layer1TotalDiscount = c.Double(),
                    Layer1NetPremium = c.Double(),
                    Layer1GrossPremiumAlt = c.Double(),
                    Layer1TotalDiscountAlt = c.Double(),
                    Layer1NetPremiumAlt = c.Double(),
                    SpecialIndicator1 = c.String(),
                    SpecialIndicator2 = c.String(),
                    SpecialIndicator3 = c.String(),
                    IndicatorJointLife = c.String(maxLength: 1),
                    TaxAmount = c.Double(),
                    GstIndicator = c.String(maxLength: 3),
                    GstGrossPremium = c.Double(),
                    GstTotalDiscount = c.Double(),
                    GstVitality = c.Double(),
                    GstAmount = c.Double(),
                    Mfrs17BasicRider = c.String(maxLength: 5),
                    Mfrs17CellName = c.String(maxLength: 50),
                    Mfrs17TreatyCode = c.String(maxLength: 25),
                    LoaCode = c.String(maxLength: 20),
                    CurrencyRate = c.Double(),
                    NoClaimBonus = c.Double(),
                    SurrenderValue = c.Double(),
                    DatabaseCommision = c.Double(),
                    GrossPremiumAlt = c.Double(),
                    NetPremiumAlt = c.Double(),
                    Layer1FlatExtraPremium = c.Double(),
                    TransactionPremium = c.Double(),
                    OriginalPremium = c.Double(),
                    TransactionDiscount = c.Double(),
                    OriginalDiscount = c.Double(),
                    BrokerageFee = c.Double(),
                    MaxUwRating = c.Double(),
                    RetentionCap = c.Double(),
                    AarCap = c.Double(),
                    RiRate = c.Double(),
                    RiRate2 = c.Double(),
                    AnnuityFactor = c.Double(),
                    SumAssuredOffered = c.Double(),
                    UwRatingOffered = c.Double(),
                    FlatExtraAmountOffered = c.Double(),
                    FlatExtraDuration = c.Double(),
                    EffectiveDate = c.DateTime(precision: 7, storeType: "datetime2"),
                    OfferLetterSentDate = c.DateTime(precision: 7, storeType: "datetime2"),
                    RiskPeriodStartDate = c.DateTime(precision: 7, storeType: "datetime2"),
                    RiskPeriodEndDate = c.DateTime(precision: 7, storeType: "datetime2"),
                    Mfrs17AnnualCohort = c.Int(),
                    MaxExpiryAge = c.Int(),
                    MinIssueAge = c.Int(),
                    MaxIssueAge = c.Int(),
                    MinAar = c.Double(),
                    MaxAar = c.Double(),
                    CorridorLimit = c.Double(),
                    Abl = c.Double(),
                    RatePerBasisUnit = c.Int(),
                    RiDiscountRate = c.Double(),
                    LargeSaDiscount = c.Double(),
                    GroupSizeDiscount = c.Double(),
                    EwarpNumber = c.Int(),
                    EwarpActionCode = c.String(maxLength: 10),
                    RetentionShare = c.Double(),
                    AarShare = c.Double(),
                    ProfitComm = c.String(maxLength: 1),
                    TotalDirectRetroAar = c.Double(),
                    TotalDirectRetroGrossPremium = c.Double(),
                    TotalDirectRetroDiscount = c.Double(),
                    TotalDirectRetroNetPremium = c.Double(),
                    TotalDirectRetroNoClaimBonus = c.Double(),
                    TotalDirectRetroDatabaseCommission = c.Double(),
                    TreatyType = c.String(maxLength: 20),
                    LastUpdatedDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    RetroParty1 = c.String(maxLength: 128),
                    RetroParty2 = c.String(maxLength: 128),
                    RetroParty3 = c.String(maxLength: 128),
                    RetroShare1 = c.Double(),
                    RetroShare2 = c.Double(),
                    RetroShare3 = c.Double(),
                    RetroPremiumSpread1 = c.Double(),
                    RetroPremiumSpread2 = c.Double(),
                    RetroPremiumSpread3 = c.Double(),
                    RetroAar1 = c.Double(),
                    RetroAar2 = c.Double(),
                    RetroAar3 = c.Double(),
                    RetroReinsurancePremium1 = c.Double(),
                    RetroReinsurancePremium2 = c.Double(),
                    RetroReinsurancePremium3 = c.Double(),
                    RetroDiscount1 = c.Double(),
                    RetroDiscount2 = c.Double(),
                    RetroDiscount3 = c.Double(),
                    RetroNetPremium1 = c.Double(),
                    RetroNetPremium2 = c.Double(),
                    RetroNetPremium3 = c.Double(),
                    RetroNoClaimBonus1 = c.Double(),
                    RetroNoClaimBonus2 = c.Double(),
                    RetroNoClaimBonus3 = c.Double(),
                    RetroDatabaseCommission1 = c.Double(),
                    RetroDatabaseCommission2 = c.Double(),
                    RetroDatabaseCommission3 = c.Double(),
                    MaxApLoading = c.Double(),
                    MlreInsuredAttainedAgeAtCurrentMonth = c.Int(),
                    MlreInsuredAttainedAgeAtPreviousMonth = c.Int(),
                    InsuredAttainedAgeCheck = c.Boolean(),
                    MaxExpiryAgeCheck = c.Boolean(),
                    MlrePolicyIssueAge = c.Int(),
                    PolicyIssueAgeCheck = c.Boolean(),
                    MinIssueAgeCheck = c.Boolean(),
                    MaxIssueAgeCheck = c.Boolean(),
                    MaxUwRatingCheck = c.Boolean(),
                    ApLoadingCheck = c.Boolean(),
                    EffectiveDateCheck = c.Boolean(),
                    MinAarCheck = c.Boolean(),
                    MaxAarCheck = c.Boolean(),
                    CorridorLimitCheck = c.Boolean(),
                    AblCheck = c.Boolean(),
                    RetentionCheck = c.Boolean(),
                    AarCheck = c.Boolean(),
                    MlreStandardPremium = c.Double(),
                    MlreSubstandardPremium = c.Double(),
                    MlreFlatExtraPremium = c.Double(),
                    MlreGrossPremium = c.Double(),
                    MlreStandardDiscount = c.Double(),
                    MlreSubstandardDiscount = c.Double(),
                    MlreLargeSaDiscount = c.Double(),
                    MlreGroupSizeDiscount = c.Double(),
                    MlreVitalityDiscount = c.Double(),
                    MlreTotalDiscount = c.Double(),
                    MlreNetPremium = c.Double(),
                    NetPremiumCheck = c.Double(),
                    ServiceFeePercentage = c.Double(),
                    ServiceFee = c.Double(),
                    MlreBrokerageFee = c.Double(),
                    MlreDatabaseCommission = c.Double(),
                    ValidityDayCheck = c.Boolean(),
                    SumAssuredOfferedCheck = c.Boolean(),
                    UwRatingCheck = c.Boolean(),
                    FlatExtraAmountCheck = c.Boolean(),
                    FlatExtraDurationCheck = c.Boolean(),
                    AarShare2 = c.Double(),
                    AarCap2 = c.Double(),
                    WakalahFeePercentage = c.Double(),
                    TreatyNumber = c.String(maxLength: 128),
                    ConflictType = c.Int(nullable: false),
                    CreatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    UpdatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    CreatedById = c.Int(nullable: false),
                    UpdatedById = c.Int(),
                })
                .PrimaryKey(t => t.Id);

            DropIndex("dbo.PerLifeAggregationDetailData", new[] { "RiDataWarehouseHistoryId" });
            DropPrimaryKey("dbo.Mfrs17ReportingDetailRiDatas");
            AlterColumn("dbo.PerLifeAggregationDetailData", "RiDataWarehouseHistoryId", c => c.Long(nullable: false));
            AlterColumn("dbo.Mfrs17ReportingDetailRiDatas", "RiDataWarehouseHistoryId", c => c.Long(nullable: false));
            AddPrimaryKey("dbo.Mfrs17ReportingDetailRiDatas", new[] { "Mfrs17ReportingDetailId", "RiDataWarehouseHistoryId" });
            CreateIndex("dbo.PerLifeAggregationDetailData", "RiDataWarehouseHistoryId");
            CreateIndex("dbo.RiDataWarehouseHistories", "UpdatedById");
            CreateIndex("dbo.RiDataWarehouseHistories", "CreatedById");
            CreateIndex("dbo.RiDataWarehouseHistories", "ConflictType");
            CreateIndex("dbo.RiDataWarehouseHistories", "TreatyNumber");
            CreateIndex("dbo.RiDataWarehouseHistories", "WakalahFeePercentage");
            CreateIndex("dbo.RiDataWarehouseHistories", "AarCap2");
            CreateIndex("dbo.RiDataWarehouseHistories", "AarShare2");
            CreateIndex("dbo.RiDataWarehouseHistories", "RetroParty3");
            CreateIndex("dbo.RiDataWarehouseHistories", "RetroParty2");
            CreateIndex("dbo.RiDataWarehouseHistories", "RetroParty1");
            CreateIndex("dbo.RiDataWarehouseHistories", "RiskPeriodEndDate");
            CreateIndex("dbo.RiDataWarehouseHistories", "RiskPeriodStartDate");
            CreateIndex("dbo.RiDataWarehouseHistories", "EffectiveDate");
            CreateIndex("dbo.RiDataWarehouseHistories", "TerritoryOfIssueCode");
            CreateIndex("dbo.RiDataWarehouseHistories", "PolicyStatusCode");
            CreateIndex("dbo.RiDataWarehouseHistories", "InsuredDateOfBirth");
            CreateIndex("dbo.RiDataWarehouseHistories", "InsuredGenderCode");
            CreateIndex("dbo.RiDataWarehouseHistories", new[] { "InsuredName", "InsuredDateOfBirth", "InsuredGenderCode", "TerritoryOfIssueCode" }, name: "IX_ConflictCheckPerLifeDataValidation");
            CreateIndex("dbo.RiDataWarehouseHistories", "InsuredName");
            CreateIndex("dbo.RiDataWarehouseHistories", "MlreBenefitCode");
            CreateIndex("dbo.RiDataWarehouseHistories", "CedingBenefitRiskCode");
            CreateIndex("dbo.RiDataWarehouseHistories", "CedingBenefitTypeCode");
            CreateIndex("dbo.RiDataWarehouseHistories", "CedingPlanCode");
            CreateIndex("dbo.RiDataWarehouseHistories", "ReinsEffDatePol");
            CreateIndex("dbo.RiDataWarehouseHistories", "PolicyNumber");
            CreateIndex("dbo.RiDataWarehouseHistories", "TransactionTypeCode");
            CreateIndex("dbo.RiDataWarehouseHistories", "RiskPeriodYear");
            CreateIndex("dbo.RiDataWarehouseHistories", "RiskPeriodMonth");
            CreateIndex("dbo.RiDataWarehouseHistories", "ReportPeriodYear");
            CreateIndex("dbo.RiDataWarehouseHistories", "ReportPeriodMonth");
            CreateIndex("dbo.RiDataWarehouseHistories", "PremiumFrequencyCode");
            CreateIndex("dbo.RiDataWarehouseHistories", "FundsAccountingTypeCode");
            CreateIndex("dbo.RiDataWarehouseHistories", new[] { "TreatyCode", "CedingPlanCode", "InsuredName", "InsuredDateOfBirth", "PolicyNumber", "InsuredGenderCode", "MlreBenefitCode", "CedingBenefitRiskCode", "CedingBenefitTypeCode", "ReinsEffDatePol", "FundsAccountingTypeCode", "ReinsBasisCode", "TransactionTypeCode", "EffectiveDate", "ReportPeriodMonth", "ReportPeriodYear", "RiskPeriodMonth", "RiskPeriodYear", "RiskPeriodStartDate", "RiskPeriodEndDate" }, name: "IX_DuplicationCheck_CheckReinsBasisCode");
            CreateIndex("dbo.RiDataWarehouseHistories", new[] { "TreatyCode", "CedingPlanCode", "InsuredName", "InsuredDateOfBirth", "PolicyNumber", "InsuredGenderCode", "MlreBenefitCode", "CedingBenefitRiskCode", "CedingBenefitTypeCode", "ReinsEffDatePol", "FundsAccountingTypeCode", "TransactionTypeCode", "EffectiveDate", "ReportPeriodMonth", "ReportPeriodYear", "RiskPeriodMonth", "RiskPeriodYear", "RiskPeriodStartDate", "RiskPeriodEndDate" }, name: "IX_DuplicationCheck_NoCheckReinsBasisCode");
            CreateIndex("dbo.RiDataWarehouseHistories", "TreatyCode");
            CreateIndex("dbo.RiDataWarehouseHistories", "Quarter");
            CreateIndex("dbo.RiDataWarehouseHistories", "RecordType");
            CreateIndex("dbo.RiDataWarehouseHistories", "EndingPolicyStatus");
            CreateIndex("dbo.RiDataWarehouseHistories", "RiDataWarehouseId");
            CreateIndex("dbo.RiDataWarehouseHistories", new[] { "CutOffId", "TransactionTypeCode", "PolicyStatusCode", "RiskPeriodYear", "RiskPeriodMonth", "FundsAccountingTypeCode" }, name: "IX_PerLifeDataValidationRiDataWarehouseHistoryCursor");
            CreateIndex("dbo.RiDataWarehouseHistories", "CutOffId");
            CreateIndex("dbo.Mfrs17ReportingDetailRiDatas", "RiDataWarehouseHistoryId");
            AddForeignKey("dbo.PerLifeAggregationDetailData", "RiDataWarehouseHistoryId", "dbo.RiDataWarehouseHistories", "Id");
            AddForeignKey("dbo.Mfrs17ReportingDetailRiDatas", "RiDataWarehouseHistoryId", "dbo.RiDataWarehouseHistories", "Id");
            AddForeignKey("dbo.RiDataWarehouseHistories", "UpdatedById", "dbo.Users", "Id");
            AddForeignKey("dbo.RiDataWarehouseHistories", "RiDataWarehouseId", "dbo.RiDataWarehouse", "Id");
            AddForeignKey("dbo.RiDataWarehouseHistories", "EndingPolicyStatus", "dbo.PickListDetails", "Id");
            AddForeignKey("dbo.RiDataWarehouseHistories", "CutOffId", "dbo.CutOff", "Id");
            AddForeignKey("dbo.RiDataWarehouseHistories", "CreatedById", "dbo.Users", "Id");
        }
    }
}
