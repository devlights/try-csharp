using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;
using TryCSharp.Common;
// ReSharper disable MethodSupportsCancellation

namespace TryCSharp.Samples.Async.Channels
{
    /// <summary>
    /// <see cref="ChannelBasicReadWrite"/> の処理内容をレキシカル拘束の方法で実現するサンプルです。
    /// </summary>
    [Sample]
    public class ChannelBasicReadWriteLexicalBind : IAsyncExecutable
    {
        public async Task Execute()
        {
            // ------------------------------------------------
            // 以下の処理内容は ChannelBasicReadWrite と同じ
            // ChannelBasicReadWrite では生成したチャネルを
            // アドホックな拘束を使って使用していたので
            // 今度はレキシカルな拘束を使って処理してみる。
            // ------------------------------------------------
            const int numConsumers = 3;

            var cts = new CancellationTokenSource();
            cts.CancelAfter(TimeSpan.FromMilliseconds(200));

            var (inCh, producer) = RunProducer(cts.Token, TimeSpan.FromMilliseconds(10));
            var consumers = this.RunConsumer(numConsumers, inCh);

            await Task.WhenAll(consumers.Concat(new[] {producer}));
            Console.WriteLine("...DONE...");
        }

        (ChannelReader<int>, Task) RunProducer(CancellationToken ct, TimeSpan interval)
        {
            var ch = Channel.CreateBounded<int>(10);
            
            var t = Task.Run(async () =>
            {
                var dataCh = ch.Writer;

                try
                {
                    var count = 0;
                    
                    // ct.Registerにてチャネルのクローズを設定しているので、ここで ct 指定していない
                    while (await dataCh.WaitToWriteAsync())
                    {
                        if (!dataCh.TryWrite(count))
                        {
                            continue;
                        }

                        count++;

                        if (ct.IsCancellationRequested)
                        {
                            await Task.Delay(interval);                            
                        }
                    }
                }
                finally
                {
                    Console.WriteLine("...DONE PRODUCER...");                    
                }

            });
            
            ct.Register(() => ch.Writer.Complete());
            return (ch.Reader, t);
        }

        private Task[] RunConsumer(int numConsumers, ChannelReader<int> inCh)
        {
            var tasks = new List<Task>();
            for (var i = 0; i < numConsumers; i++)
            {
                var index = i;
                tasks.Add(Task.Run(async () =>
                {
                    try
                    {
                        // Write側でCompleteを呼べば、Read側は false となるのでここで ct は指定していない
                        while (await inCh.WaitToReadAsync())
                        {
                            while (inCh.TryRead(out var v))
                            {
                                Console.WriteLine($"[consumer{index + 1}] {v}");
                            }
                        }
                    }
                    finally
                    {
                        Console.WriteLine($"...DONE CONSUMER[{index + 1}]...");                        
                    }
                }));
            }

            return tasks.ToArray();
        }
    }
}