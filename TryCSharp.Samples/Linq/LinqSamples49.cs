using System.Diagnostics;
using System.IO;
using System.Linq;
using TryCSharp.Common;

namespace TryCSharp.Samples.Linq
{
    /// <summary>
    ///     Linqのサンプルです。
    /// </summary>
    [Sample]
    public class LinqSamples49 : IExecutable
    {
        public void Execute()
        {
            //
            // Directory.EnumerateFilesメソッドは、従来までの
            // Directory.GetFilesメソッドと同じ動作するメソッドである。
            //
            // 違いは、戻り値がIEnumerable<string>となっており
            // 遅延評価される。
            //
            // GetFilesメソッドの場合は、全リストを構築してから
            // 戻り値が返却されるので、コレクションが構築されるまで
            // 待機する必要があるが、EnumerateFilesメソッドの場合は
            // コレクション全体が返される前に、列挙可能である。
            //
            // EnumerateDirectoriesメソッド及びEnumerateFileSystemEntriesメソッドも上記と同様。
            //
            var path = @"c:\windows";
            var filter = @"*.exe";
            var watch = Stopwatch.StartNew();
            var elapsed = string.Empty;

            //
            // EnumerateFiles.
            //
            var query = from file in Directory.EnumerateFiles(path, filter, SearchOption.AllDirectories)
                    select file;

            foreach (var item in query)
            {
                if (watch != null)
                {
                    watch.Stop();
                    elapsed = watch.Elapsed.ToString();
                    watch = null;
                }

                //Output.WriteLine(item);
            }

            Output.WriteLine("================== EnumereteFiles       : {0} ==================", elapsed);

            //
            // EnumerateDirectories.
            //
            watch = Stopwatch.StartNew();
            elapsed = string.Empty;

            query = from directory in Directory.EnumerateDirectories(path)
                    select directory;

            foreach (var item in query)
            {
                if (watch != null)
                {
                    watch.Stop();
                    elapsed = watch.Elapsed.ToString();
                    watch = null;
                }

                //Output.WriteLine(item);
            }

            Output.WriteLine("================== EnumerateDirectories     : {0} ==================", elapsed);

            //
            // EnumerateFileSystemEntries.
            //
            watch = Stopwatch.StartNew();
            elapsed = string.Empty;

            query = from directory in Directory.EnumerateFileSystemEntries(path)
                    select directory;

            foreach (var item in query)
            {
                if (watch != null)
                {
                    watch.Stop();
                    elapsed = watch.Elapsed.ToString();
                    watch = null;
                }

                //Output.WriteLine(item);
            }

            Output.WriteLine("================== EnumerateFileSystemEntries : {0} ==================", elapsed);

            //
            // GetFiles.
            //
            watch = Stopwatch.StartNew();
            elapsed = string.Empty;

            var files = Directory.GetFiles(path, filter, SearchOption.AllDirectories);

            foreach (var item in files)
            {
                if (watch != null)
                {
                    watch.Stop();
                    elapsed = watch.Elapsed.ToString();
                    watch = null;
                }

                //Output.WriteLine(item);
            }

            Output.WriteLine("================== GetFiles           : {0} ==================", elapsed);

            //
            // GetDirectories.
            //
            watch = Stopwatch.StartNew();
            elapsed = string.Empty;

            var dirs = Directory.GetDirectories(path);

            foreach (var item in dirs)
            {
                if (watch != null)
                {
                    watch.Stop();
                    elapsed = watch.Elapsed.ToString();
                    watch = null;
                }

                //Output.WriteLine(item);
            }

            Output.WriteLine("================== GetDirectories       : {0} ==================", elapsed);

            //
            // GetFileSystemEntries.
            //
            watch = Stopwatch.StartNew();
            elapsed = string.Empty;

            var entries = Directory.GetFileSystemEntries(path);

            foreach (var item in entries)
            {
                if (watch != null)
                {
                    watch.Stop();
                    elapsed = watch.Elapsed.ToString();
                    watch = null;
                }

                //Output.WriteLine(item);
            }

            Output.WriteLine("================== GetFileSystemEntries     : {0} ==================", elapsed);
        }
    }
}