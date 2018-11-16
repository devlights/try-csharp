using System.Linq;
using System.Xml.Linq;
using TryCSharp.Common;

namespace TryCSharp.Samples.Linq
{
    /// <summary>
    ///     LINQ to XMLのサンプルです.
    /// </summary>
    /// <remarks>
    ///     ナビゲーション(ElementsAfterSelf, ElementsBeforeSelf)のサンプルです.
    /// </remarks>
    [Sample]
    public class LinqSamples79 : IExecutable
    {
        public void Execute()
        {
            //
            // ElementsAfterSelf(), ElementsAfterSelf(XName)
            //   現在の要素の後ろにある兄弟要素を取得する.
            //   自分自身は含まない.
            //
            //   引数無し版のメソッドの方は、予想通りの動きをするが
            //   XNameを受け取るオーバーロードの方は、AncestorsAndSelf(XName)と
            //   同じ変な挙動をする。 (MSDNに乗っているサンプルでも、兄弟要素が表示されていない)
            //   謎,,,。
            //
            var root = BuildSampleXml();
            var startingPoint = root.Descendants("Book").First();

            // 最初のBook要素の後ろにある兄弟要素が表示される.
            foreach (var elem in startingPoint.ElementsAfterSelf())
            {
                Output.WriteLine(elem);
            }

            Output.WriteLine("=====================================");

            root = BuildSampleXml();
            startingPoint = root.Descendants("Title").Last();

            // Titleの後ろにある兄弟要素が表示される
            foreach (var elem in startingPoint.ElementsAfterSelf())
            {
                Output.WriteLine(elem);
            }


            Output.WriteLine("=====================================");

            root = BuildSampleXml();
            startingPoint = root.Descendants("Title").Last();

            // 何故か、引数に指定したGenreしか表示されない？？
            // AncestorsAndSelf(XName)とかと同じ挙動.
            foreach (var elem in startingPoint.ElementsAfterSelf("Genre"))
            {
                Output.WriteLine(elem);
            }

            Output.WriteLine("=====================================");

            //
            // ElementsBeforeSelf(), ElementsBeforeSelf(XName)
            //   現在の要素の前にある兄弟要素を取得する.
            //   自分自身は含まない.
            //
            //   引数無し版のメソッドの方は、予想通りの動きをするが
            //   XNameを受け取るオーバーロードの方は、AncestorsAndSelf(XName)と
            //   同じ変な挙動をする。 (MSDNに乗っているサンプルでも、兄弟要素が表示されていない)
            //   謎,,,。
            //
            root = BuildSampleXml();
            startingPoint = root.Descendants("PublishDate").Last();

            foreach (var elem in startingPoint.ElementsBeforeSelf())
            {
                Output.WriteLine(elem);
            }

            Output.WriteLine("=====================================");

            root = BuildSampleXml();
            startingPoint = root.Descendants("Description").Last();

            // 何故か、引数に指定したPublishDateしか表示されない？？
            // AncestorsAndSelf(XName)とかと同じ挙動.
            foreach (var elem in startingPoint.ElementsBeforeSelf("PublishDate"))
            {
                Output.WriteLine(elem);
            }

            Output.WriteLine("=====================================");
        }

        private XElement BuildSampleXml()
        {
            //
            // サンプルXMLファイル
            //  see: http://msdn.microsoft.com/ja-jp/library/vstudio/ms256479(v=vs.90).aspx
            //
            return XElement.Load(@"xml/Books.xml");
        }
    }
}