using TryCSharp.Common;

namespace TryCSharp.Samples.Basic
{
    /// <summary>
    ///     匿名デリゲート(anonymous delegete)のサンプル（.NET Framework 2.0）
    /// </summary>
    [Sample]
    internal class AnonymousDelegateSample : IExecutable
    {
        /// <summary>
        ///     処理を実行します。
        /// </summary>
        public void Execute()
        {
            //
            // 匿名メソッドを構築して実行.
            //
            SampleDelegate d = delegate { Output.WriteLine("SAMPLE_ANONYMOUS_DELEGATE."); };

            d.Invoke();
        }

        /// <summary>
        ///     本サンプルで利用するデリゲートの定義
        /// </summary>
        private delegate void SampleDelegate();
    }
}