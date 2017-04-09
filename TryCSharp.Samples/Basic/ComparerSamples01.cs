using System;
using System.Collections.Generic;
using TryCSharp.Common;

namespace TryCSharp.Samples.Basic
{
    /// <summary>
    ///     Comparerについてのサンプルです。
    /// </summary>
    [Sample]
    public class ComparerSamples01 : IExecutable
    {
        public void Execute()
        {
            //
            // IComparerインターフェース及びIComparer<T>インターフェースは、ともに順序の比較をサポートするための
            // インターフェースである。
            //
            // 同じ目的で利用されるインターフェースに、IComparableインターフェースが存在するが、違いは
            // IComparableインターフェースが、対象となるクラス自身に実装する必要があるのに対して
            // IComparerインターフェースは、個別に比較処理のみを実装したクラスを用意することにある。
            //
            // これにより、同じオブジェクトに対して、複数の比較方法を実装することが出来る。
            // (ソート処理を行う際に、比較処理を担当するオブジェクトを選択することができるようになる。）
            //
            // List.SortやSortedListやSortedDictionaryがこれをサポートする。
            //
            var person1 = new Person {Id = 1, Name = "gsf_zero1"};
            var person2 = new Person {Id = 2, Name = "gsf_zero2"};
            var person3 = new Person {Id = 3, Name = "gsf_zero3"};
            var person4 = new Person {Id = 4, Name = "gsf_zero1"};

            var persons = new List<Person> {person3, person2, person4, person1};

            // ソートせずにそのまま出力.
            persons.ForEach(Output.WriteLine);

            // Idで比較処理を行うComparerを指定してソート.
            Output.WriteLine(string.Empty);
            persons.Sort(new PersonIdComparer());
            persons.ForEach(Output.WriteLine);

            // NAMEで比較処理を行うComparerを指定してソート.
            Output.WriteLine(string.Empty);
            persons.Sort(new PersonNameComparer());
            persons.ForEach(Output.WriteLine);
        }

        private enum CompareResult
        {
            SMALL = -1,
            EQUAL = 0,
            BIG = 1
        }

        private class Person
        {
            public int Id { get; set; }
            public string Name { get; set; }

            public override string ToString()
            {
                return string.Format("[ID={0}, NAME={1}]", Id, Name);
            }
        }

        //
        // Comparer<T>クラスは、抽象クラスとなっており。
        // IComparerインターフェースとIComparer<T>インターフェースの両方を実装している。
        //
        // 実際に、実装する必要があるのは以下のメソッドだけである。
        //   int Compare(T x, T y)
        //
        // IComparer.Compareメソッドについては、抽象クラス側にて明示的実装が行われている。
        //
        private class PersonIdComparer : Comparer<Person>
        {
            public override int Compare(Person x, Person y)
            {
                if (Equals(x, y))
                {
                    return (int) CompareResult.EQUAL;
                }

                var xId = x.Id;
                var yId = y.Id;

                return xId.CompareTo(yId);
            }
        }

        private class PersonNameComparer : Comparer<Person>
        {
            public override int Compare(Person x, Person y)
            {
                if (Equals(x, y))
                {
                    return (int) CompareResult.EQUAL;
                }

                var xName = x?.Name;
                var yName = y?.Name;

                return string.Compare(xName, yName, StringComparison.Ordinal);
            }
        }
    }
}