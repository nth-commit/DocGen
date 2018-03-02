using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DocGen.Templating.Rendering.Shared
{
    public interface IVersionedRenderingInstructor
    {
        int MarkupVersion { get; }
    }

    public interface IVersionedRenderingInstructor<TRenderer> where TRenderer : IVersionedTemplateRenderer
    {
        Task InstructRenderingAsync(string markup, TemplateRenderModel model, TRenderer renderer);
    }
}
