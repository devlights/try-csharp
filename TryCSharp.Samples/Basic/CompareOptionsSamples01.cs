using System.Globalization;
using TryCSharp.Common;
using TryCSharp.Samples.Helpers;

namespace TryCSharp.Samples.Basic
{
    /// <summary>
    ///     CompareOptions列挙型のサンプルです。
    /// </summary>
    [Sample]
    public class CompareOptionsSamples01 : IExecutable
    {
        public void Execute()
        {
            //
            // string.Compareメソッドには、CultureInfoとCompareOptionsを
            // 引数にとるオーバーロードが定義されている。(他にもオーバーロードメソッドが存在します。)
            //
            // このオーバーロードを利用する際、CompareOptions.IgnoreKanaTypeを指定すると
            // 「ひらがな」と「カタカナ」の違いを無視して、文字列比較を行う事が出来る。
            //
            var ja1 = "はろーわーるど";
            var ja2 = "ハローワールド";

            var ci = new CultureInfo("ja-JP");

            // 標準の比較方法で比較
            Output.WriteLine("{0}", string.Compare(ja1, ja2, ci, CompareOptions.None).ToStringResult());
            // 大文字小文字を無視して比較.
            Output.WriteLine("{0}", string.Compare(ja1, ja2, ci, CompareOptions.IgnoreCase).ToStringResult());
            // ひらがなとカタカナの違いを無視して比較
            // つまり、「はろーわーるど」と「ハローワールド」を同じ文字列として比較
            Output.WriteLine("{0}", string.Compare(ja1, ja2, ci, CompareOptions.IgnoreKanaType).ToStringResult());

            //
            // string.Compareメソッドは、内部でCutureInfoから、そのカルチャーに紐づく
            // CompareInfoを取り出して、比較処理を行っているので、自前で直接CompareInfoを
            // 用意して、Compareメソッドを呼び出しても同じ結果となる。
            //
            var compInfo = CompareInfo.GetCompareInfo("ja-JP");
            Output.WriteLine("{0}", compInfo.Compare(ja1, ja2, CompareOptions.IgnoreKanaType).ToStringResult());
        }
    }
}