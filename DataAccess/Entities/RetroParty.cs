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
    [Table("RetroParties")]
    public class RetroParty
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(50), Index]
        public string Party { get; set; }

        [Required, MaxLength(50), Index]
        public string Code { get; set; }

        [Required, MaxLength(255)]
        public string Name { get; set; }

        [Required, Index]
        public int CountryCodePickListDetailId { get; set; }

        [ExcludeTrail]
        public virtual PickListDetail CountryCodePickListDetail { get; set; }

        [MaxLength(255)]
        public string Description { get; set; }

        [Required, Index]
        public int Status { get; set; }

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

        [Index]
        public bool IsDirectRetro { get; set; } = false;

        [Index]
        public bool IsPerLifeRetro { get; set; } = false;

        [MaxLength(10), Index]
        public string AccountCode { get; set; }

        [MaxLength(255)]
        public string AccountCodeDescription { get; set; }

        public RetroParty()
        {
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
        }

        public static bool IsExists(int id)
        {
            using (var db = new AppDbContext())
            {
                return db.RetroParties.Any(q => q.Id == id);
            }
        }

        public bool IsDuplicateParty()
        {
            using (var db = new AppDbContext())
            {
                var query = db.RetroParties.Where(q => q.Party.Trim().Equals(Party.Trim(), StringComparison.OrdinalIgnoreCase));
                if (Id != 0)
                {
                    query = query.Where(q => q.Id != Id);
                }
                return query.Count() > 0;
            }
        }

        public static RetroParty Find(int? id)
        {
            using (var db = new AppDbContext())
            {
                return db.RetroParties.Where(q => q.Id == id).FirstOrDefault();
            }
        }

        public static RetroParty FindByCode(string code)
        {
            using (var db = new AppDbContext())
            {
                return db.RetroParties.Where(q => q.Code.Trim() == code.Trim()).FirstOrDefault();
            }
        }

        public static int CountByCountryCodePickListDetailId(int countryCodePickListDetailId)
        {
            using (var db = new AppDbContext())
            {
                return db.RetroParties.Where(q => q.CountryCodePickListDetailId == countryCodePickListDetailId).Count();
            }
        }

        public static IList<RetroParty> Get()
        {
            using (var db = new AppDbContext())
            {
                return db.RetroParties.OrderBy(q => q.Party).ToList();
            }
        }

        public static IList<RetroParty> GetByStatus(int? status = null, int? selectedId = null)
        {
            using (var db = new AppDbContext())
            {
                var query = db.RetroParties.AsQueryable();

                if (status != null)
                {
                    if (selectedId != null)
                        query = query.Where(q => q.Status == status || q.Id == selectedId);
                    else
                        query = query.Where(q => q.Status == status);
                }

                return query.OrderBy(q => q.Party).ToList();
            }
        }

        public DataTrail Create()
        {
            using (var db = new AppDbContext())
            {
                db.RetroParties.Add(this);
                db.SaveChanges();

                DataTrail trail = new DataTrail(this);
                return trail;
            }
        }

        public DataTrail Update()
        {
            using (var db = new AppDbContext())
            {
                RetroParty retroParties = Find(Id);
                if (retroParties == null)
                {
                    throw new Exception(MessageBag.NoRecordFound);
                }

                DataTrail trail = new DataTrail(retroParties, this);

                retroParties.Party = Party;
                retroParties.Code = Code;
                retroParties.Name = Name;
                retroParties.CountryCodePickListDetailId = CountryCodePickListDetailId;
                retroParties.Description = Description;
                retroParties.Status = Status;
                retroParties.IsDirectRetro = IsDirectRetro;
                retroParties.IsPerLifeRetro = IsPerLifeRetro;
                retroParties.AccountCode = AccountCode;
                retroParties.AccountCodeDescription = AccountCodeDescription;
                retroParties.UpdatedAt = DateTime.Now;
                retroParties.UpdatedById = UpdatedById ?? retroParties.UpdatedById;

                db.Entry(retroParties).State = EntityState.Modified;
                db.SaveChanges();

                return trail;
            }
        }

        public static DataTrail Delete(int id)
        {
            using (var db = new AppDbContext())
            {
                RetroParty retroParties = db.RetroParties.Where(q => q.Id == id).FirstOrDefault();
                if (retroParties == null)
                {
                    throw new Exception(MessageBag.NoRecordFound);
                }

                DataTrail trail = new DataTrail(retroParties, true);

                db.Entry(retroParties).State = EntityState.Deleted;
                db.RetroParties.Remove(retroParties);
                db.SaveChanges();

                return trail;
            }
        }

        public override string ToString()
        {
            if (string.IsNullOrEmpty(Name))
            {
                return Party;
            }
            return string.Format("{0} - {1}", Party, Name);
        }
    }
}
