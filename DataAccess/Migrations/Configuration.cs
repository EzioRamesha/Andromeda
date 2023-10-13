namespace DataAccess.Migrations
{
    using BusinessObject;
    using BusinessObject.Identity;
    using DataAccess.Entities;
    using DataAccess.Entities.Identity;
    using DataAccess.Entities.Sanctions;
    using DataAccess.EntityFramework;
    using Shared.DataAccess;
    using Shared.Trails;
    using System.Data.Entity.Migrations;

    public sealed class Configuration : DbMigrationsConfiguration<AppDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            CommandTimeout = 18000;
        }

        public void RunSeed(AppDbContext context)
        {
            Seed(context);
        }

        protected override void Seed(AppDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method
            //  to avoid creating duplicate seed data.

            // System.Diagnostics.Debugger.Launch();

            User superUser = User.SeedSuperUser();

            Department.SeedDepartments(superUser);
            Module.SeedModules(superUser);

            if (superUser.DepartmentId == null)
            {
                superUser.DepartmentId = DepartmentBo.DepartmentIT;
                superUser.Update();
            }

            AccessGroup superAccessGroup = AccessGroup.SeedSuperAccessGroup(superUser);
            AccessMatrix.SeedSuperAccessMatrices(superAccessGroup);

            StandardOutput.SeedStandardOutput(superUser);
            StandardClaimDataOutput.SeedClaimOutput(superUser);
            StandardSoaDataOutput.SeedStandardOutput(superUser);
            StandardRetroOutput.SeedStandardRetroOutput(superUser);

            PickList.SeedPickList(superUser);

            Source.SeedSource(superUser);

            ClaimReason.SeedClaimReason(superUser);
        }
    }
}
