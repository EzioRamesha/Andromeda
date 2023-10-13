using BusinessObject;
using BusinessObject.TreatyPricing;
using DataAccess.Entities.TreatyPricing;
using DataAccess.EntityFramework;
using Shared;
using Shared.DataAccess;
using Shared.Trails;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;

namespace Services.TreatyPricing
{
    public class TreatyPricingGroupReferralService
    {
        public static Result Result()
        {
            return new Result
            {
                Table = UtilAttribute.GetTableName(typeof(TreatyPricingGroupReferral)),
                Controller = ModuleBo.ModuleController.TreatyPricingGroupReferral.ToString()
            };
        }

        public static Expression<Func<TreatyPricingGroupReferral, TreatyPricingGroupReferralBo>> Expression()
        {
            return entity => new TreatyPricingGroupReferralBo
            {
                Id = entity.Id,
                CedantId = entity.CedantId,
                Code = entity.Code,
                Description = entity.Description,

                RiArrangementPickListDetailId = entity.RiArrangementPickListDetailId,
                InsuredGroupNameId = entity.InsuredGroupNameId,
                TreatyPricingGroupMasterLetterId = entity.TreatyPricingGroupMasterLetterId,
                IndustryNamePickListDetailId = entity.IndustryNamePickListDetailId,
                ReferredTypePickListDetailId = entity.ReferredTypePickListDetailId,

                Status = entity.Status,
                WorkflowStatus = entity.WorkflowStatus,
                PrimaryTreatyPricingProductId = entity.PrimaryTreatyPricingProductId,
                PrimaryTreatyPricingProductVersionId = entity.PrimaryTreatyPricingProductVersionId,
                SecondaryTreatyPricingProductId = entity.SecondaryTreatyPricingProductId,
                SecondaryTreatyPricingProductVersionId = entity.SecondaryTreatyPricingProductVersionId,

                FirstReferralDate = entity.FirstReferralDate,
                CoverageStartDate = entity.CoverageStartDate,
                CoverageEndDate = entity.CoverageEndDate,

                PolicyNumber = entity.PolicyNumber,
                WonVersion = entity.WonVersion,

                HasRiGroupSlip = entity.HasRiGroupSlip,
                RiGroupSlipCode = entity.RiGroupSlipCode,
                RiGroupSlipStatus = entity.RiGroupSlipStatus,
                RiGroupSlipPersonInChargeId = entity.RiGroupSlipPersonInChargeId,
                RiGroupSlipConfirmationDate = entity.RiGroupSlipConfirmationDate,
                RiGroupSlipVersionId = entity.RiGroupSlipVersionId,
                RiGroupSlipTemplateId = entity.RiGroupSlipTemplateId,
                RiGroupSlipSharePointLink = entity.RiGroupSlipSharePointLink,
                RiGroupSlipSharePointFolderPath = entity.RiGroupSlipSharePointFolderPath,

                QuotationPath = entity.QuotationPath,
                ReplyVersionId = entity.ReplyVersionId,
                ReplyTemplateId = entity.ReplyTemplateId,
                ReplySharePointLink = entity.ReplySharePointLink,
                ReplySharePointFolderPath = entity.ReplySharePointFolderPath,
            };
        }

        public static TreatyPricingGroupReferralBo FormBoML(TreatyPricingGroupReferral entity = null)
        {
            if (entity == null)
                return null;

            return new TreatyPricingGroupReferralBo
            {
                Id = entity.Id,
                CedantId = entity.CedantId,
                CedantBo = CedantService.Find(entity.CedantId),
                Code = entity.Code,
                Description = entity.Description,
                InsuredGroupNameId = entity.InsuredGroupNameId,
                InsuredGroupNameBo = InsuredGroupNameService.Find(entity.InsuredGroupNameId),

                CoverageStartDate = entity.CoverageStartDate,
                CoverageStartDateStr = entity.CoverageStartDate?.ToString(Util.GetDateFormat()),
                CoverageEndDate = entity.CoverageEndDate,
                CoverageEndDateStr = entity.CoverageEndDate?.ToString(Util.GetDateFormat()),

                RiGroupSlipCode = entity.RiGroupSlipCode,
                RiGroupSlipStatus = entity.RiGroupSlipStatus,
            };
        }

