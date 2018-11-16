using System.Linq;
using TryCSharp.Common;
using TryCSharp.Samples.Commons.Data;

namespace TryCSharp.Samples.Linq
{
    /// <summary>
    ///     Linqのサンプルです。
    /// </summary>
    /// <remarks>
    ///     このサンプルは、以下の2つのLINQサンプルが記述されています。
    ///     ・LinqSamples-18
    ///     ・LinqSamples-19
    ///     さらに以下の事柄にも触れています。
    ///     ・拡張メソッド解決
    /// </remarks>
    [Sample]
    public class LinqSamples18 : IExecutable
    {
        public void Execute()
        {
            var persons = new Persons
            {
                new Person {Id = 1, Name = "gsf_zero1"},
                new Person {Id = 2, Name = "gsf_zero2"},
                new Person {Id = 3, Name = "gsf_zero3"}
            };

            //
            // PersonExtensionが定義されているので
            // そのまま実行すると、Whereの部分にてPersonExtension.Whereが
            // 呼ばれる.
            //
            Output.WriteLine("===== 拡張メソッドを定義した状態でそのままクエリ実行 =====");
            var query = from aPerson in persons
                    where aPerson.Id == 2
                    select aPerson;

            foreach (var aPerson in query)
            {
                Output.WriteLine(aPerson);
            }

            //
            // AsEnumerableメソッドを利用して、PersonsをIEnumerable<Person>に
            // 変換すると、カスタムWhere拡張メソッドは呼ばれない。
            //
            Output.WriteLine("===== AsEnumerableメソッドを利用してから、クエリ実行 =====");
            var query2 = from aPerson in persons.AsEnumerable()
                    where aPerson.Id == 2
                    select aPerson;

            foreach (var aPerson in query2)
            {
                Output.WriteLine(aPerson);
            }
        }
    }
}