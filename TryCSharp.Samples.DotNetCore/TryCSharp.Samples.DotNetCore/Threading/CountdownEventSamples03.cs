using System;
using System.Threading;
using System.Threading.Tasks;
using TryCSharp.Common;

namespace TryCSharp.Samples.Threading
{
    /// <summary>
    ///     CountdownEventクラスについてのサンプルです。(3)
    /// </summary>
    /// <remarks>
    ///     CountdownEventクラスは、.NET 4.0から追加されたクラスです。
    ///     JavaのCountDownLatchクラスと同じ機能を持っています。
    /// </remarks>
    [Sample]
    public class CountdownEventSamples03 : IExecutable
    {
        public void Execute()
        {
            //
            // CountdownEventには、CancellationTokenを受け付けるWaitメソッドが存在する.
            // 使い方は、ManualResetEventSlimクラスの場合と同じ。
            //
            // 参考リソース:
            //   .NET クラスライブラリ探訪-042 (System.Threading.ManualResetEventSlim)
            //   http://d.hatena.ne.jp/gsf_zero1/20110323/p1
            //
            var tokenSource = new CancellationTokenSource();
            var token = tokenSource.Token;

            using (var cde = new CountdownEvent(1))
            {
                // 初期の状態を表示.
                Output.WriteLine("InitialCount={0}", cde.InitialCount);
                Output.WriteLine("CurrentCount={0}", cde.CurrentCount);
                Output.WriteLine("IsSet={0}", cde.IsSet);

                var t = Task.Factory.StartNew(() =>
                {
                    Thread.Sleep(TimeSpan.FromSeconds(2));

                    token.ThrowIfCancellationRequested();
                    cde.Signal();
                }, token);

                //
                // 処理をキャンセル.
                //
                tokenSource.Cancel();

                try
                {
                    cde.Wait(token);
                }
                catch (OperationCanceledException cancelEx)
                {
                    if (token == cancelEx.CancellationToken)
                    {
                        Output.WriteLine("＊＊＊CountdownEvent.Wait()がキャンセルされました＊＊＊");
                    }
                }

                try
                {
                    t.Wait(token);
                }
                catch (AggregateException aggEx)
                {
                    aggEx.Handle(ex =>
                    {
                        var exception = ex as OperationCanceledException;
                        if (exception != null)
                        {
                            var cancelEx = exception;

                            if (token == cancelEx.CancellationToken)
                            {
                                Output.WriteLine("＊＊＊タスクがキャンセルされました＊＊＊");
                                return true;
                            }
                        }

                        return false;
                    });
                }

                // 現在の状態を表示.
                Output.WriteLine("InitialCount={0}", cde.InitialCount);
                Output.WriteLine("CurrentCount={0}", cde.CurrentCount);
                Output.WriteLine("IsSet={0}", cde.IsSet);
            }
        }
    }
}