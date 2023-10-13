CREATE OR ALTER PROCEDURE [dbo].[SplitQuarter] (
	@Quarter VARCHAR(MAX),
	@Year INT OUTPUT,
	@StartMonth INT OUTPUT,
	@EndMonth INT OUTPUT) 

AS

DECLARE @QuarterMonth INT;

IF (dbo.IsNull(@Quarter) = 1)
	RETURN;

SET @Year = PARSENAME(REPLACE(@Quarter,' Q','.'),2);
SET @QuarterMonth = PARSENAME(REPLACE(@Quarter,' Q','.'),1);

IF (@QuarterMonth = 1)
	BEGIN
		SET @StartMonth = 1;
		SET @EndMonth = 3;
	END
ELSE IF (@QuarterMonth = 2)
	BEGIN
		SET @StartMonth = 4;
		SET @EndMonth = 6;
	END
ELSE IF (@QuarterMonth = 3)
	BEGIN
		SET @StartMonth = 7;
		SET @EndMonth = 9;
	END
ELSE IF (@QuarterMonth = 4)
	BEGIN
		SET @StartMonth = 10;
		SET @EndMonth = 12;
	END