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

namespace DataAccess.Entities.Sanctions
{
    [Table("SanctionVerificationDetails")]
    public class SanctionVerificationDetail
    {
        [Key]
        [Column(TypeName = "bigint")]
        public int Id { get; set; }

        [Required, Index]
        public int SanctionVerificationId { get; set; }
        [ExcludeTrail]
        public virtual SanctionVerification SanctionVerification { get; set; }

        [Required, Index]
        public int ModuleId { get; set; }

        [ExcludeTrail]
        [ForeignKey(nameof(ModuleId))]
        public virtual Module Module { get; set; }

        [Required, Index]
        public int ObjectId { get; set; }

        [Index]
        public int? BatchId { get; set; }

        [Index]
        public int Rule { get; set; }

        [Column(TypeName = "datetime2"), Index]
        public DateTime? UploadDate { get; set; }

        [MaxLength(128), Index]
        public string Category { get; set; }

        [MaxLength(30), Index]
        public string CedingCompany { get; set; }

        [MaxLength(35), Index]
        public string TreatyCode { get; set; }

        [MaxLength(30), Index]
        public string CedingPlanCode { get; set; }

        [MaxLength(150), Index]
        public string PolicyNumber { get; set; }

        [MaxLength(128), Index]
        public string InsuredName { get; set; }

        [Column(TypeName = "datetime2"), Index]
        public DateTime? InsuredDateOfBirth { get; set; }

        [MaxLength(15), Index]
        public string InsuredIcNumber { get; set; }

        [MaxLength(30), Index]
        public string SoaQuarter { get; set; }

        [Index]
        public double? SumReins { get; set; }

        [Index]
        public double? ClaimAmount { get; set; }

        public string SanctionRefNumber { get; set; }

        public string SanctionIdNumber { get; set; }

        public string SanctionAddress { get; set; }

        [MaxLength(5)]
        public string LineOfBusiness { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? PolicyCommencementDate { get; set; }

        [MaxLength(20)]
        public string PolicyStatusCode { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? RiskCoverageEndDate { get; set; }

        public double? GrossPremium { get; set; }

        [Index]
        public bool IsWhitelist { get; set; } = false;

        [Index]
        public bool IsExactMatch { get; set; } = false;

        public string Remark { get; set; }

        [Index]
        public int? PreviousDecision { get; set; }

        public string PreviousDecisionRemark { get; set; }

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

        public SanctionVerificationDetail()
        {
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
        }

        public static bool IsExists(int id)
        {
            using (var db = new AppDbContext())
            {
                return db.SanctionVerificationDetails.Any(q => q.Id == id);
            }
        }

        public static SanctionVerificationDetail Find(int? id)
        {
            using (var db = new AppDbContext())
            {
                return db.SanctionVerificationDetails.Where(q => q.Id == id).FirstOrDefault();
            }
        }

        public DataTrail Create()
        {
            using (var db = new AppDbContext())
            {
                db.Database.CommandTimeout = 0;

                EFExecutionStrategy connectionStrategy = EFExecutionStrategy.GetNewInstance("SanctionVerificationDetail");

                connectionStrategy.Execute(() =>
                {
                    db.SanctionVerificationDetails.Add(this);
                    db.SaveChanges();
                });

                DataTrail trail = new DataTrail(this);
                return trail;
            }
        }

        public DataTrail Update()
        {
            using (var db = new AppDbContext())
            {
                SanctionVerificationDetail sanctionVerificationDetail = Find(Id);
                if (sanctionVerificationDetail == null)
                {
                    throw new Exception(MessageBag.NoRecordFound);
                }

                DataTrail trail = new DataTrail(sanctionVerificationDetail, this);

                sanctionVerificationDetail.SanctionVerificationId = SanctionVerificationId;
                sanctionVerificationDetail.ModuleId = ModuleId;
                sanctionVerificationDetail.ObjectId = ObjectId;
                sanctionVerificationDetail.Rule = Rule;
                sanctionVerificationDetail.UploadDate = UploadDate;
                sanctionVerificationDetail.Category = Category;
                sanctionVerificationDetail.CedingCompany = CedingCompany;
                sanctionVerificationDetail.TreatyCode = TreatyCode;
                sanctionVerificationDetail.CedingPlanCode = CedingPlanCode;
                sanctionVerificationDetail.PolicyNumber = PolicyNumber;
                sanctionVerificationDetail.InsuredName = InsuredName;
                sanctionVerificationDetail.InsuredDateOfBirth = InsuredDateOfBirth;
                sanctionVerificationDetail.InsuredIcNumber = InsuredIcNumber;
                sanctionVerificationDetail.SoaQuarter = SoaQuarter;
                sanctionVerificationDetail.SumReins = SumReins;
                sanctionVerificationDetail.ClaimAmount = ClaimAmount;
                sanctionVerificationDetail.IsWhitelist = IsWhitelist;
                sanctionVerificationDetail.IsExactMatch = IsExactMatch;
                sanctionVerificationDetail.BatchId = BatchId;
                sanctionVerificationDetail.SanctionRefNumber = SanctionRefNumber;
                sanctionVerificationDetail.SanctionIdNumber = SanctionIdNumber;
                sanctionVerificationDetail.SanctionAddress = SanctionAddress;
                sanctionVerificationDetail.LineOfBusiness = LineOfBusiness;
                sanctionVerificationDetail.PolicyCommencementDate = PolicyCommencementDate;
                sanctionVerificationDetail.PolicyStatusCode = PolicyStatusCode;
                sanctionVerificationDetail.RiskCoverageEndDate = RiskCoverageEndDate;
                sanctionVerificationDetail.GrossPremium = GrossPremium;
                sanctionVerificationDetail.Remark = Remark;
                sanctionVerificationDetail.PreviousDecision = PreviousDecision;
                sanctionVerificationDetail.PreviousDecisionRemark = PreviousDecisionRemark;
                sanctionVerificationDetail.UpdatedAt = DateTime.Now;
                sanctionVerificationDetail.UpdatedById = UpdatedById ?? sanctionVerificationDetail.UpdatedById;

                db.Entry(sanctionVerificationDetail).State = EntityState.Modified;
                db.SaveChanges();

                return trail;
            }
        }

        public static DataTrail Delete(int id)
        {
            using (var db = new AppDbContext())
            {
                SanctionVerificationDetail sanctionVerificationDetail = db.SanctionVerificationDetails.Where(q => q.Id == id).FirstOrDefault();
                if (sanctionVerificationDetail == null)
                {
                    throw new Exception(MessageBag.NoRecordFound);
                }

                DataTrail trail = new DataTrail(sanctionVerificationDetail, true);

                db.Entry(sanctionVerificationDetail).State = EntityState.Deleted;
                db.SanctionVerificationDetails.Remove(sanctionVerificationDetail);
                db.SaveChanges();

                return trail;
            }
        }

        public static IList<DataTrail> DeleteBySanctionVerificationId(int sanctionVerificationId)
        {
            using (var db = new AppDbContext())
            {
                var query = db.SanctionVerificationDetails.Where(q => q.SanctionVerificationId == sanctionVerificationId);

                List<DataTrail> trails = new List<DataTrail>();
                foreach (SanctionVerificationDetail sanctionVerificationDetail in query.ToList())
                {
                    DataTrail trail = new DataTrail(sanctionVerificationDetail, true);
                    trails.Add(trail);

                    db.Entry(sanctionVerificationDetail).State = EntityState.Deleted;
                    db.SanctionVerificationDetails.Remove(sanctionVerificationDetail);
                }

                db.SaveChanges();
                return trails;
            }
        }
    }
}
