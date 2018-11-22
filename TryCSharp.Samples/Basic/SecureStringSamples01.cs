using System;
using System.Runtime.InteropServices;
using System.Security;
using TryCSharp.Common;

namespace TryCSharp.Samples.Basic
{
    /// <summary>
    ///     SecureStringについてのサンプルです。
    /// </summary>
    [Sample]
    public class SecureStringSamples01 : IExecutable
    {
        public void Execute()
        {
            //
            // SecureStringのサンプル.
            //
            // System.Security.SecureStringクラスは、通常の文字列とは
            // 違い、パスワードなどの機密情報を扱ったりする際に利用される。
            //
            // よく利用されるProcessクラスのStartメソッドではパスワードを渡す際は
            // SecureStringを渡す必要がある。
            //
            // このクラスのインスタンスに設定された内容は自動的に暗号化され
            // MakeReadOnlyメソッドを利用して、読み取り専用とすると変更できなくなる。
            //
            // SecureStringにデータを設定する際は、AppendCharメソッドを利用して
            // 1文字ずつデータを設定していく必要がある。
            //
            // SecureStringには、値を比較または変換する為のメソッドが存在しない。
            // 操作を行う為には、System.Runtime.InteropServices.MarshalのCoTaskMemUnicodeメソッドと
            // Copyメソッドを利用してchar[]に変換する必要がある。
            //

            //
            // SecureStringを構築.
            //
            // 実際はユーザからのパスワード入力を元にSecureStringを構築したりする.
            //
            var secureStr = MakeSecureString();

            //
            // ToString()メソッドを呼び出してもSecureStringの中身を
            // 見ることはできない。
            //
            Output.WriteLine(secureStr);

            //
            // IsReadOnlyメソッドで現在読み取り専用としてマークされているか否かが
            // 判別できる。読み取り専用でない場合、変更は可能。
            //
            // 読み取り専用にするにはMakeReadOnlyメソッドを使用する。
            //
            Output.WriteLine("IsReadOnly:{0}", secureStr.IsReadOnly());
            secureStr.MakeReadOnly();
            Output.WriteLine("IsReadOnly:{0}", secureStr.IsReadOnly());

            //
            // SecureStringの中身を復元するには、以下のメソッドを利用する。
            //
            // ■Marshal.SecureStringToCoTaskMemUnicodeメソッド
            // ■Marshal.Copyメソッド
            // ■Marshal.ZeroFreeCoTaskMemUnicodeメソッド
            //
            RestoreSecureString(secureStr);
        }

        private SecureString MakeSecureString()
        {
            var secureStr = new SecureString();

            foreach (var ch in "hello world")
            {
                secureStr.AppendChar(ch);
            }

            return secureStr;
        }

        private void RestoreSecureString(SecureString secureStr)
        {
            var pointer = IntPtr.Zero;
            try
            {
                //
                // コピー先のバッファを作成.
                //
                var buffer = new char[secureStr.Length];

                //
                // 復元処理.
                //
                pointer = Marshal.SecureStringToCoTaskMemUnicode(secureStr);
                Marshal.Copy(pointer, buffer, 0, buffer.Length);

                Output.WriteLine(new string(buffer));
            }
            finally
            {
                if (pointer != IntPtr.Zero)
                {
                    //
                    // 解放.
                    //
                    Marshal.ZeroFreeCoTaskMemUnicode(pointer);
                }
            }
        }
    }
}