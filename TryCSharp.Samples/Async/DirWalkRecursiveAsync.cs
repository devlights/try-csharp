using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using TryCSharp.Common;

namespace TryCSharp.Samples.Async
{
    public class DirWalkRecursiveAsync : IAsyncExecutable
    {
        public async Task Execute()
        {
            var depth = 1;
            var dir = Path.GetFullPath(@".");
            var buf = new BlockingCollection<string>();

            // ---------------------------------------------------------
            // 再帰的にディレクトリを降りていき、ファイルを出力
            // ---------------------------------------------------------
            var listDirTask = Task.Run<Task>(async () =>
            {
                await this.ListDir(dir, buf, depth);
                buf.CompleteAdding();
            });

            // ---------------------------------------------------------
            // 結果を順次出力
            // ---------------------------------------------------------
            var outputTask = Task.Run(() =>
            {
                foreach (var item in buf.GetConsumingEnumerable())
                {
                    Output.WriteLine(item);
                }
            });


            await Task.WhenAll(listDirTask, outputTask);
        }

        private async Task ListDir(string dir, BlockingCollection<string> buf, int depth)
        {
            var subDirBufs = new List<BlockingCollection<string>>();
            var dirPrefix = new string('\t', depth - 1);
            var filePrefix = new string('\t', depth);

            var d = dir;
            if (depth > 1)
            {
                d = Path.GetFileName(dir);
            }

            buf.Add($"{dirPrefix}{d}");

            // ---------------------------------------------------------
            // サブディレクトリに対して、非同期で再帰処理実行
            // ---------------------------------------------------------
            var tasks = new List<Task>();
            foreach (var subD in Directory.EnumerateDirectories(dir, "*", SearchOption.TopDirectoryOnly))
            {
                if (Path.GetFileName(subD).StartsWith("."))
                {
                    continue;
                }

                var bufSubDir = new BlockingCollection<string>();
                subDirBufs.Add(bufSubDir);

                var subDir = Path.Combine(dir, subD);
                tasks.Add(Task.Run(async () =>
                {
                    await this.ListDir(subDir, bufSubDir, depth + 1);
                    bufSubDir.CompleteAdding();
                }));
            }

            // ---------------------------------------------------------
            // ファイルはそのまま出力
            // ---------------------------------------------------------
            foreach (var subF in Directory.EnumerateFiles(dir, "*.*", SearchOption.TopDirectoryOnly))
            {
                buf.Add($"{filePrefix}{Path.GetFileName(subF)}");
            }

            // ---------------------------------------------------------
            // 非同期処理の結果を親のバッファに順次投入
            // ---------------------------------------------------------
            await Task.WhenAll(tasks).ContinueWith(task =>
            {
                foreach (var subDirBuf in subDirBufs)
                {
                    foreach (var item in subDirBuf.GetConsumingEnumerable())
                    {
                        buf.Add(item);
                    }
                }
            });
        }
    }
}