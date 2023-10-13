using BusinessObject;
using BusinessObject.Sanctions;
using DataAccess.Entities.Sanctions;
using DataAccess.EntityFramework;
using Shared;
using Shared.DataAccess;
using Shared.Trails;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Sanctions
{
    public class SanctionBlacklistService
    {
        public static Result Result()
        {
            return new Result
            {
                Table = UtilAttribute.GetTableName(typeof(SanctionBlacklist)),
                Controller = ModuleBo.ModuleController.SanctionBlacklist.ToString()
            };
        }

        public static SanctionBlacklistBo FormBo(SanctionBlacklist entity = null)
        {
            if (entity == null)
                return null;
            return new SanctionBlacklistBo
            {
                Id = entity.Id,
                PolicyNumber = entity.PolicyNumber,
                InsuredName = entity.InsuredName,
                CreatedById = entity.CreatedById,
                UpdatedById = entity.UpdatedById,
            };
        }

        public static IList<SanctionBlacklistBo> FormBos(IList<SanctionBlacklist> entities = null)
        {
            if (entities == null)
                return null;
            IList<SanctionBlacklistBo> bos = new List<SanctionBlacklistBo>() { };
            foreach (SanctionBlacklist entity in entities)
            {
                bos.Add(FormBo(entity));
            }
            return bos;
        }

        public static SanctionBlacklist FormEntity(SanctionBlacklistBo bo = null)
        {
            if (bo == null)
                return null;

            return new SanctionBlacklist()
            {
                Id = bo.Id,
                PolicyNumber = bo.PolicyNumber,
                InsuredName = bo.InsuredName,
                CreatedById = bo.CreatedById,
                UpdatedById = bo.UpdatedById,
            };
        }

        public static bool IsExists(int id)
        {
            return SanctionBlacklist.IsExists(id);
        }

        public static bool IsExists(string policyNumber, string insuredName)
        {
            return SanctionBlacklist.IsExists(policyNumber, insuredName);
        }

        public static SanctionBlacklistBo Find(int id)
        {
            return FormBo(SanctionBlacklist.Find(id));
        }

        public static SanctionBlacklistBo Find(string policyNumber, string insuredName)
        {
            return FormBo(SanctionBlacklist.Find(policyNumber, insuredName));
        }

        public static int Count()
        {
            using (var db = new AppDbContext())
            {
                return db.SanctionBlacklists.Count();
            }
        }

        public static Result Save(ref SanctionBlacklistBo bo)
        {
            if (!IsExists(bo.Id))
            {
                return Create(ref bo);
            }
            return Update(ref bo);
        }

        public static Result Save(ref SanctionBlacklistBo bo, ref TrailObject trail)
        {
            if (!IsExists(bo.Id))
            {
                return Create(ref bo, ref trail);
            }
            return Update(ref bo, ref trail);
        }

        public static Result Create(ref SanctionBlacklistBo bo)
        {
            SanctionBlacklist entity = FormEntity(bo);

            Result result = Result();

            if (result.Valid)
            {
                result.DataTrail = entity.Create();
                bo.Id = entity.Id;
            }

            return result;
        }

        public static Result Create(ref SanctionBlacklistBo bo, ref TrailObject trail)
        {
            Result result = Create(ref bo);
            if (result.Valid)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static Result Update(ref SanctionBlacklistBo bo)
        {
            Result result = Result();

            SanctionBlacklist entity = SanctionBlacklist.Find(bo.Id);
            if (entity == null)
            {
                throw new Exception(MessageBag.NoRecordFound);
            }

            if (result.Valid)
            {
                entity.PolicyNumber = bo.PolicyNumber;
                entity.InsuredName = bo.InsuredName;
                entity.UpdatedAt = DateTime.Now;
                entity.UpdatedById = bo.UpdatedById ?? entity.UpdatedById;

                result.DataTrail = entity.Update();
            }
            return result;
        }

        public static Result Update(ref SanctionBlacklistBo bo, ref TrailObject trail)
        {
            Result result = Update(ref bo);
            if (result.Valid && result.DataTrail != null)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static void Delete(SanctionBlacklistBo bo)
        {
            SanctionBlacklist.Delete(bo.Id);
        }

        public static Result Delete(SanctionBlacklistBo bo, ref TrailObject trail)
        {
            Result result = Result();

            if (result.Valid)
            {
                DataTrail dataTrail = SanctionBlacklist.Delete(bo.Id);
                dataTrail.Merge(ref trail, result.Table);
            }

            return result;
        }
    }
}
