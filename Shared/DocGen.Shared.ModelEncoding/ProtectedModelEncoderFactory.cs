using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace DocGen.Shared.ModelEncoding
{
    public class ProtectedModelEncoderFactory : IModelEncoderFactory
    {
        private readonly IServiceProvider _serviceProvider;

        public ProtectedModelEncoderFactory(
            IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public IModelEncoder<T> CreateModelEncoder<T>(string key)
        {
            return ActivatorUtilities.CreateInstance<ProtectedModelEncoder<T>>(_serviceProvider, key);
        }
    }
}
