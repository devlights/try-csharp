using System;
using TryCSharp.Common;

namespace TryCSharp.Samples.Basic
{
    /// <summary>
    ///     反変性についてのサンプルです。
    /// </summary>
    /// <remarks>
    ///     反変性は4.0から追加された機能です。
    /// </remarks>
    [Sample]
    public class ContravarianceSamples01 : IExecutable
    {
        public void Execute()
        {
            //
            // Contravariance(反変性)は、簡単に言うと、親のオブジェクトを子の型として扱う事。
            // (共変性の逆です。）
            //
            // 例：
            //   class Parent     { ... }
            //   class Child : Parent { ... }
            //
            //   delegate Parent SampleDelegate();
            //
            //   Child SampleMethod() { ... }
            //
            //   // ここで反変性が発生している。
            //   SampleDelegate theDelegate = SampleMethod;
            //
            // C# 4.0では、この概念をジェネリックインターフェースに対して適用できるようになった。
            // 共変性を表明するには、型引数を定義する際に、「in」キーワードを設定する。
            //
            // .NET 4.0では、Action<T>は以下のように定義されている。
            //   public delegate void Action<in T>(T obj)
            //
            // 「in」キーワードは、この型引数を「入力方向」にしか利用しないことを表明している。
            // つまり、「in」キーワードが付与されるとTを引数などの入力値にしか利用できなくなる。
            // (inを指定している状態で、出力方向、つまりメソッドの戻り値などにTを設定しようとすると
            //  コンパイルエラーが発生する。）
            //
            // 入力方向にしか利用しないので、親の型（つまり広義の型）を子の型（つまり狭義の型）に
            // 設定しても、問題ない。
            //  「外部の型はstringであるが、実際にデータが渡される際、内部の引数の型はobjectなので問題ない」
            //
            // 例：
            //   Action<object> objAction = x => Output.WriteLine(x);
            //   Action<string> strAction = objAction;
            //
            //   strAction("gsf_zero1");
            //
            //   上記の例だと、objActionをstrActionに設定している。つまり親クラスの型で定義されているAction<object>を
            //   子のクラスのAction<string>に設定している。
            //   その後、strAction("gsf_zero1")としているので、外部から渡された値はstring型である。
            //   しかし、objActionの引数の型は、親クラスであるobject型なので問題なく動作する。
            //   (親クラスに定義されている振る舞いしか利用できないため。）
            //
            // Covariance(共変性)は、この逆を行うものとなる。
            //
            Action<object> objAction = Output.WriteLine;
            Action<string> strAction = objAction;

            strAction("gsf_zero1");
        }
    }
}