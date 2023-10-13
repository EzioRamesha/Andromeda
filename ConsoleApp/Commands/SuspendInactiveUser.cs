using BusinessObject.Identity;
using DataAccess.Entities.Identity;
using DataAccess.EntityFramework;
using Newtonsoft.Json;
using Services.Identity;
using Shared;
using Shared.Trails;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace ConsoleApp.Commands
{
    public class SuspendInactiveUser : Command
    {
        public int DaysInactive { get; set; } = 90;
        public int Total { get; set; } = 0;
        public int Take { get; set; } = 50;
        public IQueryable<User> Query { get; set; }
        public List<User> Users { get; set; }

        public SuspendInactiveUser()
        {
            Title = "SuspendInactiveUser";
            Description = "Suspend Inactive Users";

            DaysInactive = Util.GetConfigInteger("DaysBeforeInactiveUserSuspension");
        }

        public override void Run()
        {
            using (var db = new AppDbContext(false))
            {
                Query = User.QueryInactiveUser(db, DaysInactive);
                Total = Query.Count();
                if (Total == 0)
                {
                    Log = false;
                    PrintMessage(MessageBag.NoInactiveUser);
                    return;
                }

                PrintStarting();
                while (GetNextBulkUser(db))
                {
                    foreach (var u in Users)
                    {
                        var dbUser = User.Find(u.Id);

                        var user = u;
                        var trailObject = new TrailObject { };

                        user.Status = UserBo.StatusSuspend;
                        user.UpdatedById = User.DefaultSuperUserId;

                        var result = User.Result();
                        var dataTrail = new DataTrail(dbUser, user, ignoreFields: User.IgnoreFields());
                        dataTrail.Merge(ref trailObject, result.Table);

                        db.Entry(user).State = EntityState.Modified;
                        db.SaveChanges();

                        UserTrailBo userTrailBo = new UserTrailBo(
                            user.Id,
                            "Suspend Inactive User",
                            result,
                            trailObject,
                            User.DefaultSuperUserId
                        );
                        UserTrailService.Create(ref userTrailBo);

                        SetProcessCount();
                    }
                    PrintProcessCount();
                }
                PrintEnding();
            }
        }

        public bool GetNextBulkUser(AppDbContext db)
        {
            Users = new List<User> { };
            Query = User.QueryInactiveUser(db, DaysInactive);
            Total = Query.Count();
            if (Total == 0)
                return false;
            Users = Query.Take(Take).ToList();
            return true;
        }
    }
}
