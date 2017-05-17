using System;
using System.ComponentModel;
using System.Threading;
using System.Windows.Forms;
using TryCSharp.Common;

namespace TryCSharp.Samples.Threading
{
    using WinFormsApplication = Application;
    using WinFormsButton = Button;
    using WinFormsDockStyle = DockStyle;
    using WinFormsFlowLayoutPanel = FlowLayoutPanel;
    using WinFormsForm = Form;
    using WinFormsMessageBox = MessageBox;

    /// <summary>
    ///     AsyncOperationのサンプル1です。
    /// </summary>
    [Sample]
    public class AsyncOperationSamples01 : IExecutable
    {
        [STAThread]
        public void Execute()
        {
            // 以下のコメントを外す事でコントロールが最初にnewされた際に
            // WindowsFormsSynchronizationContextが読み込まれないように出来ます。
            // falseにすると、デフォルトでSynchronizationContextが読み込まれます。
            //WindowsFormsSynchronizationContext.AutoInstall = false;

            WinFormsApplication.EnableVisualStyles();
            WinFormsApplication.Run(new SampleForm());
        }

        private class SampleForm : WinFormsForm
        {
            public SampleForm()
            {
                Thread.CurrentThread.Name = "Main Thread";
                InitializeComponent();
            }

            protected void InitializeComponent()
            {
                SuspendLayout();

                var b = new WinFormsButton
                {
                    Name = "btn01",
                    Text = "btn01"
                };
                b.Click += (s, e) =>
                {
                    WinFormsMessageBox.Show("Click Handler Begin : " + string.Format("{0}-{1}", Thread.CurrentThread.Name, Thread.CurrentThread.ManagedThreadId));

                    var btn = s as WinFormsButton;

                    // この場所でAsyncOprationを作成するとWindowsFormsSynchronizationContextオブジェクトになる。
                    // (現在のスレッドのコンテキストがWindows Formsのため)
                    // (WindowsFormsSynchronizationContext.AutoInstallの値がデフォルトでtrueになっているので、最初のコントロールが
                    //  newされた時にメッセージスレッド (ここではMain Thread)にWindowsFormsSynchronizationContextが読み込まれる)
                    //
                    // SynchonizationContextはSystem.Threading名前空間に存在し、以下の派生クラスを持つ。
                    //    ⇒System.Windows.Forms.WindowsFormsSynchronizationContext   (WindowsForms用)
                    //    ⇒System.Windows.Threading.DispatcherSynchronizationContext (WPF用)
                    // それそれの派生クラスは、基本機能に加え、各自独自の動作とプロパティを持っている。
                    WinFormsMessageBox.Show("Click Handler Begin SyncContext : " + SynchronizationContext.Current.ToString());

                    // AsyncOperationManagerはCreateOperationメソッドが呼ばれた際に現在のSynchronizationContextをコピーして
                    // AsyncOperationのコンストラクタに渡している模様。なので、AsyncOperationのPost,Sendが正確にメッセージスレッドに
                    // 同期できるかどうかは、このAsyncOperationオブジェクトを作成した時点でのスレッドにSynchronizationContextがあるかどうかによる。
                    // （新規でスレッドを作成した場合はSynchronizationContextはnullとなっている。）
                    //
                    // 尚、SynchronizationContextを直接用いて、Post, Sendを行ってもよい。
                    // 現在のスレッドのSynchronizationContextを取得するには、SynchronizationContext.Currentで取得できる。
                    // 存在しない場合は、以下のようにして設定できる。
                    //
                    // SynchronizationContext.SetSynchronizationContext(new SynchronizationContext());

                    // この場所でAsyncOperationを作成した場合、このスレッド(Main Thread)上では既にWindowsFormsSynchronizationContextが作成されている
                    // ので、それがAsyncOperationに渡る。
                    // AsyncOperationは自身のPost, PostOperationCompletedが呼ばれた際に内部で保持しているSynchronizationContextに処理を委譲します。
                    // （なので、正常にメッセージスレッドにてデリゲートが処理される事になる。）
                    var asyncOp = AsyncOperationManager.CreateOperation(btn);

                    // 別スレッド側にも同じSynchronizationContextを利用させる場合は以下のようにして
                    // コピーを作成し、保持しておき、対象のスレッドに設定する。
                    //SynchronizationContext syncContext = SynchronizationContext.Current.CreateCopy();

                    var t = new Thread(() =>
                    {
                        WinFormsMessageBox.Show("New Thread : " + $"{Thread.CurrentThread.Name}-{Thread.CurrentThread.ManagedThreadId}");

                        // 新規で別のスレッドを作成した場合は最初SynchronizationContextはnullとなっている。
                        // 新たに割り当てる場合は以下のようにする。
                        //SynchronizationContext.SetSynchronizationContext(syncContext);

                        // この場所でAsyncOprationを作成するとSynchronizationContextオブジェクトになる。
                        // (AsyncOperationManagerは、現在のスレッドにてSynchronizationContextが存在しない場合は新たにSynchronizationContextを
                        //  生成し、設定してからAsyncOperationを生成する。元々SynchronizationContextがAsyncOperationを生成する際に存在している場合は
                        //  それをAsyncOperationに渡して生成する。）
                        WinFormsMessageBox.Show("New Thread SyncContext Is Null?? (1) : " + (SynchronizationContext.Current == null));
                        //AsyncOperation asyncOp = AsyncOperationManager.CreateOperation(btn);
                        WinFormsMessageBox.Show("New Thread SyncContext Is Null?? (2) : " + (SynchronizationContext.Current == null));

                        // ここで表示されるSynchronizationContextは, AsyncOperationがどのスレッドで作成されたかによって
                        // 出力される値が変わる。
                        //
                        // ■イベントハンドラ側でAsyncOperationが生成された場合：WindowsFormsSynchronizationContext
                        // ■別スレッド側でAsyncOperationが生成された場合：    SynchronizationContext
                        WinFormsMessageBox.Show(asyncOp.SynchronizationContext.ToString());

                        // Post及びPostOperationCompletedメソッドの呼び出しは、実際にはAsyncOperationが内部で保持しているSynchronizationContext.Postを
                        // 呼び出しているので、対象となるSynchronizationContextによって同期されるスレッドが異なる。
                        //
                        // ■イベントハンドラ側でAsyncOperationが生成された場合：メッセージスレッド側に同期 (Main Thread)
                        // ■別スレッド側でAsyncOperationが生成された場合：    新たにスレッドが作成されその中で処理 (Thread Pool)
                        asyncOp.Post(state =>
                        {
                            var curThread = Thread.CurrentThread;
                            WinFormsMessageBox.Show("AsyncOp.Post : " + string.Format("{0}-{1}-IsThreadPool:{2}", curThread.Name, curThread.ManagedThreadId, curThread.IsThreadPoolThread));
                        }, asyncOp);

                        asyncOp.PostOperationCompleted(state =>
                        {
                            var curThread = Thread.CurrentThread;
                            WinFormsMessageBox.Show("AsyncOp.PostOperationCompleted : " + string.Format("{0}-{1}-IsThreadPool:{2}", curThread.Name, curThread.ManagedThreadId, curThread.IsThreadPoolThread));
                        }, asyncOp);
                    });

                    t.Name = "Sub Thread";
                    t.IsBackground = true;
                    t.Start();
                };

                var layout = new WinFormsFlowLayoutPanel();
                layout.Dock = WinFormsDockStyle.Fill;
                layout.Controls.Add(b);

                Controls.Add(layout);

                ResumeLayout();
            }
        }
    }
}