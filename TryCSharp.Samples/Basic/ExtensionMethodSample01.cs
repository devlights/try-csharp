using System.Diagnostics.CodeAnalysis;
using TryCSharp.Common;

namespace TryCSharp.Samples.Basic
{
    /// <summary>
    ///     拡張メソッドのサンプル1です。
    /// </summary>
    [Sample]
    public class ExtensionMethodSample01 : IExecutable
    {
        [SuppressMessage("ReSharper", "ExpressionIsAlwaysNull")]
        public void Execute()
        {
            string s = null;
            s.PrintMyName();
        }
    }

    // ReSharper disable once InconsistentNaming
    internal static class ExtensionMethodSample01_ExtClass
    {
        public static void PrintMyName(this string self)
        {
            Output.WriteLine(self == null);
            Output.WriteLine("GSF-ZERO1.");
        }
    }

}