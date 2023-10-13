using BusinessObject;
using BusinessObject.TreatyPricing;
using DataAccess.Entities.TreatyPricing;
using DataAccess.EntityFramework;
using Newtonsoft.Json;
using Services.Identity;
using Shared;
using Shared.DataAccess;
using Shared.Trails;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Services.TreatyPricing
{
    public class TreatyPricingGroupReferralFileService
    {
        public static Result Result()
        {
            return new Result
            {
                Table = UtilAttribute.GetTableName(typeof(TreatyPricingGroupReferralFile)),
                Controller = ModuleBo.ModuleController.TreatyPricingGroupReferralFile.ToString(),
            };
        }

        public static Expression<Func<TreatyPricingGroupReferralFile, TreatyPricingGroupReferralFileBo>> Expression()
        {
            return entity => new TreatyPricingGroupReferralFileBo
            {
                Id = entity.Id,
                FileName = entity.FileName,
                HashFileName = entity.HashFileName,
                Status = entity.Status,
                Errors = entity.Errors,

                CreatedById = entity.CreatedById,
                CreatedBy = entity.CreatedBy.FullName,
                CreatedAt = entity.CreatedAt,

                UpdatedById = entity.UpdatedById,
            };
        }

        public static TreatyPricingGroupReferralFileBo FormBo(TreatyPricingGroupReferralFile entity = null)
        {
            if (entity == null)
                return null;
            return new TreatyPricingGroupReferralFileBo
            {
                Id = entity.Id,
                TreatyPricingGroupReferralId = entity.TreatyPricingGroupReferralId,
                //TreatyPricingGroupReferralBo = TreatyPricingGroupReferralService.Find(entity.TreatyPricingGroupReferralId),
                TableTypePickListDetailId = entity.TableTypePickListDetailId,
                TableTypePickListDetailBo = PickListDetailService.Find(entity.TableTypePickListDetailId),
                UploadedType = entity.UploadedType,
                FileName = entity.FileName,
                HashFileName = entity.HashFileName,
                Status = entity.Status,
                Errors = entity.Errors,
                CreatedAt = entity.CreatedAt,
                CreatedAtStr = entity.CreatedAt.ToString(Util.GetDateTimeFormat()),
                CreatedById = entity.CreatedById,
                CreatedByBo = UserService.Find(entity.CreatedById),
                UpdatedById = entity.UpdatedById,

                FormattedErrors = !string.IsNullOrEmpty(entity.Errors) ? string.Join("\n", JsonConvert.DeserializeObject<List<string>>(entity.Errors).ToArray()) : "",
                StatusName = TreatyPricingGroupReferralFileBo.GetStatusName(entity.Status),
            };
        }

        public static IList<TreatyPricingGroupReferralFileBo> FormBos(IList<TreatyPricingGroupReferralFile> entities = null)
        {
            if (entities == null)
                return null;
            IList<TreatyPricingGroupReferralFileBo> bos = new List<TreatyPricingGroupReferralFileBo>() { };
            foreach (TreatyPricingGroupReferralFile entity in entities)
            {
                bos.Add(FormBo(entity));
            }
            return bos;
        }

        public static TreatyPricingGroupReferralFile FormEntity(TreatyPricingGroupReferralFileBo bo = null)
        {
            if (bo == null)
                return null;
            return new TreatyPricingGroupReferralFile
            {
                Id = bo.Id,
                TreatyPricingGroupReferralId = bo.TreatyPricingGroupReferralId,
                TableTypePickListDetailId = bo.TableTypePickListDetailId,
                UploadedType = bo.UploadedType,
                FileName = bo.FileName,
                HashFileName = bo.HashFileName,
                Status = bo.Status,
                Errors = bo.Errors,
                CreatedById = bo.CreatedById,
                UpdatedById = bo.UpdatedById,
            };
        }

        public static bool IsExists(int id)
        {
            return TreatyPricingGroupReferralFile.IsExists(id);
        }

        public static TreatyPricingGroupReferralFileBo Find(int? id)
        {
            return FormBo(TreatyPricingGroupReferralFile.Find(id));
        }

        public static TreatyPricingGroupReferralFileBo FindByStatus(int status)
        {
            using (var db = new AppDbContext())
            {
                return FormBo(db.TreatyPricingGroupReferralFiles.Where(q => q.Status == status).FirstOrDefault());
            }
        }

        public static IList<TreatyPricingGroupReferralFileBo> Get()
        {
            using (var db = new AppDbContext())
            {
                return FormBos(db.TreatyPricingGroupReferralFiles.OrderBy(q => q.CreatedAt).ToList());
            }
        }

        public static IList<TreatyPricingGroupReferralFileBo> GetByUploadedTypeTreatyPricingGroupReferralId(int type, int? treatyPricingGroupReferralId = null)
        {
            using (var db = new AppDbContext())
            {
                var query = db.TreatyPricingGroupReferralFiles.Where(q => q.UploadedType == type);

                if (treatyPricingGroupReferralId.HasValue)
                    query = query.Where(q => q.TreatyPricingGroupReferralId == treatyPricingGroupReferralId);

                return FormBos(query.OrderBy(q => q.CreatedAt).ToList());
            }
        }

        public static int CountByStatus(int status)
        {
            using (var db = new AppDbContext())
            {
                return db.TreatyPricingGroupReferralFiles.Where(q => q.Status == status).Count();
            }
        }

        public static Result Save(ref TreatyPricingGroupReferralFileBo bo)
        {
            if (!TreatyPricingGroupReferralFile.IsExists(bo.Id))
            {
                return Create(ref bo);
            }
            return Update(ref bo);
        }

        public static Result Save(ref TreatyPricingGroupReferralFileBo bo, ref TrailObject trail)
        {
            if (!TreatyPricingGroupReferralFile.IsExists(bo.Id))
            {
                return Create(ref bo, ref trail);
            }
            return Update(ref bo, ref trail);
        }

        public static Result Create(ref TreatyPricingGroupReferralFileBo bo)
        {
            TreatyPricingGroupReferralFile entity = FormEntity(bo);

            Result result = Result();

            if (result.Valid)
            {
                result.DataTrail = entity.Create();
                bo.Id = entity.Id;
            }

            return result;
        }

        public static Result Create(ref TreatyPricingGroupReferralFileBo bo, ref TrailObject trail)
        {
            Result result = Create(ref bo);
            if (result.Valid)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static Result Update(ref TreatyPricingGroupReferralFileBo bo)
        {
            Result result = Result();

            TreatyPricingGroupReferralFile entity = TreatyPricingGroupReferralFile.Find(bo.Id);
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

        public static Result Update(ref TreatyPricingGroupReferralFileBo bo, ref TrailObject trail)
        {
            Result result = Update(ref bo);
            if (result.Valid && result.DataTrail != null)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static void Delete(TreatyPricingGroupReferralFileBo bo)
        {
            TreatyPricingGroupReferralFile.Delete(bo.Id);
        }
    }
}
