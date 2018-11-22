using TryCSharp.Common;
using TryCSharp.Samples.Basic.InterfaceSamples01_Inner;

namespace TryCSharp.Samples.Basic
{
    /// <summary>
    ///     インターフェースの実装に関するサンプルです。
    /// </summary>
    /// <remarks>
    ///     インターフェースの「明示的実装(explicit)」「暗黙的実装(implicit)」について
    ///     記述しています。
    /// </remarks>
    [Sample]
    public class InterfaceSamples01 : IExecutable, IExplicitInterface, IImplicitInterface
    {
        public void Execute()
        {
            //
            // 暗黙的なインターフェース実装している側はそのまま呼び出せる.
            //
            var impl = new InterfaceSamples01();
            impl.Print();

            //
            // 明示的なインターフェース実装している側は
            // キャストしてその型にしてからでないと呼び出せない.
            //
            var exImpl = (IExplicitInterface) impl;
            exImpl.Print();
        }

        #region 明示的実装

        //
        // 明示的実装はスコープをprivateにして定義する必要がある.
        // 且つ、メソッド名を「インターフェース名.メソッド名」で定義する必要がある.
        // これにより、このインタフェースメソッド実装を利用する場合は、その型にキャスト
        // しないと利用する事が出来ないように出来る.
        //
        void IExplicitInterface.Print()
        {
            Output.WriteLine("IExplicitInterface.Print");
        }

        #endregion

        #region 暗黙的実装

        public void Print()
        {
            Output.WriteLine("IImplicitInterface.Print");
        }

        #endregion
    }

    namespace InterfaceSamples01_Inner
    {
        internal interface IExplicitInterface
        {
            void Print();
        }

        internal interface IImplicitInterface
        {
            void Print();
        }
    }

}