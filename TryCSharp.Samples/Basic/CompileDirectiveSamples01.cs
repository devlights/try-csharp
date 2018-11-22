using TryCSharp.Common;

namespace TryCSharp.Samples.Basic
{
    /// <summary>
    ///     コンパイルディレクティブのサンプル1です。
    /// </summary>
    [Sample]
    public class CompileDirectiveSamples01 : IExecutable
    {
        public void Execute()
        {
            Output.WriteLine("001:HELLO C#");

#if(DEBUG)
            Output.WriteLine("002:HELLO C# (DEBUG)");
#else
        Output.WriteLine("003:HELLO C# (ELSE)");
#endif
        }
    }
}