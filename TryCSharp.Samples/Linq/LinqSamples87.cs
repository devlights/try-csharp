using System.Xml;
using System.Xml.Linq;
using TryCSharp.Common;

namespace TryCSharp.Samples.Linq
{
    /// <summary>
    ///     LINQ to XMLのサンプルです.
    /// </summary>
    /// <remarks>
    ///     IXmlLineInfoインターフェースから行番号を取得するサンプルです。
    /// </remarks>
    [Sample]
    public class LinqSamples87 : IExecutable
    {
        public void Execute()
        {
            //
            // IXmlLineInfo
            //   IXmlLineInfoはXObjectにて明示的実装が行われているインターフェースである.
            //   IXmlLineInfo型にキャストすることで、値を取得できるようになる。
            //   このインターフェースには以下のメソッドとプロパティが定義されている.
            //     ■ HasLineInfoメソッド：行情報を持っているか否か.
            //     ■ LineNumberプロパティ：行番号
            //     ■ LinePositionプロパティ：行位置
            //
            //   尚、行番号などを取得する場合は、予め対象のXMLをロードする際に
            //   LoadOptions.SetLineInfoを設定してロードしておく必要がある。
            //   LoadOptions.SetLineInfoを設定せずにロードした状態で、IXmlLineInfoの
            //   情報を取得しても行番号は常に0となる.
            //
            var root = BuildSampleXml();

            foreach (var elem in root.Descendants())
            {
                var info = (IXmlLineInfo) elem;

                Output.WriteLine(elem.Name);
                Output.WriteLine("\tHasLineInfo  == {0}", info.HasLineInfo());
                Output.WriteLine("\tLineNumber   == {0}", info.LineNumber);
                Output.WriteLine("\tLinePosition == {0}", info.LinePosition);
            }

            Output.WriteLine("=========================================================");

            //
            // 要素を追加.
            //
            var newElem = new XElement("NewElement", "NewElementValue");
            root.Add(newElem);

            //
            // 再度行番号を表示.
            //   新規追加した要素からは行番号情報が取得出来ない.
            //
            foreach (var elem in root.Descendants())
            {
                var info = (IXmlLineInfo) elem;

                Output.WriteLine(elem.Name);
                Output.WriteLine("\tHasLineInfo  == {0}", info.HasLineInfo());
                Output.WriteLine("\tLineNumber   == {0}", info.LineNumber);
                Output.WriteLine("\tLinePosition == {0}", info.LinePosition);
            }

            Output.WriteLine("=========================================================");
        }

        private XElement BuildSampleXml()
        {
            //
            // サンプルXMLファイル
            //  see: http://msdn.microsoft.com/ja-jp/library/vstudio/ms256479(v=vs.90).aspx
            //
            // 行番号を取得するためにLoadOptions.SetLineInfoを設定.
            return XElement.Load(@"xml/Books.xml", LoadOptions.SetLineInfo);
        }
    }
}