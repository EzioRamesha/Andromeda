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
    [Table("StandardSoaDataOutputs")]
    public class StandardSoaDataOutput
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

        public StandardSoaDataOutput()
        {
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
        }

        public static bool IsExists(int id)
        {
            using (var db = new AppDbContext())
            {
                return db.StandardSoaDataOutputs.Any(q => q.Id == id);
            }
        }

        public bool IsDuplicateCode()
        {
            using (var db = new AppDbContext())
            {
                if (!string.IsNullOrEmpty(Code))
                {
                    var query = db.StandardSoaDataOutputs.Where(q => q.Code == Code);
                    if (Id != 0)
                    {
                        query = query.Where(q => q.Id != Id);
                    }
                    return query.Count() > 0;
                }
                return false;
            }
        }

        public static StandardSoaDataOutput Find(int id)
        {
            using (var db = new AppDbContext())
            {
                return db.StandardSoaDataOutputs.Where(q => q.Id == id).FirstOrDefault();
            }
        }

        public static StandardSoaDataOutput Find(int? id)
        {
            using (var db = new AppDbContext())
            {
                if (id != null)
                    return db.StandardSoaDataOutputs.Where(q => q.Id == id).FirstOrDefault();
                return null;
            }
        }

        public static StandardSoaDataOutput FindByType(int type)
        {
            using (var db = new AppDbContext())
            {
                return db.StandardSoaDataOutputs.Where(q => q.Type == type).FirstOrDefault();
            }
        }

        public static StandardSoaDataOutput FindByCode(string code)
        {
            using (var db = new AppDbContext())
            {
                return db.StandardSoaDataOutputs.Where(q => q.Code == code).FirstOrDefault();
            }
        }

        public static IList<StandardSoaDataOutput> Get()
        {
            using (var db = new AppDbContext())
            {
                return db.StandardSoaDataOutputs.OrderBy(q => q.Code).ToList();
            }
        }

        public DataTrail Create()
        {
            using (var db = new AppDbContext())
            {
                db.StandardSoaDataOutputs.Add(this);
                db.SaveChanges();

                DataTrail trail = new DataTrail(this);
                return trail;
            }
        }

        public DataTrail Update()
        {
            using (var db = new AppDbContext())
            {
                StandardSoaDataOutput soaDataOutput = StandardSoaDataOutput.Find(Id);
                if (soaDataOutput == null)
                {
                    throw new Exception(MessageBag.NoRecordFound);
                }

                DataTrail trail = new DataTrail(soaDataOutput, this);

                soaDataOutput.Type = Type;
                soaDataOutput.Code = Code;
                soaDataOutput.Name = Name;
                soaDataOutput.DataType = DataType;
                soaDataOutput.UpdatedAt = DateTime.Now;
                soaDataOutput.UpdatedById = UpdatedById ?? soaDataOutput.UpdatedById;

                db.Entry(soaDataOutput).State = EntityState.Modified;
                db.SaveChanges();

                return trail;
            }
        }

        public static DataTrail Delete(int id)
        {
            using (var db = new AppDbContext())
            {
                StandardSoaDataOutput soaDataOutput = db.StandardSoaDataOutputs.Where(q => q.Id == id).FirstOrDefault();
                if (soaDataOutput == null)
                {
                    throw new Exception(MessageBag.NoRecordFound);
                }

                DataTrail trail = new DataTrail(soaDataOutput, true);

                db.Entry(soaDataOutput).State = EntityState.Deleted;
                db.StandardSoaDataOutputs.Remove(soaDataOutput);
                db.SaveChanges();

                return trail;
            }
        }

        public static Result Result()
        {
            return new Result
            {
                Table = UtilAttribute.GetTableName(typeof(StandardSoaDataOutput)),
                Controller = ModuleBo.ModuleController.StandardSoaDataOutput.ToString()
            };
        }

        public static void SeedStandardOutput(User superUser)
        {
            TrailObject trail = new TrailObject();
            string table = UtilAttribute.GetTableName(typeof(StandardSoaDataOutput));

            for (int i = 1; i <= StandardSoaDataOutputBo.TypeMax; i++)
            {
                trail = new TrailObject();
                DataTrail dataTrail = null;

                StandardSoaDataOutput standardSoaDataOutput = StandardSoaDataOutput.Find(i);

                if (standardSoaDataOutput == null)
                {
                    standardSoaDataOutput = new StandardSoaDataOutput()
                    {
                        Id = i,
                        Type = i,
                        DataType = StandardSoaDataOutputBo.GetDataTypeByType(i),
                        Code = StandardSoaDataOutputBo.GetCodeByType(i),
                        Name = StandardSoaDataOutputBo.GetTypeByName(i),
                        CreatedById = superUser.Id,
                        UpdatedById = superUser.Id,
                    };

                    dataTrail = standardSoaDataOutput.Create();
                }
                else
                {
                    standardSoaDataOutput.DataType = StandardSoaDataOutputBo.GetDataTypeByType(i);
                    standardSoaDataOutput.Code = StandardSoaDataOutputBo.GetCodeByType(i);
                    standardSoaDataOutput.Name = StandardSoaDataOutputBo.GetTypeByName(i);

                    dataTrail = standardSoaDataOutput.Update();
                }

                if (dataTrail != null)
                    dataTrail.Merge(ref trail, table);

                if (trail.IsValid())
                {
                    UserTrailBo userTrailBo = new UserTrailBo(
                        standardSoaDataOutput.Id,
                        "Create/Update Standard SOA Data Output",
                        Result(),
                        trail,
                        standardSoaDataOutput.CreatedById
                    );

                    UserTrail userTrail = new UserTrail(userTrailBo);
                    userTrail.Create();
                }

            }
        }
    }
}
