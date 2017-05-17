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
    public class ThreadingNamespaceSamples02 : IExecutable
    {
        public void Execute()
        {
            ////////////////////////////////////////////////////////////
            //
            // 無名データスロット.
            //    データスロットはどれかのスレッドで最初に作成したら
            //    全てのスレッドに対してスロットが割り当てられる。
            //
            var slot = Thread.AllocateDataSlot();

            var threads = new List<Thread>();
            for (var i = 0; i < 10; i++)
            {
                var thread = new Thread(DoAnonymousDataSlotProcess);
                thread.Name = $"Thread-{i}";
                thread.IsBackground = true;

                threads.Add(thread);

                thread.Start(slot);
            }

            threads.ForEach(thread => { thread.Join(); });

            Output.WriteLine(string.Empty);

            ////////////////////////////////////////////////////////////
            //
            // 名前付きデータスロット.
            //    名前がつけられる事以外は、無名のスロットと同じです。
            //    名前付きデータスロットは、最初にその名前が呼ばれた
            //    際に作成されます。
            //    FreeNamedDataSlotメソッドを呼ぶと現在のスロット設定
            //    が解放されます。
            //
            threads.Clear();
            for (var i = 0; i < 10; i++)
            {
                var thread = new Thread(DoNamedDataSlotProcess);
                thread.Name = $"Thread-{i}";
                thread.IsBackground = true;

                threads.Add(thread);

                thread.Start(slot);
            }

            threads.ForEach(thread => { thread.Join(); });

            //
            // 利用したデータスロットを解放.
            //
            Thread.FreeNamedDataSlot("SampleSlot");
        }

        private void DoAnonymousDataSlotProcess(object stateObj)
        {
            var slot = stateObj as LocalDataStoreSlot;

            //
            // スロットにデータを設定
            //
            Thread.SetData(slot, $"ManagedThreadId={Thread.CurrentThread.ManagedThreadId}");

            //
            // 設定した内容を確認.
            //
            Output.WriteLine("[BEFORE] Thread:{0}   DataSlot:{1}", Thread.CurrentThread.Name, Thread.GetData(slot));

            //
            // 別のスレッドに処理を行って貰う為に一旦Sleepする。
            //
            Thread.Sleep(TimeSpan.FromSeconds(1));

            //
            // 再度確認.
            //
            Output.WriteLine("[AFTER ] Thread:{0}   DataSlot:{1}", Thread.CurrentThread.Name, Thread.GetData(slot));
        }

        private void DoNamedDataSlotProcess(object stateObj)
        {
            //
            // スロットにデータを設定
            //
            Thread.SetData(Thread.GetNamedDataSlot("SampleSlot"), $"ManagedThreadId={Thread.CurrentThread.ManagedThreadId}");

            //
            // 設定した内容を確認.
            //
            Output.WriteLine("[BEFORE] Thread:{0}   DataSlot:{1}", Thread.CurrentThread.Name, Thread.GetData(Thread.GetNamedDataSlot("SampleSlot")));

            //
            // 別のスレッドに処理を行って貰う為に一旦Sleepする。
            //
            Thread.Sleep(TimeSpan.FromSeconds(1));

            //
            // 再度確認.
            //
            Output.WriteLine("[AFTER ] Thread:{0}   DataSlot:{1}", Thread.CurrentThread.Name, Thread.GetData(Thread.GetNamedDataSlot("SampleSlot")));
        }
    }
}