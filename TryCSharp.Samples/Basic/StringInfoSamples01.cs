using System;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;
using TryCSharp.Common;

namespace TryCSharp.Samples.Basic
{
    //
    // Alias設定.
    //
    using GDISize = Size;
    using WinFormsApplication = Application;
    using WinFormsButton = Button;
    using WinFormsControl = Control;
    using WinFormsDockStyle = DockStyle;
    using WinFormsFlowDirection = FlowDirection;
    using WinFormsFlowLayoutPanel = FlowLayoutPanel;
    using WinFormsForm = Form;
    using WinFormsFormStartPosition = FormStartPosition;
    using WinFormsMessageBox = MessageBox;
    using WinFormsTextBox = TextBox;

    /// <summary>
    ///     StringInfoについてのサンプルです。
    /// </summary>
    /// <remarks>
    ///     サロゲートペアについて記述しています。
    /// </remarks>
    [Sample]
    public class StringInfoSamples01 : IExecutable
    {
        [STAThread]
        public void Execute()
        {
            WinFormsApplication.EnableVisualStyles();
            WinFormsApplication.Run(new StringInfoSampleForm());
        }

        public class StringInfoSampleForm : WinFormsForm
        {
            public StringInfoSampleForm()
            {
                InitializeComponent();
            }

            private void InitializeComponent()
            {
                SuspendLayout();

                Size = new GDISize(350, 100);
                StartPosition = WinFormsFormStartPosition.CenterScreen;
                Text = "サロゲートペアの確認サンプル";


                var t = new WinFormsTextBox();
                t.Text = "\uD867\uDE3D"; // 魚へんに花という文字。魚のホッケの文字を指定。

                var b = new WinFormsButton {Text = "Exec"};
                b.Click += (s, e) =>
                {
                    var str = t.Text;
                    WinFormsMessageBox.Show(string.Format("文字：{0}, 長さ：{1}", str, str.Length), "Stringでの表示");

                    //
                    // サロゲートペアの文字列
                    //
                    // サロゲートペアの文字列は１文字で
                    // ４バイトとなっている。
                    //
                    // サロゲートペアの文字列に対して
                    // String.Lengthプロパティで長さを取得すると
                    // １文字なのに２と返ってくる。
                    //
                    // これを１文字として認識するには以下のクラスを利用する。
                    //
                    // System.Globalization.StringInfo
                    //
                    // このクラスの以下のプロパティを利用することで
                    // １文字と認識することが出来る。
                    //
                    // LengthInTextElementsプロパティ
                    //
                    var si = new StringInfo(str);
                    WinFormsMessageBox.Show($"文字：{si.String}, 長さ：{si.LengthInTextElements}", "StringInfoでの表示");
                };

                var contentPane = new WinFormsFlowLayoutPanel {FlowDirection = WinFormsFlowDirection.TopDown, WrapContents = true};
                contentPane.Controls.AddRange(new WinFormsControl[] {t, b});
                contentPane.Dock = WinFormsDockStyle.Fill;

                Controls.Add(contentPane);

                ResumeLayout();
            }
        }
    }
}