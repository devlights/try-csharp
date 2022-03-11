using System;
using System.Collections.Generic;
using TryCSharp.Common;

namespace TryCSharp.Samples.Basic
{
    /// <summary>
    ///     EqualityComparerのサンプルです。
    /// </summary>
    [Sample]
    public class EqualityComparerSamples01 : IExecutable
    {
        public void Execute()
        {
            var d1 = new Data("data1", "data1-value1");
            var d2 = new Data("data2", "data2-value1");
            var d3 = new Data("data3", "data3-value1");

            // d1と同じ値を持つ別のインスタンスを作成しておく.
            var d1_2 = new Data(d1.Name, d1.Value);

            /////////////////////////////////////////////////////////
            //
            // object.Equalsで比較.
            //
            Output.WriteLine("===== object.Equalsで比較. =====");
            Output.WriteLine("d1.Equals(d2) : {0}", d1.Equals(d2));
            Output.WriteLine("d1.Equals(d3) : {0}", d1.Equals(d3));
            Output.WriteLine("d1.Equals(d1_2) : {0}", d1.Equals(d1_2));

            /////////////////////////////////////////////////////////
            //
            // EqualityComparerで比較.
            //
            var comparer = new DataEqualityComparer();

            Output.WriteLine("===== EqualityComparerで比較. =====");
            Output.WriteLine("d1.Equals(d2) : {0}", comparer.Equals(d1, d2));
            Output.WriteLine("d1.Equals(d3) : {0}", comparer.Equals(d1, d3));
            Output.WriteLine("d1.Equals(d1_2) : {0}", comparer.Equals(d1, d1_2));

            /////////////////////////////////////////////////////////
            //
            // Dictionaryで一致するか否かを確認 (EqualityComparer無し)
            //
            var dict1 = new Dictionary<Data, string>();

            dict1[d1] = d1.Value;
            dict1[d2] = d2.Value;
            dict1[d3] = d3.Value;

            // 以下のコードでは、ちゃんと値が取得できる. (参照が同じため)
            Output.WriteLine("===== Dictionaryで一致するか否かを確認 (EqualityComparer無し). =====");
            Output.WriteLine("key:d1 ==> {0}", dict1[d1]);
            Output.WriteLine("key:d3 ==> {0}", dict1[d3]);

            // 以下のコードでは、ちゃんとtrueが取得できる. (参照が同じため)
            Output.WriteLine("contains-key: d1 ==> {0}", dict1.ContainsKey(d1));
            Output.WriteLine("contains-key: d2 ==> {0}", dict1.ContainsKey(d2));
            Output.WriteLine("contains-key: d3 ==> {0}", dict1.ContainsKey(d3));

            //
            // 同じ値を持つ、別インスタンスを作成し、EqualityComparerなしのDictionaryで試してみる.
            //
            var d4 = new Data(d1.Name, d1.Value);
            var d5 = new Data(d2.Name, d2.Value);
            var d6 = new Data(d3.Name, d3.Value);

            // 以下のコードを実行すると例外が発生する. (キーとして一致しないため)
            try
            {
                Output.WriteLine("===== 同じ値を持つ、別インスタンスを作成し、EqualityComparerなしのDictionaryで試してみる. =====");
                Output.WriteLine("key:d4 ==> {0}", dict1[d4]);
            }
            catch (KeyNotFoundException)
            {
                Output.WriteLine("キーとしてd4を指定しましたが、一致するキーが見つかりませんでした。");
            }

            // 当然、ContainsKeyメソッドもfalseを返す.
            Output.WriteLine("contains-key: d4 ==> {0}", dict1.ContainsKey(d4));


            /////////////////////////////////////////////////////////
            //
            // Dictionaryを作成する際に、EqualityComparerを指定して作成.
            //
            var dict2 = new Dictionary<Data, string>(comparer);

            dict2[d1] = d1.Value;
            dict2[d2] = d2.Value;
            dict2[d3] = d3.Value;

            // 以下のコードでは、ちゃんと値が取得できる. (EqualityComparerを指定しているため)
            Output.WriteLine("===== Dictionaryを作成する際に、EqualityComparerを指定して作成. =====");
            Output.WriteLine("key:d4 ==> {0}", dict2[d4]);
            Output.WriteLine("key:d6 ==> {0}", dict2[d6]);

            // 以下のコードでは、ちゃんとtrueが取得できる. (EqualityComparerを指定しているため)
            Output.WriteLine("contains-key: d4 ==> {0}", dict2.ContainsKey(d4));
            Output.WriteLine("contains-key: d5 ==> {0}", dict2.ContainsKey(d5));
            Output.WriteLine("contains-key: d6 ==> {0}", dict2.ContainsKey(d6));

            /////////////////////////////////////////////////////////
            //
            // EqualityComparer<T>には、Defaultという静的プロパティが存在する.
            // このプロパティは、Tに指定された型がIEquatable<T>を実装しているかどうかを
            // チェックし、実装している場合は、内部でIEquatable<T>の実装を利用する
            // EqualityComaparer<T>を作成して返してくれる.
            //
            // Tに指定された型が、IEquatable<T>を実装していない場合
            // object.Equals, object.GetHashCodeを利用する実装を返す.
            //
            // 本サンプルで利用するサンプルクラスは、以下のようになっている.
            //   Dataクラス： IEquatable<T>を実装していない.
            //   Data2クラス： IEquatable<T>を実装している.
            //
            // 上記のクラスに対して、それぞれEqualityComparer<T>.Defaultを呼び出すと以下の
            // クラスのインスタンスが返ってくる.
            //   Dataクラス：  ObjectEqualityComparer`1
            //   Data2クラス: GenericEqualityComparer`1
            // IEquatable<T>を実装している場合は、GenericEqualityComparerが
            // 実装していない場合は、ObjectEqualityComparerとなる。
            //
            var dataEqualityComparer = EqualityComparer<Data>.Default;
            var data2EqualityComparer = EqualityComparer<Data2>.Default;

            // 生成された型を表示.
            Output.WriteLine("===== EqualityComparer<T>.Defaultの動作. =====");
            Output.WriteLine("Data={0}, Data2={1}", dataEqualityComparer.GetType().Name, data2EqualityComparer.GetType().Name);

            // それぞれサンプルデータを作成して、比較してみる.
            // 尚、どちらの場合も1番目のデータと3番目のデータのキーが同じになるようにしている.
            var data_1 = new Data("data_1", "value_1");
            var data_2 = new Data("data_2", "value_2");
            var data_3 = new Data("data_1", "value_3");

            var data2_1 = new Data2("data2_1", "value2_1");
            var data2_2 = new Data2("data2_2", "value2_2");
            var data2_3 = new Data2("data2_1", "value2_3");

            // DataクラスのEqualityComparerを使用して比較.
            Output.WriteLine("data_1.Equals(data_2) : {0}", dataEqualityComparer.Equals(data_1, data_2));
            Output.WriteLine("data_1.Equals(data_3) : {0}", dataEqualityComparer.Equals(data_1, data_3));

            // Data2クラスのEqualityComparerを使用して比較.
            Output.WriteLine("data2_1.Equals(data2_2) : {0}", data2EqualityComparer.Equals(data2_1, data2_2));
            Output.WriteLine("data2_1.Equals(data2_3) : {0}", data2EqualityComparer.Equals(data2_1, data2_3));
        }

