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
    [Table("DirectRetroConfigurations")]
    public class DirectRetroConfiguration
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(255)]
        public string Name { get; set; }

        [Required, Index]
        public int TreatyCodeId { get; set; }

        [ExcludeTrail]
        public virtual TreatyCode TreatyCode { get; set; }

        [Required]
        public string RetroParty { get; set; }

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

        [ExcludeTrail]
        public virtual ICollection<DirectRetroConfigurationMapping> DirectRetroConfigurationMappings { get; set; }

        public DirectRetroConfiguration()
        {
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
        }

        public static bool IsExists(int id)
        {
            using (var db = new AppDbContext())
            {
                return db.DirectRetroConfigurations.Any(q => q.Id == id);
            }
        }

        public static DirectRetroConfiguration Find(int? id)
        {
            using (var db = new AppDbContext())
            {
                return db.DirectRetroConfigurations.Where(q => q.Id == id).FirstOrDefault();
            }
        }

        public static int CountByTreatyCodeId(int treatyCodeId)
        {
            using (var db = new AppDbContext())
            {
                return db.DirectRetroConfigurations.Where(q => q.TreatyCodeId == treatyCodeId).Count();
            }
        }

        public static IList<DirectRetroConfiguration> Get()
        {
            using (var db = new AppDbContext())
            {
                return db.DirectRetroConfigurations.ToList();
            }
        }

        public DataTrail Create()
        {
            using (var db = new AppDbContext())
            {
                db.DirectRetroConfigurations.Add(this);
                db.SaveChanges();

                DataTrail trail = new DataTrail(this);
                return trail;
            }
        }

        public DataTrail Update()
        {
            using (var db = new AppDbContext())
            {
                DirectRetroConfiguration directRetroConfiguration = Find(Id);
                if (directRetroConfiguration == null)
                {
                    throw new Exception(MessageBag.NoRecordFound);
                }

                DataTrail trail = new DataTrail(directRetroConfiguration, this);

                directRetroConfiguration.Name = Name;
                directRetroConfiguration.TreatyCodeId = TreatyCodeId;
                directRetroConfiguration.RetroParty = RetroParty;
                directRetroConfiguration.UpdatedAt = DateTime.Now;
                directRetroConfiguration.UpdatedById = UpdatedById ?? directRetroConfiguration.UpdatedById;

                db.Entry(directRetroConfiguration).State = EntityState.Modified;
                db.SaveChanges();

                return trail;
            }
        }

        public static DataTrail Delete(int id)
        {
            using (var db = new AppDbContext())
            {
                DirectRetroConfiguration directRetroConfiguration = db.DirectRetroConfigurations.Where(q => q.Id == id).FirstOrDefault();
                if (directRetroConfiguration == null)
                {
                    throw new Exception(MessageBag.NoRecordFound);
                }

                DataTrail trail = new DataTrail(directRetroConfiguration, true);

                db.Entry(directRetroConfiguration).State = EntityState.Deleted;
                db.DirectRetroConfigurations.Remove(directRetroConfiguration);
                db.SaveChanges();

                return trail;
            }
        }

        public override string ToString()
        {
            return string.Format("{0}", Name);
        }
    }
}
