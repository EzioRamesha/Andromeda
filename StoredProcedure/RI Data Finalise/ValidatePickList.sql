CREATE OR ALTER PROCEDURE [dbo].[ValidatePickList](@Code NVARCHAR(50), @StdOutputType NVARCHAR(50))

AS   

DECLARE @PicklistId INT,
@StdOutputId INT,
@return NVARCHAR(10)

SET @StdOutputId = (SELECT Id FROM StandardOutputs WHERE Type = @StdOutputType);

SET @PicklistId = (SELECT Id FROM PickLists WHERE StandardOutputId = @StdOutputId);

IF EXISTS (SELECT * FROM PickListDetails WHERE PickListId = @PicklistId AND Code = @Code)
	SET @return = 1;
ELSE
	SET @return = 0;

return @return;