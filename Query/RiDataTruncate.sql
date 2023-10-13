TRUNCATE TABLE [Mfrs17ReportingDetailRiDatas];
GO

ALTER TABLE [dbo].[Mfrs17ReportingDetailRiDatas] DROP CONSTRAINT [FK_dbo.Mfrs17ReportingDetailRiDatas_dbo.RiData_RiDataId];
GO

TRUNCATE TABLE [RiData];
GO

ALTER TABLE [dbo].[Mfrs17ReportingDetailRiDatas]  WITH CHECK ADD CONSTRAINT [FK_dbo.Mfrs17ReportingDetailRiDatas_dbo.RiData_RiDataId] FOREIGN KEY([RiDataId])
REFERENCES [dbo].[RiData] ([Id])
GO

