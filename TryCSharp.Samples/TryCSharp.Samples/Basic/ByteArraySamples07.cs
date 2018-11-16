using System.Text;
using TryCSharp.Common;

namespace TryCSharp.Samples.Basic
{
    /// <summary>
    ///     バイト配列についてのサンプルです。
    /// </summary>
    [Sample]
    public class ByteArraySamples07 : IExecutable
    {
        public void Execute()
        {
            //
            // バイト列を文字列へ.
            //
            var s = "gsf_zero1";
            var buf = Encoding.ASCII.GetBytes(s);

            Output.WriteLine(Encoding.ASCII.GetString(buf));
        }
    }
}