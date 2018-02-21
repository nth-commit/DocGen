using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DocGen.Shared.Validation
{
    public class NullIfAttribute : ValidationAttribute
    {
        public string PropertyName { get; set; }

        public object PropertyValue { get; set; }

        public NullIfAttribute(string propertyName, object propertyValue)
        {
            PropertyName = propertyName;
            PropertyValue = propertyValue;
        }

        public override bool RequiresValidationContext => true;

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var property = validationContext.ObjectType.GetProperty(PropertyName);
            if (!property.PropertyType.IsValueType)
            {
                throw new Exception("Not supported");
            }

            var actualPropertyValue = property.GetValue(validationContext.ObjectInstance);
            return PropertyValue.Equals(actualPropertyValue) && value != null ?
                new ValidationResult("Expected null value") :
                ValidationResult.Success;
        }
    }
}
