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
    public class SanctionWhitelistService
    {
        public static Result Result()
        {
            return new Result
            {
                Table = UtilAttribute.GetTableName(typeof(SanctionWhitelist)),
                Controller = ModuleBo.ModuleController.SanctionWhitelist.ToString()
            };
        }

        public static SanctionWhitelistBo FormBo(SanctionWhitelist entity = null)
        {
            if (entity == null)
                return null;
            return new SanctionWhitelistBo
            {
                Id = entity.Id,
                PolicyNumber = entity.PolicyNumber,
                InsuredName = entity.InsuredName,
                Reason = entity.Reason,
                CreatedById = entity.CreatedById,
                UpdatedById = entity.UpdatedById,
            };
        }

        public static IList<SanctionWhitelistBo> FormBos(IList<SanctionWhitelist> entities = null)
        {
            if (entities == null)
                return null;
            IList<SanctionWhitelistBo> bos = new List<SanctionWhitelistBo>() { };
            foreach (SanctionWhitelist entity in entities)
            {
                bos.Add(FormBo(entity));
            }
            return bos;
        }

        public static SanctionWhitelist FormEntity(SanctionWhitelistBo bo = null)
        {
            if (bo == null)
                return null;

            return new SanctionWhitelist()
            {
                Id = bo.Id,
                PolicyNumber = bo.PolicyNumber,
                InsuredName = bo.InsuredName,
                Reason = bo.Reason,
                CreatedById = bo.CreatedById,
                UpdatedById = bo.UpdatedById,
            };
        }

        public static bool IsExists(int id)
        {
            return SanctionWhitelist.IsExists(id);
        }

        public static bool IsExists(string policyNumber, string insuredName)
        {
            return SanctionWhitelist.IsExists(policyNumber, insuredName);
        }

        public static SanctionWhitelistBo Find(int id)
        {
            return FormBo(SanctionWhitelist.Find(id));
        }

        public static SanctionWhitelistBo Find(string policyNumber, string insuredName)
        {
            return FormBo(SanctionWhitelist.Find(policyNumber, insuredName));
        }

        public static int Count()
        {
            using (var db = new AppDbContext())
            {
                return db.SanctionWhitelists.Count();
            }
        }

        public static Result Save(ref SanctionWhitelistBo bo)
        {
            if (!IsExists(bo.Id))
            {
                return Create(ref bo);
            }
            return Update(ref bo);
        }

        public static Result Save(ref SanctionWhitelistBo bo, ref TrailObject trail)
        {
            if (!IsExists(bo.Id))
            {
                return Create(ref bo, ref trail);
            }
            return Update(ref bo, ref trail);
        }

        public static Result Create(ref SanctionWhitelistBo bo)
        {
            SanctionWhitelist entity = FormEntity(bo);

            Result result = Result();

            if (result.Valid)
            {
                result.DataTrail = entity.Create();
                bo.Id = entity.Id;
            }

            return result;
        }

        public static Result Create(ref SanctionWhitelistBo bo, ref TrailObject trail)
        {
            Result result = Create(ref bo);
            if (result.Valid)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static Result Update(ref SanctionWhitelistBo bo)
        {
            Result result = Result();

            SanctionWhitelist entity = SanctionWhitelist.Find(bo.Id);
            if (entity == null)
            {
                throw new Exception(MessageBag.NoRecordFound);
            }

            if (result.Valid)
            {
                entity.PolicyNumber = bo.PolicyNumber;
                entity.InsuredName = bo.InsuredName;
                entity.Reason = bo.Reason;
                entity.UpdatedAt = DateTime.Now;
                entity.UpdatedById = bo.UpdatedById ?? entity.UpdatedById;

                result.DataTrail = entity.Update();
            }
            return result;
        }

        public static Result Update(ref SanctionWhitelistBo bo, ref TrailObject trail)
        {
            Result result = Update(ref bo);
            if (result.Valid && result.DataTrail != null)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static void Delete(SanctionWhitelistBo bo)
        {
            SanctionWhitelist.Delete(bo.Id);
        }

        public static Result Delete(SanctionWhitelistBo bo, ref TrailObject trail)
        {
            Result result = Result();

            if (result.Valid)
            {
                DataTrail dataTrail = SanctionWhitelist.Delete(bo.Id);
                dataTrail.Merge(ref trail, result.Table);
            }

            return result;
        }
    }
}