        public static IList<TreatyPricingGroupReferralBo> FormBoMLs(IList<TreatyPricingGroupReferral> entities = null)
        {
            if (entities == null)
                return null;
            IList<TreatyPricingGroupReferralBo> bos = new List<TreatyPricingGroupReferralBo>() { };
            foreach (TreatyPricingGroupReferral entity in entities)
            {
                bos.Add(FormBoML(entity));
            }
            return bos;
        }

        public static TreatyPricingGroupReferralBo FormBoForReport(TreatyPricingGroupReferral entity = null)
        {
            if (entity == null)
                return null;

            return new TreatyPricingGroupReferralBo
            {
                Id = entity.Id,
                CedantId = entity.CedantId,
                Code = entity.Code,
                Description = entity.Description,
                CedantBo = CedantService.Find(entity.CedantId),

                InsuredGroupNameId = entity.InsuredGroupNameId,

                CoverageStartDate = entity.CoverageStartDate,
                CoverageStartDateStr = entity.CoverageStartDate?.ToString(Util.GetDateFormat())
            };
        }

        public static TreatyPricingGroupReferralBo FormBo(TreatyPricingGroupReferral entity = null, bool foreign = true)
        {
            if (entity == null)
                return null;

            string primaryProductSelect = string.Format("{0}|{1}", entity.PrimaryTreatyPricingProductVersionId, entity.PrimaryTreatyPricingProductId);

            string secondaryProductSelect = null;
            if (entity.SecondaryTreatyPricingProductId.HasValue && entity.SecondaryTreatyPricingProductVersionId.HasValue)
            {
                secondaryProductSelect = string.Format("{0}|{1}", entity.SecondaryTreatyPricingProductVersionId, entity.SecondaryTreatyPricingProductId);
            }

            return new TreatyPricingGroupReferralBo
            {
                Id = entity.Id,
                CedantId = entity.CedantId,
                CedantBo = foreign ? CedantService.Find(entity.CedantId) : null,
                Code = entity.Code,
                Description = entity.Description,

                RiArrangementPickListDetailId = entity.RiArrangementPickListDetailId,
                InsuredGroupNameId = entity.InsuredGroupNameId,
                InsuredGroupNameBo = InsuredGroupNameService.Find(entity.InsuredGroupNameId),

                Status = entity.Status,
                WorkflowStatus = entity.WorkflowStatus,
                PrimaryTreatyPricingProductId = entity.PrimaryTreatyPricingProductId,
                PrimaryTreatyPricingProductVersionId = entity.PrimaryTreatyPricingProductVersionId,
                PrimaryTreatyPricingProductSelect = primaryProductSelect,
                //PrimaryTreatyPricingProductBo = foreign ? TreatyPricingProductService.Find(entity.PrimaryTreatyPricingProductId) : null,
                SecondaryTreatyPricingProductId = entity.SecondaryTreatyPricingProductId,
                SecondaryTreatyPricingProductVersionId = entity.SecondaryTreatyPricingProductVersionId,
                SecondaryTreatyPricingProductSelect = secondaryProductSelect,
                //SecondaryTreatyPricingProductBo = foreign ? TreatyPricingProductService.Find(entity.SecondaryTreatyPricingProductId) : null,

                FirstReferralDate = entity.FirstReferralDate,
                FirstReferralDateStr = entity.FirstReferralDate?.ToString(Util.GetDateFormat()),
                CoverageStartDate = entity.CoverageStartDate,
                CoverageStartDateStr = entity.CoverageStartDate?.ToString(Util.GetDateFormat()),
                CoverageEndDate = entity.CoverageEndDate,
                CoverageEndDateStr = entity.CoverageEndDate?.ToString(Util.GetDateFormat()),

                IndustryNamePickListDetailId = entity.IndustryNamePickListDetailId,
                ReferredTypePickListDetailId = entity.ReferredTypePickListDetailId,

                PolicyNumber = entity.PolicyNumber,
                WonVersion = entity.WonVersion,

                HasRiGroupSlip = entity.HasRiGroupSlip,
                RiGroupSlipCode = entity.RiGroupSlipCode,
                RiGroupSlipStatus = entity.RiGroupSlipStatus,
                RiGroupSlipPersonInChargeId = entity.RiGroupSlipPersonInChargeId,
                RiGroupSlipConfirmationDate = entity.RiGroupSlipConfirmationDate,
                RiGroupSlipConfirmationDateStr = entity.RiGroupSlipConfirmationDate?.ToString(Util.GetDateFormat()),
                RiGroupSlipVersionId = entity.RiGroupSlipVersionId,
                RiGroupSlipTemplateId = entity.RiGroupSlipTemplateId,
                RiGroupSlipSharePointLink = entity.RiGroupSlipSharePointLink,
                RiGroupSlipSharePointFolderPath = entity.RiGroupSlipSharePointFolderPath,

                QuotationPath = entity.QuotationPath,
                ReplyVersionId = entity.ReplyVersionId,
                ReplyTemplateId = entity.ReplyTemplateId,
                ReplySharePointLink = entity.ReplySharePointLink,
                ReplySharePointFolderPath = entity.ReplySharePointFolderPath,

                TreatyPricingGroupMasterLetterId = entity.TreatyPricingGroupMasterLetterId,
                TreatyPricingGroupMasterLetterBo = foreign ? TreatyPricingGroupMasterLetterService.Find(entity.TreatyPricingGroupMasterLetterId) : null,

                CreatedById = entity.CreatedById,
                UpdatedById = entity.UpdatedById,

                //TreatyPricingGroupReferralVersionBos = foreign ? TreatyPricingGroupReferralVersionService.GetByTreatyPricingGroupReferralId(entity.Id) : null,
                TreatyPricingGroupReferralVersionBos = TreatyPricingGroupReferralVersionService.GetByTreatyPricingGroupReferralId(entity.Id),
            };
        }

