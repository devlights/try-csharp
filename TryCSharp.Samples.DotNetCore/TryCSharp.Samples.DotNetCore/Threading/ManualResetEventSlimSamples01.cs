using System;
using System.Threading;
using System.Threading.Tasks;
using TryCSharp.Common;

namespace TryCSharp.Samples.Threading
{
    /// <summary>
    ///     ManualResetEventSlimクラスについてのサンプルです。
    /// </summary>
    /// <remarks>
    ///     ManualResetEventSlimクラスは、.NET 4.0で追加されたクラスです。
    ///     元々存在していたManualResetEventクラスよりも軽量なクラスとなっています。
    ///     特徴しては、以下の点が挙げられます。
    ///     ・WaitメソッドにCancellationTokenを受け付けるオーバーロードが存在する。
    ///     ・非常に短い時間の待機の場合、このクラスは待機ハンドルではなくビジースピンを利用して待機する。
    /// </remarks>
    [Sample]
    public class ManualResetEventSlimSamples01 : IExecutable
    {
        public void Execute()
        {
            //
            // 通常の使い方.
            //
            var mres = new ManualResetEventSlim(false);

            ThreadPool.QueueUserWorkItem(DoProc, mres);

            Output.Write("メインスレッド待機中・・・");
            mres.Wait();
            Output.WriteLine("終了");

            //
            // WaitメソッドにCancellationTokenを受け付けるオーバーロードを使用。
            //
            mres.Reset();

            var tokenSource = new CancellationTokenSource();
            var token = tokenSource.Token;

            Task.Factory.StartNew(DoProc, mres);

            //
            // キャンセル状態に設定.
            //
            tokenSource.Cancel();

            Output.Write("メインスレッド待機中・・・");

            try
            {
                //
                // CancellationTokenを指定して、Wait呼び出し。
                // この場合は、以下のどちらかの条件を満たした時点でWaitが解除される。
                //  ・別の場所にて、Setが呼ばれてシグナル状態となる。
                //  ・CancellationTokenがキャンセルされる。
                //
                // トークンがキャンセルされた場合、OperationCanceledExceptionが発生するので
                // CancellationTokenを指定するWaitを呼び出す場合は、try-catchが必須となる。
                //
                // 今回の例の場合は、予めCancellationTokenをキャンセルしているので
                // タスク処理でシグナル状態に設定されるよりも先に、キャンセル状態に設定される。
                // なので、実行結果には、「*** シグナル状態に設定 ***」という文言は出力されない。
                //
                mres.Wait(token);
            }
            catch (OperationCanceledException cancelEx)
            {
                Output.Write("*** {0} *** ", cancelEx.Message);
            }

            Output.WriteLine("終了");
        }

        private void DoProc(object stateObj)
        {
            Thread.Sleep(TimeSpan.FromSeconds(1));
            Output.Write("*** シグナル状態に設定 *** ");
            (stateObj as ManualResetEventSlim)?.Set();
        }
    }
}