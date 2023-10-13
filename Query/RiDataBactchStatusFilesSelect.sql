

/*
SELECT * FROM [RiDataBatchStatusFiles]
WHERE [RiDataBatchId] IN (38)
ORDER BY [RiDataBatchId], [Id] DESC;
*/

/*
SELECT * FROM [RiDataBatchStatusFiles]
WHERE [RiDataBatchId] IN (
	SELECT [Id]
	FROM [RiDataBatches]
	WHERE [Status] IN (2, 3)
)
ORDER BY [RiDataBatchId], [Id] DESC;
*/
