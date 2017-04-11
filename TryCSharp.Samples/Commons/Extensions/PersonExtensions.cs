using System;
using TryCSharp.Common;
using TryCSharp.Samples.Commons.Data;

namespace TryCSharp.Samples.Commons.Extensions
{
    /// <summary>
    /// <see cref="Person"/>の拡張メソッドが定義されています。
    /// </summary>
    public static class PersonExtensions
    {
        /// <summary>
        /// 指定された条件に合致する<see cref="Person"/>を取得します。
        /// </summary>
        /// <param name="self">自分自身</param>
        /// <param name="predicate">抽出条件</param>
        /// <returns>絞込結果</returns>
        public static Persons Where(this Persons self, Func<Person, bool> predicate)
        {
            var result = new Persons();

            Output.WriteLine("========= WHERE ========");
            foreach (var aPerson in self)
            {
                if (predicate(aPerson))
                {
                    result.Add(aPerson);
                }
            }

            return result;
        }
    }
}