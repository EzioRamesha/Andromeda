CREATE OR ALTER FUNCTION [dbo].[IsNull] (@Value VARCHAR(MAX)) RETURNS BIT

AS

BEGIN
	DECLARE @IsNull BIT;

	IF @Value = '' OR @Value IS NULL
		BEGIN
			SET @IsNull = 1;
		END
	ELSE
		BEGIN
			SET @IsNull = 0;
		END

	RETURN @IsNull;
END