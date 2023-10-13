using BusinessObject.Identity;
using DataAccess.Entities;
using DataAccess.Entities.Identity;
using DataAccess.EntityFramework;
using DataAccess.Identity;
using DataAccess.Migrations;
using Microsoft.AspNet.Identity;
using System;
using System.Data.Entity.Migrations;
using System.Linq;

namespace ConsoleApp.Commands
{
    public class Migration : Command
    {
        public Migration()
        {
            Title = "Migration";
            Description = "To do database migrations";
            Options = new string[] {
                "--m|migration= : Enter target migration",
                "--p|pending : List all pending migrations",
                "--d|database : List all database migrations",
                "--l|local : List all local migrations",
                "--s|seed= : Seed data",
                "--n|number= : Number of seed data",
            };
        }

        public override void Run()
        {
            PrintStarting();

            string seed = Option("seed");
            if (string.IsNullOrEmpty(seed))
            {
                Process();
            }
            else
            {
                SeedData(seed);
            }

            PrintEnding();
        }

        public void Process()
        {
            Configuration configuration = new Configuration
            {
                ContextType = typeof(AppDbContext)
            };
            DbMigrator dbMigrator = new DbMigrator(configuration);

            if (IsOption("pending"))
            {
                PrintPending(dbMigrator);
            }
            else if (IsOption("database"))
            {
                PrintDatabase(dbMigrator);
            }
            else if (IsOption("local"))
            {
                PrintLocal(dbMigrator);
            }
            else
            {
                string migration = Option("migration");
                try
                {
                    if (migration == null)
                    {
                        var pendings = dbMigrator.GetPendingMigrations();

                        if (pendings.Count() > 0)
                        {
                            PrintPending(dbMigrator);

                            PrintMessage("Migrating...");
                            dbMigrator.Update();
                            PrintMessage("Migrated!");
                        }
                        else
                        {
                            PrintMessage("Nothing to migrate.");
                        }

                        using (AppDbContext db = new AppDbContext())
                        {
                            PrintMessage();
                            PrintMessage("Seeding...");
                            configuration.RunSeed(db);
                            PrintMessage("Seed Done!");
                        }
                    }
                    else
                    {
                        PrintMessage("Migrating target to " + migration + "...");
                        dbMigrator.Update(migration);
                        PrintMessage("Migrated!");
                    }
                    PrintMessage("");
                }
                catch (Exception e)
                {
                    PrintError(e.Message);
                }
            }
        }

        public void SeedData(string seed)
        {
            string numberStr = Option("number");
            int.TryParse(numberStr, out int number);

            if (number == 0)
                number = 50;

            switch (seed)
            {
                case "SeedFakeSuperUser":
                    SeedFakeSuperUser(number);
                    break;
                case "SeedFakeSuperAccessGroup":
                    SeedFakeSuperAccessGroup(number);
                    break;
                case "SeedFakeTreaty":
                    SeedFakeTreaty(number);
                    break;
            }
        }

        public void SeedFakeSuperUser(int number = 50)
        {
            using (var db = new AppDbContext())
            {
                User superUser = User.GetSuperUser();
                AccessGroup superAccessGroup = AccessGroup.GetSuperAccessGroup();

                AppUserManager userManager = AppUserManager.Default();

                User user;

                for (var i = 1; i <= number; i++)
                {
                    if (PrintCommitBuffer())
                    {
                        // nothing
                    }
                    SetProcessCount();

                    string username = string.Format("super{0}", i);
                    string email = string.Format("super+{0}@enrii.com", i);
                    int loginMethod = UserBo.LoginMethodPassword;

                    if (User.CountByUserName(username) == 0 && User.CountByEmail(email) == 0)
                    {
                        user = new User
                        {
                            Status = UserBo.StatusActive,
                            UserName = username,
                            Email = email,
                            LoginMethod = loginMethod,
                            EmailConfirmed = true,
                            CreatedById = superUser.Id,
                            UpdatedById = superUser.Id,
                        };
                        userManager.Create(user, User.DefaultPassword);

                        user.AddToAccessGroup(superAccessGroup.Id, out UserAccessGroup uag);

                        SetProcessCount("Created");
                    }
                }

                PrintProcessCount();
            }
        }

        public void SeedFakeSuperAccessGroup(int number = 50)
        {
            User superUser = User.GetSuperUser();
            AccessGroup accessGroup;

            for (var i = 1; i <= number; i++)
            {
                if (PrintCommitBuffer())
                {
                    // nothing
                }
                SetProcessCount();

                string code = string.Format("FAKESUPER{0}", i);
                string name = string.Format("FAKE Super Access Group {0}", i);

                if (AccessGroup.CountByCode(code) == 0)
                {
                    accessGroup = new AccessGroup
                    {
                        DepartmentId = DepartmentBo.DepartmentIT,
                        Code = code,
                        Name = name,
                        CreatedById = superUser.Id,
                        UpdatedById = superUser.Id,
                    };
                    accessGroup.Create();

                    SetProcessCount("Created");
                }
            }

            PrintProcessCount();
        }

        public void SeedFakeTreaty(int number = 50)
        {
            using (var db = new AppDbContext())
            {
                User superUser = User.GetSuperUser();
                Cedant cedant = db.Cedants.FirstOrDefault();
                if (cedant == null)
                {
                    PrintError("No cedant record found!");
                    return;
                }

                for (var i = 1; i <= number; i++)
                {
                    if (PrintCommitBuffer())
                    {
                        // nothing
                    }
                    SetProcessCount();

                    var treatyIdCode = string.Format("{0}-{1}", cedant.Code, i);

                    if (Treaty.CountByCode(treatyIdCode) == 0)
                    {
                        var fakeTreaty = new Treaty
                        {
                            TreatyIdCode = treatyIdCode,
                            CedantId = cedant.Id,
                            CreatedById = superUser.Id,
                            UpdatedById = superUser.Id,
                        };
                        fakeTreaty.Create();

                        SetProcessCount("Created");
                    }
                }

                PrintProcessCount();
            }
        }

        public void PrintPending(DbMigrator dbMigrator)
        {
            int index = 1;
            var pendings = dbMigrator.GetPendingMigrations();

            if (pendings.Count() > 0)
            {
                PrintMessage("--Pending Migrations--");
                foreach (string name in pendings)
                {
                    PrintMessage(string.Format("  {0}  {1}", index, name));
                    index++;
                }
            }
            else
            {
                PrintMessage("No pending migrations.");
            }
            PrintMessage("");
        }

        public void PrintDatabase(DbMigrator dbMigrator)
        {
            int index = 1;
            PrintMessage("--Database Migrations--");
            foreach (string name in dbMigrator.GetDatabaseMigrations())
            {
                PrintMessage(string.Format("  {0}  {1}", index, name));
                index++;
            }
            PrintMessage("");
        }

        public void PrintLocal(DbMigrator dbMigrator)
        {
            int index = 1;
            PrintMessage("--Local Migrations--");
            foreach (string name in dbMigrator.GetLocalMigrations())
            {
                PrintMessage(string.Format("  {0}  {1}", index, name));
                index++;
            }
            PrintMessage("");
        }
    }
}