using System;
using System.Linq;
using System.Xml.Linq;
using TryCSharp.Common;

namespace TryCSharp.Samples.Linq
{
    /// <summary>
    ///     LINQ to XMLのサンプルです.
    /// </summary>
    /// <remarks>
    ///     要素追加系メソッドのサンプルです.
    /// </remarks>
    [Sample]
    public class LinqSamples60 : IExecutable
    {
        public void Execute()
        {
            //
            // Add(object)
            //   名前の通り、現在の要素に指定された要素を追加する.
            //   追加される位置は、その要素の末尾となる
            //
            var root = BuildSampleXml();
            var newElem1 = new XElement("NewElement", "hehe");
            root.Add(newElem1);

            Output.WriteLine(root);
            Output.WriteLine("=====================================");

            //
            // AddAfterSelf(object)
            //   現在の要素の後ろに、指定された要素を追加する.
            //   追加される位置は、自分自身の中ではなく外となる事に注意.
            //   尚、ルート要素に対して本メソッドを呼び出すと親が存在しないので
            //   InvalidOperationExceptionが発生する.
            //   (発生する例外がXmlExceptionでは無いことに注意)
            //
            root = BuildSampleXml();
            var newElem4 = new XElement("AfterElement", "AfterSelf");

            try
            {
                // ルート要素に対して、自身の後ろに要素を追加しようとするので
                // エラーとなる。XmlExceptionでは無いことに注意.
                root.AddAfterSelf(newElem4);
                Output.WriteLine(root);
            }
            catch (InvalidOperationException invalidEx)
            {
                Output.WriteLine("[ERROR] {0}", invalidEx.Message);
            }
            finally
            {
                Output.WriteLine("=====================================");
            }

            root.Elements().First().AddAfterSelf(newElem4);

            Output.WriteLine(root);
            Output.WriteLine("=====================================");

            //
            // AddBeforeSelf(object)
            //   現在の要素の前に、指定された要素を追加する.
            //   追加される位置は、自分自身の中ではなく外となる事に注意.
            //   尚、ルート要素に対して本メソッドを呼び出すと親が存在しないので
            //   InvalidOperationExceptionが発生する.
            //   (発生する例外がXmlExceptionでは無いことに注意)
            //
            root = BuildSampleXml();
            var newElem5 = new XElement("BeforeElement", "BeforeSelf");

            try
            {
                // ルート要素に対して、自身の前に要素を追加しようとするので
                // エラーとなる。XmlExceptionでは無いことに注意.
                root.AddBeforeSelf(newElem5);
                Output.WriteLine(root);
            }
            catch (InvalidOperationException invalidEx)
            {
                Output.WriteLine("[ERROR] {0}", invalidEx.Message);
            }
            finally
            {
                Output.WriteLine("=====================================");
            }

            root.Elements().First().AddBeforeSelf(newElem5);

            Output.WriteLine(root);
            Output.WriteLine("=====================================");

            //
            // AddFirst(object)
            //   現在要素の先頭子要素として、指定された要素を追加する.
            //   追加される位置は、自分自身の中となる。（先頭要素）
            //
            root = BuildSampleXml();
            var newElem6 = new XElement("FirstElement", "First");

            root.AddFirst(newElem6);

            Output.WriteLine(root);
            Output.WriteLine("=====================================");

            root = BuildSampleXml();
            root.Elements().First().AddFirst(newElem6);

            Output.WriteLine(root);
            Output.WriteLine("=====================================");
        }

        private XElement BuildSampleXml()
        {
            return new XElement("Root",
                new XElement("Child",
                    new XAttribute("Id", 100),
                    new XElement("Value", "hoge")
                )
            );
        }
    }
}