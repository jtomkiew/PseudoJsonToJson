using System;
using System.Collections.Generic;
using System.Linq;

namespace JsonFixer
{
    public static class StringExtention
    {
        public static int IndexOf<TSource>(this IEnumerable<TSource> source,
            Func<TSource, bool> predicate)
        {
            var i = 0;

            foreach (var element in source)
            {
                if (predicate(element))
                    return i;

                i++;
            }

            return -1;
        }

        public static int LastIndexOf<TSource>(this IEnumerable<TSource> source,
            Func<TSource, bool> predicate)
        {
            var i = source.Count() - 1;

            foreach (var element in source.Reverse())
            {
                if (predicate(element))
                    return i;

                i--;
            }

            return -1;
        }
    }
}