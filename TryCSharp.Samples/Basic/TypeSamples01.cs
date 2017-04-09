using System.Collections.Generic;
using System.Linq;
using TryCSharp.Common;

namespace TryCSharp.Samples.Basic
{
    /// <summary>
    ///     System.Typeのサンプルです。
    /// </summary>
    [Sample]
    public class TypeSamples01 : IExecutable
    {
        public void Execute()
        {
            var theList = new List<int> {1, 2, 3, 4, 5};
            var theDictionary = new Dictionary<int, string> {{1, "hoge"}, {2, "hehe"}};

            //
            // Genericなオブジェクトの型引数の型を取得するには、System.Typeクラスの以下のメソッドを利用する。
            //
            //   ・GetGenericArguments()
            //
            // GetGenericArgumentsメソッドは、System.Typeの配列を返すので、これを利用して型引数の型を判別する。
            //
            var genericArgTypes = theList.GetType().GetGenericArguments();
            Output.WriteLine("=============== List<int>の場合 =================");
            Output.WriteLine("型引数の数={0}, 型引数の型=({1})", genericArgTypes.Count(), string.Join(",", genericArgTypes.Select(item => item.Name)));

            genericArgTypes = theDictionary.GetType().GetGenericArguments();
            Output.WriteLine("=============== Dictionary<int, string>の場合 =================");
            Output.WriteLine("型引数の数={0}, 型引数の型=({1})", genericArgTypes.Count(), string.Join(",", genericArgTypes.Select(item => item.Name)));
        }
    }
}