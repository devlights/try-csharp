namespace TryCSharp.Common
{
    /// <summary>
    /// 自身の状態を保護するためのメソッドが定義されています。
    /// </summary>
    /// <remarks>
    /// 基本的に、本インターフェースは外部に公開するものではありません。
    /// 実装する際は、「インターフェースの明示的実装」を利用してください。
    /// </remarks>
    public interface IHasDefence
    {
        /// <summary>
        /// 自身の状態を確認し、問題がある場合は例外を発生させます。
        /// </summary>
        /// <remarks>
        /// 実装例として、インスタンスAは内部に名前フィールドを持っているとします。
        /// 本メソッドで、名前フィールドがnullまたは空文字か否かをチェックし問題がある場合は
        /// 例外を発生させます。
        /// </remarks>
        void Defence();
    }
}