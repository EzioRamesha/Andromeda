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
    [Table("Mfrs17ContractCodeDetails")]
    public class Mfrs17ContractCodeDetail
    {
        [Key]
        public int Id { get; set; }

        [Required, Index]
        public int Mfrs17ContractCodeId { get; set; }

        [ForeignKey(nameof(Mfrs17ContractCodeId))]
        [ExcludeTrail]
        public virtual Mfrs17ContractCode Mfrs17ContractCode { get; set; }

        [Index, MaxLength(255)]
        public string ContractCode { get; set; }

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

        public Mfrs17ContractCodeDetail()
        {
            CreatedAt = DateTime.Now;
        }

        public static bool IsExists(int id)
        {
            using (var db = new AppDbContext())
            {
                return db.Mfrs17ContractCodeDetails.Any(q => q.Id == id);
            }
        }

        public static Mfrs17ContractCodeDetail Find(int? id)
        {
            using (var db = new AppDbContext())
            {
                return db.Mfrs17ContractCodeDetails.Where(q => q.Id == id).FirstOrDefault();
            }
        }

        public static IList<Mfrs17ContractCodeDetail> GetByMfrs17ContractCodeId(int id)
        {
            using (var db = new AppDbContext())
            {
                return db.Mfrs17ContractCodeDetails.Where(q => q.Mfrs17ContractCodeId == id).ToList();
            }
        }

        public DataTrail Create()
        {
            using (var db = new AppDbContext())
            {
                db.Mfrs17ContractCodeDetails.Add(this);
                db.SaveChanges();

                var trail = new DataTrail(this);
                return trail;
            }
        }

        public DataTrail Update()
        {
            using (var db = new AppDbContext())
            {
                var entity = Mfrs17ContractCodeDetail.Find(Id);
                if (entity == null)
                {
                    throw new Exception(MessageBag.NoRecordFound);
                }

                var trail = new DataTrail(entity, this);

                entity.Mfrs17ContractCodeId = Mfrs17ContractCodeId;
                entity.ContractCode = ContractCode;

                db.Entry(entity).State = EntityState.Modified;
                db.SaveChanges();

                return trail;
            }
        }

        public static DataTrail Delete(int id)
        {
            using (var db = new AppDbContext())
            {
                var entity = db.Mfrs17ContractCodeDetails.Where(q => q.Id == id).FirstOrDefault();
                if (entity == null)
                {
                    throw new Exception(MessageBag.NoRecordFound);
                }

                var trail = new DataTrail(entity, true);

                db.Entry(entity).State = EntityState.Deleted;
                db.Mfrs17ContractCodeDetails.Remove(entity);
                db.SaveChanges();

                return trail;
            }
        }

        public static IList<DataTrail> DeleteByMfrs17ContractCodeId(int mfrs17ContractCodeId)
        {
            using (var db = new AppDbContext())
            {
                var query = db.Mfrs17ContractCodeDetails.Where(q => q.Mfrs17ContractCodeId == mfrs17ContractCodeId);

                var trails = new List<DataTrail>();
                foreach (Mfrs17ContractCodeDetail mfrs17ContractCodeDetail in query.ToList())
                {
                    var trail = new DataTrail(mfrs17ContractCodeDetail, true);
                    trails.Add(trail);

                    db.Entry(mfrs17ContractCodeDetail).State = EntityState.Deleted;
                    db.Mfrs17ContractCodeDetails.Remove(mfrs17ContractCodeDetail);
                }

                db.SaveChanges();
                return trails;
            }
        }

        public static IList<Mfrs17ContractCodeDetail> GetByMfrs17ContractCodeIdExcept(int Mfrs17ContractCodeId, List<int> ids)
        {
            using (var db = new AppDbContext())
            {
                return db.Mfrs17ContractCodeDetails.Where(q => q.Mfrs17ContractCodeId == Mfrs17ContractCodeId && !ids.Contains(q.Id)).ToList();
            }
        }
    }

}
