using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System.Linq
{
    public static class EnumerableToLookupExtensions
    {
        public static ILookup<TKey, TSource> ToLookup<TSource, TKey>(
            this IEnumerable<TSource> source,
            Func<TSource, int, TKey> keySelector)
        {
            return source
                .Select((element, i) => new
                {
                    Element = element,
                    Index = i
                })
                .ToLookup(
                    x => keySelector(x.Element, x.Index),
                    x => x.Element);
        }

        public static ILookup<TKey, TValue> ToLookup<TSource, TKey, TValue>(
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
                .ToLookup(
                    x => keySelector(x.Element, x.Index),
                    x => valueSelector(x.Element, x.Index));
        }
    }
}
