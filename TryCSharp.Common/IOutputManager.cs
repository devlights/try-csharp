using System.IO;

namespace TryCSharp.Common
{
    /// <summary>
    ///     出力のインターフェースが定義されています。
    /// </summary>
    public interface IOutputManager
    {
        /// <summary>
        ///     出力ストリーム
        /// </summary>
        Stream OutStream { get; }

        /// <summary>
        ///     指定されたデータを出力します。(改行付与無し）
        /// </summary>
        /// <param name="data">データ</param>
        void Write(object data);

        /// <summary>
        ///     指定されたデータを出力します。（改行付与有り）
        /// </summary>
        /// <param name="data">データ</param>
        void WriteLine(object data);
    }
}