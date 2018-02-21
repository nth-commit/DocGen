using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace DocGen.Shared.Framework
{
    public interface IConfigurationProvider
    {
        IConfiguration Configuration { get; }
    }

    public static class IConfigurationProviderExtensions
    {
        public static string GetConnectionString(
            this IConfigurationProvider configurationProvider,
            Type markerType) =>
                configurationProvider.Configuration.GetConnectionString(markerType.Namespace);
    }
}
