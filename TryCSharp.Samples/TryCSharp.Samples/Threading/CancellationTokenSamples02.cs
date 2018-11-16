using System;
using System.Threading;
using System.Threading.Tasks;
using TryCSharp.Common;

namespace TryCSharp.Samples.Threading
{
    /// <summary>
    ///     CancellationTokenとCancellationTokenSourceについてのサンプルです。
    /// </summary>
    /// <remarks>
    ///     .NET 4.5から追加されたCancelAfterメソッドのサンプルです。
    /// </remarks>
    [Sample]
    internal class CancellationTokenSamples02 : IExecutable
    {
        public void Execute()
        {
            //
            // .NET 4.5からCancellationTokenSourceにCancelAfterメソッド
            // が追加された。CancelAfterメソッドは、指定時間後にキャンセル
            // 処理を行うメソッドとなっている。
            //
            // CancellationTokenSource.CancelAfter メソッド
            //   http://msdn.microsoft.com/ja-jp/library/hh194678(v=vs.110).aspx
            //
            // 引数には、Int32またはTimeSpanが指定できる。
            //
            var tokenSource = new CancellationTokenSource();
            var token = tokenSource.Token;

            Action action = () =>
            {
                while (true)
                {
                    if (token.IsCancellationRequested)
                    {
                        Output.WriteLine("Canceled!");
                        break;
                    }

                    Output.Write(".");
                    Thread.Sleep(TimeSpan.FromSeconds(1));
                }
            };

            var task = Task.Run(action, token);

            //
            // 3秒後にキャンセル
            //
            tokenSource.CancelAfter(TimeSpan.FromSeconds(3));
            task.Wait(token);

            tokenSource.Dispose();
        }
    }
}