using BusinessObject;
using BusinessObject.Identity;
using ConsoleApp.Commands.ProcessDatas.Exports;
using DataAccess.Entities.Identity;
using DataAccess.EntityFramework;
using Newtonsoft.Json;
using Services;
using Services.Identity;
using Services.Retrocession;
using Shared;
using Shared.Trails;
using System;

namespace ConsoleApp.Commands.ProcessDatas
{
    public class GenerateExportData : Command
    {
        public int? ExportId { get; set; }

        public bool Test { get; set; }

        public ExportBo ExportBo { get; set; }

        public int ExportRowRead { get; set; } = 0;
        public int ExportInstantDownloadLimit { get; set; } = 0;

        public GenerateExportData()
        {
            Title = "GenerateExportData";
            Description = "To generate data files";
            Options = new string[] {
                "--i|exportId= : Enter the ExportId",
                "--t|test : Test to generate",
            };

            ExportRowRead = Util.GetConfigInteger("ExportRowRead", 500);
            ExportInstantDownloadLimit = Util.GetConfigInteger("ExportInstantDownloadLimit", 10000);
        }

        public override void Initial()
        {
            base.Initial();

            ExportId = OptionIntegerNullable("exportId");
            Test = IsOption("test");
            Log = false;
        }

        public override void Run()
        {
            GetExportBo();
            if (ExportBo == null)
            {
                Log = false;
                PrintMessage("No Export pending to generate", true, false);
                return;
            }

            PrintStarting();

            using (var db = new AppDbContext(false))
            {
                while (GetExportBo() != null)
                {
                    var exportBo = ExportBo;
                    if (!Process(ref exportBo, db))
                        break;
                }
            }

            PrintEnding();
        }

