using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;

namespace System.Xml.Linq
{
    public static class XElementExtensions
    {
        public static bool HasContent(this XElement element)
            => !string.IsNullOrWhiteSpace(element.Value) || element.HasElements;
    }
}
