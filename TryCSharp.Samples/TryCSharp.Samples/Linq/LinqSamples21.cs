using System.Collections.Generic;
using System.Linq;
using TryCSharp.Common;

namespace TryCSharp.Samples.Linq
{
    /// <summary>
    ///     Linqのサンプルです。
    /// </summary>
    [Sample]
    public class LinqSamples21 : IExecutable
    {
        public void Execute()
        {
            var teams = new List<Team>
            {
                new Team {Name = "Team A", Members = new List<string> {"gsf_zero1", "gsf_zero2"}},
                new Team {Name = "Team B", Members = new List<string> {"gsf_zero3", "gsf_zero4"}}
            };

            //
            // SelectMany拡張メソッドは、コレクションを平坦化する際に利用できる。
            // 例えば、上記で作成したteams変数は以下のような構造になっている。
            //
            //   teams -- [0]:Teamオブジェクト
            //            └Members:IEnumerable<string>
            //        [1]:Teamオブジェクト
            //            └Members:IEnumerable<string>
            //
            // 各Teamオブジェクトは、Membersプロパティを持っているので
            // SelectManyではなく、Select拡張メソッドを利用して
            //  teams.Select(team => team.Members)
            // とすると、結果は、IEnumerable<IEnumerable<string>>となる。
            //
            // このような状態で、SelectMany拡張メソッドを利用して
            //  teams.SelectMany(team => team.Members)
            // とすると、結果は、IEnumerable<string>となる。
            // つまり、SelectMany拡張メソッドは、各Selectorが返すシーケンスを最終的に
            // 平坦化してから結果を返してくれる。
            //
            // 尚、SelectMany拡張メソッドは、クエリ式にて2段以上のfrom句を利用している場合
            // 暗黙的に利用されている。上記のteams.SelectMany(team => team.Members)は
            // 以下のクエリ式と同じである。
            //
            //   from   team   in teams
            //   from   member in team.Members
            //   select member
            //
            // 実行時には、最後のselectの部分がSelectManyに置換される。
            //
            Output.WriteLine("===== Func<TSource, IEnumerable<TResult>>のサンプル =====");
            foreach (var member in teams.SelectMany(team => team.Members))
            {
                Output.WriteLine(member);
            }

            Output.WriteLine("===== Func<TSource, int, IEnumerable<TResult>>のサンプル =====");
            foreach (var member in teams.SelectMany((team, index) => index%2 == 0 ? team.Members : new List<string>()))
            {
                Output.WriteLine(member);
            }

            Output.WriteLine("===== collectionSelectorとresultSelectorを利用しているサンプル (1) =====");
            var query = teams.SelectMany
            (
                team => team.Members, // collectionSelector
                (team, member) => new {Team = team.Name, Name = member} // resultSelector
            );

            foreach (var item in query)
            {
                Output.WriteLine(item);
            }

            Output.WriteLine("===== collectionSelectorとresultSelectorを利用しているサンプル (2) =====");
            var query2 = teams.SelectMany
            (
                (team, index) => index%2 != 0 ? team.Members : new List<string>(), // collectionSelector
                (team, member) => new {Team = team.Name, Name = member} // resultSelector
            );

            foreach (var item in query2)
            {
                Output.WriteLine(item);
            }
        }

        private class Team
        {
            public string Name { get; set; }
            public IEnumerable<string> Members { get; set; }
        }
    }
}