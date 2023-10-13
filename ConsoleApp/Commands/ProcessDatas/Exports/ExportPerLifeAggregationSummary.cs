using BusinessObject.Retrocession;
using DataAccess.EntityFramework;
using Ionic.Zip;
using Services;
using Shared;
using Shared.ProcessFile;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp.Commands.ProcessDatas.Exports
{
    public class ExportPerLifeAggregationSummary : ExportFile
    {
        public bool IsExcludedRecord { get; set; } = false;

        public bool IsRetro { get; set; } = false;

        ExportPerLifeAggregationDetailTreaty ExportPerLifeAggregationDetailTreaty;

        IExportFile ExportPerLifeAggregationDetail;

        public void SetExports()
        {
            string prefix = "";
            if (IsExcludedRecord)
                prefix = "Excluded";
            else if (IsRetro)
                prefix = "Retro";

            ExportPerLifeAggregationDetailTreaty = new ExportPerLifeAggregationDetailTreaty()
            {
                PrefixFileName = string.Format("{0}Summary", prefix),
                IsExcludedRecordSummary = IsExcludedRecord,
                IsRetroRecordSummary = IsRetro,
            };

            if (IsExcludedRecord)
            {
                ExportPerLifeAggregationDetail = new ExportPerLifeAggregationDetailData()
                {
                    PrefixFileName = string.Format("{0}Detail", prefix),
                    IsExcludedRecord = IsExcludedRecord
                };
            }

            if (IsRetro)
            {
                ExportPerLifeAggregationDetail = new ExportPerLifeAggregationMonthlyData()
                {
                    PrefixFileName = string.Format("{0}Detail", prefix),
                    IsRetroDetail = IsRetro
                };
            }
        }

        public override List<Column> GetColumns()
        {
            SetExports();

            ExportPerLifeAggregationDetailTreaty.GetColumns();
            ExportPerLifeAggregationDetail.GetColumns();

            return base.GetColumns();
        }

        public override void HandleDirectory()
        {
            ExportPerLifeAggregationDetailTreaty.HandleTempDirectory(false);
            ExportPerLifeAggregationDetail.HandleTempDirectory(false);

            base.HandleDirectory();
        }

        public override void ProcessNext()
        {
            if (Processed == 0)
                ExportPerLifeAggregationDetailTreaty.Process();

            ExportPerLifeAggregationDetail.Skip = Skip;
            ExportPerLifeAggregationDetail.Take = Take;

            ExportPerLifeAggregationDetail.ProcessNext();
            Processed = ExportPerLifeAggregationDetail.Processed;

            if (Processed >= Total)
            {
                using (ZipFile zip = new ZipFile())
                {
                    zip.AddSelectedFiles(ExportPerLifeAggregationDetailTreaty.FileName, Util.GetTemporaryPath(), "", false);
                    zip.AddSelectedFiles(ExportPerLifeAggregationDetail.FileName, Util.GetTemporaryPath(), "", false);
                    zip.Save(FilePath);
                }
            }
        }

        public override void WriteHeaderLine()
        {
            ExportPerLifeAggregationDetail.WriteHeaderLine();
        }

        public override void SetQuery(IQueryable<object> query)
        {
            var exportBo = ExportBo;
            IQueryable<PerLifeAggregationDetailTreatyBo> summaryQuery = null;
            if (IsExcludedRecord)
                summaryQuery = ExportService.GetPerLifeAggregationDetailExlcudedRecordQuery(ref exportBo, Db);
            else if (IsRetro)
                summaryQuery = ExportService.GetPerLifeAggregationDetailRetroQuery(ref exportBo, Db);

            ExportPerLifeAggregationDetailTreaty.SetQuery(summaryQuery);
            ExportPerLifeAggregationDetail.SetQuery(query);
        }
    }
}
