using BusinessObject;
using DataAccess.Entities;
using DataAccess.EntityFramework;
using Shared;
using Shared.DataAccess;
using Shared.Trails;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;

namespace Services
{
    public class RateService
    {
        public static Result Result()
        {
            return new Result
            {
                Table = UtilAttribute.GetTableName(typeof(Rate)),
                Controller = ModuleBo.ModuleController.RateTable.ToString()
            };
        }

        public static Expression<Func<Rate, RateBo>> Expression()
        {
            return entity => new RateBo
            {
                Id = entity.Id,
                Code = entity.Code,
                ValuationRate = entity.ValuationRate,
                RatePerBasis = entity.RatePerBasis,

                CreatedById = entity.CreatedById,
                UpdatedById = entity.UpdatedById,
            };
        }

        public static RateBo FormBo(Rate entity = null)
        {
            if (entity == null)
                return null;
            return new RateBo
            {
                Id = entity.Id,
                Code = entity.Code,
                ValuationRate = entity.ValuationRate,
                RatePerBasis = entity.RatePerBasis,

                CreatedById = entity.CreatedById,
                CreatedAt = entity.CreatedAt,
                UpdatedById = entity.UpdatedById,
            };
        }

        public static IList<RateBo> FormBos(IList<Rate> entities = null)
        {
            if (entities == null)
                return null;
            IList<RateBo> bos = new List<RateBo>() { };
            foreach (Rate entity in entities)
            {
                bos.Add(FormBo(entity));
            }
            return bos;
        }

        public static Rate FormEntity(RateBo bo = null)
        {
            if (bo == null)
                return null;
            return new Rate
            {
                Id = bo.Id,
                Code = bo.Code?.Trim(),
                ValuationRate = bo.ValuationRate,
                RatePerBasis = bo.RatePerBasis,
                CreatedById = bo.CreatedById,
                UpdatedById = bo.UpdatedById,
            };
        }

        public static bool IsExists(int id)
        {
            return Rate.IsExists(id);
        }

        public static RateBo Find(int id)
        {
            return FormBo(Rate.Find(id));
        }

        public static RateBo Find(int? id)
        {
            return FormBo(Rate.Find(id));
        }

        public static RateBo FindByCode(string code)
        {
            return FormBo(Rate.FindByCode(code));
        }

        public static int CountByCode(string code)
        {
            return Rate.CountByCode(code);
        }

        public static IList<RateBo> Get()
        {
            return FormBos(Rate.Get());
        }

        public static IEnumerable<string> GetRateTableCodes()
        {
            using (var db = new AppDbContext())
            {
                return db.Rates.Select(q => q.Code).ToList();
            }
        }

        public static Result Save(ref RateBo bo)
        {
            if (!Rate.IsExists(bo.Id))
            {
                return Create(ref bo);
            }
            return Update(ref bo);
        }

        public Result Save(ref RateBo bo, ref TrailObject trail)
        {
            if (!Rate.IsExists(bo.Id))
            {
                return Create(ref bo, ref trail);
            }
            return Update(ref bo, ref trail);
        }

        public static bool IsDuplicateCode(Rate rate)
        {
            return rate.IsDuplicateCode();
        }

        public static Result Create(ref RateBo bo)
        {
            Rate entity = FormEntity(bo);

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

        public static Result Create(ref RateBo bo, ref TrailObject trail)
        {
            Result result = Create(ref bo);
            if (result.Valid)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static Result Update(ref RateBo bo)
        {
            Result result = Result();

            Rate entity = Rate.Find(bo.Id);
            if (entity == null)
            {
                throw new Exception(MessageBag.NoRecordFound);
            }
            if (IsDuplicateCode(FormEntity(bo)))
            {
                result.AddTakenError("Code", bo.Code);
            }

            if (result.Valid)
            {
                entity.Id = bo.Id;
                entity.Code = bo.Code;
                entity.ValuationRate = bo.ValuationRate;
                entity.RatePerBasis = bo.RatePerBasis;
                entity.UpdatedAt = DateTime.Now;
                entity.UpdatedById = bo.UpdatedById ?? entity.UpdatedById;
                result.DataTrail = entity.Update();
            }
            return result;
        }

        public static Result Update(ref RateBo bo, ref TrailObject trail)
        {
            Result result = Update(ref bo);
            if (result.Valid && result.DataTrail != null)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static void Delete(RateBo bo)
        {
            RateDetailService.DeleteByRateId(bo.Id);
            Rate.Delete(bo.Id);
        }

        public static Result Delete(RateBo bo, ref TrailObject trail)
        {
            Result result = Result();

            if (
                RateTableService.CountByRateId(bo.Id) > 0
            )
            {
                result.AddErrorRecordInUsed();
            }

            if (result.Valid)
            {
                RateDetailService.DeleteByRateId(bo.Id, ref trail);

                var files = RateDetailUploadService.GetByRateId(bo.Id);
                foreach (RateDetailUploadBo file in files)
                {
                    if (File.Exists(file.GetLocalPath()))
                        File.Delete(file.GetLocalPath());
                }
                RateDetailUploadService.DeleteByRateId(bo.Id);

                DataTrail dataTrail = Rate.Delete(bo.Id);
                dataTrail.Merge(ref trail, result.Table);
            }

            return result;
        }
    }
}
