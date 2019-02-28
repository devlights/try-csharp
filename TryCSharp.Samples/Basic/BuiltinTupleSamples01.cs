using TryCSharp.Common;

namespace TryCSharp.Samples.Basic
{
    /// <summary>
    /// C# 7.0 の組み込みタプル型のサンプルです。
    /// </summary>
    [Sample]
    public class BuiltinTupleSamples01 : IExecutable
    {
        public void Execute()
        {
            //
            // C# 7.0 からサポートされた 組み込みタプル型のサンプル
            //   http://bit.ly/2XqlDwk
            //
            // Python のようにタプルを使えるようになった。
            //
            // 一つの変数で受けた場合、フィールドが自動的に生成されている
            var result1 = M1(2, 3);
            Output.WriteLine(result1);
            Output.WriteLine($"x:{result1.x}, y:{result1.y}");
            
            // 分解した状態でも受け取れる
            var (x, y) = M1(3, 4);
            Output.WriteLine($"x:{x}, y:{y}");
        }

        private static (int x, int y) M1(int a, int b)
        {
            return (a * 2, b * 2);
        }
    }
}