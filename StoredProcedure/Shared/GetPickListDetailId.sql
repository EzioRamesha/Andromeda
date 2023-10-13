CREATE OR ALTER FUNCTION [dbo].[GetPickListDetailId](@Code NVARCHAR(50), @Type INT) RETURNS INT

AS   

BEGIN
	DECLARE 
		@PickListDetailId INT

	SELECT
		@PickListDetailId = PickListDetails.Id
	FROM
		PickListDetails
	JOIN
		PickLists ON PickLists.Id = PickListDetails.PickListId
	WHERE
		PickLists.Id = @Type AND
		PickListDetails.Code = @Code;

	RETURN @PickListDetailId;
END