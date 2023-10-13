using BusinessObject;
using DataAccess.Entities;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;

namespace WebApp.Models
{
    public class ClaimCodeViewModel
    {
        public int Id { get; set; }

        [Required]
        [StringLength(30)]
        [DisplayName("Claim Code")]
        public string Code { get; set; }

        [Required]
        [StringLength(255)]
        public string Description { get; set; }

        [Required]
        public int Status { get; set; }

        public string Remarks { get; set; }

        public ClaimCodeViewModel() { }

        public ClaimCodeViewModel(ClaimCodeBo claimCodeBo)
        {
            if (claimCodeBo != null)
            {
                Id = claimCodeBo.Id;
                Code = claimCodeBo.Code;
                Status = claimCodeBo.Status;
                Description = claimCodeBo.Description;
            }
        }

        public static Expression<Func<ClaimCode, ClaimCodeViewModel>> Expression()
        {
            return entity => new ClaimCodeViewModel
            {
                Id = entity.Id,
                Code = entity.Code,
                Status = entity.Status,
                Description = entity.Description,
            };
        }
    }
}