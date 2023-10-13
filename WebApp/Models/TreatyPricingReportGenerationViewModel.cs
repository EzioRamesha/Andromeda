using BusinessObject;
using BusinessObject.TreatyPricing;
using DataAccess.Entities;
using DataAccess.Entities.TreatyPricing;
using DataAccess.EntityFramework;
using Services;
using Services.Identity;
using Shared;
using Shared.Forms.Attributes;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;

namespace WebApp.Models
{
    public class TreatyPricingReportGenerationViewModel
    {
        public int Id { get; set; }

        [DisplayName("Report Name")]
        public string ReportName { get; set; }

        [DisplayName("Report Parameters")]
        public string ReportParams { get; set; }

        [DisplayName("File Name")]
        public string FileName { get; set; }

        [DisplayName("Hash File Name")]
        public string HashFileName { get; set; }

        [DisplayName("Status")]
        public int Status { get; set; }

        [DisplayName("Errors")]
        public string Errors { get; set; }

        [DisplayName("Date & Time Generated")]
        public DateTime CreatedAt { get; set; }

        [DisplayName("Submitted By")]
        public int CreatedById { get; set; }

        public TreatyPricingReportGenerationViewModel()
        {
            Set();
        }

        public void Set(TreatyPricingReportGenerationBo reportGenerationBo = null)
        {
            if (reportGenerationBo != null)
            {
                Id = reportGenerationBo.Id;
                ReportName = reportGenerationBo.ReportName;
                ReportParams = reportGenerationBo.ReportParams;
                FileName = reportGenerationBo.FileName;
                HashFileName = reportGenerationBo.HashFileName;
                Status = reportGenerationBo.Status;
                Errors = reportGenerationBo.Errors;
                CreatedAt = reportGenerationBo.CreatedAt;
                CreatedById = reportGenerationBo.CreatedById;
            }
        }

        public static Expression<Func<TreatyPricingReportGeneration, TreatyPricingReportGenerationViewModel>> Expression()
        {
            using (var db = new AppDbContext())
            {
                return entity => new TreatyPricingReportGenerationViewModel()
                {
                    Id = entity.Id,
                    ReportName = entity.ReportName,
                    ReportParams = entity.ReportParams,
                    FileName = entity.FileName,
                    HashFileName = entity.HashFileName,
                    Status = entity.Status,
                    Errors = entity.Errors,
                    CreatedAt = entity.CreatedAt,
                    CreatedById = entity.CreatedById,
                };
            }
        }
    }
}