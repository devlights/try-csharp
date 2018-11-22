using System;
using TryCSharp.Common;

namespace TryCSharp.Samples.Basic
{
    /// <summary>
    ///     デリゲートのサンプル（.NET Framework 1.1）
    /// </summary>
    [Sample]
    internal class DelegateSample : IExecutable
    {
        /// <summary>
        ///     処理を実行します。
        /// </summary>
        public void Execute()
        {
            Action methodInvoker = DelegateMethod;
            methodInvoker();
        }

        /// <summary>
        ///     デリゲートメソッド.
        /// </summary>
        private void DelegateMethod()
        {
            Output.WriteLine("SAMPLE_DELEGATE_METHOD.");
        }
    }
}