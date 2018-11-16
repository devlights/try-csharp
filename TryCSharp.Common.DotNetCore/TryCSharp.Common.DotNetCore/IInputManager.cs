namespace TryCSharp.Common
{
    /// <summary>
    /// 入力のインターフェースが定義されています。
    /// </summary>
    public interface IInputManager
    {
        /// <summary>
        /// １データを読み取ります。
        /// </summary>
        /// <returns>読み込んだデータ</returns>
        object Read();

        /// <summary>
        /// 一行分のデータを読み込みます。
        /// </summary>
        /// <returns>一行分のデータ</returns>
        object ReadLine();
    }
}