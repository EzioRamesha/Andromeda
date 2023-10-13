using BusinessObject;
using BusinessObject.Identity;
using DataAccess.Entities.Identity;
using DataAccess.EntityFramework;
using Microsoft.AspNet.Identity;
using Shared;
using Shared.DataAccess;
using Shared.DirectoryServices;
using Shared.Trails;
using System;
using System.Collections.Generic;
using System.DirectoryServices.AccountManagement;
using System.Linq;
using System.Web;

namespace Services.Identity
{
    public class UserService
    {
        public static Result Result()
        {
            return new Result
            {
                Table = UtilAttribute.GetTableName(typeof(User)),
                Controller = ModuleBo.ModuleController.User.ToString()
            };
        }

        public static UserBo FormBo(User entity = null, bool simplified = false)
        {
            if (entity == null)
                return null;
            UserBo userBo = new UserBo
            {
                Id = entity.Id,
                Status = entity.Status,
                UserName = entity.UserName,
                Email = entity.Email,
                FullName = entity.FullName,
                DepartmentId = entity.DepartmentId,
                LoginMethod = entity.LoginMethod,
                SessionId = entity.SessionId,
                PasswordExpiresAt = entity.PasswordExpiresAt,
                LastLoginAt = entity.LastLoginAt,
                CreatedById = entity.CreatedById,
                UpdatedById = entity.UpdatedById,
                CreatedAt = entity.CreatedAt,
            };

            if (!simplified)
            {
                userBo.DepartmentBo = DepartmentService.Find(entity.DepartmentId);
                userBo.UserAccessGroupBos = UserAccessGroupService.GetByUserId(entity.Id);
            }

            return userBo;
        }

        public static IList<UserBo> FormBos(IList<User> entities = null, bool simplified = false)
        {
            if (entities == null)
                return null;
            IList<UserBo> bos = new List<UserBo>() { };
            foreach (User entity in entities)
            {
                bos.Add(FormBo(entity, simplified));
            }
            return bos;
        }

        public static User FormEntity(UserBo bo = null)
        {
            if (bo == null)
                return null;
            return new User
            {
                Id = bo.Id,
                Status = bo.Status,
                UserName = bo.UserName,
                Email = bo.Email,
                FullName = bo.FullName,
                DepartmentId = bo.DepartmentId,
                LoginMethod = bo.LoginMethod,
                SessionId = bo.SessionId,
                PasswordExpiresAt = bo.PasswordExpiresAt,
                LastLoginAt = bo.LastLoginAt,
                CreatedById = bo.CreatedById,
                UpdatedById = bo.UpdatedById,
            };
        }

        public static UserBo Find(int id, bool simplified = false, bool deleted = false)
        {
            return FormBo(User.Find(id, deleted), simplified);
        }

        public static UserBo Find(int? id)
        {
            if (id.HasValue)
                return FormBo(User.Find(id.Value));
            return null;
        }

        public static UserBo FindByUsername(string userName)
        {
            return FormBo(User.FindByUserName(userName));
        }

        public static UserBo FindByFullName(string fullName)
        {
            using (var db = new AppDbContext())
            {
                return FormBo(db.GetUsers().Where(q => q.FullName.ToLower() == fullName.ToLower()).FirstOrDefault());
            }
        }

        public static UserBo FindByEmail(string email)
        {
            using (var db = new AppDbContext())
            {
                return FormBo(db.GetUsers().Where(q => q.Email.ToLower() == email.ToLower()).FirstOrDefault());
            }
        }

        public static UserBo FindByUsernameOrEmail(string name)
        {
            User user = User.FindByUserName(name);
            if (user == null && name.IndexOf('@') > -1)
            {
                user = User.FindByEmail(name);
            }

            return FormBo(user);
        }

        public static string GetNameById(int? id)
        {
            var bo = Find(id);
            if (bo != null)
            {
                return bo.UserName;
            }
            return "";
        }

        public static IList<UserBo> GetByIds(List<int> ids)
        {
            using (var db = new AppDbContext())
            {
                return FormBos(db.Users.Where(q => ids.Contains(q.Id)).ToList(), true);
            }
        }

        public static IList<string> GetEmailUsers(int? departmentId = null)
        {
            using (var db = new AppDbContext())
            {
                var query = db.Users.Where(q => !string.IsNullOrEmpty(q.Email));

                if (departmentId.HasValue)
                {
                    query = query.Where(q => q.DepartmentId == departmentId);
                }

                return query.Select(q => q.Email).ToList();
            }
        }

