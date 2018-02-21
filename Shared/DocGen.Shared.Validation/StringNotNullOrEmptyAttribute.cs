using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocGen.Shared.Validation
{
    public class StringNotNullOrEmptyAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            return !string.IsNullOrEmpty((string)value);
        }
    }
}
