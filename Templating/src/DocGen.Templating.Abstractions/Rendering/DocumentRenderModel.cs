using System;
using System.Collections.Generic;
using System.Text;

namespace DocGen.Templating.Rendering
{
    public class DocumentRenderModel
    {
        public IEnumerable<DocumentRenderModelItem> Items { get; set; }

        public bool Sign { get; set; }

        /// <summary>
        /// A map of input value ID's, to well-known document properties.
        /// </summary>
        public Dictionary<string, IEnumerable<string>> Exports => throw new NotImplementedException();
    }
}
