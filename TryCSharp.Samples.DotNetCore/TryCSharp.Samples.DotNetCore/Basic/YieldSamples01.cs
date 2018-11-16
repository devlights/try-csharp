using System;
using System.Collections.Generic;
using TryCSharp.Common;

namespace TryCSharp.Samples.Basic
{
    /// <summary>
    ///     yieldキーワードに関するサンプルです。
    /// </summary>
    [Sample]
    public class YieldSamples01 : IExecutable
    {
        //
        // 処理確認用実行メソッド
        //
        public void Execute()
        {
            var col1 = new SampleCollection1();
            foreach (var val in col1)
            {
                Output.WriteLine(val);
            }

            AddNewLine();

            var col2 = new SampleCollection2();
            foreach (var val in col2)
            {
                Output.WriteLine(val);
            }

            AddNewLine();

            var col3 = new SampleCollection3();
            foreach (var val in col3.InOrder)
            {
                Output.WriteLine(val);
            }

            AddNewLine();

            var col4 = new SampleCollection4();
            foreach (var val in col4.InOrder)
            {
                Output.WriteLine(val);
            }

            foreach (var val in col4.ReverseOrder)
            {
                Output.WriteLine(val);
            }

            AddNewLine();

            var col5 = new SampleCollection5();
            foreach (var val in col5.InOrderWithBreak)
            {
                Output.WriteLine(val);
            }

            AddNewLine();

            foreach (var val in Filter(Judge, AddCount(Upper(Range(10, "value")))))
            {
                Output.WriteLine(val);
            }
        }

        //
        // 簡易パイプライン用の各メソッド.
        //
        private IEnumerable<string> Range(int count, string prefix)
        {
            for (var i = 0; i < count; i++)
            {
                yield return string.Format("{0}-{1}", prefix, i);
            }
        }

        private IEnumerable<string> Upper(IEnumerable<string> enumerables)
        {
            foreach (var val in enumerables)
            {
                yield return val.ToUpper();
            }
        }

        private IEnumerable<string> AddCount(IEnumerable<string> enumerables)
        {
            var count = 1;
            foreach (var val in enumerables)
            {
                yield return string.Format("{0}-{1}", val, count++);
            }
        }

        private IEnumerable<string> Filter(Predicate<string> predicate, IEnumerable<string> enumerables)
        {
            foreach (var val in enumerables)
            {
                if (predicate(val))
                {
                    yield return val;
                }
            }
        }

        private void AddNewLine()
        {
            Output.WriteLine(string.Empty);
        }

        private bool Judge(string val)
        {
            var parts = val.Split('-');
            if (parts.Length != 3)
            {
                return false;
            }

            var number = int.Parse(parts[2]);
            if (number%2 == 0)
            {
                return true;
            }

            return false;
        }

        //
        // 最もベーシックな列挙可能クラス.
        //
        private class SampleCollection1
        {
            private readonly List<string> _list;

            public SampleCollection1()
            {
                _list = new List<string>
                {
                    "Value1"
                    , "Value2"
                    , "Value3"
                };
            }

            public IEnumerator<string> GetEnumerator()
            {
                return _list.GetEnumerator();
            }
        }

        //
        // Yieldを利用したパターン１ (IEnumerator<T>を使用)
        //
        private class SampleCollection2
        {
            public IEnumerator<string> GetEnumerator()
            {
                yield return "Value1";
                yield return "Value2";
                yield return "Value3";
            }
        }

        //
        // Yieldを利用したパターン２ (IEnumerable<T>を使用)
        //
        private class SampleCollection3
        {
            public IEnumerable<string> InOrder
            {
                get
                {
                    yield return "Value1";
                    yield return "Value2";
                    yield return "Value3";
                }
            }
        }

        //
        // Yieldを利用したパターン３ (複数のIEnumerable<T>を使用)
        //
        private class SampleCollection4
        {
            private readonly List<string> _list;

            public SampleCollection4()
            {
                _list = new List<string>
                {
                    "Value1"
                    , "Value2"
                    , "Value3"
                };
            }

            public IEnumerable<string> InOrder
            {
                get
                {
                    for (var i = 0; i < _list.Count; i++)
                    {
                        yield return _list[i];
                    }
                }
            }

            public IEnumerable<string> ReverseOrder
            {
                get
                {
                    for (var i = _list.Count - 1; i >= 0; i--)
                    {
                        yield return _list[i];
                    }
                }
            }
        }

        //
        // yield breakを利用したパターン
        //
        private class SampleCollection5
        {
            public IEnumerable<string> InOrderWithBreak
            {
                get
                {
                    yield return "Value1";
                    yield return "Value2";
                    yield return "Value3";

                    // warning CS0162: 到達できないコードが検出されました。
                    //yield return "Value4";
                }
            }
        }
    }
}