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
    [Table("DirectRetroConfigurationDetails")]
    public class DirectRetroConfigurationDetail
    {
        [Key]
        public int Id { get; set; }

        [Required, Index]
        public int DirectRetroConfigurationId { get; set; }

        [ExcludeTrail]
        public virtual DirectRetroConfiguration DirectRetroConfiguration { get; set; }

        [Column(TypeName = "datetime2"), Index]
        public DateTime? RiskPeriodStartDate { get; set; }

        [Column(TypeName = "datetime2"), Index]
        public DateTime? RiskPeriodEndDate { get; set; }

        [Column(TypeName = "datetime2"), Index]
        public DateTime? IssueDatePolStartDate { get; set; }

        [Column(TypeName = "datetime2"), Index]
        public DateTime? IssueDatePolEndDate { get; set; }

        [Column(TypeName = "datetime2"), Index]
        public DateTime? ReinsEffDatePolStartDate { get; set; }

        [Column(TypeName = "datetime2"), Index]
        public DateTime? ReinsEffDatePolEndDate { get; set; }

        [Required, Index]
        public bool IsDefault { get; set; } = false;

        [Required, Index]
        public int RetroPartyId { get; set; }

        [ExcludeTrail]
        public virtual RetroParty RetroParty { get; set; }

        [MaxLength(128)]
        public string TreatyNo { get; set; }

        [MaxLength(20)]
        public string Schedule { get; set; }

        [Required, Index]
        public double Share { get; set; }
        
        [Index]
        public int? PremiumSpreadTableId { get; set; }

        [ExcludeTrail]
        public virtual PremiumSpreadTable PremiumSpreadTable { get; set; }
        
        [Index]
        public int? TreatyDiscountTableId { get; set; }

        [ExcludeTrail]
        public virtual TreatyDiscountTable TreatyDiscountTable { get; set; }

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

        public DirectRetroConfigurationDetail()
        {
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
        }

        public static bool IsExists(int id)
        {
            using (var db = new AppDbContext())
            {
                return db.DirectRetroConfigurationDetails.Any(q => q.Id == id);
            }
        }

        public static DirectRetroConfigurationDetail Find(int? id)
        {
            using (var db = new AppDbContext())
            {
                return db.DirectRetroConfigurationDetails.Where(q => q.Id == id).FirstOrDefault();
            }
        }

        public static int CountByRetroPartyId(int retroPartyId)
        {
            using (var db = new AppDbContext())
            {
                return db.DirectRetroConfigurationDetails.Where(q => q.RetroPartyId == retroPartyId).Count();
            }
        }

        public static int CountByPremiumSpreadTableId(int premiumSpreadTableId)
        {
            using (var db = new AppDbContext())
            {
                return db.DirectRetroConfigurationDetails.Where(q => q.PremiumSpreadTableId == premiumSpreadTableId).Count();
            }
        }

        public static int CountByTreatyDiscountTableId(int TreatyDiscountTableId)
        {
            using (var db = new AppDbContext())
            {
                return db.DirectRetroConfigurationDetails.Where(q => q.TreatyDiscountTableId == TreatyDiscountTableId).Count();
            }
        }

        public static IList<DirectRetroConfigurationDetail> Get()
        {
            using (var db = new AppDbContext())
            {
                return db.DirectRetroConfigurationDetails.ToList();
            }
        }

        public static IList<DirectRetroConfigurationDetail> GetByDirectRetroConfigurationId(int directRetroConfigurationId)
        {
            using (var db = new AppDbContext())
            {
                return db.DirectRetroConfigurationDetails.Where(q => q.DirectRetroConfigurationId == directRetroConfigurationId).ToList();
            }
        }

        public static IList<DirectRetroConfigurationDetail> GetByDirectRetroConfigurationIdExcept(int directRetroConfigurationId, List<int> ids)
        {
            using (var db = new AppDbContext())
            {
                return db.DirectRetroConfigurationDetails.Where(q => q.DirectRetroConfigurationId == directRetroConfigurationId && !ids.Contains(q.Id)).ToList();
            }
        }

        public DataTrail Create()
        {
            using (var db = new AppDbContext())
            {
                db.DirectRetroConfigurationDetails.Add(this);
                db.SaveChanges();

                DataTrail trail = new DataTrail(this);
                return trail;
            }
        }

        public DataTrail Update()
        {
            using (var db = new AppDbContext())
            {
                DirectRetroConfigurationDetail directRetroConfigurationDetail = Find(Id);
                if (directRetroConfigurationDetail == null)
                {
                    throw new Exception(MessageBag.NoRecordFound);
                }

                DataTrail trail = new DataTrail(directRetroConfigurationDetail, this);

                directRetroConfigurationDetail.DirectRetroConfigurationId = DirectRetroConfigurationId;
                directRetroConfigurationDetail.RiskPeriodStartDate = RiskPeriodStartDate;
                directRetroConfigurationDetail.RiskPeriodEndDate = RiskPeriodEndDate;
                directRetroConfigurationDetail.IssueDatePolStartDate = IssueDatePolStartDate;
                directRetroConfigurationDetail.IssueDatePolEndDate = IssueDatePolEndDate;
                directRetroConfigurationDetail.ReinsEffDatePolStartDate = ReinsEffDatePolStartDate;
                directRetroConfigurationDetail.ReinsEffDatePolEndDate = ReinsEffDatePolEndDate;
                directRetroConfigurationDetail.ReinsEffDatePolEndDate = ReinsEffDatePolEndDate;
                directRetroConfigurationDetail.IsDefault = IsDefault;
                directRetroConfigurationDetail.RetroPartyId = RetroPartyId;
                directRetroConfigurationDetail.TreatyNo = TreatyNo;
                directRetroConfigurationDetail.Schedule = Schedule;
                directRetroConfigurationDetail.Share = Share;
                directRetroConfigurationDetail.PremiumSpreadTableId = PremiumSpreadTableId;
                directRetroConfigurationDetail.TreatyDiscountTableId = TreatyDiscountTableId;
                directRetroConfigurationDetail.UpdatedAt = DateTime.Now;
                directRetroConfigurationDetail.UpdatedById = UpdatedById ?? directRetroConfigurationDetail.UpdatedById;

                db.Entry(directRetroConfigurationDetail).State = EntityState.Modified;
                db.SaveChanges();

                return trail;
            }
        }

        public static DataTrail Delete(int id)
        {
            using (var db = new AppDbContext())
            {
                DirectRetroConfigurationDetail directRetroConfigurationDetail = db.DirectRetroConfigurationDetails.Where(q => q.Id == id).FirstOrDefault();
                if (directRetroConfigurationDetail == null)
                {
                    throw new Exception(MessageBag.NoRecordFound);
                }

                DataTrail trail = new DataTrail(directRetroConfigurationDetail, true);

                db.Entry(directRetroConfigurationDetail).State = EntityState.Deleted;
                db.DirectRetroConfigurationDetails.Remove(directRetroConfigurationDetail);
                db.SaveChanges();

                return trail;
            }
        }

        public static IList<DataTrail> DeleteByDirectRetroConfigurationId(int directRetroConfigurationId)
        {
            using (var db = new AppDbContext())
            {
                var query = db.DirectRetroConfigurationDetails.Where(q => q.DirectRetroConfigurationId == directRetroConfigurationId);

                List<DataTrail> trails = new List<DataTrail>();
                foreach (DirectRetroConfigurationDetail directRetroConfigurationDetail in query.ToList())
                {
                    DataTrail trail = new DataTrail(directRetroConfigurationDetail, true);
                    trails.Add(trail);

                    db.Entry(directRetroConfigurationDetail).State = EntityState.Deleted;
                    db.DirectRetroConfigurationDetails.Remove(directRetroConfigurationDetail);
                }

                db.SaveChanges();
                return trails;
            }
        }
    }
}