        public static UserBo FindInActiveDirectory(string userName)
        {
            try
            {
                DirectoryService directoryService = new DirectoryService();
                UserPrincipal userPrincipal = UserPrincipal.FindByIdentity(directoryService.Context, IdentityType.SamAccountName, userName);

                if (userPrincipal == null)
                    return null;

                string email = directoryService.Email(userPrincipal.SamAccountName);
                string name = "";
                if (!string.IsNullOrEmpty(userPrincipal.Name))
                {
                    name = userPrincipal.Name;
                }
                else if (!string.IsNullOrEmpty(userPrincipal.DisplayName))
                {
                    name = userPrincipal.DisplayName;
                }
                else if (!string.IsNullOrEmpty(userPrincipal.GivenName))
                {
                    name = userPrincipal.GivenName;
                }
                else if (!string.IsNullOrEmpty(userPrincipal.MiddleName))
                {
                    name = userPrincipal.MiddleName;
                }
                else if (!string.IsNullOrEmpty(userPrincipal.Surname))
                {
                    name = userPrincipal.Surname;
                }

                User entity = new User
                {
                    UserName = userPrincipal.SamAccountName,
                    FullName = name,
                    Email = email,
                };

                if (entity != null)
                {
                    return UserService.FormBo(entity);
                }
                return null;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public static int CountByDepartmentId(int departmentId)
        {
            return User.CountByDepartmentId(departmentId);
        }

        public static IList<UserBo> Get()
        {
            return FormBos(User.Get());
        }

        public static IList<UserBo> GetByStatus(int? status = null, List<int> selectedIds = null, bool exceptSuper = false, int? departmentId = null)
        {
            return FormBos(User.GetByStatus(status, selectedIds, exceptSuper, departmentId));
        }

        public static IList<UserBo> GetByDepartments(int? status = null, int? selectedId = null, List<int> departmentIds = null)
        {
            using (var db = new AppDbContext())
            {
                var query = db.Users.AsQueryable();

                if (status != null)
                {
                    if (selectedId != null)
                        query = query.Where(q => q.Status == status || q.Id == selectedId);
                    else
                        query = query.Where(q => q.Status == status);
                }
                else
                {
                    query = query.Where(q => q.Status != UserBo.StatusDelete);
                }

                if (departmentIds != null)
                {
                    query = query.Where(q => q.DepartmentId.HasValue && departmentIds.Contains(q.DepartmentId.Value));
                }

                return FormBos(query.OrderBy(q => q.UserName).ToList());
            }
        }

        public static IList<UserBo> GetByModulePower(int moduleId, string power)
        {
            IList<AccessMatrixBo> accessMatrixBos = AccessMatrixService.GetByModule(moduleId);
            List<int> accessGroupId = new List<int>();
            foreach (AccessMatrixBo accessMatrixBo in accessMatrixBos)
            {
                if (accessMatrixBo.IsPowerExists(power))
                    accessGroupId.Add(accessMatrixBo.AccessGroupId);
            }

            return FormBos(User.GetByAccessGroupId(accessGroupId));
        }

        public static List<string> GetUsersByModulePower(int moduleId, string power)
        {
            List<string> users = new List<string>();
            foreach (var userBo in UserService.GetByModulePower(moduleId, power))
            {
                users.Add(userBo.UserName);
            }
            return users;
        }

        public static List<UserBo> GetUserBosByModulePower(int moduleId, string power)
        {
            List<UserBo> users = new List<UserBo>();
            foreach (var userBo in UserService.GetByModulePower(moduleId, power))
            {
                users.Add(userBo);
            }
            return users;
        }

        public static IList<UserBo> GetByAuthorityLimit(string claimCode, double? amount = 0)
        {
            using (var db = new AppDbContext())
            {
                if (!amount.HasValue)
                    amount = 0;

                var subQuery = db.ClaimAuthorityLimitMLReDetails.Where(c => c.ClaimCode.Code == claimCode && c.ClaimAuthorityLimitValue >= amount).Select(c => c.ClaimAuthorityLimitMLRe.UserId);
                var subQuery2 = db.ClaimAuthorityLimitMLRe.Where(c => c.IsAllowOverwriteApproval).Select(c => c.UserId);
                return FormBos(db.GetUsers().Where(q => (subQuery.Contains(q.Id) || subQuery2.Contains(q.Id)) && q.DepartmentId == DepartmentBo.DepartmentClaim).OrderBy(q => q.UserName).ToList());
            }
        }

        public static void AddToAccessGroup(UserBo userBo, int accessGroupId, ref TrailObject trail)
        {
            User entity = User.Find(userBo.Id);
            if (entity == null)
            {
                throw new Exception(MessageBag.NoRecordFound);
            }

            UserAccessGroupBo uagBo = UserAccessGroupService.Find(userBo.Id, accessGroupId);
            if (uagBo == null)
            {
                UserAccessGroupService.DeleteAllByUserId(userBo.Id, ref trail);

                var dataTrail = entity.AddToAccessGroup(accessGroupId, out UserAccessGroup uag);
                dataTrail.Merge(ref trail, UtilAttribute.GetTableName(typeof(UserAccessGroup)), uag.PrimaryKey());
            }
        }

        public static bool CheckPowerFlag(User user, string controller, string power)
        {
            var userBo = FormBo(user);
            return CheckPowerFlag(userBo, controller, power);
        }

        public static bool CheckPowerFlag(UserBo user, string controller, string power)
        {
            var valid = false;
            foreach (UserAccessGroupBo userAccessGroupBo in user.UserAccessGroupBos)
            {
                var accessMatrixBo = AccessMatrixService.FindByController(controller, userAccessGroupBo.AccessGroupId);
                if (accessMatrixBo != null && accessMatrixBo.IsPowerExists(power))
                {
                    valid = true;
                    break;
                }
            }
            return valid;
        }

        public static Result Create(ref UserBo bo)
        {
            Result result = Result();
            if (bo.LoginMethod == UserBo.LoginMethodAD)
            {
                try
                {
                    UserBo adUser = FindInActiveDirectory(bo.UserName);
                    if (adUser == null)
                    {
                        result.AddError(MessageBag.UserNotFoundInAd);
                    }
                }
                catch (Exception e)
                {
                    result.AddError(e.Message);
                }

                if (!result.Valid)
                    return result;
            }
            else
            {
                int passwordValidity = int.Parse(Util.GetConfig("DaysBeforePasswordExpiry"));
                bo.PasswordExpiresAt = DateTime.Today.AddDays(passwordValidity);
            }

            User entity = FormEntity(bo);
            result = entity.Create(bo.Password);

            // Update value after insert into database
            bo.Id = entity.Id;
            bo.PasswordHash = entity.PasswordHash;

            return result;
        }

        public static Result Create(ref UserBo bo, ref TrailObject trail)
        {
            Result result = Create(ref bo);
            result.Table = UtilAttribute.GetTableName(typeof(User));
            if (result.Valid)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static Result Update(ref UserBo bo)
        {
            User entity = User.Find(bo.Id);
            if (entity == null)
            {
                throw new Exception(MessageBag.NoRecordFound);
            }

            Result result = new Result();
            if (bo.LoginMethod == UserBo.LoginMethodAD && (entity.LoginMethod == UserBo.LoginMethodPassword || bo.UserName != entity.UserName))
            {
                try
                {
                    UserBo adUser = FindInActiveDirectory(bo.UserName);
                    if (adUser == null)
                    {
                        result.AddError(MessageBag.UserNotFoundInAd);
                    }
                }
                catch (Exception e)
                {
                    result.AddError(e.Message);
                }

                if (!result.Valid)
                    return result;
            }

            entity.Status = bo.Status;
            entity.UserName = bo.UserName;
            entity.Email = bo.Email;
            entity.FullName = bo.FullName;
            entity.DepartmentId = bo.DepartmentId;
            entity.LoginMethod = bo.LoginMethod;
            entity.UpdatedAt = DateTime.Now;
            entity.UpdatedById = bo.UpdatedById ?? entity.UpdatedById;
            return entity.Update();
        }

        public static Result Update(ref UserBo bo, ref TrailObject trail)
        {
            Result result = Update(ref bo);
            result.Table = UtilAttribute.GetTableName(typeof(User));
            if (result.Valid && result.DataTrail != null)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static Result UpdatePassword(ref UserBo bo, string newPassword, string oldPassword = "")
        {
            User entity = User.Find(bo.Id);
            if (entity == null)
            {
                throw new Exception(MessageBag.NoRecordFound);
            }

            Result result = Result();
            foreach (UserPassword userPassword in UserPassword.GetByUserId(bo.Id))
            {
                string password = userPassword.PasswordHash;
                PasswordHasher passwordHasher = new PasswordHasher();
                var checkResult = passwordHasher.VerifyHashedPassword(password, newPassword);
                if (checkResult != PasswordVerificationResult.Failed)
                {
                    result.AddError(MessageBag.PasswordUsed);
                    return result;
                }
            }

            result = entity.UpdatePassword(newPassword, oldPassword);
            bo.PasswordHash = entity.PasswordHash;

            return result;
        }

        public static Result UpdatePassword(ref UserBo bo, string oldPassword, string newPassword, ref TrailObject trail)
        {
            Result result = UpdatePassword(ref bo, newPassword, oldPassword);
            if (result.Valid)
            {
                var userPasswordBo = new UserPasswordBo
                {
                    UserId = bo.Id,
                    PasswordHash = bo.PasswordHash,
                    CreatedById = bo.UpdatedById ?? bo.CreatedById ?? User.DefaultSuperUserId,
                };
                UserPasswordService.Create(ref userPasswordBo, ref trail);

                int skip = int.Parse(Util.GetConfig("PasswordReuse"));
                var userPasswords = UserPasswordService.GetByUserId(userPasswordBo.UserId, skip);
                foreach (UserPasswordBo exceedUserPasswordBo in userPasswords)
                {
                    UserPasswordService.Delete(exceedUserPasswordBo, ref trail);
                }

                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static Result UpdatePassword(int id, string oldPassword, string newPassword, ref TrailObject trail)
        {
            UserBo bo = Find(id);
            Result result = UpdatePassword(ref bo, oldPassword, newPassword, ref trail);
            return result;
        }

        public static Result UpdatePassword(ref UserBo bo, string newPassword, ref TrailObject trail)
        {
            Result result = UpdatePassword(ref bo, "", newPassword, ref trail);
            return result;
        }

        public static void UpdateLastLoginAt(ref UserBo userBo, string ipAddress)
        {
            User entity = User.Find(userBo.Id);
            if (entity == null)
            {
                throw new Exception(MessageBag.NoRecordFound);
            }
            entity.SessionId = HttpContext.Current.Session.SessionID;
            entity.LastLoginAt = DateTime.Now;
            entity.UpdatedAt = DateTime.Now;
            entity.UpdatedById = entity.Id;
            entity.Update();

            UserTrailService.CreateLoginTrail(entity, ipAddress);
        }

        public static UserBo UpdatePasswordExpiresAt(ref UserBo bo)
        {
            User entity = User.Find(bo.Id);
            if (entity == null)
            {
                throw new Exception(MessageBag.NoRecordFound);
            }
            int passwordValidity = int.Parse(Util.GetConfig("DaysBeforePasswordExpiry"));
            entity.PasswordExpiresAt = DateTime.Today.AddDays(passwordValidity);
            entity.UpdatedAt = DateTime.Now;
            entity.Update();

            return FormBo(entity);
        }

        public static Result UpdateStatus(ref UserBo bo, ref TrailObject trail)
        {
            Result result = Update(ref bo, ref trail);
            ModuleBo moduleBo = ModuleService.FindByController(ModuleBo.ModuleController.User.ToString());
            StatusHistoryBo statusHistoryBo = new StatusHistoryBo
            {
                ModuleId = moduleBo.Id,
                ObjectId = bo.Id,
                Status = bo.Status,
                CreatedById = bo.Id,
                UpdatedById = bo.Id
            };
            StatusHistoryService.Save(ref statusHistoryBo, ref trail);

            return result;
        }

        public static void Delete(UserBo userBo)
        {
            UserAccessGroupService.DeleteAllByUserId(userBo.Id);
            UserPasswordService.DeleteAllByUserId(userBo.Id);
        }

        public static Result Delete(ref UserBo bo, ref TrailObject trail)
        {
            Result result = new Result();

            if (
                DepartmentService.CountByHodUser(bo.Id) > 0
            )
            {
                result.AddErrorRecordInUsed();
            }

            if (result.Valid)
            {
                result = UpdateStatus(ref bo, ref trail);
                UserAccessGroupService.DeleteAllByUserId(bo.Id, ref trail);
                CedantWorkgroupUserService.DeleteByUserId(bo.Id, ref trail);
            }

            return result;
        }
    }
}
