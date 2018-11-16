using System.Linq;
using System.Xml.Linq;
using TryCSharp.Common;

namespace TryCSharp.Samples.Linq
{
    /// <summary>
    ///     XDocumentクラスについてのサンプルです。
    /// </summary>
    [Sample]
    public class XDocumentSamples02 : IExecutable
    {
        public void Execute()
        {
            var xmlStrings =
                    @"<Persons>
              <Person>
                <Name>gsf_zero1</Name>
                <Age>30</Age>
              </Person>
            </Persons>";

            //
            // Parseメソッドを利用する事で、文字列から直接XDocumentを
            // 構築することが出来る。
            //
            var doc = XDocument.Parse(xmlStrings, LoadOptions.None);

            //
            // ノードを置換.
            //
            var name = (from element in doc.Descendants("Name") select element).First();

            name.ReplaceWith(new XElement("名前", name.Value));
            Output.WriteLine(doc);
        }
    }
}