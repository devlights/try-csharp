using System.Linq;
using System.Xml.Linq;
using TryCSharp.Common;

namespace TryCSharp.Samples.Linq
{
    /// <summary>
    ///     LINQ to XMLのサンプルです.
    /// </summary>
    /// <remarks>
    ///     ドキュメント順に並び替え(InDocumentOrder)のサンプルです.
    /// </remarks>
    [Sample]
    public class LinqSamples81 : IExecutable
    {
        public void Execute()
        {
            //
            // InDocumentOrder<T> where T : XNode (拡張メソッド)
            //   元のシーケンスをドキュメント内の順序に従うよう並び替える.
            //
            var root = BuildSampleXml();
            var reversed = root.Elements().Reverse().ToArray();

            // Reverseしているので逆順で表示される
            foreach (var elem in reversed)
            {
                Output.WriteLine(elem);
            }

            Output.WriteLine("=====================================");

            // InDocumentOrderを行うことにより、要素が正しい順序に並び替えられる
            foreach (var elem in reversed.InDocumentOrder())
            {
                Output.WriteLine(elem);
            }

            Output.WriteLine("=====================================");

            // 特定の要素をピックアップして、シーケンス作成。わざと順序を変えている.
            XElement[] elemList = {root.Descendants("Title").Last(), root.Descendants("Title").First()};

            // そのまま表示すると当然順序は変わったまま
            foreach (var elem in elemList)
            {
                Output.WriteLine(elem);
            }

            Output.WriteLine("=====================================");

            // InDocumentOrderを付けることにより、ドキュメント内の正しい順序に並び替えられる
            foreach (var elem in elemList.InDocumentOrder())
            {
                Output.WriteLine(elem);
            }
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