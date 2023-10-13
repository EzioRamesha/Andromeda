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
    [Table("RetroSummaries")]
    public class RetroSummary
    {
        [Key]
        public int Id { get; set; }

        [Required, Index]
        public int DirectRetroId { get; set; }

        [ExcludeTrail]
        public virtual DirectRetro DirectRetro { get; set; }

        [MaxLength(10), Index]
        public string RiskQuarter { get; set; }

        [Index]
        public int? Month { get; set; }

        [Index]
        public int? Year { get; set; }

        [MaxLength(10), Index]
        public string Type { get; set; }

        [Index]
        public int? NoOfPolicy { get; set; }

        [Index]
        public double? TotalSar { get; set; }

        [Index]
        public double? TotalRiPremium { get; set; }

        [Index]
        public double? TotalDiscount { get; set; }

        [Index]
        public int? NoOfClaims { get; set; }

        [Index]
        public double? TotalClaims { get; set; }

        [MaxLength(128), Index]
        public string RetroParty1 { get; set; }

        [MaxLength(128), Index]
        public string RetroParty2 { get; set; }

        [MaxLength(128), Index]
        public string RetroParty3 { get; set; }

        [Index]
        public double? RetroShare1 { get; set; }

        [Index]
        public double? RetroShare2 { get; set; }

        [Index]
        public double? RetroShare3 { get; set; }

        [Index]
        public double? RetroRiPremium1 { get; set; }

        [Index]
        public double? RetroRiPremium2 { get; set; }

        [Index]
        public double? RetroRiPremium3 { get; set; }

        [Index]
        public double? RetroDiscount1 { get; set; }

        [Index]
        public double? RetroDiscount2 { get; set; }

        [Index]
        public double? RetroDiscount3 { get; set; }

        [Index]
        public double? RetroClaims1 { get; set; }

        [Index]
        public double? RetroClaims2 { get; set; }

        [Index]
        public double? RetroClaims3 { get; set; }

        [MaxLength(35), Index]
        public string TreatyCode { get; set; }

        [Index]
        public double? RetroPremiumSpread1 { get; set; }

        [Index]
        public double? RetroPremiumSpread2 { get; set; }

        [Index]
        public double? RetroPremiumSpread3 { get; set; }

        [Index]
        public double? TotalDirectRetroAar { get; set; }

        [Index]
        public int ReportingType { get; set; }

        [Index]
        public int? Mfrs17AnnualCohort { get; set; }

        [MaxLength(25), Index]
        public string Mfrs17ContractCode { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime CreatedAt { get; set; }

        [Required]
        public int CreatedById { get; set; }

        [ExcludeTrail]
        public virtual User CreatedBy { get; set; }

        public RetroSummary()
        {
            CreatedAt = DateTime.Now;
        }
        public static bool IsExists(int id)
        {
            using (var db = new AppDbContext())
            {
                return db.RetroSummaries.Any(q => q.Id == id);
            }
        }

        public static RetroSummary Find(int? id)
        {
            using (var db = new AppDbContext())
            {
                return db.RetroSummaries.Where(q => q.Id == id).FirstOrDefault();
            }
        }

        public DataTrail Create()
        {
            using (var db = new AppDbContext())
            {
                db.RetroSummaries.Add(this);
                db.SaveChanges();

                DataTrail trail = new DataTrail(this);
                return trail;
            }
        }

        public static DataTrail Delete(int id)
        {
            using (var db = new AppDbContext())
            {
                RetroSummary retrosummary = db.RetroSummaries.Where(q => q.Id == id).FirstOrDefault();
                if (retrosummary == null)
                {
                    throw new Exception(MessageBag.NoRecordFound);
                }

                DataTrail trail = new DataTrail(retrosummary, true);

                db.Entry(retrosummary).State = EntityState.Deleted;
                db.RetroSummaries.Remove(retrosummary);
                db.SaveChanges();

                return trail;
            }
        }

        public static IList<DataTrail> DeleteByDirectRetroId(int directRetroId)
        {
            using (var db = new AppDbContext())
            {
                var query = db.RetroSummaries.Where(q => q.DirectRetroId == directRetroId);

                List<DataTrail> trails = new List<DataTrail>();
                foreach (RetroSummary retroSummary in query.ToList())
                {
                    DataTrail trail = new DataTrail(retroSummary, true);
                    trails.Add(trail);

                    db.Entry(retroSummary).State = EntityState.Deleted;
                    db.RetroSummaries.Remove(retroSummary);
                }

                db.SaveChanges();
                return trails;
            }
        }
    }
}
