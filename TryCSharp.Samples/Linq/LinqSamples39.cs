using System.Linq;
using TryCSharp.Common;

namespace TryCSharp.Samples.Linq
{
    /// <summary>
    ///     Linqのサンプルです。
    /// </summary>
    [Sample]
    public class LinqSamples39 : IExecutable
    {
        public void Execute()
        {
            var numbers = new[] {1, 2, 3};

            // 
            // Any拡張メソッドは、一つでも条件に当てはまるものが存在するか否かを判別するメソッドである。
            // この拡張メソッドは、引数無しのバージョンと引数にpredicateを渡すバージョンの２つが存在する。
            //
            // 引数を渡さずAny拡張メソッドを呼んだ場合、Any拡張メソッドは
            // 該当シーケンスに要素が存在するか否かのみで判断する。
            // つまり、要素が一つでも存在する場合は、Trueとなる。
            //
            // 引数にpredicateを指定するバージョンは、シーケンスの各要素に対してpredicateを適用し
            // 一つでも条件に合致するものが存在した時点で、Trueとなる。
            //
            Output.WriteLine("=========== 引数無しでAny拡張メソッドを利用 ===========");
            Output.WriteLine("要素有り? = {0}", numbers.Any());
            Output.WriteLine("================================================");

            Output.WriteLine("=========== predicateを指定してAny拡張メソッドを利用 ===========");
            Output.WriteLine("要素有り? = {0}", numbers.Any(item => item >= 5));
            Output.WriteLine("要素有り? = {0}", numbers.Any(item => item <= 5));
            Output.WriteLine("================================================================");
        }
    }
}