        public static TreatyPricingGroupReferralBo FormBoForGroupDashboard(TreatyPricingGroupReferral entity = null, bool foreign = true)
        {
            if (entity == null)
                return null;

            return new TreatyPricingGroupReferralBo
            {
                Id = entity.Id,

                Status = entity.Status,
                WorkflowStatus = entity.WorkflowStatus,
                Code = entity.Code,
                Description = entity.Description,


                CedantId = entity.CedantId,
                CedantBo = foreign ? CedantService.Find(entity.CedantId) : null,

                InsuredGroupNameId = entity.InsuredGroupNameId,
                InsuredGroupNameBo = foreign ? InsuredGroupNameService.Find(entity.InsuredGroupNameId) : null,

                //QuotationTAT = entity.QuotationTAT,
                //InternalTAT = entity.InternalTAT
            };
        }

        public static TreatyPricingGroupReferralBo FormBoForGroupDashboardDetail(TreatyPricingGroupReferral entity = null, bool foreign = false)
        {
            if (entity == null)
                return null;

            return new TreatyPricingGroupReferralBo
            {
                Id = entity.Id,
                CedantBo = foreign ? CedantService.Find(entity.CedantId) : null,
                InsuredGroupNameBo = foreign ? InsuredGroupNameService.Find(entity.InsuredGroupNameId) : null,
                Code = entity.Code,
                Description = entity.Description,
                Status = entity.Status,
                WorkflowStatus = entity.WorkflowStatus,

                //QuotationTAT = entity.QuotationTAT,
                //InternalTAT = entity.InternalTAT
            };
        }

        public static TreatyPricingGroupReferralBo FormBoForProductAndBenefitDetailsComparison(TreatyPricingGroupReferral entity = null)
        {
            if (entity == null)
                return null;

            return new TreatyPricingGroupReferralBo
            {
                Id = entity.Id,
                Code = entity.Code,
                Description = entity.Description,
                ReferredTypePickListDetailId = entity.ReferredTypePickListDetailId,
                ReferredTypePickListDetailBo = PickListDetailService.Find(entity.ReferredTypePickListDetailId),

                InsuredGroupNameId = entity.InsuredGroupNameId,
                InsuredGroupNameBo = InsuredGroupNameService.Find(entity.InsuredGroupNameId)
            };
        }

