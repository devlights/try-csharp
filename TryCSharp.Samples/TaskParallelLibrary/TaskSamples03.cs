using System;
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
    public class TaskSamples03 : IExecutable
    {
        public void Execute()
        {
            //
            // 入れ子タスクの作成
            //
            // タスクは入れ子にすることも可能。
            //
            // 入れ子のタスクには、以下の2種類が存在する。
            //   ・単純な入れ子タスク（デタッチされた入れ子タスク）
            //   ・子タスク（親のタスクにアタッチされた入れ子タスク）
            //
            // 以下のサンプルでは、単純な入れ子のタスクを作成し実行している。
            // 単純な入れ子のタスクとは、入れ子状態で作成されたタスクが
            // 親のタスクとの関連を持たない状態であることを示す。
            //
            // つまり、親のタスクは子のタスクの終了を待たずに、自身の処理を終了する。
            // 入れ子側のタスクにて、確実に親のタスクの終了前に自分の結果を得る必要がある場合は
            // WaitかResultを用いて、処理を完了させる必要がある。
            //
            // 親との関連を持たない入れ子のタスクは、「デタッチされた入れ子のタスク」と言う。
            //
            // デタッチされた入れ子タスクの作成は、単純に親タスクの中で新たにタスクを生成するだけである。
            //

            //
            // 単純な入れ子のタスクを作成.
            //
            Output.WriteLine("外側のタスク開始");
            var t = new Task(ParentTaskProc);
            t.Start();
            t.Wait();
            Output.WriteLine("外側のタスク終了");
        }

        private void ParentTaskProc()
        {
            PrintTaskId();

            //
            // 明示的に、TaskCreationOptionsを指定していないので
            // 以下の入れ子タスクは、「デタッチされた入れ子タスク」
            // として生成される。
            //
            var detachedTask = new Task(ChildTaskProc, TaskCreationOptions.None);
            detachedTask.Start();

            //
            // 以下のWaitをコメントアウトすると
            // 出力が
            //     外側のタスク開始
            //      Task Id: 1
            //     内側のタスク開始
            //      Task Id: 2
            //     外側のタスク終了
            //
            // と出力され、「内側のタスク終了」の出力がされないまま
            // メイン処理が終了したりする。
            //
            // これは、2つのタスクが親子関係を持っていないため
            // 別々で処理が行われているからである。
            //
            detachedTask.Wait();
        }

        private void ChildTaskProc()
        {
            Output.WriteLine("内側のタスク開始");
            PrintTaskId();
            Thread.Sleep(TimeSpan.FromSeconds(2.0));
            Output.WriteLine("内側のタスク終了");
        }

        private void PrintTaskId()
        {
            //
            // 現在実行中のタスクのIDを表示.
            //
            Output.WriteLine("\tTask Id: {0}", Task.CurrentId);
        }
    }
}