using System.Linq;
using System.Xml.Linq;
using TryCSharp.Common;

namespace TryCSharp.Samples.Linq
{
    /// <summary>
    ///     LINQ to XMLのサンプルです.
    /// </summary>
    /// <remarks>
    ///     名前空間 (XNamespace) のサンプルです.
    /// </remarks>
    [Sample]
    public class LinqSamples69 : IExecutable
    {
        public void Execute()
        {
            //
            // 名前空間なし
            //   通常そのまま要素を作成すると名前空間無しとなる.
            //   名前空間無しの場合、XNamespace.Noneが設定されている.
            //   XName.Namespaceプロパティがnullにならないことは保証されている.
            //     http://msdn.microsoft.com/ja-jp/library/system.xml.linq.xnamespace.aspx
            //
            var root = BuildSampleXml();
            var name = root.Name;

            Output.WriteLine("is XNamespace.None?? == {0}", root.Name.Namespace == XNamespace.None);
            Output.WriteLine("=====================================");

            //
            // デフォルト名前空間あり
            //   元のXMLにデフォルト名前空間が設定されている場合
            //   取得したXElement -> XNameより名前空間が取得できる
            //
            //   デフォルト名前空間なので、要素を取得する際に名前空間の付与は
            //   必要ない。（そのまま取得できる)
            //
            root = BuildSampleXmlWithDefaultNamespace();
            name = root.Name;

            Output.WriteLine("XName.LocalName={0}", name.LocalName);
            Output.WriteLine("XName.Namespace={0}", name.Namespace);
            Output.WriteLine("XName.NamespaceName={0}", name.NamespaceName);
            Output.WriteLine("=====================================");

            //
            // デフォルト名前空間とカスタム名前空間あり
            //   デフォルト名前空間に関しては、上記の通り。
            //   カスタム名前空間の場合、要素を取得する際に
            //     XNamespace + "要素名"
            //   のように、名前空間を付与して取得する必要がある.
            //   カスタム名前空間内の要素は、XNamespaceを付与しないと
            //   取得できない.
            //
            root = BuildSampleXmlWithNamespace();
            name = root.Name;

            Output.WriteLine("XName.LocalName={0}", name.LocalName);
            Output.WriteLine("XName.Namespace={0}", name.Namespace);
            Output.WriteLine("XName.NamespaceName={0}", name.NamespaceName);

            if (!root.Descendants("Value").Any())
            {
                Output.WriteLine("[Count=0] Namespaceが違うので、要素が取得できない.");
            }

            Output.WriteLine("=====================================");

            var ns = (XNamespace) "http://www.tmpurl.org/MyXml2";
            var elem = root.Descendants(ns + "Value").First();
            name = elem.Name;

            Output.WriteLine("XName.LocalName={0}", name.LocalName);
            Output.WriteLine("XName.Namespace={0}", name.Namespace);
            Output.WriteLine("XName.NamespaceName={0}", name.NamespaceName);
            Output.WriteLine("=====================================");

            //
            // 名前空間付きで要素作成 (プレフィックスなし)
            //   要素作成の際に、名前空間を付与するには
            //   予めXNamespaceを作成しておき、それを
            //      XNamespace + "要素"
            //   という風に、文字列を結合するような要領で利用する。
            //   XNamespaceは、暗黙で文字列から生成できる.
            //
            var defaultNamespace = (XNamespace) "http://www.tmpurl.org/Default";
            var customNamespace = (XNamespace) "http://www.tmpurl.org/Custom";

            var newElement = new XElement(
                defaultNamespace + "RootNode",
                Enumerable.Range(1, 3).Select(x => new XElement(customNamespace + "ChildNode", x))
            );

            Output.WriteLine(newElement);
            Output.WriteLine("=====================================");

            //
            // 名前空間付きで要素作成 (プレフィックスあり)
            //   <ns:Node>xxx</ns:Node>
            // のように、要素に名前空間プレフィックスを付与するには
            // まず、プレフィックスを付与する要素を持つ親要素にて
            //   new XAttribute(XNamespace.Xmlns + "customs", "http://xxxxx/xxxx")
            // の属性を付与する。これにより、親要素にて
            //   <Root xmlns:customs="http://xxxxx/xxxx">
            // という感じになる。
            // 後は、プレフィックスを付与する要素にて通常通り
            //   new XElement(customNamespace + "ChildNode", x)
            // と定義することにより、自動的に合致するプレフィックスが設定される。
            // 
            newElement = new XElement(
                defaultNamespace + "RootNode",
                new XAttribute(XNamespace.Xmlns + "customns", "http://www.tmpurl.org/Custom"),
                from x in Enumerable.Range(1, 3)
                select new XElement(customNamespace + "ChildNode", x),
                new XElement(defaultNamespace + "ChildNode", 4)
            );

            Output.WriteLine(newElement);
            Output.WriteLine("=====================================");

            //
            // カスタム名前空間に属する要素を表示.
            //
            foreach (var e in newElement.Descendants(customNamespace + "ChildNode"))
            {
                Output.WriteLine(e);
            }

            Output.WriteLine("=====================================");

            //
            // デフォルト名前空間に属する要素を表示.
            //
            foreach (var e in newElement.Descendants(defaultNamespace + "ChildNode"))
            {
                Output.WriteLine(e);
            }

            Output.WriteLine("=====================================");

            //
            // 名前空間無しの要素を表示.
            //
            foreach (var e in newElement.Descendants("ChildNode"))
            {
                Output.WriteLine(e);
            }
        }

        private XElement BuildSampleXml()
        {
            return XElement.Parse("<Root><Child Id=\"100\" Id2=\"200\"><Value Id=\"300\">hoge</Value></Child></Root>");
        }

        private XElement BuildSampleXmlWithDefaultNamespace()
        {
            return XElement.Parse("<Root xmlns=\"http://www.tmpurl.org/MyXml\"><Child Id=\"100\" Id2=\"200\"><Value Id=\"300\">hoge</Value></Child></Root>");
        }

        private XElement BuildSampleXmlWithNamespace()
        {
            return XElement.Parse("<Root xmlns=\"http://www.tmpurl.org/MyXml\" xmlns:x=\"http://www.tmpurl.org/MyXml2\"><Child Id=\"100\" Id2=\"200\"><x:Value Id=\"300\">hoge</x:Value></Child></Root>");
        }
    }
}