using BusinessObject;
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
    [Table("AccountCodeMappingDetails")]
    public class AccountCodeMappingDetail
    {
        [Key]
        public int Id { get; set; }

        [Required, Index]
        public int AccountCodeMappingId { get; set; }

        [ForeignKey(nameof(AccountCodeMappingId))]
        [ExcludeTrail]
        public virtual AccountCodeMapping AccountCodeMapping { get; set; }

        [MaxLength(20), Index]
        public string TreatyType { get; set; }

        [MaxLength(30), Index]
        public string ClaimCode { get; set; }

        [MaxLength(30), Index]
        public string BusinessOrigin { get; set; }

        [MaxLength(50), Index]
        public string InvoiceField { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime CreatedAt { get; set; }

        [Required]
        public int CreatedById { get; set; }

        [ForeignKey(nameof(CreatedById))]
        [ExcludeTrail]
        public virtual User CreatedBy { get; set; }

        public AccountCodeMappingDetail()
        {
            CreatedAt = DateTime.Now;
        }

        public static bool IsExists(int id)
        {
            using (var db = new AppDbContext())
            {
                return db.AccountCodeMappingDetails.Any(q => q.Id == id);
            }
        }

        public static bool IsDuplicate(
            int reportType,
            int type,
            bool isBalanceSheet = false,
            string treatyType = null,
            int? treatyCodeId = null,
            string claimCode = null,
            string businessOrigin = null,
            int? transactionTypeCodeId = null,
            int? retroRegisterFieldId = null,
            int? modifiedContractCode = null,
            string invoiceField = null,
            int? accountCodeMappingId = null
        )
        {
            using (var db = new AppDbContext())
            {
                var query = db.AccountCodeMappingDetails
                    .Where(q => q.AccountCodeMapping.ReportType == reportType)
                          .Where(q => q.AccountCodeMapping.Type == type)
                          .Where(q => q.AccountCodeMapping.IsBalanceSheet == isBalanceSheet);


                if (!string.IsNullOrEmpty(treatyType))
                {
                    query = query.Where(q => (!string.IsNullOrEmpty(q.TreatyType) && q.TreatyType == treatyType) || string.IsNullOrEmpty(q.TreatyType));
                }
                else
                {
                    query = query.Where(q => string.IsNullOrEmpty(q.TreatyType));
                }

                if (!string.IsNullOrEmpty(claimCode))
                {
                    query = query.Where(q => (!string.IsNullOrEmpty(q.ClaimCode) && q.ClaimCode == claimCode) || string.IsNullOrEmpty(q.ClaimCode));
                }
                else
                {
                    query = query.Where(q => string.IsNullOrEmpty(q.ClaimCode));
                }

                if (!string.IsNullOrEmpty(businessOrigin))
                {
                    query = query.Where(q => (!string.IsNullOrEmpty(q.BusinessOrigin) && q.BusinessOrigin == businessOrigin) || string.IsNullOrEmpty(q.BusinessOrigin));
                    var test = query.ToList();
                }
                else
                {
                    query = query.Where(q => string.IsNullOrEmpty(q.BusinessOrigin));
                }

                if (!string.IsNullOrEmpty(invoiceField))
                {
                    query = query.Where(q => (!string.IsNullOrEmpty(q.InvoiceField) && q.InvoiceField == invoiceField) || string.IsNullOrEmpty(q.InvoiceField));
                }
                else
                {
                    query = query.Where(q => string.IsNullOrEmpty(q.InvoiceField));
                }

                if (treatyCodeId.HasValue)
                {
                    query = query.Where(q =>
                    (q.AccountCodeMapping.TreatyCodeId.HasValue && q.AccountCodeMapping.TreatyCodeId == treatyCodeId) ||
                    !q.AccountCodeMapping.TreatyCodeId.HasValue);
                }
                else
                {
                    query = query.Where(q => !q.AccountCodeMapping.TreatyCodeId.HasValue);
                }

                if (retroRegisterFieldId.HasValue)
                {
                    query = query.Where(q =>
                    (q.AccountCodeMapping.RetroRegisterFieldPickListDetailId.HasValue && q.AccountCodeMapping.RetroRegisterFieldPickListDetailId == retroRegisterFieldId) ||
                    !q.AccountCodeMapping.RetroRegisterFieldPickListDetailId.HasValue);
                }
                else
                {
                    query = query.Where(q => !q.AccountCodeMapping.RetroRegisterFieldPickListDetailId.HasValue);
                }

                if (transactionTypeCodeId.HasValue)
                {
                    query = query.Where(q =>
                    (q.AccountCodeMapping.TransactionTypeCodePickListDetailId.HasValue && q.AccountCodeMapping.TransactionTypeCodePickListDetailId == transactionTypeCodeId) ||
                    !q.AccountCodeMapping.TransactionTypeCodePickListDetailId.HasValue);
                }
                else
                {
                    query = query.Where(q => !q.AccountCodeMapping.TransactionTypeCodePickListDetailId.HasValue);
                }

                if (modifiedContractCode.HasValue)
                {
                    query = query.Where(q =>
                    (q.AccountCodeMapping.ModifiedContractCodeId.HasValue && q.AccountCodeMapping.ModifiedContractCodeId == modifiedContractCode) ||
                    !q.AccountCodeMapping.ModifiedContractCodeId.HasValue);
                }
                else
                {
                    query = query.Where(q => !q.AccountCodeMapping.ModifiedContractCodeId.HasValue);
                }

                if (accountCodeMappingId != null)
                {
                    query = query.Where(q => q.AccountCodeMappingId != accountCodeMappingId);
                    var test = query.ToList();
                }


                if (type == AccountCodeMappingBo.TypeClaimProvision || type == AccountCodeMappingBo.TypeClaimRecovery || type == AccountCodeMappingBo.TypeCedantAccountCode)
                {
                    var balanceSheet = db.AccountCodeMappingDetails
                        .Where(q => q.AccountCodeMapping.ReportType == reportType)
                        .Where(q => q.AccountCodeMapping.Type == type)
                        .Where(q => q.AccountCodeMapping.TreatyType == null)
                        .Where(q => !q.AccountCodeMapping.TreatyCodeId.HasValue)
                        .Where(q => q.AccountCodeMapping.ClaimCode == null)
                        .Where(q => q.AccountCodeMapping.BusinessOrigin == null)
                        .Where(q => !q.AccountCodeMapping.TransactionTypeCodePickListDetailId.HasValue)
                        .Where(q => !q.AccountCodeMapping.ModifiedContractCodeId.HasValue)
                        .Where(q => q.AccountCodeMapping.InvoiceField == null)
                        .FirstOrDefault();

                    if (balanceSheet != null)
                    {
                        query = query.Where(q => q.AccountCodeMappingId != balanceSheet.Id);
                    }
                }

                var test2 = query.ToList();
                return query.Any();
            }
        }

        public static AccountCodeMappingDetail Find(int id)
        {
            using (var db = new AppDbContext())
            {
                return db.AccountCodeMappingDetails.Where(q => q.Id == id).FirstOrDefault();
            }
        }

        public DataTrail Create()
        {
            using (var db = new AppDbContext())
            {
                db.AccountCodeMappingDetails.Add(this);
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

                entity.AccountCodeMappingId = AccountCodeMappingId;
                entity.TreatyType = TreatyType;
                entity.ClaimCode = ClaimCode;
                entity.BusinessOrigin = BusinessOrigin;
                entity.InvoiceField = InvoiceField;

                db.Entry(entity).State = EntityState.Modified;
                db.SaveChanges();

                return trail;
            }
        }

        public static DataTrail Delete(int id)
        {
            using (var db = new AppDbContext())
            {
                var entity = db.AccountCodeMappingDetails.Where(q => q.Id == id).FirstOrDefault();
                if (entity == null)
                {
                    throw new Exception(MessageBag.NoRecordFound);
                }

                var trail = new DataTrail(entity, true);

                db.Entry(entity).State = EntityState.Deleted;
                db.AccountCodeMappingDetails.Remove(entity);
                db.SaveChanges();

                return trail;
            }
        }

        public static IList<DataTrail> DeleteByAccountCodeMappingId(int accountCodeMappingId)
        {
            using (var db = new AppDbContext())
            {
                var query = db.AccountCodeMappingDetails.Where(q => q.AccountCodeMappingId == accountCodeMappingId);

                var trails = new List<DataTrail>();
                foreach (AccountCodeMappingDetail accountCodeMappingDetail in query.ToList())
                {
                    var trail = new DataTrail(accountCodeMappingDetail, true);
                    trails.Add(trail);

                    db.Entry(accountCodeMappingDetail).State = EntityState.Deleted;
                    db.AccountCodeMappingDetails.Remove(accountCodeMappingDetail);
                }

                db.SaveChanges();
                return trails;
            }
        }
    }
}
