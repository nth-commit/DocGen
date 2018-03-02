using System;
using System.Collections.Generic;
using System.Text;

namespace DocGen.Templating.Validation
{
    public interface ISchemaFileLocator
    {
        string GetSchemaPath(int markupVersion);
    }
}
