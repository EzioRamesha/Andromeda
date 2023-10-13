
/*
SELECT TOP (1000) [Id]
      ,[Type]
      ,[Controller]
      ,[ObjectId]
      ,[Description]
      ,[IpAddress]
      ,[Data]
      ,[CreatedAt]
      ,[CreatedById]
FROM [UserTrails]
ORDER BY Id DESC;
*/

SELECT TOP (1000) [Id]
      ,[Type]
      ,[Controller]
      ,[ObjectId]
      ,[Description]
      ,[IpAddress]
      ,[Data]
      ,[CreatedAt]
      ,[CreatedById]
FROM [UserTrails]
WHERE [ObjectId] IN (1535, 1532)
AND [Controller] = 'RiDataFile'
ORDER BY Id DESC;