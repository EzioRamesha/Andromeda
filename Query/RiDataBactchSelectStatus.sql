/*
StatusPending = 1;
StatusSubmitForProcessing = 2;
StatusProcessing = 3;
StatusSuccess = 4;
StatusFailed = 5;
StatusSubmitForFinalise = 6;
StatusFinalising = 7;
StatusFinalised = 8;
StatusFinaliseFailed = 9;
*/

/*
SELECT TOP(100) *
FROM [RiDataBatches]
ORDER BY [Id] DESC;
*/

/*
SELECT TOP(100) *
FROM [RiDataBatches]
WHERE [Status] = 8
ORDER BY [Id] DESC;
*/

/*
SELECT *
FROM [RiDataBatches]
WHERE [Id] IN (1415)
ORDER BY [Status] DESC, [Id];
*/

/*
SELECT [Id]
      ,[Status]
      ,[CedantId]
      ,[TreatyCodeId]
      ,[RiDataConfigId]
      ,[Configs]
      ,[OverrideProperties]
      ,[Quarter]
      ,[TreatyId]
FROM [RiDataBatches]
WHERE [Status] IN (2, 3)
ORDER BY [Status] DESC, [Id];
*/

/*
SELECT [Id]
      ,[Status]
      ,[CedantId]
      ,[TreatyCodeId]
      ,[RiDataConfigId]
      ,[Configs]
      ,[OverrideProperties]
      ,[Quarter]
      ,[TreatyId]
FROM [RiDataBatches]
WHERE [Status] IN (6, 7)
ORDER BY [Status] DESC, [Id];
*/

/*
SELECT * FROM [StatusHistories] WHERE [ObjectId] IN (
	SELECT [Id]
	FROM [RiDataBatches]
	WHERE [Status] IN (2, 3)
);
*/
/*
SELECT * FROM [StatusHistories] WHERE [ObjectId] IN (
	SELECT [Id]
	FROM [RiDataBatches]
	WHERE [Status] IN (6, 7)
);
*/

/*
SELECT * FROM [StatusHistories] WHERE [ObjectId] IN (1159, 1416)
ORDER BY [ObjectId], [Id];
*/

/*
SELECT * FROM [RiDataBatchStatusFiles] WHERE [RiDataBatchId] = 1415;
*/
