using BusinessObject;
using DataAccess.Entities;
using DataAccess.EntityFramework;
using Services;
using Shared;
using Shared.Forms.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;

namespace WebApp.Models
{
    public class TreatyBenefitCodeMappingUploadViewModel
    {
        public int Id { get; set; }

        [Required, DisplayName("Status")]
        public int Status { get; set; }

        [DisplayName("File Name")]
        public string FileName { get; set; }

        [DisplayName("Hash File Name")]
        public string HashFileName { get; set; }

        [DisplayName("Errors")]
        public string Errors { get; set; }

        [DisplayName("Date & Time Generated")]
        public DateTime CreatedAt { get; set; }

        [DisplayName("Submitted By")]
        public int CreatedById { get; set; }

        public TreatyBenefitCodeMappingUploadViewModel() { }

        public TreatyBenefitCodeMappingUploadViewModel(TreatyBenefitCodeMappingUploadBo treatyBenefitCodeMappingUploadBo)
        {
            Set(treatyBenefitCodeMappingUploadBo);
        }

        public void Set(TreatyBenefitCodeMappingUploadBo bo)
        {
            if (bo != null)
            {
                Id = bo.Id;
                Status = bo.Status;
                FileName = bo.FileName;
                HashFileName = bo.HashFileName;
                Errors = bo.Errors;
                CreatedAt = bo.CreatedAt;
                CreatedById = bo.CreatedById;
            }
        }

        public static Expression<Func<TreatyBenefitCodeMappingUpload, TreatyBenefitCodeMappingUploadViewModel>> Expression()
        {
            using (var db = new AppDbContext())
            {
                return entity => new TreatyBenefitCodeMappingUploadViewModel
                {
                    Id = entity.Id,
                    Status = entity.Status,
                    FileName = entity.FileName,
                    HashFileName = entity.HashFileName,
                    Errors = entity.Errors,
                    CreatedAt = entity.CreatedAt,
                    CreatedById = entity.CreatedById,
                };
            }
        }
    }
}