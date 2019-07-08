using System.Diagnostics.CodeAnalysis;
using TryCSharp.Common;

namespace TryCSharp.Samples.CSharp7
{
    [Sample]
    public class PatternVariableWithIsOperator : IExecutable
    {
        [SuppressMessage("ReSharper", "ConditionIsAlwaysTrueOrFalse")]
        public void Execute()
        {
            // C# 7.0 から　Pattern Variables の概念が導入された
            // is 演算子とともに変数を宣言することが可能になった
            object x = "hello world";
            
            // C# 7.0 以前
            // ReSharper disable once MergeCastWithTypeCheck
            if (x is string)
            {
                // ReSharper disable once TryCastAlwaysSucceeds
                var cs6 = x as string;
                Output.WriteLine($"C# 6.0 val:{cs6}, length:{cs6.Length}");
            }
            
            // C# 7.0
            if (x is string cs7)
            {
                Output.WriteLine($"C# 7.0 val:{cs7}, length:{cs7.Length}");
            }
        }
    }
}