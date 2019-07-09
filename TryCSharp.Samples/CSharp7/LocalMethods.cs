using System;
using TryCSharp.Common;

namespace TryCSharp.Samples.CSharp7
{
    [Sample]
    public class LocalMethods : IExecutable
    {
        public void Execute()
        {
            // C# 7.0 にて ローカルメソッド が導入された
            // ローカルメソッド は、メソッドの中でローカルに利用できるメソッドを定義する機能
            // 元々 Action や Func を用いても同様のことが出来たが、それが基本機能となった

            int Sum(int x, int y) => x + y;
            
            Output.WriteLine(Sum(1, 2));
            Output.WriteLine(Sum(100, 1_000_00));
            
            // delegate として利用することも可能
            Func<int, int, int> f = Sum;
            Output.WriteLine(f(100, 1_000_00));
            
            // delegate として渡すことも可能
            ExecLocalMethods(f);
            ExecLocalMethods(Sum);
        }

        private void ExecLocalMethods(Func<int, int, int> f)
        {
            Output.WriteLine(f(9, 1));
        }
    }
}