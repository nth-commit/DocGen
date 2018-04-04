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

        public bool HasContent { get; set; }

        public bool IsPreviousSiblingBlockLike => !IsFirstChild && BlockLikeElements.Contains(Previous);

        public IEnumerable<int> ListItemIndexPath { get; private set; } = Enumerable.Empty<int>();

        public DocumentInstructionContextV1 BeforeBeginListItem(int index, bool hasChildren)
        {
            var other = BeforeBegin("list-item", hasChildren);
            other.ListItemIndexPath = ListItemIndexPath.Concat(index);
            return other;
        }

        public DocumentInstructionContextV1 AfterEndListItem()
        {
            var other = AfterEnd();
            other.ListItemIndexPath = ListItemIndexPath.Take(ListItemIndexPath.Count() - 1);
            return other;
        }

        public DocumentInstructionContextV1 BeforeBegin(string element, bool hasContent) => new DocumentInstructionContextV1()
        {
            Path = Path.Concat(element),
            ListItemIndexPath = ListItemIndexPath,
            Previous = Previous,
            HasContent = hasContent
        };

        public DocumentInstructionContextV1 AfterBegin() => new DocumentInstructionContextV1()
        {
            Path = Path,
            ListItemIndexPath = ListItemIndexPath,
            Previous = null,
            HasContent = HasContent
        };

        public DocumentInstructionContextV1 BeforeEnd() => this;

        public DocumentInstructionContextV1 AfterEnd() => new DocumentInstructionContextV1()
        {
            Path = Path.Take(Path.Count() - 1),
            ListItemIndexPath = ListItemIndexPath,
            Previous = Path.Last(),
            HasContent = HasContent
        };
    }
}
