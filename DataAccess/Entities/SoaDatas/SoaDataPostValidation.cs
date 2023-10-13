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

namespace DataAccess.Entities.SoaDatas
{
    [Table("SoaDataPostValidations")]
    public class SoaDataPostValidation
    {
        [Key]
        public int Id { get; set; }


        [Required, Index]
        public int SoaDataBatchId { get; set; }
        [ExcludeTrail]
        public virtual SoaDataBatch SoaDataBatch { get; set; }


        [Required, Index]
        public int Type { get; set; }


        [MaxLength(12), Index]
        public string BusinessCode { get; set; }
        [MaxLength(35), Index]
        public string TreatyCode { get; set; }
        [MaxLength(32), Index]
        public string SoaQuarter { get; set; }
        [MaxLength(32), Index]
        public string RiskQuarter { get; set; }
        [Index]
        public int? RiskMonth { get; set; }


        public double? NbPremium { get; set; }
        public double? RnPremium { get; set; }
        public double? AltPremium { get; set; }
        public double? GrossPremium { get; set; }


        public double? NbDiscount { get; set; }
        public double? RnDiscount { get; set; }
        public double? AltDiscount { get; set; }
        public double? TotalDiscount { get; set; }


        public double? NoClaimBonus { get; set; }
        public double? Claim { get; set; }
        public double? SurrenderValue { get; set; }
        public double? Gst { get; set; }
        public double? NetTotalAmount { get; set; }


        [MaxLength(3), Index]
        public string CurrencyCode { get; set; }
        public double? CurrencyRate { get; set; }


        public int? NbCession { get; set; }
        public int? RnCession { get; set; }
        public int? AltCession { get; set; }


        public double? NbSar { get; set; }
        public double? RnSar { get; set; }
        public double? AltSar { get; set; }


        public double? DTH { get; set; }
        public double? TPA { get; set; }
        public double? TPS { get; set; }
        public double? PPD { get; set; }
        public double? CCA { get; set; }
        public double? CCS { get; set; }
        public double? PA { get; set; }
        public double? HS { get; set; }
        public double? TPD { get; set; }
        public double? CI { get; set; }


        [Column(TypeName = "datetime2")]
        public DateTime CreatedAt { get; set; }
        [Required]
        public int CreatedById { get; set; }
        [ExcludeTrail]
        public virtual User CreatedBy { get; set; }


        [Column(TypeName = "datetime2")]
        public DateTime UpdatedAt { get; set; }
        public int? UpdatedById { get; set; }
        [ExcludeTrail]
        public virtual User UpdatedBy { get; set; }


        public SoaDataPostValidation()
        {
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
        }

        public static bool IsExists(int id)
        {
            using (var db = new AppDbContext())
            {
                return db.SoaDataPostValidations.Any(q => q.Id == id);
            }
        }

        public static SoaDataPostValidation Find(int? id)
        {
            using (var db = new AppDbContext())
            {
                return db.SoaDataPostValidations.Where(q => q.Id == id).FirstOrDefault();
            }
        }

        public static IList<SoaDataPostValidation> GetBySoaDataBatchIdTypeCurrencyCode(int soaDataBatchId, int type, bool originalCurrency)
        {
            using (var db = new AppDbContext())
            {
                var query = db.SoaDataPostValidations.Where(q => q.SoaDataBatchId == soaDataBatchId && q.Type == type);

                if (originalCurrency)
                    query = query.Where(q => !string.IsNullOrEmpty(q.CurrencyCode) && q.CurrencyCode != PickListDetailBo.CurrencyCodeMyr);
                else
                    query = query.Where(q => string.IsNullOrEmpty(q.CurrencyCode) || q.CurrencyCode == PickListDetailBo.CurrencyCodeMyr);

                return query.OrderBy(q => q.TreatyCode).ThenBy(q => q.RiskQuarter).ThenBy(q => q.RiskMonth).ToList();
            }
        }

        public DataTrail Create()
        {
            using (var db = new AppDbContext())
            {
                db.SoaDataPostValidations.Add(this);
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
                {
                    throw new Exception(MessageBag.NoRecordFound);
                }

                var trail = new DataTrail(entity, this);

                entity.SoaDataBatchId = SoaDataBatchId;
                entity.Type = Type;
                entity.BusinessCode = BusinessCode;
                entity.TreatyCode = TreatyCode;
                entity.SoaQuarter = SoaQuarter;
                entity.RiskQuarter = RiskQuarter;
                entity.RiskMonth = RiskMonth;

                entity.NbPremium = NbPremium;
                entity.RnPremium = RnPremium;
                entity.AltPremium = AltPremium;
                entity.GrossPremium = GrossPremium;

                entity.NbDiscount = NbDiscount;
                entity.RnDiscount = RnDiscount;
                entity.AltDiscount = AltDiscount;

                entity.TotalDiscount = TotalDiscount;

                entity.NoClaimBonus = NoClaimBonus;
                entity.Claim = Claim;
                entity.SurrenderValue = SurrenderValue;
                entity.Gst = Gst;
                entity.NetTotalAmount = NetTotalAmount;

                entity.CurrencyCode = CurrencyCode;
                entity.CurrencyRate = CurrencyRate;

                entity.NbCession = NbCession;
                entity.RnCession = RnCession;
                entity.AltCession = AltCession;

                entity.NbSar = NbSar;
                entity.RnSar = RnSar;
                entity.AltSar = AltSar;

                entity.DTH = DTH;
                entity.TPA = TPA;
                entity.TPS = TPS;
                entity.PPD = PPD;
                entity.CCA = CCA;
                entity.CCS = CCS;
                entity.PA = PA;
                entity.HS = HS;
                entity.TPD = TPD;
                entity.CI = CI;

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
                var entity = db.SoaDataPostValidations.Where(q => q.Id == id).FirstOrDefault();
                if (entity == null)
                {
                    throw new Exception(MessageBag.NoRecordFound);
                }

                var trail = new DataTrail(entity, true);

                db.Entry(entity).State = EntityState.Deleted;
                db.SoaDataPostValidations.Remove(entity);
                db.SaveChanges();

                return trail;
            }
        }
    }
}
