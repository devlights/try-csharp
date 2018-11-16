using System.Text;
using TryCSharp.Common;

namespace TryCSharp.Samples.Basic
{
    /// <summary>
    ///     全角チェックと半角チェックのサンプルです。
    /// </summary>
    /// <remarks>
    ///     単純な全角チェックと半角チェックを定義しています。
    /// </remarks>
    [Sample]
    public class ZenkakuHankakuCheckSample01 : IExecutable
    {
        public void Execute()
        {
            var zenkakuOnlyStrings = "あいうえお";
            var hankakuOnlyStrings = "ｱｲｳｴｵ";
            var zenkakuAndHankakuStrings = "あいうえおｱｲｳｴｵ";

            Output.WriteLine("IsZenkaku:zenkakuOnly:{0}", IsZenkaku(zenkakuOnlyStrings));
            Output.WriteLine("IsZenkaku:hankakuOnlyStrings:{0}", IsZenkaku(hankakuOnlyStrings));
            Output.WriteLine("IsZenkaku:zenkakuAndHankakuStrings:{0}", IsZenkaku(zenkakuAndHankakuStrings));
            Output.WriteLine("IsHankaku:zenkakuOnly:{0}", IsHankaku(zenkakuOnlyStrings));
            Output.WriteLine("IsHankaku:hankakuOnlyStrings:{0}", IsHankaku(hankakuOnlyStrings));
            Output.WriteLine("IsHankaku:zenkakuAndHankakuStrings:{0}", IsHankaku(zenkakuAndHankakuStrings));
        }

        private bool IsZenkaku(string value)
        {
            //
            // 指定された文字列が全て全角文字で構成されているか否かは
            // 文字列を一旦SJISに変換し取得したバイト数と元文字列の文字数＊２が
            // 成り立つか否かで決定できる。
            //
            return Encoding.GetEncoding("sjis").GetByteCount(value) == value.Length*2;
        }

        private bool IsHankaku(string value)
        {
            //
            // 指定された文字列が全て半角文字で構成されているか否かは
            // 文字列を一旦SJISに変換し取得したバイト数と元文字列の文字数が
            // 成り立つか否かで決定できる。
            //
            return Encoding.GetEncoding("sjis").GetByteCount(value) == value.Length;
        }
    }
}