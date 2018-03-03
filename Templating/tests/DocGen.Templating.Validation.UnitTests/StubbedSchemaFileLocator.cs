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

        public string GetSchemaPath(int markupVersion)
        {
            return _path;
        }
    }
}
