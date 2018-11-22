using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using TryCSharp.Common;

namespace TryCSharp.Samples.Linq
{
    /// <summary>
    ///     LINQ to XMLのサンプルです。
    /// </summary>
    /// <remarks>
    ///     LINQ to XMLにてXMLファイルを新規作成するサンプルです.
    /// </remarks>
    [Sample]
    public class LinqSamples56 : IExecutable
    {
        public void Execute()
        {
            //
            // LINQ to XMLにてXMLを新規作成するには
            // 以下のどちらかのインスタンスを作成する必要がある.
            //   ・XDocument
            //   ・XElement
            // 通常、よく利用されるのはXElementの方となる.
            // 保存を行うには、Saveメソッドを利用する.
            // Saveメソッドには、以下のオーバーロードが存在する. (XElement)
            //   Save(Stream)
            //   Save(String)
            //   Save(TextWriter)
            //   Save(XmlWriter)
            //   Save(Stream, SaveOptions)
            //   Save(String, SaveOptions)
            //   Save(TextWriter, SaveOptions)
            //
            var element = new XElement("RootNode",
                from i in Enumerable.Range(1, 10)
                select new XElement("Child", i)
            );

            //
            // Save(Stream)
            //
            using (var stream = new MemoryStream())
            {
                element.Save(stream);

                stream.Position = 0;
                using (var reader = new StreamReader(stream))
                {
                    Output.WriteLine(reader.ReadToEnd());
                }
            }

            Output.WriteLine("===================================");

            //
            // Save(String)
            //
            var tmpFile = Path.GetRandomFileName();
            element.Save(tmpFile);
            Output.WriteLine(File.ReadAllText(tmpFile));
            File.Delete(tmpFile);

            Output.WriteLine("===================================");

            //
            // Save(TextWriter)
            //
            using (var writer = new UTF8StringWriter())
            {
                element.Save(writer);
                Output.WriteLine(writer);
            }

            Output.WriteLine("===================================");

            //
            // Save(XmlWriter)
            //
            using (var backingStore = new UTF8StringWriter())
            {
                using (var xmlWriter = XmlWriter.Create(backingStore, new XmlWriterSettings {Indent = true}))
                {
                    element.Save(xmlWriter);
                }

                Output.WriteLine(backingStore);
            }

            Output.WriteLine("===================================");

            //
            // SaveOptions付きで書き込み.
            //   DisableFormattingを指定すると、出力されるXMLに書式が設定されなくなる.
            //
            using (var writer = new UTF8StringWriter())
            {
                element.Save(writer, SaveOptions.DisableFormatting);
                Output.WriteLine(writer);
            }
        }

        private class UTF8StringWriter : StringWriter
        {
            public override Encoding Encoding => Encoding.UTF8;
        }
    }
}