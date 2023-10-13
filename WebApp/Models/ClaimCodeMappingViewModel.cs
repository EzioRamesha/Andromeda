using BusinessObject;
using DataAccess.Entities;
using Services;
using Shared;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace WebApp.Models
{
    public class ClaimCodeMappingViewModel
    {
        public int Id { get; set; }

        [DisplayName("MLRe Event Code")]
        [ValidateMlreEventCode]
        public string MlreEventCode { get; set; }

        [DisplayName("MLRe Benefit Code")]
        [ValidateMlreBenefitCode]
        public string MlreBenefitCode { get; set; }

        [DisplayName("Claim Code")]
        public int ClaimCodeId { get; set; }

        public ClaimCode ClaimCode { get; set; }

        public ClaimCodeBo ClaimCodeBo { get; set; }

        public virtual ICollection<ClaimCodeMappingDetail> ClaimCodeMappingDetails { get; set; }

        public ClaimCodeMappingViewModel() { }

        public ClaimCodeMappingViewModel(ClaimCodeMappingBo claimCodeMapping)
        {
            Set(claimCodeMapping);
        }

        public void Set(ClaimCodeMappingBo bo)
        {
            if (bo != null)
            {
                Id = bo.Id;
                MlreEventCode = bo.MlreEventCode;
                MlreBenefitCode = bo.MlreBenefitCode;
                ClaimCodeId = bo.ClaimCodeId;
                ClaimCodeBo = bo.ClaimCodeBo;
            }
        }

        public ClaimCodeMappingBo FormBo(int createdById, int updatedById)
        {
            return new ClaimCodeMappingBo
            {
                MlreEventCode = MlreEventCode,
                MlreBenefitCode = MlreBenefitCode,
                ClaimCodeId = ClaimCodeId,
                ClaimCodeBo = ClaimCodeService.Find(ClaimCodeId),

                CreatedById = createdById,
                UpdatedById = updatedById,
            };
        }

        public static Expression<Func<ClaimCodeMapping, ClaimCodeMappingViewModel>> Expression()
        {
            return entity => new ClaimCodeMappingViewModel
            {
                Id = entity.Id,
                MlreEventCode = entity.MlreEventCode,
                MlreBenefitCode = entity.MlreBenefitCode,
                ClaimCodeId = entity.ClaimCodeId,
                ClaimCode = entity.ClaimCode,

                ClaimCodeMappingDetails = entity.ClaimCodeMappingDetails,
            };
        }
    }

    public class ValidateMlreEventCode : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value != null)
            {
                string mlreEventCode = value.ToString();
                string[] resultsArray = mlreEventCode.ToArraySplitTrim();
                IList<EventCodeBo> eventCodeBos = EventCodeService.Get();
                List<string> mlreEventCodes = new List<string>();
                if (eventCodeBos != null)
                {
                    foreach (EventCodeBo eventCodeBo in eventCodeBos)
                    {
                        mlreEventCodes.Add(eventCodeBo.Code);
                    }
                }
                foreach (string result in resultsArray)
                {
                    if (!mlreEventCodes.Contains(result))
                    {
                        return new ValidationResult("Please Enter Valid MLRe Event Code.");
                    }
                }
            }
            return ValidationResult.Success;
        }
    }
}