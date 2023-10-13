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
    [Table("Mfrs17CellMappings")]
    public class Mfrs17CellMapping
    {
        [Key]
        public int Id { get; set; }

        public int? TreatyCodeId { get; set; }

        [Required]
        public string TreatyCode { get; set; }

        [Required, Index]
        public int ReinsBasisCodePickListDetailId { get; set; }

        [ForeignKey(nameof(ReinsBasisCodePickListDetailId))]
        [ExcludeTrail]
        public virtual PickListDetail ReinsBasisCodePickListDetail { get; set; }

        [Column(TypeName = "datetime2"), Index]
        public DateTime? ReinsEffDatePolStartDate { get; set; }

        [Column(TypeName = "datetime2"), Index]
        public DateTime? ReinsEffDatePolEndDate { get; set; }

        public string CedingPlanCode { get; set; }

        public string BenefitCode { get; set; }

        [Required, Index]
        public int BasicRiderPickListDetailId { get; set; }

        [ForeignKey(nameof(BasicRiderPickListDetailId))]
        [ExcludeTrail]
        public virtual PickListDetail BasicRiderPickListDetail { get; set; }

        [Required, MaxLength(50), Index]
        public string CellName { get; set; }

        //[MaxLength(25), Index]
        //public string Mfrs17TreatyCode { get; set; }

        public int? Mfrs17ContractCodeDetailId { get; set; }

        [ForeignKey(nameof(Mfrs17ContractCodeDetailId))]
        [ExcludeTrail]
        public virtual Mfrs17ContractCodeDetail Mfrs17ContractCodeDetail { get; set; }
        
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

        [ExcludeTrail]
        public virtual ICollection<Mfrs17CellMappingDetail> Mfrs17CellMappingDetails { get; set; }

        [MaxLength(20)]
        public string LoaCode { get; set; }

        [MaxLength(1), Index]
        public string ProfitComm { get; set; }

        [Index]
        public int? ProfitCommPickListDetailId { get; set; }

        [ForeignKey(nameof(ProfitCommPickListDetailId))]
        [ExcludeTrail]
        public virtual PickListDetail ProfitCommPickListDetail { get; set; }

        [MaxLength(128), Index]
        public string RateTable { get; set; }

        public Mfrs17CellMapping()
        {
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
        }

        public static bool IsExists(int id)
        {
            using (var db = new AppDbContext())
            {
                return db.Mfrs17CellMappings.Any(q => q.Id == id);
            }
        }

        public static Mfrs17CellMapping Find(int id)
        {
            using (var db = new AppDbContext())
            {
                return db.Mfrs17CellMappings.Where(q => q.Id == id).FirstOrDefault();
            }
        }

        public static int CountByTreatyCodeId(int treatyCodeId)
        {
            using (var db = new AppDbContext())
            {
                TreatyCode treatyCode = Entities.TreatyCode.Find(treatyCodeId);
                if (treatyCode != null)
                    return db.Mfrs17CellMappings.Where(q => q.TreatyCode.Contains(treatyCode.Code)).Count();
                return 0;
            }
        }

        public static int CountByCellName(string cellName)
        {
            using (var db = new AppDbContext())
            {
                return db.Mfrs17CellMappings.Where(q => q.CellName == cellName).Count();
            }
        }

        public static int CountByReinsBasisCodePickListDetailId(int reinsBasisCodePickListDetailId)
        {
            using (var db = new AppDbContext())
            {
                return db.Mfrs17CellMappings.Where(q => q.ReinsBasisCodePickListDetailId == reinsBasisCodePickListDetailId).Count();
            }
        }

        public static int CountByBasicRiderPickListDetailId(int basicRiderPickListDetailId)
        {
            using (var db = new AppDbContext())
            {
                return db.Mfrs17CellMappings.Where(q => q.BasicRiderPickListDetailId == basicRiderPickListDetailId).Count();
            }
        }

        public static int CountByBenefitCode(string benefitCode)
        {
            using (var db = new AppDbContext())
            {
                return db.Mfrs17CellMappings.Where(q => q.Mfrs17CellMappingDetails.Any(d => d.BenefitCode == benefitCode)).Count();
            }
        }

        public DataTrail Create()
        {
            using (var db = new AppDbContext())
            {
                db.Mfrs17CellMappings.Add(this);
                db.SaveChanges();

                DataTrail trail = new DataTrail(this);
                return trail;
            }
        }

        public DataTrail Update()
        {
            using (var db = new AppDbContext())
            {
                Mfrs17CellMapping mfrs17CellMapping = Mfrs17CellMapping.Find(Id);
                if (mfrs17CellMapping == null)
                {
                    throw new Exception(MessageBag.NoRecordFound);
                }

                DataTrail trail = new DataTrail(mfrs17CellMapping, this);

                mfrs17CellMapping.TreatyCode = TreatyCode;
                mfrs17CellMapping.ReinsBasisCodePickListDetailId = ReinsBasisCodePickListDetailId;
                mfrs17CellMapping.ReinsEffDatePolStartDate = ReinsEffDatePolStartDate;
                mfrs17CellMapping.ReinsEffDatePolEndDate = ReinsEffDatePolEndDate;
                mfrs17CellMapping.CedingPlanCode = CedingPlanCode;
                mfrs17CellMapping.BenefitCode = BenefitCode;
                //mfrs17CellMapping.ProfitComm = ProfitComm;
                mfrs17CellMapping.ProfitCommPickListDetailId = ProfitCommPickListDetailId;
                mfrs17CellMapping.BasicRiderPickListDetailId = BasicRiderPickListDetailId;
                mfrs17CellMapping.CellName = CellName;
                //mfrs17CellMapping.Mfrs17TreatyCode = Mfrs17TreatyCode;
                mfrs17CellMapping.Mfrs17ContractCodeDetailId = Mfrs17ContractCodeDetailId;
                mfrs17CellMapping.LoaCode = LoaCode;
                mfrs17CellMapping.RateTable = RateTable;
                mfrs17CellMapping.UpdatedAt = DateTime.Now;
                mfrs17CellMapping.UpdatedById = UpdatedById ?? mfrs17CellMapping.UpdatedById;

                db.Entry(mfrs17CellMapping).State = EntityState.Modified;
                db.SaveChanges();

                return trail;
            }
        }

        public static DataTrail Delete(int id)
        {
            using (var db = new AppDbContext())
            {
                Mfrs17CellMapping mfrs17CellMapping = db.Mfrs17CellMappings.Where(q => q.Id == id).FirstOrDefault();
                if (mfrs17CellMapping == null)
                {
                    throw new Exception(MessageBag.NoRecordFound);
                }

                DataTrail trail = new DataTrail(mfrs17CellMapping, true);

                db.Entry(mfrs17CellMapping).State = EntityState.Deleted;
                db.Mfrs17CellMappings.Remove(mfrs17CellMapping);
                db.SaveChanges();

                return trail;
            }
        }
    }
}
