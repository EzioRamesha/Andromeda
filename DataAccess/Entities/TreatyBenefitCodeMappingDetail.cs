using BusinessObject;
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
    [Table("TreatyBenefitCodeMappingDetails")]
    public class TreatyBenefitCodeMappingDetail
    {
        [Key]
        public int Id { get; set; }

        [Index]
        public int Type { get; set; }

        [Required, Index]
        public int TreatyBenefitCodeMappingId { get; set; }

        [ForeignKey(nameof(TreatyBenefitCodeMappingId))]
        [ExcludeTrail]
        public virtual TreatyBenefitCodeMapping TreatyBenefitCodeMapping { get; set; }

        [Required, MaxLength(255), Index]
        public string Combination { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime CreatedAt { get; set; }

        [Required]
        public int CreatedById { get; set; }

        [ForeignKey(nameof(CreatedById))]
        [ExcludeTrail]
        public virtual User CreatedBy { get; set; }

        [MaxLength(30), Index]
        public string CedingPlanCode { get; set; }

        [MaxLength(32), Index]
        public string CedingBenefitTypeCode { get; set; }

        [MaxLength(50), Index]
        public string CedingBenefitRiskCode { get; set; }

        [MaxLength(30), Index]
        public string CedingTreatyCode { get; set; }

        [MaxLength(10), Index]
        public string CampaignCode { get; set; }

        public TreatyBenefitCodeMappingDetail()
        {
            CreatedAt = DateTime.Now;
        }

        public static bool IsExists(int id)
        {
            using (var db = new AppDbContext())
            {
                return db.TreatyBenefitCodeMappingDetails.Any(q => q.Id == id);
            }
        }

        public static IQueryable<TreatyBenefitCodeMappingDetail> QueryCombinationForTreaty(
            AppDbContext db,
            int cedantId,
            string combination,
            DateTime? reinsEffDatePolStartDate,
            DateTime? reinsEffDatePolEndDate,
            int? attAgeFrom,
            int? attAgeTo,
            DateTime? reportingStartDate,
            DateTime? reportingEndDate,
            double? underwriterRatingFrom,
            double? underwriterRatingTo,
            double? oriSumAssuredFrom,
            double? oriSumAssuredTo,
            int? reinsuranceIssueAgeFrom,
            int? reinsuranceIssueAgeTo,
            int? treatyBenefitCodeMappingId = null
        )
        {
            var query = db.TreatyBenefitCodeMappingDetails
                    .Where(q => q.TreatyBenefitCodeMapping.CedantId == cedantId)
                    .Where(q => q.Combination.Trim() == combination.Trim())
                    .Where(q => q.Type == TreatyBenefitCodeMappingDetailBo.CombinationTypeTreaty);

            if (reinsEffDatePolStartDate != null && reinsEffDatePolEndDate != null)
            {
                query = query
                    .Where(q =>
                    (
                        DbFunctions.TruncateTime(q.TreatyBenefitCodeMapping.ReinsEffDatePolStartDate) <= DbFunctions.TruncateTime(reinsEffDatePolStartDate)
                        && DbFunctions.TruncateTime(q.TreatyBenefitCodeMapping.ReinsEffDatePolEndDate) >= DbFunctions.TruncateTime(reinsEffDatePolStartDate)
                        ||
                        DbFunctions.TruncateTime(q.TreatyBenefitCodeMapping.ReinsEffDatePolStartDate) <= DbFunctions.TruncateTime(reinsEffDatePolEndDate)
                        && DbFunctions.TruncateTime(q.TreatyBenefitCodeMapping.ReinsEffDatePolEndDate) >= DbFunctions.TruncateTime(reinsEffDatePolEndDate)
                    )
                    || (q.TreatyBenefitCodeMapping.ReinsEffDatePolStartDate == null && q.TreatyBenefitCodeMapping.ReinsEffDatePolEndDate == null)
                );
            }

            if (attAgeFrom != null && attAgeTo != null)
            {
                query = query
                    .Where(q =>
                    (
                        q.TreatyBenefitCodeMapping.AttainedAgeFrom <= attAgeFrom && q.TreatyBenefitCodeMapping.AttainedAgeTo >= attAgeFrom
                        ||
                        q.TreatyBenefitCodeMapping.AttainedAgeFrom <= attAgeTo && q.TreatyBenefitCodeMapping.AttainedAgeTo >= attAgeTo
                    )
                    || (q.TreatyBenefitCodeMapping.AttainedAgeFrom == null && q.TreatyBenefitCodeMapping.AttainedAgeTo == null)
                );
            }

            if (reportingStartDate != null && reportingEndDate != null)
            {
                query = query
                    .Where(q =>
                    (
                        DbFunctions.TruncateTime(q.TreatyBenefitCodeMapping.ReportingStartDate) <= DbFunctions.TruncateTime(reportingStartDate)
                        && DbFunctions.TruncateTime(q.TreatyBenefitCodeMapping.ReportingEndDate) >= DbFunctions.TruncateTime(reportingStartDate)
                        ||
                        DbFunctions.TruncateTime(q.TreatyBenefitCodeMapping.ReportingStartDate) <= DbFunctions.TruncateTime(reportingEndDate)
                        && DbFunctions.TruncateTime(q.TreatyBenefitCodeMapping.ReportingEndDate) >= DbFunctions.TruncateTime(reportingEndDate)
                    )
                    || (q.TreatyBenefitCodeMapping.ReportingStartDate == null && q.TreatyBenefitCodeMapping.ReportingEndDate == null)
                );
            }

            if (underwriterRatingFrom != null && underwriterRatingTo != null)
            {
                query = query
                    .Where(q =>
                    (
                        q.TreatyBenefitCodeMapping.UnderwriterRatingFrom <= underwriterRatingFrom && q.TreatyBenefitCodeMapping.UnderwriterRatingTo >= underwriterRatingFrom
                        ||
                        q.TreatyBenefitCodeMapping.UnderwriterRatingFrom <= underwriterRatingTo && q.TreatyBenefitCodeMapping.UnderwriterRatingTo >= underwriterRatingTo
                    )
                    || (q.TreatyBenefitCodeMapping.UnderwriterRatingFrom == null && q.TreatyBenefitCodeMapping.UnderwriterRatingTo == null)
                );
            }

            if (oriSumAssuredFrom != null && oriSumAssuredTo != null)
            {
                query = query
                    .Where(q =>
                    (
                        q.TreatyBenefitCodeMapping.OriSumAssuredFrom <= oriSumAssuredFrom && q.TreatyBenefitCodeMapping.OriSumAssuredTo >= oriSumAssuredFrom
                        ||
                        q.TreatyBenefitCodeMapping.OriSumAssuredFrom <= oriSumAssuredTo && q.TreatyBenefitCodeMapping.OriSumAssuredTo >= oriSumAssuredTo
                    )
                    || (q.TreatyBenefitCodeMapping.OriSumAssuredFrom == null && q.TreatyBenefitCodeMapping.OriSumAssuredTo == null)
                );
            }

            if (reinsuranceIssueAgeFrom != null && reinsuranceIssueAgeTo != null)
            {
                query = query
                    .Where(q =>
                    (
                        q.TreatyBenefitCodeMapping.ReinsuranceIssueAgeFrom <= attAgeFrom && q.TreatyBenefitCodeMapping.ReinsuranceIssueAgeTo >= reinsuranceIssueAgeFrom
                        ||
                        q.TreatyBenefitCodeMapping.ReinsuranceIssueAgeFrom <= attAgeTo && q.TreatyBenefitCodeMapping.ReinsuranceIssueAgeTo >= reinsuranceIssueAgeTo
                    )
                    || (q.TreatyBenefitCodeMapping.ReinsuranceIssueAgeFrom == null && q.TreatyBenefitCodeMapping.ReinsuranceIssueAgeTo == null)
                );
            }

            if (treatyBenefitCodeMappingId != null)
            {
                query = query.Where(q => q.TreatyBenefitCodeMappingId != treatyBenefitCodeMappingId);
            }

            return query;
        }

        public static IQueryable<TreatyBenefitCodeMappingDetail> QueryCombinationForBenefit(
            AppDbContext db,
            int cedantId,
            string combination,
            int? attAgeFrom,
            int? attAgeTo,
            DateTime? reportingStartDate,
            DateTime? reportingEndDate,
            int? treatyCodeId,
            DateTime? reinsEffDatePolStartDate,
            DateTime? reinsEffDatePolEndDate,
            double? underwriterRatingFrom,
            double? underwriterRatingTo,
            double? oriSumAssuredFrom,
            double? oriSumAssuredTo,
            int? reinsuranceIssueAgeFrom,
            int? reinsuranceIssueAgeTo,
            int? treatyBenefitCodeMappingId = null
        )
        {
            var query = db.TreatyBenefitCodeMappingDetails
                .Where(q => q.TreatyBenefitCodeMapping.CedantId == cedantId)
                .Where(q => q.Combination.Trim() == combination.Trim())
                .Where(q => q.Type == TreatyBenefitCodeMappingDetailBo.CombinationTypeBenefit);

            if (attAgeFrom != null && attAgeTo != null)
            {
                query = query
                    .Where(q =>
                    (
                        q.TreatyBenefitCodeMapping.AttainedAgeFrom <= attAgeFrom && q.TreatyBenefitCodeMapping.AttainedAgeTo >= attAgeFrom
                        ||
                        q.TreatyBenefitCodeMapping.AttainedAgeFrom <= attAgeTo && q.TreatyBenefitCodeMapping.AttainedAgeTo >= attAgeTo
                    )
                    || (q.TreatyBenefitCodeMapping.AttainedAgeFrom == null && q.TreatyBenefitCodeMapping.AttainedAgeTo == null)
                );
            }

            if (reportingStartDate != null && reportingEndDate != null)
            {
                query = query
                    .Where(q =>
                    (
                        DbFunctions.TruncateTime(q.TreatyBenefitCodeMapping.ReportingStartDate) <= DbFunctions.TruncateTime(reportingStartDate)
                        && DbFunctions.TruncateTime(q.TreatyBenefitCodeMapping.ReportingEndDate) >= DbFunctions.TruncateTime(reportingStartDate)
                        ||
                        DbFunctions.TruncateTime(q.TreatyBenefitCodeMapping.ReportingStartDate) <= DbFunctions.TruncateTime(reportingEndDate)
                        && DbFunctions.TruncateTime(q.TreatyBenefitCodeMapping.ReportingEndDate) >= DbFunctions.TruncateTime(reportingEndDate)
                    )
                    || (q.TreatyBenefitCodeMapping.ReportingStartDate == null && q.TreatyBenefitCodeMapping.ReportingEndDate == null)
                );
            }

            if (treatyCodeId != null)
            {
                query = query.Where(q => q.TreatyBenefitCodeMapping.TreatyCodeId == treatyCodeId);
            }

            if (reinsEffDatePolStartDate != null && reinsEffDatePolEndDate != null)
            {
                query = query
                    .Where(q =>
                    (
                        DbFunctions.TruncateTime(q.TreatyBenefitCodeMapping.ReinsEffDatePolStartDate) <= DbFunctions.TruncateTime(reinsEffDatePolStartDate)
                        && DbFunctions.TruncateTime(q.TreatyBenefitCodeMapping.ReinsEffDatePolEndDate) >= DbFunctions.TruncateTime(reinsEffDatePolStartDate)
                        ||
                        DbFunctions.TruncateTime(q.TreatyBenefitCodeMapping.ReinsEffDatePolStartDate) <= DbFunctions.TruncateTime(reinsEffDatePolEndDate)
                        && DbFunctions.TruncateTime(q.TreatyBenefitCodeMapping.ReinsEffDatePolEndDate) >= DbFunctions.TruncateTime(reinsEffDatePolEndDate)
                    )
                    || (q.TreatyBenefitCodeMapping.ReinsEffDatePolStartDate == null && q.TreatyBenefitCodeMapping.ReinsEffDatePolEndDate == null)
                );
            }

            if (underwriterRatingFrom != null && underwriterRatingTo != null)
            {
                query = query
                    .Where(q =>
                    (
                        q.TreatyBenefitCodeMapping.UnderwriterRatingFrom <= underwriterRatingFrom && q.TreatyBenefitCodeMapping.UnderwriterRatingTo >= underwriterRatingFrom
                        ||
                        q.TreatyBenefitCodeMapping.UnderwriterRatingFrom <= underwriterRatingTo && q.TreatyBenefitCodeMapping.UnderwriterRatingTo >= underwriterRatingTo
                    )
                    || (q.TreatyBenefitCodeMapping.UnderwriterRatingFrom == null && q.TreatyBenefitCodeMapping.UnderwriterRatingTo == null)
                );
            }

            if (oriSumAssuredFrom != null && oriSumAssuredTo != null)
            {
                query = query
                    .Where(q =>
                    (
                        q.TreatyBenefitCodeMapping.OriSumAssuredFrom <= oriSumAssuredFrom && q.TreatyBenefitCodeMapping.OriSumAssuredTo >= oriSumAssuredFrom
                        ||
                        q.TreatyBenefitCodeMapping.OriSumAssuredFrom <= oriSumAssuredTo && q.TreatyBenefitCodeMapping.OriSumAssuredTo >= oriSumAssuredTo
                    )
                    || (q.TreatyBenefitCodeMapping.OriSumAssuredFrom == null && q.TreatyBenefitCodeMapping.OriSumAssuredTo == null)
                );
            }

            if (reinsuranceIssueAgeFrom != null && reinsuranceIssueAgeTo != null)
            {
                query = query
                    .Where(q =>
                    (
                        q.TreatyBenefitCodeMapping.ReinsuranceIssueAgeFrom <= reinsuranceIssueAgeFrom && q.TreatyBenefitCodeMapping.ReinsuranceIssueAgeTo >= reinsuranceIssueAgeFrom
                        ||
                        q.TreatyBenefitCodeMapping.ReinsuranceIssueAgeFrom <= reinsuranceIssueAgeTo && q.TreatyBenefitCodeMapping.ReinsuranceIssueAgeTo >= reinsuranceIssueAgeTo
                    )
                    || (q.TreatyBenefitCodeMapping.ReinsuranceIssueAgeFrom == null && q.TreatyBenefitCodeMapping.ReinsuranceIssueAgeTo == null)
                );
            }

            if (treatyBenefitCodeMappingId != null)
            {
                query = query.Where(q => q.TreatyBenefitCodeMappingId != treatyBenefitCodeMappingId);
            }

            return query;
        }

        public static IQueryable<TreatyBenefitCodeMappingDetail> QueryCombinationForProductFeature(
            AppDbContext db,
            int cedantId,
            string combination,
            DateTime? reinsEffDatePolStartDate,
            DateTime? reinsEffDatePolEndDate,
            int? attAgeFrom,
            int? attAgeTo,
            int? treatyCodeId,
            int? benefitId,
            double? underwriterRatingFrom,
            double? underwriterRatingTo,
            double? oriSumAssuredFrom,
            double? oriSumAssuredTo,
            int? reinsuranceIssueAgeFrom,
            int? reinsuranceIssueAgeTo,
            DateTime? reportingStartDate,
            DateTime? reportingEndDate,
            int? treatyBenefitCodeMappingId = null
        )
        {
            var query = db.TreatyBenefitCodeMappingDetails
                .Where(q => q.TreatyBenefitCodeMapping.CedantId == cedantId)
                .Where(q => q.Combination.Trim() == combination.Trim())
                .Where(q => q.Type == TreatyBenefitCodeMappingDetailBo.CombinationTypeProductFeature);

            if (reinsEffDatePolStartDate != null && reinsEffDatePolEndDate != null)
            {
                query = query
                    .Where(q =>
                    (
                        DbFunctions.TruncateTime(q.TreatyBenefitCodeMapping.ReinsEffDatePolStartDate) <= DbFunctions.TruncateTime(reinsEffDatePolStartDate)
                        && DbFunctions.TruncateTime(q.TreatyBenefitCodeMapping.ReinsEffDatePolEndDate) >= DbFunctions.TruncateTime(reinsEffDatePolStartDate)
                        ||
                        DbFunctions.TruncateTime(q.TreatyBenefitCodeMapping.ReinsEffDatePolStartDate) <= DbFunctions.TruncateTime(reinsEffDatePolEndDate)
                        && DbFunctions.TruncateTime(q.TreatyBenefitCodeMapping.ReinsEffDatePolEndDate) >= DbFunctions.TruncateTime(reinsEffDatePolEndDate)
                    )
                    || (q.TreatyBenefitCodeMapping.ReinsEffDatePolStartDate == null && q.TreatyBenefitCodeMapping.ReinsEffDatePolEndDate == null)
                );
            }

            if (attAgeFrom != null && attAgeTo != null)
            {
                query = query
                    .Where(q =>
                    (
                        q.TreatyBenefitCodeMapping.AttainedAgeFrom <= attAgeFrom && q.TreatyBenefitCodeMapping.AttainedAgeTo >= attAgeFrom
                        ||
                        q.TreatyBenefitCodeMapping.AttainedAgeFrom <= attAgeTo && q.TreatyBenefitCodeMapping.AttainedAgeTo >= attAgeTo
                    )
                    || (q.TreatyBenefitCodeMapping.AttainedAgeFrom == null && q.TreatyBenefitCodeMapping.AttainedAgeTo == null)
                );
            }

            if (treatyCodeId != null)
            {
                query = query.Where(q => q.TreatyBenefitCodeMapping.TreatyCodeId == treatyCodeId);
            }

            if (benefitId != null)
            {
                query = query.Where(q => q.TreatyBenefitCodeMapping.BenefitId == benefitId);
            }

            if (underwriterRatingFrom != null && underwriterRatingTo != null)
            {
                query = query
                    .Where(q =>
                    (
                        q.TreatyBenefitCodeMapping.UnderwriterRatingFrom <= underwriterRatingFrom && q.TreatyBenefitCodeMapping.UnderwriterRatingTo >= underwriterRatingFrom
                        ||
                        q.TreatyBenefitCodeMapping.UnderwriterRatingFrom <= underwriterRatingTo && q.TreatyBenefitCodeMapping.UnderwriterRatingTo >= underwriterRatingTo
                    )
                    || (q.TreatyBenefitCodeMapping.UnderwriterRatingFrom == null && q.TreatyBenefitCodeMapping.UnderwriterRatingTo == null)
                );
            }

            if (oriSumAssuredFrom != null && oriSumAssuredTo != null)
            {
                query = query
                    .Where(q =>
                    (
                        q.TreatyBenefitCodeMapping.OriSumAssuredFrom <= oriSumAssuredFrom && q.TreatyBenefitCodeMapping.OriSumAssuredTo >= oriSumAssuredFrom
                        ||
                        q.TreatyBenefitCodeMapping.OriSumAssuredFrom <= oriSumAssuredTo && q.TreatyBenefitCodeMapping.OriSumAssuredTo >= oriSumAssuredTo
                    )
                    || (q.TreatyBenefitCodeMapping.OriSumAssuredFrom == null && q.TreatyBenefitCodeMapping.OriSumAssuredTo == null)
                );
            }

            if (reportingStartDate != null && reportingEndDate != null)
            {
                query = query
                    .Where(q =>
                    (
                        DbFunctions.TruncateTime(q.TreatyBenefitCodeMapping.ReportingStartDate) <= DbFunctions.TruncateTime(reportingStartDate)
                        && DbFunctions.TruncateTime(q.TreatyBenefitCodeMapping.ReportingEndDate) >= DbFunctions.TruncateTime(reportingStartDate)
                        ||
                        DbFunctions.TruncateTime(q.TreatyBenefitCodeMapping.ReportingStartDate) <= DbFunctions.TruncateTime(reportingEndDate)
                        && DbFunctions.TruncateTime(q.TreatyBenefitCodeMapping.ReportingEndDate) >= DbFunctions.TruncateTime(reportingEndDate)
                    )
                    || (q.TreatyBenefitCodeMapping.ReportingStartDate == null && q.TreatyBenefitCodeMapping.ReportingEndDate == null)
                );
            }

            if (reinsuranceIssueAgeFrom != null && reinsuranceIssueAgeTo != null)
            {
                query = query
                    .Where(q =>
                    (
                        q.TreatyBenefitCodeMapping.ReinsuranceIssueAgeFrom <= reinsuranceIssueAgeFrom && q.TreatyBenefitCodeMapping.ReinsuranceIssueAgeTo >= reinsuranceIssueAgeFrom
                        ||
                        q.TreatyBenefitCodeMapping.ReinsuranceIssueAgeFrom <= reinsuranceIssueAgeTo && q.TreatyBenefitCodeMapping.ReinsuranceIssueAgeTo >= reinsuranceIssueAgeTo
                    )
                    || (q.TreatyBenefitCodeMapping.ReinsuranceIssueAgeFrom == null && q.TreatyBenefitCodeMapping.ReinsuranceIssueAgeTo == null)
                );
            }

            if (treatyBenefitCodeMappingId != null)
            {
                query = query.Where(q => q.TreatyBenefitCodeMappingId != treatyBenefitCodeMappingId);
            }

            return query;
        }

        public static IQueryable<TreatyBenefitCodeMappingDetail> QueryByTreatyParams(
            AppDbContext db,
            int cedantId,
            string cedingPlanCode,
            string cedingBenefitTypeCode,
            DateTime? reinsEffDatePol,
            string cedingBenefitRiskCode = null,
            string cedingTreatyCode = null,
            string campaignCode = null,
            int? reinsBasisCodeId = null,
            int? insuredAttAge = null,
            DateTime? reportDate = null,
            double? underwriterRating = null,
            double? oriSumAssured = null,
            int? reinsuranceIssueAge = null,
            bool groupById = false
        )
        {
            db.Database.CommandTimeout = 0;

            EFExecutionStrategy connectionStrategy = EFExecutionStrategy.GetNewRiDataRowProcessingInstance(455, "TreatyBenefitCodeMappingDetail");

            var query = connectionStrategy.Execute(() => db.TreatyBenefitCodeMappingDetails
                          .Where(q => q.Type == TreatyBenefitCodeMappingDetailBo.CombinationTypeTreaty) // IMPORTANT
                          .Where(q => q.TreatyBenefitCodeMapping.CedantId == cedantId)
                          .Where(q => q.CedingPlanCode.Trim() == cedingPlanCode.Trim())
                          .Where(q => q.CedingBenefitTypeCode.Trim() == cedingBenefitTypeCode.Trim()));

            if (reinsEffDatePol != null)
            {
                connectionStrategy.Reset(464);
                query = connectionStrategy.Execute(() => query
                   .Where(q =>
                       (
                           DbFunctions.TruncateTime(q.TreatyBenefitCodeMapping.ReinsEffDatePolStartDate) <= DbFunctions.TruncateTime(reinsEffDatePol)
                           && DbFunctions.TruncateTime(q.TreatyBenefitCodeMapping.ReinsEffDatePolEndDate) >= DbFunctions.TruncateTime(reinsEffDatePol)
                           ||
                           DbFunctions.TruncateTime(q.TreatyBenefitCodeMapping.ReinsEffDatePolStartDate) <= DbFunctions.TruncateTime(reinsEffDatePol)
                           && DbFunctions.TruncateTime(q.TreatyBenefitCodeMapping.ReinsEffDatePolEndDate) >= DbFunctions.TruncateTime(reinsEffDatePol)
                       )
                       ||
                       (q.TreatyBenefitCodeMapping.ReinsEffDatePolStartDate == null && q.TreatyBenefitCodeMapping.ReinsEffDatePolEndDate == null)
                   ));
            }
            else
            {
                connectionStrategy.Reset(480);
                query = connectionStrategy.Execute(() => query
                    .Where(q => q.TreatyBenefitCodeMapping.ReinsEffDatePolStartDate == null && q.TreatyBenefitCodeMapping.ReinsEffDatePolEndDate == null));
            }

            if (!string.IsNullOrEmpty(cedingBenefitRiskCode))
            {
                connectionStrategy.Reset(487);
                query = connectionStrategy.Execute(() => query
                    .Where(q =>
                        (!string.IsNullOrEmpty(q.CedingBenefitRiskCode) && q.CedingBenefitRiskCode.Trim() == cedingBenefitRiskCode.Trim())
                        ||
                        q.CedingBenefitRiskCode == null
                    ));
            }
            else
            {
                connectionStrategy.Reset(497);
                query = connectionStrategy.Execute(() => query
                    .Where(q => q.CedingBenefitRiskCode == null));
            }

            if (!string.IsNullOrEmpty(cedingTreatyCode))
            {
                connectionStrategy.Reset(504);
                query = connectionStrategy.Execute(() => query
                    .Where(q =>
                        (!string.IsNullOrEmpty(q.CedingTreatyCode) && q.CedingTreatyCode.Trim() == cedingTreatyCode.Trim())
                        ||
                        q.CedingTreatyCode == null
                    ));
            }
            else
            {
                connectionStrategy.Reset(514);
                query = connectionStrategy.Execute(() => query
                    .Where(q => q.CedingTreatyCode == null));
            }

            if (!string.IsNullOrEmpty(campaignCode))
            {
                connectionStrategy.Reset(521);
                query = connectionStrategy.Execute(() => query
                    .Where(q =>
                        (!string.IsNullOrEmpty(q.CampaignCode) && q.CampaignCode.Trim() == campaignCode.Trim())
                        ||
                        q.CampaignCode == null
                    ));
            }
            else
            {
                connectionStrategy.Reset(531);
                query = connectionStrategy.Execute(() => query
                    .Where(q => q.CampaignCode == null));
            }

            if (reinsBasisCodeId != null)
            {
                connectionStrategy.Reset(538);
                query = connectionStrategy.Execute(() => query
                    .Where(q =>
                        (q.TreatyBenefitCodeMapping.ReinsBasisCodePickListDetailId != null && q.TreatyBenefitCodeMapping.ReinsBasisCodePickListDetailId == reinsBasisCodeId)
                        ||
                        q.TreatyBenefitCodeMapping.ReinsBasisCodePickListDetailId == null
                    ));
            }
            else
            {
                connectionStrategy.Reset(548);
                query = connectionStrategy.Execute(() => query
                    .Where(q => q.TreatyBenefitCodeMapping.ReinsBasisCodePickListDetailId == null));
            }

            if (insuredAttAge != null)
            {
                connectionStrategy.Reset(555);
                query = connectionStrategy.Execute(() => query
                    .Where(q =>
                        (
                            (
                                q.TreatyBenefitCodeMapping.AttainedAgeFrom <= insuredAttAge && q.TreatyBenefitCodeMapping.AttainedAgeTo >= insuredAttAge
                                ||
                                q.TreatyBenefitCodeMapping.AttainedAgeFrom <= insuredAttAge && q.TreatyBenefitCodeMapping.AttainedAgeTo >= insuredAttAge
                            )
                            ||
                            (q.TreatyBenefitCodeMapping.AttainedAgeFrom == null && q.TreatyBenefitCodeMapping.AttainedAgeTo == null)
                        )
                    ));
            }
            else
            {
                connectionStrategy.Reset(571);
                query = connectionStrategy.Execute(() => query
                    .Where(q => q.TreatyBenefitCodeMapping.AttainedAgeFrom == null && q.TreatyBenefitCodeMapping.AttainedAgeTo == null));
            }

            if (reportDate != null)
            {
                connectionStrategy.Reset(578);
                query = connectionStrategy.Execute(() => query
                    .Where(q =>
                        (
                            DbFunctions.TruncateTime(q.TreatyBenefitCodeMapping.ReportingStartDate) <= DbFunctions.TruncateTime(reportDate)
                            && DbFunctions.TruncateTime(q.TreatyBenefitCodeMapping.ReportingEndDate) >= DbFunctions.TruncateTime(reportDate)
                            ||
                            DbFunctions.TruncateTime(q.TreatyBenefitCodeMapping.ReportingStartDate) <= DbFunctions.TruncateTime(reportDate)
                            && DbFunctions.TruncateTime(q.TreatyBenefitCodeMapping.ReportingEndDate) >= DbFunctions.TruncateTime(reportDate)
                        )
                        ||
                        (q.TreatyBenefitCodeMapping.ReportingStartDate == null && q.TreatyBenefitCodeMapping.ReportingEndDate == null)
                    ));
            }
            else
            {
                connectionStrategy.Reset(594);
                query = connectionStrategy.Execute(() => query
                    .Where(q => q.TreatyBenefitCodeMapping.ReportingStartDate == null && q.TreatyBenefitCodeMapping.ReportingEndDate == null));
            }

            if (underwriterRating.HasValue)
            {
                connectionStrategy.Reset(601);
                query = connectionStrategy.Execute(() => query
                    .Where(q =>
                        (
                            q.TreatyBenefitCodeMapping.UnderwriterRatingFrom <= underwriterRating && q.TreatyBenefitCodeMapping.UnderwriterRatingTo >= underwriterRating
                            ||
                            q.TreatyBenefitCodeMapping.UnderwriterRatingFrom == null && q.TreatyBenefitCodeMapping.UnderwriterRatingTo == null
                        )
                    ));
            }
            else
            {
                connectionStrategy.Reset(613);
                query = connectionStrategy.Execute(() => query
                    .Where(q => q.TreatyBenefitCodeMapping.UnderwriterRatingFrom == null && q.TreatyBenefitCodeMapping.UnderwriterRatingTo == null));
            }

            if (oriSumAssured.HasValue)
            {
                connectionStrategy.Reset(620);
                query = connectionStrategy.Execute(() => query
                    .Where(q =>
                        (
                            q.TreatyBenefitCodeMapping.OriSumAssuredFrom <= underwriterRating && q.TreatyBenefitCodeMapping.OriSumAssuredTo >= oriSumAssured
                            ||
                            q.TreatyBenefitCodeMapping.OriSumAssuredFrom == null && q.TreatyBenefitCodeMapping.OriSumAssuredTo == null
                        )
                    ));
            }
            else
            {
                connectionStrategy.Reset(632);
                query = connectionStrategy.Execute(() => query
                    .Where(q => q.TreatyBenefitCodeMapping.OriSumAssuredFrom == null && q.TreatyBenefitCodeMapping.OriSumAssuredTo == null));
            }

            if (reinsuranceIssueAge != null)
            {
                connectionStrategy.Reset(639);
                query = connectionStrategy.Execute(() => query
                    .Where(q =>
                        (
                            (
                                q.TreatyBenefitCodeMapping.ReinsuranceIssueAgeFrom <= reinsuranceIssueAge && q.TreatyBenefitCodeMapping.ReinsuranceIssueAgeTo >= reinsuranceIssueAge
                                ||
                                q.TreatyBenefitCodeMapping.ReinsuranceIssueAgeFrom <= reinsuranceIssueAge && q.TreatyBenefitCodeMapping.ReinsuranceIssueAgeTo >= reinsuranceIssueAge
                            )
                            ||
                            (q.TreatyBenefitCodeMapping.ReinsuranceIssueAgeFrom == null && q.TreatyBenefitCodeMapping.ReinsuranceIssueAgeTo == null)
                        )
                    ));
            }
            else
            {
                connectionStrategy.Reset(655);
                query = connectionStrategy.Execute(() => query
                    .Where(q => q.TreatyBenefitCodeMapping.ReinsuranceIssueAgeFrom == null && q.TreatyBenefitCodeMapping.ReinsuranceIssueAgeTo == null));
            }

            // NOTE: Group by should put at the end of query
            if (groupById)
            {
                connectionStrategy.Reset(663);
                query = connectionStrategy.Execute(() => query.GroupBy(q => q.TreatyBenefitCodeMappingId).Select(q => q.FirstOrDefault()));
            }

            return query;
        }

        public static IQueryable<TreatyBenefitCodeMappingDetail> QueryByTreatyParamsForClaim(
            AppDbContext db,
            int cedantId,
            string cedingPlanCode,
            string cedingBenefitTypeCode,
            DateTime? reinsEffDatePol,
            string cedingBenefitRiskCode = null,
            string cedingTreatyCode = null,
            string campaignCode = null,
            int? reinsBasisCodeId = null,
            bool groupById = false
        )
        {
            var query = db.TreatyBenefitCodeMappingDetails
                          .Where(q => q.Type == TreatyBenefitCodeMappingDetailBo.CombinationTypeTreaty) // IMPORTANT
                          .Where(q => q.TreatyBenefitCodeMapping.CedantId == cedantId)
                          .Where(q => q.CedingPlanCode.Trim() == cedingPlanCode.Trim())
                          .Where(q => q.CedingBenefitTypeCode.Trim() == cedingBenefitTypeCode.Trim());

            if (reinsEffDatePol != null)
            {
                query = query
                    .Where(q =>
                        (
                            DbFunctions.TruncateTime(q.TreatyBenefitCodeMapping.ReinsEffDatePolStartDate) <= DbFunctions.TruncateTime(reinsEffDatePol)
                            && DbFunctions.TruncateTime(q.TreatyBenefitCodeMapping.ReinsEffDatePolEndDate) >= DbFunctions.TruncateTime(reinsEffDatePol)
                            ||
                            DbFunctions.TruncateTime(q.TreatyBenefitCodeMapping.ReinsEffDatePolStartDate) <= DbFunctions.TruncateTime(reinsEffDatePol)
                            && DbFunctions.TruncateTime(q.TreatyBenefitCodeMapping.ReinsEffDatePolEndDate) >= DbFunctions.TruncateTime(reinsEffDatePol)
                        )
                        ||
                        (q.TreatyBenefitCodeMapping.ReinsEffDatePolStartDate == null && q.TreatyBenefitCodeMapping.ReinsEffDatePolEndDate == null)
                    );
            }
            else
            {
                query = query
                    .Where(q => q.TreatyBenefitCodeMapping.ReinsEffDatePolStartDate == null && q.TreatyBenefitCodeMapping.ReinsEffDatePolEndDate == null);
            }

            if (!string.IsNullOrEmpty(cedingBenefitRiskCode))
            {
                query = query
                    .Where(q =>
                        (!string.IsNullOrEmpty(q.CedingBenefitRiskCode) && q.CedingBenefitRiskCode.Trim() == cedingBenefitRiskCode.Trim())
                        ||
                        q.CedingBenefitRiskCode == null
                    );
            }
            else
            {
                query = query
                    .Where(q => q.CedingBenefitRiskCode == null);
            }

            if (!string.IsNullOrEmpty(cedingTreatyCode))
            {
                query = query
                    .Where(q =>
                        (!string.IsNullOrEmpty(q.CedingTreatyCode) && q.CedingTreatyCode.Trim() == cedingTreatyCode.Trim())
                        ||
                        q.CedingTreatyCode == null
                    );
            }
            else
            {
                query = query
                    .Where(q => q.CedingTreatyCode == null);
            }

            if (!string.IsNullOrEmpty(campaignCode))
            {
                query = query
                    .Where(q =>
                        (!string.IsNullOrEmpty(q.CampaignCode) && q.CampaignCode.Trim() == campaignCode.Trim())
                        ||
                        q.CampaignCode == null
                    );
            }
            else
            {
                query = query
                    .Where(q => q.CampaignCode == null);
            }

            if (reinsBasisCodeId != null)
            {
                query = query
                    .Where(q =>
                        (q.TreatyBenefitCodeMapping.ReinsBasisCodePickListDetailId != null && q.TreatyBenefitCodeMapping.ReinsBasisCodePickListDetailId == reinsBasisCodeId)
                        ||
                        q.TreatyBenefitCodeMapping.ReinsBasisCodePickListDetailId == null
                    );
            }
            else
            {
                query = query
                    .Where(q => q.TreatyBenefitCodeMapping.ReinsBasisCodePickListDetailId == null);
            }

            // NOTE: Group by should put at the end of query
            if (groupById)
            {
                query = query.GroupBy(q => q.TreatyBenefitCodeMappingId).Select(q => q.FirstOrDefault());
            }

            return query;
        }

        public static IQueryable<TreatyBenefitCodeMappingDetail> QueryByBenefitParams(
            AppDbContext db,
            int cedantId,
            string cedingPlanCode,
            string cedingBenefitTypeCode,
            DateTime? reinsEffDatePol,
            string cedingBenefitRiskCode = null,
            int? insuredAttAge = null,
            DateTime? reportDate = null,
            string treatyCode = null,
            string cedingTreatyCode = null,
            string campaignCode = null,
            int? reinsBasisCodeId = null,
            double? underwriterRating = null,
            double? oriSumAssured = null,
            int? reinsuranceIssueAge = null,
            bool groupById = false
        )
        {
            db.Database.CommandTimeout = 0;

            EFExecutionStrategy connectionStrategy = EFExecutionStrategy.GetNewRiDataRowProcessingInstance(801, "TreatyBenefitCodeMappingDetail");

            return connectionStrategy.Execute(() =>
            {
                connectionStrategy.Reset(804);
                var query = db.TreatyBenefitCodeMappingDetails
                           .Where(q => q.Type == TreatyBenefitCodeMappingDetailBo.CombinationTypeBenefit) // IMPORTANT
                           .Where(q => q.TreatyBenefitCodeMapping.CedantId == cedantId)
                           .Where(q => q.CedingPlanCode.Trim() == cedingPlanCode.Trim())
                           .Where(q => q.CedingBenefitTypeCode.Trim() == cedingBenefitTypeCode.Trim());

                if (reinsEffDatePol != null)
                {
                    connectionStrategy.Reset(813);
                    query = query.Where(q =>
                            (
                                DbFunctions.TruncateTime(q.TreatyBenefitCodeMapping.ReinsEffDatePolStartDate) <= DbFunctions.TruncateTime(reinsEffDatePol)
                                && DbFunctions.TruncateTime(q.TreatyBenefitCodeMapping.ReinsEffDatePolEndDate) >= DbFunctions.TruncateTime(reinsEffDatePol)
                                ||
                                DbFunctions.TruncateTime(q.TreatyBenefitCodeMapping.ReinsEffDatePolStartDate) <= DbFunctions.TruncateTime(reinsEffDatePol)
                                && DbFunctions.TruncateTime(q.TreatyBenefitCodeMapping.ReinsEffDatePolEndDate) >= DbFunctions.TruncateTime(reinsEffDatePol)
                            )
                            ||
                            (q.TreatyBenefitCodeMapping.ReinsEffDatePolStartDate == null && q.TreatyBenefitCodeMapping.ReinsEffDatePolEndDate == null)
                        );
                }
                else
                {
                    connectionStrategy.Reset(827);
                    query = query.Where(q => q.TreatyBenefitCodeMapping.ReinsEffDatePolStartDate == null && q.TreatyBenefitCodeMapping.ReinsEffDatePolEndDate == null);
                }

                if (!string.IsNullOrEmpty(cedingBenefitRiskCode))
                {
                    connectionStrategy.Reset(833);
                    query = query.Where(q => (!string.IsNullOrEmpty(q.CedingBenefitRiskCode) && q.CedingBenefitRiskCode.Trim() == cedingBenefitRiskCode.Trim()) || q.CedingBenefitRiskCode == null);
                }
                else
                {
                    connectionStrategy.Reset(839);
                    query = query.Where(q => q.CedingBenefitRiskCode == null);
                }

                if (insuredAttAge != null)
                {
                    connectionStrategy.Reset(844);
                    query = query.Where(q =>
                            (
                                (
                                    q.TreatyBenefitCodeMapping.AttainedAgeFrom <= insuredAttAge && q.TreatyBenefitCodeMapping.AttainedAgeTo >= insuredAttAge
                                    ||
                                    q.TreatyBenefitCodeMapping.AttainedAgeFrom <= insuredAttAge && q.TreatyBenefitCodeMapping.AttainedAgeTo >= insuredAttAge
                                )
                                ||
                                (q.TreatyBenefitCodeMapping.AttainedAgeFrom == null && q.TreatyBenefitCodeMapping.AttainedAgeTo == null)
                            )
                        );
                }
                else
                {
                    connectionStrategy.Reset(860);
                    query = query.Where(q => q.TreatyBenefitCodeMapping.AttainedAgeFrom == null && q.TreatyBenefitCodeMapping.AttainedAgeTo == null);
                }

                if (reportDate != null)
                {
                    connectionStrategy.Reset(866);
                    query = query.Where(q =>
                            (
                                DbFunctions.TruncateTime(q.TreatyBenefitCodeMapping.ReportingStartDate) <= DbFunctions.TruncateTime(reportDate)
                                && DbFunctions.TruncateTime(q.TreatyBenefitCodeMapping.ReportingEndDate) >= DbFunctions.TruncateTime(reportDate)
                                ||
                                DbFunctions.TruncateTime(q.TreatyBenefitCodeMapping.ReportingStartDate) <= DbFunctions.TruncateTime(reportDate)
                                && DbFunctions.TruncateTime(q.TreatyBenefitCodeMapping.ReportingEndDate) >= DbFunctions.TruncateTime(reportDate)
                            )
                            ||
                            (q.TreatyBenefitCodeMapping.ReportingStartDate == null && q.TreatyBenefitCodeMapping.ReportingEndDate == null)
                        );
                }
                else
                {
                    connectionStrategy.Reset(880);
                    query = query.Where(q => q.TreatyBenefitCodeMapping.ReportingStartDate == null && q.TreatyBenefitCodeMapping.ReportingEndDate == null);
                }

                if (!string.IsNullOrEmpty(treatyCode))
                {
                    connectionStrategy.Reset(887);
                    query = query.Where(q => q.TreatyBenefitCodeMapping.TreatyCode.Code.Trim() == treatyCode.Trim());
                }
                else
                {
                    connectionStrategy.Reset(891);
                    query = query.Where(q => q.TreatyBenefitCodeMapping.TreatyCode == null);
                }

                if (!string.IsNullOrEmpty(cedingTreatyCode))
                {
                    connectionStrategy.Reset(897);
                    query = query.Where(q =>
                            (!string.IsNullOrEmpty(q.CedingTreatyCode) && q.CedingTreatyCode.Trim() == cedingTreatyCode.Trim())
                            ||
                            q.CedingTreatyCode == null);
                }
                else
                {
                    connectionStrategy.Reset(905);
                    query = query.Where(q => q.CedingTreatyCode == null);
                }

                if (!string.IsNullOrEmpty(campaignCode))
                {
                    connectionStrategy.Reset(912);
                    query = query.Where(q =>
                            (!string.IsNullOrEmpty(q.CampaignCode) && q.CampaignCode.Trim() == campaignCode.Trim())
                            ||
                            q.CampaignCode == null);
                }
                else
                {
                    connectionStrategy.Reset(919);
                    query = query.Where(q => q.CampaignCode == null);
                }

                if (reinsBasisCodeId != null)
                {
                    connectionStrategy.Reset(925);
                    query = query.Where(q =>
                            (q.TreatyBenefitCodeMapping.ReinsBasisCodePickListDetailId != null && q.TreatyBenefitCodeMapping.ReinsBasisCodePickListDetailId == reinsBasisCodeId)
                            ||
                            q.TreatyBenefitCodeMapping.ReinsBasisCodePickListDetailId == null
                        );
                }
                else
                {
                    connectionStrategy.Reset(934);
                    query = query.Where(q => q.TreatyBenefitCodeMapping.ReinsBasisCodePickListDetailId == null);
                }

                if (underwriterRating.HasValue)
                {
                    connectionStrategy.Reset(941);
                    query = query.Where(q =>
                            (
                                q.TreatyBenefitCodeMapping.UnderwriterRatingFrom <= underwriterRating && q.TreatyBenefitCodeMapping.UnderwriterRatingTo >= underwriterRating
                                ||
                                q.TreatyBenefitCodeMapping.UnderwriterRatingFrom == null && q.TreatyBenefitCodeMapping.UnderwriterRatingTo == null
                            )
                        );
                }
                else
                {
                    connectionStrategy.Reset(976);
                    query = query.Where(q => q.TreatyBenefitCodeMapping.UnderwriterRatingFrom == null && q.TreatyBenefitCodeMapping.UnderwriterRatingTo == null);
                }

                if (oriSumAssured.HasValue)
                {
                    connectionStrategy.Reset(957);
                    query = query.Where(q =>
                            (
                                q.TreatyBenefitCodeMapping.OriSumAssuredFrom <= underwriterRating && q.TreatyBenefitCodeMapping.OriSumAssuredTo >= oriSumAssured
                                ||
                                q.TreatyBenefitCodeMapping.OriSumAssuredFrom == null && q.TreatyBenefitCodeMapping.OriSumAssuredTo == null
                            )
                        );
                }
                else
                {
                    connectionStrategy.Reset(968);
                    query = query.Where(q => q.TreatyBenefitCodeMapping.UnderwriterRatingFrom == null && q.TreatyBenefitCodeMapping.OriSumAssuredTo == null);
                }

                if (reinsuranceIssueAge != null)
                {
                    connectionStrategy.Reset(975);
                    query = query.Where(q =>
                            (
                                (
                                    q.TreatyBenefitCodeMapping.ReinsuranceIssueAgeFrom <= reinsuranceIssueAge && q.TreatyBenefitCodeMapping.ReinsuranceIssueAgeTo >= reinsuranceIssueAge
                                    ||
                                    q.TreatyBenefitCodeMapping.ReinsuranceIssueAgeFrom <= reinsuranceIssueAge && q.TreatyBenefitCodeMapping.ReinsuranceIssueAgeTo >= reinsuranceIssueAge
                                )
                                ||
                                (q.TreatyBenefitCodeMapping.ReinsuranceIssueAgeFrom == null && q.TreatyBenefitCodeMapping.ReinsuranceIssueAgeTo == null)
                            )
                        );
                }
                else
                {
                    connectionStrategy.Reset(990);
                    query = query.Where(q => q.TreatyBenefitCodeMapping.ReinsuranceIssueAgeFrom == null && q.TreatyBenefitCodeMapping.ReinsuranceIssueAgeTo == null);
                }

                // NOTE: Group by should put at the end of query
                if (groupById)
                {
                    connectionStrategy.Reset(897);
                    query = query.GroupBy(q => q.TreatyBenefitCodeMappingId).Select(q => q.FirstOrDefault());
                }

                return query;
            });

        }

        public static IQueryable<TreatyBenefitCodeMappingDetail> QueryByProductFeatureParams(
            AppDbContext db,
            int cedantId,
            string cedingPlanCode,
            string cedingBenefitTypeCode,
            string treatyCode,
            string benefitCode,
            DateTime? reinsEffDatePol,
            int? insuredAttAge = null,
            string campaignCode = null,
            double? underwriterRating = null,
            double? oriSumAssured = null,
            int? reinsuranceIssueAge = null,
            DateTime? reportDate = null,
            string cedingBenefitRiskCode = null,
            string cedingTreatyCode = null,
            int? reinsBasisCodeId = null,
            bool groupById = false
        )
        {
            db.Database.CommandTimeout = 0;

            EFExecutionStrategy connectionStrategy = EFExecutionStrategy.GetNewRiDataRowProcessingInstance(1029, "TreatyBenefitCodeMappingDetail");

            return connectionStrategy.Execute(() =>
            {
                connectionStrategy.Reset(1032);
                var query = db.TreatyBenefitCodeMappingDetails
                           .Where(q => q.Type == TreatyBenefitCodeMappingDetailBo.CombinationTypeProductFeature) // IMPORTANT
                           .Where(q => q.TreatyBenefitCodeMapping.CedantId == cedantId)
                           .Where(q => q.CedingPlanCode.Trim() == cedingPlanCode.Trim())
                           .Where(q => q.TreatyBenefitCodeMapping.TreatyCode.Code.Trim() == treatyCode.Trim())
                           .Where(q => q.TreatyBenefitCodeMapping.Benefit.Code.Trim() == benefitCode.Trim())
                           .Where(q => q.CedingBenefitTypeCode.Trim() == cedingBenefitTypeCode.Trim());

                if (reinsEffDatePol.HasValue)
                {
                    connectionStrategy.Reset(1044);
                    query = query.Where(q =>
                            (
                                DbFunctions.TruncateTime(q.TreatyBenefitCodeMapping.ReinsEffDatePolStartDate) <= DbFunctions.TruncateTime(reinsEffDatePol)
                                && DbFunctions.TruncateTime(q.TreatyBenefitCodeMapping.ReinsEffDatePolEndDate) >= DbFunctions.TruncateTime(reinsEffDatePol)
                            )
                            ||
                            q.TreatyBenefitCodeMapping.ReinsEffDatePolStartDate == null && q.TreatyBenefitCodeMapping.ReinsEffDatePolEndDate == null
                        );
                }
                else
                {
                    connectionStrategy.Reset(1056);
                    query = query.Where(q => q.TreatyBenefitCodeMapping.ReinsEffDatePolStartDate == null && q.TreatyBenefitCodeMapping.ReinsEffDatePolEndDate == null);
                }

                if (insuredAttAge.HasValue)
                {
                    connectionStrategy.Reset(1061);
                    query = query.Where(q =>
                            (
                                q.TreatyBenefitCodeMapping.AttainedAgeFrom <= insuredAttAge && q.TreatyBenefitCodeMapping.AttainedAgeTo >= insuredAttAge
                                ||
                                q.TreatyBenefitCodeMapping.AttainedAgeFrom == null && q.TreatyBenefitCodeMapping.AttainedAgeTo == null
                            ));
                }
                else
                {
                    connectionStrategy.Reset(1070);
                    query = query.Where(q => q.TreatyBenefitCodeMapping.AttainedAgeFrom == null && q.TreatyBenefitCodeMapping.AttainedAgeTo == null);
                }

                if (!string.IsNullOrEmpty(campaignCode))
                {
                    connectionStrategy.Reset(1077);
                    query = query.Where(q =>
                            (!string.IsNullOrEmpty(q.CampaignCode) && q.CampaignCode.Trim() == campaignCode.Trim())
                            ||
                            q.CampaignCode == null
                        );
                }
                else
                {
                    connectionStrategy.Reset(1085);
                    query = query.Where(q => q.CampaignCode == null);
                }

                if (underwriterRating.HasValue)
                {
                    connectionStrategy.Reset(1093);
                    query = query.Where(q =>
                            (
                                q.TreatyBenefitCodeMapping.UnderwriterRatingFrom <= underwriterRating && q.TreatyBenefitCodeMapping.UnderwriterRatingTo >= underwriterRating
                                ||
                                q.TreatyBenefitCodeMapping.UnderwriterRatingFrom == null && q.TreatyBenefitCodeMapping.UnderwriterRatingTo == null
                            )
                        );
                }
                else
                {
                    connectionStrategy.Reset(1103);
                    query = query.Where(q => q.TreatyBenefitCodeMapping.UnderwriterRatingFrom == null && q.TreatyBenefitCodeMapping.UnderwriterRatingTo == null);
                }

                if (oriSumAssured.HasValue)
                {
                    connectionStrategy.Reset(1044);
                    query = query.Where(q =>
                            (
                                q.TreatyBenefitCodeMapping.OriSumAssuredFrom <= oriSumAssured && q.TreatyBenefitCodeMapping.OriSumAssuredTo >= oriSumAssured
                                ||
                                q.TreatyBenefitCodeMapping.OriSumAssuredFrom == null && q.TreatyBenefitCodeMapping.OriSumAssuredTo == null
                            )
                        );
                }
                else
                {
                    connectionStrategy.Reset(1120);
                    query = query.Where(q => q.TreatyBenefitCodeMapping.UnderwriterRatingFrom == null && q.TreatyBenefitCodeMapping.OriSumAssuredTo == null);
                }

                if (reinsuranceIssueAge != null)
                {
                    connectionStrategy.Reset(1126);
                    query = query.Where(q =>
                            (
                                (
                                    q.TreatyBenefitCodeMapping.ReinsuranceIssueAgeFrom <= reinsuranceIssueAge && q.TreatyBenefitCodeMapping.ReinsuranceIssueAgeTo >= reinsuranceIssueAge
                                    ||
                                    q.TreatyBenefitCodeMapping.ReinsuranceIssueAgeFrom <= reinsuranceIssueAge && q.TreatyBenefitCodeMapping.ReinsuranceIssueAgeTo >= reinsuranceIssueAge
                                )
                                ||
                                (q.TreatyBenefitCodeMapping.ReinsuranceIssueAgeFrom == null && q.TreatyBenefitCodeMapping.ReinsuranceIssueAgeTo == null)
                            )
                        );
                }
                else
                {
                    connectionStrategy.Reset(1141);
                    query = query.Where(q => q.TreatyBenefitCodeMapping.ReinsuranceIssueAgeFrom == null && q.TreatyBenefitCodeMapping.ReinsuranceIssueAgeTo == null);
                }

                if (reportDate != null)
                {
                    connectionStrategy.Reset(1147);
                    query = query.Where(q =>
                            (
                                DbFunctions.TruncateTime(q.TreatyBenefitCodeMapping.ReportingStartDate) <= DbFunctions.TruncateTime(reportDate)
                                && DbFunctions.TruncateTime(q.TreatyBenefitCodeMapping.ReportingEndDate) >= DbFunctions.TruncateTime(reportDate)
                                ||
                                DbFunctions.TruncateTime(q.TreatyBenefitCodeMapping.ReportingStartDate) <= DbFunctions.TruncateTime(reportDate)
                                && DbFunctions.TruncateTime(q.TreatyBenefitCodeMapping.ReportingEndDate) >= DbFunctions.TruncateTime(reportDate)
                            )
                            ||
                            (q.TreatyBenefitCodeMapping.ReportingStartDate == null && q.TreatyBenefitCodeMapping.ReportingEndDate == null)
                        );
                }
                else
                {
                    connectionStrategy.Reset(1161);
                    query = query.Where(q => q.TreatyBenefitCodeMapping.ReportingStartDate == null && q.TreatyBenefitCodeMapping.ReportingEndDate == null);
                }

                if (!string.IsNullOrEmpty(cedingBenefitRiskCode))
                {
                    connectionStrategy.Reset(1168);
                    query = query.Where(q =>
                            (!string.IsNullOrEmpty(q.CedingBenefitRiskCode) && q.CedingBenefitRiskCode.Trim() == cedingBenefitRiskCode.Trim())
                            ||
                            q.CedingBenefitRiskCode == null
                        );
                }
                else
                {
                    connectionStrategy.Reset(1176);
                    query = query.Where(q => q.CedingBenefitRiskCode == null);
                }

                if (!string.IsNullOrEmpty(cedingTreatyCode))
                {
                    connectionStrategy.Reset(1182);
                    query = query.Where(q =>
                            (!string.IsNullOrEmpty(q.CedingTreatyCode) && q.CedingTreatyCode.Trim() == cedingTreatyCode.Trim())
                            ||
                            q.CedingTreatyCode == null
                        );
                }
                else
                {
                    connectionStrategy.Reset(1191);
                    query = query.Where(q => q.CedingTreatyCode == null);
                }

                if (reinsBasisCodeId != null)
                {
                    connectionStrategy.Reset(1197);
                    query = query.Where(q =>
                            (q.TreatyBenefitCodeMapping.ReinsBasisCodePickListDetailId != null && q.TreatyBenefitCodeMapping.ReinsBasisCodePickListDetailId == reinsBasisCodeId)
                            ||
                            q.TreatyBenefitCodeMapping.ReinsBasisCodePickListDetailId == null);
                }
                else
                {
                    connectionStrategy.Reset(1205);
                    query = query.Where(q => q.TreatyBenefitCodeMapping.ReinsBasisCodePickListDetailId == null);
                }

                // NOTE: Group by should put at the end of query
                if (groupById)
                {
                    connectionStrategy.Reset(1213);
                    query = query.GroupBy(q => q.TreatyBenefitCodeMappingId).Select(q => q.FirstOrDefault());
                }

                return query;
            });
        }

        public static TreatyBenefitCodeMappingDetail Find(int id)
        {
            using (var db = new AppDbContext())
            {
                return db.TreatyBenefitCodeMappingDetails.Where(q => q.Id == id).FirstOrDefault();
            }
        }

        public static TreatyBenefitCodeMappingDetail FindCombinationForTreaty(
            int cedantId,
            string combination,
            DateTime? reinsEffDatePolStartDate,
            DateTime? reinsEffDatePolEndDate,
            int? attAgeFrom = null,
            int? attAgeTo = null,
            DateTime? reportingStartDate = null,
            DateTime? reportingEndDate = null,
            double? underwriterRatingFrom = null,
            double? underwriterRatingTo = null,
            double? oriSumAssuredFrom = null,
            double? oriSumAssuredTo = null,
            int? reinsuranceIssueAgeFrom = null,
            int? reinsuranceIssueAgeTo = null,
            int? treatyBenefitCodeMappingId = null
        )
        {
            using (var db = new AppDbContext())
            {
                return QueryCombinationForTreaty(
                        db,
                        cedantId,
                        combination,
                        reinsEffDatePolStartDate,
                        reinsEffDatePolEndDate,
                        attAgeFrom,
                        attAgeTo,
                        reportingStartDate,
                        reportingEndDate,
                        underwriterRatingFrom,
                        underwriterRatingTo,
                        oriSumAssuredFrom,
                        oriSumAssuredTo,
                        reinsuranceIssueAgeFrom,
                        reinsuranceIssueAgeTo,
                        treatyBenefitCodeMappingId
                    ).FirstOrDefault();
            }
        }

        public static TreatyBenefitCodeMappingDetail FindByTreatyParams(
            int cedantId,
            string cedingPlanCode,
            string cedingBenefitTypeCode,
            DateTime? reinsEffDatePol,
            string cedingBenefitRiskCode = null,
            string cedingTreatyCode = null,
            string campaignCode = null,
            int? reinsBasisCodeId = null,
            int? insuredAttAge = null,
            DateTime? reportDate = null,
            double? underwriterRating = null,
            double? oriSumAssured = null,
            int? reinsuranceIssueAge = null,
            bool groupById = false
        )
        {
            using (var db = new AppDbContext())
            {
                return QueryByTreatyParams(
                    db,
                    cedantId,
                    cedingPlanCode,
                    cedingBenefitTypeCode,
                    reinsEffDatePol,
                    cedingBenefitRiskCode,
                    cedingTreatyCode,
                    campaignCode,
                    reinsBasisCodeId,
                    insuredAttAge,
                    reportDate,
                    underwriterRating,
                    oriSumAssured,
                    reinsuranceIssueAge,
                    groupById
                ).FirstOrDefault();
            }
        }

        public static TreatyBenefitCodeMappingDetail FindByTreatyParamsForClaim(
            int cedantId,
            string cedingPlanCode,
            string cedingBenefitTypeCode,
            DateTime? reinsEffDatePol,
            string cedingBenefitRiskCode = null,
            string cedingTreatyCode = null,
            string campaignCode = null,
            int? reinsBasisCodeId = null,
            bool groupById = false
        )
        {
            using (var db = new AppDbContext())
            {
                return QueryByTreatyParamsForClaim(
                    db,
                    cedantId,
                    cedingPlanCode,
                    cedingBenefitTypeCode,
                    reinsEffDatePol,
                    cedingBenefitRiskCode,
                    cedingTreatyCode,
                    campaignCode,
                    reinsBasisCodeId,
                    groupById
                ).FirstOrDefault();
            }
        }

        public static TreatyBenefitCodeMappingDetail FindCombinationForBenefit(
            int cedantId,
            string combination,
            int? attAgeFrom,
            int? attAgeTo,
            DateTime? reportingStartDate,
            DateTime? reportingEndDate,
            int? treatyCodeId,
            DateTime? reinsEffDatePolStartDate,
            DateTime? reinsEffDatePolEndDate,
            double? underwriterRatingFrom,
            double? underwriterRatingTo,
            double? oriSumAssuredFrom,
            double? oriSumAssuredTo,
            int? reinsuranceIssueAgeFrom,
            int? reinsuranceIssueAgeTo,
            int? treatyBenefitCodeMappingId = null
        )
        {
            using (var db = new AppDbContext())
            {
                return QueryCombinationForBenefit(
                        db,
                        cedantId,
                        combination,
                        attAgeFrom,
                        attAgeTo,
                        reportingStartDate,
                        reportingEndDate,
                        treatyCodeId,
                        reinsEffDatePolStartDate,
                        reinsEffDatePolEndDate,
                        underwriterRatingFrom,
                        underwriterRatingTo,
                        oriSumAssuredFrom,
                        oriSumAssuredTo,
                        reinsuranceIssueAgeFrom,
                        reinsuranceIssueAgeTo,
                        treatyBenefitCodeMappingId
                    ).FirstOrDefault();
            }
        }

        public static TreatyBenefitCodeMappingDetail FindByBenefitParams(
            int cedantId,
            string cedingPlanCode,
            string cedingBenefitTypeCode,
            DateTime? reinsEffDatePol,
            string cedingBenefitRiskCode = null,
            int? insuredAttAge = null,
            DateTime? reportDate = null,
            string treatyCode = null,
            string cedingTreatyCode = null,
            string campaignCode = null,
            int? reinsBasisCodeId = null,
            double? underwriterRating = null,
            double? oriSumAssured = null,
            int? reinsuranceIssueAge = null,
            bool groupById = false
        )
        {
            using (var db = new AppDbContext())
            {
                return QueryByBenefitParams(
                    db,
                    cedantId,
                    cedingPlanCode,
                    cedingBenefitTypeCode,
                    reinsEffDatePol,
                    cedingBenefitRiskCode,
                    insuredAttAge,
                    reportDate,
                    treatyCode,
                    cedingTreatyCode,
                    campaignCode,
                    reinsBasisCodeId,
                    underwriterRating,
                    oriSumAssured,
                    reinsuranceIssueAge,
                    groupById
                ).FirstOrDefault();
            }
        }

        public static TreatyBenefitCodeMappingDetail FindCombinationForProductFeature(
            int cedantId,
            string combination,
            DateTime? reinsEffDatePolStartDate,
            DateTime? reinsEffDatePolEndDate,
            int? attAgeFrom = null,
            int? attAgeTo = null,
            int? treatyCodeId = null,
            int? benefitId = null,
            double? underwriterRatingFrom = null,
            double? underwriterRatingTo = null,
            double? oriSumAssuredFrom = null,
            double? oriSumAssuredTo = null,
            int? reinsuranceIssueAgeFrom = null,
            int? reinsuranceIssueAgeTo = null,
            DateTime? reportingStartDate = null,
            DateTime? reportingEndDate = null,
            int? treatyBenefitCodeMappingId = null
        )
        {
            using (var db = new AppDbContext())
            {
                return QueryCombinationForProductFeature(
                        db,
                        cedantId,
                        combination,
                        reinsEffDatePolStartDate,
                        reinsEffDatePolEndDate,
                        attAgeFrom,
                        attAgeTo,
                        treatyCodeId,
                        benefitId,
                        underwriterRatingFrom,
                        underwriterRatingTo,
                        oriSumAssuredFrom,
                        oriSumAssuredTo,
                        reinsuranceIssueAgeFrom,
                        reinsuranceIssueAgeTo,
                        reportingStartDate,
                        reportingEndDate,
                        treatyBenefitCodeMappingId
                    ).FirstOrDefault();
            }
        }

        public static TreatyBenefitCodeMappingDetail FindByProductFeatureParams(
            int cedantId,
            string cedingPlanCode,
            string cedingBenefitTypeCode,
            string treatyCode,
            string benefitCode,
            DateTime? reinsEffDatePol,
            int? insuredAttAge = null,
            string campaignCode = null,
            double? underwriterRating = null,
            double? oriSumAssured = null,
            int? reinsuranceIssueAge = null,
            DateTime? reportDate = null,
            string cedingBenefitRiskCode = null,
            string cedingTreatyCode = null,
            int? reinsBasisCodeId = null,
            bool groupById = false
        )
        {
            using (var db = new AppDbContext())
            {
                return QueryByProductFeatureParams(
                    db,
                    cedantId,
                    cedingPlanCode,
                    cedingBenefitTypeCode,
                    treatyCode,
                    benefitCode,
                    reinsEffDatePol,
                    insuredAttAge,
                    campaignCode,
                    underwriterRating,
                    oriSumAssured,
                    reinsuranceIssueAge,
                    reportDate,
                    cedingBenefitRiskCode,
                    cedingTreatyCode,
                    reinsBasisCodeId,
                    groupById
                ).FirstOrDefault();
            }
        }

        public static int CountCombinationForTreaty(
            int cedantId,
            string combination,
            DateTime? reinsEffDatePolStartDate,
            DateTime? reinsEffDatePolEndDate,
            int? attAgeFrom,
            int? attAgeTo,
            DateTime? reportingStartDate,
            DateTime? reportingEndDate,
            double? underwriterRatingFrom,
            double? underwriterRatingTo,
            double? oriSumAssuredFrom,
            double? oriSumAssuredTo,
            int? reinsuranceIssueAgeFrom,
            int? reinsuranceIssueAgeTo,
            int? treatyBenefitCodeMappingId = null
        )
        {
            using (var db = new AppDbContext())
            {
                return QueryCombinationForTreaty(
                        db,
                        cedantId,
                        combination,
                        reinsEffDatePolStartDate,
                        reinsEffDatePolEndDate,
                        attAgeFrom,
                        attAgeTo,
                        reportingStartDate,
                        reportingEndDate,
                        underwriterRatingFrom,
                        underwriterRatingTo,
                        oriSumAssuredFrom,
                        oriSumAssuredTo,
                        reinsuranceIssueAgeFrom,
                        reinsuranceIssueAgeTo,
                        treatyBenefitCodeMappingId
                    ).Count();
            }
        }

        public static int CountByTreatyParams(
            int cedantId,
            string cedingPlanCode,
            string cedingBenefitTypeCode,
            DateTime? reinsEffDatePol,
            string cedingBenefitRiskCode = null,
            string cedingTreatyCode = null,
            string campaignCode = null,
            int? reinsBasisCodeId = null,
            int? insuredAttAge = null,
            DateTime? reportDate = null,
            double? underwriterRating = null,
            double? oriSumAssured = null,
            int? reinsuranceIssueAge = null,
            bool groupById = false
        )
        {
            using (var db = new AppDbContext())
            {
                return QueryByTreatyParams(
                    db,
                    cedantId,
                    cedingPlanCode,
                    cedingBenefitTypeCode,
                    reinsEffDatePol,
                    cedingBenefitRiskCode,
                    cedingTreatyCode,
                    campaignCode,
                    reinsBasisCodeId,
                    insuredAttAge,
                    reportDate,
                    underwriterRating,
                    oriSumAssured,
                    reinsuranceIssueAge,
                    groupById
                ).Count();
            }
        }

        public static int CountByTreatyParamsForClaim(
            int cedantId,
            string cedingPlanCode,
            string cedingBenefitTypeCode,
            DateTime? reinsEffDatePol,
            string cedingBenefitRiskCode = null,
            string cedingTreatyCode = null,
            string campaignCode = null,
            int? reinsBasisCodeId = null,
            bool groupById = false
        )
        {
            using (var db = new AppDbContext())
            {
                return QueryByTreatyParamsForClaim(
                    db,
                    cedantId,
                    cedingPlanCode,
                    cedingBenefitTypeCode,
                    reinsEffDatePol,
                    cedingBenefitRiskCode,
                    cedingTreatyCode,
                    campaignCode,
                    reinsBasisCodeId,
                    groupById
                ).Count();
            }
        }

        public static int CountCombinationForBenefit(
            int cedantId,
            string combination,
            int? attAgeFrom,
            int? attAgeTo,
            DateTime? reportingStartDate,
            DateTime? reportingEndDate,
            int? treatyCodeId,
            DateTime? reinsEffDatePolStartDate,
            DateTime? reinsEffDatePolEndDate,
            double? underwriterRatingFrom,
            double? underwriterRatingTo,
            double? oriSumAssuredFrom,
            double? oriSumAssuredTo,
            int? reinsuranceIssueAgeFrom,
            int? reinsuranceIssueAgeTo,
            int? treatyBenefitCodeMappingId = null
        )
        {
            using (var db = new AppDbContext())
            {
                var query = db.TreatyBenefitCodeMappingDetails
                    .Where(q => q.TreatyBenefitCodeMapping.CedantId == cedantId)
                    .Where(q => q.Combination == combination)
                    .Where(q => q.Type == TreatyBenefitCodeMappingDetailBo.CombinationTypeBenefit);

                if (attAgeFrom != null && attAgeTo != null)
                {
                    query = query
                        .Where(q =>
                        (
                            q.TreatyBenefitCodeMapping.AttainedAgeFrom <= attAgeFrom && q.TreatyBenefitCodeMapping.AttainedAgeTo >= attAgeFrom
                            ||
                            q.TreatyBenefitCodeMapping.AttainedAgeFrom <= attAgeTo && q.TreatyBenefitCodeMapping.AttainedAgeTo >= attAgeTo
                        )
                        || (q.TreatyBenefitCodeMapping.AttainedAgeFrom == null && q.TreatyBenefitCodeMapping.AttainedAgeTo == null)
                    );
                }

                if (reportingStartDate != null && reportingEndDate != null)
                {
                    query = query
                        .Where(q =>
                        (
                            DbFunctions.TruncateTime(q.TreatyBenefitCodeMapping.ReportingStartDate) <= DbFunctions.TruncateTime(reportingStartDate)
                            && DbFunctions.TruncateTime(q.TreatyBenefitCodeMapping.ReportingEndDate) >= DbFunctions.TruncateTime(reportingStartDate)
                            ||
                            DbFunctions.TruncateTime(q.TreatyBenefitCodeMapping.ReportingStartDate) <= DbFunctions.TruncateTime(reportingEndDate)
                            && DbFunctions.TruncateTime(q.TreatyBenefitCodeMapping.ReportingEndDate) >= DbFunctions.TruncateTime(reportingEndDate)
                        )
                        || (q.TreatyBenefitCodeMapping.ReportingStartDate == null && q.TreatyBenefitCodeMapping.ReportingEndDate == null)
                    );
                }

                if (treatyCodeId != null)
                {
                    query = query.Where(q => q.TreatyBenefitCodeMapping.TreatyCodeId == treatyCodeId);
                }



                if (reinsEffDatePolStartDate != null && reinsEffDatePolEndDate != null)
                {
                    query = query
                        .Where(q =>
                        (
                            DbFunctions.TruncateTime(q.TreatyBenefitCodeMapping.ReinsEffDatePolStartDate) <= DbFunctions.TruncateTime(reinsEffDatePolStartDate)
                            && DbFunctions.TruncateTime(q.TreatyBenefitCodeMapping.ReinsEffDatePolEndDate) >= DbFunctions.TruncateTime(reinsEffDatePolStartDate)
                            ||
                            DbFunctions.TruncateTime(q.TreatyBenefitCodeMapping.ReinsEffDatePolStartDate) <= DbFunctions.TruncateTime(reinsEffDatePolEndDate)
                            && DbFunctions.TruncateTime(q.TreatyBenefitCodeMapping.ReinsEffDatePolEndDate) >= DbFunctions.TruncateTime(reinsEffDatePolEndDate)
                        )
                        || (q.TreatyBenefitCodeMapping.ReinsEffDatePolStartDate == null && q.TreatyBenefitCodeMapping.ReinsEffDatePolEndDate == null)
                    );
                }

                if (underwriterRatingFrom != null && underwriterRatingTo != null)
                {
                    query = query
                        .Where(q =>
                        (
                            q.TreatyBenefitCodeMapping.UnderwriterRatingFrom <= underwriterRatingFrom && q.TreatyBenefitCodeMapping.UnderwriterRatingTo >= underwriterRatingFrom
                            ||
                            q.TreatyBenefitCodeMapping.UnderwriterRatingFrom <= underwriterRatingTo && q.TreatyBenefitCodeMapping.UnderwriterRatingTo >= underwriterRatingTo
                        )
                        || (q.TreatyBenefitCodeMapping.UnderwriterRatingFrom == null && q.TreatyBenefitCodeMapping.UnderwriterRatingTo == null)
                    );
                }

                if (oriSumAssuredFrom != null && oriSumAssuredTo != null)
                {
                    query = query
                        .Where(q =>
                        (
                            q.TreatyBenefitCodeMapping.OriSumAssuredFrom <= oriSumAssuredFrom && q.TreatyBenefitCodeMapping.OriSumAssuredTo >= oriSumAssuredFrom
                            ||
                            q.TreatyBenefitCodeMapping.OriSumAssuredFrom <= oriSumAssuredTo && q.TreatyBenefitCodeMapping.OriSumAssuredTo >= oriSumAssuredTo
                        )
                        || (q.TreatyBenefitCodeMapping.OriSumAssuredFrom == null && q.TreatyBenefitCodeMapping.OriSumAssuredTo == null)
                    );
                }

                if (reinsuranceIssueAgeFrom != null && reinsuranceIssueAgeTo != null)
                {
                    query = query
                        .Where(q =>
                        (
                            q.TreatyBenefitCodeMapping.ReinsuranceIssueAgeFrom <= reinsuranceIssueAgeFrom && q.TreatyBenefitCodeMapping.ReinsuranceIssueAgeTo >= reinsuranceIssueAgeFrom
                            ||
                            q.TreatyBenefitCodeMapping.ReinsuranceIssueAgeFrom <= reinsuranceIssueAgeTo && q.TreatyBenefitCodeMapping.ReinsuranceIssueAgeTo >= reinsuranceIssueAgeTo
                        )
                        || (q.TreatyBenefitCodeMapping.ReinsuranceIssueAgeFrom == null && q.TreatyBenefitCodeMapping.ReinsuranceIssueAgeTo == null)
                    );
                }

                if (treatyBenefitCodeMappingId != null)
                {
                    query = query.Where(q => q.TreatyBenefitCodeMappingId != treatyBenefitCodeMappingId);
                }

                return query.Count();
            }
        }

        public static int CountCombinationForProductFeature(
            int cedantId,
            string combination,
            DateTime? reinsEffDatePolStartDate,
            DateTime? reinsEffDatePolEndDate,
            int? attAgeFrom,
            int? attAgeTo,
            int? treatyCodeId,
            int? benefitId,
            double? underwriterRatingFrom,
            double? underwriterRatingTo,
            double? oriSumAssuredFrom,
            double? oriSumAssuredTo,
            int? reinsuranceIssueAgeFrom,
            int? reinsuranceIssueAgeTo,
            DateTime? reportingStartDate,
            DateTime? reportingEndDate,
            int? treatyBenefitCodeMappingId = null
        )
        {
            using (var db = new AppDbContext())
            {
                return QueryCombinationForProductFeature(
                        db,
                        cedantId,
                        combination,
                        reinsEffDatePolStartDate,
                        reinsEffDatePolEndDate,
                        attAgeFrom,
                        attAgeTo,
                        treatyCodeId,
                        benefitId,
                        underwriterRatingFrom,
                        underwriterRatingTo,
                        oriSumAssuredFrom,
                        oriSumAssuredTo,
                        reinsuranceIssueAgeFrom,
                        reinsuranceIssueAgeTo,
                        reportingStartDate,
                        reportingEndDate,
                        treatyBenefitCodeMappingId
                    ).Count();
            }
        }

        public static int CountByBenefitParams(
            int cedantId,
            string cedingPlanCode,
            string cedingBenefitTypeCode,
            DateTime? reinsEffDatePol,
            string cedingBenefitRiskCode = null,
            int? insuredAttAge = null,
            DateTime? reportDate = null,
            string treatyCode = null,
            string cedingTreatyCode = null,
            string campaignCode = null,
            int? reinsBasisCodeId = null,
            double? underwriterRating = null,
            double? oriSumAssured = null,
            int? reinsuranceIssueAge = null,
            bool groupById = false
        )
        {
            using (var db = new AppDbContext())
            {
                return QueryByBenefitParams(
                    db,
                    cedantId,
                    cedingPlanCode,
                    cedingBenefitTypeCode,
                    reinsEffDatePol,
                    cedingBenefitRiskCode,
                    insuredAttAge,
                    reportDate,
                    treatyCode,
                    cedingTreatyCode,
                    campaignCode,
                    reinsBasisCodeId,
                    underwriterRating,
                    oriSumAssured,
                    reinsuranceIssueAge,
                    groupById
                ).Count();
            }
        }

        public static int CountByProductFeatureParams(
            int cedantId,
            string cedingPlanCode,
            string cedingBenefitTypeCode,
            string treatyCode,
            string benefitCode,
            DateTime? reinsEffDatePol,
            int? insuredAttAge = null,
            string campaignCode = null,
            double? underwriterRating = null,
            double? oriSumAssured = null,
            int? reinsuranceIssueAge = null,
            DateTime? reportDate = null,
            string cedingBenefitRiskCode = null,
            string cedingTreatyCode = null,
            int? reinsBasisCodeId = null,
            bool groupById = false
        )
        {
            using (var db = new AppDbContext())
            {
                return QueryByProductFeatureParams(
                    db,
                    cedantId,
                    cedingPlanCode,
                    cedingBenefitTypeCode,
                    treatyCode,
                    benefitCode,
                    reinsEffDatePol,
                    insuredAttAge,
                    campaignCode,
                    underwriterRating,
                    oriSumAssured,
                    reinsuranceIssueAge,
                    reportDate,
                    cedingBenefitRiskCode,
                    cedingTreatyCode,
                    reinsBasisCodeId,
                    groupById
                ).Count();
            }
        }

        public static IList<TreatyBenefitCodeMappingDetail> GetByTreatyParams(
            int cedantId,
            string cedingPlanCode,
            string cedingBenefitTypeCode,
            DateTime? reinsEffDatePol,
            string cedingBenefitRiskCode = null,
            string cedingTreatyCode = null,
            string campaignCode = null,
            int? reinsBasisCodeId = null,
            int? insuredAttAge = null,
            DateTime? reportDate = null,
            double? underwriterRating = null,
            double? oriSumAssured = null,
            int? reinsuranceIssueAge = null,
            bool groupById = false
        )
        {
            using (var db = new AppDbContext())
            {
                return QueryByTreatyParams(
                    db,
                    cedantId,
                    cedingPlanCode,
                    cedingBenefitTypeCode,
                    reinsEffDatePol,
                    cedingBenefitRiskCode,
                    cedingTreatyCode,
                    campaignCode,
                    reinsBasisCodeId,
                    insuredAttAge,
                    reportDate,
                    underwriterRating,
                    oriSumAssured,
                    reinsuranceIssueAge,
                    groupById
                ).ToList();
            }
        }

        public static IList<TreatyBenefitCodeMappingDetail> GetByBenefitParams(
            int cedantId,
            string cedingPlanCode,
            string cedingBenefitTypeCode,
            DateTime? reinsEffDatePol,
            string cedingBenefitRiskCode = null,
            int? insuredAttAge = null,
            DateTime? reportDate = null,
            string treatyCode = null,
            string cedingTreatyCode = null,
            string campaignCode = null,
            int? reinsBasisCodeId = null,
            double? underwriterRating = null,
            double? oriSumAssured = null,
            int? reinsuranceIssueAge = null,
            bool groupById = false
        )
        {
            using (var db = new AppDbContext())
            {
                return QueryByBenefitParams(
                    db,
                    cedantId,
                    cedingPlanCode,
                    cedingBenefitTypeCode,
                    reinsEffDatePol,
                    cedingBenefitRiskCode,
                    insuredAttAge,
                    reportDate,
                    treatyCode,
                    cedingTreatyCode,
                    campaignCode,
                    reinsBasisCodeId,
                    underwriterRating,
                    oriSumAssured,
                    reinsuranceIssueAge,
                    groupById
                ).ToList();
            }
        }

        public static IList<TreatyBenefitCodeMappingDetail> GetByProductFeatureParams(
            int cedantId,
            string cedingPlanCode,
            string cedingBenefitTypeCode,
            string treatyCode,
            string benefitCode,
            DateTime? reinsEffDatePol,
            int? insuredAttAge = null,
            string campaignCode = null,
            double? underwriterRating = null,
            double? oriSumAssured = null,
            int? reinsuranceIssueAge = null,
            DateTime? reportDate = null,
            string cedingBenefitRiskCode = null,
            string cedingTreatyCode = null,
            int? reinsBasisCodeId = null,
            bool groupById = false
        )
        {
            using (var db = new AppDbContext())
            {
                return QueryByProductFeatureParams(
                    db,
                    cedantId,
                    cedingPlanCode,
                    cedingBenefitTypeCode,
                    treatyCode,
                    benefitCode,
                    reinsEffDatePol,
                    insuredAttAge,
                    campaignCode,
                    underwriterRating,
                    oriSumAssured,
                    reinsuranceIssueAge,
                    reportDate,
                    cedingBenefitRiskCode,
                    cedingTreatyCode,
                    reinsBasisCodeId,
                    groupById
                ).ToList();
            }
        }

        public static int CountDuplicateByParamsForTreaty(
            int cedantId,
            string cedingPlanCode,
            string cedingBenefitTypeCode,
            string cedingBenefitRiskCode,
            string cedingTreatyCode,
            string campaignCode,
            int? reinsBasisCodePickListDetailId,
            DateTime? reinsEffDatePolStartDate,
            DateTime? reinsEffDatePolEndDate,
            int? attAgeFrom,
            int? attAgeTo,
            DateTime? reportingStartDate,
            DateTime? reportingEndDate,
            double? underwriterRatingFrom,
            double? underwriterRatingTo,
            double? oriSumAssuredFrom,
            double? oriSumAssuredTo,
            int? reinsuranceIssueAgeFrom,
            int? reinsuranceIssueAgeTo,
            int? treatyBenefitCodeMappingId = null
        )
        {
            using (var db = new AppDbContext())
            {
                return QueryDuplicateByParamsForTreaty(
                        db,
                        cedantId,
                        cedingPlanCode,
                        cedingBenefitTypeCode,
                        cedingBenefitRiskCode,
                        cedingTreatyCode,
                        campaignCode,
                        reinsBasisCodePickListDetailId,
                        reinsEffDatePolStartDate,
                        reinsEffDatePolEndDate,
                        attAgeFrom,
                        attAgeTo,
                        reportingStartDate,
                        reportingEndDate,
                        underwriterRatingFrom,
                        underwriterRatingTo,
                        oriSumAssuredFrom,
                        oriSumAssuredTo,
                        reinsuranceIssueAgeFrom,
                        reinsuranceIssueAgeTo,
                        treatyBenefitCodeMappingId
                    ).Count();
            }
        }



        public static IQueryable<TreatyBenefitCodeMappingDetail> QueryDuplicateByParamsForTreaty(
            AppDbContext db,
            int cedantId,
            string cedingPlanCode,
            string cedingBenefitTypeCode,
            string cedingBenefitRiskCode,
            string cedingTreatyCode,
            string campaignCode,
            int? reinsBasisCodePickListDetailId,
            DateTime? reinsEffDatePolStartDate,
            DateTime? reinsEffDatePolEndDate,
            int? attAgeFrom,
            int? attAgeTo,
            DateTime? reportingStartDate,
            DateTime? reportingEndDate,
            double? underwriterRatingFrom,
            double? underwriterRatingTo,
            double? oriSumAssuredFrom,
            double? oriSumAssuredTo,
            int? reinsuranceIssueAgeFrom,
            int? reinsuranceIssueAgeTo,
            int? treatyBenefitCodeMappingId = null
        )
        {
            var query = db.TreatyBenefitCodeMappingDetails
                    .Where(q => q.TreatyBenefitCodeMapping.CedantId == cedantId)
                    .Where(q => q.Type == TreatyBenefitCodeMappingDetailBo.CombinationTypeTreaty);

            if (!string.IsNullOrEmpty(cedingPlanCode))
            {
                query = query
                    .Where(q => (!string.IsNullOrEmpty(q.CedingPlanCode) && q.CedingPlanCode.Trim() == cedingPlanCode.Trim()) || q.CedingPlanCode == null);
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

            if (!string.IsNullOrEmpty(cedingTreatyCode))
            {
                query = query
                    .Where(q => (!string.IsNullOrEmpty(q.CedingTreatyCode) && q.CedingTreatyCode.Trim() == cedingTreatyCode.Trim()) || q.CedingTreatyCode == null);
            }

            if (!string.IsNullOrEmpty(campaignCode))
            {
                query = query
                    .Where(q => (!string.IsNullOrEmpty(q.CampaignCode) && q.CampaignCode.Trim() == campaignCode.Trim()) || q.CampaignCode == null);
            }

            if (reinsBasisCodePickListDetailId.HasValue)
            {
                query = query
                    .Where(q => (q.TreatyBenefitCodeMapping.ReinsBasisCodePickListDetailId.HasValue && q.TreatyBenefitCodeMapping.ReinsBasisCodePickListDetailId == reinsBasisCodePickListDetailId) || !q.TreatyBenefitCodeMapping.ReinsBasisCodePickListDetailId.HasValue);
            }

            if (reinsEffDatePolStartDate != null && reinsEffDatePolEndDate != null)
            {
                query = query
                    .Where(q =>
                    (
                        DbFunctions.TruncateTime(q.TreatyBenefitCodeMapping.ReinsEffDatePolStartDate) <= DbFunctions.TruncateTime(reinsEffDatePolStartDate)
                        && DbFunctions.TruncateTime(q.TreatyBenefitCodeMapping.ReinsEffDatePolEndDate) >= DbFunctions.TruncateTime(reinsEffDatePolStartDate)
                        ||
                        DbFunctions.TruncateTime(q.TreatyBenefitCodeMapping.ReinsEffDatePolStartDate) <= DbFunctions.TruncateTime(reinsEffDatePolEndDate)
                        && DbFunctions.TruncateTime(q.TreatyBenefitCodeMapping.ReinsEffDatePolEndDate) >= DbFunctions.TruncateTime(reinsEffDatePolEndDate)
                    )
                    || (q.TreatyBenefitCodeMapping.ReinsEffDatePolStartDate == null && q.TreatyBenefitCodeMapping.ReinsEffDatePolEndDate == null)
                );
            }

            if (attAgeFrom != null && attAgeTo != null)
            {
                query = query
                    .Where(q =>
                    (
                        q.TreatyBenefitCodeMapping.AttainedAgeFrom <= attAgeFrom && q.TreatyBenefitCodeMapping.AttainedAgeTo >= attAgeFrom
                        ||
                        q.TreatyBenefitCodeMapping.AttainedAgeFrom <= attAgeTo && q.TreatyBenefitCodeMapping.AttainedAgeTo >= attAgeTo
                    )
                    || (q.TreatyBenefitCodeMapping.AttainedAgeFrom == null && q.TreatyBenefitCodeMapping.AttainedAgeTo == null)
                );
            }

            if (reportingStartDate != null && reportingEndDate != null)
            {
                query = query
                    .Where(q =>
                    (
                        DbFunctions.TruncateTime(q.TreatyBenefitCodeMapping.ReportingStartDate) <= DbFunctions.TruncateTime(reportingStartDate)
                        && DbFunctions.TruncateTime(q.TreatyBenefitCodeMapping.ReportingEndDate) >= DbFunctions.TruncateTime(reportingStartDate)
                        ||
                        DbFunctions.TruncateTime(q.TreatyBenefitCodeMapping.ReportingStartDate) <= DbFunctions.TruncateTime(reportingEndDate)
                        && DbFunctions.TruncateTime(q.TreatyBenefitCodeMapping.ReportingEndDate) >= DbFunctions.TruncateTime(reportingEndDate)
                    )
                    || (q.TreatyBenefitCodeMapping.ReportingStartDate == null && q.TreatyBenefitCodeMapping.ReportingEndDate == null)
                );
            }

            if (underwriterRatingFrom != null && underwriterRatingTo != null)
            {
                query = query
                    .Where(q =>
                    (
                        q.TreatyBenefitCodeMapping.UnderwriterRatingFrom <= underwriterRatingFrom && q.TreatyBenefitCodeMapping.UnderwriterRatingTo >= underwriterRatingFrom
                        ||
                        q.TreatyBenefitCodeMapping.UnderwriterRatingFrom <= underwriterRatingTo && q.TreatyBenefitCodeMapping.UnderwriterRatingTo >= underwriterRatingTo
                    )
                    || (q.TreatyBenefitCodeMapping.UnderwriterRatingFrom == null && q.TreatyBenefitCodeMapping.UnderwriterRatingTo == null)
                );
            }

            if (oriSumAssuredFrom != null && oriSumAssuredTo != null)
            {
                query = query
                    .Where(q =>
                    (
                        q.TreatyBenefitCodeMapping.OriSumAssuredFrom <= oriSumAssuredFrom && q.TreatyBenefitCodeMapping.OriSumAssuredTo >= oriSumAssuredFrom
                        ||
                        q.TreatyBenefitCodeMapping.OriSumAssuredFrom <= oriSumAssuredTo && q.TreatyBenefitCodeMapping.OriSumAssuredTo >= oriSumAssuredTo
                    )
                    || (q.TreatyBenefitCodeMapping.OriSumAssuredFrom == null && q.TreatyBenefitCodeMapping.OriSumAssuredTo == null)
                );
            }

            if (reinsuranceIssueAgeFrom != null && reinsuranceIssueAgeTo != null)
            {
                query = query
                    .Where(q =>
                    (
                        q.TreatyBenefitCodeMapping.ReinsuranceIssueAgeFrom <= attAgeFrom && q.TreatyBenefitCodeMapping.ReinsuranceIssueAgeTo >= reinsuranceIssueAgeFrom
                        ||
                        q.TreatyBenefitCodeMapping.ReinsuranceIssueAgeFrom <= attAgeTo && q.TreatyBenefitCodeMapping.ReinsuranceIssueAgeTo >= reinsuranceIssueAgeTo
                    )
                    || (q.TreatyBenefitCodeMapping.ReinsuranceIssueAgeFrom == null && q.TreatyBenefitCodeMapping.ReinsuranceIssueAgeTo == null)
                );
            }

            if (treatyBenefitCodeMappingId != null)
            {
                query = query.Where(q => q.TreatyBenefitCodeMappingId != treatyBenefitCodeMappingId);
            }

            return query;
        }

        public static int CountDuplicateByParamsForBenefit(
            int cedantId,
            string cedingPlanCode,
            string cedingBenefitTypeCode,
            string cedingBenefitRiskCode,
            string cedingTreatyCode,
            string campaignCode,
            int? reinsBasisCodePickListDetailId,
            int? attAgeFrom,
            int? attAgeTo,
            DateTime? reportingStartDate,
            DateTime? reportingEndDate,
            int? treatyCodeId,
            DateTime? reinsEffDatePolStartDate,
            DateTime? reinsEffDatePolEndDate,
            double? underwriterRatingFrom,
            double? underwriterRatingTo,
            double? oriSumAssuredFrom,
            double? oriSumAssuredTo,
            int? reinsuranceIssueAgeFrom,
            int? reinsuranceIssueAgeTo,
            int? treatyBenefitCodeMappingId = null
        )
        {
            using (var db = new AppDbContext())
            {
                return QueryDuplicateByParamsForBenefit(
                        db,
                        cedantId,
                        cedingPlanCode,
                        cedingBenefitTypeCode,
                        cedingBenefitRiskCode,
                        cedingTreatyCode,
                        campaignCode,
                        reinsBasisCodePickListDetailId,
                        attAgeFrom,
                        attAgeTo,
                        reportingStartDate,
                        reportingEndDate,
                        treatyCodeId,
                        reinsEffDatePolStartDate,
                        reinsEffDatePolEndDate,
                        underwriterRatingFrom,
                        underwriterRatingTo,
                        oriSumAssuredFrom,
                        oriSumAssuredTo,
                        reinsuranceIssueAgeFrom,
                        reinsuranceIssueAgeTo,
                        treatyBenefitCodeMappingId
                    ).Count();
            }
        }

        public static IQueryable<TreatyBenefitCodeMappingDetail> QueryDuplicateByParamsForBenefit(
            AppDbContext db,
            int cedantId,
            string cedingPlanCode,
            string cedingBenefitTypeCode,
            string cedingBenefitRiskCode,
            string cedingTreatyCode,
            string campaignCode,
            int? reinsBasisCodePickListDetailId,
            int? attAgeFrom,
            int? attAgeTo,
            DateTime? reportingStartDate,
            DateTime? reportingEndDate,
            int? treatyCodeId,
            DateTime? reinsEffDatePolStartDate,
            DateTime? reinsEffDatePolEndDate,
            double? underwriterRatingFrom,
            double? underwriterRatingTo,
            double? oriSumAssuredFrom,
            double? oriSumAssuredTo,
            int? reinsuranceIssueAgeFrom,
            int? reinsuranceIssueAgeTo,
            int? treatyBenefitCodeMappingId = null
        )
        {
            var query = db.TreatyBenefitCodeMappingDetails
                .Where(q => q.TreatyBenefitCodeMapping.CedantId == cedantId)
                .Where(q => q.Type == TreatyBenefitCodeMappingDetailBo.CombinationTypeBenefit);

            if (!string.IsNullOrEmpty(cedingPlanCode))
            {
                query = query
                    .Where(q => (!string.IsNullOrEmpty(q.CedingPlanCode) && q.CedingPlanCode.Trim() == cedingPlanCode.Trim()) || q.CedingPlanCode == null);
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

            if (!string.IsNullOrEmpty(cedingTreatyCode))
            {
                query = query
                    .Where(q => (!string.IsNullOrEmpty(q.CedingTreatyCode) && q.CedingTreatyCode.Trim() == cedingTreatyCode.Trim()) || q.CedingTreatyCode == null);
            }

            if (!string.IsNullOrEmpty(campaignCode))
            {
                query = query
                    .Where(q => (!string.IsNullOrEmpty(q.CampaignCode) && q.CampaignCode.Trim() == campaignCode.Trim()) || q.CampaignCode == null);
            }

            if (reinsBasisCodePickListDetailId.HasValue)
            {
                query = query
                    .Where(q => (q.TreatyBenefitCodeMapping.ReinsBasisCodePickListDetailId.HasValue && q.TreatyBenefitCodeMapping.ReinsBasisCodePickListDetailId == reinsBasisCodePickListDetailId) || !q.TreatyBenefitCodeMapping.ReinsBasisCodePickListDetailId.HasValue);
            }

            if (attAgeFrom != null && attAgeTo != null)
            {
                query = query
                    .Where(q =>
                    (
                        q.TreatyBenefitCodeMapping.AttainedAgeFrom <= attAgeFrom && q.TreatyBenefitCodeMapping.AttainedAgeTo >= attAgeFrom
                        ||
                        q.TreatyBenefitCodeMapping.AttainedAgeFrom <= attAgeTo && q.TreatyBenefitCodeMapping.AttainedAgeTo >= attAgeTo
                    )
                    || (q.TreatyBenefitCodeMapping.AttainedAgeFrom == null && q.TreatyBenefitCodeMapping.AttainedAgeTo == null)
                );
            }

            if (reportingStartDate != null && reportingEndDate != null)
            {
                query = query
                    .Where(q =>
                    (
                        DbFunctions.TruncateTime(q.TreatyBenefitCodeMapping.ReportingStartDate) <= DbFunctions.TruncateTime(reportingStartDate)
                        && DbFunctions.TruncateTime(q.TreatyBenefitCodeMapping.ReportingEndDate) >= DbFunctions.TruncateTime(reportingStartDate)
                        ||
                        DbFunctions.TruncateTime(q.TreatyBenefitCodeMapping.ReportingStartDate) <= DbFunctions.TruncateTime(reportingEndDate)
                        && DbFunctions.TruncateTime(q.TreatyBenefitCodeMapping.ReportingEndDate) >= DbFunctions.TruncateTime(reportingEndDate)
                    )
                    || (q.TreatyBenefitCodeMapping.ReportingStartDate == null && q.TreatyBenefitCodeMapping.ReportingEndDate == null)
                );
            }

            if (treatyCodeId != null)
            {
                query = query.Where(q => q.TreatyBenefitCodeMapping.TreatyCodeId == treatyCodeId);
            }

            if (reinsEffDatePolStartDate != null && reinsEffDatePolEndDate != null)
            {
                query = query
                    .Where(q =>
                    (
                        DbFunctions.TruncateTime(q.TreatyBenefitCodeMapping.ReinsEffDatePolStartDate) <= DbFunctions.TruncateTime(reinsEffDatePolStartDate)
                        && DbFunctions.TruncateTime(q.TreatyBenefitCodeMapping.ReinsEffDatePolEndDate) >= DbFunctions.TruncateTime(reinsEffDatePolStartDate)
                        ||
                        DbFunctions.TruncateTime(q.TreatyBenefitCodeMapping.ReinsEffDatePolStartDate) <= DbFunctions.TruncateTime(reinsEffDatePolEndDate)
                        && DbFunctions.TruncateTime(q.TreatyBenefitCodeMapping.ReinsEffDatePolEndDate) >= DbFunctions.TruncateTime(reinsEffDatePolEndDate)
                    )
                    || (q.TreatyBenefitCodeMapping.ReinsEffDatePolStartDate == null && q.TreatyBenefitCodeMapping.ReinsEffDatePolEndDate == null)
                );
            }

            if (underwriterRatingFrom != null && underwriterRatingTo != null)
            {
                query = query
                    .Where(q =>
                    (
                        q.TreatyBenefitCodeMapping.UnderwriterRatingFrom <= underwriterRatingFrom && q.TreatyBenefitCodeMapping.UnderwriterRatingTo >= underwriterRatingFrom
                        ||
                        q.TreatyBenefitCodeMapping.UnderwriterRatingFrom <= underwriterRatingTo && q.TreatyBenefitCodeMapping.UnderwriterRatingTo >= underwriterRatingTo
                    )
                    || (q.TreatyBenefitCodeMapping.UnderwriterRatingFrom == null && q.TreatyBenefitCodeMapping.UnderwriterRatingTo == null)
                );
            }

            if (oriSumAssuredFrom != null && oriSumAssuredTo != null)
            {
                query = query
                    .Where(q =>
                    (
                        q.TreatyBenefitCodeMapping.OriSumAssuredFrom <= oriSumAssuredFrom && q.TreatyBenefitCodeMapping.OriSumAssuredTo >= oriSumAssuredFrom
                        ||
                        q.TreatyBenefitCodeMapping.OriSumAssuredFrom <= oriSumAssuredTo && q.TreatyBenefitCodeMapping.OriSumAssuredTo >= oriSumAssuredTo
                    )
                    || (q.TreatyBenefitCodeMapping.OriSumAssuredFrom == null && q.TreatyBenefitCodeMapping.OriSumAssuredTo == null)
                );
            }

            if (reinsuranceIssueAgeFrom != null && reinsuranceIssueAgeTo != null)
            {
                query = query
                    .Where(q =>
                    (
                        q.TreatyBenefitCodeMapping.ReinsuranceIssueAgeFrom <= reinsuranceIssueAgeFrom && q.TreatyBenefitCodeMapping.ReinsuranceIssueAgeTo >= reinsuranceIssueAgeFrom
                        ||
                        q.TreatyBenefitCodeMapping.ReinsuranceIssueAgeFrom <= reinsuranceIssueAgeTo && q.TreatyBenefitCodeMapping.ReinsuranceIssueAgeTo >= reinsuranceIssueAgeTo
                    )
                    || (q.TreatyBenefitCodeMapping.ReinsuranceIssueAgeFrom == null && q.TreatyBenefitCodeMapping.ReinsuranceIssueAgeTo == null)
                );
            }

            if (treatyBenefitCodeMappingId != null)
            {
                query = query.Where(q => q.TreatyBenefitCodeMappingId != treatyBenefitCodeMappingId);
            }

            return query;
        }

        public static int CountDuplicateByParamsForProductFeature(
            int cedantId,
            string cedingPlanCode,
            string cedingBenefitTypeCode,
            string cedingBenefitRiskCode,
            string cedingTreatyCode,
            string campaignCode,
            int? reinsBasisCodePickListDetailId,
            DateTime? reinsEffDatePolStartDate,
            DateTime? reinsEffDatePolEndDate,
            int? attAgeFrom,
            int? attAgeTo,
            int? treatyCodeId,
            int? benefitId,
            double? underwriterRatingFrom,
            double? underwriterRatingTo,
            double? oriSumAssuredFrom,
            double? oriSumAssuredTo,
            int? reinsuranceIssueAgeFrom,
            int? reinsuranceIssueAgeTo,
            DateTime? reportingStartDate,
            DateTime? reportingEndDate,
            int? treatyBenefitCodeMappingId = null
        )
        {
            using (var db = new AppDbContext())
            {
                return QueryDuplicateByParamsForProductFeature(
                        db,
                        cedantId,
                        cedingPlanCode,
                        cedingBenefitTypeCode,
                        cedingBenefitRiskCode,
                        cedingTreatyCode,
                        campaignCode,
                        reinsBasisCodePickListDetailId,
                        reinsEffDatePolStartDate,
                        reinsEffDatePolEndDate,
                        attAgeFrom,
                        attAgeTo,
                        treatyCodeId,
                        benefitId,
                        underwriterRatingFrom,
                        underwriterRatingTo,
                        oriSumAssuredFrom,
                        oriSumAssuredTo,
                        reinsuranceIssueAgeFrom,
                        reinsuranceIssueAgeTo,
                        reportingStartDate,
                        reportingEndDate,
                        treatyBenefitCodeMappingId
                    ).Count();
            }
        }

        public static IQueryable<TreatyBenefitCodeMappingDetail> QueryDuplicateByParamsForProductFeature(
            AppDbContext db,
            int cedantId,
            string cedingPlanCode,
            string cedingBenefitTypeCode,
            string cedingBenefitRiskCode,
            string cedingTreatyCode,
            string campaignCode,
            int? reinsBasisCodePickListDetailId,
            DateTime? reinsEffDatePolStartDate,
            DateTime? reinsEffDatePolEndDate,
            int? attAgeFrom,
            int? attAgeTo,
            int? treatyCodeId,
            int? benefitId,
            double? underwriterRatingFrom,
            double? underwriterRatingTo,
            double? oriSumAssuredFrom,
            double? oriSumAssuredTo,
            int? reinsuranceIssueAgeFrom,
            int? reinsuranceIssueAgeTo,
            DateTime? reportingStartDate,
            DateTime? reportingEndDate,
            int? treatyBenefitCodeMappingId = null
        )
        {
            var query = db.TreatyBenefitCodeMappingDetails
                .Where(q => q.TreatyBenefitCodeMapping.CedantId == cedantId)
                .Where(q => q.Type == TreatyBenefitCodeMappingDetailBo.CombinationTypeProductFeature);

            if (!string.IsNullOrEmpty(cedingPlanCode))
            {
                query = query
                    .Where(q => (!string.IsNullOrEmpty(q.CedingPlanCode) && q.CedingPlanCode.Trim() == cedingPlanCode.Trim()) || q.CedingPlanCode == null);
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

            if (!string.IsNullOrEmpty(cedingTreatyCode))
            {
                query = query
                    .Where(q => (!string.IsNullOrEmpty(q.CedingTreatyCode) && q.CedingTreatyCode.Trim() == cedingTreatyCode.Trim()) || q.CedingTreatyCode == null);
            }

            if (!string.IsNullOrEmpty(campaignCode))
            {
                query = query
                    .Where(q => (!string.IsNullOrEmpty(q.CampaignCode) && q.CampaignCode.Trim() == campaignCode.Trim()) || q.CampaignCode == null);
            }

            if (reinsBasisCodePickListDetailId.HasValue)
            {
                query = query
                    .Where(q => (q.TreatyBenefitCodeMapping.ReinsBasisCodePickListDetailId.HasValue && q.TreatyBenefitCodeMapping.ReinsBasisCodePickListDetailId == reinsBasisCodePickListDetailId) || !q.TreatyBenefitCodeMapping.ReinsBasisCodePickListDetailId.HasValue);
            }

            if (reinsEffDatePolStartDate != null && reinsEffDatePolEndDate != null)
            {
                query = query
                    .Where(q =>
                    (
                        DbFunctions.TruncateTime(q.TreatyBenefitCodeMapping.ReinsEffDatePolStartDate) <= DbFunctions.TruncateTime(reinsEffDatePolStartDate)
                        && DbFunctions.TruncateTime(q.TreatyBenefitCodeMapping.ReinsEffDatePolEndDate) >= DbFunctions.TruncateTime(reinsEffDatePolStartDate)
                        ||
                        DbFunctions.TruncateTime(q.TreatyBenefitCodeMapping.ReinsEffDatePolStartDate) <= DbFunctions.TruncateTime(reinsEffDatePolEndDate)
                        && DbFunctions.TruncateTime(q.TreatyBenefitCodeMapping.ReinsEffDatePolEndDate) >= DbFunctions.TruncateTime(reinsEffDatePolEndDate)
                    )
                    || (q.TreatyBenefitCodeMapping.ReinsEffDatePolStartDate == null && q.TreatyBenefitCodeMapping.ReinsEffDatePolEndDate == null)
                );
            }

            if (attAgeFrom != null && attAgeTo != null)
            {
                query = query
                    .Where(q =>
                    (
                        q.TreatyBenefitCodeMapping.AttainedAgeFrom <= attAgeFrom && q.TreatyBenefitCodeMapping.AttainedAgeTo >= attAgeFrom
                        ||
                        q.TreatyBenefitCodeMapping.AttainedAgeFrom <= attAgeTo && q.TreatyBenefitCodeMapping.AttainedAgeTo >= attAgeTo
                    )
                    || (q.TreatyBenefitCodeMapping.AttainedAgeFrom == null && q.TreatyBenefitCodeMapping.AttainedAgeTo == null)
                );
            }

            if (treatyCodeId != null)
            {
                query = query.Where(q => q.TreatyBenefitCodeMapping.TreatyCodeId == treatyCodeId);
            }

            if (benefitId != null)
            {
                query = query.Where(q => q.TreatyBenefitCodeMapping.BenefitId == benefitId);
            }

            if (underwriterRatingFrom != null && underwriterRatingTo != null)
            {
                query = query
                    .Where(q =>
                    (
                        q.TreatyBenefitCodeMapping.UnderwriterRatingFrom <= underwriterRatingFrom && q.TreatyBenefitCodeMapping.UnderwriterRatingTo >= underwriterRatingFrom
                        ||
                        q.TreatyBenefitCodeMapping.UnderwriterRatingFrom <= underwriterRatingTo && q.TreatyBenefitCodeMapping.UnderwriterRatingTo >= underwriterRatingTo
                    )
                    || (q.TreatyBenefitCodeMapping.UnderwriterRatingFrom == null && q.TreatyBenefitCodeMapping.UnderwriterRatingTo == null)
                );
            }

            if (oriSumAssuredFrom != null && oriSumAssuredTo != null)
            {
                query = query
                    .Where(q =>
                    (
                        q.TreatyBenefitCodeMapping.OriSumAssuredFrom <= oriSumAssuredFrom && q.TreatyBenefitCodeMapping.OriSumAssuredTo >= oriSumAssuredFrom
                        ||
                        q.TreatyBenefitCodeMapping.OriSumAssuredFrom <= oriSumAssuredTo && q.TreatyBenefitCodeMapping.OriSumAssuredTo >= oriSumAssuredTo
                    )
                    || (q.TreatyBenefitCodeMapping.OriSumAssuredFrom == null && q.TreatyBenefitCodeMapping.OriSumAssuredTo == null)
                );
            }

            if (reportingStartDate != null && reportingEndDate != null)
            {
                query = query
                    .Where(q =>
                    (
                        DbFunctions.TruncateTime(q.TreatyBenefitCodeMapping.ReportingStartDate) <= DbFunctions.TruncateTime(reportingStartDate)
                        && DbFunctions.TruncateTime(q.TreatyBenefitCodeMapping.ReportingEndDate) >= DbFunctions.TruncateTime(reportingStartDate)
                        ||
                        DbFunctions.TruncateTime(q.TreatyBenefitCodeMapping.ReportingStartDate) <= DbFunctions.TruncateTime(reportingEndDate)
                        && DbFunctions.TruncateTime(q.TreatyBenefitCodeMapping.ReportingEndDate) >= DbFunctions.TruncateTime(reportingEndDate)
                    )
                    || (q.TreatyBenefitCodeMapping.ReportingStartDate == null && q.TreatyBenefitCodeMapping.ReportingEndDate == null)
                );
            }

            if (reinsuranceIssueAgeFrom != null && reinsuranceIssueAgeTo != null)
            {
                query = query
                    .Where(q =>
                    (
                        q.TreatyBenefitCodeMapping.ReinsuranceIssueAgeFrom <= reinsuranceIssueAgeFrom && q.TreatyBenefitCodeMapping.ReinsuranceIssueAgeTo >= reinsuranceIssueAgeFrom
                        ||
                        q.TreatyBenefitCodeMapping.ReinsuranceIssueAgeFrom <= reinsuranceIssueAgeTo && q.TreatyBenefitCodeMapping.ReinsuranceIssueAgeTo >= reinsuranceIssueAgeTo
                    )
                    || (q.TreatyBenefitCodeMapping.ReinsuranceIssueAgeFrom == null && q.TreatyBenefitCodeMapping.ReinsuranceIssueAgeTo == null)
                );
            }

            if (treatyBenefitCodeMappingId != null)
            {
                query = query.Where(q => q.TreatyBenefitCodeMappingId != treatyBenefitCodeMappingId);
            }

            return query;
        }

        public DataTrail Create()
        {
            using (var db = new AppDbContext())
            {
                db.TreatyBenefitCodeMappingDetails.Add(this);
                db.SaveChanges();

                DataTrail trail = new DataTrail(this);
                return trail;
            }
        }

        public DataTrail Update()
        {
            using (var db = new AppDbContext())
            {
                var entity = TreatyBenefitCodeMappingDetail.Find(Id);
                if (entity == null)
                {
                    throw new Exception(MessageBag.NoRecordFound);
                }

                var trail = new DataTrail(entity, this);

                entity.Type = Type;
                entity.TreatyBenefitCodeMappingId = TreatyBenefitCodeMappingId;
                entity.Combination = Combination;
                entity.CedingPlanCode = CedingPlanCode;
                entity.CedingBenefitTypeCode = CedingBenefitTypeCode;
                entity.CedingBenefitRiskCode = CedingBenefitRiskCode;
                entity.CedingTreatyCode = CedingTreatyCode;
                entity.CampaignCode = CampaignCode;

                db.Entry(entity).State = EntityState.Modified;
                db.SaveChanges();

                return trail;
            }
        }

        public static DataTrail Delete(int id)
        {
            using (var db = new AppDbContext())
            {
                var entity = db.TreatyBenefitCodeMappingDetails.Where(q => q.Id == id).FirstOrDefault();
                if (entity == null)
                {
                    throw new Exception(MessageBag.NoRecordFound);
                }

                var trail = new DataTrail(entity, true);

                db.Entry(entity).State = EntityState.Deleted;
                db.TreatyBenefitCodeMappingDetails.Remove(entity);
                db.SaveChanges();

                return trail;
            }
        }

        public static IList<DataTrail> DeleteAllByTreatyBenefitCodeMappingId(int treatyBenefitCodeMappingId)
        {
            using (var db = new AppDbContext())
            {
                var query = db.TreatyBenefitCodeMappingDetails.Where(q => q.TreatyBenefitCodeMappingId == treatyBenefitCodeMappingId);

                var trails = new List<DataTrail>();
                foreach (TreatyBenefitCodeMappingDetail treatyBenefitCodeMappingDetail in query.ToList())
                {
                    var trail = new DataTrail(treatyBenefitCodeMappingDetail, true);
                    trails.Add(trail);

                    db.Entry(treatyBenefitCodeMappingDetail).State = EntityState.Deleted;
                    db.TreatyBenefitCodeMappingDetails.Remove(treatyBenefitCodeMappingDetail);
                }

                db.SaveChanges();
                return trails;
            }
        }
    }
}