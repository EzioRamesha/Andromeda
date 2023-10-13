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
    [Table("Documents")]
    public class Document
    {
        [Key]
        public int Id { get; set; }

        [Required, Index]
        public int ModuleId { get; set; }

        [ExcludeTrail]
        [ForeignKey(nameof(ModuleId))]
        public virtual Module Module { get; set; }

        [Required, Index]
        public int ObjectId { get; set; }

        public string SubModuleController { get; set; }

        [Index]
        public int? SubObjectId { get; set; }

        public int? RemarkId { get; set; }

        [ExcludeTrail]
        [ForeignKey(nameof(RemarkId))]
        public virtual Remark Remark { get; set; }

        public int Type { get; set; }

        [MaxLength(255), Index]
        public string FileName { get; set; }

        [MaxLength(255), Index]
        public string HashFileName { get; set; }

        [MaxLength(255)]
        public string Description { get; set; }

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

        public Document()
        {
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
        }

        public static bool IsExists(int id)
        {
            using (var db = new AppDbContext())
            {
                return db.Documents.Any(q => q.Id == id);
            }
        }

        public static Document Find(int id)
        {
            using (var db = new AppDbContext())
            {
                return db.Documents.Where(q => q.Id == id).FirstOrDefault();
            }
        }

        public static IList<Document> GetByModuleIdObjectId(int moduleId, int objectId)
        {
            using (var db = new AppDbContext())
            {
                return db.Documents.Where(q => q.ModuleId == moduleId && q.ObjectId == objectId).ToList();
            }
        }

        public static IList<Document> GetByModuleIdObjectIdExcept(int moduleId, int objectId, List<int> ids)
        {
            using (var db = new AppDbContext())
            {
                return db.Documents.Where(q => q.ModuleId == moduleId && q.ObjectId == objectId && !ids.Contains(q.Id)).ToList();
            }
        }

        public DataTrail Create()
        {
            using (var db = new AppDbContext())
            {
                db.Documents.Add(this);
                db.SaveChanges();

                DataTrail trail = new DataTrail(this);
                return trail;
            }
        }

        public DataTrail Update()
        {
            using (var db = new AppDbContext())
            {
                Document document = Document.Find(Id);
                if (document == null)
                {
                    throw new Exception(MessageBag.NoRecordFound);
                }

                DataTrail trail = new DataTrail(document, this);

                document.ModuleId = ModuleId;
                document.ObjectId = ObjectId;
                document.RemarkId = RemarkId;
                document.Type = Type;
                document.FileName = FileName;
                document.HashFileName = HashFileName;
                document.Description = Description;
                document.UpdatedAt = DateTime.Now;
                document.UpdatedById = UpdatedById ?? document.UpdatedById;

                db.Entry(document).State = EntityState.Modified;
                db.SaveChanges();

                return trail;
            }
        }

        public static DataTrail Delete(int id)
        {
            using (var db = new AppDbContext())
            {
                Document document = Document.Find(id);
                if (document == null)
                {
                    throw new Exception(MessageBag.NoRecordFound);
                }

                DataTrail trail = new DataTrail(document, true);

                db.Entry(document).State = EntityState.Deleted;
                db.Documents.Remove(document);
                db.SaveChanges();

                return trail;
            }
        }
    }
}
