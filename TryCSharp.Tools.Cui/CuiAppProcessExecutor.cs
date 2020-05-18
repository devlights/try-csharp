using System;
using System.Diagnostics;
using System.Threading.Tasks;
using TryCSharp.Common;

namespace TryCSharp.Tools.Cui
{
    /// <summary>
    /// 実行を担当するクラスです。
    /// </summary>
    public class CuiAppProcessExecutor : IExecutor
    {
        /// <summary>
        /// 開始ログ
        /// </summary>
        private string StartLogMessage { get; } = "================== START ==================";

        /// <summary>
        /// 終了ログ
        /// </summary>
        private string EndLogMessage { get; } = "==================  END  ==================";

        /// <summary>
        /// 実行します。
        /// </summary>
        /// <param name="target">実行可能なもの</param>
        public void Execute(IExecutable target)
        {
            if (target == null)
            {
                throw new ArgumentNullException(nameof(target));
            }

            using (new TimeTracer())
            {
                Output.WriteLine($"[EXAMPLE] {target.GetType().Name}");
                Output.WriteLine(this.StartLogMessage);
                target.Execute();
                Output.WriteLine(this.EndLogMessage);
            }
        }

        /// <summary>
        /// 非同期実行します。
        /// </summary>
        /// <param name="target">実行可能なもの</param>
        public async Task Execute(IAsyncExecutable target)
        {
            if (target == null)
            {
                throw new ArgumentNullException(nameof(target));
            }

            using (new TimeTracer())
            {
                Output.WriteLine($"[EXAMPLE] {target.GetType().Name}");
                Output.WriteLine(this.StartLogMessage);

                var t = Task.Run(async () =>
                {
                    Output.WriteLine("[Async] **** BEGIN ****");
                    await target.Execute();
                    Output.WriteLine("[Async] ****  END  ****");
                });
                
                var timeout = Task.Delay(TimeSpan.FromSeconds(15));
                if (await Task.WhenAny(t, timeout) == timeout)
                {
                    Output.WriteLine("**** TIMEOUT ****");
                }

                Output.WriteLine(this.EndLogMessage);
            }
        }
    }

    internal class TimeTracer : IDisposable
    {
        private readonly Stopwatch _watch;

        public TimeTracer()
        {
            this._watch = Stopwatch.StartNew();
        }

        public void Dispose()
        {
            this._watch.Stop();
            Output.WriteLine("Elapsed Time: {0}", this._watch.Elapsed);
        }
    }
}