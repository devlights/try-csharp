using System;
using System.Threading;
using TryCSharp.Common;
// ReSharper disable PossibleNullReferenceException

namespace TryCSharp.Samples.Threading
{
    /// <summary>
    ///     スレッドプール(ThreadPool)を利用したスレッド処理のサンプルです。
    /// </summary>
    [Sample]
    public class ThreadPoolSample : IExecutable
    {
        /// <summary>
        ///     処理を実行します。
        /// </summary>
        public void Execute()
        {
            for (var i = 0; i < 15; i++)
            {
                ThreadPool.QueueUserWorkItem(stateInfo =>
                    {
                        if (stateInfo == null)
                        {
                            return;
                        }

                        var p = (StateInfo)stateInfo;
                        Thread.Sleep(150);
                        Output.WriteLine("Thread Count:{0}, Time:{1}", p.Count, p.Time.ToString("hh:mm:ss.fff"));
                    },
                    new StateInfo
                    {
                        Count = i,
                        Time = DateTime.Now
                    });
            }

            Thread.Sleep(2000);
        }

        /// <summary>
        ///     スレッドの状態を表すデータクラスです。
        /// </summary>
        private class StateInfo
        {
            public int Count { get; set; }

            public DateTime Time { get; set; }
        }
    }
}