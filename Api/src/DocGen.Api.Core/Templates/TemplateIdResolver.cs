using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DocGen.Api.Core.Templates
{
    public class TemplateIdResolver : ITemplateIdResolver
    {
        public static TemplateIdResolver Instance = new TemplateIdResolver(); // TODO: Remove and add MapWithServices overload to AutoMapper 

        public string ResolveStepId(TemplateStepCreate step) => ResolvePathId(step.ParentNames.Concat(step.Name));

        public string ResolveStepInputId(TemplateStepCreate step, TemplateStepInputCreate stepInput) => ResolvePathId(step.ParentNames.Concat(step.Name).Concat(stepInput.Name));

        public string ResolvePathId(IEnumerable<string> path) => string.Join(".", path.Select(n => n.ToLowerInvariant().Replace(' ', '_')));
    }
}
