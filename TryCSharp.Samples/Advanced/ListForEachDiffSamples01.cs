using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using TryCSharp.Common;

namespace TryCSharp.Samples.Advanced
{
    /// <summary>
    ///     Listをforeachでループする場合と、List.ForEachする場合の速度差をテスト
    /// </summary>
    [Sample]
    public class ListForEachDiffSamples01 : IExecutable
    {
        public void Execute()
        {
            Prepare();

            //
            // Listをforeachで処理するか、List.ForEachで処理するかで
            // どちらの方が速いのかを計測。
            //
            // ILレベルで見ると
            //   foreachの場合： callが2つ
            //   List.ForEachの場合： callvirtが1つ
            // となる。
            //
            var elementCounts = new[] {1000, 3000, 5000, 10000, 50000, 100000, 150000, 500000, 700000, 1000000};
            foreach (var elementCount in elementCounts)
            {
                Output.WriteLine("===== [Count:{0}] =====", elementCount);

                var theList = new List<int>(Enumerable.Range(1, elementCount));

                var watch = Stopwatch.StartNew();
                Sum_foreach(theList);
                watch.Stop();
                Output.WriteLine("foreach:      {0}", watch.Elapsed);

                watch = Stopwatch.StartNew();
                Sum_List_ForEach(theList);
                watch.Stop();
                Output.WriteLine("List.ForEach: {0}", watch.Elapsed);
            }
        }

        private void Prepare()
        {
            var result = 0;
            foreach (var x in new List<int>(Enumerable.Range(1, 1000)))
            {
                result += x;
            }

            result = 0;
            new List<int>(Enumerable.Range(1, 1000)).ForEach(x => result += x);
        }

        private int Sum_foreach(List<int> theList)
        {
            var result = 0;
            foreach (var x in theList)
            {
                result += x;
            }
            return result;
        }

        private int Sum_List_ForEach(List<int> theList)
        {
            var result = 0;
            theList.ForEach(x => result += x);
            return result;
        }
    }
}