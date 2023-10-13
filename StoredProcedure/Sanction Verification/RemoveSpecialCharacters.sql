CREATE OR ALTER FUNCTION [dbo].[RemoveSpecialCharacters] (@string VARCHAR(MAX)) RETURNS varchar(max)

AS

BEGIN
    WHILE @string LIKE '%[$&+,:;=?@#|''<>.-^*()%!]%'
    BEGIN
        SET @string = REPLACE(@string,SUBSTRING(@string,PATINDEX('%[$&+,:;=?@#|''<>.-^*()%!]%',@string),1), '')
    END

    SET @string = UPPER(@string)

    RETURN @string
END