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
    public class LinqSamples08 : IExecutable
    {
        public enum ProjectState
        {
            NotStarted,
            Processing,
            Done
        }

        // メンバー
        private readonly IEnumerable<Person> persons =
                new[]
                {
                    new Person
                    {
                        Id = "001"
                        , Name = "gsf_zero1"
                        , Age = 30
                        , Birthday = new DateTime(1979, 11, 18)
                    }
                    , new Person
                    {
                        Id = "002"
                        , Name = "gsf_zero2"
                        , Age = 20
                        , Birthday = new DateTime(1989, 11, 18)
                    }
                    , new Person
                    {
                        Id = "003"
                        , Name = "gsf_zero3"
                        , Age = 18
                        , Birthday = new DateTime(1991, 11, 18)
                    }
                    , new Person
                    {
                        Id = "004"
                        , Name = "gsf_zero4"
                        , Age = 40
                        , Birthday = new DateTime(1969, 11, 18)
                    }
                    , new Person
                    {
                        Id = "005"
                        , Name = "gsf_zero5"
                        , Age = 44
                        , Birthday = new DateTime(1965, 11, 18)
                    }
                };

        // プロジェクト
        private IEnumerable<Project> projects =
                new[]
                {
                    new Project
                    {
                        Id = "001"
                        , Name = "project_1"
                        , Members = new[] {"001", "002"}
                        , From = new DateTime(2009, 8, 1)
                        , To = new DateTime(2009, 10, 15)
                        , State = ProjectState.Done
                    }
                    , new Project
                    {
                        Id = "002"
                        , Name = "project_2"
                        , Members = new[] {"003", "004"}
                        , From = new DateTime(2009, 9, 1)
                        , To = new DateTime(2009, 11, 25)
                        , State = ProjectState.Done
                    }
                    , new Project
                    {
                        Id = "003"
                        , Name = "project_3"
                        , Members = new[] {"001"}
                        , From = new DateTime(2007, 1, 10)
                        , To = null
                        , State = ProjectState.Processing
                    }
                    , new Project
                    {
                        Id = "004"
                        , Name = "project_4"
                        , Members = new[] {"001", "002", "003", "004"}
                        , From = new DateTime(2010, 1, 5)
                        , To = null
                        , State = ProjectState.NotStarted
                    }
                };

        // チーム
        private IEnumerable<Team> teams =
                new[]
                {
                    new Team
                    {
                        Id = "001"
                        , Name = "team_1"
                        , Members = new[] {"001", "004"}
                    }
                    , new Team
                    {
                        Id = "002"
                        , Name = "team_2"
                        , Members = new[] {"002", "003"}
                    }
                };

        public void Execute()
        {
            //
            // letのサンプル
            // letは一時的な変数をクエリ内で作成する際に利用する。
            //
            // 各年齢層のカウントを算出.
            //
            var query1 = from person in persons
                    let ageLevel = Math.Truncate(person.Age*0.1)*10
                    group person by ageLevel
                    into personByAge
                    orderby personByAge.Key ascending
                    select new
                    {
                        AgeLevel = personByAge.Key,
                        Count = personByAge.Count()
                    };

            foreach (var item in query1)
            {
                Output.WriteLine(item);
            }
        }

        public class Person
        {
            public string? Id { get; set; }

            public string? Name { get; set; }

            public int Age { get; set; }

            public DateTime Birthday { get; set; }
        }

        public class Team
        {
            public string? Id { get; set; }

            public string? Name { get; set; }

            public IEnumerable<string>? Members { get; set; }
        }

        public class Project
        {
            public string? Id { get; set; }

            public string? Name { get; set; }

            public IEnumerable<string>? Members { get; set; }

            public DateTime From { get; set; }

            public DateTime? To { get; set; }

            public ProjectState State { get; set; }
        }
    }
}