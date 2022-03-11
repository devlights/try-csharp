using System.Collections.Generic;
using System.Linq;
using TryCSharp.Common;

namespace TryCSharp.Samples.Linq
{
    /// <summary>
    ///     Linqのサンプルです。
    /// </summary>
    [Sample]
    public class LinqSamples20 : IExecutable
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
            // Select拡張メソッドを利用。
            //   クエリ式で利用しているselect句と利用方法は同じ。
            //   select拡張メソッドは、２つのオーバーロードをもっており
            //
            //   一つは、Func<TSource, TResult>のタイプ。もう一つは、Func<TSource, int, TResult>のタイプとなる。
            //   2つ目のタイプの場合、その時のインデックスが引数として設定される。
            //
            Output.WriteLine("===== Func<TSource, TResult>のタイプ =====");
            foreach (var name in persons.Select(item => item.Name))
            {
                Output.WriteLine("NAME={0}", name);
            }

            Output.WriteLine("===== Func<TSource, int, TResult>のタイプ =====");
            foreach (var name in persons.Select((item, index) => string.Format("{0}_{1}", item.Name, index)))
            {
                Output.WriteLine("NAME={0}", name);
            }
        }

        private class Person
        {
            public int Id { get; set; }
            public string? Name { get; set; }
        }
    }
}