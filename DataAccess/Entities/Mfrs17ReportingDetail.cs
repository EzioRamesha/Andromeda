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
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Entities
{
    [Table("Mfrs17ReportingDetails")]
    public class Mfrs17ReportingDetail
    {
        [Key]
        [Column(TypeName = "bigint")]
        public int Id { get; set; }

        [Required, Index]
        public int Mfrs17ReportingId { get; set; }

        [ForeignKey(nameof(Mfrs17ReportingId))]
        [ExcludeTrail]
        public virtual Mfrs17Reporting Mfrs17Reporting { get; set; }

        [Required, Index]
        public int CedantId { get; set; }

        [ForeignKey(nameof(CedantId))]
        [ExcludeTrail]
        public virtual Cedant Cedant { get; set; }

        public int? TreatyCodeId { get; set; }

        [Required, Index]
        public int PremiumFrequencyCodePickListDetailId { get; set; }

        [ForeignKey(nameof(PremiumFrequencyCodePickListDetailId))]
        [ExcludeTrail]
        public virtual PickListDetail PremiumFrequencyCodePickListDetail { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime LatestDataStartDate { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime LatestDataEndDate { get; set; }

        [Required, Index]
        public int Record { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime CreatedAt { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime UpdatedAt { get; set; }

        [Required]
        public int CreatedById { get; set; }

        public int? UpdatedById { get; set; }

        [ForeignKey(nameof(CreatedById))]
        [ExcludeTrail]
        public virtual User CreatedBy { get; set; }

        [ForeignKey(nameof(UpdatedById))]
        [ExcludeTrail]
        public virtual User UpdatedBy { get; set; }

        [MaxLength(30), Index]
        public string TreatyCode { get; set; }

        [MaxLength(64), Index]
        public string RiskQuarter { get; set; }

        [MaxLength(25), Index]
        public string Mfrs17TreatyCode { get; set; }

        [MaxLength(30), Index]
        public string CedingPlanCode { get; set; }

        [Index]
        public int? Status { get; set; }

        [Index]
        public bool IsModified { get; set; } = false;

        [Index]
        public int? GenerateStatus { get; set; }

        public Mfrs17ReportingDetail()
        {
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
        }

        public static bool IsExists(int id)
        {
            using (var db = new AppDbContext())
            {
                db.Database.CommandTimeout = 0;

                EFExecutionStrategy connectionStrategy = EFExecutionStrategy.GetNewInstance("Mfrs17ReportingDetail");

                return connectionStrategy.Execute(() =>
                {
                    return db.Mfrs17ReportingDetails.Any(q => q.Id == id);
                });
            }
        }

        public static Mfrs17ReportingDetail Find(int id)
        {
            using (var db = new AppDbContext())
            {
                db.Database.CommandTimeout = 0;

                EFExecutionStrategy connectionStrategy = EFExecutionStrategy.GetNewInstance("Mfrs17ReportingDetail");

                return connectionStrategy.Execute(() =>
                {
                    return db.Mfrs17ReportingDetails.Where(q => q.Id == id).FirstOrDefault();
                });
            }
        }

        public static Mfrs17ReportingDetail FindByMfrs17ReportingIdTreatyCodePaymentMode(int mfrs17ReportingId, string treatyCode, int paymentMode)
        {
            using (var db = new AppDbContext())
            {
                db.Database.CommandTimeout = 0;

                EFExecutionStrategy connectionStrategy = EFExecutionStrategy.GetNewInstance("Mfrs17ReportingDetail");

                return connectionStrategy.Execute(() =>
                {
                    return db.Mfrs17ReportingDetails.Where(q => q.Mfrs17ReportingId == mfrs17ReportingId).Where(q => q.TreatyCode == treatyCode).Where(q => q.PremiumFrequencyCodePickListDetailId == paymentMode).FirstOrDefault();
                });
            }
        }

        public static int CountByCedantId(int cedantId)
        {
            using (var db = new AppDbContext())
            {
                db.Database.CommandTimeout = 0;

                EFExecutionStrategy connectionStrategy = EFExecutionStrategy.GetNewInstance("Mfrs17ReportingDetail");

                return connectionStrategy.Execute(() =>
                {
                    return db.Mfrs17ReportingDetails.Where(q => q.CedantId == cedantId).Count();
                });
            }
        }

        public static int CountByMfrs17ReportingId(int mfrs17ReportingId)
        {
            using (var db = new AppDbContext(false))
            {
                db.Database.CommandTimeout = 0;

                EFExecutionStrategy connectionStrategy = EFExecutionStrategy.GetNewInstance("Mfrs17ReportingDetail");

                return connectionStrategy.Execute(() =>
                {
                    return db.Mfrs17ReportingDetails.Where(q => q.Mfrs17ReportingId == mfrs17ReportingId).Count();
                });
            }
        }

        public static int CountByTreatyCodeId(int treatyCodeId)
        {
            using (var db = new AppDbContext())
            {
                db.Database.CommandTimeout = 0;
                TreatyCode treatyCode = Entities.TreatyCode.Find(treatyCodeId);
                if (treatyCode != null)
                {
                    EFExecutionStrategy connectionStrategy = EFExecutionStrategy.GetNewInstance("Mfrs17ReportingDetail");
                    return connectionStrategy.Execute(() =>
                    {
                        return db.Mfrs17ReportingDetails.Where(q => q.TreatyCode == treatyCode.Code).Count();
                    });
                }
                return 0;
            }
        }

        public static Dictionary<int, DateTime> GetLatestEndDateByMfrs17ReportingId(int mfrs17Reportingid)
        {
            using (var db = new AppDbContext())
            {
                db.Database.CommandTimeout = 0;

                EFExecutionStrategy connectionStrategy = EFExecutionStrategy.GetNewInstance("Mfrs17ReportingDetail");

                return connectionStrategy.Execute(() =>
                {
                    return db.Mfrs17ReportingDetails.Where(q => q.Mfrs17ReportingId == mfrs17Reportingid).ToDictionary(q => q.Id, q => q.LatestDataEndDate);
                });
            }
        }

        public static IList<Mfrs17ReportingDetail> GetByMfrs17ReportingId(int mfrs17Reportingid)
        {
            using (var db = new AppDbContext())
            {
                db.Database.CommandTimeout = 0;

                EFExecutionStrategy connectionStrategy = EFExecutionStrategy.GetNewInstance("Mfrs17ReportingDetail");

                return connectionStrategy.Execute(() =>
                {
                    return db.Mfrs17ReportingDetails.Where(q => q.Mfrs17ReportingId == mfrs17Reportingid).ToList();
                });
            }
        }

        public static IList<Mfrs17ReportingDetail> GetByMfrs17ReportingIdStatus(int mfrs17Reportingid, List<int> status)
        {
            using (var db = new AppDbContext())
            {
                db.Database.CommandTimeout = 0;

                EFExecutionStrategy connectionStrategy = EFExecutionStrategy.GetNewInstance("Mfrs17ReportingDetail");

                return connectionStrategy.Execute(() =>
                {
                    return db.Mfrs17ReportingDetails.Where(q => q.Mfrs17ReportingId == mfrs17Reportingid && q.Status.HasValue && status.Contains(q.Status.Value)).ToList();
                });
            }
        }

        public static IList<Mfrs17ReportingDetail> GetByMfrs17ReportingId(int mfrs17Reportingid, int skip = 0, int take = 50)
        {
            using (var db = new AppDbContext(false))
            {
                db.Database.CommandTimeout = 0;

                EFExecutionStrategy connectionStrategy = EFExecutionStrategy.GetNewInstance("Mfrs17ReportingDetail");

                return connectionStrategy.Execute(() =>
                {
                    return db.Mfrs17ReportingDetails.Where(q => q.Mfrs17ReportingId == mfrs17Reportingid).OrderBy(q => q.Id).Skip(skip).Take(take).ToList();
                });
            }
        }

        public static IList<Mfrs17ReportingDetail> GetByGroupedDetail(int mfrs17Reportingid, int cedantId, string treatyCode, int premiumFrequencyCodePickListDetailId, string riskQuarter, string cedingPlanCode, DateTime? latestDataStartDate = null, DateTime? latestDataEndDate = null)
        {
            using (var db = new AppDbContext())
            {
                db.Database.CommandTimeout = 0; // execution timeout

                EFExecutionStrategy connectionStrategy = EFExecutionStrategy.GetNewInstance("Mfrs17ReportingDetail");

                return connectionStrategy.Execute(() =>
                {
                    var query = db.Mfrs17ReportingDetails.Where(
                        q => q.Mfrs17ReportingId == mfrs17Reportingid &&
                        q.CedantId == cedantId &&
                        q.TreatyCode == treatyCode &&
                        q.PremiumFrequencyCodePickListDetailId == premiumFrequencyCodePickListDetailId &&
                        q.RiskQuarter == riskQuarter);

                    if (latestDataStartDate.HasValue && latestDataEndDate.HasValue)
                    {
                        query = query.Where(q => q.LatestDataStartDate == latestDataStartDate)
                            .Where(q => q.LatestDataEndDate == latestDataEndDate);
                    }

                    if (!string.IsNullOrEmpty(cedingPlanCode))
                    {
                        query = query.Where(q => q.CedingPlanCode == cedingPlanCode);
                    }

                    return query.ToList();
                });
            }
        }

        public static int CountDataByMfrs17ReportingId(int mfrs17ReportingId, bool isDefault = true)
        {
            using (var db = new AppDbContext())
            {
                db.Database.CommandTimeout = 0;

                EFExecutionStrategy connectionStrategy = EFExecutionStrategy.GetNewInstance("Mfrs17ReportingDetail");
                if (isDefault)
                {
                    return connectionStrategy.Execute(() =>
                    {
                        return QueryCedantData(db, mfrs17ReportingId).Count();
                    });
                }
                else
                {
                    return connectionStrategy.Execute(() =>
                    {
                        return QueryMfrs17TreatyCodeData(db, mfrs17ReportingId).Count();
                    });
                }
            }
        }

        public static IList<Mfrs17ReportingDetailData> GetDataByMfrs17ReportingId(int mfrs17ReportingId, bool isDefault = true)
        {
            using (var db = new AppDbContext())
            {
                db.Database.CommandTimeout = 0;
                EFExecutionStrategy connectionStrategy = EFExecutionStrategy.GetNewInstance("Mfrs17ReportingDetail");
                if (isDefault)
                {
                    return connectionStrategy.Execute(() =>
                    {
                        return QueryCedantData(db, mfrs17ReportingId).ToList();
                    });
                }
                else
                {
                    return connectionStrategy.Execute(() =>
                    {
                        return QueryMfrs17TreatyCodeData(db, mfrs17ReportingId).ToList();
                    });
                }
            }
        }

        public static IList<Mfrs17ReportingDetailData> GetDataByMfrs17ReportingId(int mfrs17ReportingId, int skip = 0, int take = 50, bool isDefault = true)
        {
            using (var db = new AppDbContext())
            {
                db.Database.CommandTimeout = 0;
                EFExecutionStrategy connectionStrategy = EFExecutionStrategy.GetNewInstance("Mfrs17ReportingDetail");
                if (isDefault)
                {
                    return connectionStrategy.Execute(() =>
                    {
                        return QueryCedantData(db, mfrs17ReportingId).Skip(skip).Take(take).ToList();
                    });
                }
                else
                {
                    return connectionStrategy.Execute(() =>
                    {
                        return QueryMfrs17TreatyCodeData(db, mfrs17ReportingId).Skip(skip).Take(take).ToList();
                    });
                }
            }
        }

        public static IList<Mfrs17ReportingDetail> GetModifieBydMfrs17TreatyCode(int mfrs17ReportingId, string mfrs17TreatyCode, bool modifiedOnly = false)
        {
            using (var db = new AppDbContext())
            {
                db.Database.CommandTimeout = 0;

                EFExecutionStrategy connectionStrategy = EFExecutionStrategy.GetNewInstance("Mfrs17ReportingDetail");

                return connectionStrategy.Execute(() =>
                {
                    var query = db.Mfrs17ReportingDetails.Where(
                        q => q.Mfrs17ReportingId == mfrs17ReportingId &&
                        q.Mfrs17TreatyCode == mfrs17TreatyCode);

                    if (modifiedOnly)
                        query = query.Where(q => q.IsModified == true);

                    return query.ToList();
                });
            }
        }

        public static IQueryable<Mfrs17ReportingDetailData> QueryCedantData(
            AppDbContext db,
            int mfrs17ReportingId
        )
        {
            var query = db.Mfrs17ReportingDetails
                    .Where(q => q.Mfrs17ReportingId == mfrs17ReportingId)
                    .GroupBy(g => new { g.CedantId, g.TreatyCode, g.PremiumFrequencyCodePickListDetailId, g.RiskQuarter, g.LatestDataStartDate, g.LatestDataEndDate, g.Status, g.CedingPlanCode })
                    .Select(r => new Mfrs17ReportingDetailData
                    {
                        CedantId = r.Key.CedantId,
                        TreatyCode = r.Key.TreatyCode,
                        PremiumFrequencyCodePickListDetailId = r.Key.PremiumFrequencyCodePickListDetailId,
                        RiskQuarter = r.Key.RiskQuarter,
                        LatestDataStartDate = r.Key.LatestDataStartDate,
                        LatestDataEndDate = r.Key.LatestDataEndDate,
                        Record = r.Sum(x => x.Record),
                        Status = r.Key.Status,
                        CedingPlanCode = r.Key.CedingPlanCode,
                    })
                    .OrderBy(q => q.CedantId)
                    .ThenBy(q => q.TreatyCode)
                    .ThenBy(q => q.PremiumFrequencyCodePickListDetailId);

            return query;
        }

        public static IQueryable<Mfrs17ReportingDetailData> QueryMfrs17TreatyCodeData(
            AppDbContext db,
            int mfrs17ReportingId
        )
        {
            var query = db.Mfrs17ReportingDetails
                    .Where(q => q.Mfrs17ReportingId == mfrs17ReportingId)
                    .Where(q => q.Status != Mfrs17ReportingDetailBo.StatusDeleted)
                    .GroupBy(g => new { g.PremiumFrequencyCodePickListDetailId, g.RiskQuarter, g.LatestDataStartDate, g.LatestDataEndDate, g.Mfrs17TreatyCode })
                    .Select(r => new Mfrs17ReportingDetailData
                    {
                        PremiumFrequencyCodePickListDetailId = r.Key.PremiumFrequencyCodePickListDetailId,
                        RiskQuarter = r.Key.RiskQuarter,
                        LatestDataStartDate = r.Key.LatestDataStartDate,
                        LatestDataEndDate = r.Key.LatestDataEndDate,
                        Record = r.Sum(x => x.Record),
                        Mfrs17TreatyCode = r.Key.Mfrs17TreatyCode,
                    })
                    .OrderBy(q => q.Mfrs17TreatyCode)
                    .ThenBy(q => q.PremiumFrequencyCodePickListDetailId);

            return query;
        }

        public static int SumRecordsByMfrs17ReportingId(int mfrs17ReportingId)
        {
            using (var db = new AppDbContext())
            {
                db.Database.CommandTimeout = 0;

                EFExecutionStrategy connectionStrategy = EFExecutionStrategy.GetNewInstance("Mfrs17ReportingDetail");

                return connectionStrategy.Execute(() =>
                {
                    return db.Mfrs17ReportingDetails
                            .Where(q => q.Mfrs17ReportingId == mfrs17ReportingId)
                            .Where(q => q.Status != Mfrs17ReportingDetailBo.StatusDeleted)
                            .Sum(q => q.Record);
                });
            }
        }

        public static List<string> GetDistinctMfrs17TreatyCodes(int mfrs17ReportingId, bool modifiedOnly = false, bool resume = false)
        {
            using (var db = new AppDbContext())
            {
                db.Database.CommandTimeout = 0;

                EFExecutionStrategy connectionStrategy = EFExecutionStrategy.GetNewInstance("Mfrs17ReportingDetail");

                return connectionStrategy.Execute(() =>
                {
                    var query = db.Mfrs17ReportingDetails
                        .Where(q => q.Mfrs17ReportingId == mfrs17ReportingId);

                    if (modifiedOnly)
                        query = query.Where(q => q.IsModified == true);
                    //else
                    //    query = query.Where(q => q.Status != Mfrs17ReportingDetailBo.StatusDeleted);

                    if (resume)
                        query = query.Where(q => q.GenerateStatus != Mfrs17ReportingDetailBo.GenerateStatusSuccess);

                    return query.GroupBy(q => q.Mfrs17TreatyCode)
                        .Select(q => q.FirstOrDefault().Mfrs17TreatyCode)
                        .ToList();
                });
            }
        }

        public DataTrail Create()
        {
            using (var db = new AppDbContext())
            {
                db.Database.CommandTimeout = 0;

                EFExecutionStrategy connectionStrategy = EFExecutionStrategy.GetNewInstance("Mfrs17ReportingDetail");

                connectionStrategy.Execute(() =>
                {
                    db.Mfrs17ReportingDetails.Add(this);
                    db.SaveChanges();
                });

                DataTrail trail = new DataTrail(this);
                return trail;
            }
        }

        public DataTrail Update()
        {
            using (var db = new AppDbContext())
            {
                db.Database.CommandTimeout = 0;
                Mfrs17ReportingDetail mfrs17ReportingDetail = Find(Id);
                if (mfrs17ReportingDetail == null)
                {
                    throw new Exception(MessageBag.NoRecordFound);
                }

                DataTrail trail = new DataTrail(mfrs17ReportingDetail, this);

                mfrs17ReportingDetail.Mfrs17ReportingId = Mfrs17ReportingId;
                mfrs17ReportingDetail.CedantId = CedantId;
                mfrs17ReportingDetail.TreatyCode = TreatyCode;
                mfrs17ReportingDetail.PremiumFrequencyCodePickListDetailId = PremiumFrequencyCodePickListDetailId;
                mfrs17ReportingDetail.RiskQuarter = RiskQuarter;
                mfrs17ReportingDetail.LatestDataStartDate = LatestDataStartDate;
                mfrs17ReportingDetail.LatestDataEndDate = LatestDataEndDate;
                mfrs17ReportingDetail.Record = Record;
                mfrs17ReportingDetail.Mfrs17TreatyCode = Mfrs17TreatyCode;
                mfrs17ReportingDetail.CedingPlanCode = CedingPlanCode;
                mfrs17ReportingDetail.Status = Status;
                mfrs17ReportingDetail.IsModified = IsModified;
                mfrs17ReportingDetail.GenerateStatus = GenerateStatus;
                mfrs17ReportingDetail.UpdatedAt = DateTime.Now;
                mfrs17ReportingDetail.UpdatedById = UpdatedById ?? mfrs17ReportingDetail.UpdatedById;

                EFExecutionStrategy connectionStrategy = EFExecutionStrategy.GetNewInstance("Mfrs17ReportingDetail");
                connectionStrategy.Execute(() =>
                {
                    db.Entry(mfrs17ReportingDetail).State = EntityState.Modified;
                    db.SaveChanges();
                });

                return trail;
            }
        }

        public static DataTrail Delete(int id)
        {
            using (var db = new AppDbContext())
            {
                db.Database.CommandTimeout = 0;
                Mfrs17ReportingDetail mfrs17ReportingDetail = db.Mfrs17ReportingDetails.Where(q => q.Id == id).FirstOrDefault();
                if (mfrs17ReportingDetail == null)
                {
                    throw new Exception(MessageBag.NoRecordFound);
                }

                DataTrail trail = new DataTrail(mfrs17ReportingDetail, true);

                EFExecutionStrategy connectionStrategy = EFExecutionStrategy.GetNewInstance("Mfrs17ReportingDetail");
                connectionStrategy.Execute(() =>
                {
                    db.Entry(mfrs17ReportingDetail).State = EntityState.Deleted;
                    db.Mfrs17ReportingDetails.Remove(mfrs17ReportingDetail);
                    db.SaveChanges();
                });

                return trail;
            }
        }
    }

    public class Mfrs17ReportingDetailData
    {
        public int CedantId { get; set; }

        public int PremiumFrequencyCodePickListDetailId { get; set; }

        public DateTime LatestDataStartDate { get; set; }

        public DateTime LatestDataEndDate { get; set; }

        public int Record { get; set; }

        public string TreatyCode { get; set; }

        public string RiskQuarter { get; set; }

        public string Mfrs17TreatyCode { get; set; }

        public string CedingPlanCode { get; set; }

        public int? Status { get; set; }
    }
}
