using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TryCSharp.Common;

namespace TryCSharp.Samples.Linq
{
    /// <summary>
    ///     Linqのサンプルです。
    /// </summary>
    [Sample]
    public class LinqSamples17 : IExecutable
    {
        public void Execute()
        {
            var persons = new List<Person>
            {
                new Person {Id = 1, Name = "gsf_zero1"},
                new Person {Id = 2, Name = "gsf_zero2"},
                new Customer {Id = 3, Name = "gsf_zero3", Orders = Enumerable.Empty<Order>()},
                new Customer
                {
                    Id = 4,
                    Name = "gsf_zero4",
                    Orders = new List<Order>
                    {
                        new Order {Id = 1, Quantity = 10},
                        new Order {Id = 2, Quantity = 2}
                    }
                },
                new Person {Id = 5, Name = "gsf_zero5"}
            };

            //
            // Castメソッドを利用することにより、特定の型のみのシーケンスに変換することができる。
            // OfTypeメソッドと違い、Castメソッドは単純にキャスト処理を行う為、キャスト出来ない型が
            // 含まれている場合は例外が発生する。
            // (OfTypeメソッドの場合、除外される。）
            //
            //
            // 尚、Castメソッドは他の変換演算子とは違い、ソースシーケンスのスナップショットを作成しない。
            // つまり、通常のクエリと同じく、Castで取得したシーケンスが列挙される度に評価される。
            // 変換演算子の中で、このような動作を行うのはAsEnumerableとOfTypeとCastである。
            //
            Output.WriteLine("========== Cast<Person>の結果 ==========");
            foreach (var data in persons.Cast<Person>())
            {
                Output.WriteLine(data);
            }

            //////////////////////////////////////////////////////////
            //
            // 以下のpersons.Cast<Customer>()はPersonオブジェクトをCustomerオブジェクトに
            // キャスト出来ない為、例外が発生する。
            //
            Output.WriteLine("========== Cast<Customer>の結果 ==========");
            try
            {
                foreach (var data in persons.Cast<Customer>())
                {
                    Output.WriteLine(data);
                }
            }
            catch (InvalidCastException ex)
            {
                Output.WriteLine(ex.Message);
            }

            /*
              IEnumerable<Person> p = persons.Cast<Person>();
              persons.Add(new Person());
              
              Output.WriteLine("aaa");
              foreach (var a in p)
              {
                Output.WriteLine(a);
              }
            */

            //
            // 元々GenericではないリストをIEnumerable<T>に変換する場合にも利用出来る.
            // 当然、Castメソッドを利用する場合は、コレクション内部のデータが全てキャスト可能で
            // ないといけない。
            //
            var arrayList = new ArrayList();
            arrayList.Add(10);
            arrayList.Add(20);
            arrayList.Add(30);
            arrayList.Add(40);

            Output.WriteLine("========== Genericではないコレクションを変換 ==========");
            var intList = arrayList.Cast<int>();
            foreach (var data in intList)
            {
                Output.WriteLine(data);
            }
        }

        private class Person
        {
            public int Id { get; set; }
            public string? Name { get; set; }
        }

        private class Customer : Person
        {
            public IEnumerable<Order>? Orders { get; set; }
        }

        private class Order
        {
            public int Id { get; set; }
            public int Quantity { get; set; }
        }
    }
}