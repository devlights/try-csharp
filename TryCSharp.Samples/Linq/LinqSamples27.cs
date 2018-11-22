using System.Collections.Generic;
using System.Linq;
using TryCSharp.Common;

namespace TryCSharp.Samples.Linq
{
    /// <summary>
    ///     Linqのサンプルです。
    /// </summary>
    [Sample]
    public class LinqSamples27 : IExecutable
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
            // グループ結合する.
            // 
            // Join拡張メソッドと書式的にはほとんど同じであるが、以下の点が異なる。
            //  ・resultSelectorの書式が、(TOuter, IEnumerable<TInner>)となっている。
            // これにより、結果をJOINした結果をグルーピングした状態で保持することが出来る。（階層構造を構築出来る。）
            //
            // 以下のクエリ式と同じ事となる。
            //  from team   in teams
            //  join person in people on team equals person.Team into personCollection
            //  select new { Team = team, Persons = personCollection }
            //
            var query = teams.GroupJoin // TOuter
            (
                people, // TInner
                team => team, // TOuterのキー
                person => person.Team, // TInnerのキー
                (team, personCollection) => // 結果 (TOuter, IEnumerable<TInner>)
                    new {Team = team, Persons = personCollection}
            );

            foreach (var item in query)
            {
                Output.WriteLine("TEAM = {0}", item.Team.Name);
                foreach (var p in item.Persons)
                {
                    Output.WriteLine("\tPERSON = {0}", p.Name);
                }
            }
        }

        private class Person
        {
            public string Name { get; set; }
            public Team Team { get; set; }
        }

        private class Team
        {
            public string Name { get; set; }
        }
    }
}