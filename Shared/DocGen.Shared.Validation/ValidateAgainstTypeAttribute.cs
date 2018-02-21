using System;
using System.Collections.Generic;
using System.Text;

namespace DocGen.Shared.Validation
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public class ValidateAgainstTypeAttribute : ValidateCustomBehaviourAttribute
    {
        public Type Type { get; private set; }

        public bool IgnoreCase { get; private set; }

        public bool AllowExtraProperties { get; private set; } // TODO: Invalid if extra properties exist on the model

        public ValidateAgainstTypeAttribute(Type type, bool ignoreCase = true, bool allowExtraProperties = false)
        {
            Type = type;
            IgnoreCase = ignoreCase;
            AllowExtraProperties = allowExtraProperties;
        }
    }
}
