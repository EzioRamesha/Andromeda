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
    [Table("PickListDepartments")]
    public class PickListDepartment
    {
        [Key, Column(Order = 0)]
        public int PickListId { get; set; }
        [ExcludeTrail]
        public virtual PickList PickList { get; set; }

        [Key, Column(Order = 1)]
        public int DepartmentId { get; set; }
        [ExcludeTrail]
        public virtual Department Department { get; set; }

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

        public PickListDepartment()
        {
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
        }

        public string PrimaryKey()
        {
            return string.Format("{0}|{1}", PickListId, DepartmentId);
        }

        public static PickListDepartment Find(int pickListId, int departmentId)
        {
            if (pickListId == 0 || departmentId == 0)
                return null;

            using (var db = new AppDbContext())
            {
                return db.PickListDepartments
                    .Where(q => q.PickListId == pickListId && q.DepartmentId == departmentId)
                    .FirstOrDefault();
            }
        }

        public static List<PickListDepartment> GetByPickListId(int pickListId)
        {
            if (pickListId == 0)
                return null;

            using (var db = new AppDbContext())
            {
                return db.PickListDepartments
                    .Where(q => q.PickListId == pickListId).ToList();
            }
        }

        public DataTrail Create()
        {
            using (var db = new AppDbContext(false))
            {
                DataTrail trail = new DataTrail(this);

                db.PickListDepartments.Add(this);
                db.SaveChanges();

                return trail;
            }
        }

        public void Create(AppDbContext db)
        {
            db.PickListDepartments.Add(this);
        }

        public static IList<DataTrail> DeleteAllByPickListId(int pickListId)
        {
            using (var db = new AppDbContext(false))
            {
                List<DataTrail> trails = new List<DataTrail>();

                db.Database.ExecuteSqlCommand("DELETE FROM [PickListDepartments] WHERE [PickListId] = {0}", pickListId);
                db.SaveChanges();

                return trails;
            }
        }

        public static void DeleteAllByPickListId(int pickListId, ref TrailObject trail)
        {
            using (var db = new AppDbContext())
            {
                var query = db.PickListDepartments.Where(q => q.PickListId == pickListId);

                foreach (PickListDepartment pickListDepartment in query.ToList())
                {
                    var dataTrail = new DataTrail(pickListDepartment, true);
                    dataTrail.Merge(ref trail, UtilAttribute.GetTableName(typeof(PickListDepartment)), pickListDepartment.PrimaryKey());

                    db.Entry(pickListDepartment).State = EntityState.Deleted;
                    db.PickListDepartments.Remove(pickListDepartment);
                }

                db.SaveChanges();
            }
        }

        public static IList<DataTrail> DeleteByPickListIdAndDepartmentId(int pickListId, int departmentId)
        {
            using (var db = new AppDbContext(false))
            {
                List<DataTrail> trails = new List<DataTrail>();

                db.Database.ExecuteSqlCommand("DELETE FROM [PickListDepartments] WHERE [PickListId] = {0} AND [DepartmentID] = {1}", pickListId, departmentId);
                db.SaveChanges();

                return trails;
            }
        }

        public static void DeleteByPickListIdAndDepartmentId(int pickListId, int departmentId, ref TrailObject trail)
        {
            using (var db = new AppDbContext())
            {
                var query = db.PickListDepartments.Where(q => q.PickListId == pickListId && q.DepartmentId == departmentId).FirstOrDefault();

                var dataTrail = new DataTrail(query, true);
                dataTrail.Merge(ref trail, UtilAttribute.GetTableName(typeof(PickListDepartment)), query.PrimaryKey());

                db.Entry(query).State = EntityState.Deleted;
                db.PickListDepartments.Remove(query);

                db.SaveChanges();
            }
        }
    }
}
