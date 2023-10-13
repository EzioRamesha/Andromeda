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
    [Table("DirectRetroConfigurationMappings")]
    public class DirectRetroConfigurationMapping
    {
        [Key]
        public int Id { get; set; }

        [Required, Index]
        public int DirectRetroConfigurationId { get; set; }

        [ExcludeTrail]
        public virtual DirectRetroConfiguration DirectRetroConfiguration { get; set; }

        [MaxLength(255), Index]
        public string Combination { get; set; }

        [MaxLength(50), Index]
        public string RetroParty { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime CreatedAt { get; set; }

        [Required]
        public int CreatedById { get; set; }

        [ExcludeTrail]
        public virtual User CreatedBy { get; set; }

        public DirectRetroConfigurationMapping()
        {
            CreatedAt = DateTime.Now;
        }

        public static bool IsExists(int id)
        {
            using (var db = new AppDbContext())
            {
                return db.DirectRetroConfigurationMappings.Any(q => q.Id == id);
            }
        }

        public static IQueryable<DirectRetroConfigurationMapping> QueryByCombination(
            AppDbContext db,
            string combination,
            int? directRetroConfigurationId = null
        )
        {
            var query = db.DirectRetroConfigurationMappings
                .Where(q => string.Equals(q.Combination, combination));

            if (directRetroConfigurationId != null)
            {
                query = query.Where(q => q.DirectRetroConfigurationId != directRetroConfigurationId);
            }

            return query;
        }

        public static IQueryable<DirectRetroConfigurationMapping> QueryByParams(
            AppDbContext db,
            string retroParty,
            string treatyCode,
            bool groupById = false
        )
        {
            var query = db.DirectRetroConfigurationMappings
                          .Where(q => q.RetroParty == retroParty)
                          .Where(q => q.DirectRetroConfiguration.TreatyCode.Code == treatyCode);

            // NOTE: Group by should put at the end of query
            if (groupById)
            {
                query = query.GroupBy(q => q.DirectRetroConfigurationId).Select(q => q.FirstOrDefault());
            }

            return query;
        }

        public static DirectRetroConfigurationMapping Find(int id)
        {
            using (var db = new AppDbContext())
            {
                return db.DirectRetroConfigurationMappings.Where(q => q.Id == id).FirstOrDefault();
            }
        }

        public static DirectRetroConfigurationMapping FindByCombination(
            string combination,
            int? directRetroConfigurationId = null
        )
        {
            using (var db = new AppDbContext())
            {
                return QueryByCombination(
                    db,
                    combination,
                    directRetroConfigurationId
                ).FirstOrDefault();
            }
        }

        public static DirectRetroConfigurationMapping FindByParams(
            string retroParty,
            string treatyCode,
            bool groupById = false
        )
        {
            using (var db = new AppDbContext())
            {
                return QueryByParams(
                    db,
                    retroParty,
                    treatyCode,
                    groupById
                ).FirstOrDefault();
            }
        }

        public static int CountByCombination(
            string combination,
            int? directRetroConfigurationId = null
        )
        {
            using (var db = new AppDbContext())
            {
                return QueryByCombination(
                    db,
                    combination,
                    directRetroConfigurationId
                ).Count();
            }
        }

        public static int CountByParams(
            string retroParty,
            string treatyCode,
            bool groupById = false
        )
        {
            using (var db = new AppDbContext())
            {
                return QueryByParams(
                    db,
                    retroParty,
                    treatyCode,
                    groupById
                ).Count();
            }
        }

        public static IList<DirectRetroConfigurationMapping> GetByParams(
            string retroParty,
            string treatyCode,
            bool groupById = false
        )
        {
            using (var db = new AppDbContext())
            {
                return QueryByParams(
                    db,
                    retroParty,
                    treatyCode,
                    groupById
                ).ToList();
            }
        }

        public DataTrail Create()
        {
            using (var db = new AppDbContext())
            {
                db.DirectRetroConfigurationMappings.Add(this);
                db.SaveChanges();

                var trail = new DataTrail(this);
                return trail;
            }
        }

        public DataTrail Update()
        {
            using (var db = new AppDbContext())
            {
                var entity = Find(Id);
                if (entity == null)
                {
                    throw new Exception(MessageBag.NoRecordFound);
                }

                var trail = new DataTrail(entity, this);

                entity.DirectRetroConfigurationId = DirectRetroConfigurationId;
                entity.Combination = Combination;
                entity.RetroParty = RetroParty;

                db.Entry(entity).State = EntityState.Modified;
                db.SaveChanges();

                return trail;
            }
        }

        public static DataTrail Delete(int id)
        {
            using (var db = new AppDbContext())
            {
                var entity = db.DirectRetroConfigurationMappings.Where(q => q.Id == id).FirstOrDefault();
                if (entity == null)
                {
                    throw new Exception(MessageBag.NoRecordFound);
                }

                var trail = new DataTrail(entity, true);

                db.Entry(entity).State = EntityState.Deleted;
                db.DirectRetroConfigurationMappings.Remove(entity);
                db.SaveChanges();

                return trail;
            }
        }

        public static IList<DataTrail> DeleteByDirectRetroConfigurationId(int directRetroConfigurationId)
        {
            using (var db = new AppDbContext())
            {
                var query = db.DirectRetroConfigurationMappings.Where(q => q.DirectRetroConfigurationId == directRetroConfigurationId);

                var trails = new List<DataTrail>();
                foreach (DirectRetroConfigurationMapping directRetroConfigurationMapping in query.ToList())
                {
                    var trail = new DataTrail(directRetroConfigurationMapping, true);
                    trails.Add(trail);

                    db.Entry(directRetroConfigurationMapping).State = EntityState.Deleted;
                    db.DirectRetroConfigurationMappings.Remove(directRetroConfigurationMapping);
                }

                db.SaveChanges();
                return trails;
            }
        }
    }
}
