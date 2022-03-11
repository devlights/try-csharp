using System.Collections.Generic;
using System.Linq;
using TryCSharp.Common;

namespace TryCSharp.Samples.Linq
{
    /// <summary>
    ///     Linqのサンプルです。
    /// </summary>
    [Sample]
    public class LinqSamples31 : IExecutable
    {
        public void Execute()
        {
            //
            // 引数なしのExcept拡張メソッドを利用.
            // この場合、既定のIEqualityComparer<T>を用いて比較が行われる。
            //
            // Except拡張メソッドは、差集合を求める。
            // (Unionは和集合、Intersectは積集合となる。）
            // 
            // このExcept拡張メソッドには、以下の仕様がある。
            //
            //   ・差集合の対象となるのは、1番目の集合のみであり、2番目の集合からは抽出されない。
            //     →つまり、引数で指定する方のシーケンスからは抽出されない。
            //   ・以下MSDNの記述を引用。
            //     「このメソッドは、second に含まれない、first の要素を返します。また、first に含まれない、second の要素は返しません。」
            // 
            var numbers1 = new[]
            {
                1, 2, 3, 4, 5
            };

            var numbers2 = new[]
            {
                1, 2, 3, 6, 7
            };

            Output.WriteLine("EXCEPT = {0}", JoinElements(numbers1.Except(numbers2)));

            //
            // 引数にIEqualityComparer<T>を指定して、Except拡張メソッドを利用。
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

            Output.WriteLine("EXCEPT = {0}", JoinElements(people1.Except(people2, new PersonComparer())));
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