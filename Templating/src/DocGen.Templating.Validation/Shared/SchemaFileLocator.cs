using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace DocGen.Templating.Validation
{
    public class SchemaFileLocator : ISchemaFileLocator
    {
        private readonly string _basePath;

        public SchemaFileLocator(string basePath)
        {
            _basePath = basePath;
        }

        public string GetSchemaPath(int markupVersion) => Path.Combine(_basePath, $"V{markupVersion}", $"markup{markupVersion}.xsd");
    }
}
