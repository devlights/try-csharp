using System;
using System.Threading;
using TryCSharp.Common;

namespace TryCSharp.Samples.Threading
{
    /// <summary>
    ///     ThreadLocal<T>クラスのサンプルです。
    /// </summary>
    [Sample]
    public class ThreadLocalSamples01 : IExecutable
    {
        public void Execute()
        {
            //
            // ThreadLocal<T>は、.NET 4.0から追加された型である。
            // ThreadStatic属性と同様に、スレッドローカルストレージ(TLS)を表現するための型である。
            //
            // 従来より存在していたThreadStatic属性には、以下の点が行えなかった。
            //   ・インスタンスフィールドには対応していない。（staticフィールドのみ)
            //    (インスタンスフィールドにも属性を付与することが出来るが、ちゃんと動作しない）
            //   ・フィールドの値は常に、その型のデフォルト値で初期化される。初期値を設定しても無視される。
            //
            // ThreadLocal<T>は、上記の点を解決している。つまり
            //   ・インスタンスフィールドに対応している。
            //   ・フィールドの値を初期値で初期化出来る。
            //
            // 利用方法は、System.Lazyと似ており、コンストラクタに初期化のためのデリゲートを渡す。
            //

            //
            // staticフィールドのThreadState属性の確認
            // ThreadStatic属性では、初期値を宣言時に設定していても無視され、強制的にデフォルト値が適用されるので
            // 出力される値は、全て0となる。
            //
            var numberOfParallels = 10;
            using (var countdown = new CountdownEvent(numberOfParallels))
            {
                for (var i = 0; i < numberOfParallels; i++)
                {
                    new Thread(() =>
                    {
                        Output.WriteLine("ThreadStatic [count]>>> {0}", count++);
                        countdown.Signal();
                    }).Start();
                }

                countdown.Wait();
            }

            //
            // staticフィールドのThreadLocal<T>の確認
            // ThreadLocal<T>は、初期値を設定できるので、出力される値は2となる。
            //
            using (var countdown = new CountdownEvent(numberOfParallels))
            {
                for (var i = 0; i < numberOfParallels; i++)
                {
                    new Thread(() =>
                    {
                        Output.WriteLine("ThreadLocal<T> [count2]>>> {0}", count2.Value++);
                        countdown.Signal();
                    }).Start();
                }

                countdown.Wait();
            }

            //
            // インスタンスフィールドのThreadStatic属性の確認
            // ThreadStatic属性は、インスタンスフィールドに対しては効果が無い。
            // なので、出力される値は2,3,4,5,6...とインクリメントされていく.
            //
            using (var countdown = new CountdownEvent(numberOfParallels))
            {
                for (var i = 0; i < numberOfParallels; i++)
                {
                    new Thread(() =>
                    {
                        Output.WriteLine("ThreadStatic [count3]>>> {0}", count3++);
                        countdown.Signal();
                    }).Start();
                }

                countdown.Wait();
            }

            //
            // インスタンスフィールドのThreadLocal<T>の確認
            // ThreadLocal<T>は、インスタンスフィールドに対しても問題なく利用できる。
            // なので、出力される値は4となる。
            //
            using (var countdown = new CountdownEvent(numberOfParallels))
            {
                for (var i = 0; i < numberOfParallels; i++)
                {
                    new Thread(() =>
                    {
                        Output.WriteLine("ThreadLocal<T> [count4]>>> {0}", count4.Value++);
                        countdown.Signal();
                    }).Start();
                }

                countdown.Wait();
            }

            count2.Dispose();
            count4.Dispose();
        }

        #region Static Fields

        // ThreadStatic
        [ThreadStatic] private static int count = 2;

        private static readonly ThreadLocal<int> count2 = new ThreadLocal<int>(() => 2);

        #endregion

        #region Fields

        [ThreadStatic] private int count3 = 2;

        private readonly ThreadLocal<int> count4 = new ThreadLocal<int>(() => 4);

        #endregion
    }
}