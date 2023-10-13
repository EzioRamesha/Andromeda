using BusinessObject;
using BusinessObject.TreatyPricing;
using DataAccess.Entities.TreatyPricing;
using DataAccess.EntityFramework;
using Shared;
using Shared.DataAccess;
using Shared.Trails;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Services.TreatyPricing
{
    public class TreatyPricingAdvantageProgramService
    {
        public static Result Result()
        {
            return new Result
            {
                Table = UtilAttribute.GetTableName(typeof(TreatyPricingAdvantageProgram)),
                Controller = ModuleBo.ModuleController.TreatyPricingAdvantageProgram.ToString()
            };
        }

        public static TreatyPricingAdvantageProgramBo FormBo(TreatyPricingAdvantageProgram entity = null, bool foreign = true)
        {
            if (entity == null)
                return null;

            var bo = new TreatyPricingAdvantageProgramBo
            {
                Id = entity.Id,
                TreatyPricingCedantId = entity.TreatyPricingCedantId,
                //TreatyPricingCedantBo = TreatyPricingCedantService.Find(entity.TreatyPricingCedantId),
                TreatyPricingAdvantageProgramVersionBos = foreign ? TreatyPricingAdvantageProgramVersionService.GetByTreatyPricingAdvantageProgramId(entity.Id) : null,
                Code = entity.Code,
                Name = entity.Name,
                Description = entity.Description,
                Status = entity.Status,
                StatusName = TreatyPricingAdvantageProgramBo.GetStatusName(entity.Status),

                CreatedById = entity.CreatedById,
                UpdatedById = entity.UpdatedById,
            };

            var benefitBo = TreatyPricingAdvantageProgramVersionBenefitService.GetLatestBenefitSummarised(bo.Id);
            if (benefitBo != null)
            {
                bo.BenefitCodeNames = benefitBo.BenefitCode;
                bo.BenefitSumAssured = benefitBo.SumAssuredStr;
                bo.BenefitEMs = benefitBo.ExtraMortalityStr;
            }

            return bo;
        }

        public static string GetBenefitCodeNames(int id)
        {
            var latestVersionBo = TreatyPricingAdvantageProgramVersionService.GetLatestByTreatyPricingAdvantageProgramId(id);
            if (latestVersionBo == null)
                return "";

            using (var db = new AppDbContext())
            {
                List<string> benefitCodes = db.TreatyPricingAdvantageProgramVersionBenefits
                    .Where(q => q.TreatyPricingAdvantageProgramVersionId == latestVersionBo.Id)
                    .Select(q => q.Benefit.Code)
                    .ToList();

                return string.Join(", ", benefitCodes);
            }
        }        

        public static TreatyPricingAdvantageProgramBo FormBo2(TreatyPricingAdvantageProgram entity = null, bool foreign = true)
        {
            if (entity == null)
                return null;
            var bo = new TreatyPricingAdvantageProgramBo
            {
                Id = entity.Id,
                TreatyPricingCedantId = entity.TreatyPricingCedantId,
                Code = entity.Code,
                Name = entity.Name,
                Description = entity.Description,
                Status = entity.Status,                
                StatusName = TreatyPricingAdvantageProgramBo.GetStatusName(entity.Status),

                CreatedById = entity.CreatedById,
                UpdatedById = entity.UpdatedById,
            };
            if (foreign)
            {
                //bo.TreatyPricingCedantBo = TreatyPricingCedantService.Find(entity.TreatyPricingCedantId);
                //bo.TreatyPricingAdvantageProgramVersionBos = TreatyPricingAdvantageProgramVersionService.GetByTreatyPricingAdvantageProgramId(entity.Id);
            }
            return bo;
        }

        public static IList<TreatyPricingAdvantageProgramBo> FormBos(IList<TreatyPricingAdvantageProgram> entities = null, bool foreign = true)
        {
            if (entities == null)
                return null;
            IList<TreatyPricingAdvantageProgramBo> bos = new List<TreatyPricingAdvantageProgramBo>() { };
            foreach (TreatyPricingAdvantageProgram entity in entities)
            {
                bos.Add(FormBo(entity, foreign));
            }
            return bos;
        }

        public static TreatyPricingAdvantageProgram FormEntity(TreatyPricingAdvantageProgramBo bo = null)
        {
            if (bo == null)
                return null;
            return new TreatyPricingAdvantageProgram
            {
                Id = bo.Id,
                TreatyPricingCedantId = bo.TreatyPricingCedantId,
                Code = bo.Code,
                Name = bo.Name,
                Description = bo.Description,
                Status = bo.Status,

                CreatedById = bo.CreatedById,
                UpdatedById = bo.UpdatedById,
            };
        }

        public static bool IsExists(int id)
        {
            return TreatyPricingAdvantageProgram.IsExists(id);
        }

        public static TreatyPricingAdvantageProgramBo Find(int? id)
        {
            return FormBo(TreatyPricingAdvantageProgram.Find(id));
        }

        public static IList<TreatyPricingAdvantageProgramBo> Get()
        {
            using (var db = new AppDbContext())
            {
                return FormBos(db.TreatyPricingAdvantagePrograms.OrderBy(q => q.Code).ToList());
            }
        }

        public static IList<TreatyPricingAdvantageProgramBo> GetByTreatyPricingCedantId(int treatyPricingCedantId)
        {
            using (var db = new AppDbContext())
            {
                return FormBos(db.TreatyPricingAdvantagePrograms.Where(q => q.TreatyPricingCedantId == treatyPricingCedantId).ToList(), false);
            }
        }

        public static List<string> GetCodeByTreatyPricingCedantIdProductName(int treatyPricingCedantId, string productName)
        {
            using (var db = new AppDbContext())
            {
                return db.TreatyPricingProductVersions
                    .Where(q => q.TreatyPricingProduct.TreatyPricingCedantId == treatyPricingCedantId)
                    .Where(q => q.TreatyPricingProduct.Name.Contains(productName))
                    .Where(q => q.TreatyPricingAdvantageProgramId.HasValue)
                    .GroupBy(q => q.TreatyPricingAdvantageProgramId)
                    .Select(g => g.FirstOrDefault())
                    .Select(q => q.TreatyPricingAdvantageProgram.Code)
                    .ToList();
            }
        }

        public static string GetNextObjectId(int treatyPricingCedantId)
        {
            using (var db = new AppDbContext())
            {
                var tpCedant = db.TreatyPricingCedants.Where(q => q.Id == treatyPricingCedantId).FirstOrDefault();
                string cedantCode = tpCedant.Code;
                int year = DateTime.Today.Year;

                string prefix = string.Format("{0}_AP_{1}_", cedantCode, year);

                var entity = db.TreatyPricingAdvantagePrograms.Where(q => !string.IsNullOrEmpty(q.Code) && q.Code.StartsWith(prefix)).OrderByDescending(q => q.Code).FirstOrDefault();

                int nextIndex = 0;
                if (entity != null)
                {
                    string code = entity.Code;
                    string currentIndexStr = code.Substring(code.LastIndexOf('_') + 1);

                    int.TryParse(currentIndexStr, out nextIndex);
                }
                nextIndex++;
                string nextIndexStr = nextIndex.ToString().PadLeft(3, '0');

                return prefix + nextIndexStr;
            }
        }

        public static int CountByTreatyPricingCedantId(int treatyPricingCedantId)
        {
            using (var db = new AppDbContext())
            {
                return db.TreatyPricingAdvantagePrograms.Where(q => q.TreatyPricingCedantId == treatyPricingCedantId).Count();
            }
        }

        public static Result Create(ref TreatyPricingAdvantageProgramBo bo)
        {
            TreatyPricingAdvantageProgram entity = FormEntity(bo);

            Result result = Result();
            if (result.Valid)
            {
                result.DataTrail = entity.Create();
                bo.Id = entity.Id;
            }

            return result;
        }

        public static Result Create(ref TreatyPricingAdvantageProgramBo bo, ref TrailObject trail)
        {
            Result result = Create(ref bo);
            if (result.Valid)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static Result Update(ref TreatyPricingAdvantageProgramBo bo)
        {
            Result result = Result();

            TreatyPricingAdvantageProgram entity = TreatyPricingAdvantageProgram.Find(bo.Id);
            if (entity == null)
            {
                throw new Exception(MessageBag.NoRecordFound);
            }

            if (result.Valid)
            {
                entity.Id = bo.Id;
                entity.TreatyPricingCedantId = bo.TreatyPricingCedantId;
                entity.Code = bo.Code;
                entity.Name = bo.Name;
                entity.Description = bo.Description;
                entity.Status = bo.Status;
                entity.UpdatedAt = DateTime.Now;
                entity.UpdatedById = bo.UpdatedById ?? entity.UpdatedById;
                result.DataTrail = entity.Update();
            }
            return result;
        }

        public static Result Update(ref TreatyPricingAdvantageProgramBo bo, ref TrailObject trail)
        {
            Result result = Update(ref bo);
            if (result.Valid && result.DataTrail != null)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static void Delete(TreatyPricingAdvantageProgramBo bo)
        {
            TreatyPricingAdvantageProgram.Delete(bo.Id);
        }

        public static Result Delete(TreatyPricingAdvantageProgramBo bo, ref TrailObject trail)
        {
            Result result = Result();

            // TODO: In Use Validation

            if (result.Valid)
            {
                DataTrail dataTrail = TreatyPricingAdvantageProgram.Delete(bo.Id);
                dataTrail.Merge(ref trail, result.Table);
            }

            return result;
        }
    }
}
