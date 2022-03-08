using System;
using System.IO;

namespace TryCSharp.Common
{
    /// <summary>
    ///     出力を管理する静的クラスです。
    /// </summary>
    public static class Output
    {
        /// <summary>
        ///     出力管理オブジェクトを取得・設定します。
        /// </summary>
        public static IOutputManager? OutputManager { get; set; }

        /// <summary>
        ///     出力ストリーム
        /// </summary>
        public static Stream OutStream
        {
            get
            {
                if (OutputManager == null)
                {
                    throw new InvalidOperationException("No OutputManager was found. ");
                }
                return OutputManager.OutStream;
            }
        }

        /// <summary>
        ///     指定されたデータを出力します。（改行付与無し）
        /// </summary>
        /// <param name="data">データ</param>
        public static void Write(object data)
        {
            if (OutputManager == null)
            {
                throw new InvalidOperationException("No OutputManager was found. ");
            }
            OutputManager.Write(data);
        }

        /// <summary>
        ///     指定されたデータを出力します。（改行付与無し）
        /// </summary>
        /// <param name="format">フォーマット</param>
        /// <param name="args">フォーマット引数</param>
        public static void Write(string format, params object[] args)
        {
            if (OutputManager == null)
            {
                throw new InvalidOperationException("No OutputManager was found. ");
            }
            OutputManager.Write(string.Format(format, args));
        }

        /// <summary>
        ///     空行を出力します。
        /// </summary>
        public static void WriteLine()
        {
            WriteLine(string.Empty);
        }

        /// <summary>
        ///     指定されたデータを出力します。（改行付与有り）
        /// </summary>
        /// <param name="data">データ</param>
        public static void WriteLine(object data)
        {
            if (OutputManager == null)
            {
                throw new InvalidOperationException("No OutputManager was found. ");
            }
            OutputManager.WriteLine(data);
        }

        /// <summary>
        ///     指定されたデータを出力します。（改行付与有り）
        /// </summary>
        /// <param name="format">フォーマット</param>
        /// <param name="arg">フォーマット引数</param>
        public static void WriteLine(string format, params object?[] arg)
        {
            if (OutputManager == null)
            {
                throw new InvalidOperationException("No OutputManager was found. ");
            }

            OutputManager.WriteLine(string.Format(format, arg));
        }
    }
}