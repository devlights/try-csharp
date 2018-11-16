using System.Linq;
using TryCSharp.Common;

namespace TryCSharp.Samples.Linq
{
    /// <summary>
    ///     Linqのサンプルです。
    /// </summary>
    [Sample]
    public class LinqSamples46 : IExecutable
    {
        public void Execute()
        {
            //
            // FirstOrDefault拡張メソッドは、First拡張メソッドと同じ動作をする。
            // 違いは、シーケンスに要素が存在しない場合に規定値を返す点である。
            //
            var emptySequence = Enumerable.Empty<string>();
            var languages = new[] {"csharp", "visualbasic", "java", "python", "ruby", "php", "c++"};

            try
            {
                // First拡張メソッドは要素が存在しない場合例外が発生する.
                emptySequence.First();
            }
            catch
            {
                Output.WriteLine("First拡張メソッドで例外発生");
            }

            Output.WriteLine("FirstOrDefaultの場合: {0}", emptySequence.FirstOrDefault() ?? "null");
            Output.WriteLine("FirstOrDefaultの場合(predicate): {0}", languages.FirstOrDefault(item => item.EndsWith("z")) ?? "null");

            //
            // LastOrDefault拡張メソッドは、Last拡張メソッドと同じ動作をする。
            // 違いは、シーケンスに要素が存在しない場合に規定値を返す点である。
            //
            try
            {
                // Last拡張メソッドは要素が存在しない場合例外が発生する.
                emptySequence.Last();
            }
            catch
            {
                Output.WriteLine("Last拡張メソッドで例外発生");
            }

            Output.WriteLine("LastOrDefaultの場合: {0}", emptySequence.LastOrDefault() ?? "null");
            Output.WriteLine("LastOrDefaultの場合(predicate): {0}", languages.LastOrDefault(item => item.EndsWith("z")) ?? "null");

            //
            // SingleOrDefault拡張メソッドは、Single拡張メソッドと同じ動作をする。
            // 違いは、シーケンスに要素が存在しない場合に規定値を返す点である。
            //
            try
            {
                // Last拡張メソッドは要素が存在しない場合例外が発生する.
                emptySequence.Single();
            }
            catch
            {
                Output.WriteLine("Single拡張メソッドで例外発生");
            }

            Output.WriteLine("SingleOrDefaultの場合: {0}", emptySequence.SingleOrDefault() ?? "null");
            Output.WriteLine("SingleOrDefaultの場合(predicate): {0}", languages.SingleOrDefault(item => item.EndsWith("z")) ?? "null");

            //
            // DefaultIfEmpty拡張メソッドは、シーケンスが空の場合に規定値を返すメソッド。
            //
            // シーケンスに要素が存在する場合は、そのままの状態で返す。
            // LINQにて外部結合を行う際に必須となるメソッド。
            //
            Output.WriteLine("================ DefaultIfEmpty ====================");

            var emptyIntegers = Enumerable.Empty<int>();
            foreach (var item in emptyIntegers.DefaultIfEmpty())
            {
                Output.WriteLine("基本型の場合: {0}", item);
            }

            foreach (var item in emptySequence.DefaultIfEmpty())
            {
                Output.WriteLine("参照型の場合: {0}", item ?? "null");
            }

            foreach (var item in languages.DefaultIfEmpty())
            {
                Output.WriteLine(item ?? "null");
            }

            foreach (var item in emptySequence.DefaultIfEmpty("デフォルト値"))
            {
                Output.WriteLine(item ?? "null");
            }
        }
    }
}