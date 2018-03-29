using System;
using System.Collections.Generic;
using System.Text;

namespace DocGen.Web.Shared.Signing
{
    public interface ISigningKeyEncoder
    {
        string Encode(SigningKey signingKey);

        SigningKey Decode(string encoded);
    }
}
