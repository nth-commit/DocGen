using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;

namespace DocGen.Shared.Core.Dynamic
{
    public static class ExpandoObjectFactory
    {
        public static dynamic CreateDynamic(Dictionary<string, object> properties)
        {
            var result = new ExpandoObject() as IDictionary<string, object>;
            foreach (var kvp in properties)
            {
                result[kvp.Key] = kvp.Value;
            }
            return result;
        }
    }
}