        public bool Process(ref ExportBo bo, AppDbContext db)
        {
            #region Checking for jobs with status 'Generating'
            var exportBos = ExportService.GetFailedByHours();
            ExportBo failedBo;

            if (exportBos.Count > 0)
            {
                PrintStarting();
                PrintMessage("Failing GenerateExportData stucked");
                foreach (ExportBo eachBo in exportBos)
                {
                    PrintMessage("Failing Id: " + eachBo.Id);
                    PrintMessage();
                    eachBo.Status = ExportBo.StatusFailed;
                    failedBo = eachBo;

                    ExportService.Update(ref failedBo);
                }
            }
            #endregion

            var exportBo = bo;
            exportBo.ConvertParametersObject();

            var query = ExportService.GetQuery(ref exportBo, db);
            ExportService.CountQueryTotal(ref exportBo, db, query);

            if (Log == true)
                Title += bo.Id;

            IExportFile exportFile = null;
            switch (exportBo.Type)
            {
                case ExportBo.TypeRiData:
                    exportFile = new ExportRiData();
                    break;
                case ExportBo.TypeClaimData:
                    exportFile = new ExportClaimData();
                    break;
                case ExportBo.TypeClaimRegister:
                    exportFile = new ExportClaimRegister();
                    break;
                case ExportBo.TypeRiDataWarehouse:
                    exportFile = new ExportRiDataWarehouse();
                    break;
                case ExportBo.TypeRiDataSearch:
                    exportFile = new ExportRiDataSearch();
                    break;
                case ExportBo.TypeClaimRegisterSearch:
                    exportFile = new ExportClaimRegisterSearch() { IsWithAdjustmentDetails = exportBo.TempFlag };
                    break;
                case ExportBo.TypeReferralClaim:
                    exportFile = new ExportReferralClaim();
                    break;
                case ExportBo.TypeInvoiceRegister:
                    exportFile = new ExportInvoiceRegister();
                    break;
                case ExportBo.TypeRetroRegister:
                    exportFile = new ExportRetroRegister();
                    break;
                case ExportBo.TypeClaimRegisterHistorySearch:
                    exportFile = new ExportClaimRegisterHistorySearch() { IsWithAdjustmentDetails = exportBo.TempFlag };
                    break;
                case ExportBo.TypeTreatyWorkflow:
                    exportFile = new ExportTreatyWorkflow();
                    break;
                case ExportBo.TypeQuotationWorkflow:
                    exportFile = new ExportQuotationWorkflow();
                    break;
                case ExportBo.TypeValidDuplicationList:
                    exportFile = new ExportValidDuplicationList();
                    break;
                case ExportBo.TypeRiDataWarehouseHistory:
                    exportFile = new ExportRiDataWarehouseHistory();
                    break;
                case ExportBo.TypeGroupReferral:
                    exportFile = new ExportGroupReferral();
                    break;
                case ExportBo.TypePerLifeClaimSummaryPaidClaims:
                    exportFile = new ExportPerLifeClaimSummaryPaidClaims();
                    break;
                case ExportBo.TypePerLifeClaimSummaryPendingClaims:
                    exportFile = new ExportPerLifeClaimSummaryPendingClaims();
                    break;
                case ExportBo.TypePerLifeClaimSummaryClaimsRemoved:
                    exportFile = new ExportPerLifeClaimSummaryClaimsRemoved();
                    break;
                case ExportBo.TypePerLifeAggregationConflictListing:
                    exportFile = new ExportPerLifeAggregationConflictListing();
                    break;
                case ExportBo.TypePerLifeAggregationDuplicationListing:
                    exportFile = new ExportPerLifeAggregationDuplicationListing();
                    break;
                case ExportBo.TypePerLifeAggregationRiData:
                    exportFile = new ExportPerLifeAggregationDetailData();
                    break;
                case ExportBo.TypePerLifeAggregationException:
                    exportFile = new ExportPerLifeAggregationDetailData
                    {
                        IsException = true
                    };
                    break;
                case ExportBo.TypePerLifeAggregationRetroRiData:
                    exportFile = new ExportPerLifeAggregationMonthlyData
                    {
                        IsRetroRiData = true
                    };
                    break;
                case ExportBo.TypePerLifeAggregationRetentionPremium:
                    exportFile = new ExportPerLifeAggregationMonthlyData
                    {
                        IsRetentionPremium = true,
                        RetroParties = PerLifeAggregationMonthlyRetroDataService.GetDistinctRetroPartyByPerLifeAggregationDetailId(exportBo.ObjectId)
                    };
                    break;
                case ExportBo.TypePerLifeAggregationRetroSummaryExcludedRecord:
                case ExportBo.TypePerLifeAggregationSummaryExcludedRecord:
                    exportFile = new ExportPerLifeAggregationSummary()
                    {
                        IsExcludedRecord = true
                    };
                    break;
                case ExportBo.TypePerLifeAggregationRetroSummaryRetro:
                case ExportBo.TypePerLifeAggregationSummaryRetro:
                    exportFile = new ExportPerLifeAggregationSummary()
                    {
                        IsRetro = true
                    };
                    break;
                case ExportBo.TypeGroupReferralTrackingCase:
                    exportFile = new ExportGroupReferral() { IsTracking = true };
                    break;
                case ExportBo.TypeFacMasterListing:
                    exportFile = new ExportFacMasterListing();
                    break;
                case ExportBo.TypeRateDetail:
                    exportFile = new ExportRateDetail();
                    break;
                default:
                    break;
            }

            if (exportFile == null)
                return false;

            exportFile.Db = db;
            exportFile.Total = exportBo.Total;
            exportFile.Take = ExportRowRead;
            exportFile.GetColumns();
            exportFile.SetExportBo(exportBo);
            exportFile.SetQuery(query);
            exportFile.Init();
            exportFile.HandleDirectory();

            // Update status
            if (!Test)
            {
                // update status to generating
                exportBo.Status = ExportBo.StatusGenerating;
                UpdateExportStatus(ref exportBo, db, "Generating");
            }

            PrintOutputTitle(string.Format("Generate Export Id: {0}, Type: {1}", bo.Id, ExportBo.GetTypeName(bo.Type)));

            SetProcessCount("File");

            bool suspend = false;
            exportFile.WriteHeaderLine();
            if (exportFile.IsRangeQuery)
            {
                while (exportFile.Processed < exportFile.Total)
                {
                    exportFile.ProcessNext();

                    // reload the object
                    exportBo.Processed = exportFile.Processed;
                    SetProcessCount("Lines", exportFile.Processed);

                    PrintProcessCount();

                    exportBo = RefreshExport(exportBo, db);
                    if (exportBo.IsSuspended())
                    {
                        SetProcessCount("Suspended");

                        // stop generating
                        suspend = true;
                        break;
                    }
                }
            }
            else
            {
                for (exportFile.Skip = 0; exportFile.Skip < exportFile.Total + exportFile.Take; exportFile.Skip += exportFile.Take)
                {
                    if (exportFile.Skip >= exportFile.Total)
                        break;

                    exportFile.ProcessNext();

                    // reload the object
                    exportBo.Processed = exportFile.Processed;
                    SetProcessCount("Lines", exportFile.Processed);

                    PrintProcessCount();

                    exportBo = RefreshExport(exportBo, db);
                    if (exportBo.IsSuspended())
                    {
                        SetProcessCount("Suspended");

                        // stop generating
                        suspend = true;
                        break;
                    }
                }
            }



            if (!exportFile.IsTextFile)
            {
                exportFile.CloseExcel();
            }

            if (!Test)
            {
                if (!suspend)
                {
                    // update status
                    exportBo.Status = ExportBo.StatusCompleted;
                    UpdateExportStatus(ref exportBo, db, "Completed", false);
                }
            }
            else
            {
                return false;
            }

            if (ExportId != null)
            {
                return false;
            }

            return true;
        }

