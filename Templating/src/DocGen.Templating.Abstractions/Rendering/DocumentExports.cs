using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DocGen.Templating.Rendering
{
    /// <summary>
    /// A map of input value ID's, to well-known document properties.
    /// </summary>
    public class DocumentExports : Dictionary<string, string>
    {
    }

    public static class DocumentExportsExtensions
    {
        public static DocumentSignatory GetSignatory(this DocumentExports exports, string id)
        {
            return exports.ListSignatories().FirstOrDefault(s => s.Id == id);
        }

        public static IEnumerable<DocumentSignatory> ListSignatories(this DocumentExports exports)
        {
            return exports
                .Where(kvp => kvp.Key.StartsWith("signatory_"))
                .ToLookup(
                    kvp => kvp.Key.Split('.')[0])
                .Select(g => new DocumentSignatory()
                {
                    Id = g.Where(kvp => kvp.Key.Split('.')[1] == "id").Select(kvp => kvp.Value).FirstOrDefault(),
                    Name = g.Where(kvp => kvp.Key.Split('.')[1] == "name").Select(kvp => kvp.Value).FirstOrDefault(),
                });
        }
    }

    public class DocumentSignatory
    {
        public string Id { get; set; }

        public string Name { get; set; }
    }
}
