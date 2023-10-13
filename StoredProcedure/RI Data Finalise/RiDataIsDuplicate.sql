CREATE OR ALTER PROCEDURE [dbo].[RiDataIsDuplicate](@RiDataId INT)

AS

DECLARE @TreatyCode VARCHAR(255),
@RiskPeriodMonth VARCHAR(255),
@RiskPeriodYear VARCHAR(255),
@TransactionTypeCode VARCHAR(255),
@PolicyNumber VARCHAR(255),
@CedingBasicPlanCode VARCHAR(255),
@CedingBenefitTypeCode VARCHAR(255),
@CedingBenefitRiskCode VARCHAR(255),
@InsuredName VARCHAR(255),
@NetPremium VARCHAR(255),
@RiDataBatchId VARCHAR(255),
@CountNotSameBatch INT,
@CountSameBatch INT,
@ReturnValue INT = 0

SELECT 
	@TreatyCode =  TreatyCode,
	@RiskPeriodMonth =  RiskPeriodMonth,
	@RiskPeriodYear =  RiskPeriodYear,
	@TransactionTypeCode =  TransactionTypeCode,
	@PolicyNumber =  PolicyNumber,
	@CedingBasicPlanCode =  CedingBasicPlanCode,
	@CedingBenefitTypeCode =  CedingBenefitTypeCode,
	@CedingBenefitRiskCode =  CedingBenefitRiskCode,
	@InsuredName =  InsuredName,
	@NetPremium =  NetPremium,
	@RiDataBatchId = RiDataBatchId
FROM 
	RiData
WHERE 
	Id = @RiDataId;


SET @CountNotSameBatch = (
	SELECT COUNT(*) FROM RiData JOIN RiDataBatches ON RiData.RiDataBatchId = RiDataBatches.Id
	WHERE 
		RiData.TreatyCode = @TreatyCode 
		AND RiData.RiskPeriodMonth = @RiskPeriodMonth 
		AND RiData.RiskPeriodYear = @RiskPeriodYear 
		AND RiData.TransactionTypeCode = @TransactionTypeCode 
		AND RiData.PolicyNumber = @PolicyNumber 
		AND RiData.CedingBasicPlanCode = @CedingBasicPlanCode 
		AND RiData.CedingBenefitTypeCode = @CedingBenefitTypeCode 
		AND RiData.CedingBenefitRiskCode = @CedingBenefitRiskCode 
		AND RiData.InsuredName = @InsuredName 
		AND RiData.NetPremium = @NetPremium 
		AND RiData.RiDataBatchId <> @RiDataBatchId
		AND RiDataBatches.Status = 8
)

If @CountNotSameBatch > 0
Begin
    SET @ReturnValue = @ReturnValue + @CountNotSameBatch
End

SET @CountSameBatch = (
	SELECT COUNT(*) FROM RiData 
	WHERE 
		TreatyCode = @TreatyCode 
		AND RiskPeriodMonth = @RiskPeriodMonth 
		AND RiskPeriodYear = @RiskPeriodYear 
		AND TransactionTypeCode = @TransactionTypeCode 
		AND PolicyNumber = @PolicyNumber 
		AND CedingBasicPlanCode = @CedingBasicPlanCode 
		AND CedingBenefitTypeCode = @CedingBenefitTypeCode 
		AND CedingBenefitRiskCode = @CedingBenefitRiskCode 
		AND InsuredName = @InsuredName 
		AND NetPremium = @NetPremium 
		AND RiDataBatchId = @RiDataBatchId
		AND Id <> @RiDataId
)

If @CountSameBatch > 0
Begin
    SET @ReturnValue = @ReturnValue + @CountSameBatch
End

return @ReturnValue


