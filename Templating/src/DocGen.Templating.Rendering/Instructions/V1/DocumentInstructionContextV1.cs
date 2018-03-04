using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DocGen.Templating.Rendering.Instructions.V1
{
    public class DocumentInstructionContextV1
    {
        public IEnumerable<string> Path { get; private set; } = Enumerable.Empty<string>();

        public string Previous { get; private set; }

        public string Current => Path.LastOrDefault();


        public DocumentInstructionContextV1 BeforeBegin(string element) => new DocumentInstructionContextV1()
        {
            Path = Path.Concat(element),
            Previous = Previous
        };

        public DocumentInstructionContextV1 AfterBegin() => new DocumentInstructionContextV1()
        {
            Path = Path,
            Previous = null
        };

        public DocumentInstructionContextV1 BeforeEnd() => this;

        public DocumentInstructionContextV1 AfterEnd() => new DocumentInstructionContextV1()
        {
            Path = Path.Take(Path.Count() - 1),
            Previous = Path.Last()
        };
    }
}
