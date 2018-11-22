using System.Linq;
using TryCSharp.Common;

namespace TryCSharp.Samples.Linq
{
    /// <summary>
    ///     Linqのサンプルです。
    /// </summary>
    [Sample]
    public class LinqSamples44 : IExecutable
    {
        public void Execute()
        {
            //
            // Skip拡張メソッドは、シーケンスの先頭から指定された件数分をスキップするメソッド。
            //
            //   ・シーケンスの要素数より多い数を指定した場合、空のシーケンスが返る.
            //   ・0以下の値を指定した場合、シーケンスの全ての要素が返る.
            //
            var names = new[] {"gsf_zero1", "gsf_zero2", "gsf_zero3", "gsf_zero4", "gsf_zero5"};

            Output.WriteLine("================ Skip ===========================");
            var last2Elements = names.Skip(3);
            foreach (var item in last2Elements)
            {
                Output.WriteLine(item);
            }

            Output.WriteLine("シーケンスの要素数以上の数を指定: COUNT={0}", names.Skip(20).Count());

            foreach (var item in names.Skip(-1))
            {
                Output.WriteLine(item);
            }

            //
            // SkipWhile拡張メソッドは、指定された条件が満たされる間シーケンスから要素を抽出し
            // 返すメソッド。
            //
            Output.WriteLine("================ SkipWhile ======================");
            var greaterThan4 = names.SkipWhile(name => int.Parse(name.Last().ToString()) <= 3);
            foreach (var item in greaterThan4)
            {
                Output.WriteLine(item);
            }
        }
    }
}