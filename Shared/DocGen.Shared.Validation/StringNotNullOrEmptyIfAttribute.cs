using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DocGen.Shared.Validation
{
    public class StringNotNullOrEmptyIfAttribute : ValidationAttribute
    {
        public string PropertyName { get; set; }

        public object PropertyValue { get; set; }

        public StringNotNullOrEmptyIfAttribute(string propertyName, object propertyValue)
        {
            PropertyName = propertyName;
            PropertyValue = propertyValue;
        }

        public override bool IsValid(object value)
        {
            // TODO: Validate me
            return base.IsValid(value);
        }
    }
}
