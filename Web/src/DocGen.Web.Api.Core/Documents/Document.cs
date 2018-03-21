using System;
using System.Collections.Generic;
using System.Text;

namespace DocGen.Web.Api.Core.Documents
{
    public class Document
    {
        public string Id { get; set; }

        public string TemplateId { get; set; }

        public string TemplateVersion { get; set; }

        public IDictionary<string, IDictionary<string, object>> Values { get; set; }

        public DateTime Created { get; set; }

        public string CreatedByIpAddress { get; set; }
    }
}