        public static TreatyPricingGroupReferralBo FormBoForHipsReport(TreatyPricingGroupReferral entity = null)
        {
            if (entity == null)
                return null;

            return new TreatyPricingGroupReferralBo
            {
                Id = entity.Id,
                CedantId = entity.CedantId,
                CedantBo = CedantService.Find(entity.CedantId),
                Code = entity.Code
            };
        }

        public static IList<TreatyPricingGroupReferralBo> FormBosForReport(IList<TreatyPricingGroupReferral> entities = null)
        {
            if (entities == null)
                return null;
            IList<TreatyPricingGroupReferralBo> bos = new List<TreatyPricingGroupReferralBo>() { };
            foreach (TreatyPricingGroupReferral entity in entities)
            {
                bos.Add(FormBoForReport(entity));
            }
            return bos;
        }

        public static IList<TreatyPricingGroupReferralBo> FormBos(IList<TreatyPricingGroupReferral> entities = null, bool foreign = true)
        {
            if (entities == null)
                return null;
            IList<TreatyPricingGroupReferralBo> bos = new List<TreatyPricingGroupReferralBo>() { };
            foreach (TreatyPricingGroupReferral entity in entities)
            {
                bos.Add(FormBo(entity, foreign));
            }
            return bos;
        }

        public static TreatyPricingGroupReferral FormEntity(TreatyPricingGroupReferralBo bo = null)
        {
            if (bo == null)
                return null;
            return new TreatyPricingGroupReferral
            {
                Id = bo.Id,
                CedantId = bo.CedantId,
                Code = bo.Code,
                Description = bo.Description,
                RiArrangementPickListDetailId = bo.RiArrangementPickListDetailId,
                InsuredGroupNameId = bo.InsuredGroupNameId,
                Status = bo.Status,
                WorkflowStatus = bo.WorkflowStatus,
                PrimaryTreatyPricingProductId = bo.PrimaryTreatyPricingProductId,
                PrimaryTreatyPricingProductVersionId = bo.PrimaryTreatyPricingProductVersionId,
                SecondaryTreatyPricingProductId = bo.SecondaryTreatyPricingProductId,
                SecondaryTreatyPricingProductVersionId = bo.SecondaryTreatyPricingProductVersionId,
                PolicyNumber = bo.PolicyNumber,
                FirstReferralDate = bo.FirstReferralDate,
                CoverageStartDate = bo.CoverageStartDate,
                CoverageEndDate = bo.CoverageEndDate,
                IndustryNamePickListDetailId = bo.IndustryNamePickListDetailId,
                ReferredTypePickListDetailId = bo.ReferredTypePickListDetailId,
                WonVersion = bo.WonVersion,
                HasRiGroupSlip = bo.HasRiGroupSlip,
                RiGroupSlipCode = bo.RiGroupSlipCode,
                RiGroupSlipStatus = bo.RiGroupSlipStatus,
                RiGroupSlipPersonInChargeId = bo.RiGroupSlipPersonInChargeId,
                RiGroupSlipConfirmationDate = bo.RiGroupSlipConfirmationDate,
                RiGroupSlipVersionId = bo.RiGroupSlipVersionId,
                RiGroupSlipTemplateId = bo.RiGroupSlipTemplateId,
                RiGroupSlipSharePointLink = bo.RiGroupSlipSharePointLink,
                RiGroupSlipSharePointFolderPath = bo.RiGroupSlipSharePointFolderPath,
                QuotationPath = bo.QuotationPath,
                ReplyVersionId = bo.ReplyVersionId,
                ReplyTemplateId = bo.ReplyTemplateId,
                ReplySharePointLink = bo.ReplySharePointLink,
                ReplySharePointFolderPath = bo.ReplySharePointFolderPath,
                TreatyPricingGroupMasterLetterId = bo.TreatyPricingGroupMasterLetterId,
                AverageScore = bo.AverageScore,

                CreatedById = bo.CreatedById,
                UpdatedById = bo.UpdatedById,
            };
        }

        public static bool IsExists(int id)
        {
            return TreatyPricingGroupReferral.IsExists(id);
        }

