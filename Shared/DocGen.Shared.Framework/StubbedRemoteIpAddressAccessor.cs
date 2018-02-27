using System;
using System.Collections.Generic;
using System.Text;

namespace DocGen.Shared.Framework
{
    public class StubbedRemoteIpAddressAccessor : IRemoteIpAddressAccessor
    {
        public string RemoteIpAddress => "127.0.0.1";
    }
}