        protected ExportBo RefreshExport(ExportBo exportBo, AppDbContext db)
        {
            var dbExportBo = ExportService.Find(exportBo.Id);

            if (!Test)
            {
                if (dbExportBo.IsSuspended())
                {
                    // save the GenerateEndAt date time
                    exportBo.GenerateEndAt = DateTime.Now;
                    exportBo.Status = dbExportBo.Status;
                }

                ExportService.Update(ref exportBo, db);
            }

            return exportBo;
        }

        public void UpdateExportStatus(ref ExportBo bo, AppDbContext db, string description, bool start = true)
        {
            var exportBo = bo;
            exportBo.UpdatedById = User.DefaultSuperUserId;

            if (start)
            {
                exportBo.Processed = 0;
                exportBo.GenerateStartAt = DateTime.Now;
                exportBo.GenerateEndAt = null; // clear the GenerateEndAt date time
            }
            else
            {
                exportBo.GenerateEndAt = DateTime.Now;
            }

            var trail = new TrailObject();
            var result = ExportService.Update(ref exportBo, ref trail, db);
            UserTrailBo userTrailBo = new UserTrailBo(
                exportBo.Id,
                description,
                result,
                trail,
                User.DefaultSuperUserId
            );
            UserTrailService.Create(ref userTrailBo, db);
        }

        public ExportBo CreateExport(int userId, int type, string description, dynamic parameters = null, int? id = null)
        {
            using (var db = new AppDbContext())
            {
                db.Database.CommandTimeout = 0;

                var exportBo = new ExportBo
                {
                    Type = type,
                    ObjectId = id,
                    Parameters = JsonConvert.SerializeObject(parameters),
                    ParameterObject = parameters,
                    CreatedById = userId,
                    UpdatedById = userId,
                };
                ExportService.CountQueryTotal(ref exportBo, db);

                var trail = new TrailObject();
                var result = ExportService.Create(ref exportBo, ref trail, db);
                UserTrailBo userTrailBo = new UserTrailBo(
                    exportBo.Id,
                    description,
                    result,
                    trail,
                    userId
                );
                UserTrailService.Create(ref userTrailBo, db);

                #region (no longer in use) Checking for jobs with same type and parameters with status 'Generating'
                /*var exportBos = ExportService.GetFailedByParameters(exportBo.Type, exportBo.Parameters, ExportBo.StatusGenerating);
                ExportBo failedBo;

                foreach (ExportBo bo in exportBos)
                {
                    bo.Status = ExportBo.StatusFailed;
                    failedBo = bo;

                    ExportService.Update(ref failedBo);
                }*/
                #endregion

                return exportBo;
            }
        }

