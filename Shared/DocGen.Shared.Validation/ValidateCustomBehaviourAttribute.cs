using System;
using System.Collections.Generic;
using System.Text;

namespace DocGen.Shared.Validation
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public class ValidateCustomBehaviourAttribute : Attribute
    {
    }
}
