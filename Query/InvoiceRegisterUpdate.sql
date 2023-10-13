
/*
SELECT 
  pld.Code, ir.InvoiceType, ir.TotalPaid
FROM InvoiceRegister ir
JOIN TreatyCodes tc
    ON tc.Id = ir.TreatyCodeId
JOIN Treaties t
    ON t.Id = tc.TreatyId
JOIN PickListDetails pld
    ON pld.Id = t.BusinessOriginPickListDetailId
WHERE ir.InvoiceRegisterBatchId = 2 AND
      ir.RiskQuarter <> ir.SoaQuarter
*/

UPDATE ir
SET InvoiceType = (CASE pld.Code WHEN 'WM' THEN (CASE WHEN ir.TotalPaid < 0 THEN 2 ELSE 3 END) WHEN 'OM' THEN (CASE WHEN ir.TotalPaid < 0 THEN 5 ELSE 6 END) WHEN 'SF' THEN (CASE WHEN ir.TotalPaid < 0 THEN 8 ELSE 9 END) END)
FROM InvoiceRegister ir
JOIN TreatyCodes tc
    ON tc.Id = ir.TreatyCodeId
JOIN Treaties t
    ON t.Id = tc.TreatyId
JOIN PickListDetails pld
    ON pld.Id = t.BusinessOriginPickListDetailId
WHERE ir.InvoiceRegisterBatchId = 2 AND
      ir.RiskQuarter <> ir.SoaQuarter

