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
    public class TaskSamples04 : IExecutable
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
            // 以下のサンプルでは、子タスクを作成して実行している。
            // 子タスクとは、単純な入れ子タスクと違い、親タスクと親子関係を
            // 持った状態でタスク処理が行われる。
            //
            // つまり、親のタスクは子のタスクの終了を待ってから、自身の処理を終了する。
            //
            // 親との関連を持つ入れ子のタスクは、「アタッチされた入れ子のタスク」と言う。
            //
            // アタッチされた入れ子タスクの作成は、タスクを生成する際に以下のTaskCreationOptionsを
            // 指定する。
            //   TaskCreationOptions.AttachedToParent
            //

            //
            // 親子関係を持つ子タスクを作成.
            //
            Output.WriteLine("親のタスク開始");
            var t = new Task(ParentTaskProc);
            t.Start();
            t.Wait();
            Output.WriteLine("親のタスク終了");
        }

        private void ParentTaskProc()
        {
            PrintTaskId();

            //
            // 明示的にTaskCreationOptionsを指定して
            // アタッチされた入れ子タスクを指定する。
            //
            var childTask = new Task(ChildTaskProc, TaskCreationOptions.AttachedToParent);
            childTask.Start();

            //
            // 「デタッチされた入れ子タスク」と違い、親タスクにアタッチされた入れ子タスクは
            // 明示的にWaitをしなくても、親のタスクが子のタスクの終了を待ってくれる。
            //
        }

        private void ChildTaskProc()
        {
            Output.WriteLine("子のタスク開始");
            PrintTaskId();
            Thread.Sleep(TimeSpan.FromSeconds(2.0));
            Output.WriteLine("子のタスク終了");
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