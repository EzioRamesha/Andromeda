using BusinessObject;
using DataAccess.Entities;
using Services;
using Shared;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;

namespace WebApp.Models
{
    public class EventClaimCodeMappingViewModel
    {
        public int Id { get; set; }

        [ValidateCedantId]
        [Required, Display(Name = "Ceding Company")]
        public int CedantId { get; set; }

        [Display(Name = "Ceding Company")]
        public virtual Cedant Cedant { get; set; }

        [Display(Name = "Ceding Company")]
        public CedantBo CedantBo { get; set; }

        [Required, Display(Name = "MLRe Event Code")]
        public int EventCodeId { get; set; }

        public virtual EventCode EventCode { get; set; }

        [Display(Name = "MLRe Event Code")]
        public EventCodeBo EventCodeBo { get; set; }

        [RequiredIfEmpty, Display(Name = "Ceding Event Code")]
        public string CedingEventCode { get; set; }

        [RequiredIfEmpty, Display(Name = "Ceding Claim Type")]
        public string CedingClaimType { get; set; }

        public virtual ICollection<EventClaimCodeMappingDetail> EventClaimCodeMappingDetails { get; set; }

        public EventClaimCodeMappingViewModel() { }

        public EventClaimCodeMappingViewModel(EventClaimCodeMappingBo eventClaimCodeMappingBo)
        {
            if (eventClaimCodeMappingBo != null)
            {
                Id = eventClaimCodeMappingBo.Id;
                CedantId = eventClaimCodeMappingBo.CedantId;
                CedantBo = eventClaimCodeMappingBo.CedantBo;
                EventCodeId = eventClaimCodeMappingBo.EventCodeId;
                EventCodeBo = eventClaimCodeMappingBo.EventCodeBo;
                CedingEventCode = eventClaimCodeMappingBo.CedingEventCode;
                CedingClaimType = eventClaimCodeMappingBo.CedingClaimType;
            }
        }

        public static Expression<Func<EventClaimCodeMapping, EventClaimCodeMappingViewModel>> Expression()
        {
            return entity => new EventClaimCodeMappingViewModel
            {
                Id = entity.Id,
                CedantId = entity.CedantId,
                Cedant = entity.Cedant,
                EventCodeId = entity.EventCodeId,
                EventCode = entity.EventCode,
                CedingEventCode = entity.CedingEventCode,
                CedingClaimType = entity.CedingClaimType,

                EventClaimCodeMappingDetails = entity.EventClaimCodeMappingDetails,
            };
        }

        public EventClaimCodeMappingBo FormBo(int createdById, int updatedById)
        {
            var bo = new EventClaimCodeMappingBo
            {
                Id = Id,
                CedantId = CedantId,
                CedantBo = CedantService.Find(CedantId),
                EventCodeId = EventCodeId,
                EventCodeBo = EventCodeService.Find(EventCodeId),
                CedingEventCode = CedingEventCode,
                CedingClaimType = CedingClaimType,
                CreatedById = createdById,
                UpdatedById = updatedById,
            };
            return bo;
        }
    }

    public class RequiredIfEmpty : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var Model = validationContext.ObjectInstance as EventClaimCodeMappingViewModel;
            if (string.IsNullOrEmpty(Model.CedingEventCode) && string.IsNullOrEmpty(Model.CedingClaimType))
            {
                return new ValidationResult(string.Format(MessageBag.Required, "The Ceding Event Code"), new[] { nameof(Model.CedingEventCode) });
            }
            return null;
        }
    }
}