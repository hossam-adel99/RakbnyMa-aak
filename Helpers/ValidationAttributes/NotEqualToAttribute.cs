using System.ComponentModel.DataAnnotations;

namespace RakbnyMa_aak.Helpers.ValidationAttributes
{
    public class NotEqualToAttribute : ValidationAttribute
    {
        private readonly string _comparisonProperty;

        public NotEqualToAttribute(string comparisonProperty)
        {
            _comparisonProperty = comparisonProperty;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var currentValue = value?.ToString();

            var property = validationContext.ObjectType.GetProperty(_comparisonProperty);
            if (property == null)
                throw new ArgumentException($"العنصر {_comparisonProperty} غير موجود.");

            var comparisonValue = property.GetValue(validationContext.ObjectInstance)?.ToString();

            if (currentValue == comparisonValue)
            {
                return new ValidationResult($"{validationContext.DisplayName} يجب أن يكون مختلفًا عن {_comparisonProperty}.");
            }

            return ValidationResult.Success;
        }
    }
}
