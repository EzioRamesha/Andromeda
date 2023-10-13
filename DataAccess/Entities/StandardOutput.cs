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
    [Table("StandardOutputs")]
    public class StandardOutput
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

        public StandardOutput()
        {
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
        }

        public static bool IsExists(int id)
        {
            using (var db = new AppDbContext())
            {
                return db.StandardOutputs.Any(q => q.Id == id);
            }
        }

        public bool IsDuplicateCode()
        {
            using (var db = new AppDbContext())
            {
                if (!string.IsNullOrEmpty(Type.ToString()))
                {
                    var query = db.StandardOutputs.Where(q => q.Type == Type);
                    if (Id != 0)
                    {
                        query = query.Where(q => q.Id != Id);
                    }
                    return query.Count() > 0;
                }
                return false;
            }
        }

        public static StandardOutput Find(int id)
        {
            using (var db = new AppDbContext())
            {
                return db.StandardOutputs.Where(q => q.Id == id).FirstOrDefault();
            }
        }

        public static StandardOutput Find(int? id)
        {
            using (var db = new AppDbContext())
            {
                db.Database.CommandTimeout = 0;

                if (id != null)
                {
                    EFExecutionStrategy connectionStrategy = EFExecutionStrategy.GetNewInstance("StandardOutput");
                    return connectionStrategy.Execute(() => db.StandardOutputs.Where(q => q.Id == id).FirstOrDefault());
                }

                return null;
            }
        }

        public static StandardOutput FindByType(int type)
        {
            using (var db = new AppDbContext())
            {
                return db.StandardOutputs.Where(q => q.Type == type).FirstOrDefault();
            }
        }

        public static IList<StandardOutput> Get()
        {
            using (var db = new AppDbContext())
            {
                return db.StandardOutputs.Where(q => q.Id != StandardOutputBo.TypeCustomField).OrderBy(q => q.Code).ToList();
            }
        }

        public DataTrail Create()
        {
            using (var db = new AppDbContext())
            {
                db.StandardOutputs.Add(this);
                db.SaveChanges();

                DataTrail trail = new DataTrail(this);
                return trail;
            }
        }

        public DataTrail Update()
        {
            using (var db = new AppDbContext())
            {
                StandardOutput standardOutput = StandardOutput.Find(Id);
                if (standardOutput == null)
                {
                    throw new Exception(MessageBag.NoRecordFound);
                }

                DataTrail trail = new DataTrail(standardOutput, this);

                standardOutput.Type = Type;
                standardOutput.Code = Code;
                standardOutput.Name = Name;
                standardOutput.DataType = DataType;
                standardOutput.UpdatedAt = DateTime.Now;
                standardOutput.UpdatedById = UpdatedById ?? standardOutput.UpdatedById;

                db.Entry(standardOutput).State = EntityState.Modified;
                db.SaveChanges();

                return trail;
            }
        }

        public static DataTrail Delete(int id)
        {
            using (var db = new AppDbContext())
            {
                StandardOutput standardOutput = db.StandardOutputs.Where(q => q.Id == id).FirstOrDefault();
                if (standardOutput == null)
                {
                    throw new Exception(MessageBag.NoRecordFound);
                }

                DataTrail trail = new DataTrail(standardOutput, true);

                db.Entry(standardOutput).State = EntityState.Deleted;
                db.StandardOutputs.Remove(standardOutput);
                db.SaveChanges();

                return trail;
            }
        }

        public static Result Result()
        {
            return new Result
            {
                Table = UtilAttribute.GetTableName(typeof(StandardOutput)),
                Controller = ModuleBo.ModuleController.StandardOutput.ToString()
            };
        }

        public static void SeedStandardOutput(User superUser)
        {
            TrailObject trail = new TrailObject();
            string table = UtilAttribute.GetTableName(typeof(StandardOutput));

            for (int i = StandardOutputBo.TypeCustomField; i <= StandardOutputBo.TypeMax; i++)
            {
                trail = new TrailObject();
                DataTrail dataTrail = null;

                StandardOutput standardOutput = Find(i);

                if (standardOutput == null)
                {
                    standardOutput = new StandardOutput()
                    {
                        Id = i,
                        Type = i,
                        DataType = StandardOutputBo.GetDataTypeByType(i),
                        Code = StandardOutputBo.GetCodeByType(i),
                        Name = StandardOutputBo.GetTypeName(i),
                        CreatedById = superUser.Id,
                        UpdatedById = superUser.Id,
                    };

                    dataTrail = standardOutput.Create();
                }
                else
                {
                    standardOutput.DataType = StandardOutputBo.GetDataTypeByType(i);
                    standardOutput.Code = StandardOutputBo.GetCodeByType(i);
                    standardOutput.Name = StandardOutputBo.GetTypeName(i);

                    dataTrail = standardOutput.Update();
                }

                if (dataTrail != null)
                    dataTrail.Merge(ref trail, table);

                if (trail.IsValid())
                {
                    UserTrailBo userTrailBo = new UserTrailBo(
                        standardOutput.Id,
                        "Create/Update Standard Output",
                        Result(),
                        trail,
                        standardOutput.CreatedById
                    );

                    UserTrail userTrail = new UserTrail(userTrailBo);
                    userTrail.Create();
                }

            }
        }
    }
}