        public ExportBo CreateExportRiData(int objectId, int userId, dynamic parameters = null)
        {
            return CreateExport(userId, ExportBo.TypeRiData, "Create RI Data Export", parameters, objectId);
        }

        public ExportBo CreateExportClaimData(int objectId, int userId, dynamic parameters = null)
        {
            return CreateExport(userId, ExportBo.TypeClaimData, "Create Claim Data Export", parameters, objectId);
        }

        public ExportBo CreateExportClaimRegister(int userId, dynamic parameters = null)
        {
            return CreateExport(userId, ExportBo.TypeClaimRegister, "Create Claim Register Export", parameters);
        }

        public ExportBo CreateExportRiDataWarehouse(int userId, dynamic parameters = null)
        {
            return CreateExport(userId, ExportBo.TypeRiDataWarehouse, "Create RI Data Warehouse Export", parameters);
        }

        public ExportBo CreateExportRiDataWarehouseHistory(int userId, dynamic parameters = null)
        {
            return CreateExport(userId, ExportBo.TypeRiDataWarehouseHistory, "Create RI Data Warehouse Export", parameters);
        }

        public ExportBo CreateExportRiDataSearch(int userId, dynamic parameters = null)
        {
            return CreateExport(userId, ExportBo.TypeRiDataSearch, "Create RI Data Search Export", parameters);
        }

        public ExportBo CreateExportClaimRegisterSearch(int userId, dynamic parameters = null)
        {
            return CreateExport(userId, ExportBo.TypeClaimRegisterSearch, "Create Claim Register Search Export", parameters);
        }

        public ExportBo CreateExportClaimRegisterHistorySearch(int userId, dynamic parameters = null)
        {
            return CreateExport(userId, ExportBo.TypeClaimRegisterHistorySearch, "Create Claim Register Search Export", parameters);
        }

        public ExportBo CreateExportReferralClaim(int userId, dynamic parameters = null)
        {
            return CreateExport(userId, ExportBo.TypeReferralClaim, "Create Referral Claim Export", parameters);
        }

        public ExportBo CreateExportInvoiceRegister(int userId, dynamic parameters = null)
        {
            return CreateExport(userId, ExportBo.TypeInvoiceRegister, "Create Invoice Register Export", parameters);
        }

        public ExportBo CreateExportRetroRegister(int userId, dynamic parameters = null)
        {
            return CreateExport(userId, ExportBo.TypeRetroRegister, "Create Retro Register Export", parameters);
        }

        public ExportBo CreateExportTreatyWorkflow(int userId, dynamic parameters = null)
        {
            return CreateExport(userId, ExportBo.TypeTreatyWorkflow, "Create Treaty Pricing Treaty Workflow Export", parameters);
        }

        public ExportBo CreateExportQuotationWorkflow(int userId, dynamic parameters = null)
        {
            return CreateExport(userId, ExportBo.TypeQuotationWorkflow, "Create Treaty Pricing Quotation Workflow Export", parameters);
        }

        public ExportBo CreateExportValidDuplicationList(int userId, dynamic parameters = null)
        {
            return CreateExport(userId, ExportBo.TypeValidDuplicationList, "Create Valid Duplication Listing Export", parameters);
        }

        public ExportBo CreateExportGroupReferral(int userId, dynamic parameters = null)
        {
            return CreateExport(userId, ExportBo.TypeGroupReferral, "Create Group Referral Export", parameters);
        }

        public ExportBo CreateExportPerLifeClaimSummaryPaidClaims(int objectId, int userId, dynamic parameters = null)
        {
            return CreateExport(userId, ExportBo.TypePerLifeClaimSummaryPaidClaims, "Create Per Life Claim Summary Paid Claims Export", parameters, objectId);
        }

        public ExportBo CreateExportPerLifeClaimSummaryPendingClaims(int objectId, int userId, dynamic parameters = null)
        {
            return CreateExport(userId, ExportBo.TypePerLifeClaimSummaryPendingClaims, "Create Per Life Claim Summary Pending Claims Export", parameters, objectId);
        }

