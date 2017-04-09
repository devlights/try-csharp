using System;
using System.Windows.Forms;
using TryCSharp.Common;

namespace TryCSharp.Samples.Basic
{
    /// <summary>
    ///     ラムダ(lambda)のサンプル（.NET Framework 3.5）
    /// </summary>
    [Sample]
    internal class LambdaSample : IExecutable
    {
        /// <summary>
        ///     処理を実行します。
        /// </summary>
        public void Execute()
        {
            MethodInvoker methodInvoker = () => { Output.WriteLine("SAMPLE_LAMBDA_METHOD."); };

            methodInvoker();

            Action action = () => { Output.WriteLine("SAMPLE_LAMBDA_METHOD_ACTION."); };

            action();

            Func<int, int, int> sum = (x, y) => x + y;

            Output.WriteLine(sum(10, 20));

            Func<int, int, int> sum2 = (x, y) => x + y;

            Output.WriteLine(sum2(10, 20));
        }
    }
}