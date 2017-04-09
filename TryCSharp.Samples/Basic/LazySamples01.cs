using System;
using System.Threading;
using System.Threading.Tasks;
using TryCSharp.Common;

namespace TryCSharp.Samples.Basic
{
    /// <summary>
    ///     Lazy<T>, LazyInitializerクラスのサンプルです。
    /// </summary>
    [Sample]
    public class LazySamples01 : IExecutable
    {
        public void Execute()
        {
            //
            // Lazy<T>クラスは、遅延初期化 (Lazy Initialize)機能を付与するクラスである。
            //
            // 利用する際は、LazyクラスのコンストラクタにFunc<T>を指定することにより
            // 初期化処理を指定する。（たとえば、コストのかかるオブジェクトの構築などをFuncデリゲート内にて処理など）
            //
            // また、コンストラクタにはFunc<T>の他にも、第二引数としてスレッドセーフモードを指定出来る。
            // (System.Threading.LazyThreadSafetyMode)
            //
            // スレッドセーフモードは、Lazyクラスが遅延初期化処理を行う際にどのレベルのスレッドセーフ処理を適用するかを指定するもの。
            // スレッドセーフモードの指定は、LazyクラスのコンストラクタにてLazyThreadSafetyModeかboolで指定する。
            //   ・None:                    スレッドセーフ無し。速度が必要な場合、または、呼び元にてスレッドセーフが保証出来る場合に利用
            //   ・PublicationOnly:         複数のスレッドが同時に値の初期化を行う事を許可するが、最初に初期化に成功したスレッドが
            //                             Lazyインスタンスの値を設定するモード。（race-to initialize)
            //   ・ExecutionAndPublication: 完全スレッドセーフモード。一つのスレッドのみが初期化を行えるモード。
            //                             (double-checked locking)
            //
            // Lazyクラスのコンストラクタにて、スレッドセーフモードをbool型で指定する場合、以下のLazyThreadSafetyModeの値が指定された事と同じになる。
            //    ・true : LazyThreadSafetyMode.ExecutionAndPublicationと同じ。
            //    ・false: LazyThreadSafetyMode.Noneと同じ。
            //
            // Lazyクラスは、例外のキャッシュ機能を持っている。これは、Lazy.Valueを呼び出した際にコンストラクタで指定した
            // 初期化処理内で例外が発生した事を検知する際に利用する。Lazyクラスのコンストラクタにて、既定コンストラクタを使用するタイプの
            // 設定を行っている場合、例外のキャッシュは有効にならない。
            //
            // また、LazyThreadSafetyMode.PublicationOnlyを指定した場合も、例外のキャッシュは有効とならない。
            //
            // 排他モードで初期化処理を実行
            var lazy1 = new Lazy<HeavyObject>(() => new HeavyObject(TimeSpan.FromMilliseconds(100)), LazyThreadSafetyMode.ExecutionAndPublication);
            // 尚、上は以下のように第二引数をtrueで指定した場合と同じ事。
            // var lazy1 = new Lazy(() => new HeavyObject(TimeSpan.FromSeconds(1)), true);

            // 値が初期化済みであるかどうかは、IsValueCreatedで確認出来る。
            Output.WriteLine("値構築済み？ == {0}", lazy1.IsValueCreated);

            //
            // 複数のスレッドから同時に初期化を試みてみる。 (ExecutionAndPublication)
            //
            Parallel.Invoke
            (
                () =>
                {
                    Output.WriteLine("[lambda1] 初期化処理実行 start.");

                    if (lazy1.IsValueCreated)
                    {
                        Output.WriteLine("[lambda1] 既に値が作成されている。(IsValueCreated=true)");
                    }
                    else
                    {
                        Output.WriteLine("[lambda1] ThreadId={0}", Thread.CurrentThread.ManagedThreadId);
                        var obj = lazy1.Value;
                    }

                    Output.WriteLine("[lambda1] 初期化処理実行 end.");
                },
                () =>
                {
                    Output.WriteLine("[lambda2] 初期化処理実行 start.");

                    if (lazy1.IsValueCreated)
                    {
                        Output.WriteLine("[lambda2] 既に値が作成されている。(IsValueCreated=true)");
                    }
                    else
                    {
                        Output.WriteLine("[lambda2] ThreadId={0}", Thread.CurrentThread.ManagedThreadId);
                        var obj = lazy1.Value;
                    }

                    Output.WriteLine("[lambda2] 初期化処理実行 end.");
                }
            );

            Output.WriteLine("==========================================");

            //
            // 複数のスレッドにて同時に初期化処理の実行を許可するが、最初に初期化した値が設定されるモード。
            // (PublicationOnly)
            //
            var lazy2 = new Lazy<HeavyObject>(() => new HeavyObject(TimeSpan.FromMilliseconds(100)), LazyThreadSafetyMode.PublicationOnly);

            Parallel.Invoke
            (
                () =>
                {
                    Output.WriteLine("[lambda1] 初期化処理実行 start.");

                    if (lazy2.IsValueCreated)
                    {
                        Output.WriteLine("[lambda1] 既に値が作成されている。(IsValueCreated=true)");
                    }
                    else
                    {
                        Output.WriteLine("[lambda1] ThreadId={0}", Thread.CurrentThread.ManagedThreadId);
                        var obj = lazy2.Value;
                    }

                    Output.WriteLine("[lambda1] 初期化処理実行 end.");
                },
                () =>
                {
                    Output.WriteLine("[lambda2] 初期化処理実行 start.");

                    if (lazy2.IsValueCreated)
                    {
                        Output.WriteLine("[lambda2] 既に値が作成されている。(IsValueCreated=true)");
                    }
                    else
                    {
                        Output.WriteLine("[lambda2] ThreadId={0}", Thread.CurrentThread.ManagedThreadId);
                        var obj = lazy2.Value;
                    }

                    Output.WriteLine("[lambda2] 初期化処理実行 end.");
                }
            );

            Output.WriteLine("値構築済み？ == {0}", lazy1.IsValueCreated);
            Output.WriteLine("値構築済み？ == {0}", lazy2.IsValueCreated);

            Output.WriteLine("lazy1のスレッドID: {0}", lazy1.Value.CreatedThreadId);
            Output.WriteLine("lazy2のスレッドID: {0}", lazy2.Value.CreatedThreadId);
        }

        private class HeavyObject
        {
            public HeavyObject(TimeSpan waitSpan)
            {
                Output.WriteLine(">>>>>> HeavyObjectのコンストラクタ start. [{0}]", Thread.CurrentThread.ManagedThreadId);
                Initialize(waitSpan);
                Output.WriteLine(">>>>>> HeavyObjectのコンストラクタ end.   [{0}]", Thread.CurrentThread.ManagedThreadId);
            }

            public int CreatedThreadId { get; private set; }

            private void Initialize(TimeSpan waitSpan)
            {
                Thread.Sleep(waitSpan);
                CreatedThreadId = Thread.CurrentThread.ManagedThreadId;
            }
        }
    }
}