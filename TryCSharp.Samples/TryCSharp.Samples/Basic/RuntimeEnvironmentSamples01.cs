using System.Runtime.InteropServices;
using TryCSharp.Common;

namespace TryCSharp.Samples.Basic
{
    /// <summary>
    ///     RuntimeEnvironmentクラスについてのサンプルです。
    /// </summary>
    [Sample]
    public class RuntimeEnvironmentSamples01 : IExecutable
    {
        public void Execute()
        {
            //
            // System.Runtime.InteropServices.RuntimeEnvironmentクラスを利用する事で
            // .NETのランタイムパスなどを取得することができる。
            //
            Output.WriteLine("Runtime PATH:{0}", RuntimeEnvironment.GetRuntimeDirectory());
            Output.WriteLine("System Version:{0}", RuntimeEnvironment.GetSystemVersion());
        }
    }
}