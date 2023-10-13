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
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Entities
{
    [Table("Mfrs17Reportings")]
    public class Mfrs17Reporting
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(64), Index]
        public string Quarter { get; set; }

        [Required, Index]
        public int Status { get; set; }

        [Required, Index]
        public int TotalRecord { get; set; }

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

        public int? GenerateType { get; set; }

        public bool? GenerateModifiedOnly { get; set; }

        public double? GeneratePercentage { get; set; }

        [Required, Index]
        public int CutOffId { get; set; }

        [ExcludeTrail]
        public virtual CutOff CutOff { get; set; }

        [Index]
        public bool? IsResume { get; set; } = false;

        public Mfrs17Reporting()
        {
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
        }

        public static bool IsExists(int id)
        {
            using (var db = new AppDbContext())
            {
                db.Database.CommandTimeout = 0;

                EFExecutionStrategy connectionStrategy = EFExecutionStrategy.GetNewInstance("Mfrs17Reporting");

                return connectionStrategy.Execute(() =>
                {
                    return db.Mfrs17Reportings.Any(q => q.Id == id);
                });
            }
        }

        public static Mfrs17Reporting Find(int id)
        {
            using (var db = new AppDbContext())
            {
                db.Database.CommandTimeout = 0;

                EFExecutionStrategy connectionStrategy = EFExecutionStrategy.GetNewInstance("Mfrs17Reporting");

                return connectionStrategy.Execute(() =>
                {
                    return db.Mfrs17Reportings.Where(q => q.Id == id).FirstOrDefault();
                });
            }
        }

        public static Mfrs17Reporting FindByQuarter(string quarter)
        {
            using (var db = new AppDbContext())
            {
                db.Database.CommandTimeout = 0;

                EFExecutionStrategy connectionStrategy = EFExecutionStrategy.GetNewInstance("Mfrs17Reporting");

                return connectionStrategy.Execute(() =>
                {
                    return db.Mfrs17Reportings.Where(q => q.Quarter.Contains(quarter)).FirstOrDefault();
                });
            }
        }

        public static Mfrs17Reporting FindByStatus(int status)
        {
            using (var db = new AppDbContext())
            {
                db.Database.CommandTimeout = 0;

                EFExecutionStrategy connectionStrategy = EFExecutionStrategy.GetNewInstance("Mfrs17Reporting");

                return connectionStrategy.Execute(() =>
                {
                    return db.Mfrs17Reportings.Where(q => q.Status == status).FirstOrDefault();
                });
            }
        }

        public static int CountByStatus(int status)
        {
            using (var db = new AppDbContext())
            {
                db.Database.CommandTimeout = 0;

                EFExecutionStrategy connectionStrategy = EFExecutionStrategy.GetNewInstance("Mfrs17Reporting");

                return connectionStrategy.Execute(() =>
                {
                    return db.Mfrs17Reportings.Where(q => q.Status == status).Count();
                });
            }
        }

        public DataTrail Create()
        {
            using (var db = new AppDbContext())
            {
                db.Database.CommandTimeout = 0;

                EFExecutionStrategy connectionStrategy = EFExecutionStrategy.GetNewInstance("Mfrs17Reporting");

                connectionStrategy.Execute(() =>
                {
                    db.Mfrs17Reportings.Add(this);
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
                Mfrs17Reporting mfrs17Reporting = Mfrs17Reporting.Find(Id);
                if (mfrs17Reporting == null)
                {
                    throw new Exception(MessageBag.NoRecordFound);
                }

                DataTrail trail = new DataTrail(mfrs17Reporting, this);

                mfrs17Reporting.Quarter = Quarter;
                mfrs17Reporting.Status = Status;
                mfrs17Reporting.TotalRecord = TotalRecord;
                mfrs17Reporting.GenerateType = GenerateType;
                mfrs17Reporting.GenerateModifiedOnly = GenerateModifiedOnly;
                mfrs17Reporting.GeneratePercentage = GeneratePercentage;
                mfrs17Reporting.CutOffId = CutOffId;
                mfrs17Reporting.IsResume = IsResume;
                mfrs17Reporting.UpdatedAt = DateTime.Now;
                mfrs17Reporting.UpdatedById = UpdatedById ?? mfrs17Reporting.UpdatedById;
                db.Entry(mfrs17Reporting).State = EntityState.Modified;

                EFExecutionStrategy connectionStrategy = EFExecutionStrategy.GetNewInstance("Mfrs17Reporting");
                connectionStrategy.Execute(() =>
                {
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
                Mfrs17Reporting mfrs17Reporting = db.Mfrs17Reportings.Where(q => q.Id == id).FirstOrDefault();
                if (mfrs17Reporting == null)
                {
                    throw new Exception(MessageBag.NoRecordFound);
                }

                DataTrail trail = new DataTrail(mfrs17Reporting, true);

                db.Entry(mfrs17Reporting).State = EntityState.Deleted;
                db.Mfrs17Reportings.Remove(mfrs17Reporting);

                EFExecutionStrategy connectionStrategy = EFExecutionStrategy.GetNewInstance("Mfrs17Reporting");
                connectionStrategy.Execute(() =>
                {
                    db.SaveChanges();
                });
                return trail;
            }
        }
    }
}
