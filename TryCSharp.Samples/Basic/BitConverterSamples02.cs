using System;
using System.Collections.Generic;
using System.Linq;
using TryCSharp.Common;
using TryCSharp.Samples.Basic.BitConverterSamples02_Extensions;

namespace TryCSharp.Samples.Basic
{
    /// <summary>
    ///     System.BitConverterクラスのサンプルです。
    /// </summary>
    /// <remarks>
    ///     BitConverterがデフォルトでは対応していない
    ///     decimalとDateTimeのバイナリ化についてのサンプルです。
    /// </remarks>
    public class BitConverterSamples02 : IExecutable
    {
        public void Execute()
        {
            var hexConverter = new Func<byte, string>(i => i.ToString("x"));

            //
            // decimal型のバイト列変換
            //   decimal.GetBitsメソッドを使用することで
            //   対象のdecimal値の内部表現をintの配列で取得できる。
            //   後は、それをBitConverterを使ってバイト列にする。
            //
            decimal.GetBits(decimal.MaxValue)
                .SelectMany(BitConverter.GetBytes)
                .JoinAndPrint(Output.WriteLine, hexConverter);

            Output.WriteLine();

            //
            // DateTimeのバイト列変換
            //   DateTime.ToBinaryメソッドを使用することで
            //   対象のDateTime値をlongで取得できる。
            //   後は、それをBitConverterを使ってバイト列にする。
            //
            BitConverter.GetBytes(DateTime.MaxValue.ToBinary())
                .JoinAndPrint(Output.WriteLine, hexConverter);
        }
    }

    namespace BitConverterSamples02_Extensions
    {
        internal static class ListExtensions
        {
            public static void JoinAndPrint<T>(this IEnumerable<T> self, Action<object> action,
                Func<T, string> hexConverter, string prefix = "0x")
            {
                var enumerable = self as T[] ?? self.ToArray();
                var queryHex = enumerable.Select(hexConverter);

                action(string.Join(" ", enumerable));
                action(string.Join(" ", queryHex));
            }
        }
    }

}