using System;
using System.Threading;
using TryCSharp.Common;

namespace TryCSharp.Samples.Threading
{
    [Sample]
    public class ThreadingNamespaceSamples05 : IExecutable
    {
        public void Execute()
        {
            //
            // 普通にスレッドタイマーを作成し、コールバックの呼び出し間隔を無効に
            // した状態でタイマーを開始させる.
            //
            var timer = new Timer(TimerCallback);
            timer.Change(0, Timeout.Infinite);

            Thread.Sleep(10000);
        }

        private void TimerCallback(object? state)
        {
            Output.WriteLine("Timer Callback!!");

            var rnd = new Random();

            // 時間のかかる処理をシミュレート
            Thread.Sleep(rnd.Next(1000));
            Output.WriteLine("\tsleep done.");

            //
            // 再度Changeメソッドを呼び出して、次のコールバックを設定.
            //
            var timer = state as Timer;
            timer?.Change(rnd.Next(700), Timeout.Infinite);
        }
    }
}