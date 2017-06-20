using System;
using System.Linq;
using System.Xml.Linq;
using TryCSharp.Common;

namespace TryCSharp.Samples.Linq
{
    /// <summary>
    ///     LINQ to XMLのサンプルです.
    /// </summary>
    /// <remarks>
    ///     属性追加系メソッドのサンプルです.
    /// </remarks>
    [Sample]
    public class LinqSamples65 : IExecutable
    {
        public void Execute()
        {
            //
            // Add(object)
            //   Addメソッドは、要素の設定にも属性の設定にも利用できる.
            //   注意点として、このメソッドは重複した属性を指定した場合に
            //   InvalidOperationExceptionを発生させる。
            //
            var root = BuildSampleXml();
            var elem = root.Elements("Child").First();

            elem.Add(new XAttribute("Id3", 400));
            Output.WriteLine(root);

            try
            {
                //
                // すでに存在する属性をAddしようとすると
                // InvalidOperationExceptionが発生する.
                //
                elem.Add(new XAttribute("Id2", 500));
                Output.WriteLine(root);
            }
            catch (InvalidOperationException invalidOpEx)
            {
                Output.WriteLine("[ERROR] {0}", invalidOpEx.Message);
            }

            Output.WriteLine("=====================================");

            //
            // SetAttributeValue(XName, object)
            //   動作的には、要素の値設定に利用する
            //   SetElementValueメソッドと同じとなる。
            //     - 存在しない属性名称を指定すると追加される
            //     - 存在する属性名称を指定すると更新される
            //     - 値にnullを指定すると属性が削除される
            //
            root = BuildSampleXml();
            elem = root.Elements("Child").First();

            elem.SetAttributeValue("Id3", 400);
            Output.WriteLine(elem);

            elem.SetAttributeValue("Id3", 500);
            Output.WriteLine(elem);

            elem.SetAttributeValue("Id3", null);
            Output.WriteLine(elem);

            Output.WriteLine(root);
            Output.WriteLine("=====================================");
        }

        private XElement BuildSampleXml()
        {
            return XElement.Parse("<Root><Child Id=\"100\" Id2=\"200\"><Value Id=\"300\">hoge</Value></Child></Root>");
        }
    }
}