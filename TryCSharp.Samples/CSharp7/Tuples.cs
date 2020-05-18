using System;
using System.Threading;
using TryCSharp.Common;

namespace TryCSharp.Samples.CSharp7
{
    /// <summary>
    ///     C# 7　の新機能についてのサンプルです。
    /// </summary>
    /// <remarks>
    ///     Tuples について
    ///     https://docs.microsoft.com/ja-jp/dotnet/csharp/whats-new/csharp-7#tuples
    ///     https://ufcpp.net/study/csharp/cheatsheet/ap_ver7/#tuple
    /// </remarks>
    [Sample]
    public class Tuples : IExecutable
    {
        public void Execute()
        {
            // ----------------------------------------------------------------
            // タプル
            //
            // C# 7 から python や Go のように タプル がサポートされるようになった。
            // 元々、System.Tuple という型で存在していたのだが、それが言語として
            // サポートされるようになった感じ。内部的には ValueTask として表現される。
            //
            // 前までは
            //   Tuple<int, int> t = Tuple.Create(10, 11)
            // としていた部分を
            //   var (x, y) = (10, 11);
            // とかけるようになっている。
            //
            // この記述は メソッドの引数でも戻り値にも利用できる。
            //
            // 基本 Python などの言語やっている人には馴染みやすい形となった。
            // ----------------------------------------------------------------
            var oldStyle = Tuple.Create(10, 11); // Tuple<int, int>
            var newStyle = (10, 11); // ValueTuple<int, int>

            Output.WriteLine($"[old] {oldStyle.GetType().Name}");
            Output.WriteLine($"[new] {newStyle.GetType().Name}");

            // C# 7 の別の新機能 Deconstruct (分解) を利用して以下のようにもかける
            var (x, y) = (10, 11);
            Output.WriteLine($"[x] {x}\t[y] {y}");

            // メソッドの引数にも戻り値にも利用できる
            (Thread, int) GetThreadInfo()
            {
                var t = Thread.CurrentThread;
                return (t, t.ManagedThreadId);
            }

            var (_, id) = GetThreadInfo(); // 不要な情報がある場合は Go のように アンダースコア で捨てる
            Output.WriteLine($"[main] threadId: {id}");
        }
    }
}