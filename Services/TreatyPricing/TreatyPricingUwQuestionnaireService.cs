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

namespace Services.TreatyPricing
{
    public class TreatyPricingUwQuestionnaireService
    {
        public static Result Result()
        {
            return new Result
            {
                Table = UtilAttribute.GetTableName(typeof(TreatyPricingUwQuestionnaire)),
                Controller = ModuleBo.ModuleController.TreatyPricingUwQuestionnaire.ToString()
            };
        }

        public static Expression<Func<TreatyPricingUwQuestionnaire, TreatyPricingUwQuestionnaireBo>> Expression()
        {
            return entity => new TreatyPricingUwQuestionnaireBo
            {
                Id = entity.Id,
                TreatyPricingCedantId = entity.TreatyPricingCedantId,
                Code = entity.Code,
                Name = entity.Name,
                Description = entity.Description,
                Status = entity.Status,
                BenefitCode = entity.BenefitCode,
                DistributionChannel = entity.DistributionChannel,
                CreatedById = entity.CreatedById,
                UpdatedById = entity.UpdatedById,                
            };
        }

        public static TreatyPricingUwQuestionnaireBo FormBo(TreatyPricingUwQuestionnaire entity = null, bool foreign = true)
        {
            if (entity == null)
                return null;
            var bo = new TreatyPricingUwQuestionnaireBo
            {
                Id = entity.Id,
                TreatyPricingCedantId = entity.TreatyPricingCedantId,
                TreatyPricingCedantBo = foreign ? TreatyPricingCedantService.Find(entity.TreatyPricingCedantId) : null,
                Code = entity.Code,
                Name = entity.Name,
                Description = entity.Description,
                Status = entity.Status,
                BenefitCode = entity.BenefitCode,
                DistributionChannel = entity.DistributionChannel,
                TypeName = GetQuestionnaireType(entity.Id),
                CreatedById = entity.CreatedById,
                UpdatedById = entity.UpdatedById,

                StatusName = TreatyPricingUwQuestionnaireBo.GetStatusName(entity.Status),
                TreatyPricingUwQuestionnaireVersionBos = foreign ? TreatyPricingUwQuestionnaireVersionService.GetByTreatyPricingUwQuestionnaireId(entity.Id) : null,
            };
            return bo;
        }

        public static IList<TreatyPricingUwQuestionnaireBo> FormBos(IList<TreatyPricingUwQuestionnaire> entities = null, bool foreign = true)
        {
            if (entities == null)
                return null;
            IList<TreatyPricingUwQuestionnaireBo> bos = new List<TreatyPricingUwQuestionnaireBo>() { };
            foreach (TreatyPricingUwQuestionnaire entity in entities)
            {
                bos.Add(FormBo(entity, foreign));
            }
            return bos;
        }

        public static TreatyPricingUwQuestionnaire FormEntity(TreatyPricingUwQuestionnaireBo bo = null)
        {
            if (bo == null)
                return null;
            return new TreatyPricingUwQuestionnaire
            {
                Id = bo.Id,
                TreatyPricingCedantId = bo.TreatyPricingCedantId,
                Code = bo.Code,
                Name = bo.Name,
                Description = bo.Description,
                Status = bo.Status,
                BenefitCode = bo.BenefitCode,
                DistributionChannel = bo.DistributionChannel,
                CreatedById = bo.CreatedById,
                UpdatedById = bo.UpdatedById,
            };
        }

        public static bool IsExists(int id)
        {
            return TreatyPricingUwQuestionnaire.IsExists(id);
        }

        public static TreatyPricingUwQuestionnaireBo Find(int? id)
        {
            return FormBo(TreatyPricingUwQuestionnaire.Find(id));
        }

        public static IList<TreatyPricingUwQuestionnaireBo> FindByStatus(int status)
        {
            using (var db = new AppDbContext())
            {
                return FormBos(db.TreatyPricingUwQuestionnaires.Where(q => q.Status == status).ToList());
            }
        }

