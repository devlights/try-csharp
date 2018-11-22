using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using TryCSharp.Common;

namespace TryCSharp.Samples.Threading
{
    /// <summary>
    ///     Barrierクラスについてのサンプルです。
    /// </summary>
    /// <remarks>
    ///     Barrierクラスは、.NET 4.0から追加されたクラスです。
    /// </remarks>
    [Sample]
    public class BarrierSamples01 : IExecutable
    {
        // 計算値を保持する変数
        private long _count;

        public void Execute()
        {
            //
            // Barrierクラスは、並行処理を複数のフェーズ毎に協調動作させる場合に利用する.
            // つまり、同時実行操作を同期する際に利用出来る。
            //
            // 例えば、論理的に3フェーズ存在する処理があったとして、並行して動作する処理が2つあるとする。
            // 各並行処理に対して、フェーズ毎に一旦結果を収集し、また平行して処理を行う事とする。
            // そのような場合に、Barrierクラスが役に立つ。
            //
            // Barrierクラスをインスタンス化する際に、対象となる並行処理の数をコンストラクタに指定する。
            // コンストラクタには、フェーズ毎に実行されるコールバックを設定することも出来る。
            //
            // 後は、Barrier.SignalAndWaitを、各並行処理が呼び出せば良い。
            // コンストラクタに指定した数分、SignalAndWaitが呼び出された時点で1フェーズ終了となり
            // 設定したコールバックが実行される。
            //
            // 各並行処理は、SignalAndWaitを呼び出した後、Barrierにて指定した処理数分のSignalAndWaitが
            // 呼び出されるまで、ブロックされる。
            //
            // 対象とする並行処理数は、以下のメソッドを利用することにより増減させることが出来る。
            //   ・AddParticipants
            //   ・RemoveParticipants
            //
            // CountdownEvent, ManualResetEventSlimと同じく、このクラスのSignalAndWaitメソッドも
            // CancellationTokenを受け付けるオーバーロードが存在する。
            //
            // CountdownEventと同じく、このクラスもIDisposableを実装しているのでusing可能。
            //

            //
            // 5つの処理を、特定のフェーズ毎に同期させながら実行.
            // さらに、フェーズ単位で途中結果を出力するようにする.
            //
            using (var barrier = new Barrier(5, PostPhaseProc))
            {
                Parallel.Invoke(
                    () => ParallelProc(barrier, 10, 123456, 2),
                    () => ParallelProc(barrier, 20, 678910, 3),
                    () => ParallelProc(barrier, 30, 749827, 5),
                    () => ParallelProc(barrier, 40, 847202, 7),
                    () => ParallelProc(barrier, 50, 503295, 777)
                );
            }

            Output.WriteLine("最終値：{0}", _count);
        }

        //
        // 各並列処理用のアクション.
        //
        private void ParallelProc(Barrier barrier, int randomMaxValue, int randomSeed, int modValue)
        {
            //
            // 第一フェーズ.
            //
            Calculate(barrier, randomMaxValue, randomSeed, modValue, 100);

            //
            // 第二フェーズ.
            //
            Calculate(barrier, randomMaxValue, randomSeed, modValue, 5000);

            //
            // 第三フェーズ.
            //
            Calculate(barrier, randomMaxValue, randomSeed, modValue, 10000);
        }

        //
        // 計算処理.
        //
        private void Calculate(Barrier barrier, int randomMaxValue, int randomSeed, int modValue, int loopCountMaxValue)
        {
            var rnd = new Random(randomSeed);
            var watch = Stopwatch.StartNew();

            var loopCount = rnd.Next(loopCountMaxValue);
            Output.WriteLine("[Phase{0}] ループカウント：{1}, TASK:{2}", barrier.CurrentPhaseNumber, loopCount, Task.CurrentId);

            for (var i = 0; i < loopCount; i++)
            {
                // 適度に時間がかかるように調整.
                if (rnd.Next(10000)%modValue == 0)
                {
                    Thread.Sleep(TimeSpan.FromMilliseconds(10));
                }

                Interlocked.Add(ref _count, i + rnd.Next(randomMaxValue));
            }

            watch.Stop();
            Output.WriteLine("[Phase{0}] SignalAndWait -- TASK:{1}, ELAPSED:{2}", barrier.CurrentPhaseNumber, Task.CurrentId, watch.Elapsed);

            try
            {
                //
                // シグナルを発行し、仲間のスレッドが揃うのを待つ.
                //
                barrier.SignalAndWait();
            }
            catch (BarrierPostPhaseException postPhaseEx)
            {
                //
                // Post Phaseアクションにてエラーが発生した場合はここに来る.
                // (本来であれば、キャンセルするなどのエラー処理が必要)
                //
                Output.WriteLine("*** {0} ***", postPhaseEx.Message);
                throw;
            }
        }

        //
        // Barrierにて、各フェーズ毎が完了した際に呼ばれるコールバック.
        // (Barrierクラスのコンストラクタにて設定する)
        //
        private void PostPhaseProc(Barrier barrier)
        {
            //
            // Post Phaseアクションは、同時実行している処理が全てSignalAndWaitを
            // 呼ばなければ発生しない。
            //
            // つまり、この処理が走っている間、他の同時実行処理は全てブロックされている状態となる。
            //
            var current = Interlocked.Read(ref _count);

            Output.WriteLine("現在のフェーズ：{0}, 参加要素数：{1}", barrier.CurrentPhaseNumber, barrier.ParticipantCount);
            Output.WriteLine("t現在値：{0}", current);

            //
            // 以下のコメントを外すと、次のPost Phaseアクションにて
            // 全てのSignalAndWaitを呼び出している、処理にてBarrierPostPhaseExceptionが
            // 発生する。
            //
            //throw new InvalidOperationException("dummy");
        }
    }
}