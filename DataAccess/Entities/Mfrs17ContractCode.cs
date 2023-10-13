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
    [Table("Mfrs17ContractCodes")]
    public class Mfrs17ContractCode
    {
        [Key]
        public int Id { get; set; }

        [Index]
        public int CedingCompanyId { get; set; }
        [ExcludeTrail]
        public virtual Cedant CedingCompany { get; set; }

        [Index, MaxLength(255)]
        public string ModifiedContractCode { get; set; }

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

        public Mfrs17ContractCode()
        {
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
        }

        public static bool IsExists(int id)
        {
            using (var db = new AppDbContext())
            {
                return db.Mfrs17ContractCodes.Any(q => q.Id == id);
            }
        }

        public static Mfrs17ContractCode Find(int id)
        {
            using (var db = new AppDbContext())
            {
                return db.Mfrs17ContractCodes.Where(q => q.Id == id).FirstOrDefault();
            }
        }

        public static Mfrs17ContractCode FindByCedingCompanyAndModifiedContractCode(int cedingCompanyId, string code)
        {
            using (var db = new AppDbContext())
            {
                return db.Mfrs17ContractCodes.Where(q => q.CedingCompanyId == cedingCompanyId).Where(q => q.ModifiedContractCode.Trim() == code.Trim()).FirstOrDefault();
            }
        }

        public static int CountByCedingCompanyId(int cedingCompanyId)
        {
            using (var db = new AppDbContext())
            {
                var cedingCompany = Cedant.Find(cedingCompanyId);
                if (cedingCompany != null)
                    return db.Mfrs17ContractCodes.Where(q => q.CedingCompanyId == cedingCompany.Id).Count();
                return 0;
            }
        }

        public static IList<Mfrs17ContractCode> GetByCedingCompanyId(int id)
        {
            using (var db = new AppDbContext())
            {
                var cedingCompany = Cedant.Find(id);
                if (cedingCompany != null)
                    return db.Mfrs17ContractCodes.Where(q => q.CedingCompanyId == cedingCompany.Id).ToList();
                return null;
            }
        }

        public static IList<Mfrs17ContractCode> Get()
        {
            using (var db = new AppDbContext())
            {
               return db.Mfrs17ContractCodes.ToList();
            }
        }

        public DataTrail Create()
        {
            using (var db = new AppDbContext())
            {
                db.Mfrs17ContractCodes.Add(this);
                db.SaveChanges();

                DataTrail trail = new DataTrail(this);
                return trail;
            }
        }

        public DataTrail Update()
        {
            using (var db = new AppDbContext())
            {
                Mfrs17ContractCode mfrs17ContractCode = Mfrs17ContractCode.Find(Id);
                if (mfrs17ContractCode == null)
                {
                    throw new Exception(MessageBag.NoRecordFound);
                }

                DataTrail trail = new DataTrail(mfrs17ContractCode, this);

                mfrs17ContractCode.CedingCompanyId = CedingCompanyId;
                mfrs17ContractCode.ModifiedContractCode = ModifiedContractCode;
                mfrs17ContractCode.UpdatedAt = DateTime.Now;
                mfrs17ContractCode.UpdatedById = UpdatedById ?? mfrs17ContractCode.UpdatedById;

                db.Entry(mfrs17ContractCode).State = EntityState.Modified;
                db.SaveChanges();

                return trail;
            }
        }

        public static DataTrail Delete(int id)
        {
            using (var db = new AppDbContext())
            {
                Mfrs17ContractCode mfrs17ContractCode = db.Mfrs17ContractCodes.Where(q => q.Id == id).FirstOrDefault();
                if (mfrs17ContractCode == null)
                {
                    throw new Exception(MessageBag.NoRecordFound);
                }

                DataTrail trail = new DataTrail(mfrs17ContractCode, true);

                db.Entry(mfrs17ContractCode).State = EntityState.Deleted;
                db.Mfrs17ContractCodes.Remove(mfrs17ContractCode);
                db.SaveChanges();

                return trail;
            }
        }

        public override string ToString()
        {
            return string.Format("{0} - {1}", CedingCompany.Code, ModifiedContractCode);
        }
    }
}
