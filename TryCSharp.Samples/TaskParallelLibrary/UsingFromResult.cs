using System;
using System.Diagnostics;
using System.Threading.Tasks;
using TryCSharp.Common;

namespace TryCSharp.Samples.TaskParallelLibrary
{
    [Sample]
    public class UsingFromResult : IAsyncExecutable
    {
        public async Task Execute()
        {
            // -------------------------------------------------------------------
            // .NET Framework 4.5 から
            //      Task<T> Task.FromResult<T>(T result)
            // というメソッドが追加されている。
            // このプロパティから取得できる Task はすでに完了状態になっている。
            //
            // 処理にて、チェックに引っかかった場合に 完了済み の Task を返すときなどに
            // とても便利。
            // -------------------------------------------------------------------
            var sw = new Stopwatch();
            
            sw.Start();
            var r1 = await this.Fn1();
            sw.Stop();
            Output.WriteLine($"[Fn1] result:{r1:D3}\tElappsed: {sw.Elapsed}");
            
            sw.Reset();
            sw.Start();
            var r2 = await this.Fn2();
            sw.Stop();
            Output.WriteLine($"[Fn2] result:{r2:D3}\tElappsed: {sw.Elapsed}");
        }

        Task<int> Fn1()
        {
            return Task.FromResult(0);
        }

        async Task<int> Fn2()
        {
            await Task.Delay(TimeSpan.FromSeconds(1));
            return 100;
        }
    }
}