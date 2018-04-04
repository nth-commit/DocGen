using System;
using System.Collections.Generic;
using System.Text;

namespace DocGen.Templating.Rendering
{
    public class DocumentRenderModel
    {
        public IEnumerable<DocumentRenderModelItem> Items { get; set; }

        public bool Sign { get; set; }

        public DocumentExports Exports { get; set; }
    }
}
