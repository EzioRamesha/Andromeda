CREATE OR ALTER FUNCTION [dbo].[FormatQuarter] (@Year VARCHAR(MAX), @Month VARCHAR(MAX)) RETURNS VARCHAR(MAX)

AS

BEGIN
	DECLARE @Quarter VARCHAR(MAX);

	SET @Quarter = @Year + ' ' +
		CASE
			WHEN @Month = 1 THEN 'Q1'
			WHEN @Month = 2 THEN 'Q1'
			WHEN @Month = 3 THEN 'Q1'
			WHEN @Month = 4 THEN 'Q2'
			WHEN @Month = 5 THEN 'Q2'
			WHEN @Month = 6 THEN 'Q2'
			WHEN @Month = 7 THEN 'Q3'
			WHEN @Month = 8 THEN 'Q3'
			WHEN @Month = 9 THEN 'Q3'
			WHEN @Month = 10 THEN 'Q4'
			WHEN @Month = 11 THEN 'Q4'
			WHEN @Month = 12 THEN 'Q4'
		END

	RETURN @Quarter;
END