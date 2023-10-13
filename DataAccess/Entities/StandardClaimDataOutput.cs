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

namespace DataAccess.Entities
{
    [Table("StandardClaimDataOutputs")]
    public class StandardClaimDataOutput
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

        [ExcludeTrail]
        public virtual User CreatedBy { get; set; }
        [ExcludeTrail]
        public virtual User UpdatedBy { get; set; }

        public StandardClaimDataOutput()
        {
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
        }

        public static bool IsExists(int id)
        {
            using (var db = new AppDbContext())
            {
                return db.StandardClaimDataOutputs.Any(q => q.Id == id);
            }
        }

        public bool IsDuplicateCode()
        {
            using (var db = new AppDbContext())
            {
                if (!string.IsNullOrEmpty(Type.ToString()))
                {
                    var query = db.StandardClaimDataOutputs.Where(q => q.Type == Type);
                    if (Id != 0)
                    {
                        query = query.Where(q => q.Id != Id);
                    }
                    return query.Count() > 0;
                }
                return false;
            }
        }

        public static StandardClaimDataOutput Find(int id)
        {
            using (var db = new AppDbContext())
            {
                return db.StandardClaimDataOutputs.Where(q => q.Id == id).FirstOrDefault();
            }
        }

        public static StandardClaimDataOutput Find(int? id)
        {
            using (var db = new AppDbContext())
            {
                if (id != null)
                    return db.StandardClaimDataOutputs.Where(q => q.Id == id).FirstOrDefault();
                return null;
            }
        }

        public static StandardClaimDataOutput FindByType(int type)
        {
            using (var db = new AppDbContext())
            {
                return db.StandardClaimDataOutputs.Where(q => q.Type == type).FirstOrDefault();
            }
        }

        public static IList<StandardClaimDataOutput> Get()
        {
            using (var db = new AppDbContext())
            {
                return db.StandardClaimDataOutputs.OrderBy(q => q.Code).ToList();
            }
        }

        public DataTrail Create()
        {
            using (var db = new AppDbContext())
            {
                db.StandardClaimDataOutputs.Add(this);
                db.SaveChanges();

                return new DataTrail(this);
            }
        }

        public DataTrail Update()
        {
            using (var db = new AppDbContext())
            {
                var entity = Find(Id);
                if (entity == null)
                    throw new Exception(MessageBag.NoRecordFound);

                var trail = new DataTrail(entity, this);

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
                var entity = Find(id);
                if (entity == null)
                    throw new Exception(MessageBag.NoRecordFound);

                var trail = new DataTrail(entity, true);

                db.Entry(entity).State = EntityState.Deleted;
                db.StandardClaimDataOutputs.Remove(entity);
                db.SaveChanges();

                return trail;
            }
        }

        public static Result Result()
        {
            return new Result
            {
                Table = UtilAttribute.GetTableName(typeof(StandardClaimDataOutput)),
                Controller = ModuleBo.ModuleController.StandardClaimDataOutput.ToString()
            };
        }

        public static void SeedClaimOutput(User superUser)
        {
            var trail = new TrailObject();
            var table = UtilAttribute.GetTableName(typeof(StandardClaimDataOutput));

            for (int i = 1; i <= StandardClaimDataOutputBo.TypeMax; i++)
            {
                trail = new TrailObject();
                var standardClaimDataOutput = Find(i);

                DataTrail dataTrail;
                if (standardClaimDataOutput == null)
                {
                    standardClaimDataOutput = new StandardClaimDataOutput()
                    {
                        Id = i,
                        Type = i,
                        DataType = StandardClaimDataOutputBo.GetDataTypeByType(i),
                        Code = StandardClaimDataOutputBo.GetCodeByType(i),
                        Name = StandardClaimDataOutputBo.GetTypeName(i),
                        CreatedById = superUser.Id,
                        UpdatedById = superUser.Id,
                    };

                    dataTrail = standardClaimDataOutput.Create();
                }
                else
                {
                    standardClaimDataOutput.DataType = StandardClaimDataOutputBo.GetDataTypeByType(i);
                    standardClaimDataOutput.Code = StandardClaimDataOutputBo.GetCodeByType(i);
                    standardClaimDataOutput.Name = StandardClaimDataOutputBo.GetTypeName(i);

                    dataTrail = standardClaimDataOutput.Update();
                }

                if (dataTrail != null)
                    dataTrail.Merge(ref trail, table);

                if (trail.IsValid())
                {
                    var userTrailBo = new UserTrailBo(
                        standardClaimDataOutput.Id,
                        "Create/Update Standard Claim Data Output",
                        Result(),
                        trail,
                        standardClaimDataOutput.CreatedById
                    );

                    var userTrail = new UserTrail(userTrailBo);
                    userTrail.Create();
                }

            }
        }
    }
}
