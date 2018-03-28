using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DocGen.Web.Api.Core.Signing
{
    public interface ISigningRequestRepository
    {
        Task<SigningRequest> GetSigningRequestAsync(string id);

        Task<SigningRequest> CreateSigningRequestAsync(SigningRequest signingRequest);
    }
}