        public static IList<TreatyPricingUwQuestionnaireBo> GetByTreatyPricingCedantId(int treatyPricingCedantId)
        {
            using (var db = new AppDbContext())
            {
                return FormBos(db.TreatyPricingUwQuestionnaires
                    .Where(q => q.TreatyPricingCedantId == treatyPricingCedantId)
                    .ToList(), false);
            }
        }

        public static IList<TreatyPricingUwQuestionnaireBo> GetByTreatyPricingCedantIdWithForeign(int treatyPricingCedantId)
        {
            using (var db = new AppDbContext())
            {
                return FormBos(db.TreatyPricingUwQuestionnaires
                    .Where(q => q.TreatyPricingCedantId == treatyPricingCedantId)
                    .ToList());
            }
        }

        public static List<string> GetCodeByTreatyPricingCedantIdProductName(int treatyPricingCedantId, string productName)
        {
            using (var db = new AppDbContext())
            {
                return db.TreatyPricingProductVersions
                    .Where(q => q.TreatyPricingProduct.TreatyPricingCedantId == treatyPricingCedantId)
                    .Where(q => q.TreatyPricingProduct.Name.Contains(productName))
                    .Where(q => q.TreatyPricingUwQuestionnaireId.HasValue)
                    .GroupBy(q => q.TreatyPricingUwQuestionnaireId)
                    .Select(g => g.FirstOrDefault())
                    .Select(q => q.TreatyPricingUwQuestionnaire.Code)
                    .ToList();
            }
        }

        public static List<int> GetIdByUwQuestionnaireIdsTreatyPricingCedantId(int cedantId)
        {
            using (var db = new AppDbContext())
            {
                return db.TreatyPricingUwQuestionnaires
                    .Where(q => q.TreatyPricingCedantId == cedantId)
                    .Select(q => q.Id)
                    .ToList();
            }
        }

        public static IList<TreatyPricingUwQuestionnaireBo> GetByParams(int? treatyPricingCedantId, string benefitCode, string distributionChannel, int? status)
        {
            using (var db = new AppDbContext())
            {
                var query = (from a in db.TreatyPricingUwQuestionnaires
                             where a.BenefitCode.Contains(benefitCode) && a.DistributionChannel.Contains(distributionChannel)
                             && a.TreatyPricingCedantId == treatyPricingCedantId && a.Status == status
                             select a).ToList()
                             .Where(a => a.BenefitCode.Split(',').Contains(benefitCode))
                             .Where(a => a.DistributionChannel.Split(',').Contains(distributionChannel));

                return FormBos(query.ToList());
            }
        }

        public static IList<string> GetDistinctBenefitCodeByCedantId(int cedantId)
        {
            using (var db = new AppDbContext())
            {
                var bos = GetByTreatyPricingCedantId(cedantId);
                List<string> benefitCodeList = new List<string>();

                foreach (var bo in bos.Where(q => !string.IsNullOrEmpty(q.BenefitCode)))
                {
                    var benefitCodes = bo.BenefitCode.Split(',').ToList().Select(s => s.Trim());

                    foreach (string benefitCode in benefitCodes)
                    {
                        benefitCodeList.Add(benefitCode);
                    }
                }
                return benefitCodeList.Distinct().ToList();
            }
        }

        public static IList<string> GetDistinctDistributionChannelByCedantId(int cedantId)
        {
            using (var db = new AppDbContext())
            {
                var bos = GetByTreatyPricingCedantId(cedantId);
                List<string> distributionChannelList = new List<string>();

                foreach (var bo in bos.Where(q => !string.IsNullOrEmpty(q.DistributionChannel)))
                {
                    var distributionChannels = bo.DistributionChannel.Split(',').ToList().Select(s => s.Trim());

                    foreach (string distributionChannel in distributionChannels)
                    {
                        distributionChannelList.Add(distributionChannel);
                    }
                }
                return distributionChannelList.Distinct().ToList();
            }
        }

