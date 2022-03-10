using System.IO;
using System.Runtime.CompilerServices;
using TryCSharp.Common;

namespace TryCSharp.Samples.Basic
{
    /// <summary>
    ///     Caller Information (呼び元情報）についてのサンプルです。
    /// </summary>
    /// <remarks>
    ///     C# 5.0にて追加された Caller Information属性についてのサンプルです。
    ///     以下の3つの属性について記述しています。
    ///     -CallerFilePath
    ///     -CallerLineNumber
    ///     -CallerMemberName
    /// </remarks>
    public class CallerInformationSamples01 : IExecutable
    {
        public void Execute()
        {
            //
            // Caller Informationは、C# 5.0にて追加された機能である。
            // Cの__FILE__や__LINE__と同じ要領で利用できる属性。
            // 追加された属性は以下の3つ。
            //   CallerFilePath:   ファイルパス
            //   CallerLineNumber: 行番号
            //   CallerMemberName: メンバー名
            // 上記の属性はメソッドの引数に指定して利用する.
            //
            // 注意点として、以下の点が存在する。
            //   ・引数にデフォルト値を指定していないとダメ
            //   ・コンパイル時に解決される情報である
            //
            // コンストラクタやデストラクタなどの一部のメソッドは
            // 特殊な名称が設定される。（.ctor, .cctor, Finalize)
            // 詳細については
            //   http://msdn.microsoft.com/en-us/library/hh534540.aspx
            // を参照の事。
            //
            // 利用する場合、以下の名前空間をインポートしておくこと。
            //   System.Runtime.CompilerServices
            //
            var manager = new CallerInfoManager();

            //
            // 各呼び出し時の呼び元情報を取得して表示.
            //
            Output.WriteLine(manager.Snap());
            Output.WriteLine(MethodA(manager));
            Output.WriteLine(MethodB(manager));
        }

        private CallerInfoManager MethodA(CallerInfoManager manager)
        {
            return manager.Snap();
        }

        private CallerInfoManager MethodB(CallerInfoManager manager)
        {
            return manager.Snap();
        }
    }

    /// <summary>
    ///     Caller Informationを管理するクラス.
    /// </summary>
    internal class CallerInfoManager
    {
        /// <summary>
        ///     ファイルパス
        /// </summary>
        public string? FilePath { get; private set; }

        /// <summary>
        ///     行番号
        /// </summary>
        public int LineNumber { get; private set; }

        /// <summary>
        ///     メンバー名
        /// </summary>
        public string? MemberName { get; private set; }

        /// <summary>
        ///     現在のコンテキストでのCaller Informationをスナップします。
        /// </summary>
        /// <param name="file">ファイルパス</param>
        /// <param name="line">行番号</param>
        /// <param name="member">メンバー名</param>
        /// <returns>自分自身 (レシーバ）</returns>
        public CallerInfoManager Snap([CallerFilePath] string file = "", [CallerLineNumber] int line = 0, [CallerMemberName] string member = "")
        {
            FilePath = file;
            LineNumber = line;
            MemberName = member;

            return this;
        }

        public override string ToString()
        {
            return $"File:{Path.GetFileName(FilePath)}\nLine:{LineNumber}\nMember:{MemberName}";
        }
    }

}