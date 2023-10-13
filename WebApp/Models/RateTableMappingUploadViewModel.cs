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
    public class RateTableMappingUploadViewModel
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

        public RateTableMappingUploadViewModel() { }

        public RateTableMappingUploadViewModel(RateTableMappingUploadBo rateTableMappingUploadBo)
        {
            Set(rateTableMappingUploadBo);
        }

        public void Set(RateTableMappingUploadBo bo)
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

        public static Expression<Func<RateTableMappingUpload, RateTableMappingUploadViewModel>> Expression()
        {
            using (var db = new AppDbContext())
            {
                return entity => new RateTableMappingUploadViewModel
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