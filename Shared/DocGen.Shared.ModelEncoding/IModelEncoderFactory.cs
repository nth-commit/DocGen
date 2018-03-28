using System;
using System.Collections.Generic;
using System.Text;

namespace DocGen.Shared.ModelEncoding
{
    public interface IModelEncoderFactory
    {
        IModelEncoder<T> CreateModelEncoder<T>(string key);
    }
}
