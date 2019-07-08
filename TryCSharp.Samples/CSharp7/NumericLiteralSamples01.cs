using TryCSharp.Common;

namespace TryCSharp.Samples.CSharp7
{
    /// <summary>
    /// C# 7.0 の新機能についてのサンプルです。
    /// </summary>
    /// <remarks>
    /// Numeric Literal について
    /// </remarks>
    [Sample]
    public class NumericLiteralSamples01 : IExecutable
    {
        public void Execute()
        {
            // C# 7.0 から数値の表現に「アンダースコア」を含めることが可能となった
            // Pythonと同様な記載ができる
            var million = 1_000_000;
            
            // バイナリ表記も同様
            var b = 0b1010_1011_1100_1101_1110_1111;
            
            Output.WriteLine($"{million}, {b}");
        }
    }
}