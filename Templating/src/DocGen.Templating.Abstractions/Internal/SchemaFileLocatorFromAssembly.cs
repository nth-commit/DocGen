using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace DocGen.Templating.Internal
{
    public class SchemaFileLocatorFromAssembly : SchemaFileLocator
    {
        public SchemaFileLocatorFromAssembly() : base(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location))
        {
        }
    }
}
