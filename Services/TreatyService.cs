using BusinessObject;
using DataAccess.Entities;
using DataAccess.Entities.RiDatas;
using DataAccess.EntityFramework;
using Shared;
using Shared.DataAccess;
using Shared.Trails;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Services
{
    public class TreatyService
    {
        public static Result Result()
        {
            return new Result
            {
                Table = UtilAttribute.GetTableName(typeof(Treaty)),
                Controller = ModuleBo.ModuleController.Treaty.ToString()
            };
        }

        public static TreatyBo FormBo(Treaty entity = null, bool foreign = true)
        {
            if (entity == null)
                return null;
            return new TreatyBo
            {
                Id = entity.Id,
                CedantId = entity.CedantId,
                CedantBo = foreign ? CedantService.Find(entity.CedantId) : null,
                TreatyIdCode = entity.TreatyIdCode,
                Description = entity.Description,
                Remarks = entity.Remarks,
                CreatedById = entity.CreatedById,
                UpdatedById = entity.UpdatedById,
                BusinessOriginPickListDetailId = entity.BusinessOriginPickListDetailId,
                BusinessOriginPickListDetailBo = PickListDetailService.Find(entity.BusinessOriginPickListDetailId),
                BlockDescription = entity.BlockDescription,
            };
        }

        public static IList<TreatyBo> FormBos(IList<Treaty> entities = null)
        {
            if (entities == null)
                return null;
            IList<TreatyBo> bos = new List<TreatyBo>() { };
            foreach (Treaty entity in entities)
            {
                bos.Add(FormBo(entity));
            }
            return bos;
        }

        public static Treaty FormEntity(TreatyBo bo = null)
        {
            if (bo == null)
                return null;
            return new Treaty
            {
                Id = bo.Id,
                CedantId = bo.CedantId,
                TreatyIdCode = bo.TreatyIdCode?.Trim(),
                Description = bo.Description?.Trim(),
                Remarks = bo.Remarks?.Trim(),
                CreatedById = bo.CreatedById,
                UpdatedById = bo.UpdatedById,
                BusinessOriginPickListDetailId = bo.BusinessOriginPickListDetailId,
                BlockDescription = bo.BlockDescription?.Trim(),
            };
        }

        public static bool IsDuplicateCode(Treaty treaty)
        {
            using (var db = new AppDbContext())
            {
                if (!string.IsNullOrEmpty(treaty.TreatyIdCode?.Trim()))
                {
                    var query = db.Treaties.Where(q => q.TreatyIdCode.Trim().Equals(treaty.TreatyIdCode.Trim(), StringComparison.OrdinalIgnoreCase));
                    if (treaty.Id != 0)
                    {
                        query = query.Where(q => q.Id != treaty.Id);
                    }
                    return query.Count() > 0;
                }
                return false;
            }
        }

        public static bool IsExists(int id)
        {
            return Treaty.IsExists(id);
        }

        public static TreatyBo Find(int id)
        {
            return FormBo(Treaty.Find(id));
        }

        public static TreatyBo Find(int? id)
        {
            return FormBo(Treaty.Find(id));
        }

        public static TreatyBo FindByCode(string code, bool foreign = true)
        {
            return FormBo(Treaty.FindByCode(code), foreign);
        }

        public static int CountByCode(string code)
        {
            return Treaty.CountByCode(code);
        }

        public static int CountByBusinessOriginPickListDetailId(int businessOriginPickListDetailId)
        {
            return Treaty.CountByBusinessOriginPickListDetailId(businessOriginPickListDetailId);
        }

        public static IList<TreatyBo> Get()
        {
            return FormBos(Treaty.Get());
        }

        public static IList<TreatyBo> GetByCedantId(int cedantId)
        {
            return FormBos(Treaty.GetByCedantId(cedantId));
        }

        public static IEnumerable<string> GetTreatyIdCodesByCedantId(int cedantId)
        {
            using (var db = new AppDbContext(false))
            {
                return db.Treaties.Where(q => q.CedantId == cedantId).Select(q => q.TreatyIdCode).ToList();
            }
        }

        public static Result Save(ref TreatyBo bo)
        {
            if (!Treaty.IsExists(bo.Id))
            {
                return Create(ref bo);
            }
            return Update(ref bo);
        }

        public Result Save(ref TreatyBo bo, ref TrailObject trail)
        {
            if (!Treaty.IsExists(bo.Id))
            {
                return Create(ref bo, ref trail);
            }
            return Update(ref bo, ref trail);
        }

        public static Result Create(ref TreatyBo bo)
        {
            Treaty entity = FormEntity(bo);

            Result result = Result();
            if (IsDuplicateCode(entity))
            {
                result.AddTakenError("Treaty ID", bo.TreatyIdCode);
            }
            if (result.Valid)
            {
                result.DataTrail = entity.Create();
                bo.Id = entity.Id;
            }

            return result;
        }

        public static Result Create(ref TreatyBo bo, ref TrailObject trail)
        {
            Result result = Create(ref bo);
            if (result.Valid)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static Result Update(ref TreatyBo bo)
        {
            Result result = Result();

            Treaty entity = Treaty.Find(bo.Id);
            if (entity == null)
            {
                throw new Exception(MessageBag.NoRecordFound);
            }
            if (IsDuplicateCode(FormEntity(bo)))
            {
                result.AddTakenError("Treaty ID", bo.TreatyIdCode);
            }

            if (result.Valid)
            {
                entity.CedantId = bo.CedantId;
                entity.TreatyIdCode = bo.TreatyIdCode;
                entity.Description = bo.Description;
                entity.BusinessOriginPickListDetailId = bo.BusinessOriginPickListDetailId;
                entity.BlockDescription = bo.BlockDescription;
                entity.Remarks = bo.Remarks;
                entity.UpdatedAt = DateTime.Now;
                entity.UpdatedById = bo.UpdatedById ?? entity.UpdatedById;
                result.DataTrail = entity.Update();
            }
            return result;
        }

        public static Result Update(ref TreatyBo bo, ref TrailObject trail)
        {
            Result result = Update(ref bo);
            if (result.Valid && result.DataTrail != null)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static void Delete(TreatyBo bo)
        {
            Treaty.Delete(bo.Id);
        }

        public static Result Delete(TreatyBo bo, ref TrailObject trail)
        {
            Result result = Result();

            if (
                TreatyCode.CountByTreatyId(bo.Id) > 0 ||
                RiDataConfig.CountByTreatyId(bo.Id) > 0 ||
                RiDataBatch.CountByTreatyId(bo.Id) > 0
            )
            {
                result.AddErrorRecordInUsed();
            }

            if (result.Valid)
            {
                DataTrail dataTrail = Treaty.Delete(bo.Id);
                dataTrail.Merge(ref trail, result.Table);
            }

            return result;
        }
    }
}