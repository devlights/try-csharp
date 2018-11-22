using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using TryCSharp.Common;

namespace TryCSharp.Samples.Linq
{
    /// <summary>
    ///     LINQ to XMLのサンプルです。
    /// </summary>
    /// <remarks>
    ///     XDocumentオブジェクトを関数型構築するサンプルです。
    /// </remarks>
    [Sample]
    public class LinqSamples52 : IExecutable
    {
        public void Execute()
        {
            //
            // XDocumentは複数のコンストラクタを持っているが
            // 以下を利用すると、関数型構築が行える.
            // 関数型構築とは、単一のステートメントでXMLツリーを作成するための機能である。
            // 
            // public XDocument(object[])
            // public XDocument(XDeclaration, object[])
            //
            // XDocumentを起点として関数型構築を行う場合
            // ルート要素となるXElementを作成し、その子要素に
            // 様々な要素を設定する.
            //
            // 例：
            // var doc = new XDocument
            //           (
            //             new XElement
            //             (
            //               "RootElement",
            //               new XElement("ChildElement", "ChildValue1"),
            //               new XElement("ChildElement", "ChildValue2"),
            //               new XElement("ChildElement", "ChildValue3")
            //             )
            //           );
            //
            // 上記例は、以下のXMLツリーを構築する.
            // <RootElement>
            //   <ChildElement>ChildValue1</ChildElement>
            //   <ChildElement>ChildValue2</ChildElement>
            //   <ChildElement>ChildValue3</ChildElement>
            // </RootElement>
            //
            var doc = MakeDocument();
            Output.WriteLine(doc);
        }

        private XDocument MakeDocument()
        {
            var doc = new XDocument
            (
                new XDeclaration("1.0", "utf-8", "yes"),
                new XElement("Persons", MakePersonElements())
            );

            return doc;
        }

        private IEnumerable<XElement> MakePersonElements()
        {
            var names = new[] {"foo", "bar", "baz"};
            var query = from name in names
                    select new XElement
                    (
                        "Person",
                        new XAttribute("name", name),
                        $"VALUE-{name}"
                    );

            return query;
        }
    }
}