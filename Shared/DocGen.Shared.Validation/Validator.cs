using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using SystemValidator = System.ComponentModel.DataAnnotations.Validator;

namespace DocGen.Shared.Validation
{
    public static class Validator
    {
        public static (bool isValid, ModelErrorDictionary modelErrors) IsValid(
           object model,
           IServiceProvider serviceProvider = null,
           IDictionary<object, object> items = null)
        {
            var validationContext = new ValidationContext(model, serviceProvider, items);
            var validationResults = new List<ValidationResult>();

            SystemValidator.TryValidateObject(model, validationContext, validationResults, validateAllProperties: true);
            var (resultsWithMember, resultsWithoutMember) = validationResults.Fork(v => v.MemberNames.Count() > 0);

            var modelErrors = new ModelErrorDictionary(
                resultsWithMember
                    .SelectMany(v => v.MemberNames.Select(m => (MemberName: m, ErrorMessage: v.ErrorMessage)))
                    .ToLookup(t => t.MemberName)
                    .ToDictionary(g => g.Key, g => g.Select(t => t.ErrorMessage)),
                resultsWithoutMember.Select(v => v.ErrorMessage));

            model.GetType().GetProperties().ForEach(pi =>
            {
                var attributes = pi.GetCustomAttributes(typeof(ValidateCustomBehaviourAttribute), false);
                attributes.ForEach(attribute =>
                {
                    if (attribute is ValidateEnumerableElementsAttribute)
                    {
                        ValidateEnumerableElements(model, serviceProvider, items, modelErrors, pi);
                        return;
                    }

                    var validateAgainstTypeIfAttribute = attribute as ValidateAgainstTypeIfAttribute;
                    if (validateAgainstTypeIfAttribute != null)
                    {
                        ValidateAgainstTypeIf(model, serviceProvider, items, modelErrors, pi, validateAgainstTypeIfAttribute);
                        return;
                    }

                    var validateAgainstTypeAttribute = attribute as ValidateAgainstTypeAttribute;
                    if (validateAgainstTypeAttribute != null)
                    {
                        ValidateAgainstType(model, serviceProvider, items, modelErrors, pi, validateAgainstTypeAttribute);
                        return;
                    }

                    throw new Exception("Unsupported ValidateCustomBehaviourAttribute");
                });
            });

            return (!modelErrors.HasErrors, modelErrors);
        }

        public static void Validate(object model, IServiceProvider serviceProvider = null, IDictionary<object, object> items = null)
        {
            var (isValid, modelErrors) = IsValid(model, serviceProvider, items);
            if (!isValid)
            {
                throw new ClientModelValidationException(modelErrors);
            }
        }

        public static void ValidateNotNull(object model, string paramName)
        {
            if (model == null)
            {
                throw new ClientArgumentNullException(paramName);
            }
        }

        public static void ValidateStringNotNullOrEmpty(string str, string paramName)
        {
            if (str == null)
            {
                throw new ClientArgumentNullException(paramName);
            }
            else if (str == string.Empty)
            {
                throw new ClientArgumentException(paramName);
            }
        }


        #region Helpers

        private static void ValidateEnumerableElements(
            object model,
            IServiceProvider serviceProvider,
            IDictionary<object, object> items,
            ModelErrorDictionary modelErrors,
            PropertyInfo pi)
        {
            var enumerableModelErrors = new ModelErrorDictionary();
            var enumerableValue = (IEnumerable)pi.GetValue(model);

            enumerableValue.Cast<object>().ForEach((elementModel, index) =>
            {
                var elementValidationResult = IsValid(elementModel, serviceProvider, items);
                enumerableModelErrors.Add(elementValidationResult.modelErrors, index);
            });

            modelErrors.Add(enumerableModelErrors, pi.Name);
        }

        private static void ValidateAgainstType(
            object model,
            IServiceProvider serviceProvider,
            IDictionary<object, object> items,
            ModelErrorDictionary modelErrors,
            PropertyInfo pi,
            ValidateAgainstTypeAttribute attr)
        {
            var propertyModel = pi.GetValue(model);
            if (propertyModel == null)
            {
                // Everything is fine. RequiredAttribute should handle validation here if not.
                return;
            }

            #region Instantiate an object of the target type so we can validate it

            var propertyModelPropertiesByName = propertyModel.GetType().GetProperties()
                .ToDictionary(pi2 => pi2.Name, attr.IgnoreCase ? StringComparer.InvariantCultureIgnoreCase : StringComparer.InvariantCulture);

            var retypedPropertyModel = Activator.CreateInstance(attr.Type);
            attr.Type.GetProperties().ForEach(retypedProperty =>
            {
                if (propertyModelPropertiesByName.TryGetValue(retypedProperty.Name, out PropertyInfo property)) {
                    retypedProperty.SetValue(retypedPropertyModel, property.GetValue(propertyModel));
                }
            });

            #endregion

            var retypedValidationResult = IsValid(retypedPropertyModel, serviceProvider, items);
            modelErrors.Add(retypedValidationResult.modelErrors, pi.Name);
        }

        private static void ValidateAgainstTypeIf(
            object model,
            IServiceProvider serviceProvider,
            IDictionary<object, object> items,
            ModelErrorDictionary modelErrors,
            PropertyInfo pi,
            ValidateAgainstTypeIfAttribute attr)
        {
            var targetProperty = model.GetType().GetProperties().First(pi2 => pi2.Name == attr.PropertyName);
            var targetPropertyValue = targetProperty.GetValue(model);
            if (targetPropertyValue.Equals(attr.PropertyValue))
            {
                ValidateAgainstType(model, serviceProvider, items, modelErrors, pi, attr);
            }
        }

        #endregion
    }
}
