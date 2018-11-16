using System;
using System.Linq;
using TryCSharp.Common;

namespace TryCSharp.Samples.Basic
{
    /// <summary>
    ///     文字列中の改行コードの数を算出するサンプルです。
    /// </summary>
    [Sample]
    public class NewLineDetectSample01 : IExecutable
    {
        public void Execute()
        {
            var testStrings = string.Format("あt{0}いt{0}う{0}えt{0}お{0}", Environment.NewLine);

            Output.WriteLine("=== 元文字列 start ===");
            Output.WriteLine(testStrings);
            Output.WriteLine("=== 元文字列 end  ===");

            //
            // 改行コードを判定するための、比較元文字配列を構築.
            //
            var newLineChars = Environment.NewLine.ToCharArray();

            //
            // 改行コードのカウントを算出.
            //
            var count = 0;
            var prevChar = char.MaxValue;
            foreach (var ch in testStrings)
            {
                //
                // プラットフォームによっては、改行コードが２文字の構成 (CRLF)となるため
                // 前後の文字のパターンが両方一致する場合に改行コードであるとみなす。
                //
                if (newLineChars.Contains(prevChar) && newLineChars.Contains(ch))
                {
                    count++;
                }

                prevChar = ch;
            }

            Output.WriteLine("改行コードの数: {0}", count);
        }
    }
}