using System;
using System.Collections.Generic;
using System.Text;

namespace DocGen.Web.Api.Core.Templates
{
    public interface ITemplateComponent
    {
        string Name { get; set; }

        string Description { get; set; }

        IEnumerable<TemplateStepCondition> Conditions { get; set; }
    }
}
