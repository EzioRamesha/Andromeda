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
    public class RetroBenefitCodeService
    {
        public static Result Result()
        {
            return new Result
            {
                Table = UtilAttribute.GetTableName(typeof(RetroBenefitCode)),
                Controller = ModuleBo.ModuleController.RetroBenefitCode.ToString()
            };
        }

        public static Expression<Func<RetroBenefitCode, RetroBenefitCodeBo>> Expression()
        {
            return entity => new RetroBenefitCodeBo
            {
                Id = entity.Id,
                Code = entity.Code,
                Description = entity.Description,
                EffectiveDate = entity.EffectiveDate,
                CeaseDate = entity.CeaseDate,
                Status = entity.Status,
                Remarks = entity.Remarks,

                CreatedById = entity.CreatedById,
                UpdatedById = entity.UpdatedById,
            };
        }

        public static RetroBenefitCodeBo FormBo(RetroBenefitCode entity = null)
        {
            if (entity == null)
                return null;
            return new RetroBenefitCodeBo
            {
                Id = entity.Id,
                Code = entity.Code,
                Description = entity.Description,
                EffectiveDate = entity.EffectiveDate,
                CeaseDate = entity.CeaseDate,
                Status = entity.Status,
                Remarks = entity.Remarks,

                CreatedById = entity.CreatedById,
                CreatedAt = entity.CreatedAt,
                UpdatedById = entity.UpdatedById,
            };
        }

        public static IList<RetroBenefitCodeBo> FormBos(IList<RetroBenefitCode> entities = null)
        {
            if (entities == null)
                return null;
            IList<RetroBenefitCodeBo> bos = new List<RetroBenefitCodeBo>() { };
            foreach (RetroBenefitCode entity in entities)
            {
                bos.Add(FormBo(entity));
            }
            return bos;
        }

        public static RetroBenefitCode FormEntity(RetroBenefitCodeBo bo = null)
        {
            if (bo == null)
                return null;
            return new RetroBenefitCode
            {
                Id = bo.Id,
                Code = bo.Code,
                Description = bo.Description,
                EffectiveDate = bo.EffectiveDate,
                CeaseDate = bo.CeaseDate,
                Status = bo.Status,
                Remarks = bo.Remarks,

                CreatedById = bo.CreatedById,
                UpdatedById = bo.UpdatedById,
            };
        }

        public static bool IsExists(int id)
        {
            return RetroBenefitCode.IsExists(id);
        }

        public static RetroBenefitCodeBo Find(int? id)
        {
            return FormBo(RetroBenefitCode.Find(id));
        }

        public static IList<RetroBenefitCodeBo> Get()
        {
            using (var db = new AppDbContext())
            {
                return FormBos(db.RetroBenefitCodes.OrderBy(q => q.Code).ToList());
            }
        }

        public static IList<RetroBenefitCodeBo> GetByStatus(int? status = null, int? selectedId = null, string selectedCode = null)
        {
            using (var db = new AppDbContext())
            {
                var query = db.RetroBenefitCodes.AsQueryable();

                if (status != null)
                {
                    if (selectedId != null)
                        query = query.Where(q => q.Status == status || q.Id == selectedId);
                    else if (!string.IsNullOrEmpty(selectedCode))
                        query = query.Where(q => q.Status == status || q.Code == selectedCode);
                    else
                        query = query.Where(q => q.Status == status);
                }

                return FormBos(query.OrderBy(q => q.Code).ToList());
            }
        }

        public static IList<RetroBenefitCodeBo> GetByStatusByIds(int? status = null, List<int> selectedIds = null)
        {
            using (var db = new AppDbContext())
            {
                var query = db.RetroBenefitCodes.AsQueryable();

                if (status != null)
                {
                    if (selectedIds != null)
                        query = query.Where(q => q.Status == status || selectedIds.Contains(q.Id));
                    else
                        query = query.Where(q => q.Status == status);
                }

                return FormBos(query.OrderBy(q => q.Code).ToList());
            }
        }

        public static Result Save(ref RetroBenefitCodeBo bo)
        {
            if (!RetroBenefitCode.IsExists(bo.Id))
            {
                return Create(ref bo);
            }
            return Update(ref bo);
        }

        public Result Save(ref RetroBenefitCodeBo bo, ref TrailObject trail)
        {
            if (!RetroBenefitCode.IsExists(bo.Id))
            {
                return Create(ref bo, ref trail);
            }
            return Update(ref bo, ref trail);
        }

        public static bool IsDuplicateCode(RetroBenefitCode retroBenefitCode)
        {
            return retroBenefitCode.IsDuplicateCode();
        }

        public static Result Create(ref RetroBenefitCodeBo bo)
        {
            RetroBenefitCode entity = FormEntity(bo);

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

        public static Result Create(ref RetroBenefitCodeBo bo, ref TrailObject trail)
        {
            Result result = Create(ref bo);
            if (result.Valid)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static Result Update(ref RetroBenefitCodeBo bo)
        {
            Result result = Result();

            RetroBenefitCode entity = RetroBenefitCode.Find(bo.Id);
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
                entity.Description = bo.Description;
                entity.EffectiveDate = bo.EffectiveDate;
                entity.CeaseDate = bo.CeaseDate;
                entity.Status = bo.Status;
                entity.Remarks = bo.Remarks;

                entity.UpdatedAt = DateTime.Now;
                entity.UpdatedById = bo.UpdatedById ?? entity.UpdatedById;
                result.DataTrail = entity.Update();
            }
            return result;
        }

        public static Result Update(ref RetroBenefitCodeBo bo, ref TrailObject trail)
        {
            Result result = Update(ref bo);
            if (result.Valid && result.DataTrail != null)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static void Delete(RetroBenefitCodeBo bo)
        {
            RetroBenefitCode.Delete(bo.Id);
        }

        public static Result Delete(RetroBenefitCodeBo bo, ref TrailObject trail)
        {
            Result result = Result();

            if (
                RetroBenefitRetentionLimitService.CountByRetroBenefitCodeId(bo.Id) > 0 ||
                RetroBenefitCodeMappingDetailService.CountByRetroBenefitCodeId(bo.Id) > 0
            )
            {
                result.AddErrorRecordInUsed();
            }

            if (result.Valid)
            {
                DataTrail dataTrail = RetroBenefitCode.Delete(bo.Id);
                dataTrail.Merge(ref trail, result.Table);
            }

            return result;
        }
    }
}
