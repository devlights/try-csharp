using System;
using System.Collections.Generic;
using System.Linq;

namespace TryCSharp.Samples.Linq
{
    public static class LinqSamples59_Extensions
    {
        /// <summary>
        ///     シーケンスを指定されたサイズのチャンクに分割します.
        /// </summary>
        public static IEnumerable<IEnumerable<T>> Chunk<T>(this IEnumerable<T> self, int chunkSize)
        {
            if (chunkSize <= 0)
            {
                throw new ArgumentException("Chunk size must be greater than 0.", nameof(chunkSize));
            }

            var enumerable = self as T[] ?? self.ToArray();
            while (enumerable.Any())
            {
                yield return enumerable.Take(chunkSize);
                self = enumerable.Skip(chunkSize);
            }
        }
    }
}