using System.Collections.Generic;
using TryCSharp.Common;

namespace TryCSharp.Samples.Basic
{
    /// <summary>
    ///     共変性についてのサンプルです。
    /// </summary>
    /// <remarks>
    ///     共変性は4.0から追加された機能です。
    /// </remarks>
    [Sample]
    public class CovarianceSamples01 : IExecutable
    {
        public void Execute()
        {
            //
            // Covariance(共変性)は、簡単に言うと、子のオブジェクトを親の型として扱う事。
            //
            // 例：
            //   string str = "gsf_zero1";
            //   object obj = str;
            //
            // C# 4.0では、この概念をジェネリックインターフェースに対して適用できるようになった。
            // 共変性を表明するには、型引数を定義する際に、「out」キーワードを設定する。
            //
            // .NET 4.0では、IEnumerable<T>は以下のように定義されている。
            //   public interface IEnumerable<out T> : IEnumerable { ... }
            //
            // 「out」キーワードは、この型引数を「出力方向」にしか利用しないことを表明している。
            // つまり、「out」キーワードが付与されるとTを戻り値などの出力値にしか利用できなくなる。
            // (outを指定している状態で、入力方向、つまりメソッドの引数などにTを設定しようとすると
            //  コンパイルエラーが発生する。）
            //
            // 出力方向にしか利用しないので、子の型（つまり狭義の型）を親の型（つまり広義の型）に
            // 設定しても、問題ない。
            //  「内部の型はstringであるが、実際に値を取り出す際には親の型で受け取るので問題ない」
            //
            // Contravariance(反変性)は、この逆を行うものとなる。
            //
            IEnumerable<string> strings = new[] {"gsf_zero1", "gsf_zero2"};
            IEnumerable<object> objects = strings;

            foreach (var obj in objects)
            {
                Output.WriteLine("VALUE={0}, TYPE={1}", obj, obj.GetType().Name);
            }
        }
    }
}