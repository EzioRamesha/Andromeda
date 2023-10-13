using BusinessObject;
using BusinessObject.Identity;
using BusinessObject.Retrocession;
using BusinessObject.RiDatas;
using DataAccess.Entities.Identity;
using DataAccess.Entities.Retrocession;
using DataAccess.Entities.RiDatas;
using DataAccess.EntityFramework;
using Services;
using Services.Identity;
using Services.Retrocession;
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
    public class DeletePerLifeAggregation : Command
    {
        public bool Delete { get; set; } = false;

        public DeletePerLifeAggregation()
        {
            Title = "DeletePerLifeAggregation";
            Description = "To delete PerLifeAggregation which status is Pending Delete";
            Options = new string[] {
                "--d|delete : To confirm delete PerLifeAggregation",
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

            ModuleBo moduleBo = ModuleService.FindByController(ModuleBo.ModuleController.PerLifeAggregation.ToString());

            var perLifeAggregation = GetNext(db);
            while (perLifeAggregation != null)
            {
                var result = PerLifeAggregationService.Result();
                var trail = new TrailObject();

                PrintLine();
                PrintDetail("PerLifeAggregationId", perLifeAggregation.Id);

                PrintMessage("Deleting PerLifeAggregationData...");

                var storedProcedure = new StoredProcedure(StoredProcedure.DeletePerLifeAggregation);
                storedProcedure.AddParameter("PerLifeAggregationId", perLifeAggregation.Id);
                storedProcedure.Execute(true);
                if (storedProcedure.ParseResult())
                {
                    PrintMessage("Deleted PerLifeAggregationData!");

                    PrintMessage();

                    var dataTrail = PerLifeAggregation.Delete(perLifeAggregation.Id);
                    dataTrail.Merge(ref trail, result.Table);

                    int? deleteById = User.DefaultSuperUserId;
                    if (perLifeAggregation.UpdatedById != null && perLifeAggregation.UpdatedById != 0)
                        deleteById = perLifeAggregation.UpdatedById;

                    var userTrailBo = new UserTrailBo(
                        perLifeAggregation.Id,
                        "Delete Per Life Aggregation",
                        result,
                        trail,
                        deleteById.Value
                    );
                    UserTrailService.Create(ref userTrailBo);

                    db.SaveChanges();
                }
                else
                {
                    PrintMessage(storedProcedure.Result);
                }

                perLifeAggregation = GetNext(db);
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
                var perLifeAggregation = GetNext(db, processed);
                if (perLifeAggregation != null)
                {
                    PrintLine();
                    PrintDetail("PerLifeAggregationId", perLifeAggregation.Id);
                    PrintMessage();
                }
                processed++;
            }
        }

        public IQueryable<PerLifeAggregation> GetQuery(AppDbContext db)
        {
            return db.PerLifeAggregations.Where(q => q.Status == PerLifeAggregationBo.StatusPendingDelete).OrderBy(q => q.Id);
        }

        public PerLifeAggregation GetNext(AppDbContext db, int skip = 0, int take = 1)
        {
            return GetQuery(db).Skip(skip).Take(take).FirstOrDefault();
        }

        public int Count(AppDbContext db)
        {
            return GetQuery(db).Count();
        }
    }
}
