

SELECT TOP (1000)
    [RiData].[Id]
    ,[RiData].[RiDataBatchId]
    ,[RiData].[RiDataFileId]

    ,[RiData].[PolicyNumber]
    ,[RiData].[InsuredName]

    ,[RiData].[TreatyCode]
    ,[RiData].[RiskPeriodMonth]
    ,[RiData].[RiskPeriodYear]

    ,[RiData].[NetPremium]
    ,[RiData].[StandardPremium]
    ,[RiData].[SubstandardPremium]
    ,[RiData].[FlatExtraPremium]
    
    ,[RiData].[MlreNetPremium]
    ,[RiData].[MlreStandardPremium]
    ,[RiData].[MlreSubstandardPremium]
    ,[RiData].[MlreFlatExtraPremium]
    
    ,[RiData].[Layer1NetPremium]
    ,[RiData].[Layer1StandardPremium]
    ,[RiData].[Layer1SubstandardPremium]
    ,[RiData].[Layer1FlatExtraPremium]

    ,[RiData].[StandardDiscount]
    ,[RiData].[SubstandardDiscount]
FROM [dbo].[RiData]

/*
WHERE
*/
    /*
    [RiData].[TreatyCode] = 'GEL-08'
    */
    /*
    [RiData].[RiskPeriodMonth] = 10
    */
    /*
    [RiData].[TransactionTypeCode] = 'AL'
    */

;

/*
UPDATE [RiData]
   SET
   [RiskPeriodMonth] = 12
WHERE [Id] < 21;
*/

/*
SELECT TOP (1000)
    [RiData].[Id]
    ,[RiData].[RiDataBatchId]
    ,[RiData].[RiDataFileId]
    
    ,[RiData].[PolicyNumber]
    ,[RiData].[InsuredName]

    ,[RiData].[TreatyCode]
    ,[RiData].[RiskPeriodMonth]
    ,[RiData].[RiskPeriodYear]

    ,[RiData].[NetPremium]
    ,[RiData].[StandardPremium]
    ,[RiData].[SubstandardPremium]
    ,[RiData].[FlatExtraPremium]
    
    ,[RiData].[MlreNetPremium]
    ,[RiData].[MlreStandardPremium]
    ,[RiData].[MlreSubstandardPremium]
    ,[RiData].[MlreFlatExtraPremium]
    
    ,[RiData].[Layer1NetPremium]
    ,[RiData].[Layer1StandardPremium]
    ,[RiData].[Layer1SubstandardPremium]
    ,[RiData].[Layer1FlatExtraPremium]

    ,[RiData].[StandardDiscount]
    ,[RiData].[SubstandardDiscount]
FROM [RiData]
WHERE [RiDataBatchId] = 1380
AND [FinaliseStatus] = 3;
*/

/*
SELECT TOP (1000)
    [RiData].[Id]
    ,[RiData].[RiDataBatchId]
    ,[RiData].[RiDataFileId]
    
    ,[RiData].[PolicyNumber]
    ,[RiData].[InsuredName]

    ,[RiData].[TreatyCode]
    ,[RiData].[RiskPeriodMonth]
    ,[RiData].[RiskPeriodYear]

    ,[RiData].[NetPremium]
    ,[RiData].[StandardPremium]
    ,[RiData].[SubstandardPremium]
    ,[RiData].[FlatExtraPremium]
    
    ,[RiData].[MlreNetPremium]
    ,[RiData].[MlreStandardPremium]
    ,[RiData].[MlreSubstandardPremium]
    ,[RiData].[MlreFlatExtraPremium]
    
    ,[RiData].[Layer1NetPremium]
    ,[RiData].[Layer1StandardPremium]
    ,[RiData].[Layer1SubstandardPremium]
    ,[RiData].[Layer1FlatExtraPremium]

    ,[RiData].[StandardDiscount]
    ,[RiData].[SubstandardDiscount]
FROM [RiData]
WHERE [RiDataBatchId] = 1415;
*/

/*
SELECT TOP (1000)
    [RiData].[Id]
    ,[RiData].[RiDataBatchId]
    ,[RiData].[RiDataFileId]
    
    ,[RiData].[PolicyNumber]
    ,[RiData].[InsuredName]

    ,[RiData].[TreatyCode]
    ,[RiData].[RiskPeriodMonth]
    ,[RiData].[RiskPeriodYear]

    ,[RiData].[NetPremium]
    ,[RiData].[StandardPremium]
    ,[RiData].[SubstandardPremium]
    ,[RiData].[FlatExtraPremium]
    
    ,[RiData].[MlreNetPremium]
    ,[RiData].[MlreStandardPremium]
    ,[RiData].[MlreSubstandardPremium]
    ,[RiData].[MlreFlatExtraPremium]
    
    ,[RiData].[Layer1NetPremium]
    ,[RiData].[Layer1StandardPremium]
    ,[RiData].[Layer1SubstandardPremium]
    ,[RiData].[Layer1FlatExtraPremium]

    ,[RiData].[StandardDiscount]
    ,[RiData].[SubstandardDiscount]
FROM [RiData]
WHERE [Id] IN (66488741);
*/

