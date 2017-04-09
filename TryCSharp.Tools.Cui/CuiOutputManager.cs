using System;
using System.IO;
using TryCSharp.Common;

namespace TryCSharp.Tools.Cui
{
    /// <summary>
    /// CUIアプリでの出力を管理するクラスです。
    /// </summary>
    public class CuiOutputManager : IOutputManager
    {
        /// <summary>
        /// 指定されたデータを出力します。（改行付与無し）
        /// </summary>
        /// <param name="data">データ</param>
        public void Write(object data)
        {
            Console.Write(data);
        }

        /// <summary>
        /// 指定されたデータを出力します。（改行付与有り）
        /// </summary>
        /// <param name="data">データ</param>
        public void WriteLine(object data)
        {
            Console.WriteLine(data);
        }

        /// <summary>
        /// 出力ストリーム
        /// </summary>
        public Stream OutStream => Console.OpenStandardOutput();
    }
}