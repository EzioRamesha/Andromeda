using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Forms.Attributes
{
    public class RequiredVersion : RequiredAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var currentVersion = validationContext.ObjectType.GetProperty("CurrentVersion");
            var editableVersion = validationContext.ObjectType.GetProperty("EditableVersion");

            var currentVersionValue = currentVersion.GetValue(validationContext.ObjectInstance, null);
            var editableVersionValue = editableVersion.GetValue(validationContext.ObjectInstance, null);

            if (currentVersionValue.ToString() != editableVersionValue.ToString())
                return ValidationResult.Success;

            return base.IsValid(value, validationContext);
        }
    }
}
