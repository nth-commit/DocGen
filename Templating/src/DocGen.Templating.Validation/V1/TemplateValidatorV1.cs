using DocGen.Templating.Validation.Shared;
using MoreLinq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.Linq;

namespace DocGen.Templating.Validation.V1
{
    public class TemplateValidatorV1 : VersionedTemplateValidator, IVersionedTemplateValidator
    {
        public TemplateValidatorV1(ISchemaFileLocator schemaFileLocator) : base(schemaFileLocator) { }

        public override int MarkupVersion => 1;

        protected override void ValidateExpressions(XDocument document, IEnumerable<ReferenceDefinition> references)
        {
            var referencesByName = references.ToDictionary(r => r.Name);

            var referenceExpressions = new Dictionary<string, IList<LineInfo>>();
            var ifExpressions = new Dictionary<string, IList<LineInfo>>();

            document
               .Descendants()
               .ForEach(element =>
               {
                   var elementReferenceExpressions = GetReferenceExpressions(element);
                   foreach (var referenceExpression in elementReferenceExpressions)
                   {
                       if (!referenceExpressions.TryGetValue(referenceExpression, out IList<LineInfo> occurences))
                       {
                           occurences = new List<LineInfo>();
                           referenceExpressions.Add(referenceExpression, occurences);
                       }

                       var elementLineInfo = (IXmlLineInfo)element;
                       occurences.Add(new LineInfo()
                       {
                           LineNumber = elementLineInfo.LineNumber,
                           LinePosition = elementLineInfo.LinePosition + element.Name.LocalName.Length + ">".Length
                       });
                   }

                   var ifAttribute = element.Attributes().SingleOrDefault(a => a.Name == "if");
                   if (ifAttribute != null)
                   {
                       var expression = ifAttribute.Value;
                       if (!ifExpressions.TryGetValue(expression, out IList<LineInfo> occurences))
                       {
                           occurences = new List<LineInfo>();
                           ifExpressions.Add(expression, occurences);
                       }

                       var elementLineInfo = (IXmlLineInfo)element;
                       var attributeLineInfo = (IXmlLineInfo)ifAttribute;
                       occurences.Add(new LineInfo()
                       {
                           LineNumber = elementLineInfo.LineNumber,
                           LinePosition = attributeLineInfo.LinePosition + ifAttribute.Name.LocalName.Length + "\"=".Length
                       });
                   }
               });

            var errors = new List<TemplateSyntaxError>();

            var usedReferences = ValidateDataExpressions(errors, referenceExpressions, referencesByName);
            var usedReferencesFromIf = ValidateIfExpressions(errors, ifExpressions, referencesByName);

            var unusedReferenceNames = referencesByName.Keys.Except(usedReferences.Union(usedReferencesFromIf));
            errors.AddRange(unusedReferenceNames.Select(r => new TemplateSyntaxError
            {
                Message = $"Unused reference: \"{r}\"",
                Code = TemplateSyntaxErrorCode.UnusedReference,
                Level = TemplateSyntaxErrorLevel.Warning,
                LineNumber = -1
            }));

            if (errors.Any())
            {
                throw new InvalidTemplateSyntaxException(errors);
            }
        }

        private IEnumerable<string> GetReferenceExpressions(XElement element)
        {
            if (element.Name.LocalName == "data")
            {
                return new List<string>() { ((XText)element.FirstNode).Value };
            }
            else if (element.Name.LocalName == "signature")
            {
                var result = new List<string>();

                var signerAttribute = element.Attributes().Single(a => a.Name == "signer");
                result.Add(signerAttribute.Value);

                var representingAttribute = element.Attributes().FirstOrDefault(a => a.Name == "representing");
                if (representingAttribute != null)
                {
                    result.Add(representingAttribute.Value);
                }

                return result;
            }
            else
            {
                return Enumerable.Empty<string>();
            }
        }

        private HashSet<string> ValidateDataExpressions(List<TemplateSyntaxError> errors, Dictionary<string, IList<LineInfo>> dataExpressions, IDictionary<string, ReferenceDefinition> referencesByName)
        {
            var (validReferences, invalidReferences) = dataExpressions
                .Fork(expr => IsValidReference(expr.Key, referencesByName));

            errors.AddRange(invalidReferences.SelectMany(expr => expr.Value
                .Select(lineInfo => GetUnknownReferenceError(lineInfo, expr.Key))));

            return validReferences.Select(expr => expr.Key).ToHashSet();
        }

