using System.Linq;
using TryCSharp.Common;

namespace TryCSharp.Samples.Linq
{
    /// <summary>
    ///     Linqのサンプルです。
    /// </summary>
    [Sample]
    public class LinqSamples43 : IExecutable
    {
        public void Execute()
        {
            //
            // Take拡張メソッドは、シーケンスの先頭から指定された件数分を返すメソッド。
            //
            //   ・シーケンスの要素数より多い数を指定した場合、そのシーケンス全てが返る.
            //   ・0以下の値を指定した場合、空のシーケンスが返る.
            //
            var names = new[] {"gsf_zero1", "gsf_zero2", "gsf_zero3", "gsf_zero4", "gsf_zero5"};

            Output.WriteLine("================ Take ======================");
            var top3 = names.Take(3);
            foreach (var item in top3)
            {
                Output.WriteLine(item);
            }

            foreach (var item in names.Take(20))
            {
                Output.WriteLine(item);
            }

            Output.WriteLine("0以下の数値を指定: COUNT={0}", names.Take(-1).Count());

            //
            // TakeWhile拡張メソッドは、指定された条件が満たされる間シーケンスから要素を抽出し
            // 返すメソッド。
            //
            Output.WriteLine("================ TakeWhile ======================");
            var lessThan4 = names.TakeWhile(name => int.Parse(name.Last().ToString()) <= 4);
            foreach (var item in lessThan4)
            {
                Output.WriteLine(item);
            }
        }
    }
}