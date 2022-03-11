using System.Threading;
using TryCSharp.Common;

namespace TryCSharp.Samples.Threading
{
    [Sample]
    public class ThreadingNamespaceSamples04 : IExecutable
    {
        public void Execute()
        {
            //
            // .NET 4.0より、Threadクラスに以下のメソッドが追加された。
            //   ・Yieldメソッド
            //
            // Yieldメソッドは、別のスレッドにタイムスライスを引き渡す為のメソッド。
            // 今までは、Thread.Sleepを利用したりして、タイムスライスを切り替えるよう
            // にしていたが、今後はこのメソッドを利用することが推奨される。
            //
            // 戻り値は、タイムスライスの引き渡しが成功したか否かが返ってくる。
            //

            //
            // テスト用にスレッドを２つ起動する.
            //
            var t1 = new Thread(ThreadProc);
            var t2 = new Thread(ThreadProc);

            t1.Start("T1");
            t2.Start("T2");

            t1.Join();
            t2.Join();
        }

        private void ThreadProc(object? stateObj)
        {
            var threadName = stateObj!.ToString();
            Output.WriteLine("{0} Start", threadName);

            //
            // タイムスライスを切り替え.
            //
            Output.WriteLine("{0} Yield Call", threadName);
            var isSuccess = Thread.Yield();
            Output.WriteLine("{0} Yield={1}", threadName, isSuccess);

            Output.WriteLine("{0} End", threadName);
        }
    }
}