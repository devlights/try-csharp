using System;
using System.Threading;
using System.Threading.Tasks;
using TryCSharp.Common;

namespace TryCSharp.Samples.Basic
{
    /// <summary>
    ///     LazyInitializerのサンプルです。
    /// </summary>
    [Sample]
    public class LazyInitializerSamples01 : IExecutable
    {
        public void Execute()
        {
            //
            // LazyInitializerは、Lazyと同様に遅延初期化を行うための
            // クラスである。このクラスは、staticメソッドのみで構成され
            // Lazyでの記述を簡便化するために存在する。
            //
            // EnsureInitializedメソッドは
            // Lazyクラスにて、LazyThreadSafetyMode.PublicationOnlyを
            // 指定した場合と同じ動作となる。(race-to-initialize)
            //
            var hasHeavy = new HasHeavyData();

            Parallel.Invoke
            (
                () => { Output.WriteLine("Created. [{0}]", hasHeavy.Heavy.CreatedThreadId); },
                () => { Output.WriteLine("Created. [{0}]", hasHeavy.Heavy.CreatedThreadId); },
                // 少し待機してから、作成済みの値にアクセス.
                () =>
                {
                    Thread.Sleep(TimeSpan.FromMilliseconds(2000));
                    Output.WriteLine(">>少し待機してから、作成済みの値にアクセス.");
                    Output.WriteLine(">>Created. [{0}]", hasHeavy.Heavy.CreatedThreadId);
                }
            );
        }

        private class HasHeavyData
        {
            private HeavyObject? _heavy;

            public HeavyObject Heavy
            {
                get
                {
                    //
                    // LazyInitializerを利用して、遅延初期化.
                    //
                    Output.WriteLine("[ThreadId {0}] 値初期化処理開始. start", Thread.CurrentThread.ManagedThreadId);
                    LazyInitializer.EnsureInitialized(ref _heavy, () => new HeavyObject(TimeSpan.FromMilliseconds(100)));
                    Output.WriteLine("[ThreadId {0}] 値初期化処理開始. end", Thread.CurrentThread.ManagedThreadId);

                    return _heavy;
                }
            }
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