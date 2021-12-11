using System.ComponentModel.DataAnnotations;
using Deveel.Math;

namespace CelestialObjectCatalog.WebApi.Validation.Attributes
{
    public class IsBigDecimalAttribute : ValidationAttribute
    {
        protected override ValidationResult 
            IsValid(object value, ValidationContext validationContext)
        {
            //if value is null
            if (value is null)
            {
                return new ValidationResult("Value cannot be null");
            }

            //make the validations
            return BigDecimal.TryParse(value.ToString(), out _)
                ? ValidationResult.Success 
                : new ValidationResult($"Value:'{value}' is not a valid decimal number");
        }
    }
}
