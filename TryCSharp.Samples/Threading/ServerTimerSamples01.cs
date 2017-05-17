using System.Threading;
using System.Timers;
using System.Windows.Forms;
using TryCSharp.Common;
using Timer = System.Timers.Timer;

namespace TryCSharp.Samples.Threading
{
    //
    // Alias設定.
    //
    using WinFormsApplication = Application;
    using WinFormsDockStyle = DockStyle;
    using WinFormsForm = Form;
    using WinFormsFormClosingEventArgs = FormClosingEventArgs;
    using WinFormsListBox = ListBox;

    /// <summary>
    ///     System.Timers.Timerクラスについてのサンプルです。
    /// </summary>
    [Sample]
    public class ServerTimerSamples01 : WinFormsForm, IExecutable
    {
        private WinFormsListBox _listBox;
        private Timer _timer;

        public ServerTimerSamples01()
        {
            InitializeComponent();
            SetTimer();
        }

        public void Execute()
        {
            WinFormsApplication.EnableVisualStyles();
            WinFormsApplication.Run(new ServerTimerSamples01());
        }

        private void InitializeComponent()
        {
            SuspendLayout();

            Text = "Timer Sample.";
            FormClosing += OnFormClosing;

            _listBox = new WinFormsListBox();
            _listBox.Dock = WinFormsDockStyle.Fill;

            Controls.Add(_listBox);

            ResumeLayout();
        }

        private void SetTimer()
        {
            _timer = new Timer();

            _timer.Elapsed += OnTimerElapsed;

            //
            // System.Timers.Timerはサーバータイマの為
            // ThreadPoolにてイベントが発生する。
            //
            // Elapsedイベント内で、UIコントロールにアクセスする必要がある場合
            // そのままだと、別スレッドからコントロールに対してアクセスしてしまう可能性があるので
            // イベント内にて、Control.Invokeするか、以下のようにSynchronizingObjectを
            // 設定して、イベントの呼び出しをマーシャリングするようにする。
            //
            _timer.SynchronizingObject = this;

            //
            // 繰り返しの設定.
            //
            _timer.Interval = 1000;
            _timer.AutoReset = true;

            //
            // タイマを開始.
            //
            _timer.Enabled = true;
        }

        private void OnFormClosing(object sender, WinFormsFormClosingEventArgs e)
        {
            _timer.Enabled = false;
            _timer.Dispose();
        }

        private void OnTimerElapsed(object sender, ElapsedEventArgs e)
        {
            _listBox.Items.Add($"Time:{e.SignalTime:HH:mm:ss}, ThreadID:{Thread.CurrentThread.ManagedThreadId}");
        }
    }
}