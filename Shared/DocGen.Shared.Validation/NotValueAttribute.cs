using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DocGen.Shared.Validation
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public class NotValueAttribute : ValidationAttribute
    {
        public object Value { get; set; }

        public NotValueAttribute(object value)
        {
            Value = value;
        }

        public override bool IsValid(object value)
        {
            if (value == null)
            {
                // Assume this is Nullable with the value null, which is fine because Nullable implies not-required
                return true;
            }

            if (!value.GetType().IsValueType)
            {
                throw new Exception("Not supported");
            }

            var isValue = Value.Equals(value); // Using == will invoke object.Equals()
            return !isValue;
        }
    }
}
