using System;
using System.Globalization;
using TryCSharp.Common;

namespace TryCSharp.Samples.Basic
{
    /// <summary>
    ///     日付の解析に関するサンプルです。(DateTime)
    /// </summary>
    [Sample]
    public class DateParseSample01 : IExecutable
    {
        public void Execute()
        {
            //
            // ParseExactメソッドの場合は、値が2011, フォーマットがyyyy
            // の場合でも日付変換出来る。
            //
            try
            {
                var d = DateTime.ParseExact("2011", "yyyy", null);
                Output.WriteLine(d);
            }
            catch (Exception e)
            {
                Output.WriteLine(e);
            }

            //
            // TryParseメソッドの場合は、以下のどちらもFalseとなる。
            // 恐らく、IFormatProviderを設定しないと動かないと思われる。
            //
            DateTime d2;
            Output.WriteLine(DateTime.TryParse("2011", out d2));
            Output.WriteLine(DateTime.TryParse("2011", null, DateTimeStyles.None, out d2));

            //
            // TryParseExactメソッドの場合は、値が2011、フォーマットがyyyy
            // の場合でも日付変換出来る。
            //
            DateTime d3;
            Output.WriteLine(DateTime.TryParseExact("2011", "yyyy", null, DateTimeStyles.None, out d3));

            Output.WriteLine(DateTime.Now.ToString("yyyyMMddHHmmssfff"));

            var d98 = DateTime.Now;
            var d99 = DateTime.ParseExact(d98.ToString("yyyyMMddHHmmssfff"), "yyyyMMddHHmmssfff", null);
            Output.WriteLine(d98 == d99);
            Output.WriteLine(d98.Ticks);
            Output.WriteLine(d98 == new DateTime(d98.Ticks));

            // 時分秒を指定していない場合は、00:00:00となる
            var d100 = new DateTime(2011, 11, 12);
            Output.WriteLine("{0}, {1}, {2}", d100.Hour, d100.Minute, d100.Second);
        }
    }
}