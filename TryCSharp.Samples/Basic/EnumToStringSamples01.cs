using TryCSharp.Common;

namespace TryCSharp.Samples.Basic
{
    /// <summary>
    ///     Enumに関するサンプルです。
    /// </summary>
    [Sample]
    public class EnumToStringSamples01 : IExecutable
    {
        public void Execute()
        {
            var blue = MyColor.Blue;

            // 2
            Output.WriteLine(blue.ToString("D"));
            // 0x00000002
            Output.WriteLine("0x{0}", blue.ToString("X"));
            // Blue
            Output.WriteLine(blue.ToString("G"));
        }

        private enum MyColor
        {
            Red = 1,
            Blue,
            White,
            Black
        }
    }
}