using DocGen.Templating.Validation.Shared;
using MoreLinq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.Linq;

namespace DocGen.Templating.Validation.V1
{
    public class TemplateMarkupValidatorV1 : BaseVersionedTemplateMarkupValidator, IVersionedTemplateMarkupValidator
    {
        public override int MarkupVersion => 1;

        protected override void ValidateExpressions(XDocument document, IEnumerable<ReferenceDefinition> references)
        {
            var referencesByName = references.ToDictionary(r => r.Name);

            var dataExpressions = new Dictionary<string, IList<LineInfo>>();
            var ifExpressions = new Dictionary<string, IList<LineInfo>>();

            document
               .Descendants()
               .ForEach(e =>
               {
                   if (e.Name.LocalName == "data")
                   {
                       var expression = ((XText)e.FirstNode).Value;
                       if (!dataExpressions.TryGetValue(expression, out IList<LineInfo> occurences))
                       {
                           occurences = new List<LineInfo>();
                           dataExpressions.Add(expression, occurences);
                       }
                       occurences.Add(new LineInfo(e));
                   }

                   var ifAttribute = e.Attributes().SingleOrDefault(a => a.Name == "if");
                   if (ifAttribute != null)
                   {
                       var expression = ifAttribute.Value;
                       if (!ifExpressions.TryGetValue(expression, out IList<LineInfo> occurences))
                       {
                           occurences = new List<LineInfo>();
                           ifExpressions.Add(expression, occurences);
                       }
                       occurences.Add(new LineInfo(e));
                   }
               });

            var errors = new List<TemplateSyntaxError>();

            var usedReferencesFromData = ValidateDataExpressions(errors, dataExpressions, referencesByName);
            var usedReferencesFromIf = ValidateIfExpressions(errors, ifExpressions, referencesByName);

            var unusedReferenceNames = referencesByName.Keys.Except(usedReferencesFromData.Union(usedReferencesFromIf));
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
                if (value != "true" || value != "false")
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

            public LineInfo(IXmlLineInfo xmlLineInfo)
            {
                LineNumber = xmlLineInfo.LineNumber;
                LinePosition = xmlLineInfo.LinePosition;
            }
        }
    }
}
