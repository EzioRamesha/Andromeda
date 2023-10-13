using System;
using System.ComponentModel.DataAnnotations;

namespace Shared.Forms.Attributes
{
    public class ValidateDate : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value != null)
            {
                if (!Util.TryParseDateTime(value.ToString(), out DateTime? _, out string _))
                {
                    return new ValidationResult(string.Format(MessageBag.InvalidDateFormatWithValue, value.ToString()));
                }
            }
            return ValidationResult.Success;
        }
    }
}
