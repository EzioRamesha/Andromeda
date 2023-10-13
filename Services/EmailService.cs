using BusinessObject;
using DataAccess.Entities;
using DataAccess.EntityFramework;
using Services.Identity;
using Shared;
using Shared.DataAccess;
using Shared.Trails;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class EmailService
    {
        public static Result Result()
        {
            return new Result
            {
                Table = UtilAttribute.GetTableName(typeof(Email)),
                Controller = ModuleBo.ModuleController.User.ToString()
            };
        }

        public static EmailBo FormBo(Email entity = null, bool foreign = false)
        {
            if (entity == null)
                return null;
            return new EmailBo
            {
                Id = entity.Id,
                RecipientUserId = entity.RecipientUserId,
                RecipientUserBo = foreign ? UserService.Find(entity.RecipientUserId) : null,
                ModuleController = entity.ModuleController,
                ObjectId = entity.ObjectId,
                Type = entity.Type,
                Status = entity.Status,
                EmailAddress = entity.EmailAddress,
                Data = entity.Data,
                CreatedById = entity.CreatedById,
                UpdatedById = entity.UpdatedById,
            };
        }

        public static IList<EmailBo> FormBos(IList<Email> entities = null, bool simplified = false)
        {
            if (entities == null)
                return null;
            IList<EmailBo> bos = new List<EmailBo>() { };
            foreach (Email entity in entities)
            {
                bos.Add(FormBo(entity, simplified));
            }
            return bos;
        }

        public static Email FormEntity(EmailBo bo = null)
        {
            if (bo == null)
                return null;
            return new Email
            {
                Id = bo.Id,
                RecipientUserId = bo.RecipientUserId,
                ModuleController = bo.ModuleController,
                ObjectId = bo.ObjectId,
                Type = bo.Type,
                Status = bo.Status,
                EmailAddress = bo.EmailAddress,
                Data = bo.Data,
                CreatedById = bo.CreatedById,
                UpdatedById = bo.UpdatedById,
            };
        }

        public static EmailBo Find(int? id)
        {
            if (!id.HasValue)
                return null;

            return FormBo(Email.Find(id.Value));
        }

        public static List<string> GetUserNameByModuleObject(string moduleController, int objectId)
        {
            using (var db = new AppDbContext())
            {
                return db.Emails.LeftOuterJoin(db.Users, e => e.RecipientUserId, u => u.Id, (e, u) => new { Email = e, User = u })
                    .Where(q => q.Email.ModuleController == moduleController)
                    .Where(q => q.Email.ObjectId == objectId)
                    .Select(q => q.User == null ? q.Email.EmailAddress : q.User.FullName)
                    .ToList();

                //List<int> userIds = db.Emails.Where(q => q.ModuleController == moduleController && q.ObjectId == objectId && q.RecipientUserId.HasValue).Select(q => q.RecipientUserId.Value).ToList();
                //return db.Users.Where(q => userIds.Contains(q.Id)).Select(q => q.FullName).ToList();
            }
        }

        public static Result Create(ref EmailBo bo)
        {
            Email entity = FormEntity(bo);

            Result result = Result();
            if (result.Valid)
            {
                result.DataTrail = entity.Create();
                bo.Id = entity.Id;
            }

            return result;
        }

        public static Result Create(ref EmailBo bo, ref TrailObject trail)
        {
            Result result = Create(ref bo);
            if (result.Valid)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static Result Update(ref EmailBo bo)
        {
            Result result = Result();

            var entity = Email.Find(bo.Id);
            if (entity == null)
            {
                throw new Exception(MessageBag.NoRecordFound);
            }

            if (result.Valid)
            {
                entity = FormEntity(bo);
                entity.UpdatedAt = DateTime.Now;
                entity.UpdatedById = bo.UpdatedById ?? entity.UpdatedById;
                result.DataTrail = entity.Update();
            }

            return result;
        }

        public static Result Update(ref EmailBo bo, ref TrailObject trail)
        {
            Result result = Update(ref bo);
            if (result.Valid && result.DataTrail != null)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static void Delete(EmailBo bo)
        {
            Email.Delete(bo.Id);
        }

        public static Result Delete(EmailBo bo, ref TrailObject trail)
        {
            Result result = Result();

            DataTrail dataTrail = Email.Delete(bo.Id);
            dataTrail.Merge(ref trail, result.Table);

            return result;
        }
    }
}
