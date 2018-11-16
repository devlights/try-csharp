using System.Globalization;
using TryCSharp.Common;
using TryCSharp.Samples.Helpers;

namespace TryCSharp.Samples.Basic
{
    /// <summary>
    ///     CompareOptions列挙型のサンプルです。
    /// </summary>
    [Sample]
    public class CompareOptionsSamples02 : IExecutable
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
            // さらに、CompareOptionsには、IgnoreWidthという値も存在し
            // これを指定すると、全角と半角の違いを無視して、文字列比較を行う事が出来る。
            //
            var ja1 = "ハローワールド";
            var ja2 = "ﾊﾛｰﾜｰﾙﾄﾞ";
            var ja3 = "はろーわーるど";

            var ci = new CultureInfo("ja-JP");

            // 全角半角の違いを無視して、「ハローワールド」と「ﾊﾛｰﾜｰﾙﾄﾞ」を比較
            Output.WriteLine("{0}", string.Compare(ja1, ja2, ci, CompareOptions.IgnoreWidth).ToStringResult());
            // 全角半角の違いを無視して、「はろーわーるど」と「ﾊﾛｰﾜｰﾙﾄﾞ」を比較
            Output.WriteLine("{0}", string.Compare(ja3, ja2, ci, CompareOptions.IgnoreWidth).ToStringResult());
            // 全角半角の違いを無視し、且つ、ひらがなとカタカナの違いを無視して、「はろーわーるど」と「ﾊﾛｰﾜｰﾙﾄﾞ」を比較
            Output.WriteLine("{0}", string.Compare(ja3, ja2, ci, CompareOptions.IgnoreWidth | CompareOptions.IgnoreKanaType).ToStringResult());
        }
    }
}