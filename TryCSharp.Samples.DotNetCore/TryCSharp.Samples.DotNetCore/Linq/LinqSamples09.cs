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
    public class LinqSamples09 : IExecutable
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
        private readonly IEnumerable<Team> teams =
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
            // joinを用いた内部結合(INNER JOIN)のサンプル.
            //
            // joinキーワードの構文は以下の通り.
            //    join 内部側の変数 in 内部側のシーケンス on 外部側のキー equals 内部側のキー
            //
            // Linqのjoinでは等結合（つまり等値の場合の結合）のみが行える。
            //
            //====================================================================================
            // Teamから辿るパターン
            var query1 = from team in teams
                    from personId in team.Members
                    join person in persons on personId equals person.Id
                    orderby team.Id ascending, person.Id ascending
                    select new
                    {
                        Team = team.Name,
                        Person = person.Name
                    };

            Output.WriteLine("=========================================");
            foreach (var item in query1)
            {
                Output.WriteLine(item);
            }

            //
            // Personから辿るパターン.
            // joinキーワードを利用せずに手動で結合処理.
            //
            // この場合、上記と同じように
            //
            // from person   in persons
            // from team   in teams
            // join personId in team.Members on person.Id equals personId
            //
            // とすると、コンテストが違うというエラーとなる。
            // これは、join内部で新たなコンテキストが発生するからである。
            // query1では、内部側のシーケンスをjoin内部だけで利用していたので
            // 問題ない.
            //
            var query2 = from person in persons
                    from team in teams
                    where team.Members.Contains(person.Id)
                    orderby team.Id ascending, person.Id ascending
                    select new
                    {
                        Team = team.Name,
                        Person = person.Name
                    };

            Output.WriteLine("=========================================");
            foreach (var item in query2)
            {
                Output.WriteLine(item);
            }

            //
            // query2をjoinで収めるパターン.
            //
            // 上記の例をjoinに収めるためには、join内で目的のシーケンスを
            // 構築する必要がある。そのため、以下の例では目的のシーケンスを作成するために
            // join内にて、さらにサブクエリを作成している。
            //
            var query3 = from person in persons
                    join team in
                    (
                        from team in teams
                        from member in team.Members
                        select new
                        {
                            team.Id,
                            team.Name,
                            PersonId = member
                        }
                    ) on person.Id equals team.PersonId
                    orderby team.Id ascending, person.Id ascending
                    select new
                    {
                        Team = team.Name,
                        Person = person.Name
                    };

            Output.WriteLine("=========================================");
            foreach (var item in query3)
            {
                Output.WriteLine(item);
            }

            //
            // CROSS JOIN.
            //
            // 普通にfromを並べればCROSS JOIN状態となる。
            //
            var query4 = from person in persons
                    from team in teams
                    orderby team.Id ascending, person.Id ascending
                    select new
                    {
                        Team = team.Name,
                        Person = person.Name
                    };

            Output.WriteLine("=========================================");
            foreach (var item in query4)
            {
                Output.WriteLine(item);
            }
        }

        public class Person
        {
            public string Id { get; set; }

            public string Name { get; set; }

            public int Age { get; set; }

            public DateTime Birthday { get; set; }
        }

        public class Team
        {
            public string Id { get; set; }

            public string Name { get; set; }

            public IEnumerable<string> Members { get; set; }
        }

        public class Project
        {
            public string Id { get; set; }

            public string Name { get; set; }

            public IEnumerable<string> Members { get; set; }

            public DateTime From { get; set; }

            public DateTime? To { get; set; }

            public ProjectState State { get; set; }
        }
    }
}