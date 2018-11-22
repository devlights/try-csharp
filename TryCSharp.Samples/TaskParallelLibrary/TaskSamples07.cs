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
    public class TaskSamples07 : IExecutable
    {
        public void Execute()
        {
            //
            // タスクの戻り値は
            //   Resultプロパティ
            // から取得できる。
            //
            // 非同期処理内で例外が発生していた場合
            // Resultプロパティにアクセスした段階で
            // 例外 (AggregateException) が発生するので注意。
            //
            // タスクにて、内部で例外が発生していた場合に
            // 以下のメソッドにアクセスすると保留されていた
            // 例外が発生する.
            //
            //   ・Resultプロパティ
            //   ・Waitメソッド
            //

            //
            // 以下、いろいろな方法でタスクを作成して結果を取得.
            //
            var task1 = Task.Run(() => GetStringResult());
            Output.WriteLine(task1.Result);

            var task2 = Task.Factory.StartNew(() => GetIntResult());
            Output.WriteLine(task2.Result);

            var task3 = new Task<string>(() => GetStringResult());
            task3.Start();
            Output.WriteLine(task3.Result);

            var tokenSource = new CancellationTokenSource();
            var task4 = Task.Run(() => GetDelayResult(tokenSource.Token), tokenSource.Token);
            try
            {
                //
                // 指定時間後にキャンセル.
                //   CancellationTokenSource.Cancelで発生する例外はOperationCanceledExceptionだが
                //   タスクに対して、予め同じキャンセルトークンを渡している場合
                //   キャンセル例外が発生したことをタスクが認識してTaskCanceledExceptionを発生させる.
                //
                tokenSource.CancelAfter(TimeSpan.FromMilliseconds(500));
                Output.WriteLine(task4.Result);
            }
            catch (AggregateException aggreEx)
            {
                aggreEx.Handle(ex =>
                {
                    if (ex is TaskCanceledException)
                    {
                        Output.WriteLine("[CANCEL] {0}", ex);
                        return true;
                    }

                    return false;
                });
            }
        }

        internal int GetIntResult()
        {
            return 100;
        }

        internal string GetStringResult()
        {
            return "Hello world";
        }

        internal async Task<string> GetDelayResult(CancellationToken token)
        {
            await Task.Delay(TimeSpan.FromMilliseconds(5000), token);
            token.ThrowIfCancellationRequested();
            return GetStringResult();
        }
    }
}