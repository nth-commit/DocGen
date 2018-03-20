using System;
using System.Collections.Generic;
using System.Text;

namespace DocGen.Templating.Internal
{
    public interface ISchemaFileLocator
    {
        string GetSchemaDirectory(int markupVersion);

        string GetSchemaPath(int markupVersion);
    }
}
