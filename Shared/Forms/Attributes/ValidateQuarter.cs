using System.ComponentModel.DataAnnotations;

namespace Shared.Forms.Attributes
{
    public class ValidateQuarter : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value != null)
            {
                if (!Util.ValidateQuarter(value.ToString()))
                {
                    return new ValidationResult("Incorrect Quarter Format");
                }
            }
            return ValidationResult.Success;
        }
    }
}
