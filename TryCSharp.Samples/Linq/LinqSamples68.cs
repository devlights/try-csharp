using System.Linq;
using System.Xml.Linq;
using TryCSharp.Common;

namespace TryCSharp.Samples.Linq
{
    /// <summary>
    ///     LINQ to XMLのサンプルです.
    /// </summary>
    /// <remarks>
    ///     属性置換系メソッドのサンプルです.
    /// </remarks>
    [Sample]
    public class LinqSamples68 : IExecutable
    {
        public void Execute()
        {
            // 
            // ReplaceAttributes
            //   現在の要素に付属している属性を一括で置換する。
            //   ノードの置換に利用するReplaceNodesメソッドと同じ要領で
            //   利用できる。（クエリを利用しながら、置換用のシーケンスを作成する)
            //   
            var root = BuildSampleXml();
            var elem = root.Elements("Child").First();

            elem.ReplaceAttributes
            (
                from attr in elem.Attributes()
                where attr.Name.ToString().EndsWith("d")
                select new XAttribute($"{attr.Name}-Update", attr.Value)
            );

            Output.WriteLine(root);
            Output.WriteLine("=====================================");
        }

        private XElement BuildSampleXml()
        {
            return XElement.Parse("<Root><Child Id=\"100\" Id2=\"200\"><Value Id=\"300\">hoge</Value></Child></Root>");
        }
    }
}