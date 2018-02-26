using System;
using System.Collections.Generic;
using System.Text;

namespace DocGen.Api.Core.Templates
{
    public interface ITemplateComponent
    {
        string Name { get; set; }

        string Description { get; set; }

        TemplateComponentConditionType? ConditionType { get; set; }

        dynamic ConditionTypeData { get; set; }
    }
}
