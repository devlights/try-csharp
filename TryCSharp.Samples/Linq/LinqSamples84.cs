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
    ///     Changing, Changedイベントについてのサンプルです。
    /// </remarks>
    [Sample]
    public class LinqSamples84 : IExecutable
    {
        public void Execute()
        {
            //
            // Changing, Changedイベントは、どちらもXObjectに属するイベントである.
            //

            //
            // Changingイベント
            //   このイベントは、XMLツリーの変更によってのみ発生する。
            //   XMLツリーの作成では発生しないことに注意。
            // イベント引数として、XObjectChangeEventArgsを受け取る.
            // XObjectChangeEventArgsは、ObjectChangeというプロパティを持つ.      
            //
            var root = BuildSampleXml();

            root.Changing += OnNodeChanging;

            var book = root.Elements("Book").First();
            var title = book.Elements("Title").First();

            // 属性値を変更
            //   Changingイベントなので、イベントハンドラ内にて見えるsenderの値は*更新前*の値となる。 (Change)
            book.Attribute("id")!.Value = "updated";
            // 要素の値を変更
            //   Title要素は内部にXTextを持っているので、まずそれが削除される (Remove)
            //   その後、更新後の値を持つXTextが設定される. (Add)
            title.Value = "updated";
            title.Remove();
            // 要素を追加
            //   要素が追加される (Add)
            book.Add(new XElement("newelem", "hogehoge"));

            Output.WriteLine("=====================================");

            //
            // Changed
            //   このイベントは、XMLツリーの変更によってのみ発生する。
            //   XMLツリーの作成では発生しないことに注意。
            // イベント引数として、XObjectChangeEventArgsを受け取る.
            // XObjectChangeEventArgsは、ObjectChangeというプロパティを持つ.
            //
            root = BuildSampleXml();

            root.Changed += OnNodeChanged;

            book = root.Elements("Book").First();
            title = book.Elements("Title").First();

            // 属性値を変更
            //   Changedイベントなので、イベントハンドラ内にて見えるsenderの値は*更新後*の値となる。 (Change)
            book.Attribute("id")!.Value = "updated";
            title.Value = "updated";
            title.Remove();
            book.Add(new XElement("newelem", "hogehoge"));

            Output.WriteLine("=====================================");
        }

        // Changingイベントハンドラ
        private void OnNodeChanging(object? sender, XObjectChangeEventArgs e)
        {
            Output.WriteLine("Changing: sender--{0}:{1}, ObjectChange--{2}", sender!.GetType().Name, sender, e.ObjectChange);
        }

        // Changedイベントハンドラ
        private void OnNodeChanged(object? sender, XObjectChangeEventArgs e)
        {
            Output.WriteLine("Changed: sender--{0}:{1}, ObjectChange--{2}", sender!.GetType().Name, sender, e.ObjectChange);
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