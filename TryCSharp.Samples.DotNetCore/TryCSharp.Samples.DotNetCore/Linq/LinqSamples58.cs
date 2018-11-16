using System.Xml.Linq;
using TryCSharp.Common;

namespace TryCSharp.Samples.Linq
{
    /// <summary>
    ///     LINQ to XMLのサンプルです.
    /// </summary>
    /// <remarks>
    ///     要素のクローンとアタッチについてのサンプルです.
    /// </remarks>
    [Sample]
    public class LinqSamples58 : IExecutable
    {
        public void Execute()
        {
            //
            // 親要素を持たない要素を作成し、特定のXMLツリーの中に組み込む. (アタッチ)
            //
            var noParent = new XElement("NoParent", true);
            var tree1 = new XElement("Parent", noParent);

            var noParent2 = tree1.Element("NoParent");
            Output.WriteLine("参照が同じ？ = {0}", noParent == noParent2);
            Output.WriteLine(tree1);

            // 値を変更して確認.
            noParent.SetValue(false);
            Output.WriteLine(noParent.Value);
            Output.WriteLine(tree1.Element("NoParent")?.Value);

            Output.WriteLine("==========================================");

            //
            // 親要素を持つ要素を作成し、特定のXMLツリーの中に組み込む. (クローン)
            //
            var origTree = new XElement("Parent", new XElement("WithParent", true));
            var tree2 = new XElement("Parent", origTree.Element("WithParent"));

            Output.WriteLine("参照が同じ？ = {0}", origTree.Element("WithParent") == tree2.Element("WithParent"));
            Output.WriteLine(tree2);

            // 値を変更して確認
            origTree.Element("WithParent")?.SetValue(false);
            Output.WriteLine(origTree.Element("WithParent")?.Value);
            Output.WriteLine(tree2.Element("WithParent")?.Value);
        }
    }
}