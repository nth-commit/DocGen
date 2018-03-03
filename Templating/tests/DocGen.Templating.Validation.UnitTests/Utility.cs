using DocGen.Templating.Validation.Shared;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Xunit;

namespace DocGen.Templating.Validation
{
    public static class Utility
    {
        public static void RunValidation<TMarkupValidator>(
            int version,
            string testName,
            IEnumerable<ReferenceDefinition> references,
            IEnumerable<TemplateSyntaxError> expectedErrors) where TMarkupValidator : VersionedTemplateValidator
        {
            var validator = (VersionedTemplateValidator)Activator.CreateInstance(
                typeof(TMarkupValidator), new SchemaFileLocator(Directory.GetCurrentDirectory()));

            var markup = File.ReadAllText(Path.Combine(
                Directory.GetCurrentDirectory(),
                $"V{version}",
                "Templates",
                $"{testName}.xml"));

            try
            {
                validator.Validate(markup, references);
            }
            catch (InvalidTemplateSyntaxException ex)
            {
                Assert.Equal(expectedErrors, ex.Errors, new TemplateSyntaxErrorEqualityComparer());
            }
        }

        private class TemplateSyntaxErrorEqualityComparer : IEqualityComparer<TemplateSyntaxError>
        {
            public bool Equals(TemplateSyntaxError x, TemplateSyntaxError y)
            {
                return x.Code == y.Code &&
                    x.LineNumber == y.LineNumber &&
                    x.LinePosition == y.LinePosition;
            }

            public int GetHashCode(TemplateSyntaxError obj)
            {
                return 1;
            }
        }
    }
}
