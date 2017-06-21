using System.Linq;
using System.Xml.Linq;
using TryCSharp.Common;

namespace TryCSharp.Samples.Linq
{
    /// <summary>
    ///     LINQ to XMLのサンプルです.
    /// </summary>
    /// <remarks>
    ///     ナビゲーション(Parentプロパティ)のサンプルです.
    /// </remarks>
    [Sample]
    public class LinqSamples75 : IExecutable
    {
        public void Execute()
        {
            //
            // Parent
            //   文字通り、現在の要素の親要素を取得する.
            //   親要素が存在しない場合、nullとなる.
            //
            var root = BuildSampleXml();
            var elem = root.Elements("Child").First();

            Output.WriteLine("root.Parent = {0}", root.Parent?.ToString() ?? "null");
            Output.WriteLine("elem.Parent = {0}", elem.Parent);

            var newElem = new XElement("GrandChild", "value5");
            Output.WriteLine("newElem.Parent = {0}", newElem.Parent?.ToString() ?? "null");

            root.Elements("Child").Last().Add(newElem);
            Output.WriteLine("newElem.Parent = {0}", newElem.Parent);
        }

        private XElement BuildSampleXml()
        {
            var root = new XElement("Root",
                new XElement("Child", "value1"),
                new XElement("Child", "value2"),
                new XElement("Child", "value3"),
                new XElement("Child", "value4")
            );

            return root;
        }
    }
}