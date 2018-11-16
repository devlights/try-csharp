using System;
using TryCSharp.Common;

namespace TryCSharp.Tools.Cui
{
    /// <summary>
    /// CUIアプリでの入力を管理するクラスです。
    /// </summary>
    public class CuiInputManager : IInputManager
    {
        /// <summary>
        /// 1文字を読み込みます。
        /// </summary>
        /// <returns>読み込んだデータ</returns>
        /// <remarks>
        /// コンソールから文字を読み込んでいるので、戻り値の型はintになります。
        /// </remarks>
        public object Read() => Console.Read();

        /// <summary>
        /// 一行分のデータを読み込みます。
        /// </summary>
        /// <returns>一行分のデータ</returns>
        /// <remarks>
        /// コンソールから文字列を読み込んでいるので、戻り値の型はstringになります。
        /// </remarks>
        public object ReadLine() => Console.ReadLine();
    }
}