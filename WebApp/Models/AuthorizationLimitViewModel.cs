using BusinessObject;
using DataAccess.Entities;
using Shared;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using System.Web;

namespace WebApp.Models
{
    public class AuthorizationLimitViewModel : IValidatableObject
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Access Group")]
        public int AccessGroupId { get; set; }

        public virtual AccessGroup AccessGroup { get; set; }

        [Display(Name = "Positive Amount From")]
        [Required, Range(0, double.MaxValue)]
        public double? PositiveAmountFrom { get; set; }

        [Display(Name = "Positive Amount To")]
        [Required, Range(0, double.MaxValue)]
        public double? PositiveAmountTo { get; set; }

        [Display(Name = "Negative Amount From")]
        [Required, Range(double.MinValue, 0)]
        public double? NegativeAmountFrom { get; set; }

        [Display(Name = "Negative Amount To")]
        [Required, Range(double.MinValue, 0)]
        public double? NegativeAmountTo { get; set; }

        [Display(Name = "Percentage")]
        [Required]
        public double? Percentage { get; set; }

        public AuthorizationLimitViewModel() { }

        public AuthorizationLimitViewModel(AuthorizationLimitBo authorizationLimitBo)
        {
            Set(authorizationLimitBo);
        }

        public void Set(AuthorizationLimitBo authorizationLimitBo)
        {
            if (authorizationLimitBo != null)
            {
                Id = authorizationLimitBo.Id;
                AccessGroupId = authorizationLimitBo.AccessGroupId;
                PositiveAmountFrom = authorizationLimitBo.PositiveAmountFrom;
                PositiveAmountTo = authorizationLimitBo.PositiveAmountTo;
                NegativeAmountFrom = authorizationLimitBo.NegativeAmountFrom;
                NegativeAmountTo = authorizationLimitBo.NegativeAmountTo;
                Percentage = authorizationLimitBo.Percentage;
            }
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            List<ValidationResult> results = new List<ValidationResult>();

            if (PositiveAmountFrom > PositiveAmountTo)
            {
                results.Add(new ValidationResult(
                string.Format(MessageBag.GreaterThan, "Positive Amount To", "Positive Amount From"),
                new[] { nameof(PositiveAmountTo) }));
            }
            
            if (NegativeAmountFrom > NegativeAmountTo)
            {
                results.Add(new ValidationResult(
                string.Format(MessageBag.GreaterThan, "Negative Amount To", "Negative Amount From"),
                new[] { nameof(NegativeAmountTo) }));
            }

            return results;
        }

        public static Expression<Func<AuthorizationLimit, AuthorizationLimitViewModel>> Expression()
        {
            return entity => new AuthorizationLimitViewModel
            {
                Id = entity.Id,
                PositiveAmountFrom = entity.PositiveAmountFrom,
                PositiveAmountTo = entity.PositiveAmountTo,
                NegativeAmountFrom = entity.NegativeAmountFrom,
                NegativeAmountTo = entity.NegativeAmountTo,
                Percentage = entity.Percentage,

                AccessGroupId = entity.AccessGroupId,
                AccessGroup = entity.AccessGroup,
            };
        }
    }
}
