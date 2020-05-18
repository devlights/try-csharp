using System;
using System.Threading;
using System.Threading.Tasks;
using TryCSharp.Common;

namespace TryCSharp.Samples.CSharp6
{
    /// <summary>
    ///     C# 6 新機能についてのサンプルです。
    /// </summary>
    /// <remarks>
    ///     Await in Catch and Finally blocks (catch と finally ブロックの中での await) について
    ///     https://docs.microsoft.com/ja-jp/dotnet/csharp/whats-new/csharp-6#await-in-catch-and-finally-blocks
    ///     https://ufcpp.net/study/csharp/ap_ver6.html#await-in-catch
    /// </remarks>
    [Sample]
    public class AwaitInCatchAndFinally : IAsyncExecutable
    {
        public async Task Execute()
        {
            // ------------------------------------------------------------------
            // C# 5 では、await を catch, finally の中で使うことが出来なかった。
            // C# 6 から、利用できるようになっている。
            // ------------------------------------------------------------------
            var mainThreadId = Thread.CurrentThread.ManagedThreadId;
            Console.WriteLine($"[main] threadId: {mainThreadId}");

            await this.Log("Start");
            try
            {
                await this.Log("Processing...");
            }
            catch (Exception ex)
            {
                // C# 5 だとこれはコンパイルエラーになる
                await this.Log(ex.Message);
            }
            finally
            {
                // C# 5 だとこれはコンパイルエラーになる
                await this.Log("End");
            }
        }

        private async Task Log(string message)
        {
            await Task.Yield();

            var threadId = Thread.CurrentThread.ManagedThreadId;
            Console.WriteLine($"[Log ] threadId: {threadId}\tmessage: {message}");
        }
    }
}