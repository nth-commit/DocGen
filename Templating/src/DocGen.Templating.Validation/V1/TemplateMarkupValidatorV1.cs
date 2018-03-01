using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Xml;
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

        private XmlDocument GetSchemaValidatedDocument(string markup)
        {
            var assemblyLocation = Assembly.GetEntryAssembly().Location;
            var schemaPath = Path.Combine(Path.GetDirectoryName(assemblyLocation), $"V{MarkupVersion}", $"markup{MarkupVersion}.xsd");
            using (var schemaXmlReader = XmlReader.Create(File.OpenRead(schemaPath)))
            {
                var settings = new XmlReaderSettings();
                settings.Schemas.Add("http://tempuri.org/markup1.xsd", schemaXmlReader);
                settings.Schemas.Compile();

                settings.ValidationType = ValidationType.Schema;
                settings.ValidationFlags |= XmlSchemaValidationFlags.ProcessInlineSchema;
                settings.ValidationFlags |= XmlSchemaValidationFlags.ProcessSchemaLocation;
                settings.ValidationFlags |= XmlSchemaValidationFlags.ReportValidationWarnings;

                settings.ValidationEventHandler += Settings_ValidationEventHandler;

                // Need to append the namespace to the document and then read it again in order to validate it.
                using (var markupReader = new StringReader(markup))
                using (var markupXmlReader = XmlReader.Create(markupReader))
                {
                    var tempDocument = new XmlDocument();
                    tempDocument.Load(markupXmlReader);

                    var root = tempDocument.FirstChild;
                    var namespaceAttribute = tempDocument.CreateAttribute("xmlns"); //,"http://www.w3.org/2000/xmlns/");
                    namespaceAttribute.Value = $"http://tempuri.org/markup{MarkupVersion}.xsd";
                    root.Attributes.Append(namespaceAttribute);

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
                        var document = new XmlDocument();
                        document.Load(tempDocumentXmlReader);
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
