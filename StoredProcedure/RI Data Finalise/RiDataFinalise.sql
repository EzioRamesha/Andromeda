CREATE OR ALTER PROCEDURE [dbo].[RiDataFinalise](@RiDataBatchId INT, @Result VARCHAR(MAX) OUTPUT) 

AS 

DECLARE 
    @RiDataId                INT, 
    @TreatyCode              VARCHAR(MAX), 
    @ReinsBasisCode          VARCHAR(MAX), 
    @FundsAccountingTypeCode VARCHAR(MAX), 
    @PremiumFrequencyCode    VARCHAR(MAX), 
    @RiskPeriodMonth         VARCHAR(MAX), 
    @RiskPeriodYear          VARCHAR(MAX), 
    @ReportPeriodMonth       VARCHAR(MAX), 
    @ReportPeriodYear        VARCHAR(MAX), 
    @TransactionTypeCode     VARCHAR(MAX), 
    @PolicyNumber            VARCHAR(MAX), 
    @ReinsEffDatePol         VARCHAR(MAX), 
    @CedingBenefitTypeCode   VARCHAR(MAX), 
    @MlreBenefitCode         VARCHAR(MAX), 
    @Aar                     VARCHAR(MAX), 
    @InsuredDateOfBirth      VARCHAR(MAX), 
    @RateTable               VARCHAR(MAX), 
    @UnderwriterRating       VARCHAR(MAX), 
    @GrossPremium            VARCHAR(MAX), 
    @NetPremium              VARCHAR(MAX), 
    @Mfrs17BasicRider        VARCHAR(MAX), 
    @Mfrs17CellName          VARCHAR(MAX), 
    @Mfrs17TreatyCode        VARCHAR(MAX), 
    @InsuredGenderCode       VARCHAR(MAX), 
    @InsuredTobaccoUse       VARCHAR(MAX), 
    @InsuredGenderCode2nd    VARCHAR(MAX), 
    @InsuredTobaccoUse2nd    VARCHAR(MAX), 
    @InsuredOccupationCode   VARCHAR(MAX), 
    @PolicyStatusCode        VARCHAR(MAX), 
    @PolicyPaymentMethod     VARCHAR(MAX), 
    @DependantIndicator      VARCHAR(MAX), 
    @SpecialIndicator1       VARCHAR(MAX), 
    @SpecialIndicator2       VARCHAR(MAX), 
    @SpecialIndicator3       VARCHAR(MAX), 
    @FundCode                VARCHAR(MAX), 
    @LineOfBusiness          VARCHAR(MAX), 
    @GstIndicator            VARCHAR(MAX), 
    @StaffPlanIndicator      VARCHAR(MAX), 
    @IndicatorJointLife      VARCHAR(MAX), 
    @TerritoryOfIssueCode    VARCHAR(MAX), 
    @CurrencyCode            VARCHAR(MAX), 
    @TransactionPremium      VARCHAR(MAX), 
    @TransactionDiscount     VARCHAR(MAX), 
    @ProfitComm              VARCHAR(MAX), 
    @SoaQuarter              VARCHAR(MAX), 
    @ReportQuarter           VARCHAR(MAX), 
    @BatchTreatyID           INT, 
    @ValidateBoolean         INT, 
    @ValidateDuplicate       INT, 
    @ErrorCount              INT, 
    @ErrorMessage            VARCHAR(MAX),
    @FullErrorMessage        VARCHAR(MAX),
    @Total                   INT,
    @TotalSuccess            INT,
    @TotalFailed             INT

SELECT 
  @SoaQuarter = Quarter,
  @BatchTreatyID = TreatyId
FROM 
  RiDataBatches
WHERE
  Id = @RiDataBatchId;

SET @Total = 0;
SET @TotalSuccess = 0;
SET @TotalFailed = 0;

DECLARE RiDataCursor CURSOR FOR 
  SELECT 
    [Id], 
    [TreatyCode], 
    [ReinsBasisCode], 
    [FundsAccountingTypeCode], 
    [PremiumFrequencyCode], 
    [RiskPeriodMonth], 
    [RiskPeriodYear], 
    [ReportPeriodMonth], 
    [ReportPeriodYear], 
    [TransactionTypeCode], 
    [PolicyNumber], 
    [ReinsEffDatePol], 
    [CedingBenefitTypeCode], 
    [MlreBenefitCode], 
    [Aar], 
    [InsuredDateOfBirth], 
    [RateTable], 
    [UnderwriterRating], 
    [GrossPremium], 
    [NetPremium], 
    [Mfrs17BasicRider], 
    [Mfrs17CellName], 
    [Mfrs17TreatyCode], 
    [InsuredGenderCode], 
    [InsuredTobaccoUse], 
    [InsuredGenderCode2nd], 
    [InsuredTobaccoUse2nd], 
    [InsuredOccupationCode], 
    [PolicyStatusCode], 
    [PolicyPaymentMethod], 
    [DependantIndicator], 
    [SpecialIndicator1], 
    [SpecialIndicator2], 
    [SpecialIndicator3], 
    [FundCode], 
    [LineOfBusiness], 
    [GstIndicator], 
    [StaffPlanIndicator], 
    [IndicatorJointLife], 
    [TerritoryOfIssueCode], 
    [CurrencyCode],
    [TransactionPremium], 
    [TransactionDiscount],
    [ProfitComm]
  FROM   
    RiData 
  WHERE  
    RiDataBatchId = @RiDataBatchId; 

