using System;
using System.Collections.Generic;
using System.Threading;
using TryCSharp.Common;

namespace TryCSharp.Samples.Threading
{
    /// <summary>
    ///     ThreadStatic属性に関するサンプルです。
    /// </summary>
    [Sample]
    public class ThreadStaticAttributeSamples01 : IExecutable
    {
        public void Execute()
        {
            var threads = new List<Thread>();
            for (var i = 0; i < 5; i++)
            {
                var thread = new Thread(ThreadState.DoThreadProcess);

                thread.Name = $"Thread-{i}";
                thread.IsBackground = true;

                threads.Add(thread);

                thread.Start();
            }

            threads.ForEach(thread => { thread.Join(); });
        }

        private class ThreadState
        {
            /// <summary>
            ///     各スレッド毎に固有の値を持つフィールド.
            /// </summary>
            [ThreadStatic] private static KeyValuePair<string, int> NameAndId;

            /// <summary>
            ///     各スレッドで共有されるフィールド.
            /// </summary>
            private static KeyValuePair<string, int> SharedNameAndId;

            public static void DoThreadProcess()
            {
                var thread = Thread.CurrentThread;

                //
                // ThreadStatic属性が付加されたフィールドと共有されたフィールドの両方に値を設定.
                //
                NameAndId = new KeyValuePair<string, int>(thread.Name, thread.ManagedThreadId);
                SharedNameAndId = new KeyValuePair<string, int>(thread.Name, thread.ManagedThreadId);

                Output.WriteLine("[BEFORE] ThreadStatic={0} Shared={1}", NameAndId, SharedNameAndId);

                //
                // 他のスレッドが動作できるようにする.
                //
                Thread.Sleep(TimeSpan.FromMilliseconds(200));

                Output.WriteLine("[AFTER ] ThreadStatic={0} Shared={1}", NameAndId, SharedNameAndId);
            }
        }
    }
}