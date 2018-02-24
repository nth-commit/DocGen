using Microsoft.CSharp.RuntimeBinder;
using System;
using System.Collections.Generic;
using System.Text;

namespace DocGen.Shared.Core.Dynamic
{
    public class DynamicUtility
    {
        public static T Unwrap<T>(Func<T> accessor) where T : class
        {
            T value = null;
            try
            {
                value = accessor();
            }
            catch (RuntimeBinderException)
            {
            }
            return value;
        }

        public static T UnwrapValue<T>(Func<T> accessor, T defaultValue = default(T)) where T : struct, IComparable
        {
            T value = defaultValue;
            try
            {
                value = accessor();
            }
            catch (RuntimeBinderException)
            {
            }
            return value;
        }

        public static T? UnwrapNullableValue<T>(Func<T?> accessor, T? defaultValue = default(T?)) where T : struct, IComparable
        {
            T? value = defaultValue;
            try
            {
                var nullableValue = accessor();
                if (nullableValue.HasValue)
                {
                    value = nullableValue.Value;
                }
            }
            catch (RuntimeBinderException)
            {
            }
            return value;
        }
    }
}
