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
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Services.TreatyPricing
{
    public class TreatyPricingGroupReferralVersionService
    {
        public static Result Result()
        {
            return new Result
            {
                Table = UtilAttribute.GetTableName(typeof(TreatyPricingGroupReferralVersion)),
                Controller = ModuleBo.ModuleController.TreatyPricingGroupReferralVersion.ToString()
            };
        }

        public static Expression<Func<TreatyPricingGroupReferralVersion, TreatyPricingGroupReferralVersionBo>> Expression()
        {
            return entity => new TreatyPricingGroupReferralVersionBo
            {
                Id = entity.TreatyPricingGroupReferralId,
                GroupReferralCode = entity.TreatyPricingGroupReferral.Code,
                GroupReferralDescription = entity.TreatyPricingGroupReferral.Description,
                CedantId = entity.TreatyPricingGroupReferral.CedantId,
                GroupReferralStatus = entity.TreatyPricingGroupReferral.Status,
                InsuredGroupNameId = entity.TreatyPricingGroupReferral.InsuredGroupNameId,
                IndustryNamePickListDetailId = entity.TreatyPricingGroupReferral.IndustryNamePickListDetailId,
                ReferredTypePickListDetailId = entity.TreatyPricingGroupReferral.ReferredTypePickListDetailId,
                PolicyNumber = entity.TreatyPricingGroupReferral.PolicyNumber,

                RequestTypePickListDetailId = entity.RequestTypePickListDetailId,
                GroupSize = entity.GroupSize,
                GrossRiskPremium = entity.GrossRiskPremium,
                ReinsurancePremium = entity.ReinsurancePremium,
                Version = entity.Version,
                GroupReferralPersonInChargeId = entity.GroupReferralPersonInChargeId,
                CedantPersonInCharge = entity.CedantPersonInCharge,
                QuotationTAT = entity.QuotationTAT,
                InternalTAT = entity.InternalTAT,
                AverageScore = entity.TreatyPricingGroupReferral.AverageScore,
                WorkflowStatusId = entity.TreatyPricingGroupReferral.WorkflowStatus,

                FirstReferralDate = entity.TreatyPricingGroupReferral.FirstReferralDate,
                CoverageStartDate = entity.TreatyPricingGroupReferral.CoverageStartDate,
                CoverageEndDate = entity.TreatyPricingGroupReferral.CoverageEndDate,
                RequestReceivedDate = entity.RequestReceivedDate,

                HasRiGroupSlip = entity.TreatyPricingGroupReferral.HasRiGroupSlip,
                WonVersion = entity.TreatyPricingGroupReferral.WonVersion,
                RiGroupSlipPersonInChargeId = entity.TreatyPricingGroupReferral.RiGroupSlipPersonInChargeId,
                RiGroupSlipConfirmationDate = entity.TreatyPricingGroupReferral.RiGroupSlipConfirmationDate,
                RiGroupSlipStatus = entity.TreatyPricingGroupReferral.RiGroupSlipStatus,
                RiGroupSlipCode = entity.TreatyPricingGroupReferral.RiGroupSlipCode,

                TreatyPricingGroupMasterLetterId = entity.TreatyPricingGroupReferral.TreatyPricingGroupMasterLetterId,

                ChecklistPendingUnderwriting = entity.ChecklistPendingUnderwriting,
                ChecklistPendingHealth = entity.ChecklistPendingHealth,
                ChecklistPendingClaims = entity.ChecklistPendingClaims,
                ChecklistPendingBD = entity.ChecklistPendingBD,
                ChecklistPendingCR = entity.ChecklistPendingCR,

                // Commission, Expense & Profit Margin get Version Id - will get the value during export
                CommissionMarginDEA = entity.Id.ToString(),
                CommissionMarginMSE = entity.Id.ToString(),
                ExpenseMarginDEA = entity.Id.ToString(),
                ExpenseMarginMSE = entity.Id.ToString(),
                ProfitMarginDEA = entity.Id.ToString(),
                ProfitMarginMSE = entity.Id.ToString(),
            };
        }

        public static TreatyPricingGroupReferralVersionBo FormBoForReport(TreatyPricingGroupReferralVersion entity = null)
        {
            if (entity == null)
                return null;
            return new TreatyPricingGroupReferralVersionBo
            {
                Id = entity.Id,
                TreatyPricingGroupReferralId = entity.TreatyPricingGroupReferralId,
                Version = entity.Version
            };
        }
        public static TreatyPricingGroupReferralVersionBo FormBo(TreatyPricingGroupReferralVersion entity = null, bool foreign = false)
        {
            if (entity == null)
                return null;
            return new TreatyPricingGroupReferralVersionBo
            {
                Id = entity.Id,
                TreatyPricingGroupReferralId = entity.TreatyPricingGroupReferralId,
                TreatyPricingGroupReferralBo = foreign ? TreatyPricingGroupReferralService.Find(entity.TreatyPricingGroupReferralId) : null,
                Version = entity.Version,
                GroupReferralPersonInChargeId = entity.GroupReferralPersonInChargeId,
                CedantPersonInCharge = entity.CedantPersonInCharge,
                RequestTypePickListDetailId = entity.RequestTypePickListDetailId,
                PremiumTypePickListDetailId = entity.PremiumTypePickListDetailId,
                GrossRiskPremium = entity.GrossRiskPremium,
                ReinsurancePremium = entity.ReinsurancePremium,
                GrossRiskPremiumGTL = entity.GrossRiskPremiumGTL,
                ReinsurancePremiumGTL = entity.ReinsurancePremiumGTL,
                GrossRiskPremiumGHS = entity.GrossRiskPremiumGHS,
                ReinsurancePremiumGHS = entity.ReinsurancePremiumGHS,
                AverageSumAssured = entity.AverageSumAssured,
                GroupSize = entity.GroupSize,
                IsCompulsoryOrVoluntary = entity.IsCompulsoryOrVoluntary,
                UnderwritingMethod = entity.UnderwritingMethod,
                Remarks = entity.Remarks,
                RequestReceivedDate = entity.RequestReceivedDate,
                EnquiryToClientDate = entity.EnquiryToClientDate,
                ClientReplyDate = entity.ClientReplyDate,
                QuotationSentDate = entity.QuotationSentDate,
                Score = entity.Score,
                HasPerLifeRetro = entity.HasPerLifeRetro,
                ChecklistRemark = entity.ChecklistRemark,
                QuotationTAT = entity.QuotationTAT,
                InternalTAT = entity.InternalTAT,
                QuotationValidityDate = entity.QuotationValidityDate,
                QuotationValidityDay = entity.QuotationValidityDay,
                FirstQuotationSentWeek = entity.FirstQuotationSentWeek,
                FirstQuotationSentMonth = entity.FirstQuotationSentMonth,
                FirstQuotationSentQuarter = entity.FirstQuotationSentQuarter,
                FirstQuotationSentYear = entity.FirstQuotationSentYear,

                RequestReceivedDateStr = entity.RequestReceivedDate?.ToString(Util.GetDateFormat()),
                EnquiryToClientDateStr = entity.EnquiryToClientDate?.ToString(Util.GetDateFormat()),
                ClientReplyDateStr = entity.ClientReplyDate?.ToString(Util.GetDateFormat()),
                QuotationSentDateStr = entity.QuotationSentDate?.ToString(Util.GetDateFormat()),
                QuotationValidityDateStr = entity.QuotationValidityDate?.ToString(Util.GetDateFormat()),

                GrossRiskPremiumStr = Util.DoubleToString(entity.GrossRiskPremium, 2),
                ReinsurancePremiumStr = Util.DoubleToString(entity.ReinsurancePremium, 2),
                GrossRiskPremiumGTLStr = Util.DoubleToString(entity.GrossRiskPremiumGTL, 2),
                ReinsurancePremiumGTLStr = Util.DoubleToString(entity.ReinsurancePremiumGTL, 2),
                GrossRiskPremiumGHSStr = Util.DoubleToString(entity.GrossRiskPremiumGHS, 2),
                ReinsurancePremiumGHSStr = Util.DoubleToString(entity.ReinsurancePremiumGHS, 2),
                AverageSumAssuredStr = Util.DoubleToString(entity.AverageSumAssured, 2),
                GroupSizeStr = Util.DoubleToString(entity.GroupSize, 0),

                CreatedById = entity.CreatedById,
                UpdatedById = entity.UpdatedById,

                TreatyPricingGroupReferralVersionBenefit = TreatyPricingGroupReferralVersionBenefitService.GetJsonByVersionId(entity.Id),
            };
        }

        public static TreatyPricingGroupReferralVersionBo FormBoForGroupDashboard(TreatyPricingGroupReferralVersion entity = null, bool foreign = false)
        {
            if (entity == null)
                return null;
            return new TreatyPricingGroupReferralVersionBo
            {
                Id = entity.Id,
                TreatyPricingGroupReferralId = entity.TreatyPricingGroupReferralId,
                TreatyPricingGroupReferralBo = foreign ? TreatyPricingGroupReferralService.FindForGroupDashboard(entity.TreatyPricingGroupReferralId, false) : null,
                GroupReferralPersonInChargeId = entity.GroupReferralPersonInChargeId,
                Version = entity.Version,
                QuotationSentDate = entity.QuotationSentDate,
                QuotationSentDateStr = entity.QuotationSentDate?.ToString(Util.GetDateFormat()),
                RequestReceivedDate = entity.RequestReceivedDate,
                Score = entity.Score,
                QuotationTAT = entity.QuotationTAT,
                InternalTAT = entity.InternalTAT
            };
        }

        public static TreatyPricingGroupReferralVersionBo FormBoForGroupDashboardDetail(TreatyPricingGroupReferralVersion entity = null, bool foreign = false)
        {
            if (entity == null)
                return null;
            return new TreatyPricingGroupReferralVersionBo
            {
                Id = entity.Id,
                TreatyPricingGroupReferralId = entity.TreatyPricingGroupReferralId,
                TreatyPricingGroupReferralBo = foreign ? TreatyPricingGroupReferralService.FindForGroupDashboardDetail(entity.TreatyPricingGroupReferralId) : null,
                GroupReferralPersonInChargeId = entity.GroupReferralPersonInChargeId,
                QuotationSentDate = entity.QuotationSentDate,
                QuotationSentDateStr = entity.QuotationSentDate?.ToString(Util.GetDateFormat()),
                RequestReceivedDate = entity.RequestReceivedDate,
                Score = entity.Score,
                Version = entity.Version
            };
        }



        public static TreatyPricingGroupReferralVersionBo FormBoForProductAndDetailsComparison(TreatyPricingGroupReferralVersion entity = null)
        {
            if (entity == null)
                return null;
            return new TreatyPricingGroupReferralVersionBo
            {
                Id = entity.Id,
                TreatyPricingGroupReferralId = entity.TreatyPricingGroupReferralId,
                TreatyPricingGroupReferralBo = TreatyPricingGroupReferralService.FindForProductAndBenefitDetailsComparison(entity.TreatyPricingGroupReferralId),
                Version = entity.Version,
                RequestTypePickListDetailId = entity.RequestTypePickListDetailId,
                PremiumTypePickListDetailId = entity.PremiumTypePickListDetailId,
                GrossRiskPremium = entity.GrossRiskPremium,
                ReinsurancePremium = entity.ReinsurancePremium,
                GrossRiskPremiumGTL = entity.GrossRiskPremiumGTL,
                ReinsurancePremiumGTL = entity.ReinsurancePremiumGTL,
                GrossRiskPremiumGHS = entity.GrossRiskPremiumGHS,
                ReinsurancePremiumGHS = entity.ReinsurancePremiumGHS,
                AverageSumAssured = entity.AverageSumAssured,
                GroupSize = entity.GroupSize,
                IsCompulsoryOrVoluntary = entity.IsCompulsoryOrVoluntary,
                UnderwritingMethod = entity.UnderwritingMethod,
                Remarks = entity.Remarks
            };
        }

        public static TreatyPricingGroupReferralVersionBo FormBoForHipsComparison(TreatyPricingGroupReferralVersion entity = null)
        {
            if (entity == null)
                return null;
            return new TreatyPricingGroupReferralVersionBo
            {
                Id = entity.Id,
                TreatyPricingGroupReferralId = entity.TreatyPricingGroupReferralId,
                TreatyPricingGroupReferralBo = TreatyPricingGroupReferralService.FindForHipsComparison(entity.TreatyPricingGroupReferralId)
            };
        }

        public static IList<TreatyPricingGroupReferralVersionBo> FormBosForReport(IList<TreatyPricingGroupReferralVersion> entities = null)
        {
            if (entities == null)
                return null;
            IList<TreatyPricingGroupReferralVersionBo> bos = new List<TreatyPricingGroupReferralVersionBo>() { };
            foreach (TreatyPricingGroupReferralVersion entity in entities)
            {
                bos.Add(FormBoForReport(entity));
            }
            return bos;
        }

        public static IList<TreatyPricingGroupReferralVersionBo> FormBos(IList<TreatyPricingGroupReferralVersion> entities = null, bool foreign = false)
        {
            if (entities == null)
                return null;
            IList<TreatyPricingGroupReferralVersionBo> bos = new List<TreatyPricingGroupReferralVersionBo>() { };
            foreach (TreatyPricingGroupReferralVersion entity in entities)
            {
                bos.Add(FormBo(entity, foreign));
            }
            return bos;
        }

        public static IList<TreatyPricingGroupReferralVersionBo> FormBosForGroupDashboard(IList<TreatyPricingGroupReferralVersion> entities = null, bool foreign = false)
        {
            if (entities == null)
                return null;
            IList<TreatyPricingGroupReferralVersionBo> bos = new List<TreatyPricingGroupReferralVersionBo>() { };
            foreach (TreatyPricingGroupReferralVersion entity in entities)
            {
                bos.Add(FormBoForGroupDashboard(entity, foreign));
            }
            return bos;
        }

        public static TreatyPricingGroupReferralVersion FormEntity(TreatyPricingGroupReferralVersionBo bo = null)
        {
            if (bo == null)
                return null;
            return new TreatyPricingGroupReferralVersion
            {
                Id = bo.Id,
                TreatyPricingGroupReferralId = bo.TreatyPricingGroupReferralId,
                Version = bo.Version,
                GroupReferralPersonInChargeId = bo.GroupReferralPersonInChargeId,
                CedantPersonInCharge = bo.CedantPersonInCharge,
                RequestTypePickListDetailId = bo.RequestTypePickListDetailId,
                PremiumTypePickListDetailId = bo.PremiumTypePickListDetailId,
                GrossRiskPremium = bo.GrossRiskPremium,
                ReinsurancePremium = bo.ReinsurancePremium,
                GrossRiskPremiumGTL = bo.GrossRiskPremiumGTL,
                ReinsurancePremiumGTL = bo.ReinsurancePremiumGTL,
                GrossRiskPremiumGHS = bo.GrossRiskPremiumGHS,
                ReinsurancePremiumGHS = bo.ReinsurancePremiumGHS,
                AverageSumAssured = bo.AverageSumAssured,
                GroupSize = bo.GroupSize,
                IsCompulsoryOrVoluntary = bo.IsCompulsoryOrVoluntary,
                UnderwritingMethod = bo.UnderwritingMethod,
                Remarks = bo.Remarks,
                RequestReceivedDate = bo.RequestReceivedDate,
                EnquiryToClientDate = bo.EnquiryToClientDate,
                ClientReplyDate = bo.ClientReplyDate,
                QuotationSentDate = bo.QuotationSentDate,
                Score = bo.Score,
                HasPerLifeRetro = bo.HasPerLifeRetro,
                ChecklistRemark = bo.ChecklistRemark,
                ChecklistPendingUnderwriting = bo.ChecklistPendingUnderwriting,
                ChecklistPendingHealth = bo.ChecklistPendingHealth,
                ChecklistPendingClaims = bo.ChecklistPendingClaims,
                ChecklistPendingBD = bo.ChecklistPendingBD,
                ChecklistPendingCR = bo.ChecklistPendingCR,
                QuotationTAT = bo.QuotationTAT,
                InternalTAT = bo.InternalTAT,
                QuotationValidityDate = bo.QuotationValidityDate,
                QuotationValidityDay = bo.QuotationValidityDay,
                FirstQuotationSentWeek = bo.FirstQuotationSentWeek,
                FirstQuotationSentMonth = bo.FirstQuotationSentMonth,
                FirstQuotationSentQuarter = bo.FirstQuotationSentQuarter,
                FirstQuotationSentYear = bo.FirstQuotationSentYear,
                CreatedById = bo.CreatedById,
                UpdatedById = bo.UpdatedById,
            };
        }

        public static bool IsExists(int id)
        {
            return TreatyPricingGroupReferralVersion.IsExists(id);
        }

        public static TreatyPricingGroupReferralVersionBo FindForReport(int? id)
        {
            return FormBoForReport(TreatyPricingGroupReferralVersion.Find(id));
        }

        public static TreatyPricingGroupReferralVersionBo Find(int? id, bool foreign = false)
        {
            return FormBo(TreatyPricingGroupReferralVersion.Find(id), foreign);
        }

        public static TreatyPricingGroupReferralVersionBo FindForGroupDashboardDetail(int? id, bool foreign = false)
        {
            return FormBoForGroupDashboardDetail(TreatyPricingGroupReferralVersion.Find(id), foreign);
        }

        public static TreatyPricingGroupReferralVersionBo FindForGroupDashboard(int? id, bool foreign = false, int type = 1)
        {
            return FormBoForGroupDashboard(TreatyPricingGroupReferralVersion.Find(id), foreign);
        }

        public static TreatyPricingGroupReferralVersionBo FindForProductAndBenefitDetailComparison(int? id)
        {
            return FormBoForProductAndDetailsComparison(TreatyPricingGroupReferralVersion.Find(id));
        }

        public static TreatyPricingGroupReferralVersionBo FindForHipsComparison(int? id)
        {
            return FormBoForHipsComparison(TreatyPricingGroupReferralVersion.Find(id));
        }

        public static IList<TreatyPricingGroupReferralVersionBo> GetAll(bool foreign = false)
        {
            using (var db = new AppDbContext())
            {
                return FormBos(db.TreatyPricingGroupReferralVersions.ToList(), foreign);
            }
        }

        public static IList<TreatyPricingGroupReferralVersionBo> GetAllByYear(int year, bool foreign = false)
        {
            var yearStart = new DateTime(year, 1, 1);
            var yearEnd = new DateTime(year, 12, 31);
            using (var db = new AppDbContext())
            {
                return FormBosForGroupDashboard(db.TreatyPricingGroupReferralVersions
                .Where(q => q.RequestReceivedDate >= yearStart && q.RequestReceivedDate <= yearEnd)
                .ToList(), foreign);
            }
        }

        public static IList<TreatyPricingGroupReferralVersionBo> GetAllUnassignedForGroupDashboard(bool foreign = false)
        {
            using (var db = new AppDbContext())
            {
                return FormBosForGroupDashboard(db.TreatyPricingGroupReferralVersions
                    .Where(q => !q.GroupReferralPersonInChargeId.HasValue)
                .ToList(), foreign);
            }
        }

        public static TreatyPricingGroupReferralVersionBo FindByIdAndVersion(int? id, int ver)
        {
            using (var db = new AppDbContext())
            {
                return FormBoForReport(db.TreatyPricingGroupReferralVersions
                    .Where(q => q.TreatyPricingGroupReferralId == id)
                    .Where(q => q.Version == ver)
                    .FirstOrDefault());
            }
        }

        public static TreatyPricingGroupReferralVersionBo GetLatestVersionByTreatyPricingGroupReferralId(int treatyPricingGroupReferralId)
        {
            using (var db = new AppDbContext())
            {
                return FormBo(db.TreatyPricingGroupReferralVersions
                    .Where(q => q.TreatyPricingGroupReferralId == treatyPricingGroupReferralId)
                    .OrderByDescending(q => q.Version)
                    .FirstOrDefault());
            }
        }

        public static TreatyPricingGroupReferralVersionBo GetLatestVersionByTreatyPricingGroupReferralIdForGroupDashboard(int treatyPricingGroupReferralId)
        {
            using (var db = new AppDbContext())
            {
                return FormBoForGroupDashboard(db.TreatyPricingGroupReferralVersions
                    .Where(q => q.TreatyPricingGroupReferralId == treatyPricingGroupReferralId)
                    .OrderByDescending(q => q.Version)
                    .FirstOrDefault());
            }
        }

        public static IList<TreatyPricingGroupReferralVersionBo> GetByTreatyPricingGroupReferralId(int? treatyPricingGroupReferralId)
        {
            using (var db = new AppDbContext())
            {
                return FormBos(db.TreatyPricingGroupReferralVersions
                    .Where(q => q.TreatyPricingGroupReferralId == treatyPricingGroupReferralId)
                    //.OrderByDescending(q => q.Version)
                    .ToList());
            }
        }

        public static IList<TreatyPricingGroupReferralVersionBo> GetByTreatyPricingGroupReferralIdForReport(int? treatyPricingGroupReferralId)
        {
            using (var db = new AppDbContext())
            {
                return FormBosForReport(db.TreatyPricingGroupReferralVersions
                    .Where(q => q.TreatyPricingGroupReferralId == treatyPricingGroupReferralId)
                    //.OrderByDescending(q => q.Version)
                    .ToList());
            }
        }

        public static IList<TreatyPricingGroupReferralVersionBo> GetByTreatyPricingGroupReferralIdForGroupDashboard(int? treatyPricingGroupReferralId)
        {
            using (var db = new AppDbContext())
            {
                return FormBosForGroupDashboard(db.TreatyPricingGroupReferralVersions
                    .Where(q => q.TreatyPricingGroupReferralId == treatyPricingGroupReferralId)
                    //.OrderByDescending(q => q.Version)
                    .ToList());
            }
        }

        public static IList<TreatyPricingGroupReferralVersionBo> GetByTreatyPricingGroupReferralVersion(int version)
        {
            using (var db = new AppDbContext())
            {
                return FormBos(db.TreatyPricingGroupReferralVersions.Where(q => q.Id == version).ToList());
            }
        }

        public static IList<TreatyPricingGroupDashboardBo> GetDashboardTurnaroundTimeList()
        {
            //var date = DateTime.Today;
            //var dayCount = 0;

            //while (dayCount < day)
            //{
            //    if (date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday)
            //    {
            //        date = date.AddDays(-1);
            //        continue;
            //    }

            //    if (PublicHolidayDetailService.IsExists(date))
            //    {
            //        date = date.AddDays(-1);
            //        continue;
            //    }

            //    date = date.AddDays(-1);
            //    dayCount++;
            //}

            var bos = TreatyPricingGroupDashboardBo.GetDashboardTurnaroundTimeList();

            using (var db = new AppDbContext())
            {
                foreach (var bo in bos)
                {
                    //int cedantActiveCase = 0;
                    //int internalActiveCase = 0;

                    // calculate unassigned case only
                    if (!bo.TurnaroundTimeDay.HasValue && !bo.IsExceedDay.HasValue)
                    {
                        var treatyPricingGroupReferralBos = db.TreatyPricingGroupReferralVersions
                            .GroupBy(q => q.TreatyPricingGroupReferralId)
                            .Select(g => g.OrderByDescending(q => q.Version).FirstOrDefault())
                            .Where(q => !q.GroupReferralPersonInChargeId.HasValue)
                            .Select(q => q.TreatyPricingGroupReferral);
                    }
                }
            }
            return bos;
        }

        public static int GetVersionId(int treatyPricingGroupReferralId, int treatyPricingGroupReferralVersion)
        {
            using (var db = new AppDbContext())
            {
                return db.TreatyPricingGroupReferralVersions
                    .FirstOrDefault(q => q.TreatyPricingGroupReferralId == treatyPricingGroupReferralId
                        && q.Version == treatyPricingGroupReferralVersion).Id;
            }
        }

        // if the "Quotation Sent Date" is not filled yet, it be calculated as "Today's Date" first
        public static int? GenerateQuotationTat(TreatyPricingGroupReferralVersionBo verBo)
        {
            int? quotationTat = null;
            if (verBo.RequestReceivedDate.HasValue)
            {
                if (verBo.QuotationSentDate.HasValue)
                    quotationTat = CalculateDaysWithHolidays(verBo.RequestReceivedDate.Value, verBo.QuotationSentDate.Value);
                else
                    quotationTat = CalculateDaysWithHolidays(verBo.RequestReceivedDate.Value, DateTime.Today);
            }

            return quotationTat;
        }

        public static int? GenerateInternalTat(TreatyPricingGroupReferralVersionBo verBo)
        {
            int? internalTat = null;
            if (verBo.ClientReplyDate.HasValue && verBo.EnquiryToClientDate.HasValue && verBo.RequestReceivedDate.HasValue)
            {
                if (verBo.QuotationSentDate.HasValue)
                    internalTat = CalculateDaysWithHolidays(verBo.ClientReplyDate.Value, verBo.QuotationSentDate.Value) + CalculateDaysWithHolidays(verBo.RequestReceivedDate.Value, verBo.EnquiryToClientDate.Value);
                else
                    internalTat = CalculateDaysWithHolidays(verBo.ClientReplyDate.Value, DateTime.Today) + CalculateDaysWithHolidays(verBo.RequestReceivedDate.Value, verBo.EnquiryToClientDate.Value);
            }

            return internalTat;
        }

        public static DateTime? GenerateQuotationValidityDate(TreatyPricingGroupReferralVersionBo verBo)
        {
            DateTime? quotationValidityDate = null;
            if (verBo.QuotationSentDate.HasValue) quotationValidityDate = verBo.QuotationSentDate.Value;

            return quotationValidityDate;
        }

        public static int? GenerateFirstQuotationSentWeek(TreatyPricingGroupReferralVersionBo verBo)
        {
            CultureInfo cul = CultureInfo.CurrentCulture;
            int? firstQuotationSentWeek = null;

            if (verBo.QuotationSentDate.HasValue)
                firstQuotationSentWeek = cul.Calendar.GetWeekOfYear(verBo.QuotationSentDate.Value, CalendarWeekRule.FirstDay, DayOfWeek.Sunday);

            return firstQuotationSentWeek;
        }

        public static int? GenerateFirstQuotationSentMonth(TreatyPricingGroupReferralVersionBo verBo)
        {
            int? firstQuotationSentMonth = null;
            if (verBo.QuotationSentDate.HasValue) firstQuotationSentMonth = verBo.QuotationSentDate.Value.Month;

            return firstQuotationSentMonth;
        }

        public static string GenerateFirstQuotationSentQuarter(TreatyPricingGroupReferralVersionBo verBo)
        {
            int? firstQuotationSentMonth = null;
            if (verBo.QuotationSentDate.HasValue) firstQuotationSentMonth = GetQuarter(verBo.QuotationSentDate.Value);

            return firstQuotationSentMonth.HasValue ? string.Format("Q{0}", firstQuotationSentMonth) : null;
        }

        public static int? GenerateFirstQuotationSentYear(TreatyPricingGroupReferralVersionBo verBo)
        {
            int? firstQuotationSentYear = null;
            if (verBo.QuotationSentDate.HasValue) firstQuotationSentYear = verBo.QuotationSentDate.Value.Year;

            return firstQuotationSentYear;
        }

        public static double? CalculateAverageScore(int? treatyPricingGroupReferralId)
        {
            using (var db = new AppDbContext())
            {
                return db.TreatyPricingGroupReferralVersions
                    .Where(q => q.TreatyPricingGroupReferralId == treatyPricingGroupReferralId)
                    .Average(q => q.Score);
            }
        }

        public static Result Save(ref TreatyPricingGroupReferralVersionBo bo)
        {
            if (!TreatyPricingGroupReferralVersion.IsExists(bo.Id))
            {
                return Create(ref bo);
            }
            return Update(ref bo);
        }

        public Result Save(ref TreatyPricingGroupReferralVersionBo bo, ref TrailObject trail)
        {
            if (!TreatyPricingGroupReferralVersion.IsExists(bo.Id))
            {
                return Create(ref bo, ref trail);
            }
            return Update(ref bo, ref trail);
        }

        public static Result Create(ref TreatyPricingGroupReferralVersionBo bo)
        {
            TreatyPricingGroupReferralVersion entity = FormEntity(bo);

            Result result = Result();
            if (result.Valid)
            {
                result.DataTrail = entity.Create();
                bo.Id = entity.Id;
            }

            return result;
        }

        public static Result Create(ref TreatyPricingGroupReferralVersionBo bo, ref TrailObject trail)
        {
            Result result = Create(ref bo);
            if (result.Valid)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static Result Update(ref TreatyPricingGroupReferralVersionBo bo)
        {
            Result result = Result();

            TreatyPricingGroupReferralVersion entity = TreatyPricingGroupReferralVersion.Find(bo.Id);
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

        public static Result Update(ref TreatyPricingGroupReferralVersionBo bo, ref TrailObject trail)
        {
            Result result = Update(ref bo);
            if (result.Valid && result.DataTrail != null)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static void Delete(TreatyPricingGroupReferralVersionBo bo)
        {
            TreatyPricingGroupReferralVersion.Delete(bo.Id);
        }

        public static Result Delete(TreatyPricingGroupReferralVersionBo bo, ref TrailObject trail)
        {
            Result result = Result();

            // TODO: In Use Validation

            if (result.Valid)
            {
                DataTrail dataTrail = TreatyPricingGroupReferralVersion.Delete(bo.Id);
                dataTrail.Merge(ref trail, result.Table);
            }

            return result;
        }

        public static IList<TreatyPricingGroupReferralVersionBo> GetGroupOverallTatCount(int year)
        {
            using (var db = new AppDbContext())
            {
                var yearStart = new DateTime(year, 1, 1);
                var yearEnd = new DateTime(year, 12, 31);

                var versionBos = db.TreatyPricingGroupReferralVersions
                    //.GroupBy(p => p.TreatyPricingGroupReferralId)
                    //.Select(g => g.OrderByDescending(p => p.Version).FirstOrDefault())
                    .Where(q => q.RequestReceivedDate >= yearStart && q.RequestReceivedDate <= yearEnd)
                    .ToList();

                return FormBos(versionBos, true);
            }

        }

        public static int? GenerateScore(int? internalTAT)
        {
            int? score = null;
            int? calculation = null;

            if (internalTAT.HasValue)
                calculation = internalTAT;

            //if (verBo.QuotationSentDate.HasValue && verBo.ClientReplyDate.HasValue && verBo.EnquiryToClientDate.HasValue && verBo.RequestReceivedDate.HasValue)
            //{
            //    //calculation = (verBo.QuotationSentDate.Value - verBo.ClientReplyDate.Value).Days + (verBo.EnquiryToClientDate.Value - verBo.RequestReceivedDate.Value).Days;
            //    calculation = CalculateDaysWithHolidays(verBo.ClientReplyDate.Value, verBo.QuotationSentDate.Value) + CalculateDaysWithHolidays(verBo.RequestReceivedDate.Value, verBo.EnquiryToClientDate.Value);
            //}

            if (calculation != null)
            {
                switch (calculation)
                {
                    case 0:
                        score = 5;
                        break;
                    case 1:
                    case 2:
                        score = 4;
                        break;
                    case 3:
                        score = 3;
                        break;
                    case 4:
                    case 5:
                    case 6:
                        score = 2;
                        break;
                    default: // >= 7
                        score = 1;
                        break;
                }
            }

            return score;
        }
        public static IList<TreatyPricingGroupReferralVersionBo> GetByPendingPIC(int picId, bool foreign = false)
        {
            var currentYear = DateTime.Now.Year;
            var yearStart = new DateTime(currentYear, 1, 1);
            var yearEnd = new DateTime(currentYear, 12, 31);

            using (var db = new AppDbContext())
            {
                return FormBosForGroupDashboard(db.TreatyPricingGroupReferralVersions
                    .Where(q => q.RequestReceivedDate >= yearStart && q.RequestReceivedDate <= yearEnd)
                    .Where(q => q.GroupReferralPersonInChargeId == picId)
                    .Where(q => !q.QuotationSentDate.HasValue)
                    .ToList(), foreign);
            }
        }

        public static IList<TreatyPricingGroupReferralVersionBo> GetByTurnaroundTime(int type, bool foreign = false)
        {
            var currentYear = DateTime.Now.Year;
            var yearStart = new DateTime(currentYear, 1, 1);
            var yearEnd = new DateTime(currentYear, 12, 31);

            using (var db = new AppDbContext())
            {
                if (type == 1)
                {
                    return FormBosForGroupDashboard(db.TreatyPricingGroupReferralVersions
                    .Where(q => q.RequestReceivedDate >= yearStart && q.RequestReceivedDate <= yearEnd)
                    .Where(q => q.QuotationTAT == 0)
                    .Where(q => !q.QuotationSentDate.HasValue)
                    .ToList(), foreign);
                }
                else if (type == 2)
                {
                    return FormBosForGroupDashboard(db.TreatyPricingGroupReferralVersions
                    .Where(q => q.RequestReceivedDate >= yearStart && q.RequestReceivedDate <= yearEnd)
                    .Where(q => q.InternalTAT == 0)
                    .Where(q => !q.QuotationSentDate.HasValue)
                    .ToList(), foreign);
                }
                else if (type == 3)
                {
                    return FormBosForGroupDashboard(db.TreatyPricingGroupReferralVersions
                    .Where(q => q.RequestReceivedDate >= yearStart && q.RequestReceivedDate <= yearEnd)
                    .Where(q => q.QuotationTAT == 1)
                    .Where(q => !q.QuotationSentDate.HasValue)
                    .ToList(), foreign);
                }
                else if (type == 4)
                {
                    return FormBosForGroupDashboard(db.TreatyPricingGroupReferralVersions
                    .Where(q => q.RequestReceivedDate >= yearStart && q.RequestReceivedDate <= yearEnd)
                    .Where(q => q.InternalTAT == 1)
                    .Where(q => !q.QuotationSentDate.HasValue)
                    .ToList(), foreign);
                }
                else if (type == 5)
                {
                    return FormBosForGroupDashboard(db.TreatyPricingGroupReferralVersions
                    .Where(q => q.RequestReceivedDate >= yearStart && q.RequestReceivedDate <= yearEnd)
                    .Where(q => q.QuotationTAT == 2)
                    .Where(q => !q.QuotationSentDate.HasValue)
                    .ToList(), foreign);
                }
                else if (type == 6)
                {
                    return FormBosForGroupDashboard(db.TreatyPricingGroupReferralVersions
                    .Where(q => q.RequestReceivedDate >= yearStart && q.RequestReceivedDate <= yearEnd)
                    .Where(q => q.InternalTAT == 2)
                    .Where(q => !q.QuotationSentDate.HasValue)
                    .ToList(), foreign);
                }
                else if (type == 7)
                {
                    return FormBosForGroupDashboard(db.TreatyPricingGroupReferralVersions
                    .Where(q => q.RequestReceivedDate >= yearStart && q.RequestReceivedDate <= yearEnd)
                    .Where(q => q.QuotationTAT == 3)
                    .Where(q => !q.QuotationSentDate.HasValue)
                    .ToList(), foreign);
                }
                else if (type == 8)
                {
                    return FormBosForGroupDashboard(db.TreatyPricingGroupReferralVersions
                    .Where(q => q.RequestReceivedDate >= yearStart && q.RequestReceivedDate <= yearEnd)
                    .Where(q => q.InternalTAT == 3)
                    .Where(q => !q.QuotationSentDate.HasValue)
                    .ToList(), foreign);
                }
                else if (type == 9)
                {
                    return FormBosForGroupDashboard(db.TreatyPricingGroupReferralVersions
                    .Where(q => q.RequestReceivedDate >= yearStart && q.RequestReceivedDate <= yearEnd)
                    .Where(q => q.QuotationTAT > 3)
                    .Where(q => !q.QuotationSentDate.HasValue)
                    .ToList(), foreign);
                }
                else
                {
                    return FormBosForGroupDashboard(db.TreatyPricingGroupReferralVersions
                    .Where(q => q.RequestReceivedDate >= yearStart && q.RequestReceivedDate <= yearEnd)
                    .Where(q => q.InternalTAT > 3)
                    .Where(q => !q.QuotationSentDate.HasValue)
                    .ToList(), foreign);
                }

            }
        }

        private static int CalculateDaysWithHolidays(DateTime startDate, DateTime endDate)
        {
            int days = 0;
            for (DateTime date = startDate; date < endDate; date = date.AddDays(1))
            {
                if (startDate.DayOfWeek != DayOfWeek.Saturday && startDate.DayOfWeek != DayOfWeek.Sunday && !PublicHolidayDetailService.IsExists(date))
                {
                    days++;
                }
                startDate = startDate.AddDays(1);
            }

            return days;
        }

        private static int GetQuarter(DateTime date)
        {
            if (date.Month >= 1 && date.Month < 4)
                return 1;
            else if (date.Month >= 4 && date.Month < 6)
                return 2;
            else if (date.Month >= 6 && date.Month < 9)
                return 3;
            else
                return 4;
        }
    }
}
