using System.Linq;
using System.Xml.Linq;
using TryCSharp.Common;

namespace TryCSharp.Samples.Linq
{
    /// <summary>
    ///     LINQ to XMLのサンプルです.
    /// </summary>
    /// <remarks>
    ///     属性削除系メソッドのサンプルです.
    /// </remarks>
    [Sample]
    public class LinqSamples67 : IExecutable
    {
        public void Execute()
        {
            //
            // XAttribute.Remove
            //   現在の属性を削除する.
            //
            var root = BuildSampleXml();
            var elem = root.Elements("Child").First();

            var attr = elem.Attribute("Id");
            attr?.Remove();

            //
            // 削除後の属性に値を設定しても、反映されない.
            //
            if (attr != null)
            {
                attr.Value = "999";
            }

            Output.WriteLine(root);
            Output.WriteLine("=====================================");

            //
            // SetAttributeValue
            //   属性の値を設定するメソッドであるが
            //   値にnullを指定することで、属性を削除することができる.
            //
            root = BuildSampleXml();
            elem = root.Elements("Child").First();

            elem.SetAttributeValue("Id", null);

            Output.WriteLine(root);
            Output.WriteLine("=====================================");

            //
            // RemoveAttributes
            //   現在の要素に存在する属性を全て削除する.
            //
            root = BuildSampleXml();
            elem = root.Elements("Child").First();

            elem.RemoveAttributes();

            Output.WriteLine(root);
            Output.WriteLine("=====================================");
        }

        private XElement BuildSampleXml()
        {
            return XElement.Parse("<Root><Child Id=\"100\" Id2=\"200\"><Value Id=\"300\">hoge</Value></Child></Root>");
        }
    }
}