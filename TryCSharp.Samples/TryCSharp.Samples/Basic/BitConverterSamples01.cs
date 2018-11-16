using System;
using TryCSharp.Common;

namespace TryCSharp.Samples.Basic
{
    /// <summary>
    ///     System.BitConverterクラスのサンプルです。
    /// </summary>
    [Sample]
    public class BitConverterSamples01 : IExecutable
    {
        public void Execute()
        {
            //
            // バイト列から16進文字列への変換.
            //
            byte[] bytes = {1, 2, 10, 15, (byte) 'a', (byte) 'b', (byte) 'q'};
            Output.WriteLine(BitConverter.ToString(bytes));

            //
            // 数値からバイト列への変換.
            // (一旦数値をバイト列に変換してから、16進に変換して表示)
            //
            var i = 100;
            Output.WriteLine(BitConverter.ToString(BitConverter.GetBytes(i)));

            var i2 = 0x12345678;
            Output.WriteLine(BitConverter.ToString(BitConverter.GetBytes(i2)));

            //
            // バイト列から数値への変換.
            //
            bytes = new byte[] {1};
            Output.WriteLine(BitConverter.ToBoolean(bytes, 0));

            bytes = new byte[] {1, 0, 0, 0};
            Output.WriteLine(BitConverter.ToInt32(bytes, 0));

            bytes = BitConverter.GetBytes((byte) 'a');
            Output.WriteLine(BitConverter.ToChar(bytes, 0));
        }
    }
}