using BusinessObject.Sanctions;
using DataAccess.Entities.Identity;
using DataAccess.Entities.Sanctions;
using Services.Sanctions;
using Shared;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace WebApp.Models
{
    public class SanctionBatchViewModel
    {
        public int Id { get; set; }

        [DisplayName("Source")]
        public int SourceId { get; set; }

        public Source Source { get; set; }

        [DisplayName("Filename")]
        public string FileName { get; set; }

        [DisplayName("Hash Filename")]
        public string HashFileName { get; set; }

        [DisplayName("Update Method")]
        public int Method { get; set; }

        [DisplayName("Status")]
        public int Status { get; set; }

        [DisplayName("Record")]
        public int Record { get; set; }

        [DisplayName("Upload Date & Time")]
        public DateTime UploadedAt { get; set; }

        public string UploadedAtStr { get; set; }

        [DisplayName("Uploaded By")]
        public int CreatedById { get; set; }

        public User CreatedBy { get; set; }

        public string Errors { get; set; }

        public SanctionBatchViewModel()
        {
            Set();
        }

        public SanctionBatchViewModel(SanctionBatchBo sanctionBatchBo)
        {
            Set(sanctionBatchBo);
        }

        public void Set(SanctionBatchBo bo = null)
        {
            if (bo != null)
            {
                Id = bo.Id;
                SourceId = bo.SourceId;
                FileName = bo.FileName;
                HashFileName = bo.HashFileName;
                Record = bo.Record;
                Method = bo.Method;
                Status = bo.Status;
                UploadedAt = bo.UploadedAt;
                UploadedAtStr = bo.UploadedAt.ToString(Util.GetDateTimeFormat());
                Errors = bo.Errors;
            }
        }

        public SanctionBatchBo FormBo(int createdById, int updatedById)
        {
            var bo = new SanctionBatchBo
            {
                Id = Id,
                SourceId = SourceId,
                SourceBo = SourceService.Find(SourceId),
                FileName = FileName,
                HashFileName = HashFileName,
                Record = Record,
                Method = Method,
                Status = Status,
                UploadedAt = UploadedAt,
                Errors = Errors,
                CreatedById = createdById,
                UpdatedById = updatedById,
            };

            return bo;
        }

        public static Expression<Func<SanctionBatch, SanctionBatchViewModel>> Expression()
        {
            return entity => new SanctionBatchViewModel
            {
                Id = entity.Id,
                SourceId = entity.SourceId,
                Source = entity.Source,
                FileName = entity.FileName,
                HashFileName = entity.HashFileName,
                Record = entity.Record,
                Method = entity.Method,
                Status = entity.Status,
                UploadedAt = entity.UploadedAt,
                Errors = entity.Errors,
                CreatedById = entity.CreatedById,
                CreatedBy = entity.CreatedBy,
            };
        }
    }

    public class SanctionViewModel
    {
        public int Id { get; set; }

        public int SanctionBatchId { get; set; }

        public SanctionBatch SanctionBatch { get; set; }

        [DisplayName("Publication Information")]
        public string PublicationInformation { get; set; }

        [DisplayName("Category")]
        public string Category { get; set; }

        [DisplayName("Ref No")]
        public string RefNumber { get; set; }

        [DisplayName("ID Type")]
        public string IdType { get; set; }

        [DisplayName("ID Number")]
        public string IdNumber { get; set; }

        [DisplayName("Date of Birth")]
        public DateTime? DateOfBirth { get; set; }

        [DisplayName("Year of Birth")]
        public int? YearOfBirth { get; set; }

        [DisplayName("Country")]
        public string Country { get; set; }

        [DisplayName("Address")]
        public string Address { get; set; }

        [DisplayName("Comment")]
        public string Comment { get; set; }

        [DisplayName("Source")]
        public string SourceName { get; set; }

        public virtual ICollection<SanctionName> SanctionNames { get; set; }
        public virtual ICollection<SanctionIdentity> SanctionIdentities { get; set; }
        public virtual ICollection<SanctionBirthDate> SanctionBirthDates { get; set; }
        public virtual ICollection<SanctionAddress> SanctionAddresses { get; set; }
        public virtual ICollection<SanctionCountry> SanctionCountries { get; set; }
        public virtual ICollection<SanctionComment> SanctionComments { get; set; }
        public virtual ICollection<SanctionFormatName> SanctionFormatNames { get; set; }

        public static Expression<Func<Sanction, SanctionViewModel>> Expression()
        {
            return entity => new SanctionViewModel
            {
                Id = entity.Id,
                SanctionBatchId = entity.SanctionBatchId,
                SanctionBatch = entity.SanctionBatch,
                PublicationInformation = entity.PublicationInformation,
                Category = entity.Category,
                RefNumber = entity.RefNumber,
                SanctionNames = entity.SanctionNames,
                SanctionIdentities = entity.SanctionIdentities,
                SanctionBirthDates = entity.SanctionBirthDates,
                SanctionAddresses = entity.SanctionAddresses,
                SanctionCountries = entity.SanctionCountries,
                SanctionComments = entity.SanctionComments,
                SanctionFormatNames = entity.SanctionFormatNames,
                SourceName = entity.SanctionBatch.Source.Name
            };
        }
    }
}