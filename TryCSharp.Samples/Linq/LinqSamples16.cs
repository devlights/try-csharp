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
    public class LinqSamples16 : IExecutable
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
            // OfTypeメソッドを利用することにより、特定の型のみのシーケンスに変換することができる。
            // 例えば、リストの中に基底クラスであるPersonクラスと派生クラスであるCustomerクラスの
            // オブジェクトが混在している場合、OfType<Person>とすると、そのまま。
            // OfType<Customer>とすると、Customerオブジェクトのみのシーケンスに変換される。
            //
            // 尚、OfTypeメソッドは他の変換演算子とは違い、ソースシーケンスのスナップショットを作成しない。
            // つまり、通常のクエリと同じく、OfTypeで取得したシーケンスが列挙される度に評価される。
            // 変換演算子の中で、このような動作を行うのはAsEnumerableとOfTypeとCastである。
            //
            Output.WriteLine("========== OfType<Person>の結果 ==========");
            foreach (var data in persons.OfType<Person>())
            {
                Output.WriteLine(data);
            }

            Output.WriteLine("========== OfType<Customer>の結果 ==========");
            foreach (var data in persons.OfType<Customer>())
            {
                Output.WriteLine(data);
            }

            /*
              IEnumerable<Customer> c = persons.OfType<Customer>();
              persons.Add(new Customer { Id = 99, Name = "gsf_zero3", Orders = Enumerable.Empty<Order>() });
              
              foreach (var data in c)
              {
                Output.WriteLine(data);
              }
            */

            //
            // 元々GenericではないリストをIEnumerable<T>に変換する場合にも利用出来る.
            //
            var arrayList = new ArrayList();
            arrayList.Add(10);
            arrayList.Add(20);
            arrayList.Add(30);
            arrayList.Add(40);

            Output.WriteLine("========== Genericではないコレクションを変換 ==========");
            var intList = arrayList.OfType<int>();
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