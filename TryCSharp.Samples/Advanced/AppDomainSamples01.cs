using System;
using TryCSharp.Common;

namespace TryCSharp.Samples.Advanced
{
    /// <summary>
    /// AppDomainクラスのサンプルです。
    /// </summary>
    [Sample]
    public class AppDomainSamples01 : IExecutable
    {
        public void Execute()
        {
            //
            // AppDomainには、.NET 4.0より以下のイベントが追加されている。
            //   ・FirstChanceExceptionイベント
            // このイベントは、例外が発生した際に文字通り最初に通知されるイベントである。
            // このイベントに通知されるタイミングは、catch節にて例外が補足されるよりも先となる。
            //
            // 注意点として
            //   ・このイベントは、通知のみとなる。このイベントをハンドルしたからといって例外の発生が
            //    ここで止まるわけではない。例外は通常通りプログラムコード上のcatchに入ってくる。
            //   ・このイベントは、アプリケーションドメイン毎に定義できる。
            //   ・FirstChanceExceptionイベント内での例外は、絶対にハンドラ内でキャッチしないといけない。
            //    そうしないと、再帰的にFirstChanceExceptionが発生する。
            //   ・イベント引数であるFirstChanceExceptionEventArgsクラスは
            //    System.Runtime.ExceptionServices名前空間に存在する。
            //

            // 基底のAppDomainにて、FirstChanceExceptionイベントをハンドル.
            AppDomain.CurrentDomain.FirstChanceException += FirstChanceExHandler;

            try
            {
                // わざと例外発生.
                throw new InvalidOperationException("test Ex messsage");
            }
            catch (InvalidOperationException ex)
            {
                // 本来のcatch処理.
                Output.WriteLine("Catch clause: {0}", ex.Message);
            }

            // イベントをアンバインド.
            AppDomain.CurrentDomain.FirstChanceException -= FirstChanceExHandler;
        }

        // イベントハンドラ.
        void FirstChanceExHandler(object sender, System.Runtime.ExceptionServices.FirstChanceExceptionEventArgs e)
        {
            Output.WriteLine("FirstChanceException: {0}", e.Exception.Message);
        }
    }
}