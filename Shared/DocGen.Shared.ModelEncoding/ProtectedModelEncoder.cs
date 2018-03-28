using Microsoft.AspNetCore.DataProtection;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace DocGen.Shared.ModelEncoding
{
    public class ProtectedModelEncoder<T> : IModelEncoder<T>
    {
        private readonly IDataProtectionProvider _dataProtectionProvider;
        private readonly string _key;

        public ProtectedModelEncoder(
            IDataProtectionProvider dataProtectionProvider,
            string key)
        {
            _dataProtectionProvider = dataProtectionProvider;
            _key = key;
        }

        public string Encode(T decoded)
        {
            return GetDataProtector().Protect(JsonConvert.SerializeObject(decoded));
        }

        public T Decode(string encoded)
        {
            return JsonConvert.DeserializeObject<T>(GetDataProtector().Unprotect(encoded));
        }

        private IDataProtector GetDataProtector() => _dataProtectionProvider.CreateProtector(_key);
    }
}
