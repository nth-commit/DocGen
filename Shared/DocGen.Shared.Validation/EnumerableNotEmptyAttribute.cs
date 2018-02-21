using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocGen.Shared.Validation
{
    public class EnumerableNotEmptyAttribute : ValidationAttribute
    {
        public EnumerableNotEmptyAttribute() : base("Must have at least one element") { }

        public override bool IsValid(object value)
        {
            var enumerable = (IEnumerable)value;
            return enumerable != null && enumerable.OfType<dynamic>().Count() > 0;
        }
    }
}
