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
    [Table("TreatyPricingTreatyWorkflows")]
    public class TreatyPricingTreatyWorkflow
    {
        [Key]
        public int Id { get; set; }

        [Index]
        public int DocumentType { get; set; }

        [Index]
        public int ReinsuranceTypePickListDetailId { get; set; }
        [ExcludeTrail]
        public virtual PickListDetail ReinsuranceTypePickListDetail { get; set; }

        [Index]
        public int CounterPartyDetailId { get; set; }
        [ExcludeTrail]
        public virtual Cedant CounterPartyDetail { get; set; }

        [Index]
        public int? InwardRetroPartyDetailId { get; set; }
        [ExcludeTrail]
        public virtual RetroParty InwardRetroPartyDetail { get; set; }

        [Index]
        public int? BusinessOriginPickListDetailId { get; set; }
        [ExcludeTrail]
        public virtual PickListDetail BusinessOriginPickListDetail { get; set; }

        public string TypeOfBusiness { get; set; }

        public string CountryOrigin { get; set; }

        [Index, MaxLength(255)]
        public string DocumentId { get; set; }

        [Index, MaxLength(255)]
        public string TreatyCode { get; set; }

        [Index]
        public int? CoverageStatus { get; set; }

        [Index]
        public int? DocumentStatus { get; set; }

        [Index]
        public int? DraftingStatus { get; set; }

        [Index]
        public int? DraftingStatusCategory { get; set; }


        [Column(TypeName = "datetime2")]
        public DateTime? EffectiveAt { get; set; }

        public string OrionGroupStr { get; set; }

        public string Description { get; set; }

        public string SharepointLink { get; set; }

        public string Reviewer { get; set; }

        public int LatestVersion { get; set; }

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

        public TreatyPricingTreatyWorkflow()
        {
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
        }

        public static bool IsExists(int id)
        {
            using (var db = new AppDbContext())
            {
                return db.TreatyPricingTreatyWorkflows.Any(q => q.Id == id);
            }
        }

        public static TreatyPricingTreatyWorkflow Find(int? id)
        {
            using (var db = new AppDbContext())
            {
                return db.TreatyPricingTreatyWorkflows.Where(q => q.Id == id).FirstOrDefault();
            }
        }

        public DataTrail Create()
        {
            using (var db = new AppDbContext())
            {
                db.TreatyPricingTreatyWorkflows.Add(this);
                db.SaveChanges();

                DataTrail trail = new DataTrail(this);
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
                db.TreatyPricingTreatyWorkflows.Remove(entity);
                db.SaveChanges();

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

                entity.DocumentType = DocumentType;
                entity.ReinsuranceTypePickListDetailId = ReinsuranceTypePickListDetailId;
                entity.CounterPartyDetailId = CounterPartyDetailId;
                entity.InwardRetroPartyDetailId = InwardRetroPartyDetailId;
                entity.BusinessOriginPickListDetailId = BusinessOriginPickListDetailId;
                entity.TypeOfBusiness = TypeOfBusiness;
                entity.CountryOrigin = CountryOrigin;
                entity.DocumentId = DocumentId;
                entity.TreatyCode = TreatyCode;
                entity.CoverageStatus = CoverageStatus;
                entity.DocumentStatus = DocumentStatus;
                entity.DraftingStatus = DraftingStatus;
                entity.DraftingStatusCategory = DraftingStatusCategory;
                entity.EffectiveAt = EffectiveAt;
                entity.OrionGroupStr = OrionGroupStr;
                entity.Description = Description;
                entity.SharepointLink = SharepointLink;
                entity.Reviewer = Reviewer;
                entity.LatestVersion = LatestVersion;
                entity.UpdatedAt = DateTime.Now;
                entity.UpdatedById = UpdatedById ?? entity.UpdatedById;

                db.Entry(entity).State = EntityState.Modified;
                db.SaveChanges();

                return trail;
            }
        }

        public string GetReinsuranceType(int? reinsuranceTypePickListDetailId = 0)
        {
            using (var db = new AppDbContext())
            {
                return db.PickListDetails.Where(q => q.Id == reinsuranceTypePickListDetailId).FirstOrDefault().Code;
            }
        }

        public string GetBusinessOrigin(int? businesssOriginPickListDetailId = 0)
        {
            using (var db = new AppDbContext())
            {
                return db.PickListDetails.Where(q => q.Id == businesssOriginPickListDetailId).FirstOrDefault().Code;
            }
        }

        public string GetBusinessType(int? businessTypePickListDetailId = 0)
        {
            using (var db = new AppDbContext())
            {
                return db.PickListDetails.Where(q => q.Id == businessTypePickListDetailId).FirstOrDefault().Code;
            }
        }

        public string GetInwardRetroParty(int? inwardRetroPartyDetailId = 0)
        {
            if (inwardRetroPartyDetailId.HasValue)
            {
                using (var db = new AppDbContext())
                {
                    return db.RetroParties.Where(q => q.Id == inwardRetroPartyDetailId).FirstOrDefault().Code;
                }
            }
            else
            {
                return null;
            }
        }

        public string GetCounterParty(int? counterPartyDetailId = 0)
        {
            using (var db = new AppDbContext())
            {
                return db.Cedants.Where(q => q.Id == counterPartyDetailId).FirstOrDefault().Code;
            }
        }
    }
}
