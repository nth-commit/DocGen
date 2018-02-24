using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocGen.Shared.Validation
{
    public class ClientModelValidationException : ClientValidationException
    {
        public ModelErrorDictionary ModelErrors { get; private set; }

        public ClientModelValidationException(string error, string member)
        {
            ModelErrors = new ModelErrorDictionary(error, member);
        }

        public ClientModelValidationException(ModelErrorDictionary modelErrors)
        {
            ModelErrors = modelErrors;
        }
    }
}
