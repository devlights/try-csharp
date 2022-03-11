using System;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using TryCSharp.Common;

namespace TryCSharp.Samples.IO
{
    /// <summary>
    ///     System.IO.Compression.ZipFileクラスのサンプルです。
    /// </summary>
    /// <remarks>
    ///     ZipFileクラスは、.NET Framework 4.5で追加されたクラスです。
    ///     このクラスを利用するには、「System.IO.Compression.FileSystem.dll」を
    ///     参照設定に追加する必要があります。
    ///     このクラスは、Metroアプリでは利用できません。
    ///     Metroアプリでは、代わりにZipArchiveクラスを利用します。
    /// </remarks>
    [Sample]
    public class ZipFileSamples02 : IExecutable
    {
        private string _zipFilePath;

        private string DesktopPath => Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

        public ZipFileSamples02()
        {
            _zipFilePath = string.Empty;
        }
        
        public void Execute()
        {
            //
            // ZipFileクラスの以下のメソッドを利用すると、既存のZIPファイルを開く事が出来る。
            //   ・OpenRead
            //   ・Open(string, ZipArchiveMode)
            //   ・Open(string, ZipArchiveMode, Encoding)
            // どのメソッドも、戻り値としてZipArchiveクラスのインスタンスを返す。
            // 実際にZIPファイル内のエントリ取得は、ZipArchiveから行う.
            // ZipArchiveクラスは、IDisposableを実装しているのでusingブロックで
            // 利用するのが好ましい。
            //
            // 尚、ZipArchiveクラスを利用する場合、参照設定に
            //   System.IO.Compression.dll
            // を追加する必要がある。
            //
            Prepare();

            //
            // OpenRead
            //
            using (var archive = ZipFile.OpenRead(_zipFilePath))
            {
                archive.Entries.ToList().ForEach(PrintEntry);
            }

            //
            // Open(string, ZipArchiveMode)
            //
            using (var archive = ZipFile.Open(_zipFilePath, ZipArchiveMode.Read))
            {
                //
                // ZipArchive.Entriesプロパティからは、ReadOnlyCollection<ZipArchiveEntry>が取得できる。
                // 1エントリの情報は、ZipArchiveEntryから取得できる。
                //
                // ZipArchiveEntryには、Nameというプロパティが存在し、このプロパティから実際のファイル名を取得できる。
                // また、Lengthプロパティより圧縮前のファイルサイズが取得できる。圧縮後のサイズは、CompressedLengthから取得できる。
                // エントリの内容を読み出すには、ZipArchiveEntry.Openメソッドを利用する。
                //
                archive.Entries.ToList().ForEach(PrintEntry);
            }

            //
            // Open(string, ZipArchiveMode, Encoding)
            //   テキストファイルのみ、中身を読み出して出力.
            //
            using (var archive = ZipFile.Open(_zipFilePath, ZipArchiveMode.Read, Encoding.GetEncoding("sjis")))
            {
                archive.Entries.Where(entry => entry.Name.EndsWith("txt")).ToList().ForEach(PrintEntryContents);
            }

            File.Delete(_zipFilePath);
            Directory.Delete(Path.Combine(DesktopPath, "ZipTest"), true);
        }

        private void Prepare()
        {
            //
            // サンプルZIPファイルを作成しておく.
            // (デスクトップ上にZipTest.zipという名称で出力される)
            //
            new ZipFileSamples01().Execute();
            _zipFilePath = Path.Combine(DesktopPath, "ZipTest.zip");
        }

        private void PrintEntry(ZipArchiveEntry entry)
        {
            Output.WriteLine("[{0}, {1}]", entry.Name, entry.Length);
        }

        private void PrintEntryContents(ZipArchiveEntry entry)
        {
            using (var reader = new StreamReader(entry.Open(), Encoding.GetEncoding("sjis")))
            {
                for (var line = reader.ReadLine(); line != null; line = reader.ReadLine())
                {
                    Output.WriteLine(line);
                }
            }
        }
    }
}