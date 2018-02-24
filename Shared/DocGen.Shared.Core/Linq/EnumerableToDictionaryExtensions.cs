using System;
using System.Collections.Generic;
using System.Text;

namespace System.Linq
{
    public static class EnumerableToDictionaryExtensions
    {
        public static Dictionary<TKey, TSource> ToDictionary<TSource, TKey>(
            this IEnumerable<TSource> source,
            Func<TSource, int, TKey> keySelector)
        {
            return source
                .Select((element, i) => new
                {
                    Element = element,
                    Index = i
                })
                .ToDictionary(
                    x => keySelector(x.Element, x.Index),
                    x => x.Element);
        }

        public static Dictionary<TKey, TValue> ToDictionary<TSource, TKey, TValue>(
            this IEnumerable<TSource> source,
            Func<TSource, int, TKey> keySelector,
            Func<TSource, int, TValue> valueSelector)
        {
            return source
                .Select((element, i) => new
                {
                    Element = element,
                    Index = i
                })
                .ToDictionary(
                    x => keySelector(x.Element, x.Index),
                    x => valueSelector(x.Element, x.Index));
        }
    }
}
