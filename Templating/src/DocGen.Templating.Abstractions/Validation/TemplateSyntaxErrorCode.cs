using System;
using System.Collections.Generic;
using System.Text;

namespace DocGen.Templating.Validation
{
    public enum TemplateSyntaxErrorCode
    {
        Unknown = 0,

        InvalidSchema,

        UnknownReference,

        InvalidValue,

        UnusedReference,

        UnusedValue
    }
}
