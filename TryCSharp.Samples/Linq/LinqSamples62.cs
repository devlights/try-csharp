using System.Linq;
using System.Xml.Linq;
using TryCSharp.Common;

namespace TryCSharp.Samples.Linq
{
    /// <summary>
    ///     LINQ to XMLのサンプルです.
    /// </summary>
    /// <remarks>
    ///     要素削除系メソッドのサンプルです.
    /// </remarks>
    [Sample]
    public class LinqSamples62 : IExecutable
    {
        public void Execute()
        {
            //
            // Remove()
            //   現在の要素をXMLツリーより削除する.
            //
            var root = BuildSampleXml();
            var elem = root.Descendants("Value").First();

            elem.Remove();

            Output.WriteLine(root);
            Output.WriteLine("=====================================");

            //
            // RemoveAll()
            //   現在の要素から子ノード及び属性を削除する.
            //   属性まで削除される点に注意。
            //
            root = BuildSampleXml();
            elem = root.Elements("Child").First();

            elem.RemoveAll();

            Output.WriteLine(root);
            Output.WriteLine("=====================================");

            //
            // RemoveNodes()
            //   現在の要素から子ノードを削除する
            //   RemoveAllメソッドと違い、属性は削除されない
            //
            root = BuildSampleXml();
            elem = root.Elements("Child").First();

            elem.RemoveNodes();

            Output.WriteLine(root);
            Output.WriteLine("=====================================");

            //
            // SetElementValue(XName, object)
            //   本来は、子要素の値を設定するためのメソッドであるが
            //   要素の値にnullを設定することで削除することが出来る
            //
            root = BuildSampleXml();
            elem = root.Elements("Child").First();

            elem.SetElementValue("Value", null);

            Output.WriteLine(root);
            Output.WriteLine("=====================================");
        }

        private XElement BuildSampleXml()
        {
            return XElement.Parse("<Root><Child Id=\"100\"><Value>hoge</Value></Child></Root>");
        }
    }
}