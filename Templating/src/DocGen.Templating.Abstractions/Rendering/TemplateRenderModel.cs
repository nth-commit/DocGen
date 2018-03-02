using System;
using System.Collections.Generic;
using System.Text;

namespace DocGen.Templating.Rendering
{
    public class TemplateRenderModel
    {
        public IEnumerable<TemplateRenderModelItem> Items { get; set; }
    }
}
