using DocGen.Templating.Validation.V1;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Xunit;

namespace DocGen.Templating.Validation.UnitTests.V1
{
    public class UnknownReferenceTests
    {
        [Fact]
        public void TestUnknownReference_IfAttribute()
        {
            RunValidation(
                nameof(TestUnknownReference_IfAttribute),
                new ReferenceDefinition[]
                {
                    ReferenceDefinition.String("known_reference")
                },
                new TemplateSyntaxError[]
                {
                    new TemplateSyntaxError()
                    {
                        Code = TemplateSyntaxErrorCode.UnknownReference,
                        LineNumber = 3,
                        LinePosition = 16
                    }
                });
        }

        [Fact]
        public void TestUnknownReference_DataElement()
        {
            RunValidation(
                nameof(TestUnknownReference_DataElement),
                new ReferenceDefinition[]
                {
                    ReferenceDefinition.StringFrom("known_reference", new string[] { "person" })
                },
                new TemplateSyntaxError[]
                {
                    new TemplateSyntaxError()
                    {
                        Code = TemplateSyntaxErrorCode.UnknownReference,
                        LineNumber = 4,
                        LinePosition = 13
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
