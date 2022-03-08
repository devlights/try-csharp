using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using TryCSharp.Common;
// ReSharper disable MemberCanBePrivate.Local

namespace TryCSharp.Samples.Advanced
{
    /// <summary>
    /// DebuggerDisplay属性についてのサンプルです。
    /// </summary>
    [SuppressMessage("ReSharper", "HeapView.BoxingAllocation")]
    public class DebuggerDisplayAttributeSamples01 : IExecutable
    {
        public void Execute()
        {
            var rnd = new Random();

            // --------------------------------------------------------
            // このクラスには DebuggerDisplay属性を付与している
            // ので、デバッグ時に属性に指定した内容が表示される
            // --------------------------------------------------------
            var data01 = new WithDebuggerDisplayAttr
            {
                Id = 1,
                Name = $"Name-{rnd.Next()}",
                Value = $"Val-{rnd.Next()}"
            };

            // --------------------------------------------------------
            // このクラスには DebuggerDisplay属性を付与していない
            // ので、デバッグ時に通常通りFQCNが表示される
            // --------------------------------------------------------
            var data02 = new WithoutDebuggerDisplayAttr
            {
                Id = 2,
                Name = $"Name-{rnd.Next()}",
                Value = $"Val-{rnd.Next()}"
            };

            Output.WriteLine(data01);
            Output.WriteLine(data02);
        }

        /// <summary>
        /// DebuggerDisplay属性を付与したクラス
        /// </summary>
        [DebuggerDisplay("{Id}: Name={Name}, Value={Value}")]
        private class WithDebuggerDisplayAttr
        {
            public int Id { get; set; }
            public string? Name { get; set; }
            public string? Value { get; set; }
        }

        /// <summary>
        /// DebuggerDisplay属性を付与していないクラス
        /// </summary>
        private class WithoutDebuggerDisplayAttr
        {
            public int Id { get; set; }
            public string? Name { get; set; }
            public string? Value { get; set; }
        }
    }
}