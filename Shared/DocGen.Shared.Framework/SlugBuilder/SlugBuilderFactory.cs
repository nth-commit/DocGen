using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocGen.Shared.Framework.SlugBuilder
{
    public class SlugBuilderFactory : ISlugBuilderFactory
    {
        private readonly int maxLength = 80;

        public SlugBuilderFactory()
        {
        }

        public SlugBuilderFactory(int maxLength)
        {
            this.maxLength = maxLength;
        }

        public ISlugBuilder Create()
        {
            return new SlugBuilder(this.maxLength);
        }
    }
}
