

SELECT [TreatyCodes].*
FROM [TreatyCodes]
LEFT JOIN [Treaties] ON [Treaties].[Id] = [TreatyCodes].[TreatyId]
WHERE [Treaties].[CedantId] IN (6);

