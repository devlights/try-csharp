using TryCSharp.Common;

// using static の利用
// これにより、Consoleクラスのstaticメソッドをクラス名修飾なしで利用出来る
// python の from xxx import xxx と同じような感じ。
using static System.Console;

namespace TryCSharp.Samples.CSharp6
{
    using static MyStaticClass;
            
    [Sample]
    public class UsingStaticSamples01 : IExecutable
    {
        public void Execute()
        {
            // Consoleクラスに対して、using static しているので
            // メソッド呼び出しにConsoleを付けなくても呼べる。
            WriteLine("hello world");
            
            // 当然、自前のクラスに対しても using static 出来る
            MyStaticMethod("hello world");
        }
    }

    public static class MyStaticClass
    {
        public static void MyStaticMethod(string message)
        {
            Output.WriteLine(message);
        }
    }
}