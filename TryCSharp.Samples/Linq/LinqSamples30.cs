using System.Collections.Generic;
using System.Linq;
using TryCSharp.Common;

namespace TryCSharp.Samples.Linq
{
    /// <summary>
    ///     Linqのサンプルです。
    /// </summary>
    [Sample]
    public class LinqSamples30 : IExecutable
    {
        public void Execute()
        {
            //
            // 引数なしのIntersect拡張メソッドを利用.
            // この場合、既定のIEqualityComparer<T>を用いて比較が行われる。
            //
            // Intersect拡張メソッドは、積集合を求める。
            // つまり、両方のシーケンスに存在するデータのみが抽出される。
            // (Unionは和集合、Exceptは差集合となる。）
            // 
            var numbers1 = new[]
            {
                1, 2, 3, 4, 5
            };

            var numbers2 = new[]
            {
                1, 2, 3, 6, 7
            };

            Output.WriteLine("INTERSECT = {0}", JoinElements(numbers1.Intersect(numbers2)));

            //
            // 引数にIEqualityComparer<T>を指定して、Union拡張メソッドを利用。
            // この場合、引数に指定したComparerを用いて比較が行われる。
            //
            var people1 = new[]
            {
                new Person {Name = "gsf_zero1"},
                new Person {Name = "gsf_zero2"},
                new Person {Name = "gsf_zero1"},
                new Person {Name = "gsf_zero3"}
            };

            var people2 = new[]
            {
                new Person {Name = "gsf_zero4"},
                new Person {Name = "gsf_zero5"},
                new Person {Name = "gsf_zero6"},
                new Person {Name = "gsf_zero1"}
            };

            Output.WriteLine("INTERSECT = {0}", JoinElements(people1.Intersect(people2, new PersonComparer())));
        }

        private string JoinElements<T>(IEnumerable<T> elements)
        {
            return string.Join(",", elements.Select(item => item!.ToString()));
        }

        private class Person
        {
            public string? Name { get; set; }

            public override string ToString()
            {
                return $"[NAME = {Name}]";
            }
        }

        private class PersonComparer : EqualityComparer<Person>
        {
            public override bool Equals(Person? p1, Person? p2)
            {
                if (object.Equals(p1, p2))
                {
                    return true;
                }

                if ((p1 == null) || (p2 == null))
                {
                    return false;
                }

                return p1.Name == p2.Name;
            }

            public override int GetHashCode(Person p)
            {
                return p?.Name?.GetHashCode() ?? 0;
            }
        }
    }
}