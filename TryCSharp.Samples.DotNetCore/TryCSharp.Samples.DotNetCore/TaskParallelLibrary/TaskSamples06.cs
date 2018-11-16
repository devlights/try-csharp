using System.Threading;
using System.Threading.Tasks;
using TryCSharp.Common;

namespace TryCSharp.Samples.TaskParallelLibrary
{
    /// <summary>
    ///     タスク並列ライブラリについてのサンプルです。
    /// </summary>
    /// <remarks>
    ///     タスク並列ライブラリは、.NET 4.0から追加されたライブラリです。
    /// </remarks>
    [Sample]
    public class TaskSamples06 : IExecutable
    {
        public void Execute()
        {
            //
            // タスクには作成時にTaskCreationOptionsを
            // 指定することができる。このオプションの中に
            //   TaskCreationOptions.LongRunning
            // という項目がある。LongRunningを指定すると
            // このタスクは、通常よりも長い間処理が続く
            // ということをタスクスケジューラに通知することに
            // なる。
            //
            // LongRunningと指定されたタスクは場合によって
            // スレッドプールスレッドを利用せずに実行される。
            // (長時間の処理で、かつ、ブロックする場合など)
            //
            // LongRunningを指定するとタスクスケジューラに対して
            // オーバーサブスクリプションを許可することになる。
            //
            // なので、一つや二つなどの場合はいいが
            // たくさんのタスクをLongRunningさせるべきではない。
            // (長時間かかる処理を producer/consumerパターンを
            //  利用して実装するなどの工夫が必要。)
            //

            //
            // 処理前のスレッドプールのスレッド数を表示.
            //
            Output.WriteLine("Before Task running...");
            PrintAvailableThreadPoolCount();

            //
            // 通常のタスクを開始し、その後スレッド数を表示.
            //
            var task1StartSignal = new ManualResetEventSlim(false);
            var task1 = Task.Run(() =>
                {
                    task1StartSignal.Set();
                    Output.WriteLine("Normal Task...");
                    SpinWait.SpinUntil(() => false, 5000);
                }
            );

            task1StartSignal.Wait();
            Output.WriteLine("After Task running...");
            PrintAvailableThreadPoolCount();

            //
            // TaskCreationOptions.LongRunningを
            // 指定して、タスクを開始.
            // (TaskCreationOptionsはTask.Runメソッドで指定できないので注意)
            //
            var task2StartSignal = new ManualResetEventSlim(false);
            var task2 = Task.Factory.StartNew(() =>
                {
                    task2StartSignal.Set();
                    Output.WriteLine("LongRunning Task....");
                    SpinWait.SpinUntil(() => false, 5000);
                },
                TaskCreationOptions.LongRunning
            );

            task2StartSignal.Wait();
            Output.WriteLine("After LongRunning Task running...");
            PrintAvailableThreadPoolCount();

            //
            // 終了待ち.
            //
            Task.WaitAll(task1, task2);
        }

        internal void PrintAvailableThreadPoolCount()
        {
            int availableWorkerThreadsCount;
            int availableIOThreadsCount;

            ThreadPool.GetAvailableThreads(
                out availableWorkerThreadsCount,
                out availableIOThreadsCount);

            Output.WriteLine(
                string.Format(
                    "\tWorker Threads: {0}, IO Threads: {1}",
                    availableWorkerThreadsCount,
                    availableIOThreadsCount));
        }
    }
}