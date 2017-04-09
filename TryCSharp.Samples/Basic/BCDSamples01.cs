using System;
using TryCSharp.Common;

namespace TryCSharp.Samples.Basic
{
    /// <summary>
    ///     BCD変換についてのサンプルです。
    /// </summary>
    [Sample]
    // ReSharper disable once InconsistentNaming
    public class BCDSamples01 : IExecutable
    {
        public void Execute()
        {
            var val1 = int.MaxValue;
            var val2 = long.MaxValue;

            var bcdVal1 = BCDUtils.ToBCD(val1, 5);
            var bcdVal2 = BCDUtils.ToBCD(val2, 10);

            Output.WriteLine("integer value = {0}", val1);
            Output.WriteLine("BCD   value = {0}", BitConverter.ToString(bcdVal1));
            Output.WriteLine("long  value = {0}", val2);
            Output.WriteLine("BCD   value = {0}", BitConverter.ToString(bcdVal2));

            var val3 = BCDUtils.ToInt(bcdVal1);
            var val4 = BCDUtils.ToLong(bcdVal2);

            Output.WriteLine("val1 == val3 = {0}", val1 == val3);
            Output.WriteLine("val2 == val4 = {0}", val2 == val4);
        }

        /// <summary>
        ///     BCD変換を行うユーティリティクラスです。
        /// </summary>
        public static class BCDUtils
        {
            public static int ToInt(byte[] bcd)
            {
                return Convert.ToInt32(ToLong(bcd));
            }

            public static long ToLong(byte[] bcd)
            {
                long result = 0;

                foreach (var b in bcd)
                {
                    var digit1 = b >> 4;
                    var digit2 = b & 0x0f;

                    result = result*100 + digit1*10 + digit2;
                }

                return result;
            }

            public static byte[] ToBCD(int num, int byteCount)
            {
                return ToBCD<int>(num, byteCount);
            }

            public static byte[] ToBCD(long num, int byteCount)
            {
                return ToBCD<long>(num, byteCount);
            }

            private static byte[] ToBCD<T>(T num, int byteCount) where T : struct, IConvertible
            {
                var val = Convert.ToInt64(num);

                var bcdNumber = new byte[byteCount];
                for (var i = 1; i <= byteCount; i++)
                {
                    var mod = val%100;

                    var digit2 = mod%10;
                    var digit1 = (mod - digit2)/10;

                    bcdNumber[byteCount - i] = Convert.ToByte(digit1*16 + digit2);

                    val = (val - mod)/100;
                }

                return bcdNumber;
            }
        }
    }
}