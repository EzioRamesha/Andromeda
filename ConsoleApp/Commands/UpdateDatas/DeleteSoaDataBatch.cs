using BusinessObject;
using BusinessObject.Identity;
using BusinessObject.SoaDatas;
using DataAccess.Entities.Identity;
using DataAccess.Entities.SoaDatas;
using DataAccess.EntityFramework;
using Services;
using Services.Claims;
using Services.Identity;
using Services.RiDatas;
using Services.SoaDatas;
using Shared;
using Shared.Trails;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp.Commands.UpdateDatas
{
    public class DeleteSoaDataBatch : Command
    {
        public bool Delete { get; set; } = false;

        public DeleteSoaDataBatch()
        {
            Title = "DeleteSoaDataBatch";
            Description = "To delete SoaDataBatch which status is Pending Delete";
            Options = new string[] {
                "--d|delete : To confirm delete SoaDataBatch",
            };
            Hide = true;
        }

        public override void Initial()
        {
            base.Initial();

            Delete = IsOption("delete");
        }

        public override void Run()
        {
            using (var db = new AppDbContext(false))
            {
                int count = Count(db);
                if (count == 0)
                {
                    Log = false;
                    PrintMessage(MessageBag.NoBatchPendingDelete);
                    return;
                }

                PrintStarting();

                try
                {
                    ProcessDelete(db);
                    Summary(db);
                }
                catch (Exception e)
                {
                    // catch error open file
                    var message = e.ToString();
                    if (e is DbEntityValidationException dbEx)
                    {
                        message = Util.CatchDbEntityValidationException(dbEx).ToString();
                    }

                    PrintError(message);
                }

                PrintEnding();
            }
        }

        public void ProcessDelete(AppDbContext db)
        {
            if (!Delete)
                return;

            ModuleBo moduleBo = ModuleService.FindByController(ModuleBo.ModuleController.SoaData.ToString());

            var soaDataBatch = GetNext(db);
            while (soaDataBatch != null)
            {
                var result = SoaDataBatchService.Result();
                var trail = new TrailObject();

                var soaDataFileIds = new List<int> { };
                var soaDataFiles = SoaDataFileService.GetBySoaDataBatchId(soaDataBatch.Id);
                foreach (SoaDataFileBo soaDataFile in soaDataFiles)
                {
                    if (File.Exists(soaDataFile.RawFileBo.GetLocalPath()))
                    {
                        File.Delete(soaDataFile.RawFileBo.GetLocalPath());
                        soaDataFileIds.Add(soaDataFile.RawFileId);
                    }
                }

                int count = SoaData.CountBySoaDataBatchId(soaDataBatch.Id);

                PrintLine();
                PrintDetail("SoaDataBatchId", soaDataBatch.Id);
                PrintDetail("Total number of SoaData", count);
                PrintMessage("Deleting SoaData...");
                db.Database.ExecuteSqlCommand("DELETE FROM [SoaData] WHERE [SoaDataBatchId] = {0}", soaDataBatch.Id);
                PrintMessage("Deleted SoaData!");
                PrintMessage();

                SoaDataRiDataSummaryService.DeleteBySoaDataBatchId(soaDataBatch.Id);
                SoaDataPostValidationService.DeleteBySoaDataBatchId(soaDataBatch.Id);
                SoaDataPostValidationDifferenceService.DeleteBySoaDataBatchId(soaDataBatch.Id);
                SoaDataCompiledSummaryService.DeleteBySoaDataBatchId(soaDataBatch.Id);
                SoaDataDiscrepancyService.DeleteBySoaDataBatchId(soaDataBatch.Id);

                SoaDataFileService.DeleteAllBySoaDataBatchId(soaDataBatch.Id, ref trail);
                RawFileService.DeleteByIds(soaDataFileIds, ref trail);
                SoaDataBatchStatusFileService.DeleteAllBySoaDataBatchId(soaDataBatch.Id, ref trail);
                StatusHistoryService.DeleteAllByModuleIdObjectId(moduleBo.Id, soaDataBatch.Id, ref trail);
                RemarkService.DeleteAllByModuleIdObjectId(moduleBo.Id, soaDataBatch.Id, ref trail);

                // reset SoaDataBatchId in Claim Data
                if (soaDataBatch.ClaimDataBatchId.HasValue)
                {
                    var claimDataBatches = db.ClaimDataBatches.Where(q => q.Id == soaDataBatch.ClaimDataBatchId.Value || q.SoaDataBatchId == soaDataBatch.Id).ToList();
                    if (claimDataBatches != null)
                    {
                        foreach (var claimDataBatch in claimDataBatches)
                            db.Database.ExecuteSqlCommand("UPDATE [ClaimDataBatches] SET [SoaDataBatchId] = NULL WHERE [Id] = {0}", claimDataBatch.Id);
                    }
                }
                else
                {
                    var claimDataBatches = db.ClaimDataBatches.Where(q => q.SoaDataBatchId == soaDataBatch.Id).ToList();
                    if (claimDataBatches != null)
                    {
                        foreach (var claimDataBatch in claimDataBatches)
                            db.Database.ExecuteSqlCommand("UPDATE [ClaimDataBatches] SET [SoaDataBatchId] = NULL WHERE [Id] = {0}", claimDataBatch.Id);
                    }
                }

                // reset SoaDataBatchId in Ri Data
                if (soaDataBatch.RiDataBatchId.HasValue)
                {
                    var riDataBatches = db.RiDataBatches.Where(q => q.Id == soaDataBatch.RiDataBatchId.Value || q.SoaDataBatchId == soaDataBatch.Id).ToList();
                    if (riDataBatches != null)
                    {
                        foreach (var riDataBatch in riDataBatches)
                            db.Database.ExecuteSqlCommand("UPDATE [RiDataBatches] SET [SoaDataBatchId] = NULL WHERE [Id] = {0}", riDataBatch.Id);
                    }
                }
                else
                {
                    var riDataBatches = db.RiDataBatches.Where(q => q.SoaDataBatchId == soaDataBatch.Id).ToList();
                    if (riDataBatches != null)
                    {
                        foreach (var riDataBatch in riDataBatches)
                            db.Database.ExecuteSqlCommand("UPDATE [RiDataBatches] SET [SoaDataBatchId] = NULL WHERE [Id] = {0}", riDataBatch.Id);
                    }
                }

                // reset SoaDataBatchId in Claim Register
                var claimRegisters = db.ClaimRegister.Where(q => q.SoaDataBatchId == soaDataBatch.Id).ToList();
                if (claimRegisters != null)
                {
                    foreach (var claimRegister in claimRegisters)
                        db.Database.ExecuteSqlCommand("UPDATE [ClaimRegister] SET [SoaDataBatchId] = NULL WHERE [Id] = {0}", claimRegister.Id);
                }

                // reset SoaDataBatchId in Claim Register Histories
                var claimRegisterHistories = db.ClaimRegisterHistories.Where(q => q.SoaDataBatchId == soaDataBatch.Id).ToList();
                if (claimRegisterHistories != null)
                {
                    foreach (var claimRegisterHistory in claimRegisterHistories)
                        db.Database.ExecuteSqlCommand("UPDATE [ClaimRegisterHistories] SET [SoaDataBatchId] = NULL WHERE [Id] = {0}", claimRegisterHistory.Id);
                }

                var dataTrail = SoaDataBatch.Delete(soaDataBatch.Id);
                dataTrail.Merge(ref trail, result.Table);

                int? deleteById = User.DefaultSuperUserId;
                if (soaDataBatch.UpdatedById != null && soaDataBatch.UpdatedById != 0)
                    deleteById = soaDataBatch.UpdatedById;

                var userTrailBo = new UserTrailBo(
                    soaDataBatch.Id,
                    "Delete Soa Data Batch",
                    result,
                    trail,
                    deleteById.Value
                );
                UserTrailService.Create(ref userTrailBo);

                db.SaveChanges();

                soaDataBatch = GetNext(db);
            }
        }

        public void Summary(AppDbContext db)
        {
            if (Delete)
                return;

            int total = GetQuery(db).Count();
            int processed = 0;
            while (processed < total)
            {
                var soaDataBatch = GetNext(db, processed);
                if (soaDataBatch != null)
                {
                    int count = SoaData.CountBySoaDataBatchId(soaDataBatch.Id);
                    PrintLine();
                    PrintDetail("SoaDataBatchId", soaDataBatch.Id);
                    PrintDetail("Total number of SoaData", count);
                    PrintMessage();
                }
                processed++;
            }
        }

        public IQueryable<SoaDataBatch> GetQuery(AppDbContext db)
        {
            return db.SoaDataBatches.Where(q => q.Status == SoaDataBatchBo.StatusPendingDelete).OrderBy(q => q.Id);
        }

        public SoaDataBatch GetNext(AppDbContext db, int skip = 0, int take = 1)
        {
            return GetQuery(db).Skip(skip).Take(take).FirstOrDefault();
        }

        public int Count(AppDbContext db)
        {
            return GetQuery(db).Count();
        }
    }
}
