using System.Collections.Generic;
using System.Linq;
using TryCSharp.Common;

namespace TryCSharp.Samples.Linq
{
    /// <summary>
    ///     Linqのサンプルです。
    /// </summary>
    [Sample]
    public class LinqSamples28 : IExecutable
    {
        public void Execute()
        {
            //
            // 引数なしのDistinct拡張メソッドを利用.
            // この場合、既定のIEqualityComparer<T>を用いて比較が行われる。
            // 
            var numbers = new[]
            {
                1, 2, 3, 4, 5,
                1, 2, 3, 6, 7
            };

            Output.WriteLine(JoinElements(numbers.Distinct()));

            //
            // 引数にIEqualityComparer<T>を指定して、Distinct拡張メソッドを利用。
            // この場合、引数に指定したComparerを用いて比較が行われる。
            //
            var people = new[]
            {
                new Person {Name = "gsf_zero1"},
                new Person {Name = "gsf_zero2"},
                new Person {Name = "gsf_zero1"},
                new Person {Name = "gsf_zero3"}
            };

            Output.WriteLine(JoinElements(people.Distinct(new PersonComparer())));
        }

        private string JoinElements<T>(IEnumerable<T> elements)
        {
            return string.Join(",", elements.Select(item => item.ToString()));
        }

        private class Person
        {
            public string Name { get; set; }

            public override string ToString()
            {
                return $"[NAME = {Name}]";
            }
        }

        private class PersonComparer : EqualityComparer<Person>
        {
            public override bool Equals(Person p1, Person p2)
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
                return p.Name.GetHashCode();
            }
        }
    }
}