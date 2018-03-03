using DocGen.Templating.Rendering.Builders.V1;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace DocGen.Templating.Rendering.Instructions.V1
{
    public class DocumentInstructorV1 : IDocumentInstructor, IDocumentInstructor<IDocumentBuilderV1>
    {
        public int MarkupVersion => 1;

        public async Task InstructRenderingAsync(string markup, TemplateRenderModel model, IDocumentBuilderV1 renderer)
        {
            XDocument document = null;
            using (var sr = new StringReader(markup))
            {
                document = XDocument.Load(sr);
            }
            
            var context = new DocumentInstructionContextV1();
            await renderer.BeginWriteDocumentAsync(context);
            await renderer.EndWriteDocumentAsync(context);
        }
    }
}
