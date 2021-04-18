#pragma warning disable SYSLIB0004
using System.Runtime.CompilerServices;
using System.Runtime.ConstrainedExecution;
using TryCSharp.Common;

namespace TryCSharp.Samples.Advanced
{
    /// <summary>
    ///     RuntimeHelpersクラスのサンプルです。
    /// </summary>
    /// <remarks>
    /// 本サンプルは .NET 5.0 では使用できません。（CERが非推奨扱いになっているため)
    /// 詳細については、以下に記載があります。
    ///   - https://docs.microsoft.com/ja-jp/dotnet/core/compatibility/syslib-warnings/syslib0004
    /// </remarks>
    [Sample]
    public class RuntimeHelpersSamples03 : IExecutable
    {
        public void Execute()
        {
            //
            // ExecuteCodeWithGuaranteedCleanupメソッドは, PrepareConstrainedRegionsメソッドと
            // 同様に、コードをCER（制約された実行環境）で実行するメソッドである。
            //
            // PrepareConstrainedRegionsメソッドが呼び出されたメソッドのcatch, finallyブロックを
            // CERとしてマークするのに対して、ExecuteCodeWithGuaranteedCleanupメソッドは
            // 明示的に実行コード部分とクリーンアップ部分 (バックアウトコード)を引数で渡す仕様となっている。
            //
            // ExecuteCodeWithGuaranteedCleanupメソッドは
            // TryCodeデリゲートとCleanupCodeデリゲート、及び、userDataを受け取る.
            //
            // public delegate void TryCode(object userData)
            // public delegate void CleanupCode(object userData, bool exceptionThrown)
            //
            // 前回のサンプルと同じ動作を行う.
            RuntimeHelpers.ExecuteCodeWithGuaranteedCleanup(Calc, Cleanup, null);
        }

        private void Calc(object userData)
        {
            for (var i = 0; i < 10; i++)
            {
                Output.Write("{0} ", i + 1);
            }

            Output.WriteLine("");
        }

        private void Cleanup(object userData, bool exceptionThrown)
        {
            SampleClass.Print();
        }

        // サンプルクラス
        private static class SampleClass
        {
            static SampleClass()
            {
                Output.WriteLine("SampleClass static ctor()");
            }

            //
            // このメソッドに対して、CER内で利用できるよう信頼性のコントラクトを付与.
            // ReliabilityContractAttributeおよびConsistencyやCerは
            // System.Runtime.ConstrainedExecution名前空間に存在する.
            //
            [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
            internal static void Print()
            {
                Output.WriteLine("SampleClass.Print()");
            }
        }
    }
}
#pragma warning restore SYSLIB0004
