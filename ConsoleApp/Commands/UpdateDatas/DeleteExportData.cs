using BusinessObject;
using BusinessObject.Identity;
using DataAccess.Entities;
using DataAccess.Entities.Identity;
using DataAccess.EntityFramework;
using Services;
using Services.Identity;
using Shared;
using Shared.Trails;
using System;
using System.Data.Entity.Validation;
using System.IO;
using System.Linq;

namespace ConsoleApp.Commands.UpdateDatas
{
    public class DeleteExportData : Command
    {
        public bool Delete { get; set; } = false;

        public DeleteExportData()
        {
            Title = "DeleteExportData";
            Description = "To delete Export Data which is more than 3 months";
            Options = new string[] {
                "--d|delete : To confirm delete DeleteExportData",
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

            var export = GetNext(db);
            while (export != null)
            {
                var result = ExportService.Result();
                var trail = new TrailObject();

                PrintLine();
                PrintDetail("ExportId", export.Id);
                PrintMessage("Deleting ExportData...");

                var bo = ExportService.Find(export.Id);
                if (bo != null)
                {
                    if (bo.IsFileExists())
                    {
                        File.Delete(bo.GetPath());
                    }
                }

                PrintMessage("Deleted ExportData!");
                PrintMessage();

                var dataTrail = Export.Delete(export.Id);
                dataTrail.Merge(ref trail, result.Table);

                int? deleteById = User.DefaultSuperUserId;
                if (export.UpdatedById != null && export.UpdatedById != 0)
                    deleteById = export.UpdatedById;

                var userTrailBo = new UserTrailBo(
                    export.Id,
                    "Delete Export",
                    result,
                    trail,
                    deleteById.Value
                );
                UserTrailService.Create(ref userTrailBo);

                db.SaveChanges();

                export = GetNext(db);
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
                var export = GetNext(db, processed);
                if (export != null)
                {
                    PrintLine();
                    PrintDetail("ExportId", export.Id);
                    PrintMessage();
                }
                processed++;
            }
        }

        public IQueryable<Export> GetQuery(AppDbContext db)
        {
            DateTime dtt = DateTime.Now.AddMonths(-3);
            return db.Exports.Where(q => q.Status == ExportBo.StatusCompleted).Where(q => q.CreatedAt < dtt).OrderBy(q => q.Id);
        }

        public Export GetNext(AppDbContext db, int skip = 0, int take = 1)
        {
            return GetQuery(db).Skip(skip).Take(take).FirstOrDefault();
        }

        public int Count(AppDbContext db)
        {
            return GetQuery(db).Count();
        }
    }
}
