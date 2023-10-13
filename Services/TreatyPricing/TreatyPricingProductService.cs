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
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Services.TreatyPricing
{
    public class TreatyPricingProductService
    {
        public static Result Result()
        {
            return new Result
            {
                Table = UtilAttribute.GetTableName(typeof(TreatyPricingProduct)),
                Controller = ModuleBo.ModuleController.TreatyPricingProduct.ToString()
            };
        }

        public static TreatyPricingProductBo FormBo(TreatyPricingProduct entity = null, bool loadToObjectVersion = false, bool foreign = true, bool getLatestWorkflow = false)
        {
            if (entity == null)
                return null;

            var versionBos = foreign ? TreatyPricingProductVersionService.GetByTreatyPricingProductId(entity.Id) : new List<TreatyPricingProductVersionBo>();

            TreatyPricingProductBo bo = new TreatyPricingProductBo
            {
                Id = entity.Id,
                TreatyPricingCedantId = entity.TreatyPricingCedantId,
                TreatyPricingCedantBo = foreign ? TreatyPricingCedantService.Find(entity.TreatyPricingCedantId) : null,
                Code = entity.Code,
                Name = entity.Name,
                EffectiveDate = entity.EffectiveDate,
                EffectiveDateStr = entity.EffectiveDate?.ToString(Util.GetDateFormat()),
                Summary = entity.Summary,
                QuotationName = entity.QuotationName,
                UnderwritingMethod = foreign ? TreatyPricingPickListDetailService.GetJoinCodeByObjectPickList(TreatyPricingCedantBo.ObjectProduct, entity.Id, PickListBo.UnderwritingMethod) : null,
                HasPerLifeRetro = entity.HasPerLifeRetro,
                PerLifeRetroTreatyCode = foreign ? TreatyPricingProductPerLifeRetroService.GetJoinCodeByProductId(entity.Id) : null,
                TreatyPricingProductVersionBos = loadToObjectVersion ? null : versionBos,

                CreatedById = entity.CreatedById,
                UpdatedById = entity.UpdatedById,

                #region Product & Benefit Comparison
                HasPerLifeRetroStr = entity.HasPerLifeRetro ? "Yes" : "No",
                #endregion
            };

            if (loadToObjectVersion)
            {
                bo.Name = bo.Name ?? "";
                bo.QuotationName = bo.QuotationName ?? "";
                bo.Summary = bo.Summary ?? "";
                bo.SetVersionObjects(versionBos);
            }

            if (getLatestWorkflow)
            {
                bo.LatestWorkflowObjectBo = TreatyPricingWorkflowObjectService.FindLatestByObjectTypeObjectId(TreatyPricingWorkflowObjectBo.ObjectTypeProduct, bo.Id);
            }

            return bo;
        }

        public static TreatyPricingProductBo FormBoForProductComparisonReport(TreatyPricingProduct entity = null)
        {
            if (entity == null)
                return null;

            TreatyPricingProductBo bo = new TreatyPricingProductBo
            {
                Id = entity.Id,
                TreatyPricingCedantId = entity.TreatyPricingCedantId,
                TreatyPricingCedantBo = TreatyPricingCedantService.Find(entity.TreatyPricingCedantId),
                Code = entity.Code,
                Name = entity.Name,
                EffectiveDate = entity.EffectiveDate,
                EffectiveDateStr = entity.EffectiveDate?.ToString(Util.GetDateFormat()),
                Summary = entity.Summary,
                QuotationName = entity.QuotationName,
                HasPerLifeRetro = entity.HasPerLifeRetro,
                PerLifeRetroTreatyCode = TreatyPricingProductPerLifeRetroService.GetJoinCodeByProductId(entity.Id),

                CreatedById = entity.CreatedById,
                UpdatedById = entity.UpdatedById,

                HasPerLifeRetroStr = entity.HasPerLifeRetro ? "Yes" : "No",
            };

            return bo;
        }

        public static TreatyPricingProductBo LinkFormBo(TreatyPricingProduct entity = null)
        {
            if (entity == null)
                return null;

            TreatyPricingProductBo bo = new TreatyPricingProductBo
            {
                Id = entity.Id,
                TreatyPricingCedantId = entity.TreatyPricingCedantId,
                TreatyPricingCedantCode = TreatyPricingCedantService.Find(entity.TreatyPricingCedantId)?.Code,
                Code = entity.Code,
                Name = entity.Name,
                EffectiveDate = entity.EffectiveDate,
                EffectiveDateStr = entity.EffectiveDate?.ToString(Util.GetDateFormat()),
                Summary = entity.Summary,
                QuotationName = entity.QuotationName,
                UnderwritingMethod = TreatyPricingPickListDetailService.GetJoinCodeByObjectPickList(TreatyPricingCedantBo.ObjectProduct, entity.Id, PickListBo.UnderwritingMethod),
                HasPerLifeRetro = entity.HasPerLifeRetro,
                CreatedById = entity.CreatedById,
                UpdatedById = entity.UpdatedById,
                CurrentVersionObject = TreatyPricingProductVersionService.GetLatestByTreatyPricingProductId(entity.Id, true)
            };

            return bo;
        }

        public static IList<TreatyPricingProductBo> FormBos(IList<TreatyPricingProduct> entities = null, bool loadToObjectVersion = false, bool foreign = true, bool getLatestWorkflow = false)
        {
            if (entities == null)
                return null;
            IList<TreatyPricingProductBo> bos = new List<TreatyPricingProductBo>() { };
            foreach (TreatyPricingProduct entity in entities)
            {
                bos.Add(FormBo(entity, loadToObjectVersion, foreign, getLatestWorkflow));
            }
            return bos;
        }

        public static IList<TreatyPricingProductBo> LinkFormBos(IList<TreatyPricingProduct> entities = null)
        {
            if (entities == null)
                return null;
            IList<TreatyPricingProductBo> bos = new List<TreatyPricingProductBo>() { };
            foreach (TreatyPricingProduct entity in entities)
            {
                bos.Add(LinkFormBo(entity));
            }
            return bos;
        }

        public static TreatyPricingProduct FormEntity(TreatyPricingProductBo bo = null)
        {
            if (bo == null)
                return null;
            return new TreatyPricingProduct
            {
                Id = bo.Id,
                TreatyPricingCedantId = bo.TreatyPricingCedantId,
                Code = bo.Code,
                Name = bo.Name,
                EffectiveDate = bo.EffectiveDate,
                Summary = bo.Summary,
                QuotationName = bo.QuotationName,
                HasPerLifeRetro = bo.HasPerLifeRetro,

                CreatedById = bo.CreatedById,
                UpdatedById = bo.UpdatedById,
            };
        }

        public static bool IsExists(int id)
        {
            return TreatyPricingProduct.IsExists(id);
        }

        public static TreatyPricingProductBo Find(int? id)
        {
            return FormBo(TreatyPricingProduct.Find(id));
        }

        public static TreatyPricingProductBo FindForProductComparisonReport(int? id)
        {
            return FormBoForProductComparisonReport(TreatyPricingProduct.Find(id));
        }

        public static IList<TreatyPricingProductBo> Get()
        {
            using (var db = new AppDbContext())
            {
                return FormBos(db.TreatyPricingProducts.OrderBy(q => q.Code).ToList());
            }
        }

        public static IList<TreatyPricingProductBo> GetByTreatyPricingCedantId(int treatyPricingCedantId, bool getLatestWorkflow = false)
        {
            using (var db = new AppDbContext())
            {
                return FormBos(db.TreatyPricingProducts.Where(q => q.TreatyPricingCedantId == treatyPricingCedantId).OrderBy(q => q.Code).ToList(), foreign: false, getLatestWorkflow: getLatestWorkflow);
            }
        }

        public static IList<TreatyPricingProductBo> GetProductsByProductIds(List<int> productIds)
        {
            using (var db = new AppDbContext())
            {
                return FormBos(db.TreatyPricingProducts
                    .Where(q => productIds.Contains(q.Id))
                    .Where(q => !string.IsNullOrEmpty(q.Name))
                    .ToList());//, foreign: false);
            }
        }

        public static List<int> GetVersionIdsByProductId(int productId)
        {
            using (var db = new AppDbContext())
            {
                return db.TreatyPricingProductVersions
                    .Where(q => q.TreatyPricingProductId == productId)
                    .Select(q => q.Id)
                    .ToList();
            }
        }

        public static List<string> GetDistinctNameByProductIds(List<int> productIds)
        {
            using (var db = new AppDbContext())
            {
                return db.TreatyPricingProducts
                    .Where(q => productIds.Contains(q.Id))
                    .Where(q => !string.IsNullOrEmpty(q.Name))
                    .GroupBy(q => q.Name)
                    .Select(r => r.FirstOrDefault())
                    .OrderBy(q => q.Name)
                    .Select(q => q.Name)
                    .ToList();
            }
        }

        public static List<string> GetDistinctQuotationNameByProductIds(List<int> productIds)
        {
            using (var db = new AppDbContext())
            {
                return db.TreatyPricingProducts
                    .Where(q => productIds.Contains(q.Id))
                    .Where(q => !string.IsNullOrEmpty(q.QuotationName))
                    .GroupBy(q => q.QuotationName)
                    .Select(r => r.FirstOrDefault())
                    .OrderBy(q => q.QuotationName)
                    .Select(q => q.QuotationName)
                    .ToList();
            }
        }

        public static List<int> GetProductIdByProductIdsProductName(List<int> productIds, string ProductName)
        {
            using (var db = new AppDbContext())
            {
                return db.TreatyPricingProducts
                    .Where(q => productIds.Contains(q.Id))
                    .Where(q => !string.IsNullOrEmpty(q.Name))
                    .Where(q => q.Name == ProductName)
                    .Select(q => q.Id)
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

                string prefix = string.Format("{0}_PR_{1}_", cedantCode, year);

                var entity = db.TreatyPricingProducts.Where(q => !string.IsNullOrEmpty(q.Code) && q.Code.StartsWith(prefix)).OrderByDescending(q => q.Code).FirstOrDefault();

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

        #region Product Comparison
        public static List<int> GetIdByProductIdsTreatyPricingCedantId(int cedantId)
        {
            using (var db = new AppDbContext())
            {
                return db.TreatyPricingProducts.Where(q => q.TreatyPricingCedantId == cedantId).Select(q => q.Id).ToList();
            }
        }

        public static List<int> GetIdByProductIdsQuotationName(List<int> productIds, string quotationName)
        {
            using (var db = new AppDbContext())
            {
                return db.TreatyPricingProducts
                    .Where(q => productIds.Contains(q.Id))
                    .Where(q => q.QuotationName == quotationName)
                    .Select(q => q.Id)
                    .ToList();
            }
        }
        #endregion

        public static IList<TreatyPricingProductBo> GetRateTableProduct(int rateTableId)
        {
            using (var db = new AppDbContext())
            {
                return FormBos(db.TreatyPricingProductBenefits
                    .Where(q => q.TreatyPricingRateTableId.HasValue && q.TreatyPricingRateTableId == rateTableId)
                    .GroupBy(q => q.TreatyPricingProductVersion.TreatyPricingProductId)
                    .Select(g => g.FirstOrDefault())
                    .Select(q => q.TreatyPricingProductVersion.TreatyPricingProduct)
                    .ToList(), foreign: false);
            }
        }

        public static IList<TreatyPricingProductBo> GetClaimApprovalLimitProduct(int ClaimApprovalLimitId)
        {
            using (var db = new AppDbContext())
            {
                return FormBos(db.TreatyPricingProductBenefits
                    .Where(q => q.TreatyPricingClaimApprovalLimitId.HasValue && q.TreatyPricingClaimApprovalLimitId == ClaimApprovalLimitId)
                    .GroupBy(q => q.TreatyPricingProductVersion.TreatyPricingProductId)
                    .Select(g => g.FirstOrDefault())
                    .Select(q => q.TreatyPricingProductVersion.TreatyPricingProduct)
                    .ToList(), foreign: false);
            }
        }

        public static IList<TreatyPricingProductBo> GetDefinitionAndExclusionProduct(int DefinitionAndExclusionId)
        {
            using (var db = new AppDbContext())
            {
                return FormBos(db.TreatyPricingProductBenefits
                    .Where(q => q.TreatyPricingDefinitionAndExclusionId.HasValue && q.TreatyPricingDefinitionAndExclusionId == DefinitionAndExclusionId)
                    .GroupBy(q => q.TreatyPricingProductVersion.TreatyPricingProductId)
                    .Select(g => g.FirstOrDefault())
                    .Select(q => q.TreatyPricingProductVersion.TreatyPricingProduct)
                    .ToList(), foreign: false);
            }
        }

        public static IList<TreatyPricingProductBo> GetProfitCommissionProduct(int profitCommissionId)
        {
            using (var db = new AppDbContext())
            {
                return FormBos(db.TreatyPricingProductVersions
                    .Where(q => q.TreatyPricingProfitCommissionId.HasValue && q.TreatyPricingProfitCommissionId == profitCommissionId)
                    .GroupBy(q => q.TreatyPricingProductId)
                    .Select(g => g.FirstOrDefault())
                    .Select(q => q.TreatyPricingProduct)
                    .ToList(), foreign: false);
            }
        }

        public static IList<TreatyPricingProductBo> GetUwLimitProduct(int uwLimitId)
        {
            using (var db = new AppDbContext())
            {
                return FormBos(db.TreatyPricingProductBenefits
                    .Where(q => q.TreatyPricingUwLimitId.HasValue && q.TreatyPricingUwLimitId == uwLimitId)
                    .GroupBy(q => q.TreatyPricingProductVersion.TreatyPricingProductId)
                    .Select(g => g.FirstOrDefault())
                    .Select(q => q.TreatyPricingProductVersion.TreatyPricingProduct)
                    .ToList(), foreign: false);
            }
        }

        public static IList<TreatyPricingProductBo> GetMedicalTableProduct(int medicalTableId)
        {
            using (var db = new AppDbContext())
            {
                return FormBos(db.TreatyPricingProductVersions
                    .Where(q => q.TreatyPricingMedicalTableId.HasValue && q.TreatyPricingMedicalTableId == medicalTableId)
                    .GroupBy(q => q.TreatyPricingProductId)
                    .Select(g => g.FirstOrDefault())
                    .Select(q => q.TreatyPricingProduct)
                    .ToList(), foreign: false);
            }
        }

        public static IList<TreatyPricingProductBo> GetFinancialTableProduct(int financialTableId)
        {
            using (var db = new AppDbContext())
            {
                return FormBos(db.TreatyPricingProductVersions
                    .Where(q => q.TreatyPricingFinancialTableId.HasValue && q.TreatyPricingFinancialTableId == financialTableId)
                    .GroupBy(q => q.TreatyPricingProductId)
                    .Select(g => g.FirstOrDefault())
                    .Select(q => q.TreatyPricingProduct)
                    .ToList(), foreign: false);
            }
        }

        public static IList<TreatyPricingProductBo> GetUwQuestionnaireProduct(int uwQuestionnaireId)
        {
            using (var db = new AppDbContext())
            {
                return FormBos(db.TreatyPricingProductVersions
                    .Where(q => q.TreatyPricingUwQuestionnaireId.HasValue && q.TreatyPricingUwQuestionnaireId == uwQuestionnaireId)
                    .GroupBy(q => q.TreatyPricingProductId)
                    .Select(g => g.FirstOrDefault())
                    .Select(q => q.TreatyPricingProduct)
                    .ToList(), foreign: false);
            }
        }

        public static IList<TreatyPricingProductBo> GetAdvantageProgramProduct(int advantageProgramId)
        {
            using (var db = new AppDbContext())
            {
                return FormBos(db.TreatyPricingProductVersions
                    .Where(q => q.TreatyPricingAdvantageProgramId.HasValue && q.TreatyPricingAdvantageProgramId == advantageProgramId)
                    .GroupBy(q => q.TreatyPricingProductId)
                    .Select(g => g.FirstOrDefault())
                    .Select(q => q.TreatyPricingProduct)
                    .ToList(), foreign: false);
            }
        }

        public static int CountByTreatyPricingCedantId(int treatyPricingCedantId)
        {
            using (var db = new AppDbContext())
            {
                return db.TreatyPricingProducts.Where(q => q.TreatyPricingCedantId == treatyPricingCedantId).Count();
            }
        }

        public static Result Create(ref TreatyPricingProductBo bo)
        {
            TreatyPricingProduct entity = FormEntity(bo);

            Result result = Result();
            if (result.Valid)
            {
                result.DataTrail = entity.Create();
                bo.Id = entity.Id;
            }

            return result;
        }

        public static Result Create(ref TreatyPricingProductBo bo, ref TrailObject trail)
        {
            Result result = Create(ref bo);
            if (result.Valid)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static Result Update(ref TreatyPricingProductBo bo)
        {
            Result result = Result();

            TreatyPricingProduct entity = TreatyPricingProduct.Find(bo.Id);
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
                entity.EffectiveDate = bo.EffectiveDate;
                entity.Summary = bo.Summary;
                entity.QuotationName = bo.QuotationName;
                entity.HasPerLifeRetro = bo.HasPerLifeRetro;

                entity.UpdatedAt = DateTime.Now;
                entity.UpdatedById = bo.UpdatedById ?? entity.UpdatedById;
                result.DataTrail = entity.Update();
            }
            return result;
        }

        public static Result Update(ref TreatyPricingProductBo bo, ref TrailObject trail)
        {
            Result result = Update(ref bo);
            if (result.Valid && result.DataTrail != null)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static void Delete(TreatyPricingProductBo bo)
        {
            TreatyPricingProduct.Delete(bo.Id);
        }

        public static Result Delete(TreatyPricingProductBo bo, ref TrailObject trail)
        {
            Result result = Result();

            // TODO: In Use Validation

            if (result.Valid)
            {
                DataTrail dataTrail = TreatyPricingProduct.Delete(bo.Id);
                dataTrail.Merge(ref trail, result.Table);
            }

            return result;
        }
    }
}
