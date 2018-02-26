using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocGen.Shared.Framework.SlugBuilder
{
    public interface ISlugBuilderFactory
    {
        ISlugBuilder Create();
    }
}
