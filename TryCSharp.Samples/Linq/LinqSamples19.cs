using System.Collections.Generic;
using System.Linq;
using TryCSharp.Common;

namespace TryCSharp.Samples.Linq
{
    /// <summary>
    ///     Linqのサンプルです。
    /// </summary>
    [Sample]
    public class LinqSamples19 : IExecutable
    {
        public void Execute()
        {
            var persons = new List<Person>
            {
                new Person {Id = 1, Name = "gsf_zero1"},
                new Person {Id = 2, Name = "gsf_zero2"},
                new Person {Id = 3, Name = "gsf_zero3"}
            };

            //
            // Where拡張メソッドを利用。
            //   クエリ式で利用しているwhere句と利用方法は同じ。
            //   Where拡張メソッドは、２つのオーバーロードをもっており
            //
            //   一つは、Func<T, bool>のタイプ。もう一つは、Func<T, int, bool>のタイプとなる。
            //   2つ目のタイプの場合、その時のインデックスが引数として設定される。
            //
            Output.WriteLine("===== Func<T, bool>のタイプ =====");
            foreach (var aPerson in persons.Where(item => item.Id == 2))
            {
                Output.WriteLine("ID={0}, NAME={1}", aPerson.Id, aPerson.Name);
            }

            Output.WriteLine("===== Func<T, int, bool>のタイプ =====");
            foreach (var aPerson in persons.Where((item, index) => index == 2))
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