        public static string GetNextObjectId(int treatyPricingCedantId)
        {
            using (var db = new AppDbContext())
            {
                var tpCedant = db.TreatyPricingCedants.Where(q => q.Id == treatyPricingCedantId).FirstOrDefault();
                string cedantCode = tpCedant.Code;
                int year = DateTime.Today.Year;

                string prefix = string.Format("{0}_UWQ_{1}_", cedantCode, year);

                var entity = db.TreatyPricingUwQuestionnaires.Where(q => !string.IsNullOrEmpty(q.Code) && q.Code.StartsWith(prefix)).OrderByDescending(q => q.Code).FirstOrDefault();

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

        public static string GetQuestionnaireType(int id)
        {
            var latestVersionBo = TreatyPricingUwQuestionnaireVersionService.GetLatestVersionByTreatyPricingUwQuestionnaireId(id);
            if (latestVersionBo == null)
                return "";

            return latestVersionBo.TypeName;
        }

        public static int CountByStatus(int status)
        {
            using (var db = new AppDbContext())
            {
                return db.TreatyPricingUwQuestionnaires.Where(q => q.Status == status).Count();
            }
        }

        public static Result Save(ref TreatyPricingUwQuestionnaireBo bo)
        {
            if (!TreatyPricingUwQuestionnaire.IsExists(bo.Id))
            {
                return Create(ref bo);
            }
            return Update(ref bo);
        }

        public Result Save(ref TreatyPricingUwQuestionnaireBo bo, ref TrailObject trail)
        {
            if (!TreatyPricingUwQuestionnaire.IsExists(bo.Id))
            {
                return Create(ref bo, ref trail);
            }
            return Update(ref bo, ref trail);
        }

        public static Result Create(ref TreatyPricingUwQuestionnaireBo bo)
        {
            TreatyPricingUwQuestionnaire entity = FormEntity(bo);

            Result result = Result();

            if (result.Valid)
            {
                result.DataTrail = entity.Create();
                bo.Id = entity.Id;
            }

            return result;
        }

        public static Result Create(ref TreatyPricingUwQuestionnaireBo bo, ref TrailObject trail)
        {
            Result result = Create(ref bo);
            if (result.Valid)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static Result Update(ref TreatyPricingUwQuestionnaireBo bo)
        {
            Result result = Result();

            TreatyPricingUwQuestionnaire entity = TreatyPricingUwQuestionnaire.Find(bo.Id);
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
                entity.BenefitCode = bo.BenefitCode;
                entity.DistributionChannel = bo.DistributionChannel;
                entity.UpdatedAt = DateTime.Now;
                entity.UpdatedById = bo.UpdatedById ?? entity.UpdatedById;
                result.DataTrail = entity.Update();
            }
            return result;
        }

        public static Result Update(ref TreatyPricingUwQuestionnaireBo bo, ref TrailObject trail)
        {
            Result result = Update(ref bo);
            if (result.Valid && result.DataTrail != null)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static void Delete(TreatyPricingUwQuestionnaireBo bo)
        {
            TreatyPricingUwQuestionnaire.Delete(bo.Id);
        }

        public static Result Delete(TreatyPricingUwQuestionnaireBo bo, ref TrailObject trail)
        {
            Result result = Result();

            // TODO: In Use Validation

            if (result.Valid)
            {
                DataTrail dataTrail = TreatyPricingUwQuestionnaire.Delete(bo.Id);
                dataTrail.Merge(ref trail, result.Table);
            }

            return result;
        }

        public static IList<DataTrail> DeleteAllByTreatyPricingCedantId(int treatyPricingCedantId)
        {
            return TreatyPricingUwQuestionnaire.DeleteAllByTreatyPricingCedantId(treatyPricingCedantId);
        }

        public static void DeleteAllByTreatyPricingCedantId(int treatyPricingCedantId, ref TrailObject trail)
        {
            IList<DataTrail> dataTrails = DeleteAllByTreatyPricingCedantId(treatyPricingCedantId);
            if (dataTrails.Count > 0)
            {
                foreach (DataTrail dataTrail in dataTrails)
                {
                    dataTrail.Merge(ref trail, UtilAttribute.GetTableName(typeof(TreatyPricingUwQuestionnaire)));
                }
            }
        }
    }
}
