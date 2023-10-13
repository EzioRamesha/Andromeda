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

namespace DataAccess.Entities.Identity
{
    [Table("UserAccessGroups")]
    public class UserAccessGroup
    {
        [Key, Column(Order = 0), Required]
        public int UserId { get; set; }

        [Key, Column(Order = 1), Required]
        public int AccessGroupId { get; set; }

        [ExcludeTrail]
        [ForeignKey(nameof(UserId))]
        public virtual User User { get; set; }

        [ExcludeTrail]
        [ForeignKey(nameof(AccessGroupId))]
        public virtual AccessGroup AccessGroup { get; set; }

        public string PrimaryKey()
        {
            return string.Format("{0}|{1}", UserId, AccessGroupId);
        }

        public static UserAccessGroup Find(int userId, int accessGroupId)
        {
            using (var db = new AppDbContext())
            {
                return db.UserAccessGroups.Where(q => q.UserId == userId && q.AccessGroupId == accessGroupId).FirstOrDefault();
            }
        }

        public static IList<UserAccessGroup> GetByUserId(int userId)
        {
            using (var db = new AppDbContext())
            {
                return db.UserAccessGroups.Where(q => q.UserId == userId).ToList();
            }
        }

        public static int CountByAccessGroupId(int accessGroupId)
        {
            using (var db = new AppDbContext())
            {
                return db.UserAccessGroups.Where(q => q.AccessGroupId == accessGroupId).Count();
            }
        }

        public static DataTrail Delete(int userId, int accessGroupId)
        {
            using (var db = new AppDbContext())
            {
                UserAccessGroup uag = Find(userId, accessGroupId);
                if (uag == null)
                {
                    throw new Exception(MessageBag.NoRecordFound);
                }

                DataTrail trail = new DataTrail(uag, true);

                db.Entry(uag).State = EntityState.Deleted;
                db.UserAccessGroups.Remove(uag);
                db.SaveChanges();

                return trail;
            }
        }

        public static void DeleteAllByUserId(int userId, ref TrailObject trail)
        {
            using (var db = new AppDbContext())
            {
                var query = db.UserAccessGroups.Where(q => q.UserId == userId);
                foreach (UserAccessGroup aug in query.ToList())
                {
                    var dataTrail = new DataTrail(aug, true);
                    dataTrail.Merge(ref trail, UtilAttribute.GetTableName(typeof(UserAccessGroup)), aug.PrimaryKey());

                    db.Entry(aug).State = EntityState.Deleted;
                    db.UserAccessGroups.Remove(aug);
                }
                db.SaveChanges();
            }
        }
    }
}
