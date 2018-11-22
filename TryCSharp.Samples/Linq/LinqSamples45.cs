using System.Linq;
using TryCSharp.Common;

namespace TryCSharp.Samples.Linq
{
    /// <summary>
    ///     Linqのサンプルです。
    /// </summary>
    [Sample]
    public class LinqSamples45 : IExecutable
    {
        public void Execute()
        {
            var languages = new[] {"csharp", "visualbasic", "java", "python", "ruby", "php", "c++"};

            //
            // First拡張メソッドは、シーケンスの最初の要素を返すメソッド。
            //
            // predicateを指定した場合は、その条件に合致する最初の要素が返る。
            //
            Output.WriteLine("============ First ============");
            Output.WriteLine(languages.First());
            Output.WriteLine(languages.First(item => item.StartsWith("v")));

            //
            // Last拡張メソッドは、シーケンスの最後の要素を返すメソッド。
            //
            // predicateを指定した場合は、その条件に合致する最後の要素が返る。
            //
            Output.WriteLine("============ Last ============");
            Output.WriteLine(languages.Last());
            Output.WriteLine(languages.Last(item => item.StartsWith("p")));

            //
            // Single拡張メソッドは、シーケンスの唯一の要素を返すメソッド。
            //
            // Singleを利用する場合、対象となるシーケンスには要素が一つだけ
            // でないといけない。複数の要素が存在する場合例外が発生する。
            //
            // また、空のシーケンスに対してSingleを呼ぶと例外が発生する。
            //
            // predicateを指定した場合は、その条件に合致するシーケンスの唯一の
            // 要素が返される。この場合も、結果のシーケンスには要素が一つだけの
            // 状態である必要がある。条件に合致する要素が複数であると例外が発生する、
            //
            Output.WriteLine("============ Single ============");
            var onlyOne = new[] {"csharp"};
            Output.WriteLine(onlyOne.Single());

            try
            {
                // ReSharper disable once UnusedVariable
                var val = languages.Single();
            }
            catch
            {
                Output.WriteLine("複数の要素が存在する状態でSingleを呼ぶと例外が発生する。");
            }

            Output.WriteLine(languages.Single(item => item.EndsWith("y")));

            try
            {
                // ReSharper disable once UnusedVariable
                var val = languages.Single(item => item.StartsWith("p"));
            }
            catch
            {
                Output.WriteLine("条件に合致する要素が複数存在する場合例外が発生する。");
            }
        }
    }
}