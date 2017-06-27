using System;
using System.Linq;
using TryCSharp.Common;

namespace TryCSharp.Samples.Linq
{
    [Sample]
    public class PLinqSamples02 : IExecutable
    {
        public void Execute()
        {
            // 並列LINQにて、元の順序を保持して処理するにはAsOrderedを利用する。
            // AsOrderedを指定していない場合、どの順序で処理されていくのかは保証されない。
            var query1 = from x in Enumerable.Range(1, 20)
                    select Math.Pow(x, 2);

            foreach (var item in query1)
            {
                Output.WriteLine(item);
            }

            Output.WriteLine("===============");

            //
            // 以下のように、元のデータシーケンスをIEnumerable<T>と指定した上で並列LINQを行おうとしても
            // 並列化されない。何故なら、IEnumerable<T>では、LINQはシーケンス内にいくつ要素が存在するのかを
            // 判別することが出来ないためである。
            //
            // 並列LINQは、元のシーケンスをPartionerを利用して、一定のサイズのチャンクに分けて
            // 同時実行するための機能であるため、元の要素数が分からない場合はチャンクに分けることが出来ない。
            //
            // 並列処理を行う為には、ToListかToArrayなどを行い変換してから処理を進めるか
            // ParallelEnumerable.Rangeを利用したりするとうまくいく。
            //
            // 以下の例では並列処理が行われない。
            //var query2 = from x in Enumerable.Range(1, 20).AsParallel().WithExecutionMode(ParallelExecutionMode.ForceParallelism)
            // 以下の例ではLINQがリストの要素数を判別することが出来るので、並列処理が行われる。
            var query2 = from x in Enumerable.Range(1, 20).ToList().AsParallel().WithExecutionMode(ParallelExecutionMode.ForceParallelism)
                    select Math.Pow(x, 2);

            foreach (var item in query2)
            {
                Output.WriteLine(item);
            }

            Output.WriteLine("===============");

            var query3 = from x in ParallelEnumerable.Range(1, 20).AsParallel().AsOrdered().WithExecutionMode(ParallelExecutionMode.ForceParallelism)
                    select Math.Pow(x, 2);

            foreach (var item in query3)
            {
                Output.WriteLine(item);
            }

            Output.WriteLine("===============");
        }
    }
}