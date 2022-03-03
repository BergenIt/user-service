using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DatabaseExtension
{
    public static class LinqExtensions
    {
        public static string ToLowerFirst(this string vs)
        {
            return vs?.Length > 1
                ? $"{$"{vs[0]}".ToLowerInvariant()}{vs[1..]}"
                : vs;
        }

        public static IEnumerable<TSource> DistinctBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            HashSet<TKey> knownKeys = new();

            foreach (TSource element in source)
            {
                if (knownKeys.Add(keySelector(element)))
                {
                    yield return element;
                }
            }
        }

        public static Task<IEnumerable<TSource>> WhereAsync<TSource>(this IEnumerable<TSource> source, Func<TSource, Task<bool>> predicate)
        {
            ConcurrentQueue<TSource> concurrentQueue = new();

            IEnumerable<Task> tasks = source.Select(async x =>
            {
                if (await predicate(x))
                {
                    concurrentQueue.Enqueue(x);
                }
            });

            return Task.WhenAll(tasks).ContinueWith(_ => concurrentQueue.AsEnumerable());
        }
    }
}
