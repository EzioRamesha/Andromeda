using BusinessObject;
using BusinessObject.Claims;
using DataAccess.Entities.Identity;
using DataAccess.EntityFramework;
using Shared;
using Shared.Trails;
using Shared.Trails.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Entities.Claims
{
    [Table("ClaimDataMappings")]
    public class ClaimDataMapping
    {
        [Key]
        public int Id { get; set; }

        [Required, Index]
        public int ClaimDataConfigId { get; set; }

        [ExcludeTrail]
        [ForeignKey(nameof(ClaimDataConfigId))]
        public virtual ClaimDataConfig ClaimDataConfig { get; set; }

        [Required, Index]
        public int StandardClaimDataOutputId { get; set; }

        [ExcludeTrail]
        [ForeignKey(nameof(StandardClaimDataOutputId))]
        public virtual StandardClaimDataOutput StandardClaimDataOutput { get; set; }

        public int SortIndex { get; set; }

        [Required, DefaultValue(0)]
        public int? Row { get; set; }

        [MaxLength(128)]
        public string RawColumnName { get; set; }

        public int? Length { get; set; }

        [Required]
        public int TransformFormula { get; set; }

        [MaxLength(128)]
        public string DefaultValue { get; set; }

        public int? DefaultObjectId { get; set; }

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

        public ClaimDataMapping()
        {
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
        }

        public ClaimDataMapping(ClaimDataMappingBo claimDataMappingBo) : this()
        {
            Id = claimDataMappingBo.Id;
            ClaimDataConfigId = claimDataMappingBo.ClaimDataConfigId;
            StandardClaimDataOutputId = claimDataMappingBo.StandardClaimDataOutputId;
            SortIndex = claimDataMappingBo.SortIndex;
            Row = claimDataMappingBo.Row;
            RawColumnName = claimDataMappingBo.RawColumnName;
            TransformFormula = claimDataMappingBo.TransformFormula;
            DefaultValue = claimDataMappingBo.DefaultValue;
        }

        public static bool IsExists(int id)
        {
            using (var db = new AppDbContext())
            {
                return db.ClaimDataMappings.Any(q => q.Id == id);
            }
        }

        public static ClaimDataMapping Find(int id)
        {
            using (var db = new AppDbContext())
            {
                return db.ClaimDataMappings.Where(q => q.Id == id).FirstOrDefault();
            }
        }

        public static IList<ClaimDataMapping> GetByClaimDataConfigId(int claimDataConfigId)
        {
            using (var db = new AppDbContext())
            {
                return db.ClaimDataMappings.Where(q => q.ClaimDataConfigId == claimDataConfigId).OrderBy(q => q.SortIndex).ToList();
            }
        }

        public static IList<ClaimDataMapping> GetByClaimDataConfigIdExcept(int claimDataConfigId, List<int> ids)
        {
            using (var db = new AppDbContext())
            {
                return db.ClaimDataMappings.Where(q => q.ClaimDataConfigId == claimDataConfigId && !ids.Contains(q.Id)).ToList();
            }
        }

        public static int CountByPickListDetailId(int pickListDetailId)
        {
            using (var db = new AppDbContext())
            {
                return db.ClaimDataMappings.Where(q => q.StandardClaimDataOutput.DataType == StandardOutputBo.DataTypeDropDown && q.TransformFormula == ClaimDataMappingBo.TransformFormulaFixedValue && q.DefaultObjectId == pickListDetailId).Count();
            }
        }

        public static int CountByBenefitId(int benefitId)
        {
            using (var db = new AppDbContext())
            {
                return db.ClaimDataMappings.Where(q => q.StandardClaimDataOutput.Type == StandardOutputBo.TypeMlreBenefitCode && q.TransformFormula == ClaimDataMappingBo.TransformFormulaFixedValue && q.DefaultObjectId == benefitId).Count();
            }
        }

        public DataTrail Create()
        {
            using (var db = new AppDbContext())
            {
                db.ClaimDataMappings.Add(this);
                db.SaveChanges();

                DataTrail trail = new DataTrail(this);
                return trail;
            }
        }

        public DataTrail Update()
        {
            using (var db = new AppDbContext())
            {
                ClaimDataMapping claimDataMapping = Find(Id);
                if (claimDataMapping == null)
                {
                    throw new Exception(MessageBag.NoRecordFound);
                }

                DataTrail trail = new DataTrail(claimDataMapping, this);

                claimDataMapping.ClaimDataConfigId = ClaimDataConfigId;
                claimDataMapping.StandardClaimDataOutputId = StandardClaimDataOutputId;
                claimDataMapping.SortIndex = SortIndex;
                claimDataMapping.Row = Row;
                claimDataMapping.RawColumnName = RawColumnName;
                claimDataMapping.Length = Length;
                claimDataMapping.TransformFormula = TransformFormula;
                claimDataMapping.DefaultValue = DefaultValue;
                claimDataMapping.DefaultObjectId = DefaultObjectId;
                claimDataMapping.UpdatedAt = DateTime.Now;
                claimDataMapping.UpdatedById = UpdatedById ?? claimDataMapping.UpdatedById;

                db.Entry(claimDataMapping).State = EntityState.Modified;
                db.SaveChanges();

                return trail;
            }
        }

        public static DataTrail Delete(int id)
        {
            using (var db = new AppDbContext())
            {
                ClaimDataMapping claimDataMapping = db.ClaimDataMappings.Where(q => q.Id == id).FirstOrDefault();
                if (claimDataMapping == null)
                {
                    throw new Exception(MessageBag.NoRecordFound);
                }

                DataTrail trail = new DataTrail(claimDataMapping, true);

                db.Entry(claimDataMapping).State = EntityState.Deleted;
                db.ClaimDataMappings.Remove(claimDataMapping);
                db.SaveChanges();

                return trail;
            }
        }

        public static IList<DataTrail> DeleteAllByClaimDataConfigId(int claimDataConfigId)
        {
            using (var db = new AppDbContext())
            {
                var query = db.ClaimDataMappings.Where(q => q.ClaimDataConfigId == claimDataConfigId);

                List<DataTrail> trails = new List<DataTrail>();
                foreach (ClaimDataMapping claimDataMapping in query.ToList())
                {
                    DataTrail trail = new DataTrail(claimDataMapping, true);
                    trails.Add(trail);

                    db.Entry(claimDataMapping).State = EntityState.Deleted;
                    db.ClaimDataMappings.Remove(claimDataMapping);
                }

                db.SaveChanges();
                return trails;
            }
        }
    }
}
