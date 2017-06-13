using System;
using System.Linq;
using TryCSharp.Common;

namespace TryCSharp.Samples.Linq
{
    /// <summary>
    ///     Linqのサンプルです。
    /// </summary>
    [Sample]
    public class LinqSamples47 : IExecutable
    {
        public void Execute()
        {
            //
            // ElementAt拡張メソッドは、指定した位置の要素を取得するメソッド。
            //
            // 範囲外のインデックスを指定した場合は例外が発生する.
            //
            var languages = new[] {"csharp", "visualbasic", "java", "python", "ruby", "php", "c++"};
            Output.WriteLine(languages.ElementAt(1));

            try
            {
                languages.ElementAt(100);
            }
            catch (ArgumentOutOfRangeException)
            {
                Output.WriteLine("要素の範囲外のインデックスを指定している。");
            }

            //
            // ElementAtOrDefault拡張メソッドは、ElementAt拡張メソッドと同じ動作を
            // しながら、範囲外のインデックスを指定された場合に規定値を返すメソッド。
            //
            Output.WriteLine(languages.ElementAtOrDefault(-1) ?? "null");
            Output.WriteLine(languages.ElementAtOrDefault(100) ?? "null");
        }
    }
}