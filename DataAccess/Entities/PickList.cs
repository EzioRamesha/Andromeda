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
    [Table("PickLists")]
    public class PickList
    {
        [Key]
        public int Id { get; set; }

        [Required, Index]
        public int DepartmentId { get; set; }

        [ExcludeTrail]
        [ForeignKey(nameof(DepartmentId))]
        public virtual Department Department { get; set; }

        [Index]
        public int? StandardOutputId { get; set; }

        [ExcludeTrail]
        [ForeignKey(nameof(StandardOutputId))]
        public virtual StandardOutput StandardOutput { get; set; }

        [Index]
        public int? StandardClaimDataOutputId { get; set; }

        [ExcludeTrail]
        [ForeignKey(nameof(StandardClaimDataOutputId))]
        public virtual StandardClaimDataOutput StandardClaimDataOutput { get; set; }

        [Index]
        public int? StandardSoaDataOutputId { get; set; }

        [ExcludeTrail]
        [ForeignKey(nameof(StandardSoaDataOutputId))]
        public virtual StandardSoaDataOutput StandardSoaDataOutput { get; set; }

        [Required, MaxLength(128)]
        public string FieldName { get; set; }

        public bool IsEditable { get; set; } = true;

        [Column(TypeName = "datetime2")]
        public DateTime CreatedAt { get; set; }
        [Required]
        public int CreatedById { get; set; }
        [ForeignKey(nameof(CreatedById))]
        [ExcludeTrail]
        public virtual User CreatedBy { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime UpdatedAt { get; set; }
        public int? UpdatedById { get; set; }
        [ForeignKey(nameof(UpdatedById))]
        [ExcludeTrail]
        public virtual User UpdatedBy { get; set; }

        [ExcludeTrail]
        public virtual ICollection<PickListDepartment> PickListDepartments { get; set; }

        public PickList()
        {
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
        }

        public PickList(PickListBo pickListBo) : this()
        {
            Id = pickListBo.Id;
            DepartmentId = pickListBo.DepartmentId;
            FieldName = pickListBo.FieldName;
            StandardOutputId = pickListBo.StandardOutputId;
            StandardClaimDataOutputId = pickListBo.StandardClaimDataOutputId;
            StandardSoaDataOutputId = pickListBo.StandardSoaDataOutputId;
            IsEditable = pickListBo.IsEditable;
            CreatedById = pickListBo.CreatedById;
            UpdatedById = pickListBo.UpdatedById;
        }

        public static Result Result()
        {
            return new Result
            {
                Table = UtilAttribute.GetTableName(typeof(PickList)),
                Controller = ModuleBo.ModuleController.PickList.ToString()
            };
        }

        public static bool IsExists(int id)
        {
            using (var db = new AppDbContext())
            {
                return db.PickLists.Any(q => q.Id == id);
            }
        }

        public static PickList Find(int id)
        {
            using (var db = new AppDbContext())
            {
                db.Database.CommandTimeout = 0;
                EFExecutionStrategy connectionStrategy = EFExecutionStrategy.GetNewInstance("PickList");
                return connectionStrategy.Execute(() => db.PickLists.Where(q => q.Id == id).FirstOrDefault());
            }
        }

        public DataTrail Create()
        {
            using (var db = new AppDbContext())
            {
                db.PickLists.Add(this);
                db.SaveChanges();

                return new DataTrail(this);
            }
        }

        public DataTrail Update()
        {
            using (var db = new AppDbContext())
            {
                var pickList = Find(Id);
                if (pickList == null)
                {
                    throw new Exception(MessageBag.NoRecordFound);
                }

                var trail = new DataTrail(pickList, this);

                pickList.DepartmentId = DepartmentId;
                pickList.StandardOutputId = StandardOutputId;
                pickList.StandardClaimDataOutputId = StandardClaimDataOutputId;
                pickList.FieldName = FieldName;
                pickList.IsEditable = IsEditable;
                pickList.UpdatedAt = DateTime.Now;
                pickList.UpdatedById = UpdatedById ?? pickList.UpdatedById;

                db.Entry(pickList).State = EntityState.Modified;
                db.SaveChanges();

                return trail;
            }
        }

        public static DataTrail Delete(int id)
        {
            using (var db = new AppDbContext())
            {
                var pickList = Find(id);
                if (pickList == null)
                {
                    throw new Exception(MessageBag.NoRecordFound);
                }

                var trail = new DataTrail(pickList, true);

                db.Entry(pickList).State = EntityState.Deleted;
                db.PickLists.Remove(pickList);
                db.SaveChanges();

                return trail;
            }
        }

        public static void SeedPickList(User superUser)
        {
            var trail = new TrailObject();
            string table = UtilAttribute.GetTableName(typeof(Module));

            foreach (var pickListBo in PickListBo.GetPickLists())
            {
                trail = new TrailObject();
                DataTrail dataTrail = null;

                var pickList = Find(pickListBo.Id);
                if (pickList == null)
                {
                    pickList = new PickList(pickListBo)
                    {
                        CreatedById = superUser.Id,
                        UpdatedById = superUser.Id
                    };
                    dataTrail = pickList.Create();
                }
                else
                {
                    pickList.DepartmentId = pickListBo.DepartmentId;
                    pickList.StandardOutputId = pickListBo.StandardOutputId;
                    pickList.StandardClaimDataOutputId = pickListBo.StandardClaimDataOutputId;
                    pickList.StandardSoaDataOutputId = pickListBo.StandardSoaDataOutputId;
                    pickList.IsEditable = pickListBo.IsEditable;

                    pickList.UpdatedById = superUser.Id;
                    dataTrail = pickList.Update();
                }

                if (dataTrail != null)
                    dataTrail.Merge(ref trail, table);

                var pickListDepartments = PickListDepartment.GetByPickListId(pickList.Id);

                foreach (var department in pickListBo.UsedByDepartments.OrderBy(q => q))
                {
                    if (pickListDepartments.Count != 0)
                    {
                        if (!pickListDepartments.Select(q => q.DepartmentId).Contains(department))
                        {
                            foreach (var i in pickListDepartments.Where(q => q.DepartmentId != department).ToList())
                            {
                                PickListDepartment.DeleteByPickListIdAndDepartmentId(pickList.Id, i.DepartmentId);
                            }

                        }
                    }

                    var pickListDepartment = PickListDepartment.Find(pickList.Id, department);

                    if (pickListDepartment == null)
                    {
                        var pickListDepartmentBo = new PickListDepartment()
                        {
                            PickListId = pickList.Id,
                            DepartmentId = department,
                            CreatedById = superUser.Id,
                            UpdatedById = superUser.Id
                        };
                        pickListDepartmentBo.Create();
                    }
                }

                if (trail.IsValid())
                {
                    var userTrailBo = new UserTrailBo(
                        pickList.Id,
                        "Create/Update Pick List",
                        Result(),
                        trail,
                        pickList.CreatedById
                    );

                    var userTrail = new UserTrail(userTrailBo);
                    userTrail.Create();
                }
            }
        }
    }
}
