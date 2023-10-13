using BusinessObject;
using DataAccess.Entities;
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
    public class CedantWorkgroupService
    {
        public static Result Result()
        {
            return new Result
            {
                Table = UtilAttribute.GetTableName(typeof(CedantWorkgroup)),
                Controller = ModuleBo.ModuleController.ClaimDataConfig.ToString()
            };
        }

        public static CedantWorkgroupBo FormBo(CedantWorkgroup entity = null)
        {
            if (entity == null)
                return null;

            return new CedantWorkgroupBo
            {
                Id = entity.Id,
                Code = entity.Code,
                Description = entity.Description,
                CreatedById = entity.CreatedById,
                UpdatedById = entity.UpdatedById,
            };
        }

        public static IList<CedantWorkgroupBo> FormBos(IList<CedantWorkgroup> entities = null)
        {
            if (entities == null)
                return null;
            IList<CedantWorkgroupBo> bos = new List<CedantWorkgroupBo>() { };
            foreach (CedantWorkgroup entity in entities)
            {
                bos.Add(FormBo(entity));
            }
            return bos;
        }

        public static CedantWorkgroup FormEntity(CedantWorkgroupBo bo = null)
        {
            if (bo == null)
                return null;
            return new CedantWorkgroup
            {
                Id = bo.Id,
                Code = bo.Code?.Trim(),
                Description = bo.Description?.Trim(),
                CreatedById = bo.CreatedById,
                UpdatedById = bo.UpdatedById,
            };
        }

        public static bool IsDuplicateCode(CedantWorkgroup cedantWorkgroup)
        {
            return cedantWorkgroup.IsDuplicateCode();
        }

        public static bool IsExists(int id)
        {
            return CedantWorkgroup.IsExists(id);
        }

        public static CedantWorkgroupBo Find(int? id)
        {
            return FormBo(CedantWorkgroup.Find(id));
        }

        public static Result Save(ref CedantWorkgroupBo bo)
        {
            if (!IsExists(bo.Id))
            {
                return Create(ref bo);
            }
            return Update(ref bo);
        }

        public Result Save(ref CedantWorkgroupBo bo, ref TrailObject trail)
        {
            if (!IsExists(bo.Id))
            {
                return Create(ref bo, ref trail);
            }
            return Update(ref bo, ref trail);
        }

        public static Result Create(ref CedantWorkgroupBo bo)
        {
            CedantWorkgroup entity = FormEntity(bo);

            Result result = Result();
            if (IsDuplicateCode(entity))
            {
                result.AddTakenError("Code", bo.Code);
            }

            if (result.Valid)
            {
                result.DataTrail = entity.Create();
                bo.Id = entity.Id;
            }

            return result;
        }

        public static Result Create(ref CedantWorkgroupBo bo, ref TrailObject trail)
        {
            Result result = Create(ref bo);
            if (result.Valid)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static Result Update(ref CedantWorkgroupBo bo)
        {
            Result result = Result();

            CedantWorkgroup entity = CedantWorkgroup.Find(bo.Id);
            if (entity == null)
            {
                throw new Exception(MessageBag.NoRecordFound);
            }
            if (IsDuplicateCode(entity))
            {
                result.AddTakenError("Code", bo.Code);
            }

            if (result.Valid)
            {
                entity.Code = bo.Code;
                entity.Description = bo.Description;
                entity.UpdatedAt = DateTime.Now;
                entity.UpdatedById = bo.UpdatedById ?? entity.UpdatedById;
                result.DataTrail = entity.Update();
            }
            return result;
        }

        public static Result Update(ref CedantWorkgroupBo bo, ref TrailObject trail)
        {
            Result result = Update(ref bo);
            if (result.Valid && result.DataTrail != null)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static Result Delete(CedantWorkgroupBo bo, ref TrailObject trail)
        {
            Result result = Result();

            CedantWorkgroupCedantService.DeleteAllByCedantWorkgroupId(bo.Id, ref trail);
            CedantWorkgroupUserService.DeleteAllByCedantWorkgroupId(bo.Id, ref trail);
            DataTrail dataTrail = CedantWorkgroup.Delete(bo.Id);
            dataTrail.Merge(ref trail, result.Table);

            return result;
        }
    }
}
