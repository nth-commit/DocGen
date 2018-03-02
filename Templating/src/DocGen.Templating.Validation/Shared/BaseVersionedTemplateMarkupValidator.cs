using MoreLinq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Schema;

namespace DocGen.Templating.Validation.Shared
{
    public abstract class BaseVersionedTemplateMarkupValidator : IVersionedTemplateMarkupValidator
    {
        private readonly ISchemaFileLocator _schemaFileLocator;

        public BaseVersionedTemplateMarkupValidator(
            ISchemaFileLocator schemaFileLocator)
        {
            _schemaFileLocator = schemaFileLocator;
        }

        public void Validate(string markup, IEnumerable<ReferenceDefinition> references)
        {
            var document = GetSchemaValidatedDocument(markup);
            ValidateExpressions(document, references);
        }

        private XDocument GetSchemaValidatedDocument(string markup)
        {
            XNamespace markupNs = $"http://tempuri.org/markup{MarkupVersion}.xsd";

            var schemaPath = _schemaFileLocator.GetSchemaPath(MarkupVersion);
            using (var schemaXmlReader = XmlReader.Create(File.OpenRead(schemaPath)))
            {
                var settings = new XmlReaderSettings();
                settings.Schemas.Add(markupNs.ToString(), schemaXmlReader);
                settings.Schemas.Compile();

                settings.ValidationType = ValidationType.Schema;
                settings.ValidationFlags |= XmlSchemaValidationFlags.ProcessInlineSchema;
                settings.ValidationFlags |= XmlSchemaValidationFlags.ProcessSchemaLocation;
                settings.ValidationFlags |= XmlSchemaValidationFlags.ReportValidationWarnings;

                var errors = new List<TemplateSyntaxError>();
                settings.ValidationEventHandler += (object sender, ValidationEventArgs e) =>
                {
                    var lineInfo = (IXmlLineInfo)sender;
                    errors.Add(new TemplateSyntaxError()
                    {
                        Message = e.Message,
                        LineNumber = lineInfo.LineNumber,
                        LinePosition = lineInfo.LinePosition,
                        Code = TemplateSyntaxErrorCode.InvalidSchema,
                        Level = TemplateSyntaxErrorLevel.Error
                    });
                };

                using (var markupReader = new StringReader(markup))
                using (var markupXmlReader = XmlReader.Create(markupReader))
                {
                    // TODO: Handle errors when loading this doc (malformed XML)
                    // Create a temporary document and add the namespace to the root
                    var tempDocument = XDocument.Load(markupXmlReader, LoadOptions.SetLineInfo);
                    foreach (var element in tempDocument.Descendants())
                    {
                        element.Name = markupNs + element.Name.ToString();
                    }

                    string tempDocumentText = null;
                    using (var tempDocumentStringWriter = new StringWriter())
                    using (var tempDocumentTextWriter = XmlWriter.Create(tempDocumentStringWriter))
                    {
                        tempDocument.WriteTo(tempDocumentTextWriter);
                        tempDocumentTextWriter.Flush();
                        tempDocumentText = tempDocumentStringWriter.GetStringBuilder().ToString();
                    }

                    using (var textDocumentReader = new StringReader(tempDocumentText))
                    using (var tempDocumentXmlReader = XmlReader.Create(textDocumentReader, settings))
                    {
                        var document = XDocument.Load(tempDocumentXmlReader, LoadOptions.SetLineInfo);

                        if (errors.Any())
                        {
                            throw new InvalidTemplateSyntaxException(errors);
                        }

                        return document;
                    }
                }
            }
        }

        #region Abstract

        public abstract int MarkupVersion { get; }

        protected abstract void ValidateExpressions(XDocument document, IEnumerable<ReferenceDefinition> references);

        #endregion
    }
}
