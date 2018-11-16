using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using TryCSharp.Common;

namespace TryCSharp.Samples.Basic
{
    /// <summary>
    ///     OrderedDictionaryのサンプル1です。
    /// </summary>
    [Sample]
    public class OrderedDictionarySample01 : IExecutable
    {
        public void Execute()
        {
            var dicA = new Dictionary<string, string>();
            for (var i = 0; i < 100; i++)
            {
                dicA.Add(i.ToString(), $"HOGE-{i}");
            }

            PrintDictionary(dicA);
            Output.WriteLine("");

            var dicB = new OrderedDictionary();
            for (var i = 0; i < 100; i++)
            {
                dicB.Add(i.ToString(), $"HOGE-{i}");
            }

            PrintDictionary(dicB);
        }

        private void PrintDictionary(Dictionary<string, string> dic)
        {
            foreach (var pair in dic)
            {
                Output.WriteLine("{0}-{1}", pair.Key, pair.Value);
            }
        }

        private void PrintDictionary(OrderedDictionary dic)
        {
            foreach (DictionaryEntry entry in dic)
            {
                Output.WriteLine("{0}-{1}", entry.Key, entry.Value);
            }
        }
    }
}