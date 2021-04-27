using System.ComponentModel.DataAnnotations;
using evogmtool.Models;
using static evogmtool.Constants;

namespace evogmtool.Attributes
{
    public class RoleAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (Role.IsDefined(value.ToString()))
            {
                return ValidationResult.Success;
            }
            else
            {
                return new ValidationResult("undefined");
            }
        }
    }
}
