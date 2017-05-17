using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using TryCSharp.Common;

namespace TryCSharp.Samples.Threading
{
    /// <summary>
    ///     CancellationTokenとCancellationTokenSourceについてのサンプルです。
    /// </summary>
    [Sample]
    public class CancellationTokenSamples01 : IExecutable
    {
        public void Execute()
        {
            //
            // CancellationTokenとCancellationTokenSourceは
            // .NET Framework 4.0から追加された型である。
            //
            // 非同期操作または長時間の同期処理などの際、汎用的なキャンセル処理を実装するために利用できる。
            // よくタスク (System.Threading.Tasks.Task)と一緒に利用されている
            // 例が多いが、別にタスクでなくても利用できる。（通常のThreadやManualResetEventSlimなど)
            //
            // CancellationTokenSourceとCancellationTokenは親子のような関係にあり
            //   ・CancellationTokenSourceはキャンセル操作を持つ。
            //   ・CancellationTokenは、キャンセルされた事を検知する。
            // となっている。
            //
            // CancellationTokenにて、キャンセルされたか否かを検知するには以下のプロパティまたはメソッドを利用する.
            //   ・IsCancellationRequested
            //   ・ThrowIfCancellationRequested
            // 上記の内、ThrowIfCancellationRequestedメソッドはキャンセルされていた場合に
            // OperationCanceledExceptionを発生させる。
            //
            // そのほかにも、CancellationTokenには以下のプロパティとメソッドが存在する。
            //   ・WaitHandle
            //   ・Register
            // WaitHandleプロパティは、該当トークンがキャンセルされた際に通知される待機ハンドルである。
            // この待機ハンドルを利用することで、トークンがキャンセルされた後に実行される処理などを記述出来る。
            // Registerメソッドは、トークンがキャンセルされた際に関連してキャンセル処理などを行いたいオブジェクトが存在する
            // 場合などに利用できる。CancellationTokenは操作のキャンセルを表すものであり、オブジェクトの状態をキャンセルしたい
            // 場合にこのメソッドを利用して登録しておく.
            //
            // また、CancellationTokenSourceには、以下のstaticメソッドが存在する。
            //   ・CreateLinkedTokenSource
            // CreateLinkedTokenSourceメソッドは、引数に複数のトークンを受け取り
            // それらのトークンを紐づけた状態のトークンソースを作成してくれる。
            // これを利用することにより、複数のトークン全てがキャンセルされた際にキャンセル扱いになる
            // CancellationTokenを生成する事が出来る。
            //
            // 関連する全てのトークンがキャンセル状態となった際に行うキャンセル処理を記述する場合などに利用できる。
            //
            var cts = new CancellationTokenSource();

            ////////////////////////////////////////////////////////////////////
            //
            // Threadを利用してのキャンセル処理.
            //
            var t = new Thread(() => Work1(cts.Token));
            t.Start();

            Thread.Sleep(TimeSpan.FromSeconds(3));

            // キャンセル実行.
            cts.Cancel();

            ////////////////////////////////////////////////////////////////////
            //
            // ThreadPoolを利用してのキャンセル処理.
            //
            // CancellationTokenSourceは、一度キャンセルすると
            // 再利用できない構造となっている。（つまり、キャンセル後に取得したTokenを利用しても
            // 最初からキャンセルされた事になっている。）
            //
            cts = new CancellationTokenSource();
            ThreadPool.QueueUserWorkItem(obj => Work2(cts.Token), null);

            Thread.Sleep(TimeSpan.FromSeconds(3));
            cts.Cancel();

            ////////////////////////////////////////////////////////////////////
            //
            // ManualResetEventSlimを利用してのキャンセル処理.
            //
            cts = new CancellationTokenSource();

            var waitHandle = new ManualResetEventSlim(false);
            Task.Factory.StartNew(() => Work3(cts.Token, waitHandle));

            Thread.Sleep(TimeSpan.FromSeconds(3));
            cts.Cancel();

            ////////////////////////////////////////////////////////////////////
            //
            // CancellationToken.WaitHandleを利用してのキャンセル待ち.
            //
            cts = new CancellationTokenSource();
            using (var countdown = new CountdownEvent(3))
            {
                var token = cts.Token;

                Parallel.Invoke
                (
                    // 3秒後にキャンセル処理を実行.
                    () =>
                    {
                        Thread.Sleep(TimeSpan.FromSeconds(3));
                        cts.Cancel();
                        countdown.Signal();
                    },
                    // トークンのWaitHandleを利用してキャンセル待ち.
                    () =>
                    {
                        Output.WriteLine(">>> キャンセル待ち・・・");
                        token.WaitHandle.WaitOne();
                        Output.WriteLine(">>> 操作がキャンセルされたので、WaitHandleから通知されました。");
                        countdown.Signal();
                    },
                    // キャンセルされるまで実行される処理.
                    () =>
                    {
                        try
                        {
                            while (true)
                            {
                                token.ThrowIfCancellationRequested();
                                Output.WriteLine(">>> wait...");
                                Thread.Sleep(TimeSpan.FromMilliseconds(700));
                            }
                        }
                        catch (OperationCanceledException ex)
                        {
                            Output.WriteLine(">>> {0}", ex.Message);
                        }

                        countdown.Signal();
                    }
                );

                countdown.Wait();
            }

            ////////////////////////////////////////////////////////////////////
            //
            // CancellationToken.Registerを利用した関連オブジェクトのキャンセル操作.
            // CancellationToken.Registerメソッドには、キャンセルされた際に実行される
            // アクションを設定することが出来る。これを利用することで、トークンのキャンセル時に
            // 関連してキャンセル処理やキャンセル時にのみ実行する処理を記述することが出来る。
            //
            // 以下では、WebClientを利用して非同期処理を行っている最中にトークンをキャンセルし
            // さらに、WebClientもキャンセルするようにしている。（若干強引だが・・・・w）
            //
            cts = new CancellationTokenSource();

            var token2 = cts.Token;
            var client = new WebClient();

            client.DownloadStringCompleted += (s, e) => { Output.WriteLine(">>> キャンセルされた？ == {0}", e.Cancelled); };

            token2.Register(() =>
                {
                    Output.WriteLine(">>> 操作がキャンセルされたので、WebClient側もキャンセルします。");
                    client.CancelAsync();
                }
            );

            Output.WriteLine(">>> WebClient.DownloadStringAsync...");
            client.DownloadStringAsync(new Uri(@"http://d.hatena.ne.jp/gsf_zero1/"));

            Thread.Sleep(TimeSpan.FromMilliseconds(200));
            cts.Cancel();

            ////////////////////////////////////////////////////////////////////
            //
            // CancellationTokenSourceには、複数のトークンを同期させるための
            // CreateLinkedTokenSourceメソッドが存在する。
            // このメソッドを利用することにより、複数のトークンのキャンセルを処理することが出来る。
            //
            // 尚、CreateLinkedTokenSourceで作成したリンクトークンソースは
            // Disposeしないといけない事に注意。
            //
            var cts2 = new CancellationTokenSource();
            var cts3 = new CancellationTokenSource();

            var cts2Token = cts2.Token;
            var cts3Token = cts3.Token;

            using (var linkedCts = CancellationTokenSource.CreateLinkedTokenSource(cts2Token, cts3Token))
            {
                var linkedCtsToken = linkedCts.Token;

                using (var countdown = new CountdownEvent(2))
                {
                    Parallel.Invoke
                    (
                        // 1秒後にcts2をキャンセル
                        () =>
                        {
                            Thread.Sleep(TimeSpan.FromSeconds(1));
                            Output.WriteLine(">>> cts2.Canel()");
                            cts2.Cancel();

                            countdown.Signal();
                        },
                        // 2秒後にcts3をキャンセル.
                        () =>
                        {
                            Thread.Sleep(TimeSpan.FromSeconds(2));
                            Output.WriteLine(">>> cts3.Canel()");
                            cts3.Cancel();

                            countdown.Signal();
                        }
                    );

                    countdown.Wait();
                }

                // 各トークンの状態をチェック.
                Output.WriteLine(">>>> cts2Token.IsCancellationRequested == {0}", cts2Token.IsCancellationRequested);
                Output.WriteLine(">>>> cts3Token.IsCancellationRequested == {0}", cts3Token.IsCancellationRequested);
                // リンクトークンなので、紐づくトークン全てがキャンセルになると自動的にキャンセル状態となる。
                Output.WriteLine(">>>> linkedCtsToken.IsCancellationRequested == {0}", linkedCtsToken.IsCancellationRequested);
            }

            Thread.Sleep(TimeSpan.FromSeconds(1));
        }

