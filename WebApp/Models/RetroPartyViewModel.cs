using BusinessObject;
using DataAccess.Entities;
using Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;

namespace WebApp.Models
{
    public class RetroPartyViewModel : IValidatableObject
    {
        public int Id { get; set; }

        [Required, MaxLength(50), DisplayName("Retro Party")]
        public string Party { get; set; }

        [Required, MaxLength(50), DisplayName("Party Code")]
        public string Code { get; set; }

        [Required, MaxLength(255), DisplayName("Retro Name")]
        public string Name { get; set; }

        [Required, DisplayName("Country Code")]
        public int CountryCodePickListDetailId { get; set; }

        public PickListDetail CountryCodePickListDetail { get; set; }

        public PickListDetailBo CountryCodePickListDetailBo { get; set; }

        public string CountryCode { get; set; }

        [DisplayName("Description")]
        public string Description { get; set; }

        [Required, DisplayName("Status")]
        public int Status { get; set; }

        [DisplayName("Direct Retro")]
        public bool IsDirectRetro { get; set; }

        [DisplayName("Per Life Retro")]
        public bool IsPerLifeRetro { get; set; }

        [DisplayName("Account Code")]
        public string AccountCode { get; set; }

        [DisplayName("Account Code Description")]
        public string AccountCodeDescription { get; set; }

        public RetroPartyViewModel() { }

        public RetroPartyViewModel(RetroPartyBo retroPartyBo)
        {
            Set(retroPartyBo);
        }

        public void Set(RetroPartyBo bo)
        {
            if (bo != null)
            {
                Id = bo.Id;
                Party = bo.Party?.Trim();
                Code = bo.Code?.Trim();
                Name = bo.Name?.Trim();
                CountryCodePickListDetailId = bo.CountryCodePickListDetailId;
                CountryCodePickListDetailBo = bo.CountryCodePickListDetailBo;
                Description = bo.Description?.Trim();
                Status = bo.Status;
                IsDirectRetro = bo.IsDirectRetro;
                IsPerLifeRetro = bo.IsPerLifeRetro;
                AccountCode = bo.AccountCode?.Trim();
                AccountCodeDescription = bo.AccountCodeDescription?.Trim();
            }
        }

        public RetroPartyBo FormBo(int createdById, int updatedById)
        {
            return new RetroPartyBo
            {
                Party = Party,
                Code = Code,
                Name = Name,
                CountryCodePickListDetailId = CountryCodePickListDetailId,
                CountryCodePickListDetailBo = PickListDetailService.Find(CountryCodePickListDetailId),
                Description = Description,
                Status = Status,
                IsDirectRetro = IsDirectRetro,
                IsPerLifeRetro = IsPerLifeRetro,
                AccountCode = AccountCode,
                AccountCodeDescription = AccountCodeDescription,
                CreatedById = createdById,
                UpdatedById = updatedById,
            };
        }

        public static Expression<Func<RetroParty, RetroPartyViewModel>> Expression()
        {
            return entity => new RetroPartyViewModel
            {
                Id = entity.Id,
                Party = entity.Party,
                Code = entity.Code,
                Name = entity.Name,
                CountryCodePickListDetailId = entity.CountryCodePickListDetailId,
                CountryCodePickListDetail = entity.CountryCodePickListDetail,
                Description = entity.Description,
                Status = entity.Status,
                IsDirectRetro = entity.IsDirectRetro,
                IsPerLifeRetro = entity.IsPerLifeRetro,
                AccountCode = entity.AccountCode,
                AccountCodeDescription = entity.AccountCodeDescription,
            };
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            List<ValidationResult> results = new List<ValidationResult>();

            if (!IsDirectRetro && !IsPerLifeRetro)
            {
                results.Add(new ValidationResult("At least 1 Type must be selected"));
            }

            return results;
        }
    }
}