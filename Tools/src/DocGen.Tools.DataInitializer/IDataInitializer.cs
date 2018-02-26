using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocGen.Tools.DataInitializer
{
    public interface IDataInitializer
    {
        Task RunAsync(ILogger logger);
    }
}