OPEN RiDataCursor 

FETCH NEXT FROM RiDataCursor INTO @RiDataId, @TreatyCode, @ReinsBasisCode, @FundsAccountingTypeCode, @PremiumFrequencyCode, @RiskPeriodMonth, 
  @RiskPeriodYear, @ReportPeriodMonth, @ReportPeriodYear, @TransactionTypeCode, @PolicyNumber, @ReinsEffDatePol, @CedingBenefitTypeCode, 
  @MlreBenefitCode, @Aar, @InsuredDateOfBirth, @RateTable, @UnderwriterRating, @GrossPremium, @NetPremium, @Mfrs17BasicRider, @Mfrs17CellName,
  @Mfrs17TreatyCode, @InsuredGenderCode, @InsuredTobaccoUse, @InsuredGenderCode2nd, @InsuredTobaccoUse2nd, @InsuredOccupationCode, @PolicyStatusCode, 
  @PolicyPaymentMethod, @DependantIndicator, @SpecialIndicator1, @SpecialIndicator2,  @SpecialIndicator3, @FundCode, @LineOfBusiness, 
  @GstIndicator, @StaffPlanIndicator, @IndicatorJointLife, @TerritoryOfIssueCode, @CurrencyCode, @TransactionPremium, @TransactionDiscount, @ProfitComm

