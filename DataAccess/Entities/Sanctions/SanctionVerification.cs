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

namespace DataAccess.Entities.Sanctions
{
    [Table("SanctionVerifications")]
    public class SanctionVerification
    {
        [Key]
        public int Id { get; set; }

        [Required, Index]
        public int SourceId { get; set; }
        [ExcludeTrail]
        public virtual Source Source { get; set; }

        public bool IsRiData { get; set; } = false;

        public bool IsClaimRegister { get; set; } = false;

        public bool IsReferralClaim { get; set; } = false;

        [Required, Index]
        public int Type { get; set; }

        [Index]
        public int? BatchId { get; set; }

        [Required, Index]
        public int Status { get; set; }

        public int Record { get; set; }

        public int UnprocessedRecords { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? ProcessStartAt { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? ProcessEndAt { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime CreatedAt { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime UpdatedAt { get; set; }

        [Required]
        public int CreatedById { get; set; }
        public int? UpdatedById { get; set; }

        [ExcludeTrail]
        public virtual User CreatedBy { get; set; }
        [ExcludeTrail]
        public virtual User UpdatedBy { get; set; }

        public SanctionVerification()
        {
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
        }

        public static bool IsExists(int id)
        {
            using (var db = new AppDbContext())
            {
                return db.SanctionVerifications.Any(q => q.Id == id);
            }
        }

        public static SanctionVerification Find(int? id)
        {
            using (var db = new AppDbContext())
            {
                return db.SanctionVerifications.Where(q => q.Id == id).FirstOrDefault();
            }
        }

        public DataTrail Create()
        {
            using (var db = new AppDbContext())
            {
                db.Database.CommandTimeout = 0;

                EFExecutionStrategy connectionStrategy = EFExecutionStrategy.GetNewInstance("SanctionVerification");

                connectionStrategy.Execute(() =>
                {
                    db.SanctionVerifications.Add(this);
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
                SanctionVerification sanctionVerification = Find(Id);
                if (sanctionVerification == null)
                {
                    throw new Exception(MessageBag.NoRecordFound);
                }

                DataTrail trail = new DataTrail(sanctionVerification, this);

                sanctionVerification.SourceId = SourceId;
                sanctionVerification.IsRiData = IsRiData;
                sanctionVerification.IsClaimRegister = IsClaimRegister;
                sanctionVerification.IsReferralClaim = IsReferralClaim;
                sanctionVerification.Type = Type;
                sanctionVerification.BatchId = BatchId;
                sanctionVerification.Status = Status;
                sanctionVerification.Record = Record;
                sanctionVerification.UnprocessedRecords = UnprocessedRecords;
                sanctionVerification.ProcessStartAt = ProcessStartAt;
                sanctionVerification.ProcessEndAt = ProcessEndAt;
                sanctionVerification.UpdatedAt = DateTime.Now;
                sanctionVerification.UpdatedById = UpdatedById ?? sanctionVerification.UpdatedById;

                db.Entry(sanctionVerification).State = EntityState.Modified;
                db.SaveChanges();

                return trail;
            }
        }

        public static DataTrail Delete(int id)
        {
            using (var db = new AppDbContext())
            {
                SanctionVerification sanctionVerification = db.SanctionVerifications.Where(q => q.Id == id).FirstOrDefault();
                if (sanctionVerification == null)
                {
                    throw new Exception(MessageBag.NoRecordFound);
                }

                DataTrail trail = new DataTrail(sanctionVerification, true);

                db.Entry(sanctionVerification).State = EntityState.Deleted;
                db.SanctionVerifications.Remove(sanctionVerification);
                db.SaveChanges();

                return trail;
            }
        }
    }
}
