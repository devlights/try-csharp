using System;
using TryCSharp.Common;

namespace TryCSharp.Samples.Basic
{
    /// <summary>
    ///     バイト配列についてのサンプルです。
    /// </summary>
    [Sample]
    public class ByteArraySamples05 : IExecutable
    {
        public void Execute()
        {
            //
            // バイト列を数値に
            //
            var buf = new byte[4];
            new Random().NextBytes(buf);

            var i = BitConverter.ToInt32(buf, 0);

            Output.WriteLine(i);
        }
    }
}