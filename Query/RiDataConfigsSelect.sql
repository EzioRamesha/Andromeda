
/*
SELECT * FROM [RiDataConfigs]
WHERE [Id] IN (
	SELECT [RiDataConfigId]
	FROM [RiDataFiles]
	WHERE [RiDataBatchId] IN (1340)
);
*/

SELECT * FROM [RiDataConfigs]
WHERE [Id] IN (154,155);
