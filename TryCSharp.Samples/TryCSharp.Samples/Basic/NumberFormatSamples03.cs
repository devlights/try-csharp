using System;
using System.Globalization;
using TryCSharp.Common;

namespace TryCSharp.Samples.Basic
{
    /// <summary>
    ///     数値フォーマットのサンプルです。
    /// </summary>
    [Sample]
    public class NumberFormatSamples03 : IExecutable
    {
        public void Execute()
        {
            var s = "123,456";

            try
            {
                // ERROR.
                var i2 = int.Parse(s);
                Output.WriteLine(i2);
            }
            catch (FormatException ex)
            {
                Output.WriteLine(ex.Message);
            }

            var i3 = int.Parse(s, NumberStyles.AllowThousands);
            Output.WriteLine(i3);
        }
    }
}