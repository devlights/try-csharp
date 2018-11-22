using System;
using System.Linq;
using TryCSharp.Common;

namespace TryCSharp.Samples.Basic
{
    /// <summary>
    ///     Bufferクラスのサンプルです。
    /// </summary>
    [Sample]
    public class BufferSamples01 : IExecutable
    {
        public void Execute()
        {
            //
            // System.Bufferクラスのサンプル
            //   http://msdn.microsoft.com/ja-jp/library/system.buffer.aspx
            //
            // Bufferクラスは、プリミティブ型の配列にのみ利用できるバッファである。
            // Bufferクラス内で保持されるデータは、連続したバイト列として扱われる。
            //
            // 対応している型は
            //   Boolean、Char、SByte、Byte、Int16、UInt16、Int32、UInt32、Int64、UInt64、IntPtr、UIntPtr、Single、および Double
            // となっている。
            //
            // プリミティブ型に関しては、System.Arrayの同様のメソッドよりBufferの方がパフォーマンスが良いとのこと。
            //
            const int BUFFER_LENGTH = 10000000;

            var srcArray = new int[BUFFER_LENGTH];

            var rnd = new Random();
            for (var i = 0; i < BUFFER_LENGTH; i++)
            {
                srcArray[i] = rnd.Next(127);
            }

            //
            // BlockCopyメソッド
            //   Array.Copyと使い方は同じだが、4番目の引数が[length]ではなく[count]となっている事に注意。
            //   この引数には、コピーする「バイト数」を渡す必要がある。
            //   なので、intの配列の場合は配列のLengthに
            //      sizeof(int)
            //   を掛けたものを指定する必要がある。
            //
            var destArray = new int[BUFFER_LENGTH];
            Buffer.BlockCopy(srcArray, 0, destArray, 0, destArray.Length*sizeof(int));

            // Array.Copy版
            var destArray2 = new int[BUFFER_LENGTH];
            Array.Copy(srcArray, 0, destArray2, 0, destArray2.Length);

            Output.WriteLine("[Buffer.BlockCopy] srcArray == destArray ==> {0}", srcArray.SequenceEqual(destArray));
            Output.WriteLine("[Array.Copy]       srcArray == destArray2 ==> {0}", srcArray.SequenceEqual(destArray2));
            Output.WriteLine("destArray == destArray2 ==> {0}", destArray.SequenceEqual(destArray2));

            //
            // ByteLengthメソッド
            //   指定した配列の「バイト数」を取得する。要素数で無い事に注意。
            //   指定する配列の型はプリミティブ型である必要がある。
            //
            Output.WriteLine("[Buffer.ByteLength] {0}", Buffer.ByteLength(destArray));
            Output.WriteLine("[Array.Length]      {0}", destArray2.Length);

            try
            {
                Buffer.ByteLength(new object[10]);
            }
            catch (ArgumentException argEx)
            {
                Output.WriteLine(argEx.Message);
            }

            //
            // GetByteメソッド
            //   指定したバイト位置のバイトデータを取得する.
            //
            Output.WriteLine("[Block.GetByte] {0}", Buffer.GetByte(destArray, 2));
            Output.WriteLine("[Array] {0}", destArray2[2]);

            //
            // SetByteメソッド
            //   指定したバイト位置にバイトデータを書き込む.
            //
            var b1 = Buffer.GetByte(destArray, 2);
            var i1 = destArray[0];

            Buffer.SetByte(destArray, 2, 12);

            var b2 = Buffer.GetByte(destArray, 2);
            var i2 = destArray[0];

            Output.WriteLine("[SetByte BEFORE] {0}, {1}", b1, i1);
            Output.WriteLine("[SetByte AFTER]  {0}, {1}", b2, i2);
        }
    }
}