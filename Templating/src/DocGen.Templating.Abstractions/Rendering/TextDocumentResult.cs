using System;
using System.Collections.Generic;
using System.Text;

namespace DocGen.Templating.Rendering
{
    public class TextDocumentResult
    {
        public string Body { get; set; }

        public IEnumerable<int> PageLocations { get; set; }
    }
}
