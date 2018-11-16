using System.IO;
using System.Linq;
using TryCSharp.Common;

namespace TryCSharp.Samples.IO
{
    /// <summary>
    ///     PathクラスのGetInvalidPathCharsメソッドとGetInvalidFileNameCharsメソッドのサンプルです。
    /// </summary>
    [Sample]
    public class GetInvalidPathCharsAndGetInvalidFileNameCharsSamples01 : IExecutable
    {
        public void Execute()
        {
            //
            // Pathクラスには、パス名及びファイル名に利用できない文字を取得するメソッドが存在する。
            //   パス名：GetInvalidPathChars
            // ファイル名：GetInvalidFileNameChars
            //
            // 引数などで渡されたパスやファイル名に対して不正な文字が利用されていないか
            // チェックする際などに利用できる。
            //
            // 戻り値は、どちらもcharの配列となっている。
            //
            var invalidPathChars = Path.GetInvalidPathChars();
            var invalidFileNameChars = Path.GetInvalidFileNameChars();

            const string tmpPath = @"c:usrlocaltmp_<path>_tmp";
            const string tmpFileName = @"tmp_<filename>_tmp.|||";

            Output.WriteLine("不正なパス文字が存在してる？     = {0}", invalidPathChars.Any(ch => tmpPath.Contains(ch)));
            Output.WriteLine("不正なファイル名文字が存在してる？ = {0}", invalidFileNameChars.Any(ch => tmpFileName.Contains(ch)));
        }
    }
}