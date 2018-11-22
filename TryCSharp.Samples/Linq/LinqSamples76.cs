using System.Linq;
using System.Xml.Linq;
using TryCSharp.Common;

namespace TryCSharp.Samples.Linq
{
    /// <summary>
    ///     LINQ to XMLのサンプルです.
    /// </summary>
    /// <remarks>
    ///     ナビゲーション(Descendants, Ancestorsメソッド)のサンプルです.
    /// </remarks>
    [Sample]
    public class LinqSamples76 : IExecutable
    {
        public void Execute()
        {
            //
            // Descendants(XName)
            //   現在の要素を起点として子孫要素を取得する.
            //   子孫の範囲は、直下だけでなく、ネストした子孫階層のデータも
            //   取得できる. Linq To XMLでよく利用するメソッドの一つ.
            //
            var root = BuildSampleXml();
            var elem = root.Descendants();

            Output.WriteLine("Count={0}", elem.Count());
            Output.WriteLine("=====================================");

            // "Customer"という名前の子孫要素を取得
            elem = root.Descendants("Customer");
            Output.WriteLine("Count={0}", elem.Count());
            Output.WriteLine("First item:");
            Output.WriteLine(elem.First());
            Output.WriteLine("=====================================");

            // 属性付きで絞り込み
            elem = root.Descendants("Customer").Where(x => x.Attribute("CustomerID")?.Value == "HUNGC");
            Output.WriteLine("Count={0}", elem.Count());
            Output.WriteLine("First item:");
            Output.WriteLine(elem.First());
            Output.WriteLine("=====================================");

            // クエリ式で利用
            elem = from node in root.Descendants("Customer")
                    let attr = node.Attribute("CustomerID")?.Value
                    where attr.StartsWith("L")
                    from child in node.Descendants("Region")
                    where child.Value == "CA"
                    select node;

            Output.WriteLine("Count={0}", elem.Count());
            Output.WriteLine("First item:");
            Output.WriteLine(elem.First());
            Output.WriteLine("=====================================");

            // 直接2階層下の要素名を指定
            elem = from node in root.Descendants("Region")
                    where node.Value == "CA"
                    select node;

            Output.WriteLine("Count={0}", elem.Count());
            Output.WriteLine("First item:");
            Output.WriteLine(elem.First());
            Output.WriteLine("=====================================");

            //
            // Ancestors(XName)      
            //   現在の要素の先祖要素を取得する.
            //   兄弟要素は取得できない（件数が0件となる)
            //   あくまで自分の先祖となる要素を指定する.
            //
            root = BuildSampleXml();
            var startingPoint = root.Descendants("Region").First(x => x.Value == "CA");

            var ancestors = startingPoint.Ancestors();

            Output.WriteLine("Count={0}", ancestors.Count());
            Output.WriteLine("First item:");
            Output.WriteLine(ancestors.First());
            Output.WriteLine("=====================================");

            // ContactNameは、現在の要素(Region)の先祖(FullAddress)ではないため指定しても取得できない
            ancestors = startingPoint.Ancestors("ContactName");

            Output.WriteLine("Count={0}", ancestors.Count());
            if (ancestors.Any())
            {
                Output.WriteLine("First item:");
                Output.WriteLine(ancestors.First());
            }

            Output.WriteLine("=====================================");

            // FullAddress要素の兄弟要素となるContactNameは取得できない
            startingPoint = root.Descendants("FullAddress").First();
            ancestors = startingPoint.Ancestors("ContactName");

            Output.WriteLine("Count={0}", ancestors.Count());
            if (ancestors.Any())
            {
                Output.WriteLine("First item:");
                Output.WriteLine(ancestors.First());
            }

            Output.WriteLine("=====================================");

            // FullAddress要素の先祖であるCustomer要素は取得できる.
            startingPoint = root.Descendants("FullAddress").First();
            ancestors = startingPoint.Ancestors("Customer");

            Output.WriteLine("Count={0}", ancestors.Count());
            if (ancestors.Any())
            {
                Output.WriteLine("First item:");
                Output.WriteLine(ancestors.First());
            }

            Output.WriteLine("=====================================");
        }

        private XElement BuildSampleXml()
        {
            //
            // サンプルXMLファイル
            //  see: http://msdn.microsoft.com/ja-jp/library/vstudio/bb387025.aspx
            //
            return XElement.Load(@"xml/CustomersOrders.xml");
        }
    }
}