using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DocGen.Shared.Validation
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public abstract class BaseIfAttribute : ValidationAttribute
    {
        public string TargetPropertyName { get; private set; }

        public object TargetPropertyValue { get; private set; }

        public BaseIfAttribute(string targetPropertyName, object targetPropertyValue)
        {
            TargetPropertyName = targetPropertyName;
            TargetPropertyValue = targetPropertyValue;
        }

        public bool IsConditionMet(object value, object objectInstance, Type objectType)
        {
            if (value == null)
            {
                return false;
            }

            var type = value.GetType();
            var targetProperty = objectType.GetProperty(TargetPropertyName);
            if (!targetProperty.PropertyType.IsValueType)
            {
                throw new Exception("Not supported");
            }

            var actualTargetPropertyValue = targetProperty.GetValue(objectInstance);
            return TargetPropertyValue.Equals(actualTargetPropertyValue);
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (IsConditionMet(value, validationContext.ObjectInstance, validationContext.ObjectType))
            {
                if (!IsValidWhenConditionMet(value))
                {
                    return new ValidationResult("Failed");
                }
            }

            return ValidationResult.Success;
        }

        protected abstract bool IsValidWhenConditionMet(object value);
    }
}
