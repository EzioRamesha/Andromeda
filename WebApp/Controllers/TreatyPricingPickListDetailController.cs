using BusinessObject;
using BusinessObject.TreatyPricing;
using Services;
using Services.TreatyPricing;
using Shared;
using Shared.DataAccess;
using Shared.Trails;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApp.Controllers
{
    public class TreatyPricingPickListDetailController : BaseController
    {
        public static void Save(int objectType, int objectId, int pickListId, string pickListDetailCodes, int authUserId, ref TrailObject trail)
        {
            List<string> savedCodes = new List<string>();

            if (string.IsNullOrEmpty(pickListDetailCodes))
                pickListDetailCodes = "";

            string[] codes = pickListDetailCodes.Split(',');
            foreach (string code in codes)
            {
                if (!TreatyPricingPickListDetailService.IsExists(objectType, objectId, pickListId, code))
                {
                    var bo = new TreatyPricingPickListDetailBo()
                    {
                        ObjectType = objectType,
                        ObjectId = objectId,
                        PickListId = pickListId,
                        PickListDetailCode = code,
                        CreatedById = authUserId
                    };

                    TreatyPricingPickListDetailService.Create(ref bo, ref trail);
                }

                savedCodes.Add(code);
            }

            TreatyPricingPickListDetailService.DeleteByObjectPickListExcept(objectType, objectId, pickListId, savedCodes, ref trail);
        }
    }

    public class ValidateMultiplePickListDetailCode : ValidationAttribute
    {
        public int PickListId { get; set; }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value != null)
            {
                string pickListDetailCode = value.ToString();
                string[] codes = pickListDetailCode.ToArraySplitTrim();
                IList<PickListDetailBo> bos = PickListDetailService.GetByPickListId(PickListId);
                IList<string> pickListDetailCodes = bos.Select(bo => bo.ToString()).ToList();
                List<string> invalidCodes = new List<string>();

                foreach (string code in codes)
                {
                    if (!pickListDetailCodes.Contains(code))
                    {
                        invalidCodes.Add(code);
                    }
                }

                if (invalidCodes.Count() == 0)
                    return ValidationResult.Success;

                string joinedInvalidCodes = string.Join(", ", invalidCodes);
                string error = string.Format("The value(s) '{0}' are invalid", joinedInvalidCodes);

                return new ValidationResult(error, new[] { validationContext.MemberName });
            }
            return ValidationResult.Success;
        }
    }
}