using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using TryCSharp.Common;
// ReSharper disable PossibleNullReferenceException

namespace TryCSharp.Samples.Linq
{
    /// <summary>
    ///     LINQ to XMLのサンプルです.
    /// </summary>
    /// <remarks>
    ///     LINQ to XMLのアノテーション機能についてのサンプルです。
    /// </remarks>
    [Sample]
    public class LinqSamples86 : IExecutable
    {
        public void Execute()
        {
            // LINQ to XMLでは、それぞれのデータに対して
            // アノテーションを付与することが出来る。
            //
            //  XObject.AddAnnotation
            //  XObject.Annotation(Type)
            //         .Annotation<T>()
            //         .Annotations(Type)
            //         .Annotations<T>()
            //  XObject.RemoveAnnotations(Type)
            //         .RemoveAnnotations<T>()
            //
            // アノテーションは、LINQ to XMLで処理している間のみ有効なデータ.
            // 永続化されず、ToStringにも表示されない
            // Tagプロパティのような使い方が出来る.
            //
            // コレクションを扱うAnnotationsメソッドのコードは割愛
            //
            var root = BuildSampleXml();
            var elem = root.Descendants("Price").Last();

            //
            // アノテーションを追加.
            //
            elem.AddAnnotation(new Tag("Tag Value"));

            //
            // アノテーションが付いている要素を列挙してみる.
            //
            foreach (var item in QueryHasAnnotation(root))
            {
                Output.WriteLine(item);
                Output.WriteLine(item.Annotation<Tag>()!.Value);
            }

            //
            // アノテーションを削除
            //
            elem.RemoveAnnotations<Tag>();

            Output.WriteLine(QueryHasAnnotation(root).Count());

            //
            // アノテーションを付与した状態でToStringしてみる
            //
            elem.AddAnnotation(new Tag("Tag Value"));
            Output.WriteLine(root);
        }

        private IEnumerable<XElement> QueryHasAnnotation(XElement root)
        {
            var query = from el in root.Descendants()
                    let an = el.Annotation<Tag>()
                    where an != null
                    select el;

            return query;
        }

        private XElement BuildSampleXml()
        {
            //
            // サンプルXMLファイル
            //  see: http://msdn.microsoft.com/ja-jp/library/vstudio/ms256479(v=vs.90).aspx
            //
            return XElement.Load(@"xml/Books.xml");
        }

        private class Tag
        {
            public Tag(string value)
            {
                Value = value;
            }

            public string Value { get; }
        }
    }
}