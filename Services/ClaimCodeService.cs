using BusinessObject;
using DataAccess.Entities;
using DataAccess.EntityFramework;
using Shared;
using Shared.DataAccess;
using Shared.Trails;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Services
{
    public class ClaimCodeService
    {
        public static Result Result()
        {
            return new Result
            {
                Table = UtilAttribute.GetTableName(typeof(ClaimCode)),
                Controller = ModuleBo.ModuleController.ClaimCode.ToString()
            };
        }

        public static ClaimCodeBo FormBo(ClaimCode entity = null)
        {
            if (entity == null)
                return null;
            return new ClaimCodeBo
            {
                Id = entity.Id,
                Code = entity.Code,
                Description = entity.Description,
                Status = entity.Status,
                CreatedById = entity.CreatedById,
                UpdatedById = entity.UpdatedById,
            };
        }

        public static IList<ClaimCodeBo> FormBos(IList<ClaimCode> entities = null)
        {
            if (entities == null)
                return null;
            IList<ClaimCodeBo> bos = new List<ClaimCodeBo>() { };
            foreach (ClaimCode entity in entities)
            {
                bos.Add(FormBo(entity));
            }
            return bos;
        }

        public static ClaimCode FormEntity(ClaimCodeBo bo = null)
        {
            if (bo == null)
                return null;
            return new ClaimCode
            {
                Id = bo.Id,
                Code = bo.Code?.Trim(),
                Description = bo.Description?.Trim(),
                Status = bo.Status,
                CreatedById = bo.CreatedById,
                UpdatedById = bo.UpdatedById,
            };
        }

        public static bool IsDuplicateCode(ClaimCode ClaimCode)
        {
            using (var db = new AppDbContext())
            {
                if (!string.IsNullOrEmpty(ClaimCode.Code?.Trim()))
                {
                    var query = db.ClaimCodes.Where(q => q.Code.Trim().Equals(ClaimCode.Code.Trim(), StringComparison.OrdinalIgnoreCase));
                    if (ClaimCode.Id != 0)
                    {
                        query = query.Where(q => q.Id != ClaimCode.Id);
                    }
                    return query.Count() > 0;
                }
                return false;
            }
        }

        public static bool IsExists(int id)
        {
            return ClaimCode.IsExists(id);
        }

        public static ClaimCodeBo Find(int? id)
        {
            return FormBo(ClaimCode.Find(id));
        }

        public static ClaimCodeBo FindByCode(string code)
        {
            return FormBo(ClaimCode.FindByCode(code));
        }

        public static int Count()
        {
            return ClaimCode.Count();
        }

        public static IList<ClaimCodeBo> Get()
        {
            return FormBos(ClaimCode.Get());
        }

        public static IList<ClaimCodeBo> GetByStatus(int? status = null, int? selectedId = null, string selectedCode = null)
        {
            using (var db = new AppDbContext())
            {
                var query = db.ClaimCodes.AsQueryable();

                if (status != null)
                {
                    if (selectedId != null)
                        query = query.Where(q => q.Status == status || q.Id == selectedId);
                    else if (!string.IsNullOrEmpty(selectedCode))
                        query = query.Where(q => q.Status == status || q.Code.Trim() == selectedCode.Trim());
                    else
                        query = query.Where(q => q.Status == status);
                }

                return FormBos(query.OrderBy(q => q.Code).ToList());
            }
        }

        public static int CountByCodeStatus(string code, int? status = null)
        {
            using (var db = new AppDbContext())
            {
                var query = db.ClaimCodes.Where(q => q.Code.Trim() == code.Trim());
                if (status != null)
                    query = query.Where(q => q.Status == status);
                return query.Count();
            }
        }

        public static Result Save(ref ClaimCodeBo bo)
        {
            if (!ClaimCode.IsExists(bo.Id))
            {
                return Create(ref bo);
            }
            return Update(ref bo);
        }

        public Result Save(ref ClaimCodeBo bo, ref TrailObject trail)
        {
            if (!ClaimCode.IsExists(bo.Id))
            {
                return Create(ref bo, ref trail);
            }
            return Update(ref bo, ref trail);
        }

        public static Result Create(ref ClaimCodeBo bo)
        {
            ClaimCode entity = FormEntity(bo);

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

        public static Result Create(ref ClaimCodeBo bo, ref TrailObject trail)
        {
            Result result = Create(ref bo);
            if (result.Valid)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static Result Update(ref ClaimCodeBo bo)
        {
            Result result = Result();

            ClaimCode entity = ClaimCode.Find(bo.Id);
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
                entity.Code = bo.Code;
                entity.Description = bo.Description;
                entity.Status = bo.Status;
                entity.UpdatedAt = DateTime.Now;
                entity.UpdatedById = bo.UpdatedById ?? entity.UpdatedById;
                result.DataTrail = entity.Update();
            }
            return result;
        }

        public static Result Update(ref ClaimCodeBo bo, ref TrailObject trail)
        {
            Result result = Update(ref bo);
            if (result.Valid && result.DataTrail != null)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static void Delete(ClaimCodeBo bo)
        {
            ClaimCode.Delete(bo.Id);
        }

        public static Result Delete(ClaimCodeBo bo, ref TrailObject trail)
        {
            Result result = Result();

            if (
                BenefitDetail.CountByClaimCode(bo.Id) > 0 ||
                AccountCodeMappingDetailService.CountByClaimCodeId(bo.Id) > 0 ||
                ClaimCodeMapping.CountByClaimCodeId(bo.Id) > 0 ||
                ClaimChecklist.CountByClaimCodeId(bo.Id) > 0 ||
                ClaimAuthorityLimitCedantDetail.CountByClaimCodeId(bo.Id) > 0 ||
                ClaimAuthorityLimitMLReDetail.CountByClaimCodeId(bo.Id) > 0
            )
            {
                result.AddErrorRecordInUsed();
            }

            if (result.Valid)
            {
                DataTrail dataTrail = ClaimCode.Delete(bo.Id);
                dataTrail.Merge(ref trail, result.Table);
            }

            return result;
        }
    }
}