        public static TreatyPricingGroupReferralBo FindForReport(int id)
        {
            return FormBoForReport(TreatyPricingGroupReferral.Find(id));
        }

        public static TreatyPricingGroupReferralBo Find(int id, bool foreign = true)
        {
            return FormBo(TreatyPricingGroupReferral.Find(id), foreign);
        }

        public static TreatyPricingGroupReferralBo FindForGroupDashboard(int id, bool foreign = true)
        {
            return FormBoForGroupDashboard(TreatyPricingGroupReferral.Find(id), foreign);
        }

        public static TreatyPricingGroupReferralBo FindForGroupDashboardDetail(int id)
        {
            return FormBoForGroupDashboardDetail(TreatyPricingGroupReferral.Find(id), true);
        }

        public static TreatyPricingGroupReferralBo FindForProductAndBenefitDetailsComparison(int id, bool foreign = true)
        {
            return FormBoForProductAndBenefitDetailsComparison(TreatyPricingGroupReferral.Find(id));
        }

        public static TreatyPricingGroupReferralBo FindForHipsComparison(int id)
        {
            return FormBoForHipsReport(TreatyPricingGroupReferral.Find(id));
        }

        public static TreatyPricingGroupReferralBo FindByCode(string code)
        {
            using (var db = new AppDbContext())
            {
                return FormBo(db.TreatyPricingGroupReferrals.Where(q => q.Code == code).FirstOrDefault(), false);
            }
        }

        public static IList<TreatyPricingGroupReferralBo> Get()
        {
            using (var db = new AppDbContext())
            {
                return FormBos(db.TreatyPricingGroupReferrals.OrderBy(q => q.Code).ToList());
            }
        }

        public static IList<TreatyPricingGroupReferralBo> GetByCedantId(int? cedantId, bool foreign = true)
        {
            using (var db = new AppDbContext())
            {
                return FormBos(db.TreatyPricingGroupReferrals
                    .Where(q => q.CedantId == cedantId)
                    .OrderBy(q => q.Code)
                    .ToList(), foreign);
            }
        }

        public static IList<TreatyPricingGroupReferralBo> GetByCedantIdForReport(int? cedantId)
        {
            using (var db = new AppDbContext())
            {
                return FormBosForReport(db.TreatyPricingGroupReferrals
                    .Where(q => q.CedantId == cedantId)
                    .OrderBy(q => q.Code)
                    .ToList());
            }
        }

        public static IList<TreatyPricingGroupReferralBo> GetByCedantIdHasRiGroupSlipId(int cedantId)
        {
            using (var db = new AppDbContext())
            {
                return FormBoMLs(db.TreatyPricingGroupReferrals
                    .Where(q => q.CedantId == cedantId)
                    .Where(q => !string.IsNullOrEmpty(q.RiGroupSlipCode))
                    .Where(q => !q.TreatyPricingGroupMasterLetterId.HasValue)
                    .OrderBy(q => q.Code)
                    .ToList());
            }
        }

        public static IList<TreatyPricingGroupReferralBo> GetByGroupMasterLetterIds(int groupMasterLetterId, List<int> ids = null)
        {
            using (var db = new AppDbContext())
            {
                var query = db.TreatyPricingGroupReferrals
                    .Where(q => q.TreatyPricingGroupMasterLetterId == groupMasterLetterId);

                if (ids != null)
                    query = query.Where(q => !ids.Contains(q.Id));

                return FormBos(query.ToList());
            }
        }

        public static IList<TreatyPricingGroupReferralBo> GetByTreatyPricingProductId(int productId)
        {
            using (var db = new AppDbContext())
            {
                var query = db.TreatyPricingGroupReferrals
                    .Where(q => q.PrimaryTreatyPricingProductId == productId || q.SecondaryTreatyPricingProductId == productId);

                return FormBoMLs(query.ToList());
            }
        }

        public static List<int> GetAllWhereStatusQuoting()
        {
            using (var db = new AppDbContext(false))
            {
                return db.TreatyPricingGroupReferrals
                    .Where(q => q.Status == TreatyPricingGroupReferralBo.StatusQuoting)
                    .Select(q => q.Id)
                    .ToList();
            }
        }

