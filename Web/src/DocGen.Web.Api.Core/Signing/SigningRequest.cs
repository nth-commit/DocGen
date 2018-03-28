using System;
using System.Collections.Generic;
using System.Text;

namespace DocGen.Web.Api.Core.Signing
{
    public class SigningRequest
    {
        public Guid Nonce { get; set; }

        public string TemplateId { get; set; }

        public int TemplateVersion { get; set; }

        public Dictionary<string, dynamic> InputValues { get; set; }

        public IEnumerable<string> Signatories { get; set; }
    }
}
