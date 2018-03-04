using System;
using System.Collections.Generic;
using System.Text;

namespace DocGen.Shared.Validation
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public class ValidateAgainstTypeIfAttribute : BaseIfAttribute, IValidateAgainstType
    {
        public Type Type { get; private set; }

        public bool IgnoreCase { get; private set; }

        public bool AllowExtraProperties { get; private set; } // TODO: Invalid if extra properties exist on the model

        public ValidateAgainstTypeIfAttribute(Type type, string propertyName, object propertyValue, bool ignoreCase = true, bool allowExtraProperties = false)
            : base(propertyName, propertyValue)
        {
            Type = type;
            IgnoreCase = ignoreCase;
            AllowExtraProperties = allowExtraProperties;
        }

        // Always valid; validation on property kicked off in another validator cycle.
        protected override bool IsValidWhenConditionMet(object value) => true;
    }
}
