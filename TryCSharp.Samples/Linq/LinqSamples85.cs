using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using TryCSharp.Common;
// ReSharper disable PossibleNullReferenceException

namespace TryCSharp.Samples.Linq
{
    /// <summary>
    ///     LINQ to XMLのサンプルです.
    /// </summary>
    /// <remarks>
    ///     XStreamingElementのサンプルです。
    /// </remarks>
    [Sample]
    public class LinqSamples85 : IExecutable
    {
        public void Execute()
        {
            //
            // XStreamingElement
            //   XStreamingElementは、遅延評価を行うクラス。
            //   主に、巨大なXMLデータを変換する際に利用できる.
            //
            //   参考URL:
            //     http://msdn.microsoft.com/ja-jp/library/system.xml.linq.xstreamingelement.aspx
            //     http://melma.com/backnumber_120830_4496326/
            //     http://msdn.microsoft.com/ja-jp/library/system.xml.linq.xnode.readfrom.aspx
            //     http://msdn.microsoft.com/ja-jp/library/system.xml.xmlreader.movetocontent.aspx
            //
            //   実際に利用する際は、ほとんどの場合がXmlReaderとyieldの仕組みを事前に作っておかないといけない。
            //   XmlReaderで巨大ファイルを逐次読み込みし、それをXStreamingElementで変換処理する。
            //
            // 以下の処理では、どの程度メモリを消費しているのかを確認するために
            // GC.GetTotalMemoryで消費量を表示している.
            Output.WriteLine("1:{0}", GC.GetTotalMemory(true));

            //
            // 巨大XMLファイルを作成.
            //
            var root = BuildSampleXml(CreateSampleXmlFile());

            Output.WriteLine("2:{0}", GC.GetTotalMemory(true));

            //
            // 普通にXElementを利用して変換処理.
            //
            ConvertXml(root);

            Output.WriteLine("3:{0}", GC.GetTotalMemory(true));

            //
            // XStreamingElementを利用して変換処理.
            //
            var result2 = ConvertXml2(root);

            Output.WriteLine("4:{0}", GC.GetTotalMemory(true));

            //
            // XStreamingElementで変換したデータを出力.
            //
            result2.Save(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "converted2.xml"));

            Output.WriteLine("5:{0}", GC.GetTotalMemory(true));

            //
            // ファイルの読み込みに、XmlReader+yieldを利用してXStreamingElementで変換処理.
            //
            var result3 = ConvertXml3();

            Output.WriteLine("6:{0}", GC.GetTotalMemory(true));

            //
            // XStreamingElementで変換したデータを出力.
            //
            result3.Save(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "converted3.xml"));

            Output.WriteLine("7:{0}", GC.GetTotalMemory(true));
        }

        private string CreateSampleXmlFile()
        {
            //
            // 巨大なXMLファイルをデスクトップに作成.
            //
            var dirPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            var filePath = Path.Combine(dirPath, "toobig.xml");

            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }

            //
            // <root>
            //   <data>
            //     <code>...</code>
            //     <name>...</name>
            //   </data>
            //   .
            //   .
            //   .
            // </root>
            //
            // の構造を持つXMLファイルを作成.
            //
            var doc = new XDocument
            (
                new XElement
                (
                    "root",
                    from i in Enumerable.Range(1, 100000)
                    select new XElement
                    (
                        "data",
                        new XElement("code", string.Format("{0:D5}", i)),
                        new XElement("name", string.Format("name-{0:D5}", i))
                    )
                )
            );

            doc.Save(filePath);

            return filePath;
        }

        private XElement BuildSampleXml(string filePath)
        {
            return XElement.Load(filePath);
        }

        private XElement ConvertXml(XElement original)
        {
            var result = new XElement
            (
                "newroot",
                from elem in original.Elements()
                select new XElement
                (
                    "newdata",
                    new XAttribute("code", elem.Element("code").Value),
                    new XAttribute("name", elem.Element("name").Value)
                )
            );

            return result;
        }

        private XStreamingElement ConvertXml2(XElement original)
        {
            var result = new XStreamingElement
            (
                "newroot",
                from elem in original.Elements()
                select new XElement
                (
                    "newdata",
                    new XAttribute("code", elem.Element("code").Value),
                    new XAttribute("name", elem.Element("name").Value)
                )
            );

            return result;
        }

        private XStreamingElement ConvertXml3()
        {
            var filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "toobig.xml");

            var result = new XStreamingElement
            (
                "newroot",
                from elem in StreamTooBigXml(filePath)
                select new XElement
                (
                    "newdata",
                    new XAttribute("code", elem.Element("code").Value),
                    new XAttribute("name", elem.Element("name").Value)
                )
            );

            return result;
        }

        private IEnumerable<XElement> StreamTooBigXml(string filePath)
        {
            using (var reader = XmlReader.Create(filePath))
            {
                reader.MoveToContent();

                while (reader.Read())
                {
                    if (reader.NodeType != XmlNodeType.Element)
                    {
                        continue;
                    }

                    if (reader.Name != "data")
                    {
                        continue;
                    }

                    //
                    // XElement.ReadFromを利用すると簡単にXElementを取得出来る.
                    //
                    var elem = XNode.ReadFrom(reader) as XElement;
                    if (elem != null)
                    {
                        yield return elem;
                    }
                }
            }
        }
    }
}