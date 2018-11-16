using System.Linq;
using System.Xml.Linq;
using TryCSharp.Common;

namespace TryCSharp.Samples.Linq
{
    /// <summary>
    ///     LINQ to XMLのサンプルです。
    /// </summary>
    /// <remarks>
    ///     文字列からXDocumentオブジェクトを構築するサンプルです。
    /// </remarks>
    [Sample]
    public class LinqSamples51 : IExecutable
    {
        public void Execute()
        {
            //
            // LINQ to XMLは、LINQを利用してXMLを扱うためのAPIである。
            // インメモリXMLノードの管理及びクエリ実行が可能。
            //
            // LINQ to XMLでは、まず最初にXDocumentオブジェクトまたはXElementを構築する.
            // XDocumentオブジェクトの構築には
            //   ・ファイルから読み込み
            //   ・文字列から構築
            //   ・関数型構築
            //   ・新規作成
            // の方法がある。
            //
            // 本サンプルでは、文字列からXDocumentオブジェクトを取得している.
            //

            //
            // 文字列からXDocumentを構築するには、Parseメソッドを利用する.
            // LoadOptionには、以下の値が設定出来る.
            //   None              : 意味の無い空白の削除、及び、ベースURIと行情報の読み込み無し
            //   PreserveWhitespace: 意味の無い空白保持
            //   SetBaseUri        : ベースURIの保持
            //   SetLineInfo       : 行情報の保持
            //
            var doc = XDocument.Parse(MakeSampleXml(), LoadOptions.None);

            //
            // 特定の要素の表示.
            //
            var query = from elem in doc.Descendants("Person")
                    let name = elem?.Attribute("name")?.Value
                    where name.StartsWith("b")
                    select new {name};

            foreach (var item in query)
            {
                Output.WriteLine(item);
            }
        }

        private string MakeSampleXml()
        {
            var xmlStrings =
                    @"<?xml version='1.0' encoding='UTF-8' standalone='yes'?>
              <Persons>
                <Person name='foo'/>
                <Person name='bar'/>
                <Person name='baz'/>
              </Persons>";

            return xmlStrings;
        }
    }
}