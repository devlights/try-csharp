using TryCSharp.Common;

namespace TryCSharp.Samples.CSharp6
{
    /// <summary>
    /// C# 6.0 新機能についてのサンプルです。
    /// </summary>
    /// <remarks>
    /// Auto-Property enhancements (自動プロパティの機能強化) について
    /// </remarks>
    [Sample]
    public class AutoPropSamples01 : IExecutable
    {
        /// <summary>
        /// 自動プロパティのサンプル。初期化付き。
        /// </summary>
        private string AutoProp1 { get; } = "Hello World";

        /// <summary>
        /// 自動プロパティのサンプル。初期化付き。
        /// </summary>
        private string AutoProp2 { get; set; } = "Initial Value";

        /// <summary>
        /// 処理を実行します。
        /// </summary>
        public void Execute()
        {
            // --------------------------------------------------
            // C# 6.0 より 自動プロパティの初期化が行えるようになった。
            // 以前のバージョンでは、コンストラクタで行っていた部分を
            // 宣言時に行える様になっている。
            // なので、初期値から変更されることのない値を持つ
            // 自動プロパティの場合に、private set; を持つ必要がなく
            // get; のみで宣言出来る。
            // --------------------------------------------------
            Output.WriteLine(AutoProp1);
            Output.WriteLine(AutoProp2);

            AutoProp2 = "Updated";
            Output.WriteLine(AutoProp2);
        }
    }
}