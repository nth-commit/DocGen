using DocGen.Shared.ModelEncoding;
using System;
using System.Collections.Generic;
using System.Text;

namespace DocGen.Web.Shared.Signing
{
    public class SigningKeyEncoder : ISigningKeyEncoder
    {
        private readonly IModelEncoderFactory _modelEncoderFactory;

        public SigningKeyEncoder(
            IModelEncoderFactory modelEncoderFactory)
        {
            _modelEncoderFactory = modelEncoderFactory;
        }

        public string Encode(SigningKey signingKey)
        {
            return GetModelEncoder().Encode(signingKey);
        }

        public SigningKey Decode(string encoded)
        {
            return GetModelEncoder().Decode(encoded);
        }

        private IModelEncoder<SigningKey> GetModelEncoder() => _modelEncoderFactory.CreateModelEncoder<SigningKey>("SigningRequestKey");
    }
}
