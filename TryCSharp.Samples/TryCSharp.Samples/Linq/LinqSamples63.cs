using System.Linq;
using System.Xml.Linq;
using TryCSharp.Common;

namespace TryCSharp.Samples.Linq
{
    /// <summary>
    ///     LINQ to XMLのサンプルです.
    /// </summary>
    /// <remarks>
    ///     要素置換系メソッドのサンプルです.
    /// </remarks>
    [Sample]
    public class LinqSamples63 : IExecutable
    {
        public void Execute()
        {
            //
            // ReplaceWith(object)
            //   現在の要素を指定した要素で置き換える.
            //   
            var root = BuildSampleXml();
            var elem = root.Descendants("Value").First();

            elem.ReplaceWith(new XElement("Value2", "replaced"));

            Output.WriteLine(root);
            Output.WriteLine("=====================================");

            //
            // ReplaceNodes(object)
            //   現在の要素の子ノードを指定された要素で置き換える.
            //   このメソッドはスナップショットセマンティクスという方法で置換処理を行う。
            //   スナップショットセマンティクスを用いている場合、置換前に事前に置き換える内容のコピーを
            //   作成してから、置換処理を行うため、現在の要素の状態を元に置換処理を実装することができる。
            //   (例： LINQ to XMLを利用して、現在の要素内容をクエリし、その結果を置換後として利用する。）
            //
            //   尚、このメソッドは属性を削除しない.
            //
            root = BuildSampleXml();
            elem = root.Elements("Child").First();

            elem.Add
            (
                from x in Enumerable.Range(1, 5)
                select new XElement("Value", x)
            );

            elem.ReplaceNodes
            (
                from e in elem.Elements()
                where ToInt(e.Value) >= 3
                select e
            );

            Output.WriteLine(root);
            Output.WriteLine("=====================================");

            //
            // ReplaceAll(object)
            //   現在の要素の子ノードと属性を削除し、指定された要素で置き換える.
            //   このメソッドは、スナップショットセマンティクスという方法で置換処理を行う。
            //   スナップショットセマンティクスを用いている場合、置換前に事前に置き換える内容のコピーを
            //   作成してから、置換処理を行うため、現在の要素の状態を元に置換処理を実装することができる。
            //   (例： LINQ to XMLを利用して、現在の要素内容をクエリし、その結果を置換後として利用する。)
            //
            //   尚、このメソッドは属性を削除するので利用時には注意が必要。
            //
            root = BuildSampleXml();
            elem = root.Elements("Child").First();

            elem.Add
            (
                from x in Enumerable.Range(1, 5)
                select new XElement("Value", x)
            );

            elem.ReplaceAll
            (
                from e in elem.Elements()
                where ToInt(e.Value) >= 3
                select e
            );

            Output.WriteLine(root);
            Output.WriteLine("=====================================");
        }

        private int ToInt(string value)
        {
            int tmp;
            if (!int.TryParse(value, out tmp))
            {
                return -1;
            }

            return tmp;
        }

        private XElement BuildSampleXml()
        {
            return XElement.Parse("<Root><Child Id=\"100\"><Value>hoge</Value></Child></Root>");
        }
    }
}