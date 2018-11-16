using System;
using TryCSharp.Common;

namespace TryCSharp.Samples.Advanced
{
    /// <summary>
    ///     System.Reflection.Assemblyクラスのサンプルです。
    /// </summary>
    /// <remarks>
    ///     アセンブリのバージョンが自動採番されている場合に
    ///     ビルドされた日時を求める方法について記述しています。
    /// </remarks>
    [Sample]
    public class AssemblySamples01 : IExecutable
    {
        public void Execute()
        {
            //
            // アセンブリのバージョンについて
            //
            // アセンブリのバージョンはAssemblyNameからVersionプロパティから取得できる。
            // System.Versionは、Major, Minor, Build, Revision のプロパティを持っており
            // これが、そのままVisualStudioで設定するバージョン番号に対応する。
            //
            var asm = GetType().Assembly;
            var ver = asm.GetName().Version;

            Output.WriteLine(ver);

            //
            // アセンブリバージョンは 通常 AssemblyInfo.cs 内で
            //    [assembly: AssemblyVersion("1.0.0.0")]
            // のようにデフォルトで指定してあるが、これを
            //    [assembly: AssemblyVersion("1.0.*")]
            // とすると、BuildとRevisionの部分を自動採番するように
            // 設定できる。この場合VisualStudioにてビルドする度に
            //    1.0.5582.24111
            // のようにバージョンが変わっていく。
            //
            // VisualStudioの場合、BuildとRevisionに指定される値に
            // 以下の規則性があり、計算することでビルドされた日時が求められる。
            //   ・Buildの部分は、2000/01/01からの経過日数
            //   ・Revisionの部分は、0時からの経過秒数を2で割った値
            //
            // 参考にしたURL:
            //    http://www.atmarkit.co.jp/fdotnet/dotnettips/187asmverinfo/asmverinfo.html
            //    http://stackoverflow.com/questions/826777/how-to-have-an-auto-incrementing-version-number-visual-studio
            //    http://stackoverflow.com/questions/356543/can-i-automatically-increment-the-file-build-version-when-using-visual-studio
            //    https://msdn.microsoft.com/en-us/library/system.reflection.assemblyversionattribute.aspx
            //    https://msdn.microsoft.com/ja-jp/library/system.reflection.assemblyversionattribute.aspx
            //
            // 上のリンクの内、公式なMSDNのページを見ると、英語版の方は
            //   "The default build number increments daily. The default revision number is the number of seconds since midnight local time (without taking into account time zone adjustments for daylight saving time), divided by 2."
            // と記載されている (上記ページの 2015/04/14 時点の内容から一部引用）
            // が、日本語版の方だと
            //   "既定のビルド番号は、日単位でインクリメントされます。 既定のリビジョン番号はランダムな値になります。"
            // と記載されており (上記ページの 2015/04/14 時点の内容から一部引用）、ちょっと異なる。
            //
            var build = ver.Build;
            var revision = ver.Revision;
            var baseDate = new DateTime(2000, 1, 1);

            Output.WriteLine("ビルドされた日時：{0}", baseDate.AddDays(build).AddSeconds(revision*2));

            //
            // 出力結果は、以下のようになる。
            // ================== START ==================
            // 1.0.5582.24285
            // ビルドされた日時：2015/04/14 13:29:30
            // ==================  END  ==================
            //
        }
    }
}