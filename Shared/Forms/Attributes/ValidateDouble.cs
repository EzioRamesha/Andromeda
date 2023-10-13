using System.ComponentModel.DataAnnotations;

namespace Shared.Forms.Attributes
{
    public class ValidateDouble : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value != null)
            {
                if (!double.TryParse(value.ToString(), out double _))
                {
                    return new ValidationResult(string.Format("The value '{0}' is not valid for {1}.", value.ToString(), validationContext.DisplayName));
                }
            }
            return ValidationResult.Success;
        }
    }
}
