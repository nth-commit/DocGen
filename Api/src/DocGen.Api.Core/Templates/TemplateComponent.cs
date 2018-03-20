using System;
using System.Collections.Generic;
using System.Text;

namespace DocGen.Api.Core.Templates
{
    public interface ITemplateComponent
    {
        string Name { get; set; }

        string Description { get; set; }

        IEnumerable<TemplateStepCondition> Conditions { get; set; }
    }
}
