
/*
SELECT 
  BusinessCode, InvoiceType, NetTotalAmount
FROM SoaDataCompiledSummaries
WHERE SoaDataBatchId = 3 and RiskQuarter <> SoaQuarter;
*/

UPDATE SoaDataCompiledSummaries 
SET InvoiceType = (CASE BusinessCode WHEN 'WM' THEN (CASE WHEN NetTotalAmount < 0 THEN 2 ELSE 3 END) WHEN 'OM' THEN (CASE WHEN NetTotalAmount < 0 THEN 5 ELSE 6 END) WHEN 'SF' THEN (CASE WHEN TotalAmount < 0 THEN 8 ELSE 9 END) END) 
WHERE SoaDataBatchId = 3 and RiskQuarter <> SoaQuarter