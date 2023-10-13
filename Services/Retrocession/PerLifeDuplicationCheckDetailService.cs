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
    public class PerLifeDuplicationCheckDetailService
    {
        public static Result Result()
        {
            return new Result
            {
                Table = UtilAttribute.GetTableName(typeof(PerLifeDuplicationCheckDetail)),
                Controller = ModuleBo.ModuleController.PerLifeDuplicationCheckDetail.ToString()
            };
        }

        public static Expression<Func<PerLifeDuplicationCheckDetail, PerLifeDuplicationCheckDetailBo>> Expression()
        {
            return entity => new PerLifeDuplicationCheckDetailBo
            {
                Id = entity.Id,
                PerLifeDuplicationCheckId = entity.PerLifeDuplicationCheckId,
                TreatyCode = entity.TreatyCode,

                CreatedById = entity.CreatedById,
            };
        }

        public static PerLifeDuplicationCheckDetailBo FormBo(PerLifeDuplicationCheckDetail entity = null)
        {
            if (entity == null)
                return null;
            return new PerLifeDuplicationCheckDetailBo
            {
                Id = entity.Id,
                PerLifeDuplicationCheckId = entity.PerLifeDuplicationCheckId,
                TreatyCode = entity.TreatyCode,

                CreatedById = entity.CreatedById,
                CreatedAt = entity.CreatedAt,
            };
        }

        public static IList<PerLifeDuplicationCheckDetailBo> FormBos(IList<PerLifeDuplicationCheckDetail> entities = null)
        {
            if (entities == null)
                return null;
            IList<PerLifeDuplicationCheckDetailBo> bos = new List<PerLifeDuplicationCheckDetailBo>() { };
            foreach (PerLifeDuplicationCheckDetail entity in entities)
            {
                bos.Add(FormBo(entity));
            }
            return bos;
        }

        public static PerLifeDuplicationCheckDetail FormEntity(PerLifeDuplicationCheckDetailBo bo = null)
        {
            if (bo == null)
                return null;
            return new PerLifeDuplicationCheckDetail
            {
                Id = bo.Id,
                PerLifeDuplicationCheckId = bo.PerLifeDuplicationCheckId,
                TreatyCode = bo.TreatyCode,

                CreatedById = bo.CreatedById,
            };
        }

        public static bool IsExists(int id)
        {
            return PerLifeDuplicationCheckDetail.IsExists(id);
        }

        public static PerLifeDuplicationCheckDetailBo Find(int id)
        {
            return FormBo(PerLifeDuplicationCheckDetail.Find(id));
        }

        public static IList<PerLifeDuplicationCheckDetailBo> Get()
        {
            using (var db = new AppDbContext())
            {
                return FormBos(db.PerLifeDuplicationCheckDetails.OrderBy(q => q.PerLifeDuplicationCheckId).ToList());
            }
        }

        public static IList<PerLifeDuplicationCheckDetailBo> GetByPerLifeDuplicationCheckId(int id)
        {
            using (var db = new AppDbContext())
            {
                return FormBos(db.PerLifeDuplicationCheckDetails
                    .Where(q => q.PerLifeDuplicationCheckId == id)
                    .OrderBy(q => q.PerLifeDuplicationCheckId).ToList());
            }
        }

        public static Result Save(ref PerLifeDuplicationCheckDetailBo bo)
        {
            if (!PerLifeDuplicationCheckDetail.IsExists(bo.Id))
            {
                return Create(ref bo);
            }
            return Update(ref bo);
        }

        public Result Save(ref PerLifeDuplicationCheckDetailBo bo, ref TrailObject trail)
        {
            if (!PerLifeDuplicationCheckDetail.IsExists(bo.Id))
            {
                return Create(ref bo, ref trail);
            }
            return Update(ref bo, ref trail);
        }

        public static Result Create(ref PerLifeDuplicationCheckDetailBo bo)
        {
            PerLifeDuplicationCheckDetail entity = FormEntity(bo);

            Result result = Result();

            if (result.Valid)
            {
                result.DataTrail = entity.Create();
                bo.Id = entity.Id;
            }

            return result;
        }

        public static Result Create(ref PerLifeDuplicationCheckDetailBo bo, ref TrailObject trail)
        {
            Result result = Create(ref bo);
            if (result.Valid)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static Result Update(ref PerLifeDuplicationCheckDetailBo bo)
        {
            Result result = Result();

            PerLifeDuplicationCheckDetail entity = PerLifeDuplicationCheckDetail.Find(bo.Id);
            if (entity == null)
            {
                throw new Exception(MessageBag.NoRecordFound);
            }

            if (result.Valid)
            {
                entity.Id = bo.Id;
                entity.PerLifeDuplicationCheckId = bo.PerLifeDuplicationCheckId;
                entity.TreatyCode = bo.TreatyCode;
                result.DataTrail = entity.Update();
            }
            return result;
        }

        public static Result Update(ref PerLifeDuplicationCheckDetailBo bo, ref TrailObject trail)
        {
            Result result = Update(ref bo);
            if (result.Valid && result.DataTrail != null)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static void Delete(PerLifeDuplicationCheckDetailBo bo)
        {
            PerLifeDuplicationCheckDetail.Delete(bo.Id);
        }

        public static Result Delete(PerLifeDuplicationCheckDetailBo bo, ref TrailObject trail)
        {
            Result result = Result();

            // TODO: In Use Validation

            if (result.Valid)
            {
                DataTrail dataTrail = PerLifeDuplicationCheckDetail.Delete(bo.Id);
                dataTrail.Merge(ref trail, result.Table);
            }

            return result;
        }

        public static IList<DataTrail> DeleteByPerLifeDuplicationCheckId(int perLifeDuplicationCheckId)
        {
            return PerLifeDuplicationCheckDetail.DeleteByPerLifeDuplicationCheckId(perLifeDuplicationCheckId);
        }

        public static int CountByTreatyCode(string treatyCode, PerLifeDuplicationCheckBo perLifeDuplicationCheckBo)
        {
            return CountByTreatyCode(
                treatyCode,
                perLifeDuplicationCheckBo.ReinsuranceEffectiveStartDate,
                perLifeDuplicationCheckBo.ReinsuranceEffectiveEndDate,
                perLifeDuplicationCheckBo.Id
            );
        }

        public static int CountByTreatyCode(
            string treatyCode,
            DateTime? startDate,
            DateTime? endDate,
            int? duplicationCheckId = null
        )
        {
            return PerLifeDuplicationCheckDetail.CountByTreatyCode(
                treatyCode,
                startDate,
                endDate,
                duplicationCheckId
            );
        }
    }
}
