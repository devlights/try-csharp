using System;
using System.Diagnostics;
using System.Threading.Tasks;
using TryCSharp.Common;

namespace TryCSharp.Samples.TaskParallelLibrary
{
    /// <summary>
    /// Task.CompletedTask プロパティについてのサンプルです。
    /// </summary>
    /// <remarks>
    /// https://docs.microsoft.com/en-us/dotnet/api/system.threading.tasks.task.completedtask?view=netcore-3.1
    /// </remarks>
    [Sample]
    public class UsingCompletedTask : IAsyncExecutable
    {
        public async Task Execute()
        {
            // -------------------------------------------------------------------
            // .NET Framework 4.6 から Task.CompletedTask というプロパティが
            // 追加されている。このプロパティから取得できる Task はすでに完了状態になっている。
            //
            // 処理にて、チェックに引っかかった場合に 完了済み の Task を返すときなどに
            // とても便利。
            // -------------------------------------------------------------------
            var sw = new Stopwatch();
            
            sw.Start();
            await this.Fn1();
            sw.Stop();
            Output.WriteLine($"[Fn1] Elappsed: {sw.Elapsed}");
            
            sw.Reset();
            sw.Start();
            await this.Fn2();
            sw.Stop();
            Output.WriteLine($"[Fn2] Elappsed: {sw.Elapsed}");
        }

        Task Fn1()
        {
            return Task.CompletedTask;
        }

        Task Fn2()
        {
            return Task.Delay(TimeSpan.FromSeconds(1));
        }
    }
}