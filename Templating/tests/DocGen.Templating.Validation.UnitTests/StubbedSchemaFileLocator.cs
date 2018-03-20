using DocGen.Templating.Internal;
using System;
using System.Collections.Generic;
using System.Text;

namespace DocGen.Templating.Validation
{
    public class StubbedSchemaFileLocator : ISchemaFileLocator
    {
        private readonly string _path;

        public StubbedSchemaFileLocator(string path)
        {
            _path = path;
        }

        public string GetSchemaDirectory(int markupVersion)
        {
            throw new NotImplementedException();
        }

        public string GetSchemaPath(int markupVersion)
        {
            return _path;
        }
    }
}
