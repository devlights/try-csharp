using System.Linq;
using System.Xml.Linq;
using TryCSharp.Common;

namespace TryCSharp.Samples.Linq
{
    /// <summary>
    ///     LINQ to XMLのサンプルです。
    /// </summary>
    /// <remarks>
    ///     LINQ to XMLにてクエリを使用して対象の要素を取得するサンプルです。
    /// </remarks>
    [Sample]
    public class LinqSamples57 : IExecutable
    {
        public void Execute()
        {
            //
            // LINQ to XMLでは、クエリを利用して特定の要素や属性などを取得する。
            // 取得方法はいろいろあるが、今回はElementsメソッドとElementメソッドを用いて要素の取得を行っている.
            //
            //
            // Books.xmlは、ルート要素がCataglogで内部に複数のBook要素を持っている.
            // 各Book要素は、一つのAuthor要素を持っている.
            //
            // Elementsメソッドは、引数に指定された要素名に合致する要素の集合を返す.
            // Elementメソッドは、引数に指定された要素名に合致する最初の要素を返す.
            //
            var root = XElement.Load(@"xml/Books.xml");
            var query = from book in root.Elements("Book")
                    select book.Element("Author");

            foreach (var author in query)
            {
                Output.WriteLine(author);
            }
        }
    }
}