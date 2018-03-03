using DocGen.Templating.Rendering.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DocGen.Templating.Rendering.Instructions
{
    public interface IDocumentInstructor
    {
        int MarkupVersion { get; }
    }

    public interface IDocumentInstructor<TBuilder> where TBuilder : IDocumentBuilder
    {
        Task InstructRenderingAsync(string markup, TemplateRenderModel model, TBuilder renderer);
    }
}
