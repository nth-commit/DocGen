﻿using System;
using System.Collections.Generic;
using System.Text;

namespace System.Linq
{
    public static class EnumerableExtensions
    {
        public static (IEnumerable<T> matches, IEnumerable<T> nonMatches) Fork<T>(
            this IEnumerable<T> source,
            Func<T, bool> pred)
        {
            var groupedByMatching = source.ToLookup(pred);
            return (groupedByMatching[true], groupedByMatching[false]);
        }

        public static IEnumerable<T> Concat<T>(
            this IEnumerable<T> source,
            params T[] elements)
        {
            return source.Concat(elements.AsEnumerable());
        }

        public static IEnumerable<T> TakeAllExceptLast<T>(
            this IEnumerable<T> source)
        {
            return source.Take(source.Count() - 1);
        }

        //public static void ForEach<T>(
        //    this IEnumerable<T> source,
        //    Action<T, int> action)
        //{
        //    var sourceList = source.ToList();
        //    for (int i = 0; i < sourceList.Count; i++)
        //    {
        //        action(sourceList[i], i);
        //    }
        //}
    }
}
