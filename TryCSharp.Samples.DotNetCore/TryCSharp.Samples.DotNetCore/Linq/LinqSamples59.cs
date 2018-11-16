using System.Linq;
using TryCSharp.Common;

namespace TryCSharp.Samples.Linq
{
    /// <summary>
    ///     Linqにて、シーケンスをチャンクに分割して処理するサンプルです.
    /// </summary>
    [Sample]
    public class LinqSamples59 : IExecutable
    {
        public void Execute()
        {
            //
            // 要素が10のシーケンスを2つずつのチャンクに分割.
            //
            foreach (var chunk in Enumerable.Range(1, 10).Chunk(2))
            {
                Output.WriteLine("Chunk:");
                foreach (var item in chunk)
                {
                    Output.WriteLine("\t--> {0}", item);
                }
            }

            //
            // 要素が10000のシーケンスを1000ずつのチャンクに分割し
            // それぞれのチャンクごとにインデックスを付与.
            //
            foreach (var chunk in Enumerable.Range(1, 10000).Chunk(1000).Select((x, i) => new {Index = i, Count = x.Count()}))
            {
                Output.WriteLine(chunk);
            }
        }
    }
}