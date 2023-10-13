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
UPDATE [RiDataBatches] SET [Status] = 2 WHERE [Id] IN (1349,1350,1351,1352,1353,1354);
*/
/*
UPDATE [RiDataBatches] SET [Status] = 1 WHERE [Id] IN (1349,1350,1351,1352,1353,1354);
*/

/*
UPDATE [RiDataBatches] SET [Status] = 6 WHERE [Id] IN (1400,1406,1414,1416,1428,1429);
*/
/*
UPDATE [RiDataBatches] SET [Status] = 7 WHERE [Id] IN (1400,1406,1414,1416,1428,1429);
*/

/*
UPDATE [RiDataBatches] SET [Status] = 6 WHERE [Id] IN (1415,1398);
*/

/* to find those batch status is finalised but the ri data finalise status is pending */
/*
UPDATE [RiDataBatches] SET [Status] = 6 WHERE [Id] IN (44,1405,1401,1403,1359,1360,1392,1395,1409,1410,1340,1404,1408,1394,1411,1412,1413,1391,1402);
*/
