using System.Linq;
using System.Xml.Linq;
using TryCSharp.Common;

namespace TryCSharp.Samples.Linq
{
    /// <summary>
    ///     LINQ to XMLのサンプルです.
    /// </summary>
    /// <remarks>
    ///     ナビゲーション(NodesAfterSelf, NodesBeforeSelf)のサンプルです.
    /// </remarks>
    [Sample]
    public class LinqSamples80 : IExecutable
    {
        public void Execute()
        {
            //
            // NodesAfterSelf
            //   現在の要素の後ろにある兄弟ノードを取得
            //   ElementsAfterSelfとの違いは、XElementであるかXNodeであるか
            //
            var root = BuildSampleXml();
            var startingPoint = root.Descendants("Book").First();

            foreach (var node in startingPoint.NodesAfterSelf())
            {
                Output.WriteLine(node);
            }

            Output.WriteLine("=====================================");

            root = BuildSampleXml();
            startingPoint = root.Descendants("Title").Last();

            foreach (var node in startingPoint.NodesAfterSelf())
            {
                Output.WriteLine(node);
            }

            //
            // NodesBeforeSelf
            //   現在の要素の前にある兄弟ノードを取得
            //   ElementsBeforeSelfとの違いは、XElementであるかXNodeであるか
            //
            root = BuildSampleXml();
            startingPoint = root.Descendants("PublishDate").Last();

            foreach (var node in startingPoint.NodesBeforeSelf())
            {
                Output.WriteLine(node);
            }
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