using BusinessObject;
using BusinessObject.Identity;
using BusinessObject.RiDatas;
using DataAccess.Entities.Identity;
using DataAccess.Entities.RiDatas;
using DataAccess.EntityFramework;
using Services;
using Services.Identity;
using Services.RiDatas;
using Shared;
using Shared.Trails;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.IO;
using System.Linq;

namespace ConsoleApp.Commands.UpdateDatas
{
    public class DeleteRiDataBatch : Command
    {
        public bool Delete { get; set; } = false;

        public DeleteRiDataBatch()
        {
            Title = "DeleteRiDataBatch";
            Description = "To delete RiDataBatch which status is Pending Delete";
            Options = new string[] {
                "--d|delete : To confirm delete RiDataBatch",
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
            if (!Delete)
                return;

            ModuleBo moduleBo = ModuleService.FindByController(ModuleBo.ModuleController.RiData.ToString());

            var riDataBatch = GetNext(db);
            while (riDataBatch != null)
            {
                var result = RiDataBatchService.Result();
                var trail = new TrailObject();

                var riDataFileIds = new List<int> { };
                var riDataFiles = RiDataFileService.GetByRiDataBatchId(riDataBatch.Id);
                foreach (RiDataFileBo riDataFile in riDataFiles)
                {
                    if (File.Exists(riDataFile.RawFileBo.GetLocalPath()))
                    {
                        File.Delete(riDataFile.RawFileBo.GetLocalPath());
                        riDataFileIds.Add(riDataFile.RawFileId);
                    }
                }

                int count = RiData.CountByRiDataBatchId(riDataBatch.Id);

                PrintLine();
                PrintDetail("RiDataBatchId", riDataBatch.Id);
                PrintDetail("Total number of RiData", count);
                PrintMessage("Deleting RiData...");
                db.Database.ExecuteSqlCommand("DELETE FROM [RiData] WHERE [RiDataBatchId] = {0}", riDataBatch.Id);
                PrintMessage("Deleted RiData!");
                PrintMessage();

                RiDataFileService.DeleteAllByRiDataBatchId(riDataBatch.Id, ref trail);
                RawFileService.DeleteByIds(riDataFileIds, ref trail);
                RiDataBatchStatusFileService.DeleteAllByRiDataBatchId(riDataBatch.Id, ref trail);
                StatusHistoryService.DeleteAllByModuleIdObjectId(moduleBo.Id, riDataBatch.Id, ref trail);
                RemarkService.DeleteAllByModuleIdObjectId(moduleBo.Id, riDataBatch.Id, ref trail);

                var dataTrail = RiDataBatch.Delete(riDataBatch.Id);
                dataTrail.Merge(ref trail, result.Table);

                int? deleteById = User.DefaultSuperUserId;
                if (riDataBatch.UpdatedById != null && riDataBatch.UpdatedById != 0)
                    deleteById = riDataBatch.UpdatedById;

                var userTrailBo = new UserTrailBo(
                    riDataBatch.Id,
                    "Delete Ri Data Batch",
                    result,
                    trail,
                    deleteById.Value
                );
                UserTrailService.Create(ref userTrailBo);

                db.SaveChanges();

                riDataBatch = GetNext(db);
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
                var riDataBatch = GetNext(db, processed);
                if (riDataBatch != null)
                {
                    int count = RiData.CountByRiDataBatchId(riDataBatch.Id);
                    PrintLine();
                    PrintDetail("RiDataBatchId", riDataBatch.Id);
                    PrintDetail("Total number of RiData", count);
                    PrintMessage();
                }
                processed++;
            }
        }

        public IQueryable<RiDataBatch> GetQuery(AppDbContext db)
        {
            return db.RiDataBatches.Where(q => q.Status == RiDataBatchBo.StatusPendingDelete).OrderBy(q => q.Id);
        }

        public RiDataBatch GetNext(AppDbContext db, int skip = 0, int take = 1)
        {
            return GetQuery(db).Skip(skip).Take(take).FirstOrDefault();
        }

        public int Count(AppDbContext db)
        {
            return GetQuery(db).Count();
        }
    }
}
