using TryCSharp.Common;

namespace TryCSharp.Samples.CSharp7
{
    /// <summary>
    /// C# 7.0 の新機能についてのサンプルです。
    /// </summary>
    /// <remarks>
    /// Out variables on the fly について
    /// </remarks>
    [Sample]
    public class OutVariablesOnTheFly : IExecutable
    {
        public void Execute()
        {
            // C# 7.0 にて out 変数の取扱いが楽になった
            // その場で宣言して利用できるようになった
            
            // C# 7.0 まで
            // ReSharper disable once InlineOutVariableDeclaration
            int beforeCs7;
            if (int.TryParse("100", out beforeCs7))
            {
                Output.WriteLine($"Before C# 7.0: {beforeCs7}");
            }
            
            // C# 7.0 から
            if (int.TryParse("100", out var afterCs7))
            {
                Output.WriteLine($"After C# 7.0: {afterCs7}");
            }
        }
    }
}