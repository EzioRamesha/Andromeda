CREATE OR ALTER FUNCTION [dbo].[JoinIdTable] (@Data IdTable READONLY, @Glue VARCHAR(5)) RETURNS VARCHAR(MAX)

AS

BEGIN
	DECLARE @Result VARCHAR(MAX);

	SET @Result = NULL;

	SELECT 
		@Result = COALESCE(@Result + @Glue, '') + CAST(Id AS VARCHAR(20))
	FROM
		@Data

	RETURN @Result;
END