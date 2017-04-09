using TryCSharp.Common;

namespace TryCSharp.Samples.Basic
{
    /// <summary>
    ///     数値フォーマットのサンプルです。
    /// </summary>
    [Sample]
    public class NumberFormatSamples02 : IExecutable
    {
        public void Execute()
        {
            var i = 123456;
            Output.WriteLine("{0:N0}", i);
        }
    }
}