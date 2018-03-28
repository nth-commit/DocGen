using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DocGen.Web.Api.Core.Signing
{
    public interface ISigningRequestRepository
    {
        Task CreateSigningRequestAsync(SigningRequest signingRequest);

        Task<bool> HasSigningRequestAsync(SigningRequest signingRequest);
    }
}
