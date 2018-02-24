using System;
using System.Collections.Generic;
using System.Text;

namespace System.Linq
{
    public static class EnumerableToIndexedEnumerableExtensions
    {
        public static IEnumerable<IndexedElement<TSource>> ToIndexedEnumerable<TSource>(this IEnumerable<TSource> source)
            => source.Select((element, i) => new IndexedElement<TSource>()
            {
                Element = element,
                Index = i
            });

        public static ILookup<TKey, IndexedElement<TSource>> ToIndexedLookup<TKey, TSource>(
            this IEnumerable<TSource> source,
            Func<TSource, TKey> keySelector)
                => source.ToLookup(
                    (element, index) => keySelector(element),
                    (element, index) => new IndexedElement<TSource>()
                    {
                        Element = element,
                        Index = index
                    });

        public static Dictionary<TKey, IndexedElement<TSource>> ToIndexedDictionary<TKey, TSource>(
            this IEnumerable<TSource> source,
            Func<TSource, TKey> keySelector)
                => source.ToDictionary(
                    (element, index) => keySelector(element),
                    (element, index) => new IndexedElement<TSource>()
                    {
                        Element = element,
                        Index = index
                    });
    }

    public class IndexedElement<T>
    {
        public T Element { get; set; }

        public int Index { get; set; }
    } 
}
