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

namespace DataAccess.Entities.Retrocession
{
    [Table("RetroTreaties")]
    public class RetroTreaty
    {
        [Key]
        public int Id { get; set; }

        [Index]
        public int? RetroPartyId { get; set; } // To be changed to required when merge
        [ExcludeTrail]
        public virtual RetroParty RetroParty { get; set; }

        [Required, Index]
        public int Status { get; set; }

        [MaxLength(50)]
        public string Party { get; set; } // To be removed when merge

        [Required, MaxLength(50), Index]
        public string Code { get; set; }

        [Required, Index]
        public int TreatyTypePickListDetailId { get; set; }
        [ExcludeTrail]
        public virtual PickListDetail TreatyTypePickListDetail { get; set; }

        [Index]
        public bool IsLobAutomatic { get; set; } = false;

        [Index]
        public bool IsLobFacultative { get; set; } = false;

        [Index]
        public bool IsLobAdvantageProgram { get; set; } = false;

        [Index]
        public double? RetroShare { get; set; }

        [Index]
        public int? TreatyDiscountTableId { get; set; }
        [ExcludeTrail]
        public virtual TreatyDiscountTable TreatyDiscountTable { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? EffectiveStartDate { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? EffectiveEndDate { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime CreatedAt { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime UpdatedAt { get; set; }

        [Required]
        public int CreatedById { get; set; }
        [ExcludeTrail]
        public virtual User CreatedBy { get; set; }

        public int? UpdatedById { get; set; }
        [ExcludeTrail]
        public virtual User UpdatedBy { get; set; }

        public RetroTreaty()
        {
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
        }

        public static bool IsExists(int id)
        {
            using (var db = new AppDbContext())
            {
                return db.RetroTreaties.Any(q => q.Id == id);
            }
        }

        public bool IsDuplicateParty()
        {
            using (var db = new AppDbContext())
            {
                var query = db.RetroTreaties.Where(q => q.Party == Party);
                if (Id != 0)
                {
                    query = query.Where(q => q.Id != Id);
                }
                return query.Count() > 0;
            }
        }

        public static RetroTreaty Find(int? id)
        {
            using (var db = new AppDbContext())
            {
                return db.RetroTreaties.Where(q => q.Id == id).FirstOrDefault();
            }
        }

        public DataTrail Create()
        {
            using (var db = new AppDbContext())
            {
                db.RetroTreaties.Add(this);
                db.SaveChanges();

                DataTrail trail = new DataTrail(this);
                return trail;
            }
        }

        public DataTrail Update()
        {
            using (var db = new AppDbContext())
            {
                RetroTreaty entity = Find(Id);
                if (entity == null)
                {
                    throw new Exception(MessageBag.NoRecordFound);
                }

                DataTrail trail = new DataTrail(entity, this);

                entity.RetroPartyId = RetroPartyId;
                entity.Status = Status;
                //entity.Party = Party;
                entity.Code = Code;
                entity.TreatyTypePickListDetailId = TreatyTypePickListDetailId;
                entity.IsLobAutomatic = IsLobAutomatic;
                entity.IsLobFacultative = IsLobFacultative;
                entity.IsLobAdvantageProgram = IsLobAdvantageProgram;
                entity.RetroShare = RetroShare;
                entity.TreatyDiscountTableId = TreatyDiscountTableId;
                entity.EffectiveStartDate = EffectiveStartDate;
                entity.EffectiveEndDate = EffectiveEndDate;
                entity.UpdatedAt = DateTime.Now;
                entity.UpdatedById = UpdatedById ?? entity.UpdatedById;

                db.Entry(entity).State = EntityState.Modified;
                db.SaveChanges();

                return trail;
            }
        }

        public static DataTrail Delete(int id)
        {
            using (var db = new AppDbContext())
            {
                RetroTreaty entity = db.RetroTreaties.Where(q => q.Id == id).FirstOrDefault();
                if (entity == null)
                {
                    throw new Exception(MessageBag.NoRecordFound);
                }

                DataTrail trail = new DataTrail(entity, true);

                db.Entry(entity).State = EntityState.Deleted;
                db.RetroTreaties.Remove(entity);
                db.SaveChanges();

                return trail;
            }
        }

        public static int CountByTreatyDiscountTableById(int treatyDiscountTableId)
        {
            using (var db = new AppDbContext())
            {
                var count = 0;
                count = db.RetroTreaties.Where(q => q.TreatyDiscountTableId == treatyDiscountTableId).Count();

                return count;
            }
        }
    }
}
