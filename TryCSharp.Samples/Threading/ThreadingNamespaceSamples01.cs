using System;
using System.Collections.Generic;
using System.Threading;
using TryCSharp.Common;

namespace TryCSharp.Samples.Threading
{
    /// <summary>
    ///     System.Threading名前空間に存在するクラスのサンプルです。
    /// </summary>
    [Sample]
    public class ThreadingNamespaceSamples01 : IExecutable
    {
        public void Execute()
        {
            //
            // Threadクラス (1)
            //    基本的なメソッドなどについて.
            //

            //////////////////////////////////////////////////////////////////////////////
            //
            // ThreadStartデリゲートを用いた方法.
            //    ThreadStartデリゲートは引数も戻り値も持っていません。
            //    このデリゲートを利用したパターンで各スレッドにてデータを
            //    持たせる場合は、デリゲートメソッドを内包した状態クラスを作成します。
            //    後で結果を受け取る必要がある場合も同様です。
            //
            var states = new List<ThreadStartDelegateState>();
            for (var i = 0; i < 5; i++)
            {
                var state = new ThreadStartDelegateState();
                var thread = new Thread(state.ThreadStartHandlerMethod);

                state.ArgumentData = $"{i}番目のスレッド";
                state.TargetThread = thread;

                states.Add(state);

                //
                // 新規で作成したThreadはデフォルトでフォアグラウンドスレッドとなっている。
                // （ThreadPoolスレッドの場合はデフォルトでバックグラウンドスレッドとなっている。)
                //
                thread.IsBackground = true;

                //
                // 別スレッド処理を開始する。
                // Startメソッドは即座に制御を呼び元に戻す。
                //
                thread.Start();
            }

            //
            // 全スレッドが終わってから結果を確認したいので、待ち合わせ.
            // (以下の一行をコメントアウトして実行すると、高い確率でReturnDataプロパティの値が
            //  設定されていない状態で結果が出力されるはずです。(結果がセットされる前にメイン処理が
            //  結果確認処理へと進むため))
            //
            states.ForEach(state => { state.TargetThread.Join(); });

            // 結果確認.
            states.ForEach(Output.WriteLine);

            //////////////////////////////////////////////////////////////////////////////
            //
            // ParameterizedThreadStartデリゲートを用いた方法.
            //    ParameterizedThreadStartデリゲートは引数を一つ与える事が出来ます。
            //    このデリゲートを利用したパターンで各スレッドにてデータを
            //    持たせる場合は、Thread.Startメソッドにて引数を引き渡します。
            //    後で結果を受け取る必要がある場合は、ThreadStartデリゲートと同じく
            //    状態クラスを作成し、戻り値を設定する必要があります。
            //
            states.Clear();
            for (var i = 0; i < 5; i++)
            {
                var state = new ThreadStartDelegateState();
                var thread = new Thread(state.ParameterizedThreadStartHandlerMethod);

                state.TargetThread = thread;
                states.Add(state);

                thread.IsBackground = true;
                thread.Start($"{i}番目のスレッド");
            }

            states.ForEach(state => { state.TargetThread.Join(); });
            states.ForEach(Output.WriteLine);
        }

        private class ThreadStartDelegateState
        {
            public Thread TargetThread { get; set; }

            public string ArgumentData { get; set; }

            public string ReturnData { get; set; }

            public void ThreadStartHandlerMethod()
            {
                Thread.Sleep(TimeSpan.FromSeconds(1));
                ReturnData = Thread.CurrentThread.ManagedThreadId.ToString();
            }

            public void ParameterizedThreadStartHandlerMethod(object threadArgument)
            {
                ArgumentData = threadArgument as string;

                ThreadStartHandlerMethod();
            }

            public override string ToString()
            {
                return $"Argument:{ArgumentData}  Return:{ReturnData}";
            }
        }
    }
}