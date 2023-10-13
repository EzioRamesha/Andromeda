CREATE OR ALTER PROCEDURE [dbo].[PerLifeClaimDataValidation](@PerLifeClaimId INT, @Result VARCHAR(MAX) OUTPUT)

AS

SET NOCOUNT ON;

DECLARE
	@Id INT,
	@InsuredDateOfBirth DATETIME2,
	@InsuredName NVARCHAR(255),
	@ClaimCode NVARCHAR(255),
	@Errors NVARCHAR(255)

DECLARE PerLifeClaimDataCursor CURSOR FOR 
	SELECT
		PLCD.Id,
		CR.InsuredDateOfBirth,
		CR.InsuredName,
		CR.ClaimCode
	FROM 
		PerLifeClaimData AS PLCD
	JOIN
		ClaimRegisterHistories AS CRH
		ON CRH.Id = PLCD.ClaimRegisterHistoryId
	JOIN
		ClaimRegister AS CR
		ON CR.Id = CRH.ClaimRegisterId
	WHERE 
		PLCD.PerLifeClaimId = @PerLifeClaimId
		AND IsException = 0
		AND (CR.InsuredDateOfBirth IS NULL OR ISNULL(CR.InsuredName,'') = ''  OR ISNULL(CR.ClaimCode,'') = '')

OPEN PerLifeClaimDataCursor

FETCH NEXT FROM PerLifeClaimDataCursor INTO @Id, @InsuredDateOfBirth, @InsuredName, @ClaimCode

WHILE @@FETCH_STATUS = 0
BEGIN
	SET @Errors = ''

	IF (@InsuredDateOfBirth IS NULL)
	BEGIN
		SET @Errors = @Errors + 'Date of Birth'
	END

	IF (ISNULL(@InsuredName,'') = '')
	BEGIN
		IF (@Errors <> '')
		BEGIN
			SET @Errors = @Errors + ', Insured Name'
		END
		ELSE
		BEGIN
			SET @Errors = @Errors + 'Insured Name'
		END
	END

	IF (ISNULL(@ClaimCode,'') = '')
	BEGIN
		IF (@Errors <> '')
		BEGIN
			SET @Errors = @Errors + ', Claim Code'
		END
		ELSE
		BEGIN
			SET @Errors = @Errors + 'Claim Code'
		END
	END

	SET @Errors = @Errors + ' should not be empty'

	UPDATE 
		PerLifeClaimData 
	SET 
		IsException = 1,
		Errors = @Errors
	WHERE 
		Id = @Id

	FETCH NEXT FROM PerLifeClaimDataCursor INTO @Id, @InsuredDateOfBirth, @InsuredName, @ClaimCode
END

CLOSE PerLifeClaimDataCursor

DEALLOCATE PerLifeClaimDataCursor