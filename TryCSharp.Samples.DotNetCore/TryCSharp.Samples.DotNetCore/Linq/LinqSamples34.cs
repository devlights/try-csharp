using System.Linq;
using TryCSharp.Common;

namespace TryCSharp.Samples.Linq
{
    /// <summary>
    ///     Linqのサンプルです。
    /// </summary>
    [Sample]
    public class LinqSamples34 : IExecutable
    {
        public void Execute()
        {
            var numbers = new[]
            {
                1, 2, 3, 4, 5, 0, 10, 98, 99
            };

            // 
            // Min, Max拡張メソッドは、文字通り最小値、最大値を求める拡張メソッド。
            //
            // Min, Max拡張メソッドには、各基本型のオーバーロードが用意されており
            // (decimal, double, int, long, single及びそれぞれのNullable型)
            // それぞれに、引数無しとselectorを指定するバージョンのメソッドがある。
            //

            //
            // 引数無しのMin, Max拡張メソッドの使用.
            //
            Output.WriteLine("引数無し[Min] = {0}", numbers.Min());
            Output.WriteLine("引数無し[Max] = {0}", numbers.Max());

            //
            // selectorを指定するMin, Max拡張メソッドの使用.
            //
            Output.WriteLine("引数有り[Min] = {0}", numbers.Min(item => item%2 == 0 ? item : 0));
            Output.WriteLine("引数有り[Max] = {0}", numbers.Max(item => item%2 == 0 ? item : 0));
        }
    }
}