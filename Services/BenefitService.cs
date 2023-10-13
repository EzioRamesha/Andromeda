using BusinessObject;
using DataAccess.Entities;
using DataAccess.EntityFramework;
using Services.Retrocession;
using Shared;
using Shared.DataAccess;
using Shared.Trails;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Services
{
    public class BenefitService
    {
        public static Result Result()
        {
            return new Result
            {
                Table = UtilAttribute.GetTableName(typeof(Benefit)),
                Controller = ModuleBo.ModuleController.Benefit.ToString()
            };
        }

        public static Expression<Func<Benefit, BenefitBo>> Expression()
        {
            return entity => new BenefitBo
            {
                Id = entity.Id,
                Type = entity.Type,
                Code = entity.Code,
                Description = entity.Description,
                Status = entity.Status,
                ValuationBenefitCodePickListDetailId = entity.ValuationBenefitCodePickListDetailId,
                BenefitCategoryPickListDetailId = entity.BenefitCategoryPickListDetailId,
                GST = entity.GST,
                EffectiveStartDate = entity.EffectiveStartDate,
                EffectiveEndDate = entity.EffectiveEndDate,
                CreatedById = entity.CreatedById,
                UpdatedById = entity.UpdatedById,
            };
        }

        public static BenefitBo FormBo(Benefit entity = null, bool foreign = true)
        {
            if (entity == null)
                return null;
            return new BenefitBo
            {
                Id = entity.Id,
                Type = entity.Type,
                Code = entity.Code,
                Description = entity.Description,
                Status = entity.Status,
                ValuationBenefitCodePickListDetailId = entity.ValuationBenefitCodePickListDetailId,
                ValuationBenefitCodePickListDetailBo = foreign ? PickListDetailService.Find(entity.ValuationBenefitCodePickListDetailId) : null,
                BenefitCategoryPickListDetailId = entity.BenefitCategoryPickListDetailId,
                BenefitCategoryPickListDetailBo = foreign ? PickListDetailService.Find(entity.BenefitCategoryPickListDetailId) : null,
                GST = entity.GST,
                EffectiveStartDate = entity.EffectiveStartDate,
                EffectiveEndDate = entity.EffectiveEndDate,
                CreatedById = entity.CreatedById,
                UpdatedById = entity.UpdatedById,
                Value = entity.Id,
                Text = entity.Code + " - " + entity.Description
            };
        }

        public static BenefitBo FormBoForDropDownProductBenefit(Benefit entity = null)
        {
            if (entity == null)
                return null;
            return new BenefitBo
            {
                Id = entity.Id,
                Code = entity.Code,
                Description = entity.Description,
                Text = entity.Code + " - " + entity.Description
            };
        }

        public static BenefitBo FormBoForDropDownBenefit(Benefit entity = null)
        {
            if (entity == null)
                return null;
            return new BenefitBo
            {
                Id = entity.Id,
                Code = entity.Code,
                Description = entity.Description,
                Text = entity.Code + " - " + entity.Description
            };
        }

        public static IList<BenefitBo> FormBos(IList<Benefit> entities = null, bool foreign = true)
        {
            if (entities == null)
                return null;
            IList<BenefitBo> bos = new List<BenefitBo>() { };
            foreach (Benefit entity in entities)
            {
                bos.Add(FormBo(entity, foreign));
            }
            return bos;
        }

        public static IList<BenefitBo> FormBosForDropDownBenefit(IList<Benefit> entities = null)
        {
            if (entities == null)
                return null;
            IList<BenefitBo> bos = new List<BenefitBo>() { };
            foreach (Benefit entity in entities)
            {
                bos.Add(FormBoForDropDownBenefit(entity));
            }
            return bos;
        }

        public static Benefit FormEntity(BenefitBo bo = null)
        {
            if (bo == null)
                return null;
            return new Benefit
            {
                Id = bo.Id,
                Type = bo.Type?.Trim(),
                Code = bo.Code?.Trim(),
                Description = bo.Description?.Trim(),
                Status = bo.Status,
                ValuationBenefitCodePickListDetailId = bo.ValuationBenefitCodePickListDetailId,
                BenefitCategoryPickListDetailId = bo.BenefitCategoryPickListDetailId,
                GST = bo.GST,
                EffectiveStartDate = bo.EffectiveStartDate,
                EffectiveEndDate = bo.EffectiveEndDate,
                CreatedById = bo.CreatedById,
                UpdatedById = bo.UpdatedById,
            };
        }

        public static bool IsDuplicateCode(Benefit benefit)
        {
            using (var db = new AppDbContext())
            {
                if (!string.IsNullOrEmpty(benefit.Code?.Trim()))
                {
                    var query = db.Benefits.Where(q => q.Code.Trim().Equals(benefit.Code.Trim(), StringComparison.OrdinalIgnoreCase));
                    if (benefit.Id != 0)
                    {
                        query = query.Where(q => q.Id != benefit.Id);
                    }
                    return query.Count() > 0;
                }
                return false;
            }
        }

        public static bool IsExists(int id)
        {
            return Benefit.IsExists(id);
        }

        public static BenefitBo Find(int id)
        {
            return FormBo(Benefit.Find(id));
        }

        public static BenefitBo FindForDropDownProductBenefit(int id)
        {
            return FormBoForDropDownProductBenefit(Benefit.Find(id));
        }

        public static BenefitBo Find(int? id)
        {
            if (id == null)
                return null;
            return FormBo(Benefit.Find(id.Value));
        }

        public static BenefitBo FindByCode(string code, bool foreign = true)
        {
            return FormBo(Benefit.FindByCode(code), foreign);
        }

        public static int CountByCode(string code)
        {
            return Benefit.CountByCode(code);
        }

        public static int CountByValuationBenefitCodePickListDetailId(int valuationBenefitCodePickListDetailId)
        {
            return Benefit.CountByValuationBenefitCodePickListDetailId(valuationBenefitCodePickListDetailId);
        }

        public static IList<BenefitBo> Get(bool foreign = true)
        {
            return FormBos(Benefit.Get(), foreign);
        }

        public static IList<BenefitBo> GetByStatus(int? status = null, int? selectedId = null, string selectedCode = null)
        {
            using (var db = new AppDbContext())
            {
                var query = db.Benefits.AsQueryable();

                if (status != null)
                {
                    if (selectedId != null)
                        query = query.Where(q => q.Status == status || q.Id == selectedId);
                    else if (!string.IsNullOrEmpty(selectedCode))
                        query = query.Where(q => q.Status == status || q.Code.ToLower() == selectedCode.ToLower());
                    else
                        query = query.Where(q => q.Status == status);
                }

                return FormBos(query.OrderBy(q => q.Code).ToList(), false);
            }
        }

        public static IList<BenefitBo> GetByStatusForDropDownBenefit(int? status = null, int? selectedId = null, string selectedCode = null)
        {
            using (var db = new AppDbContext())
            {
                var query = db.Benefits.AsQueryable();

                if (status != null)
                {
                    if (selectedId != null)
                        query = query.Where(q => q.Status == status || q.Id == selectedId);
                    else if (!string.IsNullOrEmpty(selectedCode))
                        query = query.Where(q => q.Status == status || q.Code.ToLower() == selectedCode.ToLower());
                    else
                        query = query.Where(q => q.Status == status);
                }

                return FormBosForDropDownBenefit(query.OrderBy(q => q.Code).ToList());
            }
        }

        public static IEnumerable<string> GetBenefitCodes()
        {
            using (var db = new AppDbContext())
            {
                return db.Benefits.Select(q => q.Code).ToList();
            }
        }

        public static IList<BenefitBo> GetByBenefitCode(string benefitCode)
        {
            if (benefitCode.Contains(','))
            {
                var benefitCodes = benefitCode.Split(',');
                List<BenefitBo> benefitBos = new List<BenefitBo>();
                foreach (var bc in benefitCodes)
                {
                    foreach (var bo in FormBos(Benefit.Get().Where(q => q.Code.Contains(bc.Trim())).ToList()))
                    {
                        benefitBos.Add(bo);
                    }
                }
                return benefitBos;
            }
            else
            {
                return FormBos(Benefit.Get().Where(q => q.Code.Contains(benefitCode)).ToList());
            }
        }


        public static Result Save(ref BenefitBo bo)
        {
            if (!Benefit.IsExists(bo.Id))
            {
                return Create(ref bo);
            }
            return Update(ref bo);
        }

        public Result Save(ref BenefitBo bo, ref TrailObject trail)
        {
            if (!Benefit.IsExists(bo.Id))
            {
                return Create(ref bo, ref trail);
            }
            return Update(ref bo, ref trail);
        }

        public static Result Create(ref BenefitBo bo)
        {
            Benefit entity = FormEntity(bo);

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

        public static Result Create(ref BenefitBo bo, ref TrailObject trail)
        {
            Result result = Create(ref bo);
            if (result.Valid)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static Result Update(ref BenefitBo bo)
        {
            Result result = Result();

            Benefit entity = Benefit.Find(bo.Id);
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
                entity.Type = bo.Type;
                entity.Description = bo.Description;
                entity.Status = bo.Status;
                entity.ValuationBenefitCodePickListDetailId = bo.ValuationBenefitCodePickListDetailId;
                entity.BenefitCategoryPickListDetailId = bo.BenefitCategoryPickListDetailId;
                entity.GST = bo.GST;
                entity.EffectiveStartDate = bo.EffectiveStartDate;
                entity.EffectiveEndDate = bo.EffectiveEndDate;
                entity.UpdatedAt = DateTime.Now;
                entity.UpdatedById = bo.UpdatedById ?? entity.UpdatedById;
                result.DataTrail = entity.Update();
            }
            return result;
        }

        public static Result Update(ref BenefitBo bo, ref TrailObject trail)
        {
            Result result = Update(ref bo);
            if (result.Valid && result.DataTrail != null)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static void Delete(BenefitBo bo)
        {
            BenefitDetail.DeleteAllByBenefitId(bo.Id);
            Benefit.Delete(bo.Id);
        }

        public static Result Delete(BenefitBo bo, ref TrailObject trail)
        {
            Result result = Result();

            if (
                Mfrs17CellMapping.CountByBenefitCode(bo.Code) > 0 ||
                RateTable.CountByBenefitId(bo.Id) > 0 ||
                TreatyBenefitCodeMapping.CountByBenefitId(bo.Id) > 0 ||
                FacMasterListing.CountByBenefitCode(bo.Code) > 0 ||
                PremiumSpreadTableDetail.CountByBenefitId(bo.Id) > 0 ||
                TreatyDiscountTableDetail.CountByBenefitId(bo.Code) > 0 ||
                RetroBenefitCodeMappingService.CountByBenefitId(bo.Id) > 0
            )
            {
                result.AddErrorRecordInUsed();
            }

            if (result.Valid)
            {
                BenefitDetail.DeleteAllByBenefitId(bo.Id);

                DataTrail dataTrail = Benefit.Delete(bo.Id);
                dataTrail.Merge(ref trail, result.Table);
            }

            return result;
        }
    }
}