        public ExportBo CreateExportPerLifeClaimSummaryClaimsRemoved(int objectId, int userId, dynamic parameters = null)
        {
            return CreateExport(userId, ExportBo.TypePerLifeClaimSummaryClaimsRemoved, "Create Per Life Claim Summary Claims Removed Export", parameters, objectId);
        }

        public ExportBo CreateExportPerLifeAggregationConflictListing(int userId, dynamic parameters = null)
        {
            return CreateExport(userId, ExportBo.TypePerLifeAggregationConflictListing, "Create Per Life Aggregation Conflict Listing Export", parameters);
        }

        public ExportBo CreateExportPerLifeAggregationDuplicationListing(int userId, dynamic parameters = null)
        {
            return CreateExport(userId, ExportBo.TypePerLifeAggregationDuplicationListing, "Create Per Life Aggregation Duplication Listing Export", parameters);
        }

        public ExportBo CreateExportPerLifeAggregationRiData(int objectId, int userId, dynamic parameters = null)
        {
            return CreateExport(userId, ExportBo.TypePerLifeAggregationRiData, "Create Per Life Aggregation RI Data Export", parameters, objectId);
        }

        public ExportBo CreateExportPerLifeAggregationException(int objectId, int userId, dynamic parameters = null)
        {
            return CreateExport(userId, ExportBo.TypePerLifeAggregationException, "Create Per Life Aggregation Exception Export", parameters, objectId);
        }

        public ExportBo CreateExportPerLifeAggregationRetroRiData(int objectId, int userId, dynamic parameters = null)
        {
            return CreateExport(userId, ExportBo.TypePerLifeAggregationRetroRiData, "Create Per Life Aggregation Retro RI Data Export", parameters, objectId);
        }

        public ExportBo CreateExportPerLifeAggregationRetentionPremium(int objectId, int userId, dynamic parameters = null)
        {
            return CreateExport(userId, ExportBo.TypePerLifeAggregationRetentionPremium, "Create Per Life Aggregation Retention Premium Export", parameters, objectId);
        }

        public ExportBo CreateExportPerLifeAggregationRetroSummaryExcludedRecord(int objectId, int userId, dynamic parameters = null)
        {
            return CreateExport(userId, ExportBo.TypePerLifeAggregationRetroSummaryExcludedRecord, "Create Per Life Aggregation Retro Summary Excluded Record Export", parameters, objectId);
        }

        public ExportBo CreateExportPerLifeAggregationRetroSummaryRetro(int objectId, int userId, dynamic parameters = null)
        {
            return CreateExport(userId, ExportBo.TypePerLifeAggregationRetroSummaryRetro, "Create Per Life Aggregation Retro Summary Retro Export", parameters, objectId);
        }

        public ExportBo CreateExportPerLifeAggregationSummaryExcludedRecord(int objectId, int userId, dynamic parameters = null)
        {
            return CreateExport(userId, ExportBo.TypePerLifeAggregationSummaryExcludedRecord, "Create Per Life Aggregation Summary Excluded Record Export", parameters, objectId);
        }

        public ExportBo CreateExportPerLifeAggregationSummaryRetro(int objectId, int userId, dynamic parameters = null)
        {
            return CreateExport(userId, ExportBo.TypePerLifeAggregationSummaryRetro, "Create Per Life Aggregation Summary Retro Export", parameters, objectId);
        }

        public ExportBo CreateExportGroupReferralTrackingCase(int userId, dynamic parameters = null)
        {
            return CreateExport(userId, ExportBo.TypeGroupReferralTrackingCase, "Create Group Referral Tracking Case Export", parameters);
        }

        public ExportBo CreateExportFacMasterListing(int userId, dynamic parameters = null)
        {
            return CreateExport(userId, ExportBo.TypeFacMasterListing, "Create Fac Master Listing Export", parameters);
        }

        public ExportBo CreateExportRateDetail(int userId, dynamic parameters = null)
        {
            return CreateExport(userId, ExportBo.TypeRateDetail, "Create Rate Detail Export", parameters);
        }

        public ExportBo GetExportBo()
        {
            ExportBo = ExportService.FindByStatus(ExportBo.StatusPending, ExportId);
            return ExportBo;
        }

    }
}
