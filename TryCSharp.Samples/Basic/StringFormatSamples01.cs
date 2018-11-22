using TryCSharp.Common;

namespace TryCSharp.Samples.Basic
{
    /// <summary>
    ///     文字列の書式設定に関してのサンプルです。
    /// </summary>
    [Sample]
    internal class StringFormatSamples01 : IExecutable
    {
        public void Execute()
        {
            //
            // 書式設定は、以下のようにして設定する.
            //   {0,-20:C}
            // 最初の0はインデックスを表す。必須項目。
            //
            // 桁数を指定する場合は、カンマを付与し桁数を指定する。
            // 桁数の値が負の値の場合は、左寄せ。
            // 桁数の値が正の値の場合は、右寄せとなる。
            // 桁数の指定はオプション。
            //
            // フォーマットを指定する場合は、コロンを付与しフォーマットのタイプを指定する。
            // Cは通貨を表す。
            // フォーマットの指定はオプション。
            //
            // フォーマットの種類などについては
            // http://msdn.microsoft.com/ja-jp/library/txafckwd(v=VS.100).aspx
            // を参照。
            //
            var format = "'{0,20:C}'";
            Output.WriteLine(format, 25000);
        }
    }
}