using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DocGen.Templating.Rendering.Builders
{
    public interface IDocumentBuilder
    {
        int MarkupVersion { get; }
    }

    public interface IDocumentBuilder<T> : IDocumentBuilder
    {
        T Result { get; }
    }
}
