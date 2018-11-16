namespace TryCSharp.Common
{
    /// <summary>
    /// 何かを実行するインターフェースを持っています。
    /// </summary>
    public interface IExecutor
    {
        /// <summary>
        /// 実行します。
        /// </summary>
        /// <param name="target">実行可能なもの</param>
        void Execute(IExecutable target);

        /// <summary>
        /// 非同期実行します。
        /// </summary>
        /// <param name="target">実行可能なもの</param>
        void Execute(IAsyncExecutable target);
    }
}