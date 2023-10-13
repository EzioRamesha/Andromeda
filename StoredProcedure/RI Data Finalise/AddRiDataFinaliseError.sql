CREATE OR ALTER PROCEDURE [dbo].[AddRiDataFinaliseError](@Parameter VARCHAR(MAX), @Error VARCHAR(MAX), @CurrentErrorMessage VARCHAR(MAX) OUTPUT, @CurrentFullErrorMessage VARCHAR(MAX) OUTPUT) 

AS 

SET @CurrentErrorMessage += '"' + @Parameter + ': ' + @Error + '",';
SET @CurrentFullErrorMessage += '"' + @Parameter + '": "' + @Parameter + ': ' + @Error + '",';