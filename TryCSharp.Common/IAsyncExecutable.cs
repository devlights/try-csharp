using System.Threading.Tasks;

namespace TryCSharp.Common
{
    /// <summary>
    /// 非同期実行できることを示すインターフェースです。
    /// </summary>
    public interface IAsyncExecutable
    {
        /// <summary>
        /// 処理を実行します。
        /// </summary>
        /// <returns>タスク</returns>
        Task Execute();
    }
}