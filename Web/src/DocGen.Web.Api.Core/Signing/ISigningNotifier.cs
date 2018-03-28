using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DocGen.Web.Api.Core.Signing
{
    public interface ISigningNotifier
    {
        string NotificationTypeId { get; }

        Task<object> NotifyAsync(SigningRequest signingRequest);
    }
}
