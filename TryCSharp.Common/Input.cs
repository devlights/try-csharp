using System;

namespace TryCSharp.Common
{
    /// <summary>
    ///     入力を管理する静的クラスです。
    /// </summary>
    public static class Input
    {
        /// <summary>
        ///     入力管理オブジェクトを取得・設定します。
        /// </summary>
        public static IInputManager? InputManager { get; set; }

        /// <summary>
        ///     １データを読み込みます。
        /// </summary>
        /// <returns>読み込まれたデータ</returns>
        public static object Read()
        {
            if (InputManager == null)
            {
                throw new InvalidOperationException("No InputManager was found.");
            }
            return InputManager.Read();
        }

        /// <summary>
        ///     一行分のデータを読み込みます。
        /// </summary>
        /// <returns>一行分のデータ</returns>
        public static object ReadLine()
        {
            if (InputManager == null)
            {
                throw new InvalidOperationException("No InputManager was found.");
            }
            return InputManager.ReadLine();
        }
    }
}