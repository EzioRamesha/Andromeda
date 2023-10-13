using BusinessObject;
using BusinessObject.Identity;
using DataAccess.EntityFramework;
using DataAccess.Identity;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Shared;
using Shared.DataAccess;
using Shared.Trails;
using Shared.Trails.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace DataAccess.Entities.Identity
{
    [Table("Users")]
    public class User : IdentityUser<int, UserLogin, UserRole, UserClaim>, IUser<int>
    {
        public User()
        {
            EmailConfirmed = true;
            LockoutEnabled = true;

            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
        }

        [Key]
        public override int Id { get; set; }

        public int? DepartmentId { get; set; }

        [ExcludeTrail]
        public virtual Department Department { get; set; }

        public int Status { get; set; }

        public int LoginMethod { get; set; }

        [MaxLength(128)]
        public override string UserName { get; set; }

        [MaxLength(256), Index]
        public override string Email { get; set; }

        public override bool EmailConfirmed { get; set; }

        public override string PasswordHash { get; set; }

        [MaxLength(128)]
        public string FullName { get; set; }

        public override string SecurityStamp { get; set; }

        public override bool LockoutEnabled { get; set; }

        [Column("LockoutEndAt", TypeName = "datetime2")] // 0000-00-00 00:00:00
        public override DateTime? LockoutEndDateUtc { get; set; }

        public override int AccessFailedCount { get; set; }

        public string SessionId { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? LastLoginAt { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? PasswordExpiresAt { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime CreatedAt { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime UpdatedAt { get; set; }

        public int? CreatedById { get; set; }

        public int? UpdatedById { get; set; }

        [ExcludeTrail]
        [ForeignKey(nameof(CreatedById))]
        public virtual User CreatedBy { get; set; }

        // DO NOT FOREIGN
        //[ExcludeTrail]
        //public virtual User UpdatedBy { get; set; }

        [ExcludeTrail]
        public override string PhoneNumber { get => base.PhoneNumber; set => base.PhoneNumber = value; }

        [ExcludeTrail]
        public override bool PhoneNumberConfirmed { get => base.PhoneNumberConfirmed; set => base.PhoneNumberConfirmed = value; }

        [ExcludeTrail]
        public override bool TwoFactorEnabled { get => base.TwoFactorEnabled; set => base.TwoFactorEnabled = value; }

        [ExcludeTrail]
        public virtual IList<UserAccessGroup> UserAccessGroups { get; set; }

        public static int DefaultSuperUserId = 1;
        public static string DefaultSuperUserName = "super";
        public static string DefaultSuperFullName = "super";
        public static string DefaultSuperEmail = "super@enrii.com";
        public static string DefaultPassword = "Abc123000!";

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<User, int> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }

        public static string[] IgnoreFields()
        {
            return new string[]
            {
                "Roles",
                "Claims",
                "Logins",
            };
        }

        public static IQueryable<User> QueryInactiveUser(AppDbContext db, int daysInactive)
        {
            return db.GetUsers()
                .Where(
                    q =>
                    DbFunctions.DiffDays(q.LastLoginAt.Value, DateTime.Now) > daysInactive
                    ||
                    (q.LastLoginAt == null && DbFunctions.DiffDays(q.CreatedAt, DateTime.Now) > daysInactive)
                )
                .Where(q => q.Status == UserBo.StatusActive)
                .OrderBy(q => q.Id);
        }

        public static User Find(int id, bool deleted = false)
        {
            using (var db = new AppDbContext())
            {
                return db.GetUsers(deleted).Where(q => q.Id == id).FirstOrDefault();
            }
        }

        public static User FindByUserName(string userName)
        {
            using (var db = new AppDbContext())
            {
                return db.GetUsers().Where(q => q.UserName.ToLower() == userName.ToLower()).FirstOrDefault();
            }
        }

        public static User FindByEmail(string email)
        {
            using (var db = new AppDbContext())
            {
                return db.GetUsers().Where(q => q.Email.ToLower() == email.ToLower()).FirstOrDefault();
            }
        }

        public static int CountByUserName(string userName)
        {
            using (var db = new AppDbContext())
            {
                return db.GetUsers().Where(q => q.UserName == userName).Count();
            }
        }

        public static int CountByEmail(string email)
        {
            using (var db = new AppDbContext())
            {
                return db.GetUsers().Where(q => q.Email == email).Count();
            }
        }

        public static int CountByDepartmentId(int departmentId)
        {
            using (var db = new AppDbContext())
            {
                return db.GetUsers().Where(q => q.DepartmentId == departmentId).Count();
            }
        }

        public static IList<User> Get()
        {
            using (var db = new AppDbContext())
            {
                return db.GetUsers().ToList();
            }
        }

        public static IList<User> Get(Func<User, bool> expression)
        {
            using (var db = new AppDbContext())
            {
                return db.GetUsers().Where(expression).ToList();
            }
        }

        public static IList<User> GetByStatus(int? status = null, List<int> selectedIds = null, bool exceptSuper = false, int? departmentId = null)
        {
            using (var db = new AppDbContext())
            {
                var query = db.Users.AsQueryable();

                if (status.HasValue)
                {
                    if (selectedIds != null)
                        query = query.Where(q => q.Status == status || selectedIds.Contains(q.Id));
                    else
                        query = query.Where(q => q.Status == status);
                }

                if (exceptSuper)
                {
                    if (selectedIds != null)
                        query = query.Where(q => q.Id != DefaultSuperUserId || selectedIds.Contains(q.Id));
                    else
                        query = query.Where(q => q.Id != DefaultSuperUserId);
                }

                if (departmentId.HasValue)
                {
                    if (selectedIds != null)
                        query = query.Where(q => q.DepartmentId == departmentId || selectedIds.Contains(q.Id));
                    else
                        query = query.Where(q => q.DepartmentId == departmentId);
                }

                return query.OrderBy(q => q.FullName).ToList();
            }
        }

        public static IList<User> GetByAccessGroupId(List<int> accessGroupId)
        {
            using (var db = new AppDbContext())
            {
                return db.GetUsers().Join(db.UserAccessGroups, u => u.Id, uag => uag.UserId, (u, uag) => new { User = u, Uag = uag })
                    .Where(q => accessGroupId.Contains(q.Uag.AccessGroupId))
                    .Select(q => q.User)
                    .ToList();
            }
        }

        public static IList<User> GetByDepartmentId(int departmentId)
        {
            using (var db = new AppDbContext())
            {
                return db.GetUsers().Where(q => q.DepartmentId == departmentId)
                    .OrderBy(q => q.UserName)
                    .ToList();
            }
        }

        public static List<User> GetByPasswordExpiryDate(DateTime date)
        {
            using (var db = new AppDbContext())
            {
                return db.GetUsers().Where(q => DbFunctions.TruncateTime(q.PasswordExpiresAt.Value) == date.Date).ToList();
            }
        }

        public Result Create(string password = "", AppUserManager userManager = null)
        {
            var result = Result();

            if (userManager == null)
                userManager = AppUserManager.Default();

            IdentityResult iresult;
            try
            {
                if (LoginMethod == UserBo.LoginMethodAD)
                    iresult = userManager.Create(this);
                else
                    iresult = userManager.Create(this, password);

                if (!iresult.Succeeded)
                {
                    foreach (var error in iresult.Errors)
                    {
                        result.AddError(error);
                    }
                }
            }
            catch (Exception e)
            {
                result.AddError(e.Message);
            }

            result.DataTrail = new DataTrail(this, ignoreFields: IgnoreFields());
            return result;
        }

        public Result Update(AppUserManager userManager = null)
        {
            var result = Result();

            if (userManager == null)
                userManager = AppUserManager.Default();

            var user = userManager.FindById(Id);
            if (user == null)
            {
                throw new Exception(MessageBag.NoRecordFound);
            }

            var trail = new DataTrail(user, this, ignoreFields: IgnoreFields());

            user.Id = Id;
            user.Status = Status;
            user.UserName = UserName;
            user.Email = Email;
            user.FullName = FullName;
            user.DepartmentId = DepartmentId;
            user.LoginMethod = LoginMethod;
            user.SessionId = SessionId;
            user.LastLoginAt = LastLoginAt;
            user.PasswordExpiresAt = PasswordExpiresAt;
            user.UpdatedAt = DateTime.Now;
            user.UpdatedById = UpdatedById;

            if (user.Status == UserBo.StatusActive)
            {
                user.LockoutEndDateUtc = null;
            }

            IdentityResult iresult = null;
            if (user.LoginMethod == UserBo.LoginMethodAD && userManager.HasPassword(Id))
            {
                iresult = userManager.RemovePassword(Id);
                user.PasswordExpiresAt = null;
            }

            if (iresult == null || iresult.Succeeded)
            {
                iresult = userManager.Update(user);
            }

            if (!iresult.Succeeded)
            {
                foreach (var error in iresult.Errors)
                {
                    result.AddError(error);
                }
            }

            result.DataTrail = trail;
            return result;
        }

        public Result UpdatePassword(string newPassword, string oldPassword = "")
        {
            var result = Result();

            var userManager = AppUserManager.Default();
            var user = userManager.FindById(Id);
            if (user == null)
            {
                throw new Exception(MessageBag.NoRecordFound);
            }

            var trail = new DataTrail(user, this, ignoreFields: IgnoreFields());
            IdentityResult iresult;
            if (string.IsNullOrEmpty(oldPassword))
            {
                iresult = userManager.RemovePassword(Id);
                if (iresult.Succeeded)
                {
                    iresult = userManager.AddPassword(Id, newPassword);
                }
            }
            else
            {
                iresult = userManager.ChangePassword(Id, oldPassword, newPassword);
            }

            if (iresult.Succeeded)
            {
                PasswordHash = user.PasswordHash;
                int passwordValidity = int.Parse(Util.GetConfig("DaysBeforePasswordExpiry"));
                user.PasswordExpiresAt = DateTime.Today.AddDays(passwordValidity);
                user.UpdatedAt = DateTime.Now;
                user.UpdatedById = UpdatedById;

                iresult = userManager.Update(user);
            }

            if (!iresult.Succeeded)
            {
                foreach (var error in iresult.Errors)
                {
                    result.AddError(error);
                }
            }

            result.DataTrail = trail;
            return result;
        }

        public static DataTrail Delete(int id)
        {
            using (var db = new AppDbContext())
            {
                var user = db.GetUsers().Where(q => q.Id == id).FirstOrDefault();
                if (user == null)
                {
                    throw new Exception(MessageBag.NoRecordFound);
                }

                var trail = new DataTrail(user, true, ignoreFields: IgnoreFields());

                db.Entry(user).State = EntityState.Deleted;
                db.Users.Remove(user);
                db.SaveChanges();

                return trail;
            }
        }

        public static User GetSuperUser()
        {
            return Find(DefaultSuperUserId);
        }

        public static User SeedSuperUser()
        {
            var user = GetSuperUser();
            if (user == null)
            {
                var trail = new TrailObject();

                user = new User
                {
                    Id = DefaultSuperUserId,
                    Status = UserBo.StatusActive,
                    UserName = DefaultSuperUserName,
                    FullName = DefaultSuperFullName,
                    Email = DefaultSuperEmail,
                    LoginMethod = UserBo.LoginMethodPassword,
                    EmailConfirmed = true,
                    LockoutEnabled = false,
                };
                user.Create(DefaultPassword, new AppUserManager(new AppUserStore(new AppDbContext())));

                user.CreatedById = user.Id;
                user.UpdatedById = user.Id;

                user.Update(new AppUserManager(new AppUserStore(new AppDbContext())));

                var userPassword = new UserPassword
                {
                    UserId = user.Id,
                    PasswordHash = user.PasswordHash,
                    CreatedById = user.Id,
                };
                var dataTrail = userPassword.Create();
                dataTrail.Merge(ref trail, UtilAttribute.GetTableName(typeof(UserPassword)));

                string table = UtilAttribute.GetTableName(typeof(User));

                dataTrail = new DataTrail(user, ignoreFields: IgnoreFields());
                dataTrail.Merge(ref trail, table);

                var userTrailBo = new UserTrailBo(
                    user.Id,
                    "Insert Super User",
                    Result(),
                    trail,
                    user.Id
                );

                var userTrail = new UserTrail(userTrailBo);
                userTrail.Create();
            }

            return user;
        }

        public static Result Result()
        {
            return new Result
            {
                Table = UtilAttribute.GetTableName(typeof(User)),
                Controller = ModuleBo.ModuleController.User.ToString()
            };
        }

        public DataTrail AddToAccessGroup(int accessGroupId, out UserAccessGroup uag)
        {
            using (var db = new AppDbContext())
            {
                uag = new UserAccessGroup()
                {
                    UserId = Id,
                    AccessGroupId = accessGroupId,
                };

                db.UserAccessGroups.Add(uag);
                db.SaveChanges();

                var trail = new DataTrail(uag);

                return trail;
            }
        }
    }
}