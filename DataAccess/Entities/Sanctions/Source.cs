using BusinessObject;
using BusinessObject.Identity;
using BusinessObject.Sanctions;
using DataAccess.Entities.Identity;
using DataAccess.EntityFramework;
using Shared;
using Shared.DataAccess;
using Shared.Trails;
using Shared.Trails.Attributes;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace DataAccess.Entities.Sanctions
{
    [Table("Sources")]
    public class Source
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(128), Index]
        public string Name { get; set; }

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

        public Source()
        {
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
        }

        public Source(SourceBo sourceBo) : this()
        {
            Id = sourceBo.Id;
            Name = sourceBo.Name;
            CreatedById = sourceBo.CreatedById;
            UpdatedById = sourceBo.UpdatedById;
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
        }

        public static Result Result()
        {
            return new Result
            {
                Table = UtilAttribute.GetTableName(typeof(Source)),
                Controller = ModuleBo.ModuleController.Source.ToString()
            };
        }

        public static bool IsExists(int id)
        {
            using (var db = new AppDbContext())
            {
                return db.Sources.Any(q => q.Id == id);
            }
        }

        public static Source Find(int? id)
        {
            using (var db = new AppDbContext())
            {
                return db.Sources.Where(q => q.Id == id).FirstOrDefault();
            }
        }

        public DataTrail Create()
        {
            using (var db = new AppDbContext())
            {
                db.Sources.Add(this);
                db.SaveChanges();

                DataTrail trail = new DataTrail(this);
                return trail;
            }
        }

        public DataTrail Update()
        {
            using (var db = new AppDbContext())
            {
                Source source = Find(Id);
                if (source == null)
                {
                    throw new Exception(MessageBag.NoRecordFound);
                }

                DataTrail trail = new DataTrail(source, this);

                source.Name = Name;
                source.UpdatedAt = DateTime.Now;
                source.UpdatedById = UpdatedById ?? source.UpdatedById;

                db.Entry(source).State = EntityState.Modified;
                db.SaveChanges();

                return trail;
            }
        }

        public static DataTrail Delete(int id)
        {
            using (var db = new AppDbContext())
            {
                Source source = db.Sources.Where(q => q.Id == id).FirstOrDefault();
                if (source == null)
                {
                    throw new Exception(MessageBag.NoRecordFound);
                }

                DataTrail trail = new DataTrail(source, true);

                db.Entry(source).State = EntityState.Deleted;
                db.Sources.Remove(source);
                db.SaveChanges();

                return trail;
            }
        }

        public static void SeedSource(User superUser)
        {
            var trail = new TrailObject();
            string table = UtilAttribute.GetTableName(typeof(Module));

            foreach (var sourceBo in SourceBo.GetSources())
            {
                trail = new TrailObject();
                DataTrail dataTrail = null;

                var source = Find(sourceBo.Id);
                if (source == null)
                {
                    source = new Source(sourceBo)
                    {
                        CreatedById = superUser.Id,
                        UpdatedById = superUser.Id
                    };
                    dataTrail = source.Create();
                }

                if (dataTrail != null)
                    dataTrail.Merge(ref trail, table);

                if (trail.IsValid())
                {
                    var userTrailBo = new UserTrailBo(
                        source.Id,
                        "Create Source",
                        Result(),
                        trail,
                        source.CreatedById
                    );

                    var userTrail = new UserTrail(userTrailBo);
                    userTrail.Create();
                }
            }
        }
    }
}
