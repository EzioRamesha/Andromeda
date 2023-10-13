using BusinessObject;
using BusinessObject.Identity;
using DataAccess.Entities.Identity;
using DataAccess.EntityFramework;
using Shared;
using Shared.DataAccess;
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
    [Table("StandardRetroOutputs")]
    public class StandardRetroOutput
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int Type { get; set; }

        [Required]
        public int DataType { get; set; }

        [MaxLength(128)]
        public string Code { get; set; }

        [MaxLength(128)]
        public string Name { get; set; }

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

        public StandardRetroOutput()
        {
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
        }

        public static bool IsExists(int id)
        {
            using (var db = new AppDbContext())
            {
                return db.StandardRetroOutputs.Any(q => q.Id == id);
            }
        }

        public bool IsDuplicateCode()
        {
            using (var db = new AppDbContext())
            {
                if (!string.IsNullOrEmpty(Type.ToString()))
                {
                    var query = db.StandardRetroOutputs.Where(q => q.Type == Type);
                    if (Id != 0)
                    {
                        query = query.Where(q => q.Id != Id);
                    }
                    return query.Any();
                }
                return false;
            }
        }

        public static StandardRetroOutput Find(int? id)
        {
            if (!id.HasValue)
                return null;

            using (var db = new AppDbContext())
            {
                return Find(id.Value, db);
            }
        }

        public static StandardRetroOutput Find(int id, AppDbContext db)
        {
            return db.StandardRetroOutputs.Where(q => q.Id == id).FirstOrDefault();
        }

        public DataTrail Create()
        {
            using (var db = new AppDbContext())
            {
                db.StandardRetroOutputs.Add(this);
                db.SaveChanges();

                DataTrail trail = new DataTrail(this);
                return trail;
            }
        }

        public DataTrail Update()
        {
            using (var db = new AppDbContext())
            {
                StandardRetroOutput entity = Find(Id, db);
                if (entity == null)
                {
                    throw new Exception(MessageBag.NoRecordFound);
                }

                DataTrail trail = new DataTrail(entity, this);

                entity.Type = Type;
                entity.Code = Code;
                entity.Name = Name;
                entity.DataType = DataType;
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
                StandardRetroOutput entity = Find(id, db);
                if (entity == null)
                {
                    throw new Exception(MessageBag.NoRecordFound);
                }

                DataTrail trail = new DataTrail(entity, true);

                db.Entry(entity).State = EntityState.Deleted;
                db.StandardRetroOutputs.Remove(entity);
                db.SaveChanges();

                return trail;
            }
        }

        public static Result Result()
        {
            return new Result
            {
                Table = UtilAttribute.GetTableName(typeof(StandardRetroOutput)),
                Controller = ModuleBo.ModuleController.StandardRetroOutput.ToString()
            };
        }

        public static void SeedStandardRetroOutput(User superUser)
        {
            TrailObject trail = new TrailObject();
            string table = UtilAttribute.GetTableName(typeof(StandardRetroOutput));

            for (int i = 1; i <= StandardRetroOutputBo.TypeMax; i++)
            {
                trail = new TrailObject();
                DataTrail dataTrail = null;

                StandardRetroOutput entity = Find(i);

                if (entity == null)
                {
                    entity = new StandardRetroOutput()
                    {
                        Id = i,
                        Type = i,
                        DataType = StandardRetroOutputBo.GetDataTypeByType(i),
                        Code = StandardRetroOutputBo.GetCodeByType(i),
                        Name = StandardRetroOutputBo.GetTypeName(i),
                        CreatedById = superUser.Id,
                        UpdatedById = superUser.Id,
                    };

                    dataTrail = entity.Create();
                }
                else
                {
                    entity.DataType = StandardRetroOutputBo.GetDataTypeByType(i);
                    entity.Code = StandardRetroOutputBo.GetCodeByType(i);
                    entity.Name = StandardRetroOutputBo.GetTypeName(i);

                    dataTrail = entity.Update();
                }

                if (dataTrail != null)
                    dataTrail.Merge(ref trail, table);

                if (trail.IsValid())
                {
                    UserTrailBo userTrailBo = new UserTrailBo(
                        entity.Id,
                        "Create/Update Standard Retro Output",
                        Result(),
                        trail,
                        entity.CreatedById
                    );

                    UserTrail userTrail = new UserTrail(userTrailBo);
                    userTrail.Create();
                }

            }
        }
    }
}
