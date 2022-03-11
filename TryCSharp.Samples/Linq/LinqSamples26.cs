using System.Collections.Generic;
using System.Linq;
using TryCSharp.Common;

namespace TryCSharp.Samples.Linq
{
    /// <summary>
    ///     Linqのサンプルです。
    /// </summary>
    [Sample]
    public class LinqSamples26 : IExecutable
    {
        public void Execute()
        {
            var t1 = new Team {Name = "Team 1"};
            var t2 = new Team {Name = "Team 2"};

            var p1 = new Person {Name = "gsf_zero1", Team = t1};
            var p2 = new Person {Name = "gsf_zero2", Team = t2};
            var p3 = new Person {Name = "gsf_zero3", Team = t1};

            var teams = new List<Team> {t1, t2};
            var people = new List<Person> {p1, p2, p3};

            //
            // 結合する.
            //
            // 以下のクエリ式と同じ事となる。
            //
            //   from   team   in teams
            //   join   person in people on team equals person.Team
            //   select new { TeamName = team.Name, PersonName = person.Name }
            //
            var query =
                    teams.Join // TOuter
                    (
                        people, // TInner
                        team => team, // TOuterのキー
                        person => person.Team, // TInnerのキー
                        (team, person) => // 結果
                            new {TeamName = team.Name, PersonName = person.Name}
                    );

            foreach (var item in query)
            {
                Output.WriteLine("Team = {0}, Person = {1}", item.TeamName, item.PersonName);
            }
        }

        private class Person
        {
            public string? Name { get; set; }
            public Team? Team { get; set; }
        }

        private class Team
        {
            public string? Name { get; set; }
        }
    }
}