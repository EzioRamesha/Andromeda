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
    [Table("ReferralRiDataFiles")]
    public class ReferralRiDataFile
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int RawFileId { get; set; }

        [ForeignKey(nameof(RawFileId))]
        [ExcludeTrail]
        public virtual RawFile RawFile { get; set; }

        public int Records { get; set; }

        public int UpdatedRecords { get; set; }

        public string Error { get; set; }

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

        public ReferralRiDataFile()
        {
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
        }

        public static ReferralRiDataFile Find(int? id)
        {
            using (var db = new AppDbContext())
            {
                return db.ReferralRiDataFiles.Where(q => q.Id == id).FirstOrDefault();
            }
        }

        public static IList<ReferralRiDataFile> Get()
        {
            using (var db = new AppDbContext())
            {
                return db.ReferralRiDataFiles.ToList();
            }
        }

        public DataTrail Create()
        {
            using (var db = new AppDbContext())
            {
                db.ReferralRiDataFiles.Add(this);
                db.SaveChanges();

                DataTrail trail = new DataTrail(this);
                return trail;
            }
        }

        public DataTrail Update()
        {
            using (var db = new AppDbContext())
            {
                ReferralRiDataFile entity = Find(Id);
                if (entity == null)
                {
                    throw new Exception(MessageBag.NoRecordFound);
                }

                DataTrail trail = new DataTrail(entity, this);

                entity.RawFileId = RawFileId;
                entity.Records = Records;
                entity.UpdatedRecords = UpdatedRecords;
                entity.Error = Error;
                entity.UpdatedAt = DateTime.Now;
                entity.UpdatedById = UpdatedById ?? entity.UpdatedById;

                db.Entry(entity).State = EntityState.Modified;
                db.SaveChanges();

                return trail;
            }
        }
    }
}
