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

namespace DataAccess.Entities.TreatyPricing
{
    [Table("TreatyPricingQuotationWorkflowVersions")]
    public class TreatyPricingQuotationWorkflowVersion
    {
        [Key]
        public int Id { get; set; }

        [Required, Index]
        public int TreatyPricingQuotationWorkflowId { get; set; }
        [ExcludeTrail]
        public virtual TreatyPricingQuotationWorkflow TreatyPricingQuotationWorkflow { get; set; }

        [Required, Index]
        public int Version { get; set; }

        #region Quotation
        [Index]
        public int? BDPersonInChargeId { get; set; }
        [ExcludeTrail]
        public virtual User BDPersonInCharge { get; set; }

        [Index]
        public int? QuoteValidityDay { get; set; }

        [Column(TypeName = "ntext")]
        public string QuoteSpecTemplate { get; set; }

        [Column(TypeName = "ntext")]
        public string RateTableTemplate { get; set; }

        [Column(TypeName = "ntext")]
        public string QuoteSpecSharePointLink { get; set; }

        [Column(TypeName = "ntext")]
        public string QuoteSpecSharePointFolderPath { get; set; }

        [Column(TypeName = "ntext")]
        public string RateTableSharePointLink { get; set; }

        [Column(TypeName = "ntext")]
        public string RateTableSharePointFolderPath { get; set; }

        [MaxLength(255), Index]
        public string FinalQuoteSpecFileName { get; set; }

        [MaxLength(255), Index]
        public string FinalQuoteSpecHashFileName { get; set; }

        [MaxLength(255), Index]
        public string FinalRateTableFileName { get; set; }

        [MaxLength(255), Index]
        public string FinalRateTableHashFileName { get; set; }

        [Index]
        public bool ChecklistFinalised { get; set; }
        #endregion

        #region Pricing
        [Index]
        public int? PersonInChargeId { get; set; }
        [ExcludeTrail]
        public virtual User PersonInCharge { get; set; }

        [Index]
        public int? PersonInChargeTechReviewerId { get; set; }
        [ExcludeTrail]
        public virtual User PersonInChargeTechReviewer { get; set; }

        [Index]
        public int? PersonInChargePeerReviewerId { get; set; }
        [ExcludeTrail]
        public virtual User PersonInChargePeerReviewer { get; set; }

        [Index]
        public int? PersonInChargePricingAuthorityReviewerId { get; set; }
        [ExcludeTrail]
        public virtual User PersonInChargePricingAuthorityReviewer { get; set; }

        [MaxLength(255), Index]
        public string PendingOn { get; set; }

        [Column(TypeName = "datetime2"), Index]
        public DateTime? RequestDate { get; set; }

        [Column(TypeName = "datetime2"), Index]
        public DateTime? TargetPricingDueDate { get; set; }

        [Column(TypeName = "datetime2"), Index]
        public DateTime? RevisedPricingDueDate { get; set; }

        [Column(TypeName = "datetime2"), Index]
        public DateTime? PricingCompletedDate { get; set; }

        [Index]
        public double? ProfitMargin { get; set; }

        [Index]
        public double? FirstYearPremium { get; set; }

        [Index]
        public double? PVProfit { get; set; }

        [Index]
        public double? ROE { get; set; }

        [Index]
        public double? ExpenseMargin { get; set; }

        [Column(TypeName = "ntext")]
        public string FileLocationPricingMemo { get; set; }

        [Column(TypeName = "ntext")]
        public string FileLocationNBChecklist { get; set; }

        [Column(TypeName = "ntext")]
        public string FileLocationTechnicalChecklist { get; set; }
        #endregion

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

        public TreatyPricingQuotationWorkflowVersion()
        {
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
        }

        public static bool IsExists(int id)
        {
            using (var db = new AppDbContext())
            {
                return db.TreatyPricingQuotationWorkflowVersions.Any(q => q.Id == id);
            }
        }