WHILE @@FETCH_STATUS = 0 
  BEGIN 
    PRINT 'Processing Ri Data Id: ' + Cast(@RiDataId AS VARCHAR(255)); 

    SET @ErrorCount = 0; 
    SET @ErrorMessage = ''; 
    SET @FullErrorMessage = '{'; 

    IF @TreatyCode = '' OR @TreatyCode IS NULL 
      BEGIN 
          EXEC AddRiDataFinaliseError @Parameter='TreatyCode', @Error='Empty', @CurrentErrorMessage=@ErrorMessage OUTPUT, @CurrentFullErrorMessage=@FullErrorMessage OUTPUT
          SET @ErrorCount += 1 
      END 

    IF @ReinsBasisCode = '' OR @ReinsBasisCode IS NULL 
      BEGIN 
          EXEC AddRiDataFinaliseError @Parameter='ReinsBasisCode', @Error='Empty', @CurrentErrorMessage=@ErrorMessage OUTPUT, @CurrentFullErrorMessage=@FullErrorMessage OUTPUT
          SET @ErrorCount += 1 
      END 

    IF @FundsAccountingTypeCode = '' OR @FundsAccountingTypeCode IS NULL 
      BEGIN 
          EXEC AddRiDataFinaliseError @Parameter='FundsAccountingTypeCode', @Error='Empty', @CurrentErrorMessage=@ErrorMessage OUTPUT, @CurrentFullErrorMessage=@FullErrorMessage OUTPUT
          SET @ErrorCount += 1 
      END 

    IF @PremiumFrequencyCode = '' OR @PremiumFrequencyCode IS NULL 
      BEGIN 
          EXEC AddRiDataFinaliseError @Parameter='PremiumFrequencyCode', @Error='Empty', @CurrentErrorMessage=@ErrorMessage OUTPUT, @CurrentFullErrorMessage=@FullErrorMessage OUTPUT
          SET @ErrorCount += 1 
      END 

    IF @RiskPeriodMonth = '' OR @RiskPeriodMonth IS NULL 
      BEGIN 
          EXEC AddRiDataFinaliseError @Parameter='RiskPeriodMonth', @Error='Empty', @CurrentErrorMessage=@ErrorMessage OUTPUT, @CurrentFullErrorMessage=@FullErrorMessage OUTPUT
          SET @ErrorCount += 1 
      END 

    IF @RiskPeriodYear = '' OR @RiskPeriodYear IS NULL 
      BEGIN 
          EXEC AddRiDataFinaliseError @Parameter='RiskPeriodYear', @Error='Empty', @CurrentErrorMessage=@ErrorMessage OUTPUT, @CurrentFullErrorMessage=@FullErrorMessage OUTPUT
          SET @ErrorCount += 1 
      END 

    IF @ReportPeriodMonth = '' OR @ReportPeriodMonth IS NULL 
      BEGIN 
          EXEC AddRiDataFinaliseError @Parameter='ReportPeriodMonth', @Error='Empty', @CurrentErrorMessage=@ErrorMessage OUTPUT, @CurrentFullErrorMessage=@FullErrorMessage OUTPUT
          SET @ErrorCount += 1 
      END 

    IF @ReportPeriodYear = '' OR @ReportPeriodYear IS NULL 
      BEGIN 
          EXEC AddRiDataFinaliseError @Parameter='ReportPeriodYear', @Error='Empty', @CurrentErrorMessage=@ErrorMessage OUTPUT, @CurrentFullErrorMessage=@FullErrorMessage OUTPUT
          SET @ErrorCount += 1 
      END 

    IF @TransactionTypeCode = '' OR @TransactionTypeCode IS NULL 
      BEGIN 
          EXEC AddRiDataFinaliseError @Parameter='TransactionTypeCode', @Error='Empty', @CurrentErrorMessage=@ErrorMessage OUTPUT, @CurrentFullErrorMessage=@FullErrorMessage OUTPUT
          SET @ErrorCount += 1 
      END 

    IF @PolicyNumber = '' OR @PolicyNumber IS NULL 
      BEGIN 
          EXEC AddRiDataFinaliseError @Parameter='PolicyNumber', @Error='Empty', @CurrentErrorMessage=@ErrorMessage OUTPUT, @CurrentFullErrorMessage=@FullErrorMessage OUTPUT
          SET @ErrorCount += 1 
      END 

    IF @ReinsEffDatePol = '' OR @ReinsEffDatePol IS NULL 
      BEGIN 
          EXEC AddRiDataFinaliseError @Parameter='ReinsEffDatePol', @Error='Empty', @CurrentErrorMessage=@ErrorMessage OUTPUT, @CurrentFullErrorMessage=@FullErrorMessage OUTPUT
          SET @ErrorCount += 1 
      END 

    IF @CedingBenefitTypeCode = '' OR @CedingBenefitTypeCode IS NULL 
      BEGIN 
          EXEC AddRiDataFinaliseError @Parameter='CedingBenefitTypeCode', @Error='Empty', @CurrentErrorMessage=@ErrorMessage OUTPUT, @CurrentFullErrorMessage=@FullErrorMessage OUTPUT
          SET @ErrorCount += 1 
      END 

    IF @MlreBenefitCode = '' OR @MlreBenefitCode IS NULL 
      BEGIN 
          EXEC AddRiDataFinaliseError @Parameter='MlreBenefitCode', @Error='Empty', @CurrentErrorMessage=@ErrorMessage OUTPUT, @CurrentFullErrorMessage=@FullErrorMessage OUTPUT
          SET @ErrorCount += 1 
      END 

    IF @Aar = '' OR @Aar IS NULL 
      BEGIN 
          EXEC AddRiDataFinaliseError @Parameter='Aar', @Error='Empty', @CurrentErrorMessage=@ErrorMessage OUTPUT, @CurrentFullErrorMessage=@FullErrorMessage OUTPUT
          SET @ErrorCount += 1 
      END 

    IF @InsuredDateOfBirth = '' OR @InsuredDateOfBirth IS NULL 
      BEGIN 
          EXEC AddRiDataFinaliseError @Parameter='InsuredDateOfBirth', @Error='Empty', @CurrentErrorMessage=@ErrorMessage OUTPUT, @CurrentFullErrorMessage=@FullErrorMessage OUTPUT
          SET @ErrorCount += 1 
      END 

    IF @RateTable = '' OR @RateTable IS NULL 
      BEGIN 
          EXEC AddRiDataFinaliseError @Parameter='RateTable', @Error='Empty', @CurrentErrorMessage=@ErrorMessage OUTPUT, @CurrentFullErrorMessage=@FullErrorMessage OUTPUT
          SET @ErrorCount += 1 
      END 

    IF @UnderwriterRating = '' OR @UnderwriterRating IS NULL 
      BEGIN 
          EXEC AddRiDataFinaliseError @Parameter='UnderwriterRating', @Error='Empty', @CurrentErrorMessage=@ErrorMessage OUTPUT, @CurrentFullErrorMessage=@FullErrorMessage OUTPUT
          SET @ErrorCount += 1 
      END 

    IF @GrossPremium = '' OR @GrossPremium IS NULL 
      BEGIN 
          EXEC AddRiDataFinaliseError @Parameter='GrossPremium', @Error='Empty', @CurrentErrorMessage=@ErrorMessage OUTPUT, @CurrentFullErrorMessage=@FullErrorMessage OUTPUT
          SET @ErrorCount += 1 
      END 

    IF @NetPremium = '' OR @NetPremium IS NULL 
      BEGIN 
          EXEC AddRiDataFinaliseError @Parameter='NetPremium', @Error='Empty', @CurrentErrorMessage=@ErrorMessage OUTPUT, @CurrentFullErrorMessage=@FullErrorMessage OUTPUT
          SET @ErrorCount += 1 
      END 

    IF @Mfrs17BasicRider = '' OR @Mfrs17BasicRider IS NULL 
      BEGIN 
          EXEC AddRiDataFinaliseError @Parameter='Mfrs17BasicRider', @Error='Empty', @CurrentErrorMessage=@ErrorMessage OUTPUT, @CurrentFullErrorMessage=@FullErrorMessage OUTPUT
          SET @ErrorCount += 1 
      END 

    IF @Mfrs17CellName = '' OR @Mfrs17CellName IS NULL 
      BEGIN 
          EXEC AddRiDataFinaliseError @Parameter='Mfrs17CellName', @Error='Empty', @CurrentErrorMessage=@ErrorMessage OUTPUT, @CurrentFullErrorMessage=@FullErrorMessage OUTPUT
          SET @ErrorCount += 1 
      END 

    IF @Mfrs17TreatyCode = '' OR @Mfrs17TreatyCode IS NULL 
      BEGIN 
          EXEC AddRiDataFinaliseError @Parameter='Mfrs17TreatyCode', @Error='Empty', @CurrentErrorMessage=@ErrorMessage OUTPUT, @CurrentFullErrorMessage=@FullErrorMessage OUTPUT
          SET @ErrorCount += 1 
      END 

    IF @TransactionPremium = '' OR @TransactionPremium IS NULL 
      BEGIN 
          EXEC AddRiDataFinaliseError @Parameter='TransactionPremium', @Error='Empty', @CurrentErrorMessage=@ErrorMessage OUTPUT, @CurrentFullErrorMessage=@FullErrorMessage OUTPUT
          SET @ErrorCount += 1 
      END 

    IF @TransactionDiscount = '' OR @TransactionDiscount IS NULL 
      BEGIN 
          EXEC AddRiDataFinaliseError @Parameter='TransactionDiscount', @Error='Empty', @CurrentErrorMessage=@ErrorMessage OUTPUT, @CurrentFullErrorMessage=@FullErrorMessage OUTPUT
          SET @ErrorCount += 1 
      END 

    IF @PolicyStatusCode = '' OR @PolicyStatusCode IS NULL 
      BEGIN 
          EXEC AddRiDataFinaliseError @Parameter='PolicyStatusCode', @Error='Empty', @CurrentErrorMessage=@ErrorMessage OUTPUT, @CurrentFullErrorMessage=@FullErrorMessage OUTPUT
          SET @ErrorCount += 1 
      END 

    IF @ProfitComm = '' OR @ProfitComm IS NULL 
      BEGIN 
          EXEC AddRiDataFinaliseError @Parameter='ProfitComm', @Error='Empty', @CurrentErrorMessage=@ErrorMessage OUTPUT, @CurrentFullErrorMessage=@FullErrorMessage OUTPUT
          SET @ErrorCount += 1 
      END 

    IF @ReportPeriodYear <> '' AND @ReportPeriodMonth <> ''
      BEGIN
        SET @ReportQuarter = dbo.FormatQuarter(@ReportPeriodYear, @ReportPeriodMonth);
        IF (@SoaQuarter != @ReportQuarter)
          BEGIN 
            EXEC AddRiDataFinaliseError @Parameter='ReportPeriodMonth', @Error='Report Period Month does not match Quarter', @CurrentErrorMessage=@ErrorMessage OUTPUT, @CurrentFullErrorMessage=@FullErrorMessage OUTPUT
            EXEC AddRiDataFinaliseError @Parameter='ReportPeriodYear', @Error='Report Period Year does not match Quarter', @CurrentErrorMessage=@ErrorMessage OUTPUT, @CurrentFullErrorMessage=@FullErrorMessage OUTPUT
            SET @ErrorCount += 1 
          END
      END

    IF @TreatyCode <> ''
      BEGIN
        IF NOT EXISTS (SELECT * FROM TreatyCodes WHERE TreatyId = @BatchTreatyID AND Code = @TreatyCode) 
        BEGIN 
          EXEC AddRiDataFinaliseError @Parameter='TreatyCode', @Error='Treaty Code does not belong to Treaty', @CurrentErrorMessage=@ErrorMessage OUTPUT, @CurrentFullErrorMessage=@FullErrorMessage OUTPUT
          SET @ErrorCount += 1 
        END 
      END

    IF @MlreBenefitCode <> ''
      BEGIN
        IF NOT EXISTS (SELECT TOP(1) * FROM Benefits WHERE Code = @MlreBenefitCode) 
        BEGIN 
          EXEC AddRiDataFinaliseError @Parameter='MlreBenefitCode', @Error='Record not found in Benefit', @CurrentErrorMessage=@ErrorMessage OUTPUT, @CurrentFullErrorMessage=@FullErrorMessage OUTPUT
          SET @ErrorCount += 1 
        END 
      END

    IF @Mfrs17CellName <> ''
      BEGIN
        IF NOT EXISTS (SELECT TOP(1) * FROM Mfrs17CellMappings WHERE CellName = @Mfrs17CellName) 
        BEGIN 
          EXEC AddRiDataFinaliseError @Parameter='Mfrs17CellName', @Error='Record not found in MFRS17 Cell Mapping', @CurrentErrorMessage=@ErrorMessage OUTPUT, @CurrentFullErrorMessage=@FullErrorMessage OUTPUT
          SET @ErrorCount += 1 
        END 
      END

    IF @Mfrs17TreatyCode <> ''
      BEGIN
        IF NOT EXISTS (SELECT TOP(1) * FROM Mfrs17ContractCodeDetails WHERE ContractCode = @Mfrs17TreatyCode) 
        BEGIN 
          EXEC AddRiDataFinaliseError @Parameter='Mfrs17TreatyCode', @Error='Record not found in MFRS17 Contract Codes', @CurrentErrorMessage=@ErrorMessage OUTPUT, @CurrentFullErrorMessage=@FullErrorMessage OUTPUT
          SET @ErrorCount += 1 
        END 
      END

  -- IF @RateTable <> ''
  --   BEGIN
  --   IF NOT EXISTS (SELECT * 
  --          FROM   ratetables 
  --          WHERE  ratetablecode = @RateTable) 
  --   BEGIN 
  --     EXEC AddRiDataFinaliseError @Parameter='RateTable', @Error='Record not found in Rate Table', @CurrentErrorMessage=@ErrorMessage OUTPUT, @CurrentFullErrorMessage=@FullErrorMessage OUTPUT
  --     SET @ErrorCount += 1 
  --   END 
  --       END

    IF @ReinsBasisCode <> '' 
      BEGIN 
        EXEC @ValidateBoolean = Validatepicklist @Code=@ReinsBasisCode, @StdOutputType = 3 
        IF @ValidateBoolean = 0 
          BEGIN 
            EXEC AddRiDataFinaliseError @Parameter='ReinsBasisCode', @Error='Record not found in Pick List', @CurrentErrorMessage=@ErrorMessage OUTPUT, @CurrentFullErrorMessage=@FullErrorMessage OUTPUT
            SET @ErrorCount += 1 
          END 
      END 

    IF @PremiumFrequencyCode <> '' 
      BEGIN 
        EXEC @ValidateBoolean = Validatepicklist @Code=@PremiumFrequencyCode, @StdOutputType = 5 
        IF @ValidateBoolean = 0 
          BEGIN 
            EXEC AddRiDataFinaliseError @Parameter='PremiumFrequencyCode', @Error='Record not found in Pick List', @CurrentErrorMessage=@ErrorMessage OUTPUT, @CurrentFullErrorMessage=@FullErrorMessage OUTPUT
            SET @ErrorCount += 1 
          END 
      END 

    IF @TransactionTypeCode <> '' 
      BEGIN 
          EXEC @ValidateBoolean = Validatepicklist @Code=@TransactionTypeCode, @StdOutputType = 10 
          IF @ValidateBoolean = 0 
            BEGIN 
              EXEC AddRiDataFinaliseError @Parameter='TransactionTypeCode', @Error='Record not found in Pick List', @CurrentErrorMessage=@ErrorMessage OUTPUT, @CurrentFullErrorMessage=@FullErrorMessage OUTPUT
              SET @ErrorCount += 1 
            END 
      END 

    IF @CedingBenefitTypeCode <> '' 
      BEGIN 
        EXEC @ValidateBoolean = Validatepicklist @Code=@CedingBenefitTypeCode, @StdOutputType = 17 
        IF @ValidateBoolean = 0 
          BEGIN 
            EXEC AddRiDataFinaliseError @Parameter='CedingBenefitTypeCode', @Error='Record not found in Pick List', @CurrentErrorMessage=@ErrorMessage OUTPUT, @CurrentFullErrorMessage=@FullErrorMessage OUTPUT
            SET @ErrorCount += 1 
          END 
      END 

    IF @InsuredGenderCode <> '' 
      BEGIN 
        EXEC @ValidateBoolean = Validatepicklist @Code=@InsuredGenderCode, @StdOutputType = 30 
        IF @ValidateBoolean = 0 
          BEGIN 
            EXEC AddRiDataFinaliseError @Parameter='InsuredGenderCode', @Error='Record not found in Pick List', @CurrentErrorMessage=@ErrorMessage OUTPUT, @CurrentFullErrorMessage=@FullErrorMessage OUTPUT
            SET @ErrorCount += 1 
          END 
      END 

    IF @InsuredTobaccoUse <> '' 
      BEGIN 
        EXEC @ValidateBoolean = Validatepicklist @Code=@InsuredTobaccoUse, @StdOutputType = 31 
        IF @ValidateBoolean = 0 
          BEGIN 
            EXEC AddRiDataFinaliseError @Parameter='InsuredTobaccoUse', @Error='Record not found in Pick List', @CurrentErrorMessage=@ErrorMessage OUTPUT, @CurrentFullErrorMessage=@FullErrorMessage OUTPUT
            SET @ErrorCount += 1 
          END 
      END 

    IF @InsuredGenderCode2nd <> '' 
      BEGIN 
        EXEC @ValidateBoolean = Validatepicklist @Code=@InsuredGenderCode2nd, @StdOutputType = 39 
        IF @ValidateBoolean = 0 
          BEGIN 
            EXEC AddRiDataFinaliseError @Parameter='InsuredGenderCode2nd', @Error='Record not found in Pick List', @CurrentErrorMessage=@ErrorMessage OUTPUT, @CurrentFullErrorMessage=@FullErrorMessage OUTPUT
            SET @ErrorCount += 1 
          END 
      END 

    IF @InsuredTobaccoUse2nd <> '' 
      BEGIN 
        EXEC @ValidateBoolean = Validatepicklist @Code=@InsuredTobaccoUse2nd, @StdOutputType = 40 
        IF @ValidateBoolean = 0 
          BEGIN 
            EXEC AddRiDataFinaliseError @Parameter='InsuredTobaccoUse2nd', @Error='Record not found in Pick List', @CurrentErrorMessage=@ErrorMessage OUTPUT, @CurrentFullErrorMessage=@FullErrorMessage OUTPUT
            SET @ErrorCount += 1 
          END 
      END 

    IF @InsuredOccupationCode <> '' 
      BEGIN 
        EXEC @ValidateBoolean = Validatepicklist @Code=@InsuredOccupationCode, @StdOutputType = 33 
        IF @ValidateBoolean = 0 
          BEGIN 
            EXEC AddRiDataFinaliseError @Parameter='InsuredOccupationCode', @Error='Record not found in Pick List', @CurrentErrorMessage=@ErrorMessage OUTPUT, @CurrentFullErrorMessage=@FullErrorMessage OUTPUT
            SET @ErrorCount += 1 
          END 
      END 

    IF @PolicyStatusCode <> '' 
      BEGIN 
        EXEC @ValidateBoolean = Validatepicklist @Code=@PolicyStatusCode, @StdOutputType = 86 
        IF @ValidateBoolean = 0 
          BEGIN 
            EXEC AddRiDataFinaliseError @Parameter='PolicyStatusCode', @Error='Record not found in Pick List', @CurrentErrorMessage=@ErrorMessage OUTPUT, @CurrentFullErrorMessage=@FullErrorMessage OUTPUT
            SET @ErrorCount += 1 
          END 
      END 

    IF @PolicyPaymentMethod <> '' 
      BEGIN 
        EXEC @ValidateBoolean = Validatepicklist @Code=@PolicyPaymentMethod, @StdOutputType = 93 
        IF @ValidateBoolean = 0 
          BEGIN 
            EXEC AddRiDataFinaliseError @Parameter='PolicyPaymentMethod', @Error='Record not found in Pick List', @CurrentErrorMessage=@ErrorMessage OUTPUT, @CurrentFullErrorMessage=@FullErrorMessage OUTPUT
            SET @ErrorCount += 1 
          END 
      END 

    IF @DependantIndicator <> '' 
      BEGIN 
        EXEC @ValidateBoolean = Validatepicklist @Code=@DependantIndicator, @StdOutputType = 126 
        IF @ValidateBoolean = 0 
          BEGIN 
            EXEC AddRiDataFinaliseError @Parameter='DependantIndicator', @Error='Record not found in Pick List', @CurrentErrorMessage=@ErrorMessage OUTPUT, @CurrentFullErrorMessage=@FullErrorMessage OUTPUT
            SET @ErrorCount += 1 
          END 
      END 

    IF @SpecialIndicator1 <> '' 
      BEGIN 
        EXEC @ValidateBoolean = Validatepicklist @Code=@SpecialIndicator1, @StdOutputType = 142 
        IF @ValidateBoolean = 0 
          BEGIN 
            EXEC AddRiDataFinaliseError @Parameter='SpecialIndicator1', @Error='Record not found in Pick List', @CurrentErrorMessage=@ErrorMessage OUTPUT, @CurrentFullErrorMessage=@FullErrorMessage OUTPUT
            SET @ErrorCount += 1 
          END 
      END 

    IF @SpecialIndicator2 <> '' 
      BEGIN 
        EXEC @ValidateBoolean = Validatepicklist @Code=@SpecialIndicator2, @StdOutputType = 143 
        IF @ValidateBoolean = 0 
          BEGIN 
            EXEC AddRiDataFinaliseError @Parameter='SpecialIndicator2', @Error='Record not found in Pick List', @CurrentErrorMessage=@ErrorMessage OUTPUT, @CurrentFullErrorMessage=@FullErrorMessage OUTPUT
            SET @ErrorCount += 1 
          END 
      END 

    IF @SpecialIndicator3 <> '' 
      BEGIN 
        EXEC @ValidateBoolean = Validatepicklist @Code=@SpecialIndicator3, @StdOutputType = 144 
        IF @ValidateBoolean = 0 
          BEGIN 
            EXEC AddRiDataFinaliseError @Parameter='SpecialIndicator3', @Error='Record not found in Pick List', @CurrentErrorMessage=@ErrorMessage OUTPUT, @CurrentFullErrorMessage=@FullErrorMessage OUTPUT
            SET @ErrorCount += 1 
          END 
      END 

    IF @FundsAccountingTypeCode <> '' 
      BEGIN 
        EXEC @ValidateBoolean = Validatepicklist @Code=@FundsAccountingTypeCode, @StdOutputType = 4 
        IF @ValidateBoolean = 0 
          BEGIN 
            EXEC AddRiDataFinaliseError @Parameter='FundsAccountingTypeCode', @Error='Record not found in Pick List', @CurrentErrorMessage=@ErrorMessage OUTPUT, @CurrentFullErrorMessage=@FullErrorMessage OUTPUT
            SET @ErrorCount += 1 
          END 
      END 

    IF @Mfrs17BasicRider <> '' 
      BEGIN 
        EXEC @ValidateBoolean = Validatepicklist @Code=@Mfrs17BasicRider, @StdOutputType = 152 
        IF @ValidateBoolean = 0 
          BEGIN 
            EXEC AddRiDataFinaliseError @Parameter='Mfrs17BasicRider', @Error='Record not found in Pick List', @CurrentErrorMessage=@ErrorMessage OUTPUT, @CurrentFullErrorMessage=@FullErrorMessage OUTPUT
            SET @ErrorCount += 1 
          END 
      END 

    IF @FundCode <> '' 
      BEGIN 
        EXEC @ValidateBoolean = Validatepicklist @Code=@FundCode, @StdOutputType = 95 
        IF @ValidateBoolean = 0 
          BEGIN 
            EXEC AddRiDataFinaliseError @Parameter='FundCode', @Error='Record not found in Pick List', @CurrentErrorMessage=@ErrorMessage OUTPUT, @CurrentFullErrorMessage=@FullErrorMessage OUTPUT
            SET @ErrorCount += 1 
          END 
      END 

    IF @LineOfBusiness <> '' 
      BEGIN 
        EXEC @ValidateBoolean = Validatepicklist @Code=@LineOfBusiness, @StdOutputType = 96 
        IF @ValidateBoolean = 0 
          BEGIN 
            EXEC AddRiDataFinaliseError @Parameter='LineOfBusiness', @Error='Record not found in Pick List', @CurrentErrorMessage=@ErrorMessage OUTPUT, @CurrentFullErrorMessage=@FullErrorMessage OUTPUT
            SET @ErrorCount += 1 
          END 
      END 

    IF @GstIndicator <> '' 
      BEGIN 
        EXEC @ValidateBoolean = Validatepicklist @Code=@GstIndicator, @StdOutputType = 147 
        IF @ValidateBoolean = 0 
          BEGIN 
            EXEC AddRiDataFinaliseError @Parameter='GstIndicator', @Error='Record not found in Pick List', @CurrentErrorMessage=@ErrorMessage OUTPUT, @CurrentFullErrorMessage=@FullErrorMessage OUTPUT
            SET @ErrorCount += 1 
          END 
      END 

    IF @StaffPlanIndicator <> '' 
      BEGIN 
        EXEC @ValidateBoolean = Validatepicklist @Code=@StaffPlanIndicator, @StdOutputType = 105 
        IF @ValidateBoolean = 0 
          BEGIN 
            EXEC AddRiDataFinaliseError @Parameter='StaffPlanIndicator', @Error='Record not found in Pick List', @CurrentErrorMessage=@ErrorMessage OUTPUT, @CurrentFullErrorMessage=@FullErrorMessage OUTPUT
            SET @ErrorCount += 1 
          END 
      END 

    IF @IndicatorJointLife <> '' 
      BEGIN 
        EXEC @ValidateBoolean = Validatepicklist @Code=@IndicatorJointLife, @StdOutputType = 145 
        IF @ValidateBoolean = 0 
          BEGIN
            EXEC AddRiDataFinaliseError @Parameter='IndicatorJointLife', @Error='Record not found in Pick List', @CurrentErrorMessage=@ErrorMessage OUTPUT, @CurrentFullErrorMessage=@FullErrorMessage OUTPUT
            SET @ErrorCount += 1 
          END
      END 

    IF @TerritoryOfIssueCode <> '' 
      BEGIN 
        EXEC @ValidateBoolean = Validatepicklist @Code=@TerritoryOfIssueCode, @StdOutputType = 103
        IF @ValidateBoolean = 0 
          BEGIN 
            EXEC AddRiDataFinaliseError @Parameter='TerritoryOfIssueCode', @Error='Record not found in Pick List', @CurrentErrorMessage=@ErrorMessage OUTPUT, @CurrentFullErrorMessage=@FullErrorMessage OUTPUT
            SET @ErrorCount += 1 
          END 
      END 

    IF @CurrencyCode <> '' 
      BEGIN 
        EXEC @ValidateBoolean = Validatepicklist @Code=@CurrencyCode, @StdOutputType = 104 
        IF @ValidateBoolean = 0 
          BEGIN 
            EXEC AddRiDataFinaliseError @Parameter='CurrencyCode', @Error='Record not found in Pick List', @CurrentErrorMessage=@ErrorMessage OUTPUT, @CurrentFullErrorMessage=@FullErrorMessage OUTPUT
            SET @ErrorCount += 1 
          END 
      END 

    IF @ProfitComm <> '' 
      BEGIN 
        EXEC @ValidateBoolean = Validatepicklist @Code=@ProfitComm, @StdOutputType = 221 
        IF @ValidateBoolean = 0 
          BEGIN 
            EXEC AddRiDataFinaliseError @Parameter='ProfitComm', @Error='Record not found in Pick List', @CurrentErrorMessage=@ErrorMessage OUTPUT, @CurrentFullErrorMessage=@FullErrorMessage OUTPUT
            SET @ErrorCount += 1 
          END 
      END 

  -- EXEC @ValidateDuplicate = RiDataIsDuplicate @RiDataId=@RiDataId 
  -- IF @ValidateDuplicate > 0 
  --   BEGIN 
  --     SET @ErrorMessage += '"Duplicate record found!",'; 
  --     SET @ErrorCount += 1 
  --   END 

    SET @FullErrorMessage += '"FinaliseError": [' + @ErrorMessage + ']}';

    IF @ErrorCount > 0
      BEGIN 
        UPDATE RiData 
        SET    FinaliseStatus = 3, 
               Errors = @FullErrorMessage 
        WHERE  Id = @RiDataId; 
        SET @TotalFailed += 1;
      END 
    ELSE 
      BEGIN 
        UPDATE RiData 
        SET    FinaliseStatus = 2, 
               Errors = NULL 
        WHERE  Id = @RiDataId; 
        SET @TotalSuccess += 1;
      END 

    SET @Total += 1;

    FETCH NEXT FROM RiDataCursor INTO @RiDataId, @TreatyCode, @ReinsBasisCode, @FundsAccountingTypeCode, @PremiumFrequencyCode, @RiskPeriodMonth, 
      @RiskPeriodYear, @ReportPeriodMonth, @ReportPeriodYear, @TransactionTypeCode, @PolicyNumber, @ReinsEffDatePol, @CedingBenefitTypeCode, 
      @MlreBenefitCode, @Aar, @InsuredDateOfBirth, @RateTable, @UnderwriterRating, @GrossPremium, @NetPremium, @Mfrs17BasicRider, @Mfrs17CellName,
      @Mfrs17TreatyCode, @InsuredGenderCode, @InsuredTobaccoUse, @InsuredGenderCode2nd, @InsuredTobaccoUse2nd, @InsuredOccupationCode, @PolicyStatusCode, 
      @PolicyPaymentMethod, @DependantIndicator, @SpecialIndicator1, @SpecialIndicator2,  @SpecialIndicator3, @FundCode, @LineOfBusiness, 
      @GstIndicator, @StaffPlanIndicator, @IndicatorJointLife, @TerritoryOfIssueCode, @CurrencyCode, @TransactionPremium, @TransactionDiscount, @ProfitComm
  END 

CLOSE RiDataCursor; 

DEALLOCATE RiDataCursor; 

SET @Result = '{"Total":'+ CAST(@Total as varchar(10)) + 
      ', "Success": ' + CAST(@TotalSuccess as varchar(10)) + 
      ', "Failed": ' + CAST(@TotalFailed as varchar(10)) + '}';


