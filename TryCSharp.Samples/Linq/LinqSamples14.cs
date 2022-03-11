using System.Collections.Generic;
using System.Linq;
using TryCSharp.Common;

namespace TryCSharp.Samples.Linq
{
    /// <summary>
    ///     Linqのサンプルです。
    /// </summary>
    [Sample]
    public class LinqSamples14 : IExecutable
    {
        public void Execute()
        {
            var persons = new List<Person>
            {
                new Person {Id = 1, Name = "gsf_zero1"},
                new Person {Id = 2, Name = "gsf_zero2"},
                new Person {Id = 3, Name = "gsf_zero3"},
                new Person {Id = 4, Name = "gsf_zero4"},
                new Person {Id = 5, Name = "gsf_zero5"}
            };

            var query = from aPerson in persons
                    where aPerson.Id%2 != 0
                    select aPerson;

            Output.WriteLine("============ クエリを表示 ============");
            foreach (var aPerson in query)
            {
                Output.WriteLine("ID={0}, NAME={1}", aPerson.Id, aPerson.Name);
            }

            //
            // ToDictionaryを利用して、明示的にマップに変換.
            // (このタイミングでクエリが評価され、結果が構築される。)
            //
            // ToDictionaryは、キー要素を抽出するための引数を要求する。(keySelector)
            // 引数にkeySelectorのみを指定している場合、マップの値はそのオブジェクト自身となる。
            //   キー：int32型 (Person.Id)
            //   値　：Personオブジェクト
            //
            // このほかにも、elementSelectorを指定するオーバーロードも存在する。
            //
            var filteredPersons = query.ToDictionary(item => item.Id);

            Output.WriteLine("============ ToDictionaryで作成したリストを表示 ============");
            foreach (var pair in filteredPersons)
            {
                Output.WriteLine("KEY={0}, VALUE={1}", pair.Key, pair.Value);
            }

            //
            // 元のリストを変更.
            //
            persons.Add(new Person {Id = 6, Name = "gsf_zero6"});
            persons.Add(new Person {Id = 7, Name = "gsf_zero7"});

            //
            // もう一度、各結果を表示.
            //
            Output.WriteLine("============ クエリを表示（2回目） ============");
            foreach (var aPerson in query)
            {
                Output.WriteLine("ID={0}, NAME={1}", aPerson.Id, aPerson.Name);
            }

            Output.WriteLine("============ ToDictionaryで作成したリストを表示 （2回目）============");
            foreach (var pair in filteredPersons)
            {
                Output.WriteLine("KEY={0}, VALUE={1}", pair.Key, pair.Value);
            }

            //
            // ToDictionaryメソッドにkeySelectorとelementSelectorを指定したバージョン.
            //
            Output.WriteLine("============ keySelectorとelementSelectorの両方を指定したバージョン ============");
            foreach (var pair in query.ToDictionary(item => item.Id, item => string.Format("{0}_{1}", item.Id, item.Name)))
            {
                Output.WriteLine("KEY={0}, VALUE={1}", pair.Key, pair.Value);
            }
        }

        private class Person
        {
            public int Id { get; set; }
            public string? Name { get; set; }

            public override string ToString()
            {
                return string.Format("[{0}, {1}]", Id, Name);
            }
        }
    }
}