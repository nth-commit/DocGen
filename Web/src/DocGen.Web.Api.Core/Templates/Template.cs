using DocGen.Shared.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace DocGen.Web.Api.Core.Templates
{
    public class Template
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public int Version { get; set; }

        public string Description { get; set; }

        public string Markup { get; set; }

        public int MarkupVersion { get; set; }

        public TemplateSigningType SigningType { get; set; }

        public IEnumerable<TemplateStep> Steps { get; set; }
    }

    public static class TemplateExtensions
    {
        public static TemplateStepInput GetInputById(this Template template, string id)
        {
            if (id == "document_signed")
            {
                return template.CreateDocumentSignedInput();
            }

            var stepsById = template.Steps.ToDictionary(t => t.Id);

            TemplateStep step;
            if (stepsById.TryGetValue(id, out step))
            {
                // The input ID is the step ID
                return step.Inputs.Single(i => string.IsNullOrEmpty(i.Key));
            }
            else
            {
                var idSplit = id.Split(Constants.TemplateComponentReferenceSeparator);
                var stepId = string.Join(Constants.TemplateComponentReferenceSeparator.ToString(), idSplit.TakeAllExceptLast());
                if (stepsById.TryGetValue(stepId, out step))
                {
                    return step.Inputs.Single(i => i.Key == idSplit.Last());
                }
                else
                {
                    throw new Exception("Step not found");
                }
            }
        }

        private static TemplateStepInput CreateDocumentSignedInput(this Template template)
        {
            return new TemplateStepInput()
            {

            };
        }
    }
}
