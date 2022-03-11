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
    public class LinqSamples11 : IExecutable
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
        private readonly IEnumerable<Project> projects =
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
            // グループ化結合のサンプル
            //
            // グループ化結合したものにDefaultIfEmptyメソッドを適用したものが
            // Linqでの左外部結合となる。
            //

            //
            // 外部結合していない版.
            //
            // 以下のクエリの場合、gsf_zero5が表示されない。
            // (最後のfrom personProject in personProjectsの部分で除外される。）
            var query = from person in persons
                    join prj in
                    (
                        from project in projects
                        from member in project.Members!
                        select new {project.Id, project.Name, Member = member}
                    ) on person.Id equals prj.Member into personProjects
                    from personProject in personProjects
                    select new
                    {
                        person.Id,
                        person.Name,
                        Project = personProject.Name
                    };

            query.ToList().ForEach(Output.WriteLine);

            //
            // 外部結合版
            //
            var query2 = from person in persons
                    join prj in
                    (
                        from project in projects
                        from member in project.Members!
                        select new {project.Id, project.Name, Member = member}
                    ) on person.Id equals prj.Member into personProjects
                    // 外部結合するためにDefaultIfEmptyを使用.
                    from personProject in personProjects.DefaultIfEmpty()
                    select new
                    {
                        person.Id,
                        person.Name,
                        // 結合対象が存在しなかった場合、その型のdefault(T)の値となってので
                        // 望みの形に変換する.
                        Project = personProject == null ? "''" : personProject.Name
                    };

            Output.WriteLine("======================================================");
            query2.ToList().ForEach(Output.WriteLine);
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