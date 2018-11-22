using System;
using System.Diagnostics;
using System.Linq;
using TryCSharp.Common;

namespace TryCSharp.Samples.Linq
{
    [Sample]
    public class PLinqSamples01 : IExecutable
    {
        public void Execute()
        {
            var numbers = GetRandomNumbers();

            var watch = Stopwatch.StartNew();

            // 普通のLINQ
            // var query1 = from x in numbers
            // 並列LINQ（１）（ExecutionModeを付与していないので、並列で実行するか否かはTPLが決定する）
            // var query1 = from x in numbers.AsParallel()
            // 並列LINQ（２）（ExecutionModeを付与しているので、強制的に並列で実行するよう指示）
            var query1 = from x in numbers.AsParallel().WithExecutionMode(ParallelExecutionMode.ForceParallelism)
                    select Math.Pow(x, 2);

            foreach (var item in query1)
            {
                Output.WriteLine(item);
            }

            watch.Stop();
            Output.WriteLine(watch.Elapsed);
        }

        private byte[] GetRandomNumbers()
        {
            var result = new byte[10];
            var rnd = new Random();

            rnd.NextBytes(result);

            return result;
        }
    }
}