using System;
using System.Collections.Generic;
using System.Text;

namespace DocGen.Shared.Validation
{
    public interface IValidateAgainstType
    {
        Type Type { get; }

        bool IgnoreCase { get; }

        bool AllowExtraProperties { get; } // TODO: Invalid if extra properties exist on the model
    }
}
