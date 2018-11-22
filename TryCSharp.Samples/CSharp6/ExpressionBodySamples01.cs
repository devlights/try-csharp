using System;
using System.Threading.Tasks;
using TryCSharp.Common;

namespace TryCSharp.Samples.CSharp6
{
    /// <summary>
    /// C# 6.0 新機能についてのサンプルです。
    /// </summary>
    /// <remarks>
    /// Expression bodied function members（ラムダ式本体によるメンバーの記述）について
    /// </remarks>
    [Sample]
    public class ExpressionBodySamples01 : IAsyncExecutable
    {
        /// <summary>
        /// 処理を実行します。
        /// </summary>
        public async Task Execute()
        {
            // ---------------------------------------------------------
            // C# 6.0 より メンバーの記述にてラムダ式を利用できるようになった。
            // 注意点として、ステートメントではなく、あくまで「式」なので
            // {} が必要なレベルの記述は出来ない。pythonのlambdaと同じ感じ。
            // メソッドとプロパティの両方に適用できる。
            // ---------------------------------------------------------
            var str = "hello world";

            Output.WriteLine(this.Upper(str));
            await this.AsyncUpper(str);
            Output.WriteLine(Now);
        }

        // プロパティにラムダ式を適用した場合、このプロパティは自動的に getter のみとなる。
        private string Now => DateTime.Now.ToString("yyyy/MM/dd");

        // メソッドのラムダ式記述
        private string Upper(string original) => original.ToUpper();

        // asyncなメソッドのラムダ式記述。複数行に渡って書くことも出来るけど
        // ここまでするなら、ラムダ式にする意味が正直ない。。。。普通に書いたほうがいい。
        private async Task AsyncUpper(string original) => await Task.Run(async () =>
        {
            await Task.Delay(TimeSpan.FromSeconds(3));
            Output.WriteLine(this.Upper(original));
        });
    }
}