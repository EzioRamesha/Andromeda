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

namespace DataAccess.Entities.TreatyPricing
{
    [Table("TreatyPricingGroupReferralVersions")]
    public class TreatyPricingGroupReferralVersion
    {
        [Key]
        public int Id { get; set; }

        [Required, Index]
        public int TreatyPricingGroupReferralId { get; set; }
        [ExcludeTrail]
        public virtual TreatyPricingGroupReferral TreatyPricingGroupReferral { get; set; }

        [Required, Index]
        public int Version { get; set; }

        [Index]
        public int? GroupReferralPersonInChargeId { get; set; }
        [ExcludeTrail]
        public virtual User GroupReferralPIC { get; set; }

        [MaxLength(128)]
        public string CedantPersonInCharge { get; set; }

        public int? RequestTypePickListDetailId { get; set; }
        [ExcludeTrail]
        public virtual PickListDetail RequestTypePickListDetail { get; set; }

        public int? PremiumTypePickListDetailId { get; set; }
        [ExcludeTrail]
        public virtual PickListDetail PremiumTypePickListDetail { get; set; }

        public double? GrossRiskPremium { get; set; }

        public double? ReinsurancePremium { get; set; }

        public double? GrossRiskPremiumGTL { get; set; }

        public double? ReinsurancePremiumGTL { get; set; }

        public double? GrossRiskPremiumGHS { get; set; }

        public double? ReinsurancePremiumGHS { get; set; }

        public double? AverageSumAssured { get; set; }
        
        public double? GroupSize { get; set; }

        [Index]
        public int IsCompulsoryOrVoluntary { get; set; }

        [Column(TypeName = "ntext")]
        public string UnderwritingMethod { get; set; }

        [MaxLength(255)]
        public string Remarks { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? RequestReceivedDate { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? EnquiryToClientDate { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? ClientReplyDate { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? QuotationSentDate { get; set; }

        public int? Score { get; set; }

        [Index]
        public bool HasPerLifeRetro { get; set; }

        public string ChecklistRemark { get; set; }

        public bool ChecklistPendingUnderwriting { get; set; }

        public bool ChecklistPendingHealth { get; set; }

        public bool ChecklistPendingClaims { get; set; }

        public bool ChecklistPendingBD { get; set; }

        public bool ChecklistPendingCR { get; set; }

        public int? QuotationTAT { get; set; }

        public int? InternalTAT { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? QuotationValidityDate { get; set; }

        public string QuotationValidityDay { get; set; }

        public int? FirstQuotationSentWeek { get; set; }

        public int? FirstQuotationSentMonth { get; set; }

        [MaxLength(10)]
        public string FirstQuotationSentQuarter { get; set; }

        public int? FirstQuotationSentYear { get; set; }

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

        public TreatyPricingGroupReferralVersion()
        {
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
        }

        public static bool IsExists(int id)
        {
            using (var db = new AppDbContext())
            {
                return db.TreatyPricingGroupReferralVersions.Any(q => q.Id == id);
            }
        }

        public static TreatyPricingGroupReferralVersion Find(int? id)
        {
            using (var db = new AppDbContext())
            {
                return db.TreatyPricingGroupReferralVersions.Where(q => q.Id == id).FirstOrDefault();
            }
        }

        public DataTrail Create()
        {
            using (var db = new AppDbContext())
            {
                db.TreatyPricingGroupReferralVersions.Add(this);
                db.SaveChanges();

                DataTrail trail = new DataTrail(this);
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

                DataTrail trail = new DataTrail(entity, this);

                entity.TreatyPricingGroupReferralId = TreatyPricingGroupReferralId;
                entity.Version = Version;
                entity.GroupReferralPersonInChargeId = GroupReferralPersonInChargeId;
                entity.CedantPersonInCharge = CedantPersonInCharge;
                entity.RequestTypePickListDetailId = RequestTypePickListDetailId;
                entity.PremiumTypePickListDetailId = PremiumTypePickListDetailId;
                entity.GrossRiskPremium = GrossRiskPremium;
                entity.ReinsurancePremium = ReinsurancePremium;
                entity.GrossRiskPremiumGTL = GrossRiskPremiumGTL;
                entity.ReinsurancePremiumGTL = ReinsurancePremiumGTL;
                entity.GrossRiskPremiumGHS = GrossRiskPremiumGHS;
                entity.ReinsurancePremiumGHS = ReinsurancePremiumGHS;
                entity.AverageSumAssured = AverageSumAssured;
                entity.GroupSize = GroupSize;
                entity.IsCompulsoryOrVoluntary = IsCompulsoryOrVoluntary;
                entity.UnderwritingMethod = UnderwritingMethod;
                entity.Remarks = Remarks;
                entity.RequestReceivedDate = RequestReceivedDate;
                entity.EnquiryToClientDate = EnquiryToClientDate;
                entity.ClientReplyDate = ClientReplyDate;
                entity.QuotationSentDate = QuotationSentDate;
                entity.Score = Score;
                entity.HasPerLifeRetro = HasPerLifeRetro;
                entity.ChecklistRemark = ChecklistRemark;
                entity.ChecklistPendingUnderwriting = ChecklistPendingUnderwriting;
                entity.ChecklistPendingHealth = ChecklistPendingHealth;
                entity.ChecklistPendingClaims = ChecklistPendingClaims;
                entity.ChecklistPendingBD = ChecklistPendingBD;
                entity.ChecklistPendingCR = ChecklistPendingCR;
                entity.QuotationTAT = QuotationTAT;
                entity.InternalTAT = InternalTAT;
                entity.QuotationValidityDate = QuotationValidityDate;
                entity.QuotationValidityDay = QuotationValidityDay;
                entity.FirstQuotationSentWeek = FirstQuotationSentWeek;
                entity.FirstQuotationSentMonth = FirstQuotationSentMonth;
                entity.FirstQuotationSentQuarter = FirstQuotationSentQuarter;
                entity.FirstQuotationSentYear = FirstQuotationSentYear;
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
                var entity = db.TreatyPricingGroupReferralVersions.Where(q => q.Id == id).FirstOrDefault();
                if (entity == null)
                {
                    throw new Exception(MessageBag.NoRecordFound);
                }

                DataTrail trail = new DataTrail(entity, true);

                db.Entry(entity).State = EntityState.Deleted;
                db.TreatyPricingGroupReferralVersions.Remove(entity);
                db.SaveChanges();

                return trail;
            }
        }
    }
}
