using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TryCSharp.Common;

namespace TryCSharp.Samples.TaskParallelLibrary
{
    /// <summary>
    ///     タスク並列ライブラリについてのサンプルです。
    /// </summary>
    /// <remarks>
    ///     タスク並列ライブラリは、4.0から追加されているライブラリです。
    /// </remarks>
    [Sample]
    public class TaskSamples01 : IExecutable
    {
        public void Execute()
        {
            //
            // Taskは、タスク並列ライブラリの一部として提供されており
            // 文字通りタスクを並列処理するために利用できる。
            //
            // .NET 4.0まで、非同期処理を行う場合ThreadクラスやThreadPoolクラスが
            // 用意されていたが、利用するのに若干の専門性が必要となるものであった。
            //
            // タスク並列ライブラリは、出来るだけ容易に利用できるようデザインされた
            // 新しいライブラリである。
            //
            // さらにタスク並列ライブラリでは、同時実行の程度を内部で調整してくれることによって
            // CPUを効率的に利用するようになっている。
            //
            // ただし、それでもスレッド処理に関する基礎知識は当然必要となる。
            // (ロック、デッドロック、競合状態など）
            //
            // Taskクラスは、System.Threading.Tasks名前空間に存在する。
            //
            // タスクを利用するのに一番簡単な方法はTaskFactoryのStartNewメソッドを
            // 利用する事である。
            //
            // タスクは内部でスレッドプールを利用しているため、スレッドオブジェクトを
            // 直接作成して開始するよりも軽い負荷で実行できる。
            //
            // タスクにはキャンセル機能がデフォルトで用意されている。(CancellationToken)
            // タスクのキャンセル機能については、別の機会で記述する。
            //
            // タスクには状態管理機能がデフォルトで用意されている。
            // タスクの状態管理機能については、別の機会で記述する。
            //

            // 別スレッドでタスクが実行されている事を確認する為に、メインスレッドのスレッドIDを表示
            Output.WriteLine("Main Thread : {0}", Thread.CurrentThread.ManagedThreadId);

            //
            // Taskを新規作成して実行.
            //   引数にはActionデリゲートを指定する。
            //
            // Waitメソッドはタスクの終了を待つメソッド。
            //
            Task.Factory.StartNew(DoAction).Wait();


            //
            // Actionの部分にラムダを指定した版
            //
            Task.Factory.StartNew(() => Output.WriteLine("Lambda : {0}", Thread.CurrentThread.ManagedThreadId)).Wait();

            //
            // 多数のタスクを作成して実行.
            //   Task.WaitAllメソッドは引数で指定されたタスクが全て終了するまで待機するメソッド
            //
            Task.WaitAll(
                Enumerable.Range(1, 20).Select(i => Task.Factory.StartNew(DoActionWithSleep)).ToArray()
            );
        }

        private void DoAction()
        {
            Output.WriteLine("DoAction: {0}", Thread.CurrentThread.ManagedThreadId);
        }

        private void DoActionWithSleep()
        {
            DoAction();
            Thread.Sleep(200);
        }
    }
}