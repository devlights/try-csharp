using System.Linq;
using System.Xml.Linq;
using TryCSharp.Common;

namespace TryCSharp.Samples.Linq
{
    /// <summary>
    ///     LINQ to XMLのサンプルです.
    /// </summary>
    /// <remarks>
    ///     存在確認プロパティ (HasElements, HasAttributes) のサンプルです.
    /// </remarks>
    [Sample]
    public class LinqSamples70 : IExecutable
    {
        public void Execute()
        {
            //
            // HasElements
            //   名前の通り、現在のノードがサブノードを持っているか否かを取得する.
            //
            var root = BuildSampleXml();
            var child = root.Elements("Child").First();
            var grandChild = child.Elements("Value").First();

            Output.WriteLine("root.HasElements: {0}", root.HasElements);
            Output.WriteLine("child.HasElements: {0}", child.HasElements);
            Output.WriteLine("grand-child.HasElements: {0}", grandChild.HasElements);

            Output.WriteLine("=====================================");

            //
            // HasAttributes
            //   名前の通り、現在のノードが属性を持っているか否かを取得する.
            //
            root = BuildSampleXml();
            child = root.Elements("Child").First();
            grandChild = child.Elements("Value").First();

            Output.WriteLine("root.HasAttributes:{0}", root.HasAttributes);
            Output.WriteLine("child.HasAttributes:{0}", child.HasAttributes);
            Output.WriteLine("grand-child.HasAttributes:{0}", grandChild.HasAttributes);

            Output.WriteLine("=====================================");
        }

        private XElement BuildSampleXml()
        {
            return XElement.Parse("<Root><Child Id=\"100\" Id2=\"200\"><Value Id=\"300\">hoge</Value></Child></Root>");
        }
    }
}