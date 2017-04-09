using System;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TryCSharp.Common;

namespace TryCSharp.Samples.Advanced
{
    //
    // Alias設定.
    //
    using WinFormsApplication = System.Windows.Forms.Application;
    using WinFormsButton = System.Windows.Forms.Button;
    using WinFormsDockStyle = System.Windows.Forms.DockStyle;
    using WinFormsFlowDirection = System.Windows.Forms.FlowDirection;
    using WinFormsFlowLayoutPanel = System.Windows.Forms.FlowLayoutPanel;
    using WinFormsForm = System.Windows.Forms.Form;
    using WinFormsLabel = System.Windows.Forms.Label;
    using WinFormsProgressBar = System.Windows.Forms.ProgressBar;
    using WinFormsProgressBarStyle = System.Windows.Forms.ProgressBarStyle;

    /// <summary>
    /// System.Progress<T>のサンプルです。
    /// </summary>
    /// <remarks>
    /// このクラスは、.NET Framework 4.5から追加された型です。
    /// </remarks>
    [Sample]
    public class ProgressSamples01 : IExecutable
    {
        class SampleForm : WinFormsForm
        {
            const int MIN = 0;
            const int MAX = 100;

            WinFormsLabel _label;
            WinFormsProgressBar _bar;
            WinFormsButton _btn;

            public SampleForm()
            {
                InitializeControl();
                InitializeEvent();
            }

            void InitializeControl()
            {
                SuspendLayout();

                Width = 400;
                Height = 130;

                _label = new WinFormsLabel
                {
                    Text = string.Empty,
                    AutoSize = false,
                    Width = 350
                };

                _bar = new WinFormsProgressBar
                {
                    Minimum = MIN,
                    Maximum = MAX,
                    Width = 350,
                    Value = MIN,
                    Step = 1,
                    Style = WinFormsProgressBarStyle.Continuous
                };

                _btn = new WinFormsButton
                {
                    Text = "Cancel",
                    Width = 120
                };

                var panel = new WinFormsFlowLayoutPanel
                {
                    FlowDirection = WinFormsFlowDirection.TopDown,
                    Dock = WinFormsDockStyle.Fill
                };

                panel.Controls.Add(_label);
                panel.Controls.Add(_bar);
                panel.Controls.Add(_btn);

                Controls.Add(panel);

                ResumeLayout();
            }

            void InitializeEvent()
            {
                Load += async (s, e) =>
                {
                    var tokenSource = new CancellationTokenSource();
                    var progress = new Progress<ProgressMessage>(SetProgress);

                    _btn.Tag = tokenSource;
                    _bar.Maximum = Directory.EnumerateFiles(".").Count();

                    await Compress(tokenSource.Token, progress);

                    Text = "DONE";
                    if (tokenSource.IsCancellationRequested)
                    {
                        Text = "CANCEL";
                    }
                };

                _btn.Click += (s, e) => { (_btn.Tag as CancellationTokenSource).Cancel(); };
            }

            async Task Compress(CancellationToken token, IProgress<ProgressMessage> progress)
            {
                string ZipFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
                    "ZipTest3.zip");
                string TmpFilePath = ZipFilePath + ".tmp";

                if (File.Exists(ZipFilePath))
                {
                    File.Move(ZipFilePath, TmpFilePath);
                }

                using (var archive = ZipFile.Open(ZipFilePath, ZipArchiveMode.Create))
                {
                    foreach (var filePath in Directory.EnumerateFiles("."))
                    {
                        if (token.IsCancellationRequested)
                        {
                            break;
                        }

                        progress.Report(new BeginMessage
                        {
                            Message = string.Format("{0}を圧縮しています...", filePath),
                            Token = token
                        });

                        archive.CreateEntryFromFile(filePath, Path.GetFileName(filePath));
                        await Task.Delay(1000);

                        progress.Report(new AfterMessage
                        {
                            Message = string.Format("{0}を圧縮完了", filePath),
                            Token = token
                        });
                    }
                }

                if (token.IsCancellationRequested)
                {
                    File.Delete(ZipFilePath);

                    if (File.Exists(TmpFilePath))
                    {
                        File.Move(TmpFilePath, ZipFilePath);
                    }
                }
                else
                {
                    if (File.Exists(TmpFilePath))
                    {
                        File.Delete(TmpFilePath);
                    }
                }
            }

            void SetProgress(ProgressMessage message)
            {
                if (message.Token.IsCancellationRequested)
                {
                    _label.Text = "処理はキャンセルされました。";
                    return;
                }

                _label.Text = message.Message;
                if (message is AfterMessage)
                {
                    _bar.PerformStep();
                }
            }

            class ProgressMessage
            {
                public string Message { get; set; }

                public CancellationToken Token { get; set; }
            }

            class BeginMessage : ProgressMessage
            {
            }

            class AfterMessage : ProgressMessage
            {
            }
        }

        [STAThread]
        public void Execute()
        {
            WinFormsApplication.EnableVisualStyles();
            WinFormsApplication.Run(new SampleForm());
        }
    }
}