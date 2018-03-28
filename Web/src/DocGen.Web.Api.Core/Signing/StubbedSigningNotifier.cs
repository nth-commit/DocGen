using Microsoft.AspNetCore.DataProtection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocGen.Web.Api.Core.Signing
{
    public class StubbedSigningNotifier : ISigningNotifier
    {
        private readonly IDataProtectionProvider _dataProtectionProvider;
        private readonly HostOptions _hostOptions;

        public StubbedSigningNotifier(
            IDataProtectionProvider dataProtectionProvider,
            IOptions<HostOptions> hostOptions)
        {
            _dataProtectionProvider = dataProtectionProvider;
            _hostOptions = hostOptions.Value;
        }

        public string NotificationTypeId => "stubbed";

        public Task<object> NotifyAsync(SigningRequest signingRequest)
        {
            if (string.IsNullOrEmpty(signingRequest.Id))
            {
                throw new ArgumentNullException($"{nameof(signingRequest)}.{nameof(SigningRequest.Id)}");
            }

            var dataProtector = _dataProtectionProvider.CreateProtector("SigningRequestKey");

            var signingRequestKeyByEmail = signingRequest.Signatories.ToDictionary(
                email => email,
                email => dataProtector.Protect($"{signingRequest.Id}:{email}"));

            var signingUrlByEmail = signingRequestKeyByEmail.ToDictionary(
                kvp => kvp.Key,
                kvp => $"{_hostOptions.Signing}/{kvp.Value}?v=1");

            return Task.FromResult((object)signingUrlByEmail);
        }
    }
}
