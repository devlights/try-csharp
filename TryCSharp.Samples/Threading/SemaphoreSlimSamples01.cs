using System.Threading;
using TryCSharp.Common;

namespace TryCSharp.Samples.Threading
{
    /// <summary>
    ///     SemaphoreSlimクラスについてのサンプルです。
    /// </summary>
    /// <remarks>
    ///     SemaphoreSlimクラスは、.NET 4.0から追加されたクラスです。
    ///     従来から存在していたSemaphoreクラスの軽量版となります。
    /// </remarks>
    [Sample]
    public class SemaphoreSlimSamples01 : IExecutable
    {
        public void Execute()
        {
            //
            // SemaphoreSlimクラスは、Semaphoreクラスの軽量版として
            // .NET 4.0から追加されたクラスである。
            //
            // Semaphoreは、リソースに同時にアクセス出来るスレッドの数を制限するために利用される。
            //
            // 機能的には、Semaphoreクラスと大差ないが以下の機能が追加されている。
            //   キャンセルトークンを受け付けるWaitメソッドのオーバーロードが存在する。
            // キャンセルトークンを受け付けるWaitメソッドに関しては、CountdownEventクラスやBarrierクラス
            // と利用方法は同じである。
            //
            // 尚、元々のSemaphoreクラスでは、WaitOneメソッドだったものが
            // SemaphoreSlimクラスでは、Waitメソッドという名前に変わっている。
            //

            //
            // Waitメソッドの利用.
            //
            // Waitメソッドは、入ることが出来た場合はTrueを返す。
            // 既に上限までスレッドが入っている場合はFalseが返却される。
            // (つまりブロックされる。)
            //
            // 引数無しのWaitメソッドは、入ることが出来るまでブロックされるメソッドとなる。
            // 結果をboolで受け取る場合は、Int32を引数にとるWaitメソッドを利用する。
            // 0を指定すると即結果が返ってくる。-1を指定すると無制限に待つ。
            // (引数無しのWaitメソッドと同じ。)
            //
            // SemaphoreSlimでは、AvailableWaitHandleプロパティよりWaitHandleを取得することが出来る。
            // ただし、このWaitHandleは、SemaphoreSlim本体とは連携しているわけでは無い。
            // なので、このWaitHandle経由でWaitOneを実行しても、SemaphoreSlim側のカウントは変化しないので注意。
            //
            using (var semaphore = new SemaphoreSlim(2))
            {
                // 現在Semaphoreに入ることが可能なスレッド数を表示
                Output.WriteLine("CurrentCount={0}", semaphore.CurrentCount);

                // 1つ目
                Output.WriteLine("1つ目のWait={0}", semaphore.Wait(0));
                // 2つ目
                Output.WriteLine("2つ目のWait={0}", semaphore.Wait(0));

                // 現在Semaphoreに入ることが可能なスレッド数を表示
                Output.WriteLine("CurrentCount={0}", semaphore.CurrentCount);

                // 3つ目
                // 現在Releaseしている数は0なので、入ることが出来ない。
                // (Falseが返却される)
                Output.WriteLine("3つ目のWait={0}", semaphore.Wait(0));

                // １つリリースして、枠を空ける.
                semaphore.Release();

                // 現在Semaphoreに入ることが可能なスレッド数を表示
                Output.WriteLine("CurrentCount={0}", semaphore.CurrentCount);

                // 再度、3つ目
                // 今度は、枠が空いているので入ることが出来る。
                Output.WriteLine("3つ目のWait={0}", semaphore.Wait(0));

                // 現在Semaphoreに入ることが可能なスレッド数を表示
                Output.WriteLine("CurrentCount={0}", semaphore.CurrentCount);

                semaphore.Release();
                semaphore.Release();

                // 現在Semaphoreに入ることが可能なスレッド数を表示
                Output.WriteLine("CurrentCount={0}", semaphore.CurrentCount);
            }
        }
    }
}