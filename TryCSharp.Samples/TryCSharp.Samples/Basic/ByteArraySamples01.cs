using System;
using System.Linq;
using TryCSharp.Common;

namespace TryCSharp.Samples.Basic
{
    /// <summary>
    ///     バイト配列についてのサンプルです。
    /// </summary>
    [Sample]
    public class ByteArraySamples01 : IExecutable
    {
        public void Execute()
        {
            //
            // バイト配列を2進数表示.
            //
            var buf = new byte[4];
            buf[0] = 0;
            buf[1] = 0;
            buf[2] = 0;
            buf[3] = 98;

            Output.WriteLine(string.Join("", ToBinaryString(buf)));
        }

        /// <summary>
        ///     ２進数文字列を取得します。
        /// </summary>
        /// <param name="buf">対象バッファ</param>
        /// <returns>２進数文字列</returns>
        private static string[] ToBinaryString(byte[] buf)
        {
            if (buf == null)
            {
                throw new ArgumentNullException(nameof(buf));
            }

            var query = buf.Select(b => Convert.ToString(b, 2).PadLeft(8, '0'));

            return query.ToArray();
        }
    }
}