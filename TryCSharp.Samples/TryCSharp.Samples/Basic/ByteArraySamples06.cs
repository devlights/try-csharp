using System;
using System.Text;
using TryCSharp.Common;

namespace TryCSharp.Samples.Basic
{
    /// <summary>
    ///     バイト配列についてのサンプルです。
    /// </summary>
    [Sample]
    public class ByteArraySamples06 : IExecutable
    {
        public void Execute()
        {
            //
            // 文字列をバイト列へ
            //
            var s = "gsf_zero1";
            var buf = Encoding.ASCII.GetBytes(s);

            Output.WriteLine(BitConverter.ToString(buf));
        }
    }
}