using System;
using TryCSharp.Common;

namespace TryCSharp.Samples.Basic
{
    /// <summary>
    ///     バイト配列についてのサンプルです。
    /// </summary>
    [Sample]
    public class ByteArraySamples08 : IExecutable
    {
        public void Execute()
        {
            //
            // 数値をいろいろな基数に変換.
            //
            var i = 123;

            Output.WriteLine(Convert.ToString(i, 16));
            Output.WriteLine(Convert.ToString(i, 8));
            Output.WriteLine(Convert.ToString(i, 2));
        }
    }
}