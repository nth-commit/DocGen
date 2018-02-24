using System;
using System.Collections.Generic;
using System.Text;

namespace DocGen.Shared.Framework
{
    public interface IRemoteIpAddressAccessor
    {
        string RemoteIpAddress { get; }
    }
}
