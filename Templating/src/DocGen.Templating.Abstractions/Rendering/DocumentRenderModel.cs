using System;
using System.Collections.Generic;
using System.Text;

namespace DocGen.Templating.Rendering
{
    public class DocumentRenderModel
    {
        public IEnumerable<DocumentRenderModelItem> Items { get; set; }

        public IDictionary<string, string> Options { get; set; } = new Dictionary<string, string>();
    }
}
