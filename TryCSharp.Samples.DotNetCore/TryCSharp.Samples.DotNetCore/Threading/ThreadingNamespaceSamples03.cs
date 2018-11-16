using System.Threading;
using TryCSharp.Common;

namespace TryCSharp.Samples.Threading
{
    [Sample]
    public class ThreadingNamespaceSamples03 : IExecutable
    {
        public void Execute()
        {
            var thread = new Thread(ThreadProcess);

            thread.IsBackground = true;
            thread.Start();

            thread.Join();
        }

        private void ThreadProcess()
        {
            ///////////////////////////////////////////////////
            //
            // スレッドクラスが持つその他のプロパティについて
            //
            var currentThread = Thread.CurrentThread;

            //
            // IsAliveプロパティ
            //    ⇒スレッドが起動されており、まだ終了または中止されていない場合は
            //    　trueとなる。
            //
            Output.WriteLine("IsAlive={0}", currentThread.IsAlive);

            //
            // IsThreadPoolThread, ManagedThreadIdプロパティ
            //    ⇒それぞれ、スレッドプールスレッドかどうかとマネージスレッドを識別
            //    　する値が取得できる。
            //
            Output.WriteLine("IsThreadPoolThread={0}", currentThread.IsThreadPoolThread);
            Output.WriteLine("ManagedThreadId={0}", currentThread.ManagedThreadId);

            //
            // Priorityプロパティ
            //    ⇒対象のスレッドの優先度（プライオリティ）を取得及び設定します。
            //    　Highestが最も高く、Lowestが最も低い.
            //
            Output.WriteLine("Priority={0}", currentThread.Priority);
        }
    }
}