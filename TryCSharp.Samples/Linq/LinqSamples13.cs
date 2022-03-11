using System.Collections.Generic;
using System.Linq;
using TryCSharp.Common;

namespace TryCSharp.Samples.Linq
{
    /// <summary>
    ///     Linqのサンプルです。
    /// </summary>
    [Sample]
    public class LinqSamples13 : IExecutable
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
                    where aPerson.Id % 2 != 0
                    select aPerson;

            Output.WriteLine("============ クエリを表示 ============");
            foreach (var aPerson in query)
            {
                Output.WriteLine("ID={0}, NAME={1}", aPerson.Id, aPerson.Name);
            }

            //
            // ToListを利用して、明示的にリストに変換.
            // (このタイミングでクエリが評価され、結果が構築される。)
            //
            var filteredPersons = query.ToList();

            Output.WriteLine("============ ToListで作成したリストを表示 ============");
            foreach (var aPerson in filteredPersons)
            {
                Output.WriteLine("ID={0}, NAME={1}", aPerson.Id, aPerson.Name);
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

            Output.WriteLine("============ ToListで作成したリストを表示 （2回目）============");
            foreach (var aPerson in filteredPersons)
            {
                Output.WriteLine("ID={0}, NAME={1}", aPerson.Id, aPerson.Name);
            }
        }

        private class Person
        {
            public int Id { get; set; }
            public string? Name { get; set; }
        }
    }
}