        public static int CountByCedantId(int cedantId)
        {
            using (var db = new AppDbContext())
            {
                return db.TreatyPricingGroupReferrals.Where(q => q.CedantId == cedantId).Count();
            }
        }

        public static int CountByCedantIdHasRiGroupSlip(int cedantId, bool hasRiGroupSlip)
        {
            using (var db = new AppDbContext())
            {
                return db.TreatyPricingGroupReferrals.Where(q => q.CedantId == cedantId && q.HasRiGroupSlip == hasRiGroupSlip).Count();
            }
        }

        public static string GetNextGroupReferralCode(int cedantId)
        {
            using (var db = new AppDbContext())
            {
                DateTime today = DateTime.Today;
                var CedantBo = CedantService.Find(cedantId);

                string prefix = string.Format("{0}-GR-{1}-", CedantBo.Code, today.Year);
                TreatyPricingGroupReferral treatyPricingGroupReferral = db.TreatyPricingGroupReferrals.Where(q => q.Code.StartsWith(prefix) && q.CedantId == cedantId).OrderByDescending(q => q.Code).FirstOrDefault();

                int count = 0;
                if (treatyPricingGroupReferral != null)
                {
                    string entryNo = treatyPricingGroupReferral.Code;
                    string[] entryNoInfo = entryNo.Split('-');

                    if (entryNoInfo.Length == 4 || entryNoInfo.Length == 5)
                    {
                        string countStr = entryNoInfo[3];
                        int.TryParse(countStr, out count);
                    }
                }
                count++;
                string nextCountStr = count.ToString("D4");

                return prefix + nextCountStr;
            }
        }

        public static string GetNextRiGroupSlipCode(int cedantId)
        {
            using (var db = new AppDbContext())
            {
                DateTime today = DateTime.Today;
                var CedantBo = CedantService.Find(cedantId);

                string prefix = string.Format("{0}-{1}-", CedantBo.Code, today.Year);
                TreatyPricingGroupReferral treatyPricingGroupReferral = db.TreatyPricingGroupReferrals.Where(q => !string.IsNullOrEmpty(q.RiGroupSlipCode) && q.RiGroupSlipCode.StartsWith(prefix)).OrderByDescending(q => q.RiGroupSlipCode).FirstOrDefault();

                int count = 0;
                if (treatyPricingGroupReferral != null)
                {
                    string entryNo = treatyPricingGroupReferral.RiGroupSlipCode;
                    string[] entryNoInfo = entryNo.Split('-');

                    if (entryNoInfo.Length == 3)
                    {
                        string countStr = entryNoInfo[2];
                        int.TryParse(countStr, out count);
                    }
                }
                count++;
                string nextCountStr = count.ToString("D4");

                return prefix + nextCountStr;
            }
        }

        public static int CountByInsuredGroupNameId(int insuredGroupNameId)
        {
            using (var db = new AppDbContext())
            {
                return db.TreatyPricingGroupReferrals.Where(q => q.InsuredGroupNameId == insuredGroupNameId).Count();
            }
        }

        public static int CountByTemplateId(int templateId)
        {
            using (var db = new AppDbContext())
            {
                return db.TreatyPricingGroupReferrals.Where(q => q.ReplyTemplateId == templateId || q.RiGroupSlipTemplateId == templateId).Count();
            }
        }

        //public static int CountByIds(IList<int> ids)
        //{
        //    using (var db = new AppDbContext())
        //    {
        //        return db.TreatyPricingGroupReferrals.Where(q => ids.Contains(q.Id))
        //            .Where(q => q.InternalTAT > 3).Count();
        //    }
        //}

        public static Result Create(ref TreatyPricingGroupReferralBo bo)
        {
            TreatyPricingGroupReferral entity = FormEntity(bo);

            Result result = Result();
            if (result.Valid)
            {
                result.DataTrail = entity.Create();
                bo.Id = entity.Id;
            }

            return result;
        }

