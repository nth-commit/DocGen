using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Configuration;

namespace DocGen.Shared.Framework.Impl
{
    public class ConfigurationProvider : IConfigurationProvider
    {
        public ConfigurationProvider(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; private set; }
    }
}
