using System.Linq;
using TryCSharp.Common;

namespace TryCSharp.Samples.Linq
{
    /// <summary>
    ///     Linqのサンプルです。
    /// </summary>
    [Sample]
    public class LinqSamples33 : IExecutable
    {
        public void Execute()
        {
            var numbers = new[]
            {
                1, 2, 3, 4, 5
            };

            // 
            // Sum拡張メソッドは、文字通り合計を求める拡張メソッド。
            //
            // Sum拡張メソッドには、各基本型のオーバーロードが用意されており
            // (decimal, double, int, long, single及びそれぞれのNullable型)
            // それぞれに、引数無しとselectorを指定するバージョンのメソッドがある。
            //

            //
            // 引数無しのSum拡張メソッドの使用.
            //
            Output.WriteLine("引数無し = {0}", numbers.Sum());

            //
            // selectorを指定するSum拡張メソッドの使用.
            //
            Output.WriteLine("引数有り = {0}", numbers.Sum(item => item%2 == 0 ? item : 0));
        }
    }
}