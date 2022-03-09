using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;
using TryCSharp.Common;

namespace TryCSharp.Samples.Async.Channels
{
    /// <summary>
    /// System.Threading.Channels についてのサンプルです。
    /// </summary>
    /// <remarks>
    /// https://devblogs.microsoft.com/dotnet/an-introduction-to-system-threading-channels/
    /// https://deniskyashif.com/2019/12/08/csharp-channels-part-1/
    /// https://deniskyashif.com/2019/12/11/csharp-channels-part-2/
    /// https://deniskyashif.com/2020/01/07/csharp-channels-part-3/
    /// </remarks>
    public class ChannelIntroduction : IAsyncExecutable
    {
        public async Task Execute()
        {
            // -----------------------------------------------------------------------------
            // 以下のページを勉強した際のメモ
            //
            // An Introduction to System.Threading.Channels
            //   https://devblogs.microsoft.com/dotnet/an-introduction-to-system-threading-channels/
            // -----------------------------------------------------------------------------

            // -----------------------------------------------------------------------------
            // "生産者/消費者の問題は、私たちの生活のあらゆる面で、いたるところにあります。
            // ファストフード店のラインコックは、トマトをスライスし、それを別のコックに渡してハンバーガーを組み立てる。
            // 郵便物を配達しているドライバーは、トラックが到着して郵便受けに配達物を取りに行くのを見ているか、
            // 仕事から帰ってきた後に確認しているかのどちらかです。
            // 航空会社の従業員がジェット旅客機の貨物室からスーツケースを降ろし、ベルトコンベアーに乗せ、別の従業員がバンに荷物を移し、
            // さらに別のコンベアーに乗せてあなたのところに運んでいます。
            // そして、結婚式の招待状を送る準備をしている幸せな婚約者のカップルは、一方のパートナーが封筒に宛名を書いて、
            // もう一方のパートナーに封筒を渡して、それを詰めたり舐めたりしています。
            //
            // ソフトウェア開発者として、私たちは日常生活の中で起こる出来事がソフトウェアに反映されるのを日常的に目にしています。
            // コマンドラインでコマンドをパイプでまとめて実行したことがある人なら誰でも、
            // あるプログラムの標準出力が別のプログラムの標準入力として供給されることで、
            // プロデューサー/コンシューマーを利用したことがあるでしょう。
            // 離散的な値を計算したり、複数のサイトからデータをダウンロードしたりするために
            // 複数のワーカーを起動したことがある人は誰でも、producer/consumerを利用しています。
            // パイプラインを並列化しようとしたことがある人は誰でも、非常に明確にproducer/consumerを採用しています。
            // このようにして、パイプラインを並列化しようとしたことがある人は誰でも、非常に明確にプロデューサー/消費者を採用してきました。
            //
            // 現実世界でもソフトウェアの世界でも、これらのシナリオには共通していることがあります。
            // ファーストフード店の従業員は、完成したハンバーガーをレジ係が顧客の袋に入れるために引き出したスタンドに置きます。
            // 郵便局員は郵便物をポストに入れる。婚約しているカップルの手は、一方から他方へと材料を転送するために会う。
            // ソフトウェアでは、このような手渡しは、トランザクションを容易にするために何らかのデータ構造を必要とし、
            // 生産者が結果を転送してさらにバッファリングするために使用できるストレージを必要とし、
            // 一方で、消費者が1つ以上の結果が利用可能であることを通知できるようにします。
            // System.Threading.Channelsに入りましょう。
            //
            // www.DeepL.com/Translator（無料版）で翻訳しました。
            // -----------------------------------------------------------------------------

            // -----------------------------------------------------------------------------
            // What is a Channel?
            //
            // - チャネルとは Producer/Consumer パターンを扱うためのデータ構造
            // - 以下の特徴を持つ
            //   - スレッドセーフ
            //   - 読み書き可能
            //   - バッファリングの有無
            // - 複数のスレッドから同時に 書き込み しても良い
            // - 複数のスレッドから同時に 読み込み しても良い
            // - CancellationTokenを受け付けるので任意のタイミングでキャンセル可能
            // - ChannelWriter<T>.Complete() を呼び出すことでチャネルに今後書き込まないことを通知可能
            //   - チャネルを閉じる際に利用できる
            // -----------------------------------------------------------------------------

            // -----------------------------------------------------------------------------
            // 超ベーシックなチャネル実装
            var ch1 = new SuperBasicChannel<int>();
            var itemCount = 5;
            var producer1 = Task.Run(() =>
            {
                for (var i = 0; i < itemCount; i++)
                {
                    ch1.Write(i);
                }
            });
            var consumer1 = Task.Run(async () =>
            {
                for (var i = 0; i < itemCount; i++)
                {
                    var item = await ch1.ReadAsync();
                    Output.WriteLine($"[ch1] {item}");
                }
            });

            await Task.WhenAll(producer1, consumer1);
            Output.WriteLine("----------------------------------");

            // -----------------------------------------------------------------------------
            // Introducing System.Threading.Channels
            //
            // チャネルはざっくりと以下のようなインターフェースを持っています。
            //
            // <書き込み側のチャネル>
            // ChannelWriter<T>
            // {
            //     bool TryWrite(T item);
            //     ValueTask WriteAsync(T item, CancellationToken token = default);
            //     ValueTask<bool> WaitToWriteAsync(CancellationToken token = default);
            //     void Complete(Exception error = null);
            //     bool TryComplete(Exception error = null);
            // }
            //
            // <読み込み側のチャネル>
            // ChannelReader<T>
            // {
            //     bool TryRead(out T item);
            //     ValueTask<T> ReadAsync(CancellationToken token = default);
            //     ValueTask<bool> WaitToReadAsync(CancellationToken token = default);
            //     IAsyncEnumerable<T> ReadAllAsync(CancellationToken cancellationToken = default);
            //     Task Completion { get; }
            // }
            //
            // <チャネル本体>
            // Channel<T>
            // {
            //     ChannelWriter<T> Writer { get; }
            //     ChannelReader<T> Reader { get; }
            // }
            //
            // ChannelWriter<T>はWriteメソッドと非常によく似たTryWriteメソッドを提供していますが、
            // 抽象化されていて、Tryメソッドはブール値を返します。
            // しかし、ChannelWriter<T>はWriteAsyncメソッドも提供しています。
            // チャンネルがいっぱいで書き込みを待たなければならないような場合(「バックプレッシャー」と呼ばれることもあります)、
            // WriteAsyncを使用することができます。
            //
            // もちろん、コードがすぐに値を生成したくない状況もあります。
            // 値を生成することが高価な場合や、値が高価なリソースを表している場合
            // (多くのメモリを消費するような大きなオブジェクトかもしれませんし、オープンファイルの束を保存しているかもしれません)、
            // そして生成者が消費者よりも高速に動作している可能性がある場合、生成者は書き込みがすぐに成功することを知るまで
            // 値の生成を遅らせたいと思うかもしれません。
            // これと関連するシナリオのために、WaitToWriteAsyncがあります。
            // プロデューサは WaitToWriteAsync が真を返すのを待ち、
            // それからチャンネルへの TryWrite または WriteAsync で値を生成することを選択します。
            //
            // www.DeepL.com/Translator（無料版）で翻訳しました。
            // -----------------------------------------------------------------------------

            // -----------------------------------------------------------------------------
            // System.Threading.Channels の Channel<T> の使い方 (1)
            var ch2 = Channel.CreateUnbounded<int>();

            var producer2 = Task.Run(async () =>
            {
                var outCh = ch2.Writer;
                for (var i = 0; i < itemCount; i++)
                {
                    try
                    {
                        await outCh.WriteAsync(i);
                    }
                    catch (Exception ex)
                    {
                        Output.WriteLine(ex);
                    }
                }

                outCh.Complete();
            });

            var consumer2 = Task.Run(async () =>
            {
                var inCh = ch2.Reader;
                while (await inCh.WaitToReadAsync())
                {
                    var item = await inCh.ReadAsync();
                    Output.WriteLine($"[ch2] {item}");
                }
            });

            await Task.WhenAll(producer2, consumer2);
            Output.WriteLine("----------------------------------");

            // -----------------------------------------------------------------------------
            // System.Threading.Channels の Channel<T> の使い方 (2)

            // CreateUnboundedメソッドは、保存できるアイテムの数に制限を課さないチャンネルを作成します。
            // TryWriteは常にtrueを返し、WriteAsyncとWaitToWriteAsyncは常に同期的に完了します。
            var ch3 = Channel.CreateUnbounded<int>();
            await Task.WhenAll(
                Task.Run(async () =>
                {
                    var outCh = ch3.Writer;
                    for (var i = 0; i < itemCount; i++)
                    {
                        // WaitToWriteAsyncとTryWriteがどのように使用できるかを示すことに加えて、
                        // これはさらにいくつかの興味深いことを強調しています。
                        //
                        // 最初に、whileループがここに存在しているのは、
                        // デフォルトではチャンネルは任意の数の生産者と任意の数の消費者が同時に使用できるからです。
                        // チャンネルが保存できるアイテムの数に上限があり、複数のスレッドがバッファに書き込むために競争している場合、
                        // 2つのスレッドがWaitToWriteAsyncを介して「はい、スペースがあります」と言われる可能性がありますが、
                        // そのうちの1つが競争条件を失い、TryWriteがfalseを返すことになります。
                        //
                        // この例は、WaitToWriteAsyncがValueTaskの代わりにValueTask<bool>を返す理由と、
                        // TryWriteがfalseを返す可能性があるフルバッファを超えた状況を強調しています。
                        //
                        // チャンネルは完了という概念をサポートしています。
                        // 生産者は消費者にそれ以上のアイテムが生産されないことを知らせることができ、
                        // 消費者は消費しようとするのを潔く止めることができます。
                        //
                        // これは、以前に ChannelWriter<T> で示した Complete メソッドや TryComplete メソッドによって行われます
                        // (Complete は TryComplete を呼び出して false を返した場合に throw するように実装されているだけです)。
                        // しかし、あるプロデューサーがチャンネルを完了したとマークした場合、
                        // 他のプロデューサーはチャンネルへの書き込みを歓迎していないことを知る必要があります。
                        //
                        // その場合、TryWrite は false を返し、
                        // WaitToWriteAsync も false を返し、
                        // WriteAsync は 例外 をスローします。
                        //
                        // www.DeepL.com/Translator（無料版）で翻訳しました。
                        while (await outCh.WaitToWriteAsync().ConfigureAwait(false))
                        {
                            if (outCh.TryWrite(i))
                            {
                                break;
                            }
                        }
                    }

                    outCh.Complete();
                }),
                Task.Run(async () =>
                {
                    // ChannelReader<T> からの消費方法には、さまざまな典型的なパターンがあります。
                    // チャネルが終わりのない値のストリームを表している場合、1 つのアプローチは、
                    // 単に ReadAsync を介して消費する無限ループに座っているだけです。
                    //
                    // while (true)
                    // {
                    //     T item = await inCh.ReadAsync();
                    //     Use(item);
                    // }
                    //
                    // もちろん、値のストリームが無限ではなく、ある時点でチャネルが完了しているとマークされる場合、
                    // 一度消費者がチャネルのすべてのデータを空にしてしまうと、それ以降の ReadAsync の試みはスローされます。
                    // 対照的に TryRead は false を返し、WaitToReadAsync は false を返します。
                    // つまり、より一般的な消費パターンは、入れ子になったループを経由することです。
                    //
                    // while (await inCh.WaitToReadAsync())
                    // {
                    //     while (inCh.TryRead(out var item))
                    //     {
                    //         Use(item);
                    //     }
                    // }
                    //
                    // 内部の "while" は代わりに単純な "if" にすることができましたが、
                    // タイトな内部ループを持つことで、コスト意識の高い開発者は TryRead がアイテムを正常に消費するようなアイテムが
                    // すでに利用可能な場合に WaitToReadAsync のわずかな追加オーバーヘッドを回避することができます。
                    // 実際、これは ReadAllAsync メソッドで採用されているパターンと同じです。
                    // ReadAllAsync は .NET Core 3.0 で導入され、IAsyncEnumerable<T> を返します。
                    // これにより、おなじみの言語構造を使用してチャネルからすべてのデータを読み取ることができます。
                    //
                    // await foreach(var item in inCh.ReadAllAsync())
                    // {
                    //     Use(item);
                    // }
                    //
                    // www.DeepL.com/Translator（無料版）で翻訳しました。
                    var inCh = ch3.Reader;
                    while (await inCh.WaitToReadAsync())
                    {
                        while (inCh.TryRead(out var item))
                        {
                            Output.WriteLine($"[ch3] {item}");
                        }
                    }
                })
            );
            Output.WriteLine("----------------------------------");

            // -----------------------------------------------------------------------------
            // System.Threading.Channels の Channel<T> の使い方 (3)

            // CreateBoundedメソッドは、実装によって維持される明示的な制限を持つチャネルを作成します。
            // この容量に達するまでは、CreateUnboundedと同様に、TryWriteはtrueを返し、
            // WriteAsyncとWaitToWriteAsyncの両方が同期的に完了します。
            //
            // しかし、チャネルが一杯になると、TryWriteはfalseを返し、
            // WriteAsyncとWaitToWriteAsyncの両方が、
            // スペースが利用可能な場合、または別のプロデューサーがチャネルの完了を通知した場合にのみ
            // 非同期的に完了するようになります。
            //
            // www.DeepL.com/Translator（無料版）で翻訳しました。
            var ch4 = Channel.CreateBounded<int>(1);
            await Task.WhenAll(
                Task.Run(async () =>
                {
                    var outCh = ch4.Writer;
                    for (var i = 0; i < itemCount; i++)
                    {
                        while (await outCh.WaitToWriteAsync().ConfigureAwait(false))
                        {
                            Output.WriteLine($"[ch4] ch <- {i}");
                            if (outCh.TryWrite(i))
                            {
                                break;
                            }
                        }
                    }

                    outCh.Complete();
                }),
                Task.Run(async () =>
                {
                    var inCh = ch4.Reader;
                    await foreach (var item in inCh.ReadAllAsync())
                    {
                        Output.WriteLine($"[ch4] {item} <-ch ");
                    }
                })
            );
            Output.WriteLine("----------------------------------");

            // CreateUnbounded と CreateBounded の両方とも、
            // ChannelOptions 派生の型を受け入れるオーバーロードを持っています。
            //
            // SingleWriter - 一度に一つのProducerのみがWriterにアクセス可能にする
            // SingleReader - 一度に一つのConsumerのみがReaderにアクセス可能にする
            // AllowSynchronousContinuations - チャンネル読み/書き/待ち操作を非同期で行った場合の継続タスクを同期的に実行するかどうか
            //
            // AllowSynchronousContinuationsはある意味では、
            // プロデューサーが一時的にコンシューマーになることを可能にします。
            // チャンネルにデータがなく、消費者がやってきてReadAsyncを呼び出したとしましょう。
            // ReadAsyncから返されたタスクを待つことで、そのコンシューマはチャネルにデータが書き込まれたときに
            // 呼び出されるコールバックを効果的にフックしていることになります。
            //
            // デフォルトでは、そのコールバックは非同期的に呼び出され、
            // プロデューサーがチャネルにデータを書き込んでからそのコールバックの呼び出しをキューに入れることで、
            // コンシューマが他のスレッドで処理されている間、プロデューサーは同時に楽しい道を進むことができます。
            // しかし、状況によっては、データを書き込んだプロデューサ自身がコールバックを処理できるようにした方が
            // パフォーマンス上有利な場合もあります。
            //
            // これはオーバーヘッドを大幅に削減できますが、環境をよく理解する必要があります。
            // 例えば、TryWriteを呼び出している間にロックを保持していて、
            // AllowSynchronousContinuationsをtrueに設定していた場合、
            // ロックを保持したままコールバックを呼び出すことになるかもしれません。
            //
            // Channel.CreateBounded<T>() にわたすオプションは、BoundedChannelOptions です。
            // バウンディングに特化した追加のオプションを指定します。
            // チャネルがサポートする最大容量に加えて、
            // チャネルが一杯になったときの書き込みの動作を示す BoundedChannelFullMode も公開します。
            //
            // public enum BoundedChannelFullMode
            // {
            //     Wait,        // 待つ
            //     DropNewest,  // キュー内の最新投入分を捨てる
            //     DropOldest,  // キュー内の最古投入分を捨てる
            //     DropWrite    // 今回投入分を捨てる
            // }
            //
            // デフォルトは Wait です。
            //
            // www.DeepL.com/Translator（無料版）で翻訳しました。
            var ch5 = Channel.CreateBounded<int>(new BoundedChannelOptions(1)
            {
                FullMode = BoundedChannelFullMode.DropWrite
            });

            var w = ch5.Writer;
            var r = ch5.Reader;

            for (var i = 0; i < 5; i++)
            {
                await w.WriteAsync(i);
                Output.WriteLine($"[ch5] ch <- {i}");
            }

            w.Complete();

            await foreach (var item in r.ReadAllAsync())
            {
                Output.WriteLine($"[ch5] {item} <-ch");
            }
        }

        class SuperBasicChannel<T>
        {
            private readonly ConcurrentQueue<T> _queue = new ConcurrentQueue<T>();
            private readonly SemaphoreSlim _semaphore = new SemaphoreSlim(0);

            public void Write(T value)
            {
                this._queue.Enqueue(value);
                this._semaphore.Release();
            }

            public async ValueTask<T> ReadAsync(CancellationToken token = default)
            {
                await this._semaphore.WaitAsync(token).ConfigureAwait(false);
                this._queue.TryDequeue(out var item);
                return item!;
            }
        }
    }
}