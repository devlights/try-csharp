using System.Diagnostics.CodeAnalysis;
using TryCSharp.Common;

namespace TryCSharp.Samples.CSharp7
{
    [Sample]
    public class PatternVariableWithSwitchStatement : IExecutable
    {
        public void Execute()
        {
            // C# 7.0 から　Pattern Variables の概念が導入された
            // switch 文でもパターンが利用できるようになった
            this.SwitchWithPattern(100);
            this.SwitchWithPattern("hello");
            this.SwitchWithPattern(true);
            this.SwitchWithPattern(null);
        }

        [SuppressMessage("ReSharper", "RedundantBoolCompare")]
        [SuppressMessage("ReSharper", "RedundantStringInterpolation")]
        private void SwitchWithPattern(object x)
        {
            // 型で振り分け
            switch (x)
            {
                case int i:
                    Output.WriteLine($"x is int: {i}");
                    break;
                case string s:
                    Output.WriteLine($"x is string: {s}");
                    break;
                case bool b:
                    Output.WriteLine($"x is bool: {b}");
                    break;
                case null:
                    Output.WriteLine($"x is null");
                    break;
            }
            
            // case に when で条件を付けることが可能
            switch (x)
            {
                case int i when i == 10:
                    Output.WriteLine($"x is int, value is 10");
                    break;
                case int i when i == 100:
                    Output.WriteLine($"x is int, value is 100");
                    break;
                case string s when s == "hello":
                    Output.WriteLine($"x is string, value is hello");
                    break;
                case string s when s == "world":
                    Output.WriteLine($"x is string, value is world");
                    break;
                case bool b when b == true:
                    Output.WriteLine($"x is bool, value is true");
                    break;
                case bool b when b == false:
                    Output.WriteLine($"x is bool, value is false");
                    break;
            }
        }
    }
}