using DataAccess.Entities.Identity;
using DataAccess.Entities.SoaDatas;
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
    [Table("DirectRetro")]
    public class DirectRetro
    {
        [Key]
        public int Id { get; set; }

        [Required, Index]
        public int CedantId { get; set; }

        [ExcludeTrail]
        public virtual Cedant Cedant { get; set; }

        [Required, Index]
        public int TreatyCodeId { get; set; }

        [ExcludeTrail]
        public virtual TreatyCode TreatyCode { get; set; }

        [Required, MaxLength(10), Index]
        public string SoaQuarter { get; set; }

        [Required, Index]
        public int SoaDataBatchId { get; set; }

        [ExcludeTrail]
        public virtual SoaDataBatch SoaDataBatch { get; set; }

        [Index]
        public int RetroStatus { get; set; }

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

        public DirectRetro()
        {
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
        }

        public static bool IsExists(int id)
        {
            using (var db = new AppDbContext())
            {
                return db.DirectRetro.Any(q => q.Id == id);
            }
        }

        public static DirectRetro Find(int? id)
        {
            using (var db = new AppDbContext())
            {
                return db.DirectRetro.Where(q => q.Id == id).FirstOrDefault();
            }
        }

        public DataTrail Create()
        {
            using (var db = new AppDbContext())
            {
                db.DirectRetro.Add(this);
                db.SaveChanges();

                DataTrail trail = new DataTrail(this);
                return trail;
            }
        }

        public DataTrail Update()
        {
            using (var db = new AppDbContext())
            {
                DirectRetro directRetro = Find(Id);
                if (directRetro == null)
                {
                    throw new Exception(MessageBag.NoRecordFound);
                }

                DataTrail trail = new DataTrail(directRetro, this);

                directRetro.CedantId = CedantId;
                directRetro.TreatyCodeId = TreatyCodeId;
                directRetro.SoaQuarter = SoaQuarter;
                directRetro.SoaDataBatchId = SoaDataBatchId;
                directRetro.RetroStatus = RetroStatus;
                directRetro.UpdatedAt = DateTime.Now;
                directRetro.UpdatedById = UpdatedById ?? directRetro.UpdatedById;

                db.Entry(directRetro).State = EntityState.Modified;
                db.SaveChanges();

                return trail;
            }
        }

        public static DataTrail Delete(int id)
        {
            using (var db = new AppDbContext())
            {
                DirectRetro directRetro = db.DirectRetro.Where(q => q.Id == id).FirstOrDefault();
                if (directRetro == null)
                {
                    throw new Exception(MessageBag.NoRecordFound);
                }

                DataTrail trail = new DataTrail(directRetro, true);

                db.Entry(directRetro).State = EntityState.Deleted;
                db.DirectRetro.Remove(directRetro);
                db.SaveChanges();

                return trail;
            }
        }
    }
}
