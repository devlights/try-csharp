using System.Linq;
using System.Xml.Linq;
using TryCSharp.Common;

namespace TryCSharp.Samples.Linq
{
    /// <summary>
    ///     LINQ to XMLのサンプルです.
    /// </summary>
    /// <remarks>
    ///     ナビゲーション(DescendantsAndSelf, AncestorsAndSelfメソッド)のサンプルです.
    /// </remarks>
    [Sample]
    public class LinqSamples77 : IExecutable
    {
        public void Execute()
        {
            //
            // DescendantsAndSelf
            //   自身とその子要素を取得するためのメソッド.
            //   引数が無い版とXNameを指定する版が存在する。
            //   引数無し版は、意図した通りの結果を返してくれるが
            //   XNameを指定する版のメソッドは、子要素を含めてくれない・・・。
            //   (指定の仕方が間違っているのか？ 使い方が間違っているのか？)
            //
            //   恐らく、指定の仕方が間違っているのだと思うが、普段利用しないメソッドなので、これで一旦完了とする.
            //
            var root = BuildSampleXml();
            var startingPoint = root.Descendants("Customer").First();
            var descendantsAndSelf = startingPoint.DescendantsAndSelf();

            //
            // AndSelf付きのメソッドを利用しているので、Customer自身も結果に含まれる.
            //
            foreach (var elem in descendantsAndSelf)
            {
                Output.WriteLine(elem);
            }

            Output.WriteLine("=====================================");

            //
            // AndSelfを付けていないので、Customer自身は含まれない.
            //
            foreach (var elem in startingPoint.Descendants())
            {
                Output.WriteLine(elem);
            }

            Output.WriteLine("=====================================");

            //
            // XName付きのオーバーロードを呼び出すと、予想と違う結果となる
            // Customer要素が含まれない. (??)
            //
            // MSDNの説明には、「この要素とこの要素のすべての子要素」と記載があるが・・・。
            // (MSDNのメソッドページにあるサンプルプログラムの結果も、以下の結果と同じになっている)
            //
            //   恐らく、指定の仕方が間違っているのだと思うが、普段利用しないメソッドなので、これで一旦完了とする.
            //
            root = BuildSampleXml();
            startingPoint = root.Descendants("Customer").First();
            descendantsAndSelf = startingPoint.DescendantsAndSelf("FullAddress");

            foreach (var elem in descendantsAndSelf)
            {
                Output.WriteLine(elem);
            }

            Output.WriteLine("=====================================");

            //
            // AncestorsAndSelf
            //   自身とその先祖を取得するためのメソッド.
            //   引数が無い版とXNameを指定する版が存在する。
            //   引数無し版は、意図した通りの結果を返してくれるが
            //   XNameを指定する版のメソッドは、先祖を含めてくれない・・・。
            //   (指定の仕方が間違っているのか？ 使い方が間違っているのか？)
            //
            root = BuildSampleXml2();
            startingPoint = root.Descendants("Price").First();

            var ancestorsAndSelf = startingPoint.AncestorsAndSelf();

            //
            // AndSelf付きのメソッドを利用しているので、Price自身も結果に含まれる.
            //
            foreach (var elem in ancestorsAndSelf)
            {
                Output.WriteLine(elem);
            }

            Output.WriteLine("=====================================");

            //
            // Price自身は含まれない
            //
            foreach (var elem in startingPoint.Ancestors())
            {
                Output.WriteLine(elem);
            }

            Output.WriteLine("=====================================");

            //
            // XName付きのオーバーロードを呼び出すと、予想と違う結果となる
            // Price要素が含まれない. (??)
            //
            // MSDNの説明には、「この要素とこの要素の先祖」と記載があるが・・・。
            // (MSDNのメソッドページにあるサンプルプログラムの結果も、以下の結果と同じになっている)
            //
            root = BuildSampleXml2();
            startingPoint = root.Descendants("Price").First();
            ancestorsAndSelf = startingPoint.AncestorsAndSelf("Book");

            foreach (var elem in ancestorsAndSelf)
            {
                Output.WriteLine(elem);
            }
        }

        private XElement BuildSampleXml()
        {
            //
            // サンプルXMLファイル
            //  see: http://msdn.microsoft.com/ja-jp/library/vstudio/bb387025.aspx
            //
            return XElement.Load(@"xml/CustomersOrders.xml");
        }

        private XElement BuildSampleXml2()
        {
            //
            // サンプルXMLファイル
            //  see: http://msdn.microsoft.com/ja-jp/library/vstudio/ms256479(v=vs.90).aspx
            //
            return XElement.Load(@"xml/Books.xml");
        }
    }
}