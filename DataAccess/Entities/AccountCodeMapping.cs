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
    [Table("AccountCodeMappings")]
    public class AccountCodeMapping
    {
        [Key]
        public int Id { get; set; }

        [Index]
        public int ReportType { get; set; }

        [Index]
        public int? Type { get; set; }

        public string TreatyType { get; set; }

        public string TreatyNumber { get; set; }

        [Index]
        public int? TreatyCodeId { get; set; }

        [ExcludeTrail]
        public virtual TreatyCode TreatyCode { get; set; }

        public string ClaimCode { get; set; }

        public string BusinessOrigin { get; set; }

        [Required, Index]
        public int AccountCodeId { get; set; }

        [ExcludeTrail]
        public virtual AccountCode AccountCode { get; set; }

        [Index]
        public int? DebitCreditIndicatorPositive { get; set; }

        [Index]
        public int? DebitCreditIndicatorNegative { get; set; }

        [Index]
        public int? TransactionTypeCodePickListDetailId { get; set; }

        [ExcludeTrail]
        public virtual PickListDetail TransactionTypeCodePickListDetail { get; set; }

        [Index]
        public int? RetroRegisterFieldPickListDetailId { get; set; }

        [ExcludeTrail]
        public virtual PickListDetail RetroRegisterFieldPickListDetail { get; set; }

        [Index]
        public int? ModifiedContractCodeId { get; set; }
        [ExcludeTrail]
        public virtual Mfrs17ContractCode ModifiedContractCode { get; set; }

        [Index]
        public bool IsBalanceSheet { get; set; } = false;

        public string InvoiceField { get; set; }

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

        [ExcludeTrail]
        public virtual ICollection<AccountCodeMappingDetail> AccountCodeMappingDetails { get; set; }

        public AccountCodeMapping()
        {
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
        }

        public static bool IsExists(int id)
        {
            using (var db = new AppDbContext())
            {
                return db.AccountCodeMappings.Any(q => q.Id == id);
            }
        }

        public static AccountCodeMapping Find(int id)
        {
            using (var db = new AppDbContext())
            {
                return db.AccountCodeMappings.Where(q => q.Id == id).FirstOrDefault();
            }
        }

        public static int CountByAccountCodeId(int accountCodeId)
        {
            using (var db = new AppDbContext())
            {
                return db.AccountCodeMappings.Where(q => q.AccountCodeId == accountCodeId).Count();
            }
        }

        public static IList<AccountCodeMapping> Get()
        {
            using (var db = new AppDbContext())
            {
                return db.AccountCodeMappings.OrderBy(q => q.AccountCode.Code).ToList();
            }
        }

        public DataTrail Create()
        {
            using (var db = new AppDbContext())
            {
                db.AccountCodeMappings.Add(this);
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

                entity.ReportType = ReportType;
                entity.Type = Type;
                entity.TreatyType = TreatyType;
                entity.TreatyNumber = TreatyNumber;
                entity.TreatyCodeId = TreatyCodeId;
                entity.ClaimCode = ClaimCode;
                entity.BusinessOrigin = BusinessOrigin;
                entity.AccountCodeId = AccountCodeId;
                entity.DebitCreditIndicatorPositive = DebitCreditIndicatorPositive;
                entity.DebitCreditIndicatorNegative = DebitCreditIndicatorNegative;
                entity.TransactionTypeCodePickListDetailId = TransactionTypeCodePickListDetailId;
                entity.RetroRegisterFieldPickListDetailId = RetroRegisterFieldPickListDetailId;
                entity.ModifiedContractCodeId = ModifiedContractCodeId;
                entity.InvoiceField = InvoiceField;
                entity.IsBalanceSheet = IsBalanceSheet;

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
                db.AccountCodeMappings.Remove(entity);
                db.SaveChanges();

                return trail;
            }
        }
    }
}
