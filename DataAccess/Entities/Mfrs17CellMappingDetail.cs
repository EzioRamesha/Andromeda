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
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Entities
{
    [Table("Mfrs17CellMappingDetails")]
    public class Mfrs17CellMappingDetail
    {
        [Key]
        public int Id { get; set; }

        [Required, Index]
        public int Mfrs17CellMappingId { get; set; }

        [ForeignKey(nameof(Mfrs17CellMappingId))]
        [ExcludeTrail]
        public virtual Mfrs17CellMapping Mfrs17CellMapping { get; set; }

        [MaxLength(255), Index]
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

        [MaxLength(10), Index]
        public string BenefitCode { get; set; }

        public Mfrs17CellMappingDetail()
        {
            CreatedAt = DateTime.Now;
        }

        [Index]
        [MaxLength(35)]
        public string TreatyCode { get; set; }

        public static bool IsExists(int id)
        {
            using (var db = new AppDbContext())
            {
                return db.Mfrs17CellMappingDetails.Any(q => q.Id == id);
            }
        }

        public static IQueryable<Mfrs17CellMappingDetail> QueryByCombination(
            AppDbContext db,
            string combination,
            DateTime? reinsEffDatePolStartDate,
            DateTime? reinsEffDatePolEndDate,
            int? mfrs17CellMappingId = null
        )
        {
            var query = db.Mfrs17CellMappingDetails
                .Where(q => q.Combination.Trim() == combination.Trim());

            if (reinsEffDatePolStartDate != null && reinsEffDatePolEndDate != null)
            {
                query = query
                    .Where(q =>
                    (
                        DbFunctions.TruncateTime(q.Mfrs17CellMapping.ReinsEffDatePolStartDate) <= DbFunctions.TruncateTime(reinsEffDatePolStartDate)
                        && DbFunctions.TruncateTime(q.Mfrs17CellMapping.ReinsEffDatePolEndDate) >= DbFunctions.TruncateTime(reinsEffDatePolStartDate)
                        ||
                        DbFunctions.TruncateTime(q.Mfrs17CellMapping.ReinsEffDatePolStartDate) <= DbFunctions.TruncateTime(reinsEffDatePolEndDate)
                        && DbFunctions.TruncateTime(q.Mfrs17CellMapping.ReinsEffDatePolEndDate) >= DbFunctions.TruncateTime(reinsEffDatePolEndDate)
                    )
                    || (q.Mfrs17CellMapping.ReinsEffDatePolStartDate == null && q.Mfrs17CellMapping.ReinsEffDatePolEndDate == null)
                );
            }

            if (mfrs17CellMappingId != null)
            {
                query = query.Where(q => q.Mfrs17CellMappingId != mfrs17CellMappingId);
            }

            return query;
        }

        public static IQueryable<Mfrs17CellMappingDetail> QueryDuplicateByParams(
            AppDbContext db,
            string cedingPlanCode,
            string benefitCode,
            int? reinsBasisCodePickListId,
            string treatyCode,
            int? profitCommPickListDetailId,
            string rateTable,
            DateTime? reinsEffDatePolStartDate,
            DateTime? reinsEffDatePolEndDate,
            int? mfrs17CellMappingId = null
        )
        {
            var query = db.Mfrs17CellMappingDetails
                .Where(q => q.Mfrs17CellMapping.ReinsBasisCodePickListDetailId == reinsBasisCodePickListId);

            if (!string.IsNullOrEmpty(cedingPlanCode))
            {
                query = query
                    .Where(q => (!string.IsNullOrEmpty(q.CedingPlanCode) && q.CedingPlanCode.Trim() == cedingPlanCode.Trim()) || q.CedingPlanCode == null);
            }

            if (!string.IsNullOrEmpty(benefitCode))
            {
                query = query
                    .Where(q => (!string.IsNullOrEmpty(q.BenefitCode) && q.BenefitCode.Trim() == benefitCode.Trim()) || q.BenefitCode == null);
            }

            if (!string.IsNullOrEmpty(treatyCode))
            {
                query = query
                    .Where(q => (!string.IsNullOrEmpty(q.TreatyCode) && q.TreatyCode.Trim() == treatyCode.Trim()) || q.TreatyCode == null);
            }

            if (profitCommPickListDetailId.HasValue)
            {
                query = query
                    .Where(q => (q.Mfrs17CellMapping.ProfitCommPickListDetailId.HasValue && q.Mfrs17CellMapping.ProfitCommPickListDetailId == profitCommPickListDetailId) || !q.Mfrs17CellMapping.ProfitCommPickListDetailId.HasValue);
            }

            if (!string.IsNullOrEmpty(rateTable))
            {
                query = query
                    .Where(q => (!string.IsNullOrEmpty(q.Mfrs17CellMapping.RateTable) && q.Mfrs17CellMapping.RateTable.Trim() == rateTable.Trim()) || q.Mfrs17CellMapping.RateTable == null);
            }

            if (reinsEffDatePolStartDate != null && reinsEffDatePolEndDate != null)
            {
                query = query
                    .Where(q =>
                    (
                        DbFunctions.TruncateTime(q.Mfrs17CellMapping.ReinsEffDatePolStartDate) <= DbFunctions.TruncateTime(reinsEffDatePolStartDate)
                        && DbFunctions.TruncateTime(q.Mfrs17CellMapping.ReinsEffDatePolEndDate) >= DbFunctions.TruncateTime(reinsEffDatePolStartDate)
                        ||
                        DbFunctions.TruncateTime(q.Mfrs17CellMapping.ReinsEffDatePolStartDate) <= DbFunctions.TruncateTime(reinsEffDatePolEndDate)
                        && DbFunctions.TruncateTime(q.Mfrs17CellMapping.ReinsEffDatePolEndDate) >= DbFunctions.TruncateTime(reinsEffDatePolEndDate)
                    )
                    || (q.Mfrs17CellMapping.ReinsEffDatePolStartDate == null && q.Mfrs17CellMapping.ReinsEffDatePolEndDate == null)
                );
            }

            if (mfrs17CellMappingId != null)
            {
                query = query.Where(q => q.Mfrs17CellMappingId != mfrs17CellMappingId);
            }

            return query;
        }

        public static IQueryable<Mfrs17CellMappingDetail> QueryByParams(
            AppDbContext db,
            string treatyCode,
            string profitComm,
            int? reinsBasisCodeId,
            string cedingPlanCode = null,
            string mlreBenefitCode = null,
            DateTime? reinsEffDatePol = null,
            string rateTable = null,
            bool groupById = false
        )
        {
            db.Database.CommandTimeout = 0;

            EFExecutionStrategy connectionStrategy = EFExecutionStrategy.GetNewRiDataRowProcessingInstance(185, "Mfrs17CellMappingDetail");

            return connectionStrategy.Execute(() =>
            {
                var query = db.Mfrs17CellMappingDetails
                              .Where(q => q.TreatyCode.Trim() == treatyCode.Trim())
                              .Where(q => q.Mfrs17CellMapping.ProfitCommPickListDetailId.HasValue && q.Mfrs17CellMapping.ProfitCommPickListDetail.Code.Trim() == profitComm.Trim())
                              .Where(q => q.Mfrs17CellMapping.ReinsBasisCodePickListDetailId == reinsBasisCodeId);

                if (!string.IsNullOrEmpty(cedingPlanCode))
                {
                    connectionStrategy.Reset(195);
                    query = query.Where(q => (q.CedingPlanCode != null && q.CedingPlanCode.Trim() == cedingPlanCode.Trim()) || q.CedingPlanCode == null);
                }
                else
                {
                    connectionStrategy.Reset(200);
                    query = query.Where(q => q.CedingPlanCode == null);
                }

                if (!string.IsNullOrEmpty(mlreBenefitCode))
                {
                    connectionStrategy.Reset(206);
                    query = query.Where(q => (q.BenefitCode != null && q.BenefitCode.Trim() == mlreBenefitCode.Trim()) || q.BenefitCode == null);
                }
                else
                {
                    connectionStrategy.Reset(211);
                    query = query.Where(q => q.BenefitCode == null);
                }

                if (reinsEffDatePol != null)
                {
                    connectionStrategy.Reset(217);
                    query = query.Where(q =>
                            (
                                DbFunctions.TruncateTime(q.Mfrs17CellMapping.ReinsEffDatePolStartDate) <= DbFunctions.TruncateTime(reinsEffDatePol)
                                && DbFunctions.TruncateTime(q.Mfrs17CellMapping.ReinsEffDatePolEndDate) >= DbFunctions.TruncateTime(reinsEffDatePol)
                                ||
                                DbFunctions.TruncateTime(q.Mfrs17CellMapping.ReinsEffDatePolStartDate) <= DbFunctions.TruncateTime(reinsEffDatePol)
                                && DbFunctions.TruncateTime(q.Mfrs17CellMapping.ReinsEffDatePolEndDate) >= DbFunctions.TruncateTime(reinsEffDatePol)
                            )
                            || (q.Mfrs17CellMapping.ReinsEffDatePolStartDate == null && q.Mfrs17CellMapping.ReinsEffDatePolEndDate == null)
                        );
                }
                else
                {
                    connectionStrategy.Reset(231);
                    query = query.Where(q => q.Mfrs17CellMapping.ReinsEffDatePolStartDate == null && q.Mfrs17CellMapping.ReinsEffDatePolEndDate == null);
                }

                if (!string.IsNullOrEmpty(rateTable))
                {
                    connectionStrategy.Reset(237);
                    query = query.Where(q => (q.Mfrs17CellMapping.RateTable != null && q.Mfrs17CellMapping.RateTable.Trim() == rateTable.Trim()) || q.Mfrs17CellMapping.RateTable == null);
                }
                else
                {
                    connectionStrategy.Reset(242);
                    query = query.Where(q => q.Mfrs17CellMapping.RateTable == null);
                }

                // NOTE: Group by should put at the end of query
                if (groupById)
                {
                    connectionStrategy.Reset(248);
                    query = query.GroupBy(q => q.Mfrs17CellMappingId).Select(q => q.FirstOrDefault());
                }

                return query;
            });
        }

        public static Mfrs17CellMappingDetail Find(int id)
        {
            using (var db = new AppDbContext())
            {
                return db.Mfrs17CellMappingDetails.Where(q => q.Id == id).FirstOrDefault();
            }
        }

        public static Mfrs17CellMappingDetail FindByCombination(
            string combination,
            DateTime? reinsEffDatePolStartDate,
            DateTime? reinsEffDatePolEndDate,
            int? mfrs17CellMappingId = null
        )
        {
            using (var db = new AppDbContext())
            {
                return QueryByCombination(
                    db,
                    combination,
                    reinsEffDatePolStartDate,
                    reinsEffDatePolEndDate,
                    mfrs17CellMappingId
                ).FirstOrDefault();
            }
        }

        public static Mfrs17CellMappingDetail FindByParams(
            string treatyCode,
            string profitComm,
            int? reinsBasisCodeId,
            string cedingPlanCode = null,
            string mlreBenefitCode = null,
            DateTime? reinsEffDatePol = null,
            string rateTable = null,
            bool groupById = false
        )
        {
            using (var db = new AppDbContext())
            {
                return QueryByParams(
                    db,
                    treatyCode,
                    profitComm,
                    reinsBasisCodeId,
                    cedingPlanCode,
                    mlreBenefitCode,
                    reinsEffDatePol,
                    rateTable,
                    groupById
                ).FirstOrDefault();
            }
        }

        public static int CountByCombination(
            string combination,
            DateTime? reinsEffDatePolStartDate,
            DateTime? reinsEffDatePolEndDate,
            int? mfrs17CellMappingId = null
        )
        {
            using (var db = new AppDbContext())
            {
                return QueryByCombination(
                    db,
                    combination,
                    reinsEffDatePolStartDate,
                    reinsEffDatePolEndDate,
                    mfrs17CellMappingId
                ).Count();
            }
        }

        public static int CountDuplicateByParams(
            string cedingPlanCode,
            string benefitCode,
            int? reinsBasisCodePickListId,
            string treatyCode,
            int? profitCommPickListDetailId,
            string rateTable,
            DateTime? reinsEffDatePolStartDate,
            DateTime? reinsEffDatePolEndDate,
            int? mfrs17CellMappingId = null
        )
        {
            using (var db = new AppDbContext())
            {
                return QueryDuplicateByParams(
                    db,
                    cedingPlanCode,
                    benefitCode,
                    reinsBasisCodePickListId,
                    treatyCode,
                    profitCommPickListDetailId,
                    rateTable,
                    reinsEffDatePolStartDate,
                    reinsEffDatePolEndDate,
                    mfrs17CellMappingId
                ).Count();
            }
        }

        public static int CountByParams(
            string treatyCode,
            string profitComm,
            int? reinsBasisCodeId,
            string cedingPlanCode = null,
            string mlreBenefitCode = null,
            DateTime? reinsEffDatePol = null,
            string rateTable = null,
            bool groupById = false
        )
        {
            using (var db = new AppDbContext())
            {
                return QueryByParams(
                    db,
                    treatyCode,
                    profitComm,
                    reinsBasisCodeId,
                    cedingPlanCode,
                    mlreBenefitCode,
                    reinsEffDatePol,
                    rateTable,
                    groupById
                ).Count();
            }
        }

        public static IList<Mfrs17CellMappingDetail> GetByParams(
            string treatyCode,
            string profitComm,
            int? reinsBasisCodeId,
            string cedingPlanCode = null,
            string mlreBenefitCode = null,
            DateTime? reinsEffDatePol = null,
            string rateTable = null,
            bool groupById = false
        )
        {
            using (var db = new AppDbContext())
            {
                return QueryByParams(
                    db,
                    treatyCode,
                    profitComm,
                    reinsBasisCodeId,
                    cedingPlanCode,
                    mlreBenefitCode,
                    reinsEffDatePol,
                    rateTable,
                    groupById
                ).ToList();
            }
        }

        public static int CountByTreatyCodeId(int treatyCodeId)
        {
            using (var db = new AppDbContext())
            {
                TreatyCode treatyCode = Entities.TreatyCode.Find(treatyCodeId);
                if (treatyCode != null)
                    return db.Mfrs17CellMappingDetails.Where(q => q.TreatyCode == treatyCode.Code).Count();
                return 0;
            }
        }

        public DataTrail Create()
        {
            using (var db = new AppDbContext())
            {
                db.Mfrs17CellMappingDetails.Add(this);
                db.SaveChanges();

                var trail = new DataTrail(this);
                return trail;
            }
        }

        public DataTrail Update()
        {
            using (var db = new AppDbContext())
            {
                var entity = Mfrs17CellMappingDetail.Find(Id);
                if (entity == null)
                {
                    throw new Exception(MessageBag.NoRecordFound);
                }

                var trail = new DataTrail(entity, this);

                entity.Mfrs17CellMappingId = Mfrs17CellMappingId;
                entity.Combination = Combination;
                entity.CedingPlanCode = CedingPlanCode;
                entity.BenefitCode = BenefitCode;
                entity.TreatyCode = TreatyCode;

                db.Entry(entity).State = EntityState.Modified;
                db.SaveChanges();

                return trail;
            }
        }

        public static DataTrail Delete(int id)
        {
            using (var db = new AppDbContext())
            {
                var entity = db.Mfrs17CellMappingDetails.Where(q => q.Id == id).FirstOrDefault();
                if (entity == null)
                {
                    throw new Exception(MessageBag.NoRecordFound);
                }

                var trail = new DataTrail(entity, true);

                db.Entry(entity).State = EntityState.Deleted;
                db.Mfrs17CellMappingDetails.Remove(entity);
                db.SaveChanges();

                return trail;
            }
        }

        public static IList<DataTrail> DeleteByMfrs17CellMappingId(int mfrs17CellMappingId)
        {
            using (var db = new AppDbContext())
            {
                var query = db.Mfrs17CellMappingDetails.Where(q => q.Mfrs17CellMappingId == mfrs17CellMappingId);

                var trails = new List<DataTrail>();
                foreach (Mfrs17CellMappingDetail mfrs17CellMappingDetail in query.ToList())
                {
                    var trail = new DataTrail(mfrs17CellMappingDetail, true);
                    trails.Add(trail);

                    db.Entry(mfrs17CellMappingDetail).State = EntityState.Deleted;
                    db.Mfrs17CellMappingDetails.Remove(mfrs17CellMappingDetail);
                }

                db.SaveChanges();
                return trails;
            }
        }
    }
}
