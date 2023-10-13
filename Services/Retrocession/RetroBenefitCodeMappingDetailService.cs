using BusinessObject;
using BusinessObject.Retrocession;
using DataAccess.Entities.Retrocession;
using DataAccess.EntityFramework;
using Shared;
using Shared.DataAccess;
using Shared.Trails;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Services.Retrocession
{
    public class RetroBenefitCodeMappingDetailService
    {
        public static Result Result()
        {
            return new Result
            {
                Table = UtilAttribute.GetTableName(typeof(RetroBenefitCodeMappingDetail)),
                Controller = ModuleBo.ModuleController.RetroBenefitCodeMappingDetail.ToString()
            };
        }

        public static Expression<Func<RetroBenefitCodeMappingDetail, RetroBenefitCodeMappingDetailBo>> Expression()
        {
            return entity => new RetroBenefitCodeMappingDetailBo
            {
                Id = entity.Id,
                RetroBenefitCodeMappingId = entity.RetroBenefitCodeMappingId,
                RetroBenefitCodeId = entity.RetroBenefitCodeId,
                IsComputePremium = entity.IsComputePremium,

                CreatedById = entity.CreatedById,
                UpdatedById = entity.UpdatedById,
            };
        }

        public static RetroBenefitCodeMappingDetailBo FormBo(RetroBenefitCodeMappingDetail entity = null)
        {
            if (entity == null)
                return null;

            return new RetroBenefitCodeMappingDetailBo
            {
                Id = entity.Id,
                RetroBenefitCodeMappingId = entity.RetroBenefitCodeMappingId,
                RetroBenefitCodeMappingBo = RetroBenefitCodeMappingService.Find(entity.RetroBenefitCodeMappingId),
                RetroBenefitCodeId = entity.RetroBenefitCodeId,
                RetroBenefitCodeBo = RetroBenefitCodeService.Find(entity.RetroBenefitCodeId),
                IsComputePremium = entity.IsComputePremium,

                CreatedById = entity.CreatedById,
                CreatedAt = entity.CreatedAt,
                UpdatedById = entity.UpdatedById,
            };
        }

        public static IList<RetroBenefitCodeMappingDetailBo> FormBos(IList<RetroBenefitCodeMappingDetail> entities = null)
        {
            if (entities == null)
                return null;
            IList<RetroBenefitCodeMappingDetailBo> bos = new List<RetroBenefitCodeMappingDetailBo>() { };
            foreach (RetroBenefitCodeMappingDetail entity in entities)
            {
                bos.Add(FormBo(entity));
            }
            return bos;
        }

        public static RetroBenefitCodeMappingDetail FormEntity(RetroBenefitCodeMappingDetailBo bo = null)
        {
            if (bo == null)
                return null;
            return new RetroBenefitCodeMappingDetail
            {
                Id = bo.Id,
                RetroBenefitCodeMappingId = bo.RetroBenefitCodeMappingId,
                RetroBenefitCodeId = bo.RetroBenefitCodeId,
                IsComputePremium = bo.IsComputePremium,
                CreatedById = bo.CreatedById,
                UpdatedById = bo.UpdatedById,
            };
        }

        public static bool IsExists(int id)
        {
            return RetroBenefitCodeMappingDetail.IsExists(id);
        }

        public static RetroBenefitCodeMappingDetailBo Find(int id)
        {
            return FormBo(RetroBenefitCodeMappingDetail.Find(id));
        }

        public static IList<RetroBenefitCodeMappingDetailBo> GetByRetroBenefitCodeMappingId(int retroBenefitCodeMappingId)
        {
            using (var db = new AppDbContext())
            {
                return FormBos(db.RetroBenefitCodeMappingDetails.Where(q => q.RetroBenefitCodeMappingId == retroBenefitCodeMappingId).ToList());
            }
        }

        public static IList<RetroBenefitCodeMappingDetailBo> GetByRetroBenefitCodeMappingIdExcept(int retroBenefitCodeMappingId, List<int> ids)
        {
            using (var db = new AppDbContext())
            {
                return FormBos(db.RetroBenefitCodeMappingDetails.Where(q => q.RetroBenefitCodeMappingId == retroBenefitCodeMappingId && !ids.Contains(q.Id)).ToList());
            }
        }

        public static List<int> GetRetroBenefitCodeIdByRetroBenefitCodeMappingId(int retroBenefitCodeMappingId)
        {
            using (var db = new AppDbContext())
            {
                return db.RetroBenefitCodeMappingDetails.Where(q => q.RetroBenefitCodeMappingId == retroBenefitCodeMappingId).Select(q => q.RetroBenefitCodeId).ToList();
            }
        }

        public static int CountByRetroBenefitCodeId(int retroBenefitCodeId)
        {
            using (var db = new AppDbContext())
            {
                return db.RetroBenefitCodeMappingDetails.Where(q => q.RetroBenefitCodeId == retroBenefitCodeId).Count();
            }
        }

        public static Result Save(ref RetroBenefitCodeMappingDetailBo bo)
        {
            if (!RetroBenefitCodeMappingDetail.IsExists(bo.Id))
            {
                return Create(ref bo);
            }
            return Update(ref bo);
        }

        public static Result Save(ref RetroBenefitCodeMappingDetailBo bo, ref TrailObject trail)
        {
            if (!RetroBenefitCodeMappingDetail.IsExists(bo.Id))
            {
                return Create(ref bo, ref trail);
            }
            return Update(ref bo, ref trail);
        }

        public static Result Create(ref RetroBenefitCodeMappingDetailBo bo)
        {
            RetroBenefitCodeMappingDetail entity = FormEntity(bo);

            Result result = Result();
            if (result.Valid)
            {
                result.DataTrail = entity.Create();
                bo.Id = entity.Id;
            }

            return result;
        }

        public static Result Create(ref RetroBenefitCodeMappingDetailBo bo, ref TrailObject trail)
        {
            Result result = Create(ref bo);
            if (result.Valid)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static Result Update(ref RetroBenefitCodeMappingDetailBo bo)
        {
            Result result = Result();

            RetroBenefitCodeMappingDetail entity = RetroBenefitCodeMappingDetail.Find(bo.Id);
            if (entity == null)
            {
                throw new Exception(MessageBag.NoRecordFound);
            }

            if (result.Valid)
            {
                entity.Id = bo.Id;
                entity.RetroBenefitCodeMappingId = bo.RetroBenefitCodeMappingId;
                entity.RetroBenefitCodeId = bo.RetroBenefitCodeId;
                entity.IsComputePremium = bo.IsComputePremium;
                entity.UpdatedAt = DateTime.Now;
                entity.UpdatedById = bo.UpdatedById ?? entity.UpdatedById;
                result.DataTrail = entity.Update();
            }
            return result;
        }

        public static Result Update(ref RetroBenefitCodeMappingDetailBo bo, ref TrailObject trail)
        {
            Result result = Update(ref bo);
            if (result.Valid && result.DataTrail != null)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static void Delete(RetroBenefitCodeMappingDetailBo bo)
        {
            RetroBenefitCodeMappingDetail.Delete(bo.Id);
        }

        public static Result Delete(RetroBenefitCodeMappingDetailBo bo, ref TrailObject trail)
        {
            Result result = Result();

            DataTrail dataTrail = RetroBenefitCodeMappingDetail.Delete(bo.Id);
            dataTrail.Merge(ref trail, result.Table);

            return result;
        }

        public static Result DeleteByRetroBenefitCodeMappingIdExcept(int retroBenefitCodeMappingId, List<int> saveIds, ref TrailObject trail)
        {
            Result result = Result();
            IList<RetroBenefitCodeMappingDetail> retroBenefitCodeMappingDetails = RetroBenefitCodeMappingDetail.GetByRetroBenefitCodeMappingIdExcept(retroBenefitCodeMappingId, saveIds);
            foreach (RetroBenefitCodeMappingDetail retroBenefitCodeMappingDetail in retroBenefitCodeMappingDetails)
            {
                DataTrail dataTrail = RetroBenefitCodeMappingDetail.Delete(retroBenefitCodeMappingDetail.Id);
                dataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static IList<DataTrail> DeleteByRetroBenefitCodeMappingId(int retroBenefitCodeMappingId)
        {
            return RetroBenefitCodeMappingDetail.DeleteByRetroBenefitCodeMappingId(retroBenefitCodeMappingId);
        }

        public static void DeleteByRetroBenefitCodeMappingId(int retroBenefitCodeMappingId, ref TrailObject trail)
        {
            IList<DataTrail> dataTrails = DeleteByRetroBenefitCodeMappingId(retroBenefitCodeMappingId);
            if (dataTrails.Count > 0)
            {
                foreach (DataTrail dataTrail in dataTrails)
                {
                    dataTrail.Merge(ref trail, UtilAttribute.GetTableName(typeof(RetroBenefitCodeMappingDetail)));
                }
            }
        }
    }
}
