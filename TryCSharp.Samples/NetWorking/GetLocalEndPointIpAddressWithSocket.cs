using System.Net;
using System.Net.Sockets;
using TryCSharp.Common;

namespace TryCSharp.Samples.NetWorking
{
    /// <summary>
    ///     UDPソケットを利用して自身のIPアドレスを取得するサンプルです。
    /// </summary>
    [Sample]
    public class GetLocalEndPointIpAddressWithSocket : IExecutable
    {
        public void Execute()
        {
            //
            // UDPはコネクションレスプロトコルであるので、Connectを呼び出したタイミングでは
            // 実際にはリモートエンドポイントへの接続は確率されていない。
            //
            // Connectの呼び出しにより、実際の接続は行われていないが
            // この操作によってOSは適切なネットワークインターフェースとローカルIPアドレスを選択する。
            // 選択されるIPアドレスは、指定したリモートエンドポイントにルートが存在するネットワークインターフェースに関連付けられたものとなる。
            //
            // なので、このコードは指定したリモートIPアドレスに対して最も適切なローカルIPアドレスを選択するOSの動作を利用して
            // そのIPアドレスを取得いる。したがって、複数のIPアドレスが存在する場合でも
            // 特定のリモートエンドポイントに対して最も適切と判断されたIPアドレスが返される。
            //
            IPEndPoint? ep;
            using (var sock = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.IP))
            {
                sock.Connect("8.8.8.8", 65530);
                ep = sock.LocalEndPoint as IPEndPoint;
            }

            if (ep != null)
            {
                Output.WriteLine($"[Address] {ep.Address.ToString()}");
            }
        }
    }
}