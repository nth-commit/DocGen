using System;
using System.Collections.Generic;
using System.Text;

namespace DocGen.Shared.ModelEncoding
{
    public interface IModelEncoder<T>
    {
        string Encode(T decoded);

        T Decode(string encoded);
    }
}
