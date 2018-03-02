using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DocGen.Templating.Rendering.Shared
{
    public interface IVersionedTemplateRenderer
    {
        int MarkupVersion { get; }
    }

    public interface IVersionedTemplateRenderer<T> : IVersionedTemplateRenderer
    {
        T Result { get; }
    }
}
