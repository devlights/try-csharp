using System;
using TryCSharp.Common;

namespace TryCSharp.Samples.Basic
{
    /// <summary>
    ///     バイト配列についてのサンプルです。
    /// </summary>
    [Sample]
    public class ByteArraySamples02 : IExecutable
    {
        public void Execute()
        {
            //
            // バイト列を16進数文字列へ
            //
            var buf = new byte[5];
            new Random().NextBytes(buf);

            Output.WriteLine(BitConverter.ToString(buf));
        }
    }
}