using System.ComponentModel.DataAnnotations;

namespace Shared.Forms.Attributes
{
    public class ValidateDollar : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value != null)
            {
                if (!double.TryParse(value.ToString(), out double _))
                {
                    return new ValidationResult("Amount Format must be in \" 0, 0.00 or 1,000.00 \"");
                }
            }
            return ValidationResult.Success;
        }
    }
}
