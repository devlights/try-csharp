#define LOGGING_PRODUCER
#undef LOGGING_PRODUCER

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Channels;
using System.Threading.Tasks;
using TryCSharp.Common;

namespace TryCSharp.Samples.Async.Channels
{
    /// <summary>
    /// System.Threading.Channels の基本的な使い方についてのサンプルです。
    /// </summary>
    [Sample]
    public class ChannelBasicReadWrite : IAsyncExecutable
    {
        public async Task Execute()
        {
            // ---------------------------------------------------
            // System.Threading.Channels
            // --------------------------
            // System.Threading.Channels は、標準ライブラリとしては
            // 搭載されていないライブラリ。インストールにはNuGetを利用する。
            //
            // 2020-04-19 時点での最新安定版は v4.7.0
            // prerelease として、v5.0.0 が出ている。
            //
            // System.Threading.Channels を利用すると
            // Goの チャネル のような使い勝手で非同期データ処理が行える。
            // 標準ライブラリ側にも BlockingCollection<T> が存在するが
            // それよりも FIFO に特化しているなど処理がやりやすい。
            //
            // Goのチャネルと同様に、一つのチャネルから読み取り用と書き込み用の
            // チャネルを取得できる。
            //
            // [参考]
            // - https://devblogs.microsoft.com/dotnet/an-introduction-to-system-threading-channels/
            // - https://docs.microsoft.com/en-us/dotnet/api/system.threading.channels?view=netcore-3.1
            // - https://deniskyashif.com/2019/12/08/csharp-channels-part-1/
            // - https://deniskyashif.com/2019/12/11/csharp-channels-part-2/
            // - https://deniskyashif.com/2020/01/07/csharp-channels-part-3/
            // ---------------------------------------------------

            //
            // (1) チャネルを生成
            //     チャネルには Bounded（容量制限あり）とUnbounded（容量制限なし）の２つがある
            //
            // 今回サンプルということで、以下の構成で使ってみる
            //   - Producer/Consumerパターンでデータをやり取りするチャネル
            //   - 出力用のチャネル
            // ProducerとConsumerは１：３という構成とする。
            //
            var dataCh = Channel.CreateBounded<int>(10);
            var logCh = Channel.CreateUnbounded<string>();

            //
            // (2) Consumer を生成
            //
            const int numConsumers = 3;
            var consumers = this.RunConsumers(numConsumers, dataCh.Reader, logCh.Writer);

            //
            // (3) Producer を生成
            //
            var interval = TimeSpan.FromMilliseconds(10);
            var producer = this.RunProducer(interval, dataCh.Writer, logCh.Writer);

            //
            // (4) 出力用タスク生成
            //
            var printer = this.RunPrinter(logCh.Reader);

            //
            // (5) チャネルをクローズさせる役割のタスク生成 (Goでいうdoneチャネル)
            //
            var done = Task.Run(async () =>
            {
                await Task.Yield();
                await Task.Delay(TimeSpan.FromMilliseconds(200));

                // チャネルのクローズ
                //   - Writer.Complete() を呼ぶことでチャネルにもう書き込まないと伝える
                //   - チャネルを利用している側では WaitToXXXAsync() が false を返すのでループを抜けることになる
                dataCh.Writer.Complete();
                logCh.Writer.TryWrite("dataCh closed");

                logCh.Writer.Complete();
                Console.WriteLine("logCh closed");
            });

            //
            // (6) 待ち合わせ
            //
            await done;
            await Task.WhenAll(consumers.Concat(new[] {producer, printer}));

            Console.WriteLine("...DONE...");
        }

        private IEnumerable<Task> RunConsumers(int numConsumers, ChannelReader<int> inCh, ChannelWriter<string> logCh)
        {
            var tasks = new List<Task>();
            for (var i = 0; i < numConsumers; i++)
            {
                var index = i;
                tasks.Add(Task.Run(async () =>
                {
                    // チャネル からデータを読み取る際の基本形
                    //   - WaitToReadAsync() で読み取りできるか確認
                    //   - TryRead() で値を読み取り
                    while (await inCh.WaitToReadAsync())
                    {
                        while (inCh.TryRead(out var v))
                        {
                            if (await logCh.WaitToWriteAsync())
                            {
                                logCh.TryWrite($"[consumer{index + 1}] {v}");
                            }
                        }
                    }
                }));
            }

            return tasks.ToArray();
        }

        private Task RunProducer(TimeSpan interval, ChannelWriter<int> outCh, ChannelWriter<string> logCh)
        {
            // チャネル にデータを書き込む際の基本形
            //   - WaitToWriteAsync() で書き込みできるか確認
            //   - TryWrite() で値を書き込み
            return Task.Run(async () =>
            {
                var count = 0;
                while (await outCh.WaitToWriteAsync())
                {
                    if (!outCh.TryWrite(count))
                    {
                        continue;
                    }
                    
                    if (await logCh.WaitToWriteAsync())
                    {
#if LOGGING_PRODUCER
                        logCh.TryWrite($"[producer] {count}");
#endif
                    }
                    
                    count++;
                    await Task.Delay(interval);
                }

                Console.WriteLine($"[producer] total {count - 1} items");
            });
        }

        private Task RunPrinter(ChannelReader<string> inCh)
        {
            return Task.Run(async () =>
            {
                while (await inCh.WaitToReadAsync())
                {
                    while (inCh.TryRead(out var v))
                    {
                        Console.WriteLine(v);
                    }
                }
            });
        }
    }
}