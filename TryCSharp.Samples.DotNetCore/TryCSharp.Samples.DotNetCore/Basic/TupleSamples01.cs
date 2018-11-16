using System;
using TryCSharp.Common;

namespace TryCSharp.Samples.Basic
{
    /// <summary>
    ///     Tupleクラスについてのサンプルです。
    /// </summary>
    /// <remarks>
    ///     Tupleクラスは4.0から追加されたクラスです。
    /// </remarks>
    [Sample]
    public class TupleSamples01 : IExecutable
    {
        public void Execute()
        {
            //
            // Tupleクラスは、.NET 4.0から追加されたクラスである。
            // 複数の値を一組のデータとして、保持することができる。
            //
            // よく利用されるのは、戻り値にて複数の値を返す必要が有る場合などである。
            // (objectの配列を返すという手もあるが、その場合Boxingが発生してしまうのでパフォーマンスが
            //  厳しく要求される場面では、利用しづらい。その点、Tupleはジェネリッククラスとなっているので
            //  Boxingが発生する事がない。)
            //
            // Tupleクラスは、不可変のオブジェクトとなっている。つまり、コンストラクト時に値を設定した後は
            // その値を変更することが出来ない。（参照の先に存在しているメンバは変更可能。）
            // 各データは、「Item1」「Item2」・・・という形で取得していく。
            //
            // 以下のようなクラス定義が行われており、データの数によってインスタンス化するものが変わる。
            //   Tuple<T1>
            //   Tuple<T1, T2>
            //   Tuple<T1, T2, T3>
            //   Tuple<T1, T2, T3, T4>
            //   Tuple<T1, T2, T3, T4, T5>
            //   Tuple<T1, T2, T3, T4, T5, T6>
            //   Tuple<T1, T2, T3, T4, T5, T6, T7>
            //   Tuple<T1, T2, T3, T4, T5, T6, T7, TRest>
            //
            // データ数が７つ以上の場合は、残りの部分をTRestとして設定する。
            //
            // Tupleを作成する際は、Tuple.Createメソッドを利用してインスタンスを取得するのが楽である。
            // また、その際は型推論を利用すると便利。
            //
            // Tupleクラスでは、ToStringメソッドがオーバーライドされており、以下のように表示される。
            //   Tuple<int, int>   ==> (xxx, yyy)
            //
            var t1 = Tuple.Create(100, "gsf_zero1");
            var t2 = Tuple.Create(200, "gsf_zero2", 30); // Tuple<int, string, int>となる。

            Output.WriteLine(t1.Item1);
            Output.WriteLine(t1.Item2);

            Output.WriteLine(t2.Item1);
            Output.WriteLine(t2.Item2);
            Output.WriteLine(t2.Item3);

            var t3 = TestMethod(10, 20);
            Output.WriteLine(t3); // (100, 400)

            // 以下はエラーとなる.
            // t3.Item1 = 1000;
        }

        private Tuple<int, int> TestMethod(int x, int y)
        {
            return Tuple.Create(x*x, y*y);
        }
    }
}