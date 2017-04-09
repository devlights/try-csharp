using System;
using TryCSharp.Common;

namespace TryCSharp.Samples.Basic
{
    /// <summary>
    ///     バイト配列についてのサンプルです。
    /// </summary>
    [Sample]
    public class ByteArraySamples09 : IExecutable
    {
        public void Execute()
        {
            //
            // 利用しているアーキテクチャのエンディアンを判定.
            //
            Output.WriteLine(BitConverter.IsLittleEndian);
        }
    }
}