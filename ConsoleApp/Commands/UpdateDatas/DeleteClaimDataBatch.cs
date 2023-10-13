using BusinessObject;
using BusinessObject.Claims;
using BusinessObject.Identity;
using DataAccess.Entities.Claims;
using DataAccess.Entities.Identity;
using DataAccess.EntityFramework;
using Services;
using Services.Claims;
using Services.Identity;
using Shared;
using Shared.Trails;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp.Commands.UpdateDatas
{
    public class DeleteClaimDataBatch : Command
    {
        public bool Delete { get; set; } = false;

        public ClaimDataBatch ClaimDataBatch { get; set; }
        public int ClaimDataBatchId { get; set; }

        public DeleteClaimDataBatch()
        {
            Title = "DeleteClaimDataBatch";
            Description = "To delete Claim Data Batch which status is Pending Delete";
            Options = new string[] {
                "--d|delete : To confirm delete ClaimDataBatch",
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
                    if (Delete)
                        ProcessDelete(db);
                    else
                        Summary(db);
                }
                catch (Exception e)
                {
                    // catch error open file
                    var message = e.Message;
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
            ModuleBo moduleBo = ModuleService.FindByController(ModuleBo.ModuleController.ClaimData.ToString());

            while (GetNext(db))
            {
                var result = ClaimDataBatchService.Result();
                var trail = new TrailObject();

                var riDataFileIds = new List<int> { };
                var claimDataFiles = ClaimDataFileService.GetByClaimDataBatchId(ClaimDataBatch.Id);
                foreach (ClaimDataFileBo claimDataFile in claimDataFiles)
                {
                    if (File.Exists(claimDataFile.RawFileBo.GetLocalPath()))
                    {
                        File.Delete(claimDataFile.RawFileBo.GetLocalPath());
                        riDataFileIds.Add(claimDataFile.RawFileId);
                    }
                }


                PrintClaimDataCount();

                PrintMessage("Deleting ClaimData...");
                db.Database.ExecuteSqlCommand("DELETE FROM [ClaimData] WHERE [ClaimDataBatchId] = {0}", ClaimDataBatch.Id);
                PrintMessage("Deleted ClaimData!");
                PrintMessage();

                ClaimDataFileService.DeleteAllByClaimDataBatchId(ClaimDataBatch.Id, ref trail);
                RawFileService.DeleteByIds(riDataFileIds, ref trail);
                ClaimDataBatchStatusFileService.DeleteAllByClaimDataBatchId(ClaimDataBatch.Id, ref trail);
                StatusHistoryService.DeleteAllByModuleIdObjectId(moduleBo.Id, ClaimDataBatch.Id, ref trail);
                RemarkService.DeleteAllByModuleIdObjectId(moduleBo.Id, ClaimDataBatch.Id, ref trail);

                var dataTrail = ClaimDataBatch.Delete(ClaimDataBatch.Id);
                dataTrail.Merge(ref trail, result.Table);

                int? deleteById = User.DefaultSuperUserId;
                if (ClaimDataBatch.UpdatedById != null && ClaimDataBatch.UpdatedById != 0)
                    deleteById = ClaimDataBatch.UpdatedById;

                var userTrailBo = new UserTrailBo(
                    ClaimDataBatch.Id,
                    "Delete Claim Data Batch",
                    result,
                    trail,
                    deleteById.Value
                );
                UserTrailService.Create(ref userTrailBo);

                db.SaveChanges();
            }
        }

        public void Summary(AppDbContext db)
        {
            int total = GetQuery(db).Count();
            int processed = 0;
            while (processed < total)
            {
                GetNext(db, processed);
                PrintClaimDataCount();
                processed++;
            }
        }

        public void PrintClaimDataCount()
        {
            if (ClaimDataBatch == null)
                return;

            int count = ClaimData.CountByClaimDataBatchId(ClaimDataBatch.Id);
            PrintLine();
            PrintDetail("ClaimDataBatchId", ClaimDataBatch.Id);
            PrintDetail("Total number of ClaimData", count);
            PrintMessage();
        }

        public IQueryable<ClaimDataBatch> GetQuery(AppDbContext db)
        {
            return db.ClaimDataBatches.Where(q => q.Status == ClaimDataBatchBo.StatusPendingDelete).OrderBy(q => q.Id);
        }

        public bool GetNext(AppDbContext db, int skip = 0, int take = 1)
        {
            ClaimDataBatch = GetQuery(db).Skip(skip).Take(take).FirstOrDefault();
            return ClaimDataBatch != null;
        }

        public int Count(AppDbContext db)
        {
            return GetQuery(db).Count();
        }
    }
}
