using System.Linq;
using System.Xml.Linq;
using TryCSharp.Common;

namespace TryCSharp.Samples.Linq
{
    /// <summary>
    ///     LINQ to XMLのサンプルです.
    /// </summary>
    /// <remarks>
    ///     空要素系プロパティとメソッド (IsEmpty, EmptySequence) のサンプルです.
    /// </remarks>
    [Sample]
    public class LinqSamples72 : IExecutable
    {
        public void Execute()
        {
            //
            // EmptySequence
            //   空のIEnumerable<XElement>を返す静的メソッド.
            //
            var empty = XElement.EmptySequence;

            Output.WriteLine("Count={0}", empty.Count());

            //
            // IsEmpty
            //   現在の要素が空要素か否かを判定する.
            //   空要素の条件は、MSDNに記載があり以下の通りとなる.
            //     「開始タグのみを持ち、終了した空の要素として表される要素だけが、空の要素と見なされます。」
            //     (http://msdn.microsoft.com/ja-jp/library/system.xml.linq.xelement.isempty.aspx)
            //
            var root = BuildSampleXmlNoNode();
            Output.WriteLine("IsEmpty={0}", root.IsEmpty);

            root = BuildSampleXml();
            Output.WriteLine("IsEmpty={0}", root.IsEmpty);
        }

        private XElement BuildSampleXmlNoNode()
        {
            return new XElement("Root");
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