        public static TreatyPricingQuotationWorkflowVersion Find(int id)
        {
            using (var db = new AppDbContext())
            {
                return db.TreatyPricingQuotationWorkflowVersions.Where(q => q.Id == id).FirstOrDefault();
            }
        }

        public DataTrail Create()
        {
            using (var db = new AppDbContext())
            {
                db.TreatyPricingQuotationWorkflowVersions.Add(this);
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

                entity.TreatyPricingQuotationWorkflowId = TreatyPricingQuotationWorkflowId;
                entity.Version = Version;

                //Quotation
                entity.BDPersonInChargeId = BDPersonInChargeId;
                entity.QuoteValidityDay = QuoteValidityDay;
                entity.QuoteSpecTemplate = QuoteSpecTemplate;
                entity.RateTableTemplate = RateTableTemplate;
                entity.QuoteSpecSharePointLink = QuoteSpecSharePointLink;
                entity.QuoteSpecSharePointFolderPath = QuoteSpecSharePointFolderPath;
                entity.RateTableSharePointLink = RateTableSharePointLink;
                entity.RateTableSharePointFolderPath = RateTableSharePointFolderPath;
                entity.FinalQuoteSpecFileName = FinalQuoteSpecFileName;
                entity.FinalQuoteSpecHashFileName = FinalQuoteSpecHashFileName;
                entity.FinalRateTableFileName = FinalRateTableFileName;
                entity.FinalRateTableHashFileName = FinalRateTableHashFileName;
                entity.ChecklistFinalised = ChecklistFinalised;

                //Pricing
                entity.PersonInChargeId = PersonInChargeId;
                entity.PersonInChargeTechReviewerId = PersonInChargeTechReviewerId;
                entity.PersonInChargePeerReviewerId = PersonInChargePeerReviewerId;
                entity.PersonInChargePricingAuthorityReviewerId = PersonInChargePricingAuthorityReviewerId;
                entity.PendingOn = PendingOn;
                entity.RequestDate = RequestDate;
                entity.TargetPricingDueDate = TargetPricingDueDate;
                entity.RevisedPricingDueDate = RevisedPricingDueDate;
                entity.PricingCompletedDate = PricingCompletedDate;
                entity.ProfitMargin = ProfitMargin;
                entity.FirstYearPremium = FirstYearPremium;
                entity.PVProfit = PVProfit;
                entity.ROE = ROE;
                entity.ExpenseMargin = ExpenseMargin;
                entity.FileLocationPricingMemo = FileLocationPricingMemo;
                entity.FileLocationNBChecklist = FileLocationNBChecklist;
                entity.FileLocationTechnicalChecklist = FileLocationTechnicalChecklist;

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
                {
                    throw new Exception(MessageBag.NoRecordFound);
                }

                DataTrail trail = new DataTrail(entity, true);

                db.Entry(entity).State = EntityState.Deleted;
                db.TreatyPricingQuotationWorkflowVersions.Remove(entity);
                db.SaveChanges();

                return trail;
            }
        }

        public static IList<DataTrail> DeleteAllByTreatyPricingQuotationWorkflowId(int treatyPricingQuotationWorkflowId)
        {
            using (var db = new AppDbContext())
            {
                var query = db.TreatyPricingQuotationWorkflowVersions.Where(q => q.TreatyPricingQuotationWorkflowId == treatyPricingQuotationWorkflowId);

                List<DataTrail> trails = new List<DataTrail>();
                foreach (TreatyPricingQuotationWorkflowVersion entity in query.ToList())
                {
                    DataTrail trail = new DataTrail(entity, true);
                    trails.Add(trail);

                    db.Entry(entity).State = EntityState.Deleted;
                    db.TreatyPricingQuotationWorkflowVersions.Remove(entity);
                }

                db.SaveChanges();
                return trails;
            }
        }
    }
}
