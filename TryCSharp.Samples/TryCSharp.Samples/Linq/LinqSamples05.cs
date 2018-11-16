using System.Collections.Generic;
using System.Linq;
using TryCSharp.Common;

namespace TryCSharp.Samples.Linq
{
    /// <summary>
    ///     Linqのサンプルです。
    /// </summary>
    [Sample]
    public class LinqSamples05 : IExecutable
    {
        public void Execute()
        {
            var persons = CreateSampleData();

            //
            // into句を使用しない標準的なgroupの利用
            // (Person.Countryでグルーピング)
            //
            var query1 = from person in persons
                    group person by person.Country;

            //
            // 結果は、キーと該当するグループの状態で取得できる.
            //
            foreach (var groupedPerson in query1)
            {
                // キー
                Output.WriteLine("Country={0}", groupedPerson.Key);

                // グループ
                // グループを取得するには、もう一度ループする必要がある。
                foreach (var person in groupedPerson)
                {
                    Output.WriteLine("\tId={0}, Name={1}", person.Id, person.Name);
                }
            }

            //
            // 同じ結果を、var無しで表現.
            //
            // groupキーワードの結果は以下の型となる。
            //    IGrouping<TKey, TElement>
            // IGroupingインターフェースは、IEnumerable<TElement>を
            // 継承しているので、ループさせるとTElementが順次取得できるようになっている。
            //
            foreach (var groupedPerson in query1)
            {
                Output.WriteLine("Country={0}", groupedPerson.Key);

                foreach (var person in groupedPerson)
                {
                    Output.WriteLine("\tId={0}, Name={1}", person.Id, person.Name);
                }
            }
        }

        private IEnumerable<Person> CreateSampleData()
        {
            return new[]
            {
                new Person
                {
                    Id = "00001"
                    , Name = "gsf_zero1"
                    , Address = new AddressInfo
                    {
                        PostCode = "999-8888"
                        , Prefecture = "東京都"
                        , Municipality = "どこか１"
                        , HouseNumber = "番地１"
                        , Tel = new[] {"090-xxxx-xxxx"}
                        , Frends = new string[] {}
                    }
                    , Country = Country.Japan
                }
                , new Person
                {
                    Id = "00002"
                    , Name = "gsf_zero2"
                    , Address = new AddressInfo
                    {
                        PostCode = "888-7777"
                        , Prefecture = "京都府"
                        , Municipality = "どこか２"
                        , HouseNumber = "番地２"
                        , Tel = new[] {"080-xxxx-xxxx"}
                        , Frends = new[] {"00001"}
                    }
                    , Country = Country.Japan
                }
                , new Person
                {
                    Id = "00003"
                    , Name = "gsf_zero3"
                    , Address = new AddressInfo
                    {
                        PostCode = "777-6666"
                        , Prefecture = "北海道"
                        , Municipality = "どこか３"
                        , HouseNumber = "番地３"
                        , Tel = new[] {"070-xxxx-xxxx"}
                        , Frends = new[] {"00001", "00002"}
                    }
                    , Country = Country.America
                }
                , new Person
                {
                    Id = "00004"
                    , Name = "gsf_zero4"
                    , Address = new AddressInfo
                    {
                        PostCode = "777-6666"
                        , Prefecture = "北海道"
                        , Municipality = "どこか４"
                        , HouseNumber = "番地４"
                        , Tel = new[] {"060-xxxx-xxxx", "111-111-1111", "222-222-2222"}
                        , Frends = new[] {"00001", "00003"}
                    }
                    , Country = Country.America
                }
            };
        }

        private enum Country
        {
            Japan,
            America,
            China
        }

        private class Person
        {
            public string Id { get; set; }

            public string Name { get; set; }

            public AddressInfo Address { get; set; }

            public Country Country { get; set; }
        }

        private class AddressInfo
        {
            public string PostCode { get; set; }

            public string Prefecture { get; set; }

            public string Municipality { get; set; }

            public string HouseNumber { get; set; }

            public string[] Tel { get; set; }

            public string[] Frends { get; set; }
        }
    }
}