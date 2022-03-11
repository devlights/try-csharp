using System;
using System.Collections.Generic;
using System.Linq;
using TryCSharp.Common;

namespace TryCSharp.Samples.Linq
{
    /// <summary>
    ///     Linqのサンプルです。
    /// </summary>
    [Sample]
    public class LinqSamples01 : IExecutable
    {
        public void Execute()
        {
            var persons = CreateSampleData();

            var query = from person in persons
                    where int.Parse(person.Id!) >= 2
                    select person;

            try
            {
                foreach (var person in query)
                {
                    Output.WriteLine("{0},{1}", person.Id, person.Name);
                }
            }
            catch (Exception ex)
            {
                Output.WriteLine("クエリ実行時にエラーが発生: {0}", ex.Message);
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
                        , Tel = "090-xxxx-xxxx"
                    }
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
                        , Tel = "080-xxxx-xxxx"
                    }
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
                        , Tel = "070-xxxx-xxxx"
                    }
                }
                , new Person
                {
                    Id = "0000x"
                    , Name = "gsf_zero3"
                    , Address = new AddressInfo
                    {
                        PostCode = "777-6666"
                        , Prefecture = "北海道"
                        , Municipality = "どこか３"
                        , HouseNumber = "番地３"
                        , Tel = "070-xxxx-xxxx"
                    }
                }
            };
        }

        private class Person
        {
            public string? Id { get; set; }

            public string? Name { get; set; }

            public AddressInfo? Address { get; set; }
        }

        private class AddressInfo
        {
            public string? PostCode { get; set; }

            public string? Prefecture { get; set; }

            public string? Municipality { get; set; }

            public string? HouseNumber { get; set; }

            public string? Tel { get; set; }
        }
    }
}