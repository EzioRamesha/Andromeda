using DataAccess.Entities.Identity;
using DataAccess.EntityFramework;
using Shared;
using Shared.Trails;
using Shared.Trails.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace DataAccess.Entities
{
    [Table("RateTableDetails")]
    public class RateTableDetail
    {
        [Key]
        public int Id { get; set; }

        [Required, Index]
        public int RateTableId { get; set; }

        [ForeignKey(nameof(RateTableId))]
        [ExcludeTrail]
        public virtual RateTable RateTable { get; set; }

        [Required, MaxLength(255), Index]
        public string Combination { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime CreatedAt { get; set; }

        [Required]
        public int CreatedById { get; set; }

        [ForeignKey(nameof(CreatedById))]
        [ExcludeTrail]
        public virtual User CreatedBy { get; set; }

        [MaxLength(35), Index]
        public string TreatyCode { get; set; }

        [MaxLength(30), Index]
        public string CedingPlanCode { get; set; }

        [MaxLength(30), Index]
        public string CedingTreatyCode { get; set; }

        [MaxLength(30), Index]
        public string CedingPlanCode2 { get; set; }

        [MaxLength(30), Index]
        public string CedingBenefitTypeCode { get; set; }

        [MaxLength(50), Index]
        public string CedingBenefitRiskCode { get; set; }

        [MaxLength(30), Index]
        public string GroupPolicyNumber { get; set; }

        public RateTableDetail()
        {
            CreatedAt = DateTime.Now;
        }

        public static bool IsExists(int id)
        {
            using (var db = new AppDbContext())
            {
                return db.RateTableDetails.Any(q => q.Id == id);
            }
        }

        public static IQueryable<RateTableDetail> QueryByCombination(
            AppDbContext db,
            string combination,
            double? polAmtFrom,
            double? polAmtTo,
            int? attAgeFrom,
            int? attAgeTo,
            double? aarFrom,
            double? aarTo,
            DateTime? reinsEffDatePolStartDate,
            DateTime? reinsEffDatePolEndDate,
            DateTime? reportingStartDate,
            DateTime? reportingEndDate,
            double? policyTermFrom,
            double? policyTermTo,
            double? policyDurationFrom,
            double? policyDurationTo,
            int? rateTableId = null
        )
        {
            var query = db.RateTableDetails
                .Where(q => q.Combination.Trim() == combination.Trim());

            if (polAmtFrom != null && polAmtTo != null)
            {
                query = query
                    .Where(q =>
                    (
                        q.RateTable.PolicyAmountFrom <= polAmtFrom && q.RateTable.PolicyAmountTo >= polAmtFrom
                        ||
                        q.RateTable.PolicyAmountFrom <= polAmtTo && q.RateTable.PolicyAmountTo >= polAmtTo
                    )
                    || (q.RateTable.PolicyAmountFrom == null && q.RateTable.PolicyAmountTo == null)
                );
            }

            if (attAgeFrom != null && attAgeTo != null)
            {
                query = query
                    .Where(q =>
                    (
                        q.RateTable.AttainedAgeFrom <= attAgeFrom && q.RateTable.AttainedAgeTo >= attAgeFrom
                        ||
                        q.RateTable.AttainedAgeFrom <= attAgeTo && q.RateTable.AttainedAgeTo >= attAgeTo
                    )
                    || (q.RateTable.AttainedAgeFrom == null && q.RateTable.AttainedAgeTo == null)
                );
            }

            if (aarFrom != null && aarTo != null)
            {
                query = query
                    .Where(q =>
                    (
                        q.RateTable.AarFrom <= aarFrom && q.RateTable.AarTo >= aarFrom
                        ||
                        q.RateTable.AarFrom <= aarTo && q.RateTable.AarTo >= aarTo
                    )
                    || (q.RateTable.AarFrom == null && q.RateTable.AarTo == null)
                );
            }

            if (reinsEffDatePolStartDate != null && reinsEffDatePolEndDate != null)
            {
                query = query
                    .Where(q =>
                    (
                        DbFunctions.TruncateTime(q.RateTable.ReinsEffDatePolStartDate) <= DbFunctions.TruncateTime(reinsEffDatePolStartDate)
                        && DbFunctions.TruncateTime(q.RateTable.ReinsEffDatePolEndDate) >= DbFunctions.TruncateTime(reinsEffDatePolStartDate)
                        ||
                        DbFunctions.TruncateTime(q.RateTable.ReinsEffDatePolStartDate) <= DbFunctions.TruncateTime(reinsEffDatePolEndDate)
                        && DbFunctions.TruncateTime(q.RateTable.ReinsEffDatePolEndDate) >= DbFunctions.TruncateTime(reinsEffDatePolEndDate)
                    )
                    || (q.RateTable.ReinsEffDatePolStartDate == null && q.RateTable.ReinsEffDatePolEndDate == null)
                );
            }

            if (reportingStartDate != null && reportingEndDate != null)
            {
                query = query
                    .Where(q =>
                    (
                        DbFunctions.TruncateTime(q.RateTable.ReportingStartDate) <= DbFunctions.TruncateTime(reportingStartDate)
                        && DbFunctions.TruncateTime(q.RateTable.ReportingEndDate) >= DbFunctions.TruncateTime(reportingStartDate)
                        ||
                        DbFunctions.TruncateTime(q.RateTable.ReportingStartDate) <= DbFunctions.TruncateTime(reportingEndDate)
                        && DbFunctions.TruncateTime(q.RateTable.ReportingEndDate) >= DbFunctions.TruncateTime(reportingEndDate)
                    )
                    || (q.RateTable.ReportingStartDate == null && q.RateTable.ReportingEndDate == null)
                );
            }

            if (policyTermFrom != null && policyTermTo != null)
            {
                query = query
                    .Where(q =>
                    (
                        q.RateTable.PolicyTermFrom <= policyTermFrom && q.RateTable.PolicyTermTo >= policyTermFrom
                        ||
                        q.RateTable.PolicyTermFrom <= policyTermTo && q.RateTable.PolicyTermTo >= policyTermTo
                    )
                    || (q.RateTable.PolicyTermFrom == null && q.RateTable.PolicyTermTo == null)
                );
            }

            if (policyDurationFrom != null && policyDurationTo != null)
            {
                query = query
                    .Where(q =>
                    (
                        q.RateTable.PolicyDurationFrom <= policyDurationFrom && q.RateTable.PolicyDurationTo >= policyDurationFrom
                        ||
                        q.RateTable.PolicyDurationFrom <= policyDurationTo && q.RateTable.PolicyDurationTo >= policyDurationTo
                    )
                    || (q.RateTable.PolicyDurationFrom == null && q.RateTable.PolicyDurationTo == null)
                );
            }

            if (rateTableId != null)
            {
                query = query.Where(q => q.RateTableId != rateTableId);
            }
            return query;
        }

        public static IQueryable<RateTableDetail> QueryDuplicateByParams(
            AppDbContext db,
            string treatyCode,
            string cedingPlanCode,
            string cedingTreatyCode,
            string cedingPlanCode2,
            string cedingBenefitTypeCode,
            string cedingBenefitRiskCode,
            string groupPolicyNumber,
            int? benefitId,
            int? premiumFrequencyCodePickListDetailId,
            int? reinsBasisCodePickListDetailId,
            double? polAmtFrom,
            double? polAmtTo,
            int? attAgeFrom,
            int? attAgeTo,
            double? aarFrom,
            double? aarTo,
            DateTime? reinsEffDatePolStartDate,
            DateTime? reinsEffDatePolEndDate,
            DateTime? reportingStartDate,
            DateTime? reportingEndDate,
            double? policyTermFrom,
            double? policyTermTo,
            double? policyDurationFrom,
            double? policyDurationTo,
            int? rateTableId = null
        )
        {
            var query = db.RateTableDetails.AsQueryable();

            if (!string.IsNullOrEmpty(treatyCode))
            {
                query = query
                    .Where(q => (!string.IsNullOrEmpty(q.TreatyCode) && q.TreatyCode.Trim() == treatyCode.Trim()) || q.TreatyCode == null);
            }

            if (!string.IsNullOrEmpty(cedingPlanCode))
            {
                query = query
                    .Where(q => (!string.IsNullOrEmpty(q.CedingPlanCode) && q.CedingPlanCode.Trim() == cedingPlanCode.Trim()) || q.CedingPlanCode == null);
            }

            if (!string.IsNullOrEmpty(cedingTreatyCode))
            {
                query = query
                    .Where(q => (!string.IsNullOrEmpty(q.CedingTreatyCode) && q.CedingTreatyCode.Trim() == cedingTreatyCode.Trim()) || q.CedingTreatyCode == null);
            }

            if (!string.IsNullOrEmpty(cedingPlanCode2))
            {
                query = query
                    .Where(q => (!string.IsNullOrEmpty(q.CedingPlanCode2) && q.CedingPlanCode2.Trim() == cedingPlanCode2.Trim()) || q.CedingPlanCode2 == null);
            }

            if (!string.IsNullOrEmpty(cedingBenefitTypeCode))
            {
                query = query
                    .Where(q => (!string.IsNullOrEmpty(q.CedingBenefitTypeCode) && q.CedingBenefitTypeCode.Trim() == cedingBenefitTypeCode.Trim()) || q.CedingBenefitTypeCode == null);
            }

            if (!string.IsNullOrEmpty(cedingBenefitRiskCode))
            {
                query = query
                    .Where(q => (!string.IsNullOrEmpty(q.CedingBenefitRiskCode) && q.CedingBenefitRiskCode.Trim() == cedingBenefitRiskCode.Trim()) || q.CedingBenefitRiskCode == null);
            }

            if (!string.IsNullOrEmpty(groupPolicyNumber))
            {
                query = query
                    .Where(q => (!string.IsNullOrEmpty(q.GroupPolicyNumber) && q.GroupPolicyNumber.Trim() == groupPolicyNumber.Trim()) || q.GroupPolicyNumber == null);
            }

            if (benefitId.HasValue)
            {
                query = query
                    .Where(q => (q.RateTable.BenefitId.HasValue && q.RateTable.BenefitId == benefitId) || !q.RateTable.BenefitId.HasValue);
            }

            if (premiumFrequencyCodePickListDetailId.HasValue)
            {
                query = query
                    .Where(q => (q.RateTable.PremiumFrequencyCodePickListDetailId.HasValue && q.RateTable.PremiumFrequencyCodePickListDetailId == premiumFrequencyCodePickListDetailId) || !q.RateTable.PremiumFrequencyCodePickListDetailId.HasValue);
            }

            if (reinsBasisCodePickListDetailId.HasValue)
            {
                query = query
                    .Where(q => (q.RateTable.ReinsBasisCodePickListDetailId.HasValue && q.RateTable.ReinsBasisCodePickListDetailId == reinsBasisCodePickListDetailId) || !q.RateTable.ReinsBasisCodePickListDetailId.HasValue);
            }

            if (!string.IsNullOrEmpty(groupPolicyNumber))
            {
                query = query
                    .Where(q => (!string.IsNullOrEmpty(q.GroupPolicyNumber) && q.GroupPolicyNumber.Trim() == groupPolicyNumber.Trim()) || q.GroupPolicyNumber == null);
            }

            if (polAmtFrom != null && polAmtTo != null)
            {
                query = query
                    .Where(q =>
                    (
                        q.RateTable.PolicyAmountFrom <= polAmtFrom && q.RateTable.PolicyAmountTo >= polAmtFrom
                        ||
                        q.RateTable.PolicyAmountFrom <= polAmtTo && q.RateTable.PolicyAmountTo >= polAmtTo
                    )
                    || (q.RateTable.PolicyAmountFrom == null && q.RateTable.PolicyAmountTo == null)
                );
            }

            if (attAgeFrom != null && attAgeTo != null)
            {
                query = query
                    .Where(q =>
                    (
                        q.RateTable.AttainedAgeFrom <= attAgeFrom && q.RateTable.AttainedAgeTo >= attAgeFrom
                        ||
                        q.RateTable.AttainedAgeFrom <= attAgeTo && q.RateTable.AttainedAgeTo >= attAgeTo
                    )
                    || (q.RateTable.AttainedAgeFrom == null && q.RateTable.AttainedAgeTo == null)
                );
            }

            if (aarFrom != null && aarTo != null)
            {
                query = query
                    .Where(q =>
                    (
                        q.RateTable.AarFrom <= aarFrom && q.RateTable.AarTo >= aarFrom
                        ||
                        q.RateTable.AarFrom <= aarTo && q.RateTable.AarTo >= aarTo
                    )
                    || (q.RateTable.AarFrom == null && q.RateTable.AarTo == null)
                );
            }

            if (reinsEffDatePolStartDate != null && reinsEffDatePolEndDate != null)
            {
                query = query
                    .Where(q =>
                    (
                        DbFunctions.TruncateTime(q.RateTable.ReinsEffDatePolStartDate) <= DbFunctions.TruncateTime(reinsEffDatePolStartDate)
                        && DbFunctions.TruncateTime(q.RateTable.ReinsEffDatePolEndDate) >= DbFunctions.TruncateTime(reinsEffDatePolStartDate)
                        ||
                        DbFunctions.TruncateTime(q.RateTable.ReinsEffDatePolStartDate) <= DbFunctions.TruncateTime(reinsEffDatePolEndDate)
                        && DbFunctions.TruncateTime(q.RateTable.ReinsEffDatePolEndDate) >= DbFunctions.TruncateTime(reinsEffDatePolEndDate)
                    )
                    || (q.RateTable.ReinsEffDatePolStartDate == null && q.RateTable.ReinsEffDatePolEndDate == null)
                );
            }

            if (reportingStartDate != null && reportingEndDate != null)
            {
                query = query
                    .Where(q =>
                    (
                        DbFunctions.TruncateTime(q.RateTable.ReportingStartDate) <= DbFunctions.TruncateTime(reportingStartDate)
                        && DbFunctions.TruncateTime(q.RateTable.ReportingEndDate) >= DbFunctions.TruncateTime(reportingStartDate)
                        ||
                        DbFunctions.TruncateTime(q.RateTable.ReportingStartDate) <= DbFunctions.TruncateTime(reportingEndDate)
                        && DbFunctions.TruncateTime(q.RateTable.ReportingEndDate) >= DbFunctions.TruncateTime(reportingEndDate)
                    )
                    || (q.RateTable.ReportingStartDate == null && q.RateTable.ReportingEndDate == null)
                );
            }

            if (policyTermFrom != null && policyTermTo != null)
            {
                query = query
                    .Where(q =>
                    (
                        q.RateTable.PolicyTermFrom <= policyTermFrom && q.RateTable.PolicyTermTo >= policyTermFrom
                        ||
                        q.RateTable.PolicyTermFrom <= policyTermTo && q.RateTable.PolicyTermTo >= policyTermTo
                    )
                    || (q.RateTable.PolicyTermFrom == null && q.RateTable.PolicyTermTo == null)
                );
            }

            if (policyDurationFrom != null && policyDurationTo != null)
            {
                query = query
                    .Where(q =>
                    (
                        q.RateTable.PolicyDurationFrom <= policyDurationFrom && q.RateTable.PolicyDurationTo >= policyDurationFrom
                        ||
                        q.RateTable.PolicyDurationFrom <= policyDurationTo && q.RateTable.PolicyDurationTo >= policyDurationTo
                    )
                    || (q.RateTable.PolicyDurationFrom == null && q.RateTable.PolicyDurationTo == null)
                );
            }

            if (rateTableId != null)
            {
                query = query.Where(q => q.RateTableId != rateTableId);
            }
            return query;
        }

        public static IQueryable<RateTableDetail> QueryByParams(
            AppDbContext db,
            string treatyCode,
            string cedingPlanCode = null,
            string cedingTreatyCode = null,
            string cedingPlanCode2 = null,
            string cedingBenefitTypeCode = null,
            string cedingBenefitRiskCode = null,
            string groupPolicyNumber = null,
            string mlreBenefitCode = null,
            int? reinsBasisCodeId = null,
            int? premiumFrequencyCodeId = null,
            int? insuredAttainedAge = null,
            double? oriSumAssured = null,
            double? aar = null,
            DateTime? reinsEffDatePol = null,
            DateTime? reportingDate = null,
            double? policyTerm = null,
            double? policyDuration = null,
            bool groupById = false
        )
        {
            db.Database.CommandTimeout = 0;

            EFExecutionStrategy connectionStrategy = EFExecutionStrategy.GetNewRiDataRowProcessingInstance(427, "RateTableDetail");

            return connectionStrategy.Execute(() =>
            {
                var query = db.RateTableDetails.Where(q => q.TreatyCode.Trim() == treatyCode.Trim());

                if (!string.IsNullOrEmpty(cedingPlanCode))
                {
                    connectionStrategy.Reset(431);
                    query = query.Where(q => (!string.IsNullOrEmpty(q.CedingPlanCode) && q.CedingPlanCode.Trim() == cedingPlanCode.Trim()) || q.CedingPlanCode == null);
                }
                else
                {
                    connectionStrategy.Reset(436);
                    query = query.Where(q => q.CedingPlanCode == null);
                }

                if (!string.IsNullOrEmpty(cedingTreatyCode))
                {
                    connectionStrategy.Reset(442);
                    query = query.Where(q => (!string.IsNullOrEmpty(q.CedingTreatyCode) && q.CedingTreatyCode.Trim() == cedingTreatyCode.Trim()) || q.CedingTreatyCode == null);
                }
                else
                {
                    connectionStrategy.Reset(447);
                    query = query.Where(q => q.CedingTreatyCode == null);
                }

                if (!string.IsNullOrEmpty(cedingPlanCode2))
                {
                    connectionStrategy.Reset(454);
                    query = query.Where(q => (!string.IsNullOrEmpty(q.CedingPlanCode2) && q.CedingPlanCode2.Trim() == cedingPlanCode2.Trim()) || q.CedingPlanCode2 == null);
                }
                else
                {
                    connectionStrategy.Reset(458);
                    query = query.Where(q => q.CedingPlanCode2 == null);
                }

                if (!string.IsNullOrEmpty(cedingBenefitTypeCode))
                {
                    connectionStrategy.Reset(464);
                    query = query.Where(q => (!string.IsNullOrEmpty(q.CedingBenefitTypeCode) && q.CedingBenefitTypeCode.Trim() == cedingBenefitTypeCode.Trim()) || q.CedingBenefitTypeCode == null);
                }
                else
                {
                    connectionStrategy.Reset(470);
                    query = query.Where(q => q.CedingBenefitTypeCode == null);
                }

                if (!string.IsNullOrEmpty(cedingBenefitRiskCode))
                {
                    connectionStrategy.Reset(475);
                    query = query.Where(q => (!string.IsNullOrEmpty(q.CedingBenefitRiskCode) && q.CedingBenefitRiskCode.Trim() == cedingBenefitRiskCode.Trim()) || q.CedingBenefitRiskCode == null);
                }
                else
                {
                    connectionStrategy.Reset(481);
                    query = query.Where(q => q.CedingBenefitRiskCode == null);
                }

                if (!string.IsNullOrEmpty(groupPolicyNumber))
                {
                    connectionStrategy.Reset(487);
                    query = query.Where(q => (!string.IsNullOrEmpty(q.GroupPolicyNumber) && q.GroupPolicyNumber.Trim() == groupPolicyNumber.Trim()) || q.GroupPolicyNumber == null);
                }
                else
                {
                    connectionStrategy.Reset(492);
                    query = query.Where(q => q.GroupPolicyNumber == null);
                }

                if (!string.IsNullOrEmpty(mlreBenefitCode))
                {
                    connectionStrategy.Reset(498);
                    query = query.Where(q => (q.RateTable.Benefit != null && q.RateTable.Benefit.Code.Trim() == mlreBenefitCode.Trim()) || q.RateTable.Benefit == null);
                }
                else
                {
                    connectionStrategy.Reset(503);
                    query = query.Where(q => q.RateTable.Benefit == null);
                }

                if (reinsBasisCodeId != null)
                {
                    connectionStrategy.Reset(509);
                    query = query.Where(q => q.RateTable.ReinsBasisCodePickListDetailId == reinsBasisCodeId || q.RateTable.ReinsBasisCodePickListDetailId == null);
                }
                else
                {
                    connectionStrategy.Reset(513);
                    query = query.Where(q => q.RateTable.ReinsBasisCodePickListDetailId == null);
                }

                if (premiumFrequencyCodeId != null)
                {
                    connectionStrategy.Reset(519);
                    query = query.Where(q => q.RateTable.PremiumFrequencyCodePickListDetailId == premiumFrequencyCodeId || q.RateTable.PremiumFrequencyCodePickListDetailId == null);
                }
                else
                {
                    connectionStrategy.Reset(524);
                    query = query.Where(q => q.RateTable.PremiumFrequencyCodePickListDetailId == null);
                }

                if (insuredAttainedAge != null)
                {
                    connectionStrategy.Reset(530);
                    query = query.Where(q =>
                            (
                                q.RateTable.AttainedAgeFrom <= insuredAttainedAge && q.RateTable.AttainedAgeTo >= insuredAttainedAge
                                ||
                                q.RateTable.AttainedAgeFrom <= insuredAttainedAge && q.RateTable.AttainedAgeTo >= insuredAttainedAge
                            )
                            || (q.RateTable.AttainedAgeFrom == null && q.RateTable.AttainedAgeTo == null)
                        );
                }
                else
                {
                    connectionStrategy.Reset(542);
                    query = query.Where(q => q.RateTable.AttainedAgeFrom == null && q.RateTable.AttainedAgeTo == null);
                }

                if (oriSumAssured != null)
                {
                    connectionStrategy.Reset(548);
                    query = query.Where(q =>
                            (
                                q.RateTable.PolicyAmountFrom <= oriSumAssured && q.RateTable.PolicyAmountTo >= oriSumAssured
                                ||
                                q.RateTable.PolicyAmountFrom <= oriSumAssured && q.RateTable.PolicyAmountTo >= oriSumAssured
                            )
                            || (q.RateTable.PolicyAmountFrom == null && q.RateTable.PolicyAmountTo == null)
                        );
                }
                else
                {
                    connectionStrategy.Reset(560);
                    query = query.Where(q => q.RateTable.PolicyAmountFrom == null && q.RateTable.PolicyAmountTo == null);
                }

                if (aar != null)
                {
                    connectionStrategy.Reset(567);
                    query = query.Where(q =>
                            (
                                q.RateTable.AarFrom <= aar && q.RateTable.AarTo >= aar
                                ||
                                q.RateTable.AarFrom <= aar && q.RateTable.AarTo >= aar
                            )
                            || (q.RateTable.AarFrom == null && q.RateTable.AarTo == null)
                        );
                }

                else
                {
                    connectionStrategy.Reset(579);
                    query = query.Where(q => q.RateTable.AarFrom == null && q.RateTable.AarTo == null);
                }

                if (reinsEffDatePol != null)
                {
                    connectionStrategy.Reset(585);
                    query = query.Where(q =>
                            (
                                DbFunctions.TruncateTime(q.RateTable.ReinsEffDatePolStartDate) <= DbFunctions.TruncateTime(reinsEffDatePol)
                                && DbFunctions.TruncateTime(q.RateTable.ReinsEffDatePolEndDate) >= DbFunctions.TruncateTime(reinsEffDatePol)
                                ||
                                DbFunctions.TruncateTime(q.RateTable.ReinsEffDatePolStartDate) <= DbFunctions.TruncateTime(reinsEffDatePol)
                                && DbFunctions.TruncateTime(q.RateTable.ReinsEffDatePolEndDate) >= DbFunctions.TruncateTime(reinsEffDatePol)
                            )
                            || (q.RateTable.ReinsEffDatePolStartDate == null && q.RateTable.ReinsEffDatePolEndDate == null)
                        );
                }
                else
                {
                    connectionStrategy.Reset(600);
                    query = query.Where(q => q.RateTable.ReinsEffDatePolStartDate == null && q.RateTable.ReinsEffDatePolEndDate == null);
                }

                if (reportingDate != null)
                {
                    connectionStrategy.Reset(606);
                    query = query.Where(q =>
                            (
                                DbFunctions.TruncateTime(q.RateTable.ReportingStartDate) <= DbFunctions.TruncateTime(reportingDate)
                                && DbFunctions.TruncateTime(q.RateTable.ReportingEndDate) >= DbFunctions.TruncateTime(reportingDate)
                                ||
                                DbFunctions.TruncateTime(q.RateTable.ReportingStartDate) <= DbFunctions.TruncateTime(reportingDate)
                                && DbFunctions.TruncateTime(q.RateTable.ReportingEndDate) >= DbFunctions.TruncateTime(reportingDate)
                            )
                            ||
                            (q.RateTable.ReportingStartDate == null && q.RateTable.ReportingEndDate == null)
                        );
                }
                else
                {
                    connectionStrategy.Reset(620);
                    query = query.Where(q => q.RateTable.ReportingStartDate == null && q.RateTable.ReportingEndDate == null);
                }

                if (policyTerm != null)
                {
                    connectionStrategy.Reset(626);
                    query = query.Where(q =>
                            (
                                q.RateTable.PolicyTermFrom <= policyTerm && q.RateTable.PolicyTermTo >= policyTerm
                                ||
                                q.RateTable.PolicyTermFrom <= policyTerm && q.RateTable.PolicyTermTo >= policyTerm
                            )
                            || (q.RateTable.PolicyTermFrom == null && q.RateTable.PolicyTermTo == null)
                        );
                }
                else
                {
                    connectionStrategy.Reset(638);
                    query = query.Where(q => q.RateTable.PolicyTermFrom == null && q.RateTable.PolicyTermTo == null);
                }

                if (policyDuration != null)
                {
                    connectionStrategy.Reset(644);
                    query = query.Where(q =>
                            (
                                q.RateTable.PolicyDurationFrom <= policyDuration && q.RateTable.PolicyDurationTo >= policyDuration
                                ||
                                q.RateTable.PolicyDurationFrom <= policyDuration && q.RateTable.PolicyDurationTo >= policyDuration
                            )
                            || (q.RateTable.PolicyDurationFrom == null && q.RateTable.PolicyDurationTo == null)
                        );
                }
                else
                {
                    connectionStrategy.Reset(656);
                    query = query
                        .Where(q => q.RateTable.PolicyDurationFrom == null && q.RateTable.PolicyDurationTo == null);
                }

                // NOTE: Group by should put at the end of query
                if (groupById)
                {
                    query = query.GroupBy(q => q.RateTableId).Select(q => q.FirstOrDefault());
                }

                return query;


            });
        }

        public static RateTableDetail Find(int id)
        {
            using (var db = new AppDbContext())
            {
                return db.RateTableDetails.Where(q => q.Id == id).FirstOrDefault();
            }
        }

        public static RateTableDetail FindByCombination(
            string combination,
            double? polAmtFrom,
            double? polAmtTo,
            int? attAgeFrom,
            int? attAgeTo,
            double? aarFrom,
            double? aarTo,
            DateTime? reinsEffDatePolStartDate,
            DateTime? reinsEffDatePolEndDate,
            DateTime? reportingStartDate,
            DateTime? reportingEndDate,
            double? policyTermFrom,
            double? policyTermTo,
            double? policyDurationFrom,
            double? policyDurationTo,
            int? rateTableId = null
        )
        {
            using (var db = new AppDbContext())
            {
                return QueryByCombination(
                        db,
                        combination,
                        polAmtFrom,
                        polAmtTo,
                        attAgeFrom,
                        attAgeTo,
                        aarFrom,
                        aarTo,
                        reinsEffDatePolStartDate,
                        reinsEffDatePolEndDate,
                        reportingStartDate,
                        reportingEndDate,
                        policyTermFrom,
                        policyTermTo,
                        policyDurationFrom,
                        policyDurationTo,
                        rateTableId
                    ).FirstOrDefault();
            }
        }

        public static RateTableDetail FindByParams(
            string treatyCode,
            string cedingPlanCode = null,
            string cedingTreatyCode = null,
            string cedingPlanCode2 = null,
            string cedingBenefitTypeCode = null,
            string cedingBenefitRiskCode = null,
            string groupPolicyNumber = null,
            string mlreBenefitCode = null,
            int? reinsBasisCodeId = null,
            int? premiumFrequencyCodeId = null,
            int? insuredAttainedAge = null,
            double? oriSumAssured = null,
            double? aar = null,
            DateTime? reinsEffDatePol = null,
            DateTime? reportingDate = null,
            double? policyTerm = null,
            double? policyDuration = null,
            bool groupById = false
        )
        {
            using (var db = new AppDbContext())
            {
                return QueryByParams(
                    db,
                    treatyCode,
                    cedingPlanCode,
                    cedingTreatyCode,
                    cedingPlanCode2,
                    cedingBenefitTypeCode,
                    cedingBenefitRiskCode,
                    groupPolicyNumber,
                    mlreBenefitCode,
                    reinsBasisCodeId,
                    premiumFrequencyCodeId,
                    insuredAttainedAge,
                    oriSumAssured,
                    aar,
                    reinsEffDatePol,
                    reportingDate,
                    policyTerm,
                    policyDuration,
                    groupById
                ).FirstOrDefault();
            }
        }

        public static int CountByCombination(
            string combination,
            double? polAmtFrom,
            double? polAmtTo,
            int? attAgeFrom,
            int? attAgeTo,
            double? aarFrom,
            double? aarTo,
            DateTime? reinsEffDatePolStartDate,
            DateTime? reinsEffDatePolEndDate,
            DateTime? reportingStartDate,
            DateTime? reportingEndDate,
            double? policyTermFrom,
            double? policyTermTo,
            double? policyDurationFrom,
            double? policyDurationTo,
            int? rateTableId = null
        )
        {
            using (var db = new AppDbContext())
            {
                return QueryByCombination(
                        db,
                        combination,
                        polAmtFrom,
                        polAmtTo,
                        attAgeFrom,
                        attAgeTo,
                        aarFrom,
                        aarTo,
                        reinsEffDatePolStartDate,
                        reinsEffDatePolEndDate,
                        reportingStartDate,
                        reportingEndDate,
                        policyTermFrom,
                        policyTermTo,
                        policyDurationFrom,
                        policyDurationTo,
                        rateTableId
                    ).Count();
            }
        }

        public static int CountDuplicateByParams(
            string treatyCode,
            string cedingPlanCode,
            string cedingTreatyCode,
            string cedingPlanCode2,
            string cedingBenefitTypeCode,
            string cedingBenefitRiskCode,
            string groupPolicyNumber,
            int? benefitId,
            int? premiumFrequencyCodePickListDetailId,
            int? reinsBasisCodePickListDetailId,
            double? polAmtFrom,
            double? polAmtTo,
            int? attAgeFrom,
            int? attAgeTo,
            double? aarFrom,
            double? aarTo,
            DateTime? reinsEffDatePolStartDate,
            DateTime? reinsEffDatePolEndDate,
            DateTime? reportingStartDate,
            DateTime? reportingEndDate,
            double? policyTermFrom,
            double? policyTermTo,
            double? policyDurationFrom,
            double? policyDurationTo,
            int? rateTableId = null
        )
        {
            using (var db = new AppDbContext())
            {
                return QueryDuplicateByParams(
                        db,
                        treatyCode,
                        cedingPlanCode,
                        cedingTreatyCode,
                        cedingPlanCode2,
                        cedingBenefitTypeCode,
                        cedingBenefitRiskCode,
                        groupPolicyNumber,
                        benefitId,
                        premiumFrequencyCodePickListDetailId,
                        reinsBasisCodePickListDetailId,
                        polAmtFrom,
                        polAmtTo,
                        attAgeFrom,
                        attAgeTo,
                        aarFrom,
                        aarTo,
                        reinsEffDatePolStartDate,
                        reinsEffDatePolEndDate,
                        reportingStartDate,
                        reportingEndDate,
                        policyTermFrom,
                        policyTermTo,
                        policyDurationFrom,
                        policyDurationTo,
                        rateTableId
                    ).Count();
            }
        }

        public static int CountByParams(
            string treatyCode,
            string cedingPlanCode = null,
            string cedingTreatyCode = null,
            string cedingPlanCode2 = null,
            string cedingBenefitTypeCode = null,
            string cedingBenefitRiskCode = null,
            string groupPolicyNumber = null,
            string mlreBenefitCode = null,
            int? reinsBasisCodeId = null,
            int? premiumFrequencyCodeId = null,
            int? insuredAttainedAge = null,
            double? oriSumAssured = null,
            double? aar = null,
            DateTime? reinsEffDatePol = null,
            DateTime? reportingDate = null,
            double? policyTerm = null,
            double? policyDuration = null,
            bool groupById = false
        )
        {
            using (var db = new AppDbContext())
            {
                return QueryByParams(
                    db,
                    treatyCode,
                    cedingPlanCode,
                    cedingTreatyCode,
                    cedingPlanCode2,
                    cedingBenefitTypeCode,
                    cedingBenefitRiskCode,
                    groupPolicyNumber,
                    mlreBenefitCode,
                    reinsBasisCodeId,
                    premiumFrequencyCodeId,
                    insuredAttainedAge,
                    oriSumAssured,
                    aar,
                    reinsEffDatePol,
                    reportingDate,
                    policyTerm,
                    policyDuration,
                    groupById
                ).Count();
            }
        }

        public static IList<RateTableDetail> GetByParams(
            string treatyCode,
            string cedingPlanCode = null,
            string cedingTreatyCode = null,
            string cedingPlanCode2 = null,
            string cedingBenefitTypeCode = null,
            string cedingBenefitRiskCode = null,
            string groupPolicyNumber = null,
            string mlreBenefitCode = null,
            int? reinsBasisCodeId = null,
            int? premiumFrequencyCodeId = null,
            int? insuredAttainedAge = null,
            double? oriSumAssured = null,
            double? aar = null,
            DateTime? reinsEffDatePol = null,
            DateTime? reportingDate = null,
            double? policyTerm = null,
            double? policyDuration = null,
            bool groupById = false
        )
        {
            using (var db = new AppDbContext())
            {
                return QueryByParams(
                    db,
                    treatyCode,
                    cedingPlanCode,
                    cedingTreatyCode,
                    cedingPlanCode2,
                    cedingBenefitTypeCode,
                    cedingBenefitRiskCode,
                    groupPolicyNumber,
                    mlreBenefitCode,
                    reinsBasisCodeId,
                    premiumFrequencyCodeId,
                    insuredAttainedAge,
                    oriSumAssured,
                    aar,
                    reinsEffDatePol,
                    reportingDate,
                    policyTerm,
                    policyDuration,
                    groupById
                ).ToList();
            }
        }

        public DataTrail Create()
        {
            using (var db = new AppDbContext())
            {
                db.RateTableDetails.Add(this);
                db.SaveChanges();

                var trail = new DataTrail(this);
                return trail;
            }
        }

        public DataTrail Update()
        {
            using (var db = new AppDbContext())
            {
                var entity = RateTableDetail.Find(Id);
                if (entity == null)
                {
                    throw new Exception(MessageBag.NoRecordFound);
                }

                var trail = new DataTrail(entity, this);

                entity.RateTableId = RateTableId;
                entity.Combination = Combination;
                entity.TreatyCode = TreatyCode;
                entity.CedingPlanCode = CedingPlanCode;
                entity.CedingTreatyCode = CedingTreatyCode;
                entity.CedingPlanCode2 = CedingPlanCode2;
                entity.CedingBenefitTypeCode = CedingBenefitTypeCode;
                entity.CedingBenefitRiskCode = CedingBenefitRiskCode;
                entity.GroupPolicyNumber = GroupPolicyNumber;

                db.Entry(entity).State = EntityState.Modified;
                db.SaveChanges();

                return trail;
            }
        }

        public static DataTrail Delete(int id)
        {
            using (var db = new AppDbContext())
            {
                var entity = db.RateTableDetails.Where(q => q.Id == id).FirstOrDefault();
                if (entity == null)
                {
                    throw new Exception(MessageBag.NoRecordFound);
                }

                var trail = new DataTrail(entity, true);

                db.Entry(entity).State = EntityState.Deleted;
                db.RateTableDetails.Remove(entity);
                db.SaveChanges();

                return trail;
            }
        }

        public static IList<DataTrail> DeleteByRateTableId(int rateTableId)
        {
            using (var db = new AppDbContext())
            {
                var query = db.RateTableDetails.Where(q => q.RateTableId == rateTableId);

                var trails = new List<DataTrail>();
                foreach (RateTableDetail rateTableDetail in query.ToList())
                {
                    var trail = new DataTrail(rateTableDetail, true);
                    trails.Add(trail);

                    db.Entry(rateTableDetail).State = EntityState.Deleted;
                    db.RateTableDetails.Remove(rateTableDetail);
                }

                db.SaveChanges();
                return trails;
            }
        }
    }
}
