using System.Net.NetworkInformation;
using System.Threading;
using TryCSharp.Common;

// ReSharper disable PossibleNullReferenceException

namespace TryCSharp.Samples.NetWorking
{
    /// <summary>
    ///     Pingクラスに関するサンプルです。
    /// </summary>
    [Sample]
    public class PingSamples01 : IExecutable
    {
        public void Execute()
        {
            //
            // 同期での送信.
            //
            var hostName = "localhost";
            var timeOut = 3000;

            var p = new Ping();
            var r = p.Send(hostName, timeOut);

            if (r.Status == IPStatus.Success)
            {
                Output.WriteLine("Ping.Send() Success.");
            }
            else
            {
                Output.WriteLine("Ping.Send() Failed.");
            }

            //
            // 非同期での送信.
            //
            hostName = "www.google.com";

            p.PingCompleted += (s, e) =>
            {
                if (e.Cancelled)
                {
                    Output.WriteLine("Cancelled..");
                    return;
                }

                if (e.Error != null)
                {
                    Output.WriteLine(e.Error.ToString());
                    return;
                }

                if (e.Reply == null)
                {
                    return;
                }
                
                if (e.Reply.Status != IPStatus.Success)
                {
                    Output.WriteLine("Ping.SendAsync() Failed");
                    return;
                }

                Output.WriteLine("Ping.SendAsync() Success.");
            };

            p.SendAsync(hostName, timeOut, null);
            Thread.Sleep(3000);
        }
    }
}