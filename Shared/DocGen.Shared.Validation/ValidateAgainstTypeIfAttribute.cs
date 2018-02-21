using System;
using System.Collections.Generic;
using System.Text;

namespace DocGen.Shared.Validation
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class ValidateAgainstTypeIfAttribute : ValidateAgainstTypeAttribute
    {
        public string PropertyName { get; set; }

        public object PropertyValue { get; set; }

        public ValidateAgainstTypeIfAttribute(Type type, string propertyName, object propertyValue) : base(type)
        {
            PropertyName = propertyName;
            PropertyValue = propertyValue;
        }
    }
}
