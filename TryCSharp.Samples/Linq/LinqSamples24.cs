using System.Collections.Generic;
using System.Linq;
using TryCSharp.Common;

namespace TryCSharp.Samples.Linq
{
    /// <summary>
    ///     Linqのサンプルです。
    /// </summary>
    [Sample]
    public class LinqSamples24 : IExecutable
    {
        public void Execute()
        {
            IEnumerable<int> numbers = new[] {1, 2, 3, 4, 5};
            IEnumerable<Person> persons = new List<Person>
            {
                new Person {Id = 1001, Name = "gsf_zero1"},
                new Person {Id = 1000, Name = "gsf_zero2"},
                new Person {Id = 111, Name = "gsf_zero3"},
                new Person {Id = 9889, Name = "gsf_zero4"},
                new Person {Id = 9889, Name = "gsf_zero5"},
                new Person {Id = 100, Name = "gsf_zero6"}
            };

            //
            // Reverse拡張メソッドは、文字通りソースシーケンスを逆順に変換するメソッドである。
            // このメソッドは、そのままソースシーケンスを逆順に変換するだけである。
            //
            // 尚、本メソッドは、他のLINQ演算子と同様に遅延実行される。
            //
            var reverseNumbers = numbers.Reverse();
            var reversePersons = persons.Reverse();

            Output.WriteLine(string.Join(",", reverseNumbers.Select(element => element.ToString())));
            Output.WriteLine(string.Join(",", reversePersons.Select(element => element.ToString())));
        }

        private class Person
        {
            public int Id { get; set; }
            public string? Name { get; set; }

            public override string ToString()
            {
                return string.Format("[ID={0}, NAME={1}]", Id, Name);
            }
        }
    }
}