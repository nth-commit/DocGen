using System;
using System.Collections.Generic;
using System.Text;

namespace DocGen.Templating.Validation
{
    public class ReferenceDefinition
    {
        public string Name { get; private set; }

        public ReferenceDefinitionType Type { get; private set; }

        public object ValueRestriction { get; private set; }

        public ReferenceValueRestrictionType ValueRestrictionType { get; private set; }

        public static ReferenceDefinition String(string referenceName)
        {
            return new ReferenceDefinition()
            {
                Name = referenceName,
                Type = ReferenceDefinitionType.String,
                ValueRestriction = ReferenceValueRestrictionType.None
            };
        }

        public static ReferenceDefinition StringFrom(string referenceName, IEnumerable<string> values)
        {
            return new ReferenceDefinition()
            {
                Name = referenceName,
                Type = ReferenceDefinitionType.String,
                ValueRestriction = values,
                ValueRestrictionType = ReferenceValueRestrictionType.Included
            };
        }

        public static ReferenceDefinition StringNotFrom(string referenceName, IEnumerable<string> values)
        {
            return new ReferenceDefinition()
            {
                Name = referenceName,
                Type = ReferenceDefinitionType.String,
                ValueRestriction = values,
                ValueRestrictionType = ReferenceValueRestrictionType.Excluded
            };
        }

        public static ReferenceDefinition Boolean(string referenceName)
        {
            return new ReferenceDefinition()
            {
                Name = referenceName,
                Type = ReferenceDefinitionType.Boolean,
                ValueRestrictionType = ReferenceValueRestrictionType.None
            };
        }
    }
}
