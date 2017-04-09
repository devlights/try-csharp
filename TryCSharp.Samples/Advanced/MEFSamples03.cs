using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using TryCSharp.Common;

namespace TryCSharp.Samples.Advanced
{
    /// <summary>
    ///     MEFについてのサンプルです。
    /// </summary>
    [Sample]
    // ReSharper disable once InconsistentNaming
    public class MEFSamples03 : IExecutable
    {
        // コンテナ.
        private CompositionContainer _container;

        // Importパート (複数のExportを受け付け、且つ、メタデータ有り）
        //
        // 通常、複数のExportを受け付ける場合は以下の書式で宣言する。
        //   IEnumerable<Lazy<T>>
        //
        // Lazy<T>を利用する事により、遅延ローディングが可能となる。
        // (利用しないExportパートが合成時に全てインスタンス化されるのを防ぐ）
        //
        // また、メタデータを利用する場合は以下のようになる。
        //   IEnumerable<Lazy<T, TMetaData>>
        //
        // 尚、明示的にnullを初期値として指定しているのは、そのままだとコンパイラによって警告扱いされるため
        [ImportMany(typeof(IExporter))] private readonly IEnumerable<Lazy<IExporter, IExporterMetadata>> _exporters =
            null;

        public void Execute()
        {
            //
            // カタログ構築.
            //  AggregateCatalogは、複数のCatalogを一つにまとめる役割を持つ。
            //
            var catalog = new AggregateCatalog();
            // AssemblyCatalogを利用して、自分自身のアセンブリをカタログに追加.
            catalog.Catalogs.Add(new AssemblyCatalog(typeof(MEFSamples01).Assembly));

            //
            // コンテナを構築.
            //
            _container = new CompositionContainer(catalog);
            try
            {
                // 合成実行.
                _container.ComposeParts(this);

                // 実行.
                foreach (var lazyObj in _exporters)
                {
                    //
                    // メタデータを調べ、合致したもののみを実行する.
                    // Lazy<T, TMetadata>.Valueを呼ばない限りインスタンスは作成されない。
                    //
                    if (lazyObj.Metadata.Symbol == "SECOND")
                    {
                        Output.WriteLine(lazyObj.Value.Name);
                    }
                }
            }
            catch (CompositionException ex)
            {
                // 合成に失敗した場合.
                Output.WriteLine(ex.ToString());
            }

            if (_container != null)
            {
                _container.Dispose();
            }
        }

        // Export用のインターフェース
        public interface IExporter
        {
            string Name { get; }
        }

        // Exporter用のメタデータインターフェース
        public interface IExporterMetadata
        {
            string Symbol { get; }
        }

        [Export(typeof(IExporter))]
        [ExportMetadata("Symbol", "FIRST")]
        public class FirstExporter : IExporter
        {
            public string Name
            {
                get { return "☆☆ FIRST EXPORTER ☆☆"; }
            }
        }

        [Export(typeof(IExporter))]
        [ExportMetadata("Symbol", "SECOND")]
        public class SecondExporter : IExporter
        {
            public string Name
            {
                get { return "☆☆ SECOND EXPORTER ☆☆"; }
            }
        }

        [Export(typeof(IExporter))]
        [ExportMetadata("Symbol", "THIRD")]
        public class ThirdExporter : IExporter
        {
            public string Name
            {
                get { return "☆☆ THIRD EXPORTER ☆☆"; }
            }
        }
    }
}