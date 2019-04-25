using System;
using TryCSharp.Common;

namespace TryCSharp.Samples.Basic
{
    /// <summary>
    /// ValueTuple についてのサンプルです。
    /// </summary>
    /// <remarks>
    /// REFERENCES:: http://bit.ly/2L39RFZ
    ///              http://bit.ly/2KXgLwu
    ///              http://bit.ly/2L2okSN
    /// </remarks>
    [Sample]
    public class ValueTupleSamples01 : IExecutable
    {
        public void Execute()
        {
            // -----------------------------------------------------------
            // ValueTuple は C# 7.0 から追加された.
            // 
            // struct であり、ミュータブルである.
            // メンバは、プロパティではなくフィールドとして追加される.
            //
            // 簡易記法として以下の記述ができる.
            //   var x = (x:100, y:200);
            // これは
            //   ValueTuple<int, int> x = (x:100, y:200);
            // と同じ.
            //
            // 以前から存在する
            //   var x = new { x=100, y=100 };
            // は、class となり、要素はプロパティとして追加される.
            // 内部の型は 「AnonymousType」となる.
            //
            // AnonymousType は、MSDNに記載があるように、dynamic 以外に変換できない.
            // ----------------------------------------------------------
            var vt1 = (X: 100, Y: 200);
            var at1 = new {X = 100, Y = 200};

            Output.WriteLine($"{vt1.GetType().Name}, {at1.GetType().Name}");

            // 以下はエラーとならない
            ValueTuple<int, int> vt2 = vt1;
            // 以下はコンパイルエラー
            // Tuple<int, int> at2 = at1;

            Output.WriteLine($"vt2 == vt1 ({vt2 == vt1})");
            Output.WriteLine($"vt1={vt1}");
            Output.WriteLine("at1=(X={0}, Y={1})", at1.X, at1.Y);
            Output.WriteLine($"vt2={vt2}");
        }
    }
}