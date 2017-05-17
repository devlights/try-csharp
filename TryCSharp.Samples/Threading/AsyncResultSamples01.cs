using System;
using System.Runtime.Remoting.Messaging;
using System.Threading;
using TryCSharp.Common;

namespace TryCSharp.Samples.Threading
{
    /// <summary>
    ///     非同期処理 (IAsyncResult) のサンプル１です。
    /// </summary>
    [Sample]
    public class AsyncResultSamples01 : IExecutable
    {
        private readonly AutoResetEvent _are = new AutoResetEvent(false);

        public void Execute()
        {
            Func<DateTime, string> func = CallerMethod;

            var result = func.BeginInvoke(DateTime.Now, CallbackMethod, _are);
            _are.WaitOne();
            func.EndInvoke(result);
        }

        private string CallerMethod(DateTime d)
        {
            return d.ToString("yyyy/MM/dd HH:mm:ss");
        }

        private void CallbackMethod(IAsyncResult ar)
        {
            var result = ar as AsyncResult;
            var caller = result.AsyncDelegate as Func<DateTime, string>;
            var handle = result.AsyncState as EventWaitHandle;

            if (!result.EndInvokeCalled)
            {
                Output.WriteLine(caller.EndInvoke(result));
                handle.Set();
            }
        }
    }
}