        private HashSet<string> ValidateIfExpressions(List<TemplateSyntaxError> errors, Dictionary<string, IList<LineInfo>> ifExpressions, IDictionary<string, ReferenceDefinition> referencesByName)
        {
            var ifExpressionsSplit = ifExpressions.ToDictionary(
                kvp => kvp.Key,
                kvp =>
                {
                    var split = kvp.Key.Split('=').Select(s => s.Trim()).ToList();
                    return new
                    {
                        Lhs = split[0], // Expect reference
                        Rhs = split[1] // Expect value
                    };
                });

            var (validIfReferences, invalidIfReferences) = ifExpressions
                .Fork(expr => IsValidReference(ifExpressionsSplit[expr.Key].Lhs, referencesByName));

            errors.AddRange(invalidIfReferences
                .SelectMany(expr => expr.Value
                    .Select(lineInfo => GetUnknownReferenceError(lineInfo, expr.Key))));

            errors.AddRange(validIfReferences
                .Select(expr =>
                {
                    var ifExpressionSplit = ifExpressionsSplit[expr.Key];
                    var (isValid, invalidReason) = IsValidValue(ifExpressionSplit.Lhs, ifExpressionSplit.Rhs, referencesByName);
                    return new
                    {
                        IsValid = isValid,
                        InvalidReason = invalidReason,
                        Expression = expr
                    };
                })
                .Where(x => !x.IsValid)
                .SelectMany(x => x.Expression.Value
                    .Select(lineInfo => new TemplateSyntaxError()
                    {
                        Message = x.InvalidReason,
                        Code = TemplateSyntaxErrorCode.InvalidValue,
                        Level = TemplateSyntaxErrorLevel.Error,
                        LineNumber = lineInfo.LineNumber,
                        LinePosition = lineInfo.LinePosition,
                    })));

            errors.AddRange(validIfReferences
                .Select(expr => ifExpressionsSplit[expr.Key])
                .ToLookup(
                    ifExpressionSplit => ifExpressionSplit.Lhs,
                    ifExpressionSplit => ifExpressionSplit.Rhs)
                .SelectMany(g => GetUnusedValues(g.Key, g, referencesByName)
                .Select(unusedValue => new TemplateSyntaxError()
                {
                    Message = $"Unused value: \"{unusedValue}\"",
                    Code = TemplateSyntaxErrorCode.UnusedValue,
                    Level = TemplateSyntaxErrorLevel.Warning,
                    LineNumber = -1
                })));

            return validIfReferences.Select(expr => ifExpressionsSplit[expr.Key].Lhs).ToHashSet();
        }

        private bool IsValidReference(string reference, IDictionary<string, ReferenceDefinition> referencesByName)
        {
            return referencesByName.ContainsKey(reference);
        }

        private (bool isValid, string invalidReason) IsValidValue(string reference, string value, IDictionary<string, ReferenceDefinition> referencesByName)
        {
            var referenceDefinition = referencesByName[reference];

            if (referenceDefinition.Type == ReferenceDefinitionType.String)
            {
                if (referenceDefinition.ValueRestrictionType == ReferenceValueRestrictionType.None)
                {
                    // Should be valid here, else it would be invalid in the schema
                }
                else if (referenceDefinition.ValueRestrictionType == ReferenceValueRestrictionType.Included)
                {
                    var allowedValues = (IEnumerable<string>)referenceDefinition.ValueRestriction;
                    if (!allowedValues.Contains(value))
                    {
                        return (false, $"Expected value of the values {string.Join(", ", allowedValues.Select(v => $"\"{v}\""))}");
                    }
                }
                else if (referenceDefinition.ValueRestrictionType == ReferenceValueRestrictionType.Excluded)
                {
                    throw new NotImplementedException();
                }
                else
                {
                    throw new Exception($"Unknown {nameof(ReferenceValueRestrictionType)}");
                }
            }
            else if (referenceDefinition.Type == ReferenceDefinitionType.Boolean)
            {
                if (value != "true" && value != "false")
                {
                    return (false, $"Expected boolean value, got \"{value}\"");
                }
            }
            else
            {
                throw new Exception($"Unknown {nameof(ReferenceDefinitionType)}");
            }

            return (true, null);
        }

        private TemplateSyntaxError GetUnknownReferenceError(LineInfo lineInfo, string unknownReference)
        {
            return new TemplateSyntaxError()
            {
                Message = $"{unknownReference} is not a known reference",
                Code = TemplateSyntaxErrorCode.UnknownReference,
                Level = TemplateSyntaxErrorLevel.Error,
                LineNumber = lineInfo.LineNumber,
                LinePosition = lineInfo.LinePosition
            };
        }

        private IEnumerable<string> GetUnusedValues(string reference, IEnumerable<string> usedValues, IDictionary<string, ReferenceDefinition> referencesByName)
        {
            var referenceDefinition = referencesByName[reference];

            if (referenceDefinition.Type == ReferenceDefinitionType.String)
            {
                if (referenceDefinition.ValueRestrictionType == ReferenceValueRestrictionType.Included)
                {
                    var allowedValues = (IEnumerable<string>)referenceDefinition.ValueRestriction;
                    return allowedValues.Except(usedValues);
                }
            }

            return Enumerable.Empty<string>();
        }

        private class LineInfo
        {
            public int LineNumber { get; set; }

            public int LinePosition { get; set; }
        }
    }
}
