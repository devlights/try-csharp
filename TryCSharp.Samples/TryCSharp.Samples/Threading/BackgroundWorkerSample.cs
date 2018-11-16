using System.ComponentModel;
using System.Threading;
using TryCSharp.Common;

namespace TryCSharp.Samples.Threading
{
    /// <summary>
    ///     BackgroundWorkerを利用したスレッド処理のサンプルです。
    /// </summary>
    [Sample]
    public class BackgroundWorkerSample : IExecutable
    {
        /// <summary>
        ///     処理を実行します。
        /// </summary>
        public void Execute()
        {
            Output.WriteLine("[MAIN] START. ThreadId:{0}", Thread.CurrentThread.ManagedThreadId);

            var worker = new BackgroundWorker();

            //
            // 非同期処理のイベントをハンドル.
            //
            worker.DoWork += (s, e) =>
            {
                Output.WriteLine("[WORK] START. ThreadId:{0}", Thread.CurrentThread.ManagedThreadId);

                for (var i = 0; i < 10; i++)
                {
                    Output.WriteLine("[WORK] PROCESSING. ThreadId:{0}", Thread.CurrentThread.ManagedThreadId);
                    Thread.Sleep(105);
                }
            };

            //
            // 非同期処理が終了した際のイベントをハンドル.
            //
            worker.RunWorkerCompleted += (s, e) =>
            {
                if (e.Error != null)
                {
                    Output.WriteLine("[WORK] ERROR OCCURED. ThreadId:{0}", Thread.CurrentThread.ManagedThreadId);
                }

                Output.WriteLine("[WORK] END. ThreadId:{0}", Thread.CurrentThread.ManagedThreadId);
            };

            //
            // 非同期処理を開始.
            //
            worker.RunWorkerAsync();

            //
            // メインスレッドの処理.
            //
            for (var i = 0; i < 10; i++)
            {
                Output.WriteLine("[MAIN] PROCESSING. ThreadId:{0}", Thread.CurrentThread.ManagedThreadId);
                Thread.Sleep(100);
            }

            Thread.Sleep(1000);
            Output.WriteLine("[MAIN] END. ThreadId:{0}", Thread.CurrentThread.ManagedThreadId);
        }
    }
}