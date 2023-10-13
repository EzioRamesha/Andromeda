using BusinessObject.Retrocession;
using DataAccess.Entities;
using DataAccess.Entities.Retrocession;
using Shared;
using Shared.Forms.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace WebApp.Models
{
    public class RetroTreatyViewModel : IValidatableObject
    {
        public int Id { get; set; }

        [Required, DisplayName("Retro Party")]
        public int? RetroPartyId { get; set; }

        public RetroParty RetroParty { get; set; }

        public string RetroPartyCode { get; set; }

        [Required, DisplayName("Status")]
        public int Status { get; set; }

        //[Required, MaxLength(50), DisplayName("Retro Party")]
        //public string Party { get; set; }

        [Required, MaxLength(50), DisplayName("Retro Treaty Code")]
        public string Code { get; set; }

        [Required, DisplayName("Retro Treaty Type")]
        public int TreatyTypePickListDetailId { get; set; }

        public PickListDetail TreatyTypePickListDetail { get; set; }

        public string TreatyType { get; set; }

        [DisplayName("Automatic")]
        public bool IsLobAutomatic { get; set; }

        [DisplayName("Facultative")]
        public bool IsLobFacultative { get; set; }

        [DisplayName("Advantage Program")]
        public bool IsLobAdvantageProgram { get; set; }

        public double? RetroShare { get; set; }

        [Required, ValidateDouble, DisplayName("Retro Share (%)")]
        public string RetroShareStr { get; set; }

        [DisplayName("Retro Discount Table")]
        public int? TreatyDiscountTableId { get; set; }

        public TreatyDiscountTable TreatyDiscountTable { get; set; }

        public string TreatyDiscountTableRule { get; set; }

        public DateTime? EffectiveStartDate { get; set; }

        [Required, ValidateDate, DisplayName("Retro Treaty Effective Start Date")]
        public string EffectiveStartDateStr { get; set; }

        public DateTime? EffectiveEndDate { get; set; }

        [Required, ValidateDate, DisplayName("Retro Treaty Effective End Date")]
        public string EffectiveEndDateStr { get; set; }

        public RetroTreatyViewModel() { }

        public RetroTreatyViewModel(RetroTreatyBo retroTreatyBo)
        {
            Set(retroTreatyBo);
        }

        public void Set(RetroTreatyBo bo)
        {
            if (bo != null)
            {
                Id = bo.Id;
                RetroPartyId = bo.RetroPartyId;
                RetroPartyCode = bo.RetroPartyBo?.Code;
                Status = bo.Status;
                //Party = bo.Party;
                Code = bo.Code;
                TreatyTypePickListDetailId = bo.TreatyTypePickListDetailId;
                TreatyType = bo.TreatyTypePickListDetailBo?.Code;
                IsLobAutomatic = bo.IsLobAutomatic;
                IsLobFacultative = bo.IsLobFacultative;
                IsLobAdvantageProgram = bo.IsLobAdvantageProgram;
                RetroShare = bo.RetroShare;
                RetroShareStr = Util.DoubleToString(bo.RetroShare);
                TreatyDiscountTableId = bo.TreatyDiscountTableId;
                TreatyDiscountTableRule = bo.TreatyDiscountTableBo?.Rule;
                EffectiveStartDate = bo.EffectiveStartDate;
                EffectiveStartDateStr = bo.EffectiveStartDate?.ToString(Util.GetDateFormat());
                EffectiveEndDate = bo.EffectiveEndDate;
                EffectiveEndDateStr = bo.EffectiveEndDate?.ToString(Util.GetDateFormat());
            }
            else
            {
                IsLobAutomatic = false;
                IsLobFacultative = false;
                IsLobAdvantageProgram = false;
            }
        }

        public RetroTreatyBo FormBo(int authUserId, RetroTreatyBo bo = null)
        {
            return new RetroTreatyBo
            {
                Id = bo != null ? bo.Id : 0,
                RetroPartyId = RetroPartyId,
                Status = Status,
                //Party = Party,
                Code = Code,
                TreatyTypePickListDetailId = TreatyTypePickListDetailId,
                IsLobAutomatic = IsLobAutomatic,
                IsLobFacultative = IsLobFacultative,
                IsLobAdvantageProgram = IsLobAdvantageProgram,
                RetroShare = Util.StringToDouble(RetroShareStr),
                TreatyDiscountTableId = TreatyDiscountTableId,
                EffectiveStartDate = Util.GetParseDateTime(EffectiveStartDateStr),
                EffectiveEndDate = Util.GetParseDateTime(EffectiveEndDateStr),
                CreatedById = bo != null ? bo.CreatedById : authUserId,
                UpdatedById = authUserId,
            };
        }

        public static Expression<Func<RetroTreaty, RetroTreatyViewModel>> Expression()
        {
            return entity => new RetroTreatyViewModel
            {
                Id = entity.Id,
                RetroPartyId = entity.RetroPartyId,
                RetroParty = entity.RetroParty,
                Status = entity.Status,
                //Party = entity.Party,
                Code = entity.Code,
                TreatyTypePickListDetailId = entity.TreatyTypePickListDetailId,
                TreatyTypePickListDetail = entity.TreatyTypePickListDetail,
                IsLobAutomatic = entity.IsLobAutomatic,
                IsLobFacultative = entity.IsLobFacultative,
                IsLobAdvantageProgram = entity.IsLobAdvantageProgram,
                RetroShare = entity.RetroShare,
                TreatyDiscountTableId = entity.TreatyDiscountTableId,
                TreatyDiscountTable = entity.TreatyDiscountTable,
                EffectiveStartDate = entity.EffectiveStartDate,
                EffectiveEndDate = entity.EffectiveEndDate,
            };
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            List<ValidationResult> results = new List<ValidationResult>();

            DateTime? start = Util.GetParseDateTime(EffectiveStartDateStr);
            DateTime? end = Util.GetParseDateTime(EffectiveEndDateStr);

            if (start.HasValue && end.HasValue && end <= start)
            {
                results.Add(new ValidationResult(string.Format(MessageBag.StartDateEarlier, "Retro Treaty Effective"), new[] { nameof(EffectiveStartDateStr) }));
                results.Add(new ValidationResult(string.Format(MessageBag.EndDateLater, "Retro Treaty Effective"), new[] { nameof(EffectiveEndDateStr) }));
            }

            return results;
        }
    }
}