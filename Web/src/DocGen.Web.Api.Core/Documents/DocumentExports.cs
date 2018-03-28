using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DocGen.Web.Api.Core.Documents
{
    public class DocumentExports : Dictionary<string, string>
    {
    }

    public static class DocumentExportsExtensions
    {
        public static string GetSignatory(this DocumentExports exports, int id)
        {
            return exports
                .Where(kvp =>
                    kvp.Key.StartsWith("signatory_") &&
                    int.Parse(kvp.Key.Substring("signatory_".Length)) == id)
                .Select(kvp => kvp.Value)
                .FirstOrDefault();
        }

        public static IEnumerable<string> ListSignatories(this DocumentExports exports)
        {
            return exports
                .Where(kvp => kvp.Key.StartsWith("signatory_"))
                .Select(kvp => kvp.Value);
        }
    }
}
