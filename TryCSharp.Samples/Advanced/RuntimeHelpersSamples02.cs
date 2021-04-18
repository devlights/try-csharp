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
    public class RuntimeHelpersSamples02 : IExecutable
    {
        public void Execute()
        {
            //
            // RuntimeHelpers.PrepareConstrainedRegionsを呼び出すと、コンパイラは
            // そのメソッド内のcatch, finallyブロックをCER（制約された実行領域）としてマークする。
            //
            // CERとしてマークされた領域から、コードを呼び出す場合、そのコードには信頼性のコントラクトが必要となる。
            // コードに対して、信頼性のコントラクトを付与するには、以下の属性を利用する。
            //  [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
            //
            // CERでマークされた領域にて、コードに信頼性のコントラクトが付与されている場合、CLRは
            // try内の本処理が実行される前に、catch, finallyブロックのコードを事前コンパイルする。
            //
            // なので、例えばfinallyブロック内にて静的コンストラクタを持つクラスのメソッドを呼びだしていたり
            // すると、try内の本処理よりも先にfinallyブロック内の静的コンストラクタが呼ばれる事になる。
            // (事前コンパイルが行われると、アセンブリのロード、静的コンストラクタの実行などが発生するため)
            //
            RuntimeHelpers.PrepareConstrainedRegions();

            try
            {
                // 事前にRuntimeHelpers.PrepareConstrainedRegions()を呼び出している場合
                // 以下のメソッドが呼び出される前に、catch, finallyブロックが事前コンパイルされる.
                Calc();
            }
            finally
            {
                SampleClass.Print();
            }
        }

        private void Calc()
        {
            for (var i = 0; i < 10; i++)
            {
                Output.Write("{0} ", i + 1);
            }

            Output.WriteLine("");
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
            // RuntimeHelpers.PrepareConstrainedRegionsメソッドにて
            // 実行できるのは、Consistency.WillNotCorruptStateおよびMayCorruptInstanceの場合のみ.
            //
            // 尚、この属性はメソッドだけではなく、クラスやインターフェースにも付与できる。
            // その場合、クラス全体に対して信頼性のコントラクトを付与したことになる。
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
