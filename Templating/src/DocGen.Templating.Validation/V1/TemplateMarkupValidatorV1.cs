using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Schema;

namespace DocGen.Templating.Validation.V1
{
    public class TemplateMarkupValidatorV1 : ITemplateVersionedMarkupValidator
    {
        public int MarkupVersion => 1;

        public void Validate(string markup)
        {
            var document = GetSchemaValidatedDocument(markup);
        }

        private XDocument GetSchemaValidatedDocument(string markup)
        {
            XNamespace markupNs = $"http://tempuri.org/markup{MarkupVersion}.xsd";

            var assemblyLocation = Assembly.GetEntryAssembly().Location;
            var schemaPath = Path.Combine(Path.GetDirectoryName(assemblyLocation), $"V{MarkupVersion}", $"markup{MarkupVersion}.xsd");
            using (var schemaXmlReader = XmlReader.Create(File.OpenRead(schemaPath)))
            {
                var settings = new XmlReaderSettings();
                settings.Schemas.Add(markupNs.ToString(), schemaXmlReader);
                settings.Schemas.Compile();

                settings.ValidationType = ValidationType.Schema;
                settings.ValidationFlags |= XmlSchemaValidationFlags.ProcessInlineSchema;
                settings.ValidationFlags |= XmlSchemaValidationFlags.ProcessSchemaLocation;
                settings.ValidationFlags |= XmlSchemaValidationFlags.ReportValidationWarnings;

                settings.ValidationEventHandler += Settings_ValidationEventHandler;

                using (var markupReader = new StringReader(markup))
                using (var markupXmlReader = XmlReader.Create(markupReader))
                {
                    // Create a temporary document and add the namespace to the root
                    var tempDocument = XDocument.Load(markupXmlReader, LoadOptions.SetLineInfo);
                    tempDocument.Root.Name = markupNs + tempDocument.Root.Name.ToString();

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
                        var lineNumber = ((IXmlLineInfo)document).LineNumber;
                        return document;
                    }
                }
            }
        }

        private void Settings_ValidationEventHandler(object sender, ValidationEventArgs e)
        {
            throw new Exception(e.Message);
        }
    }
}