        public static Result Create(ref TreatyPricingGroupReferralBo bo, ref TrailObject trail)
        {
            Result result = Create(ref bo);
            if (result.Valid)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static Result Update(ref TreatyPricingGroupReferralBo bo)
        {
            Result result = Result();

            TreatyPricingGroupReferral entity = TreatyPricingGroupReferral.Find(bo.Id);
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

        public static Result Update(ref TreatyPricingGroupReferralBo bo, ref TrailObject trail)
        {
            Result result = Update(ref bo);
            if (result.Valid && result.DataTrail != null)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static void Delete(TreatyPricingGroupReferralBo bo)
        {
            TreatyPricingGroupReferral.Delete(bo.Id);
        }

        public static Result Delete(TreatyPricingGroupReferralBo bo, ref TrailObject trail)
        {
            Result result = Result();

            // TODO: In Use Validation

            if (result.Valid)
            {
                DataTrail dataTrail = TreatyPricingGroupReferral.Delete(bo.Id);
                dataTrail.Merge(ref trail, result.Table);
            }

            return result;
        }

        public static void UpdateGroupMasterLetterIdExcept(int groupMasterLetterId, List<int> ids, ref TrailObject trail)
        {
            foreach (var bo in GetByGroupMasterLetterIds(groupMasterLetterId, ids))
            {
                var GroupReferralBo = bo;
                using (var db = new AppDbContext())
                {
                    db.Database.ExecuteSqlCommand("UPDATE [TreatyPricingGroupReferrals] SET [TreatyPricingGroupMasterLetterId] = {0}, [UpdatedById] = {1}, [UpdatedAt] = {2} WHERE [Id] = {3}",
                        null, bo.UpdatedById ?? bo.CreatedById, DateTime.Now, bo.Id);
                    db.SaveChanges();
                }
                //GroupReferralBo.TreatyPricingGroupMasterLetterId = null;
                //Update(ref GroupReferralBo, ref trail);
            }
        }

        public static int? GenerateWorkflowStatus(string checklists, TreatyPricingGroupReferralVersionBo verBo, TreatyPricingGroupReferralBo groupReferralBo)
        {
            int? workflowStatus = TreatyPricingGroupReferralBo.WorkflowStatusQuoting;

            var checklistBos = new List<TreatyPricingGroupReferralChecklistBo>();
            if (!string.IsNullOrEmpty(checklists))
                checklistBos = Newtonsoft.Json.JsonConvert.DeserializeObject<List<TreatyPricingGroupReferralChecklistBo>>(checklists);

            if (checklistBos != null)
            {
                checklistBos.ForEach(a => a.TreatyPricingGroupReferralVersionBo = verBo);

                var requestedChecklist = checklistBos.Where(a => a.TreatyPricingGroupReferralVersionBo.EnquiryToClientDate.HasValue && !a.TreatyPricingGroupReferralVersionBo.ClientReplyDate.HasValue).ToList();
                if (requestedChecklist.Count > 0)
                {
                    workflowStatus = TreatyPricingGroupReferralBo.WorkflowStatusPendingClient;
                }
                else
                {
                    var groupTeamPendingChecklist = checklistBos.Where(a => a.InternalTeam == TreatyPricingGroupReferralChecklistBo.DefaultInternalTeamGroup && a.Status == TreatyPricingGroupReferralChecklistBo.StatusPendingApproval).ToList();
                    if (groupTeamPendingChecklist.Count > 0)
                    {
                        workflowStatus = TreatyPricingGroupReferralBo.WorkflowStatusPendingGroupTeamsApproval;
                    }
                    var groupTeamCompleteChecklist = checklistBos.Where(a => a.InternalTeam == TreatyPricingGroupReferralChecklistBo.DefaultInternalTeamGroup && (a.Status == TreatyPricingGroupReferralChecklistBo.StatusApproved || a.Status == TreatyPricingGroupReferralChecklistBo.StatusRejected)).ToList();
                    if (groupTeamCompleteChecklist.Count > 0)
                    {
                        workflowStatus = TreatyPricingGroupReferralBo.WorkflowStatusGroupTeamApprovedRejected;
                    }

                    var reviewerPendingChecklist = checklistBos.Where(a => a.InternalTeam == TreatyPricingGroupReferralChecklistBo.DefaultInternalTeamReviewer && a.Status == TreatyPricingGroupReferralChecklistBo.StatusPendingApproval).ToList();
                    if (reviewerPendingChecklist.Count > 0)
                    {
                        workflowStatus = TreatyPricingGroupReferralBo.WorkflowStatusPendingReviewersApproval;
                    }
                    var reviewerCompleteChecklist = checklistBos.Where(a => a.InternalTeam == TreatyPricingGroupReferralChecklistBo.DefaultInternalTeamReviewer && (a.Status == TreatyPricingGroupReferralChecklistBo.StatusApproved || a.Status == TreatyPricingGroupReferralChecklistBo.StatusRejected)).ToList();
                    if (reviewerCompleteChecklist.Count > 0)
                    {
                        workflowStatus = TreatyPricingGroupReferralBo.WorkflowStatusReviewerApprovedRejected;
                    }

                    var hodPendingChecklist = checklistBos.Where(a => a.InternalTeam == TreatyPricingGroupReferralChecklistBo.DefaultInternalTeamHOD && a.Status == TreatyPricingGroupReferralChecklistBo.StatusPendingApproval).ToList();
                    if (hodPendingChecklist.Count > 0)
                    {
                        workflowStatus = TreatyPricingGroupReferralBo.WorkflowStatusPendingHODsApproval;
                    }
                    var hodCompleteChecklist = checklistBos.Where(a => a.InternalTeam == TreatyPricingGroupReferralChecklistBo.DefaultInternalTeamHOD && (a.Status == TreatyPricingGroupReferralChecklistBo.StatusApproved || a.Status == TreatyPricingGroupReferralChecklistBo.StatusRejected)).ToList();
                    if (hodCompleteChecklist.Count > 0)
                    {
                        workflowStatus = TreatyPricingGroupReferralBo.WorkflowStatusHODApprovedRejected;
                    }

                    var ceoPendingChecklist = checklistBos.Where(a => a.InternalTeam == TreatyPricingGroupReferralChecklistBo.DefaultInternalTeamCEO && a.Status == TreatyPricingGroupReferralChecklistBo.StatusPendingApproval).ToList();
                    if (ceoPendingChecklist.Count > 0)
                    {
                        workflowStatus = TreatyPricingGroupReferralBo.WorkflowStatusPendingCEOsApproval;
                    }
                    var ceoCompleteChecklist = checklistBos.Where(a => a.InternalTeam == TreatyPricingGroupReferralChecklistBo.DefaultInternalTeamCEO && (a.Status == TreatyPricingGroupReferralChecklistBo.StatusApproved || a.Status == TreatyPricingGroupReferralChecklistBo.StatusRejected)).ToList();
                    if (ceoCompleteChecklist.Count > 0)
                    {
                        workflowStatus = TreatyPricingGroupReferralBo.WorkflowStatusCEOApprovedRejected;
                    }
                }

                var quotationSentDateFilled = checklistBos.Where(a => a.TreatyPricingGroupReferralVersionBo.QuotationSentDate != null).ToList();
                if (quotationSentDateFilled.Count > 0)
                {
                    workflowStatus = TreatyPricingGroupReferralBo.WorkflowStatusQuotationSent;
                }

                if (groupReferralBo.Status == TreatyPricingGroupReferralBo.StatusWon && groupReferralBo.HasRiGroupSlip == true)
                {
                    workflowStatus = TreatyPricingGroupReferralBo.WorkflowStatusPrepareRIGroupSlip;
                }

                if (groupReferralBo.Status == TreatyPricingGroupReferralBo.StatusLoss || (groupReferralBo.Status == TreatyPricingGroupReferralBo.StatusWon && groupReferralBo.HasRiGroupSlip == false) || (groupReferralBo.Status == TreatyPricingGroupReferralBo.StatusWon && groupReferralBo.RiGroupSlipStatus == TreatyPricingGroupReferralBo.RiGroupSlipStatusCompleted))
                {
                    workflowStatus = TreatyPricingGroupReferralBo.WorkflowStatusCompleted;
                }

            }

            return workflowStatus;
        }

        public static string GetSharePointPath(TreatyPricingGroupReferralBo bo)
        {
            string sharePointPath = bo.CedantBo.Code + "/" + bo.Code;

            return sharePointPath;
        }
    }

}
