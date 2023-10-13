CREATE OR ALTER PROCEDURE [dbo].[AddSanctionVerificationDetail] (
	@SanctionVerificationId INT, 
	@MatchedSanctionIds IdTable READONLY,
	@ModuleId INT, 
	@ObjectId INT,
	@BatchId INT = NULL,
	@Rule INT,
	@TreatyCode VARCHAR(MAX),
	@CedingPlanCode VARCHAR(MAX),
	@PolicyNumber VARCHAR(MAX),
	@InsuredName VARCHAR(128),
	@InsuredDateOfBirth VARCHAR(100),
	@CedingCompany VARCHAR(MAX),
	@InsuredIcNumber VARCHAR(MAX) = NULL,
	@SoaQuarter VARCHAR(MAX) = NULL,
	@SumReins VARCHAR(MAX) = NULL,
	@ClaimAmount VARCHAR(MAX) = NULL,
	@Category VARCHAR(30) = NULL,
	-- NEW
	@LineOfBusiness VARCHAR(MAX) = NULL,
	@PolicyCommencementDate VARCHAR(MAX) = NULL,
	@PolicyStatusCode VARCHAR(MAX) = NULL,
	@RiskCoverageEndDate VARCHAR(MAX) = NULL,
	@GrossPremium VARCHAR(MAX) = NULL
)

AS

DECLARE
	@SanctionRefNumber VARCHAR(MAX),
	@SanctionIdNumber VARCHAR(MAX),
	@SanctionAddress VARCHAR(MAX),
	@PreviousDecision VARCHAR(MAX),
	@PreviousDecisionRemark VARCHAR(MAX),
	@PolicyStatusCodeStr VARCHAR(MAX)

SET @PreviousDecision = dbo.GetConstant('PreviousDecisionPending');
IF EXISTS (SELECT * FROM SanctionBlacklists WHERE PolicyNumber = @PolicyNumber AND InsuredName = @InsuredName)
	SET @PreviousDecision = dbo.GetConstant('PreviousDecisionExactMatch');
ELSE IF EXISTS (SELECT * FROM SanctionWhitelists WHERE PolicyNumber = @PolicyNumber AND InsuredName = @InsuredName)
	BEGIN
		SET @PreviousDecision = dbo.GetConstant('PreviousDecisionWhitelist');
		SELECT 
			@PreviousDecisionRemark = Reason
		FROM 
			SanctionWhitelists
		WHERE
			PolicyNumber = @PolicyNumber AND InsuredName = @InsuredName
	END

SELECT 
	@SanctionRefNumber = COALESCE(@SanctionRefNumber + '\n', '') + RefNumber
FROM
	Sanctions
WHERE
	Id IN (SELECT Id FROM @MatchedSanctionIds)

SELECT 
	@SanctionIdNumber = COALESCE(@SanctionIdNumber + '\n', '') + IdNumber
FROM
	SanctionIdentities
WHERE
	SanctionId IN (SELECT Id FROM @MatchedSanctionIds)

SELECT 
	@SanctionAddress = COALESCE(@SanctionAddress + '\n', '') + CAST(Address AS VARCHAR(MAX))
FROM
	SanctionAddresses
WHERE
	SanctionId IN (SELECT Id FROM @MatchedSanctionIds)

IF (dbo.IsNull(@PolicyStatusCode) = 0)
	BEGIN
		SET @PolicyStatusCodeStr = CASE WHEN @PolicyStatusCode = 'IF' THEN 'Active' ELSE 'Closed' END
	END

INSERT INTO 
	SanctionVerificationDetails (
		SanctionVerificationId,
		ModuleId,
		ObjectId,
		[Rule],
		-- UploadDate,
		Category,
		CedingCompany,
		TreatyCode,
		CedingPlanCode,
		PolicyNumber,
		InsuredName,
		InsuredDateOfBirth,
		InsuredIcNumber,
		SoaQuarter,
		SumReins,
		ClaimAmount,
		SanctionRefNumber,
		SanctionIdNumber,
		SanctionAddress,
		LineOfBusiness,
		PolicyCommencementDate,
		PolicyStatusCode,
		RiskCoverageEndDate,
		GrossPremium,
		IsWhitelist,
		IsExactMatch,
		BatchId,
		PreviousDecision,
		PreviousDecisionRemark,
		CreatedAt,
		UpdatedAt,
		CreatedById,
		UpdatedById
	)
VALUES (
	@SanctionVerificationId,
	@ModuleId,
	@ObjectId,
	@Rule,
	-- @UploadDate,
	@Category,
	@CedingCompany,
	@TreatyCode,
	@CedingPlanCode,
	@PolicyNumber,
	@InsuredName,
	@InsuredDateOfBirth,
	@InsuredIcNumber,
	@SoaQuarter,
	@SumReins,
	@ClaimAmount,
	@SanctionRefNumber,
	@SanctionIdNumber,
	@SanctionAddress,
	@LineOfBusiness,
	@PolicyCommencementDate,
	@PolicyStatusCodeStr,
	@RiskCoverageEndDate,
	@GrossPremium,
	0,
	0,
	@BatchId,
	@PreviousDecision,
	@PreviousDecisionRemark,
	GETDATE(),
	GETDATE(),
	dbo.GetConstantInt('AuthUserId'),
	dbo.GetConstantInt('AuthUserId')
);