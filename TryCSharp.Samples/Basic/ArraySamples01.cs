using System;
using TryCSharp.Common;

namespace TryCSharp.Samples.Basic
{
    /// <summary>
    ///     配列(System.Array)のサンプルクラスです.
    /// </summary>
    /// <remarks>
    ///     C#のArrayクラスは、javaの配列よりも高機能です。
    ///     (配列そのものとしての機能とjava.util.Arraysの機能を足した感じ)
    ///     また、System.Arrayクラスは、.NET 2.0では、
    ///     -System.Collections.Generic.IList
    ///     -System.Collections.Generic.ICollection
    ///     -System.Collections.Generic.IEnumerable
    ///     を実装しています。
    ///     配列オブジェクトを以下のように
    ///        IList toList = (IList) ary;
    ///     のようにキャストすることは出来ますが、要素の読み込み以外の
    ///     操作（追加、挿入、削除）はできません。例外が発生します。
    ///     なぜなら、配列はリストと違いサイズが固定であるためです。
    /// </remarks>
    [Sample]
    public class ArraySamples01 : IExecutable
    {
        public void Execute()
        {
            string?[] ary = {"hoge", "hehe", "fuga"};

            ///////////////////////////////////////////////////////
            //
            // プロパティ
            //
            // IsFixedSize, IsReadOnlyは、IListを実装しているため
            // 存在しているプロパティです。配列の場合、これらの値は
            // 常にtrue, falseを返します。
            Output.WriteLine("IsFixedSize = {0}", ary.IsFixedSize);
            Output.WriteLine("IsReadOnly  = {0}", ary.IsReadOnly);
            Output.WriteLine("Length      = {0}", ary.Length);

            ///////////////////////////////////////////////////////
            //
            // メソッド
            //
            // System.Arrayメンバには、多くのメソッドがありますが
            // 他の言語に存在していてよく知られているメソッドも多いです。
            // (IndexOfやLastIndexOf, BinarySearch, Sort, Reverseなど)
            // 普通の使い方は、書いても面白くないので、
            // 今回は、特にjavaの配列には存在していないメソッドをテストしてみます。
            // .NETの配列には、Action, Predicate, Convertデリゲートなどを用いて特定の配列データを抽出
            // したり探したりできたりします。javaの場合は、別途commons-collectionなどを
            // 利用するとPredicateなどを利用して処理できたりします。
            //

            //
            // ForEachメソッド.
            //
            // このメソッドは、Arrayのstaticメソッドとして定義されていて、
            // 配列の各要素に対して指定された処理を行えます.
            // foreachにてループしてデータを出力を書き換えると以下のような感じ.
            // 第二引数に渡すのは、Action<T>のデリゲートオブジェクトです。
            //
            // Actionデリゲートの書式は、匿名デリゲートで処理する場合
            //      void xxxxx(T obj)
            // です。
            //
            Array.ForEach(ary, element => Output.WriteLine(element));

            //
            // Find/FindAllメソッド
            //
            // これら2つのメソッドは、Arrayのstaticメソッドとして定義されていて、
            // 配列の各要素に対して指定された条件に合致するものを探します。
            // 第二引数に渡すのは、Predicate<T>のデリゲートオブジェクトです。
            // Findは、最初に見つかった要素のみを返します。FindAllは見つかった全てです。
            //
            // Predicateデリゲートの書式は、匿名デリゲートで処理する場合
            //      bool xxxx(T obj)
            // です。
            //
            Array.Clear(ary, 0, ary.Length);
            ary = new[] {"1", "2", "3"};

            // 配列から奇数のデータのみを抜き出し、それを表示.
            Array.ForEach(
                Array.FindAll(
                    ary,
                    element => int.Parse(element!)%2 != 0
                ),
                element => Output.WriteLine(element));

            //
            // ConvertAllメソッド
            //
            // このメソッドは、Arrayのstaticメソッドとして定義されていて、
            // 配列の各要素の型を変換して別の型の配列を構築する処理を行います。
            // 第二引数に渡すのは、Converter<TInput, TOutput>のデリゲートオブジェクトです。
            //
            // Converterデリゲートは、型引数が2つ存在するので、そのまま匿名デリゲートには
            // できません。以下のようにConvertAllメソッドの方に型引数をつけます。
            //
            //      Array.ConvertAll<string, int>(ary, delegate(string value){});
            //
            ary = new[] {"100", "200", "300"};

            Array.ForEach(
                Array.ConvertAll(
                    ary,
                    element => int.Parse(element!)
                ),
                element => Output.WriteLine("Type={0}, Value={1}", element.GetType().FullName, element)
            );

            //
            // TrueForAllメソッド
            //
            // このメソッドは、Arrayのstaticメソッドとして定義されていて、
            // 配列の各要素に対して条件をかけ、全ての要素が条件をパス(true)
            // した場合のみtrueを返します。
            // 第二引数に指定するのは、Predicate<T>のデリゲートオブジェクトです。
            //
            // デリゲートの書式は、FindAllの場合と同じです。
            //
            Output.WriteLine(
                "配列の要素全てがnullではない値であるか？：{0}",
                Array.TrueForAll(
                    ary,
                    element => !string.IsNullOrEmpty(element)
                )
            );

            //
            // Existsメソッド
            //
            // このメソッドは、Arrayのstaticメソッドとして定義されていて、
            // 配列の各要素に対して条件をかけ、その内の一つでも条件をパス(true)
            // した場合にtrueを返します。TrueForAllが全ての要素(all)に対して
            // こちらは、どれか一つ(any)です。
            // 第二引数で指定するのは、TrueForAllと同じくPredicate<T>のデリゲートオブジェクトです。
            //
            ary = new[] {"100", null, "300"};

            Output.WriteLine(
                "配列の要素の内ひとつでもnullが存在するか？：{0}",
                Array.Exists(
                    ary,
                    element => string.IsNullOrEmpty(element)
                )
            );

            //
            // Resizeメソッド
            //
            // このメソッドは、Arrayのstaticメソッドとして定義されていて、
            // 文字通り配列をリサイズします。
            // 元の配列より大きいサイズでリサイズした場合は、元の配列の全データ
            // の後に, 初期値で埋められます。
            // 元の配列より小さいサイズでリサイズした場合は、リサイズ後の上限まで
            // 元のデータが埋められます。
            //
            ary = new[] {"1000", "2000", "3000"};

            Output.WriteLine("現在の配列のサイズ：{0}", ary.Length);
            Array.ForEach(ary, v => Output.WriteLine(v));
            Array.Resize(ref ary, ary.Length + 10);
            Output.WriteLine("リサイズ後の配列のサイズ：{0}", ary.Length);
            Array.ForEach(ary, v => Output.WriteLine(v));
            Array.Resize(ref ary, 1);
            Output.WriteLine("リサイズ後の配列のサイズ：{0}", ary.Length);
            Array.ForEach(ary, v => Output.WriteLine(v));

            //
            // 配列をリストとして扱う.
            // (IListへのキャスト.)
            //
            foreach (var v in new[] {"gsf_zero1", "gsf_zero2", "gsf_zero3"})
            {
                Output.WriteLine(v);
            }
        }
    }
}