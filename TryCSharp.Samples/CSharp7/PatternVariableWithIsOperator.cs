using System.Diagnostics.CodeAnalysis;
using TryCSharp.Common;

namespace TryCSharp.Samples.CSharp7
{
    [Sample]
    [SuppressMessage("ReSharper", "UnusedType.Global")]
    public class PatternVariableWithIsOperator : IExecutable
    {
        private class MyInnerClass
        {
            public MyInnerClass(MyInnerClass2 inner)
            {
                this.Inner = inner;
            }

            public MyInnerClass2 Inner { get; }
        }

        private class MyInnerClass2
        {
            public int Value { get; set; }
        }

        [SuppressMessage("ReSharper", "ConditionIsAlwaysTrueOrFalse")]
        [SuppressMessage("ReSharper", "ConstantConditionalAccessQualifier")]
        [SuppressMessage("ReSharper", "PatternAlwaysOfType")]
        public void Execute()
        {
            // C# 7.0 から　Pattern Variables の概念が導入された
            // is 演算子とともに変数を宣言することが可能になった
            object x = "hello world";

            // C# 7.0 以前
            // ReSharper disable once MergeCastWithTypeCheck
            if (x is string)
            {
                // ReSharper disable once TryCastAlwaysSucceeds
                var cs6 = x as string;
                Output.WriteLine($"C# 6.0 val:{cs6}, length:{cs6.Length}");
            }

            // C# 7.0
            //   is 演算子が拡張されて、以下の３つのパターンが使えるようになった
            //
            // - constant pattern
            // - type pattern
            // - var pattern
            //
            // constant pattern
            object y = null;
            if (y is null)
            {
                Output.WriteLine("y is null.");
            }

            object z = int.MaxValue;
            if (z is int.MaxValue)
            {
                Output.WriteLine("z is int.MaxValue");
            }

            if (x is "hello world")
            {
                // 文字列も定数と同じ
                Output.WriteLine("x is hello world");
            }

            // type pattern
            if (x is string cs7)
            {
                // is演算子の判定がTrueの場合のみ、変数 cs7 に値が格納される
                Output.WriteLine($"C# 7.0 val:{cs7}, length:{cs7.Length}");
            }

            // type pattern は、C#6.0 の　null条件演算子 と相性が良い
            var innerA = new MyInnerClass(new MyInnerClass2 {Value = 100});
            var innerB = new MyInnerClass(null);

            // C# 6.0 までは以下のように書く必要があった
            var v = innerA?.Inner?.Value;
            if (v.HasValue)
            {
                Output.WriteLine($"v [{v}]");
            }

            // C# 7.0 では is 演算子によって以下のように書ける
            if (innerA?.Inner?.Value is int i)
            {
                Output.WriteLine($"innerA [{i}]");
            }

            if (innerB?.Inner?.Value is int i2)
            {
                Output.WriteLine($"innerB [{i2}]");
            }

            // var pattern はちょっと特殊。
            // これは、null のときも含めて is 演算子の評価が常にTrueとなって、var で宣言した変数に入る
            if (innerB.Inner is var j)
            {
                Output.WriteLine($"j is null [{j == null}]");
            }
        }
    }
}