        private void Work1(CancellationToken cancelToken)
        {
            //
            // キャンセル処理を実装する場合、try-catchを用意して
            // OperationCanceledExceptionを受け取るようにしておく.
            //
            try
            {
                while (true)
                {
                    //
                    // もし、外部でキャンセルされていた場合
                    // このメソッドはOperationCanceledExceptionを発生させる。
                    //
                    cancelToken.ThrowIfCancellationRequested();

                    Output.WriteLine(">> wait...");
                    Thread.Sleep(TimeSpan.FromSeconds(1));
                }
            }
            catch (OperationCanceledException ex)
            {
                //
                // キャンセルされた.
                //
                Output.WriteLine(">>> {0}", ex.Message);
            }
        }

        private void Work2(CancellationToken cancelToken)
        {
            //
            // IsCancellationRequestedプロパティを利用して
            // キャンセルを検知する.
            //
            while (true)
            {
                if (cancelToken.IsCancellationRequested)
                {
                    // キャンセルされた.
                    Output.WriteLine(">>> 操作はキャンセルされました。");
                    break;
                }

                Output.WriteLine(">> wait...");
                Thread.Sleep(TimeSpan.FromSeconds(1));
            }
        }

        private void Work3(CancellationToken cancelToken, ManualResetEventSlim waitHandle)
        {
            try
            {
                Output.WriteLine(">> waitHandle.Wait...");
                waitHandle.Wait(cancelToken);
                Output.WriteLine(">> awake!");
            }
            catch (OperationCanceledException ex)
            {
                // キャンセルされた.
                Output.WriteLine(">>> {0}", ex.Message);
            }
        }
    }
}