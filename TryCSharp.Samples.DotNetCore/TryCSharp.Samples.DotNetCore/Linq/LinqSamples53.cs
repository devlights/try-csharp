using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using TryCSharp.Common;

namespace TryCSharp.Samples.Linq
{
    /// <summary>
    ///     LINQ to XMLのサンプルです。
    /// </summary>
    /// <remarks>
    ///     XElement.Loadを利用した読み込みのサンプルです。
    /// </remarks>
    [Sample]
    public class LinqSamples53 : IExecutable
    {
        private const string FILE_URI = @"LinqSamples53_Sample.xml";
        private const string DOWNLOAD_URI = @"https://sites.google.com/site/gsfzero1/Home/Books.xml?attredirects=0&d=1";

        public void Execute()
        {
            //
            // XElementもXDocumentもParseメソッドの他に
            // 自身を構築するためのLoadメソッドを持っている.
            //
            // Parseメソッドは、文字列を解析して構築する時に利用し
            // Loadメソッドは、既に存在しているものを読み込む際に利用する.
            //
            // Loadメソッドは、複数のオーバーロードを持ち
            //   ・URIを指定
            //   ・ストリームを指定
            // に大別される.
            //
            // 本サンプルでは、URIによる読み込みを記述する.
            // URIを指定するLoadメソッドのオーバーロードは以下の通り。
            //
            //   public XDocument Load(string)
            //   public XDocument Load(string, LoadOptions)
            //   public XElement  Load(string)
            //   public XElement  Load(string, LoadOptions)
            //
            CreateSampleXml();

            var rootElement = XElement.Load(FILE_URI);
            var query = from element in rootElement.Descendants("Person")
                    let name = element?.Attribute("name")?.Value
                    where !name.StartsWith("b")
                    select new {name};

            foreach (var item in query)
            {
                Output.WriteLine(item);
            }

            //
            // URLからXMLを読み込み
            //   XMLファイルは以下のサンプルを利用させてもらっている。
            //     http://msdn.microsoft.com/ja-jp/library/vstudio/bb387066.aspx
            //
            Output.WriteLine(XElement.Load(DOWNLOAD_URI));
        }

        private void CreateSampleXml()
        {
            if (File.Exists(FILE_URI))
            {
                File.Delete(FILE_URI);
            }

            var doc = MakeDocument();
            doc.Save(FILE_URI);
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