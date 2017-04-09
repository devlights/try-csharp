using System;
using System.Collections.Generic;
using System.Linq;
using TryCSharp.Common;

namespace TryCSharp.Samples.Basic
{
    /// <summary>
    ///     System.ArraySegment構造体についてのサンプルです。
    /// </summary>
    /// <remarks>
    ///     ArraySegment構造体は、.NET 4.0と.NET 4.5以降で
    ///     実装が異なります。.NET 4.5より、この構造体はコレクションインターフェースを
    ///     実装するように変更されています。
    /// </remarks>
    [Sample]
    internal class ArraySegmentSamples01 : IExecutable
    {
        public void Execute()
        {
            //
            // System.ArraySegmentは、配列の一部分の参照を保持する構造体である。
            // この構造体を利用すると部分配列を手軽に利用できる。
            // (例えば、とある配列の5番目から5つ分など）
            //
            // System.ArraySegment構造体は、.NET 4.0までと.NET 4.5以降で
            // 実装がかなり異なる。.NET 4.0まで、この型はコレクションインターフェースを
            // 一切実装していなかった。そのため、forでループさせる際にOffsetとCountプロパティ
            // を利用して要素を取得する必要があった。
            // .NET 4.5から、この型はIListやICollectionやIEnumerableなどを実装する
            // ようになったので、foreachなどで手軽にループすることが出来るようになっている。
            //
            var originalArray = Enumerable.Range(1, 10).ToArray();

            var segment1 = new ArraySegment<int>(originalArray, 0, 5);
            var segment2 = new ArraySegment<int>(originalArray, 5, 5);

            //
            // .NET 4.0まで
            // Offsetプロパティ、Countプロパティを利用してループし要素を取得する.
            //
            for (var i = 0; i < segment1.Count; i++)
            {
                Output.WriteLine(segment1.Array[segment1.Offset + i]);
            }

            for (var i = segment2.Offset; i < segment2.Offset + segment2.Count; i++)
            {
                Output.WriteLine(segment2.Array[i]);
            }

            //
            // .NET 4.5から
            // 標準のコレクションインターフェースを実装しているため、リストと同じように扱う事が可能。
            //
            foreach (var item in segment1.Zip(segment2, (x, y) => new {x, y}))
            {
                Output.WriteLine(item);
            }

            //
            // 値の変更
            // 値を変更する場合、予めIList<T>にキャストして変更する。
            // ArraySegmentは、元の配列の要素と同じ参照を保持しているので
            // ArraySegmentの要素の値を変更すると、元の配列の値も変わる。
            //
            var col1 = segment1 as IList<int>;
            col1[2] = 999;

            var col2 = segment2 as IList<int>;
            col2[2] = 888;

            foreach (var item in originalArray)
            {
                Output.WriteLine(item);
            }
        }
    }
}