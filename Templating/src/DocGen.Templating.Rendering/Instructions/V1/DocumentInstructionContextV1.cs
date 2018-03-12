using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DocGen.Templating.Rendering.Instructions.V1
{
    public class DocumentInstructionContextV1
    {
        private static readonly string[] BlockLikeElements = new string[] { "block", "list", "list-item" };

        public IEnumerable<string> Path { get; private set; } = Enumerable.Empty<string>();

        public string Parent => Path.Skip(Path.Count() - 2).First();

        public string Previous { get; private set; }

        public string Current => Path.LastOrDefault();

        public int ListNestingLevel => Path.Where(element => element == "list").Count() - 1;

        public bool IsFirstChild => Previous == null;

        public bool IsPreviousSiblingBlockLike => !IsFirstChild && BlockLikeElements.Contains(Previous);

        public IEnumerable<int> ListItemPath { get; private set; } = Enumerable.Empty<int>();

        public DocumentInstructionContextV1 BeforeBeginListItem(int index)
        {
            var other = BeforeBegin("list-item");
            other.ListItemPath = ListItemPath.Concat(index);
            return other;
        }

        public DocumentInstructionContextV1 AfterEndListItem()
        {
            var other = AfterEnd();
            other.ListItemPath = ListItemPath.Take(ListItemPath.Count() - 1);
            return other;
        }

        public DocumentInstructionContextV1 BeforeBegin(string element) => new DocumentInstructionContextV1()
        {
            Path = Path.Concat(element),
            ListItemPath = ListItemPath,
            Previous = Previous
        };

        public DocumentInstructionContextV1 AfterBegin() => new DocumentInstructionContextV1()
        {
            Path = Path,
            ListItemPath = ListItemPath,
            Previous = null
        };

        public DocumentInstructionContextV1 BeforeEnd() => this;

        public DocumentInstructionContextV1 AfterEnd() => new DocumentInstructionContextV1()
        {
            Path = Path.Take(Path.Count() - 1),
            ListItemPath = ListItemPath,
            Previous = Path.Last()
        };
    }
}
