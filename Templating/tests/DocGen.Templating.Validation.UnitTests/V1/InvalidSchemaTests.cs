using DocGen.Templating.Validation.V1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace DocGen.Templating.Validation.UnitTests.V1
{
    public class InvalidSchemaTests
    {
        [Fact]
        public void TestInvalidSchema_NoDocumentNode()
        {
            RunValidation(
                nameof(TestInvalidSchema_NoDocumentNode),
                Enumerable.Empty<ReferenceDefinition>(),
                new TemplateSyntaxError[]
                {
                    new TemplateSyntaxError()
                    {
                        Code = TemplateSyntaxErrorCode.InvalidSchema,
                        LineNumber = 1,
                        LinePosition = 41
                    }
                });
        }

        private static void RunValidation(
            string testName,
            IEnumerable<ReferenceDefinition> references,
            IEnumerable<TemplateSyntaxError> expectedErrors)
        {
            Utility.RunValidation<TemplateMarkupValidatorV1>(1, testName, references, expectedErrors);
        }
    }
}
