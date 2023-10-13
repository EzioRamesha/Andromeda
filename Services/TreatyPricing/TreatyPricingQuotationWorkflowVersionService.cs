using BusinessObject;
using BusinessObject.TreatyPricing;
using DataAccess.Entities.TreatyPricing;
using DataAccess.EntityFramework;
using Services.Identity;
using Shared;
using Shared.DataAccess;
using Shared.Trails;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Services.TreatyPricing
{
    public class TreatyPricingQuotationWorkflowVersionService
    {
        public static Result Result()
        {
            return new Result
            {
                Table = UtilAttribute.GetTableName(typeof(TreatyPricingQuotationWorkflowVersion)),
                Controller = ModuleBo.ModuleController.TreatyPricingQuotationWorkflowVersion.ToString()
            };
        }

        public static TreatyPricingQuotationWorkflowVersionBo FormBo(TreatyPricingQuotationWorkflowVersion entity = null, bool foreign = true)
        {
            if (entity == null)
                return null;
            var bo = new TreatyPricingQuotationWorkflowVersionBo
            {
                Id = entity.Id,
                TreatyPricingQuotationWorkflowId = entity.TreatyPricingQuotationWorkflowId,
                Version = entity.Version,

                //Quotation
                BDPersonInChargeId = entity.BDPersonInChargeId,
                QuoteValidityDay = entity.QuoteValidityDay,
                QuoteSpecTemplate = entity.QuoteSpecTemplate,
                RateTableTemplate = entity.RateTableTemplate,
                QuoteSpecSharePointLink = entity.QuoteSpecSharePointLink,
                QuoteSpecSharePointFolderPath = entity.QuoteSpecSharePointFolderPath,
                RateTableSharePointLink = entity.RateTableSharePointLink,
                RateTableSharePointFolderPath = entity.RateTableSharePointFolderPath,
                FinalQuoteSpecFileName = entity.FinalQuoteSpecFileName,
                FinalQuoteSpecHashFileName = entity.FinalQuoteSpecHashFileName,
                FinalRateTableFileName = entity.FinalRateTableFileName,
                FinalRateTableHashFileName = entity.FinalRateTableHashFileName,
                ChecklistFinalised = entity.ChecklistFinalised,

                BDPersonInChargeName = entity.BDPersonInChargeId.HasValue ? UserService.Find(entity.BDPersonInChargeId).FullName : "",

                //Pricing
                PersonInChargeId = entity.PersonInChargeId,
                PersonInChargeTechReviewerId = entity.PersonInChargeTechReviewerId,
                PersonInChargePeerReviewerId = entity.PersonInChargePeerReviewerId,
                PersonInChargePricingAuthorityReviewerId = entity.PersonInChargePricingAuthorityReviewerId,
                PendingOn = entity.PendingOn,
                RequestDate = entity.RequestDate,
                TargetPricingDueDate = entity.TargetPricingDueDate,
                RevisedPricingDueDate = entity.RevisedPricingDueDate,
                PricingCompletedDate = entity.PricingCompletedDate,
                ProfitMargin = entity.ProfitMargin,
                FirstYearPremium = entity.FirstYearPremium,
                PVProfit = entity.PVProfit,
                ROE = entity.ROE,
                ExpenseMargin = entity.ExpenseMargin,
                ProfitMarginStr = entity.ProfitMargin != null ? Util.DoubleToString(entity.ProfitMargin,2) : "",
                FirstYearPremiumStr = entity.FirstYearPremium != null ? Util.DoubleToString(entity.FirstYearPremium,2) : "",
                PVProfitStr = entity.PVProfit != null ? Util.DoubleToString(entity.PVProfit,2) : "",
                ROEStr = entity.ROE != null ? Util.DoubleToString(entity.ROE,2) : "",
                ExpenseMarginStr = entity.ExpenseMargin != null ? Util.DoubleToString(entity.ExpenseMargin,2) : "",
                FileLocationPricingMemo = entity.FileLocationPricingMemo,
                FileLocationNBChecklist = entity.FileLocationNBChecklist,
                FileLocationTechnicalChecklist = entity.FileLocationTechnicalChecklist,

                PersonInChargeName = entity.PersonInChargeId.HasValue ? UserService.Find(entity.PersonInChargeId).FullName : "",
                PersonInChargeTechReviewerName = entity.PersonInChargeTechReviewerId.HasValue ? UserService.Find(entity.PersonInChargeTechReviewerId).FullName : "",
                PersonInChargePeerReviewerName = entity.PersonInChargePeerReviewerId.HasValue ? UserService.Find(entity.PersonInChargePeerReviewerId).FullName : "",
                PersonInChargePricingAuthorityReviewerName = entity.PersonInChargePricingAuthorityReviewerId.HasValue ? UserService.Find(entity.PersonInChargePricingAuthorityReviewerId).FullName : "",

                RequestDateStr = entity.RequestDate.HasValue ? entity.RequestDate.Value.ToString(Util.GetDateFormat()) : "",
                TargetPricingDueDateStr = entity.TargetPricingDueDate.HasValue ? entity.TargetPricingDueDate.Value.ToString(Util.GetDateFormat()) : "",
                RevisedPricingDueDateStr = entity.RevisedPricingDueDate.HasValue ? entity.RevisedPricingDueDate.Value.ToString(Util.GetDateFormat()) : "",
                PricingCompletedDateStr = entity.PricingCompletedDate.HasValue ? entity.PricingCompletedDate.Value.ToString(Util.GetDateFormat()) : "",

                CreatedById = entity.CreatedById,
                UpdatedById = entity.UpdatedById,
            };

            if (foreign)
            {
                bo.TreatyPricingQuotationWorkflowBo = TreatyPricingQuotationWorkflowService.Find(entity.TreatyPricingQuotationWorkflowId);
            }

            return bo;
        }

        public static IList<TreatyPricingQuotationWorkflowVersionBo> FormBos(IList<TreatyPricingQuotationWorkflowVersion> entities = null, bool foreign = false)
        {
            if (entities == null)
                return null;
            IList<TreatyPricingQuotationWorkflowVersionBo> bos = new List<TreatyPricingQuotationWorkflowVersionBo>() { };
            foreach (TreatyPricingQuotationWorkflowVersion entity in entities.OrderBy(i => i.Version))
            {
                bos.Add(FormBo(entity, foreign));
            }
            return bos;
        }

        public static TreatyPricingQuotationWorkflowVersion FormEntity(TreatyPricingQuotationWorkflowVersionBo bo = null)
        {
            if (bo == null)
                return null;
            return new TreatyPricingQuotationWorkflowVersion
            {
                Id = bo.Id,
                TreatyPricingQuotationWorkflowId = bo.TreatyPricingQuotationWorkflowId,
                Version = bo.Version,

                //Quotation
                BDPersonInChargeId = bo.BDPersonInChargeId,
                QuoteValidityDay = bo.QuoteValidityDay,
                QuoteSpecTemplate = bo.QuoteSpecTemplate,
                RateTableTemplate = bo.RateTableTemplate,
                QuoteSpecSharePointLink = bo.QuoteSpecSharePointLink,
                QuoteSpecSharePointFolderPath = bo.QuoteSpecSharePointFolderPath,
                RateTableSharePointLink = bo.RateTableSharePointLink,
                RateTableSharePointFolderPath = bo.RateTableSharePointFolderPath,
                FinalQuoteSpecFileName = bo.FinalQuoteSpecFileName,
                FinalQuoteSpecHashFileName = bo.FinalQuoteSpecHashFileName,
                FinalRateTableFileName = bo.FinalRateTableFileName,
                FinalRateTableHashFileName = bo.FinalRateTableHashFileName,
                ChecklistFinalised = bo.ChecklistFinalised,

                //Pricing
                PersonInChargeId = bo.PersonInChargeId,
                PersonInChargeTechReviewerId = bo.PersonInChargeTechReviewerId,
                PersonInChargePeerReviewerId = bo.PersonInChargePeerReviewerId,
                PersonInChargePricingAuthorityReviewerId = bo.PersonInChargePricingAuthorityReviewerId,
                PendingOn = bo.PendingOn,
                RequestDate = bo.RequestDate,
                TargetPricingDueDate = bo.TargetPricingDueDate,
                RevisedPricingDueDate = bo.RevisedPricingDueDate,
                PricingCompletedDate = bo.PricingCompletedDate,
                ProfitMargin = bo.ProfitMargin,
                FirstYearPremium = bo.FirstYearPremium,
                PVProfit = bo.PVProfit,
                ROE = bo.ROE,
                ExpenseMargin = bo.ExpenseMargin,
                FileLocationPricingMemo = bo.FileLocationPricingMemo,
                FileLocationNBChecklist = bo.FileLocationNBChecklist,
                FileLocationTechnicalChecklist = bo.FileLocationTechnicalChecklist,

                CreatedById = bo.CreatedById,
                UpdatedById = bo.UpdatedById,
            };
        }

        public static bool IsExists(int id)
        {
            return TreatyPricingQuotationWorkflowVersion.IsExists(id);
        }

        public static TreatyPricingQuotationWorkflowVersionBo Find(int id)
        {
            return FormBo(TreatyPricingQuotationWorkflowVersion.Find(id));
        }

        public static IList<TreatyPricingQuotationWorkflowVersionBo> GetByTreatyPricingQuotationWorkflowId(int treatyPricingQuotationWorkflowId)
        {
            using (var db = new AppDbContext())
            {
                return FormBos(db.TreatyPricingQuotationWorkflowVersions
                    .Where(q => q.TreatyPricingQuotationWorkflowId == treatyPricingQuotationWorkflowId)
                    .OrderByDescending(q => q.Version)
                    .ToList());
            }
        }

        public static TreatyPricingQuotationWorkflowVersionBo GetLatestVersionByTreatyPricingQuotationWorkflowId(int treatyPricingQuotationWorkflowId)
        {
            using (var db = new AppDbContext())
            {
                return FormBo(db.TreatyPricingQuotationWorkflowVersions
                    .Where(q => q.TreatyPricingQuotationWorkflowId == treatyPricingQuotationWorkflowId)
                    .OrderByDescending(q => q.Version)
                    .FirstOrDefault());
            }
        }

        public static IList<TreatyPricingQuotationWorkflowVersionBo> GetLatestVersionBos()
        {
            IList<TreatyPricingQuotationWorkflowBo> bos = TreatyPricingQuotationWorkflowService.GetAll();
            IList<TreatyPricingQuotationWorkflowVersionBo> versionBos = new List<TreatyPricingQuotationWorkflowVersionBo>();

            foreach (var bo in bos)
            {
                var versionBo = GetLatestVersionByTreatyPricingQuotationWorkflowId(bo.Id);
                versionBos.Add(versionBo);
            }

            return versionBos;
        }

        public static int GetVersionId(int quotationWorkflowId, int quotationWorkflowVersion)
        {
            using (var db = new AppDbContext())
            {
                return db.TreatyPricingQuotationWorkflowVersions
                    .FirstOrDefault(q => q.TreatyPricingQuotationWorkflowId == quotationWorkflowId
                        && q.Version == quotationWorkflowVersion).Id;
            }
        }

        public static Result Save(ref TreatyPricingQuotationWorkflowVersionBo bo)
        {
            if (!TreatyPricingQuotationWorkflowVersion.IsExists(bo.Id))
            {
                return Create(ref bo);
            }
            return Update(ref bo);
        }

        public Result Save(ref TreatyPricingQuotationWorkflowVersionBo bo, ref TrailObject trail)
        {
            if (!TreatyPricingQuotationWorkflowVersion.IsExists(bo.Id))
            {
                return Create(ref bo, ref trail);
            }
            return Update(ref bo, ref trail);
        }

        public static Result Create(ref TreatyPricingQuotationWorkflowVersionBo bo)
        {
            TreatyPricingQuotationWorkflowVersion entity = FormEntity(bo);

            Result result = Result();

            if (result.Valid)
            {
                result.DataTrail = entity.Create();
                bo.Id = entity.Id;
            }

            return result;
        }

        public static Result Create(ref TreatyPricingQuotationWorkflowVersionBo bo, ref TrailObject trail)
        {
            Result result = Create(ref bo);
            if (result.Valid)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static Result Update(ref TreatyPricingQuotationWorkflowVersionBo bo)
        {
            Result result = Result();

            TreatyPricingQuotationWorkflowVersion entity = TreatyPricingQuotationWorkflowVersion.Find(bo.Id);
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

        public static Result Update(ref TreatyPricingQuotationWorkflowVersionBo bo, ref TrailObject trail)
        {
            Result result = Update(ref bo);
            if (result.Valid && result.DataTrail != null)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static void Delete(TreatyPricingQuotationWorkflowVersionBo bo)
        {
            TreatyPricingQuotationWorkflowVersion.Delete(bo.Id);
        }

        public static Result Delete(TreatyPricingQuotationWorkflowVersionBo bo, ref TrailObject trail)
        {
            Result result = Result();

            // TODO: In Use Validation

            if (result.Valid)
            {
                DataTrail dataTrail = TreatyPricingQuotationWorkflowVersion.Delete(bo.Id);
                dataTrail.Merge(ref trail, result.Table);
            }

            return result;
        }

        public static IList<DataTrail> DeleteAllByTreatyPricingQuotationWorkflowId(int treatyPricingQuotationWorkflowId)
        {
            return TreatyPricingQuotationWorkflowVersion.DeleteAllByTreatyPricingQuotationWorkflowId(treatyPricingQuotationWorkflowId);
        }

        public static void DeleteAllByTreatyPricingQuotationWorkflowId(int treatyPricingQuotationWorkflowId, ref TrailObject trail)
        {
            IList<DataTrail> dataTrails = DeleteAllByTreatyPricingQuotationWorkflowId(treatyPricingQuotationWorkflowId);
            if (dataTrails.Count > 0)
            {
                foreach (DataTrail dataTrail in dataTrails)
                {
                    dataTrail.Merge(ref trail, UtilAttribute.GetTableName(typeof(TreatyPricingQuotationWorkflowVersion)));
                }
            }
        }
    }
}
