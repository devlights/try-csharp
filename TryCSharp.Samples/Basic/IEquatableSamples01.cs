using System;
using TryCSharp.Common;

namespace TryCSharp.Samples.Basic
{
    /// <summary>
    ///     IEquatable<T>のサンプルです。
    /// </summary>
    [Sample]
    // ReSharper disable once InconsistentNaming
    public class IEquatableSamples01 : IExecutable
    {
        public void Execute()
        {
            //
            // IEquatable<T>インターフェースは、2つのインスタンスが等しいか否かを判別するための
            // 型指定のEqualsメソッドを定義しているインターフェースである。
            //
            // このインターフェースを実装することで、通常のobject.Equals以外に型が指定された
            // Equalsメソッドを持つことができるようになる。
            // このインターフェースは、特に構造体を定義する上で重要であり、構造体の場合、object.Equalsで
            // 比較を行うと、ボックス化が発生してしまうため、IEquatable<T>を実装することが多い。
            // (ボックス化が発生しなくなるため。）
            //
            // また、厳密には必須ではないが、IEquatable<T>を実装する場合、同時に以下のメソッドもオーバーライドするのが普通である。
            //   ・object.Equals
            //   ・object.GetHashCode
            // object.Equalsをオーバーライドするのは、IEquatableを実装していてもクラスによっては、それを無視して強制的にobject.Equalsで
            // 比較する処理が存在するためである。
            //
            // IEquatable<T>インターフェースは、Dictionary<TKey, TValue>, List<T>などのジェネリックコレクションにて
            // Contains, IndexOf, LastIndexOf, Removeなどの各メソッドで等価性をテストする場合に利用される。
            // (ArrayのIndexOfメソッドなどでも同様に利用される。)
            // 同じインターフェースで、比較機能を提供するものとして、IComparable<T>インターフェースがある。
            //
            // object.GetHashCodeをオーバーライドするのは、上の理由によりobject.Equalsがオーバーライドされるため。
            //
            var data1 = new Data(1, "Hello World");
            var data2 = new Data(2, "Hello World2");
            var data3 = new Data(3, "Hello World3");
            var data4 = data3;
            var data5 = new Data(1, "Hello World4");

            Output.WriteLine("data1 equals data2? ==> {0}", data1.Equals(data2));
            Output.WriteLine("data1 equals data3? ==> {0}", data1.Equals(data3));
            Output.WriteLine("data1 equals data4? ==> {0}", data1.Equals(data4));
            Output.WriteLine("data1 equals data5? ==> {0}", data1.Equals(data5));

            object d1 = data1;
            object d2 = data2;
            object d5 = data5;

            Output.WriteLine("data1 equals data2? ==> {0}", d1.Equals(d2));
            Output.WriteLine("data1 equals data5? ==> {0}", d1.Equals(d5));

            Data[] dataArray = {data1, data2, data3, data4, data5};
            Output.WriteLine("IndexOf={0}", Array.IndexOf(dataArray, data3));
        }

        private sealed class Data : IEquatable<Data>
        {
            public Data(int id, string name)
            {
                Id = id;
                Name = name;
            }

            public int Id { get; }

            public string Name { get; private set; }

            // IEquatable<T>の実装.
            public bool Equals(Data other)
            {
                Output.WriteLine("\t→→Call IEquatable.Equals");

                return Id == other?.Id;
            }

            // object.Equals
            public override bool Equals(object other)
            {
                Output.WriteLine("\t→→Call object.Equals");

                var data = other as Data;
                if (data == null)
                {
                    return false;
                }

                return Equals(data);
            }

            // object.GetHashCode
            public override int GetHashCode()
            {
                return Id.GetHashCode();
            }
        }
    }
}