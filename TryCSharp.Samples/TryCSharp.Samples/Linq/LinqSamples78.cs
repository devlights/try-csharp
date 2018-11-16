using System.Linq;
using System.Xml.Linq;
using TryCSharp.Common;

namespace TryCSharp.Samples.Linq
{
    /// <summary>
    ///     LINQ to XMLのサンプルです.
    /// </summary>
    /// <remarks>
    ///     ナビゲーション(DescendantNodes, DescendantNodesAndSelf)のサンプルです.
    /// </remarks>
    [Sample]
    public class LinqSamples78 : IExecutable
    {
        public void Execute()
        {
            //
            // DescendantNodes
            //   子孫をXNodeで取得する.
            //   属性はノードではないため、含まれない.
            //
            //   取得できるデータがXElementではなく、XNodeであることに注意.
            //
            var root = BuildSampleXml();
            var startingPoint = root.Descendants("Book").First();

            // AndSelf無しなので、Book自身は含まれない.
            foreach (var node in startingPoint.DescendantNodes())
            {
                Output.WriteLine(node);
            }

            Output.WriteLine("=====================================");

            //
            // DescendantNodesAndSelf
            //   基本的な動作はDescendantNodesと同じ。
            //   AndSelfなので、自分自身もついてくる.
            //
            //   取得できるデータがXElementではなく、XNodeであることに注意.
            //
            root = BuildSampleXml();
            startingPoint = root.Descendants("Book").First();

            // AndSelfありなので、Book自身が含まれる
            foreach (var node in startingPoint.DescendantNodesAndSelf())
            {
                Output.WriteLine(node);
            }

            Output.WriteLine("=====================================");
        }

        private XElement BuildSampleXml()
        {
            //
            // サンプルXMLファイル
            //  see: http://msdn.microsoft.com/ja-jp/library/vstudio/ms256479(v=vs.90).aspx
            //
            return XElement.Load(@"xml/Books.xml");
        }
    }
}