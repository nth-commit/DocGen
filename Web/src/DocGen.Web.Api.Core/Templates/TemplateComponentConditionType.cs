using System;
using System.Collections.Generic;
using System.Text;

namespace DocGen.Web.Api.Core.Templates
{
    public enum TemplateComponentConditionType
    {
        Unknown = 0,

        EqualsPreviousInputValue,

        IsDocumentSigned
    }
}
