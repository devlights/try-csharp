using System;
using System.Threading;
using TryCSharp.Common;
// ReSharper disable PossibleNullReferenceException

namespace TryCSharp.Samples.Threading
{
    /// <summary>
    ///     スレッドを直接作成するサンプル.
    /// </summary>
    [Sample]
    public class ThreadSample : IExecutable
    {
        /// <summary>
        ///     件数
        /// </summary>
        private int _count;

        /// <summary>
        ///     ロックオブジェクト
        /// </summary>
        private readonly object _lockObject = new object();

        /// <summary>
        ///     処理を実行します。
        /// </summary>
        public void Execute()
        {
            //
            // ThreadStartデリゲートを用いた場合.
            //
            ThreadStart ts = () =>
            {
                lock (_lockObject)
                {
                    if (_count < 10)
                    {
                        _count++;
                    }
                }

                Output.WriteLine("Count={0}", _count);
            };

            for (var i = 0; i < 15; i++)
            {
                var t = new Thread(ts);
                t.IsBackground = false;

                t.Start();

                //
                // 確実にスレッドの走る順序を揃えるには以下のようにする。
                // (もっともこれをやるとスレッドの意味がないが・・)
                //
                //t.Join();
            }

            //
            // ParameterizedThreadStartを用いた場合.
            //
            ParameterizedThreadStart pts = data =>
            {
                var p = data as ThreadParameter;
                Thread.Sleep(150);
                Output.WriteLine("Thread Count:{0}, Time:{1}", p.Count, p.Time.ToString("hh:mm:ss.fff"));
            };

            for (var i = 0; i < 15; i++)
            {
                var t = new Thread(pts);
                t.IsBackground = false;

                t.Start(new ThreadParameter
                {
                    Count = i,
                    Time = DateTime.Now
                });

                //
                // 確実にスレッドの走る順序を揃えるには以下のようにする。
                // (もっともこれをやるとスレッドの意味がないが・・)
                //
                //t.Join();
            }
        }

        /// <summary>
        ///     スレッドを実行する際の引数として利用されるクラスです。
        /// </summary>
        private class ThreadParameter
        {
            public int Count { get; set; }

            public DateTime Time { get; set; }
        }
    }
}