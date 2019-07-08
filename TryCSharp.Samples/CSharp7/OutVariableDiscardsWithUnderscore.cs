using System.Diagnostics.CodeAnalysis;
using TryCSharp.Common;

namespace TryCSharp.Samples.CSharp7
{
    [Sample]
    public class OutVariableDiscardsWithUnderscore : IExecutable
    {
        public void Execute()
        {
            // C# 7.0 から out 変数の扱いが楽になった
            // 不要な out パラメータがある場合、 アンダースコアを指定して破棄することができる
            this.OutValMethod(out _, out _, out var z);
            Output.WriteLine($"z: {z}");
        }

        [SuppressMessage("ReSharper", "UnusedParameter.Local")]
        private void OutValMethod(out int x, out int y, out int z)
        {
            x = 100;
            y = 1_000;
            z = 1_000_000;
        }
    }
}