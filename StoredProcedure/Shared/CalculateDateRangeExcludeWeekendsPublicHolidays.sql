CREATE OR ALTER FUNCTION [dbo].[CalculateDateRangeExcludeWeekendsPublicHolidays]  (@StartDate DATETIME, @EndDate DATETIME) RETURNS INT 

AS 

BEGIN
	DECLARE 
		@Days INT = -1,
		@ToSubtract INT = 0

	IF @StartDate IS NOT NULL AND @EndDate IS NOT NULL
		BEGIN
			SET @Days = DATEDIFF(D, @StartDate, @EndDate)
			DECLARE @CurrentDate DATETIME = @StartDate

			WHILE(@CurrentDate < @EndDate)
			BEGIN
				IF DATEPART(dw, @CurrentDate) = 1 OR DATEPART(dw, @CurrentDate) = 7
				BEGIN
					SET @ToSubtract = @ToSubtract + 1
				END

				IF(SELECT COUNT(*) FROM PublicHolidayDetails WHERE PublicHolidayDate = @CurrentDate) > 0
				BEGIN
					SET @ToSubtract = @ToSubtract + 1
				END

				SET @CurrentDate = DATEADD(D, 1, @CurrentDate)
			END

			SET @Days = @Days - @ToSubtract
		END

	RETURN @Days 
END