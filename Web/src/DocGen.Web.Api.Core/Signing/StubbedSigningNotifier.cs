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

        public Task NotifyAsync(Dictionary<string, string> signingUrlsByEmail)
        {
            return Task.CompletedTask;
        }
    }
}
