using BusinessObject;
using BusinessObject.RiDatas;
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

namespace DataAccess.Entities.RiDatas
{
    [Table("RiDataMappings")]
    public class RiDataMapping
    {
        [Key]
        public int Id { get; set; }

        [Required, Index]
        public int RiDataConfigId { get; set; }

        [ExcludeTrail]
        [ForeignKey(nameof(RiDataConfigId))]
        public virtual RiDataConfig RiDataConfig { get; set; }

        [Required, Index]
        public int StandardOutputId { get; set; }

        [ExcludeTrail]
        [ForeignKey(nameof(StandardOutputId))]
        public virtual StandardOutput StandardOutput { get; set; }

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

        public RiDataMapping()
        {
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
        }

        public RiDataMapping(RiDataMappingBo riDataMappingBo) : this()
        {
            Id = riDataMappingBo.Id;
            RiDataConfigId = riDataMappingBo.RiDataConfigId;
            StandardOutputId = riDataMappingBo.StandardOutputId;
            SortIndex = riDataMappingBo.SortIndex;
            Row = riDataMappingBo.Row;
            RawColumnName = riDataMappingBo.RawColumnName;
            TransformFormula = riDataMappingBo.TransformFormula;
            DefaultValue = riDataMappingBo.DefaultValue;
        }

        public static bool IsExists(int id)
        {
            using (var db = new AppDbContext())
            {
                return db.RiDataMappings.Any(q => q.Id == id);
            }
        }

        public static RiDataMapping Find(int id)
        {
            using (var db = new AppDbContext())
            {
                return db.RiDataMappings.Where(q => q.Id == id).FirstOrDefault();
            }
        }

        public static IList<RiDataMapping> GetByRiDataConfigId(int riDataConfigId)
        {
            using (var db = new AppDbContext())
            {
                db.Database.CommandTimeout = 0;

                EFExecutionStrategy connectionStrategy = EFExecutionStrategy.GetNewRiDataProcessingInstance(117, "RiDataMapping");

                return connectionStrategy.Execute(() => db.RiDataMappings.Where(q => q.RiDataConfigId == riDataConfigId).OrderBy(q => q.SortIndex).ToList());
            }
        }

        public static IList<RiDataMapping> GetByRiDataConfigIdExcept(int riDataConfigId, List<int> ids)
        {
            using (var db = new AppDbContext())
            {
                return db.RiDataMappings.Where(q => q.RiDataConfigId == riDataConfigId && !ids.Contains(q.Id)).ToList();
            }
        }

        public static int CountByPickListDetailId(int pickListDetailId)
        {
            using (var db = new AppDbContext())
            {
                return db.RiDataMappings.Where(q => q.StandardOutput.DataType == StandardOutputBo.DataTypeDropDown && q.TransformFormula == RiDataMappingBo.TransformFormulaFixedValue && q.DefaultObjectId == pickListDetailId).Count();
            }
        }

        public static int CountByBenefitId(int benefitId)
        {
            using (var db = new AppDbContext())
            {
                return db.RiDataMappings.Where(q => q.StandardOutput.Type == StandardOutputBo.TypeMlreBenefitCode && q.TransformFormula == RiDataMappingBo.TransformFormulaFixedValue && q.DefaultObjectId == benefitId).Count();
            }
        }

        public DataTrail Create()
        {
            using (var db = new AppDbContext())
            {
                db.RiDataMappings.Add(this);
                db.SaveChanges();

                DataTrail trail = new DataTrail(this);
                return trail;
            }
        }

        public DataTrail Update()
        {
            using (var db = new AppDbContext())
            {
                RiDataMapping riDataMapping = Find(Id);
                if (riDataMapping == null)
                {
                    throw new Exception(MessageBag.NoRecordFound);
                }

                DataTrail trail = new DataTrail(riDataMapping, this);

                riDataMapping.RiDataConfigId = RiDataConfigId;
                riDataMapping.StandardOutputId = StandardOutputId;
                riDataMapping.SortIndex = SortIndex;
                riDataMapping.Row = Row;
                riDataMapping.RawColumnName = RawColumnName;
                riDataMapping.Length = Length;
                riDataMapping.TransformFormula = TransformFormula;
                riDataMapping.DefaultValue = DefaultValue;
                riDataMapping.DefaultObjectId = DefaultObjectId;
                riDataMapping.UpdatedAt = DateTime.Now;
                riDataMapping.UpdatedById = UpdatedById ?? riDataMapping.UpdatedById;

                db.Entry(riDataMapping).State = EntityState.Modified;
                db.SaveChanges();

                return trail;
            }
        }

        public static DataTrail Delete(int id)
        {
            using (var db = new AppDbContext())
            {
                RiDataMapping riDataMapping = db.RiDataMappings.Where(q => q.Id == id).FirstOrDefault();
                if (riDataMapping == null)
                {
                    throw new Exception(MessageBag.NoRecordFound);
                }

                DataTrail trail = new DataTrail(riDataMapping, true);

                db.Entry(riDataMapping).State = EntityState.Deleted;
                db.RiDataMappings.Remove(riDataMapping);
                db.SaveChanges();

                return trail;
            }
        }

        public static IList<DataTrail> DeleteAllByRiDataConfigId(int riDataConfigId)
        {
            using (var db = new AppDbContext())
            {
                var query = db.RiDataMappings.Where(q => q.RiDataConfigId == riDataConfigId);

                List<DataTrail> trails = new List<DataTrail>();
                foreach (RiDataMapping riDataMapping in query.ToList())
                {
                    DataTrail trail = new DataTrail(riDataMapping, true);
                    trails.Add(trail);

                    db.Entry(riDataMapping).State = EntityState.Deleted;
                    db.RiDataMappings.Remove(riDataMapping);
                }

                db.SaveChanges();
                return trails;
            }
        }
    }
}
