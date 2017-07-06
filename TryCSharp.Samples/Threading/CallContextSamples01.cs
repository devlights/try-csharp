using System.Runtime.Remoting.Messaging;
using System.Threading;
using TryCSharp.Common;

namespace TryCSharp.Samples.Threading
{
    /// <summary>
    ///     実行コンテキスト(ExecutionContext)と論理呼び出しコンテキスト(CallContext)のサンプルです。
    /// </summary>
    [Sample]
    public class CallContextSamples01 : IExecutable
    {
        public void Execute()
        {
            //
            // すべてのスレッドには、実行コンテキスト (Execution Context) が関連付けられている。
            // 実行コンテキストには
            //    ・そのスレッドのセキュリティ設定 (圧縮スタック、Thread.Principal, Windowsの認証情報)
            //    ・ホスト設定 (HostExecutionContextManager)
            //    ・論理呼び出しコンテキストデータ (CallContext)
            // が紐付いている。
            //
            // その中でも、論理呼び出しコンテキスト (CallContext) は、LogicalSetDataメソッド、LogicalGetDataメソッドを
            // 利用することにより、同じ実行コンテキストを持つスレッド間でデータを共有することができる。
            // 既定では、CLRは起動元のスレッドの実行コンテキストが自動的に伝播されるようにしてくれる。
            //
            // 実行コンテキストの伝播方法は、ExecutionContextクラスを利用することにより変更することができる。
            // ExecutionContext.SuppressFlowメソッドにはSerucityCriticalAttribute
            // が付与されているので、環境によっては動作しなくなる可能性がある。
            // (SerucityCriticalAttributeは、完全信頼を要求する属性)
            //
            var numberOfThreads = 5;

            using (var cde = new CountdownEvent(numberOfThreads))
            {
                //
                // メインスレッド上にて、論理呼び出しコンテキストデータを設定.
                //
                CallContext.LogicalSetData("Message", "Hello World");

                //
                // 既定の設定のまま (親元のExecutionContextをそのまま継承） で、別スレッド生成.
                //
                ThreadPool.QueueUserWorkItem(ShowCallContextLogicalData, new ThreadData("First Thread", cde));

                //
                // 実行コンテキストの伝播方法を変更.
                //   SuppressFlowメソッドは、実行コンテキストフローを抑制するメソッド.
                // SuppressFlowメソッドは、AsyncFlowControlを戻り値として返却する。
                // 抑制した実行コンテキストを復元するには、AsyncFlowControl.Undoを呼び出す。
                //
                var flowControl = ExecutionContext.SuppressFlow();

                //
                // 抑制された実行コンテストの状態で、別スレッド生成.
                //
                ThreadPool.QueueUserWorkItem(ShowCallContextLogicalData, new ThreadData("Second Thread", cde));

                //
                // 実行コンテキストを復元.
                //
                flowControl.Undo();

                //
                // 再度、別スレッド生成.
                //
                ThreadPool.QueueUserWorkItem(ShowCallContextLogicalData, new ThreadData("Third Thread", cde));

                //
                // 再度、実行コンテキストを抑制し、抑制されている間に論理呼び出しコンテキストデータを変更し
                // その後、実行コンテキストを復元する.
                //
                flowControl = ExecutionContext.SuppressFlow();
                CallContext.LogicalSetData("Message", "Modified....");

                ThreadPool.QueueUserWorkItem(ShowCallContextLogicalData, new ThreadData("Fourth Thread", cde));
                flowControl.Undo();
                ThreadPool.QueueUserWorkItem(ShowCallContextLogicalData, new ThreadData("Fifth Thread", cde));

                cde.Wait();
            }
        }

        private void ShowCallContextLogicalData(object state)
        {
            var data = state as ThreadData;

            Output.WriteLine(
                "Thread: {0, -15}, Id: {1}, Message: {2}"
                , data?.Name
                , Thread.CurrentThread.ManagedThreadId
                , CallContext.LogicalGetData("Message")
            );

            data?.Counter.Signal();
        }

        #region Inner Classes

        private class ThreadData
        {
            public ThreadData(string name, CountdownEvent cde)
            {
                Name = name;
                Counter = cde;
            }

            public string Name { get; }
            public CountdownEvent Counter { get; }
        }

        #endregion
    }
}