        private class Data
        {
            public Data(string name, string value)
            {
                Name = name;
                Value = value;
            }

            public string Name { get; }

            public string Value { get; }

            public override string ToString()
            {
                return string.Format("Name={0}, Value={1}", Name, Value);
            }
        }

        private class DataEqualityComparer : EqualityComparer<Data>
        {
            public override bool Equals(Data? x, Data? y)
            {
                if (x == null && y == null)
                {
                    return true;
                }

                if (x == null || y == null)
                {
                    return false;
                }

                return x.Name == y.Name;
            }

            public override int GetHashCode(Data x)
            {
                if (x == null || string.IsNullOrEmpty(x.Name))
                {
                    return string.Empty.GetHashCode();
                }

                return x.Name.GetHashCode();
            }
        }

        private class Data2 : IEquatable<Data2>
        {
            public Data2(string name, string value)
            {
                Name = name;
                Value = value;
            }

            public string Name { get; }

            public string Value { get; private set; }

            public bool Equals(Data2? other)
            {
                if (other == null)
                {
                    return false;
                }

                return other.Name == Name;
            }

            public override bool Equals(object? other)
            {
                var data = other as Data2;
                if (data == null)
                {
                    return false;
                }

                return Equals(data);
            }

            public override int GetHashCode()
            {
                return string.IsNullOrEmpty(Name) ? string.Empty.GetHashCode() : Name.GetHashCode();
            }
        }
    }
}