using System;
using System.Diagnostics;
using System.IO;
using TryCSharp.Common;

namespace TryCSharp.Samples.Basic
{
    /// <summary>
    ///     Debugクラスについてのサンプルです。
    /// </summary>
    [Sample]
    public class DebugSamples01 : IExecutable
    {
        public void Execute()
        {
            //
            // Debugクラスは文字通りデバッグ情報を出力するために利用されるクラスである.
            // このクラスの全てのメソッドは、Conditional("DEBUG")属性が付与されている.
            // なので、DEBUG定数を指定していない場合、コンパイラの段階で呼び出しが削除される.
            //
            // Visual Studioではデフォルトで
            //   デバッグビルドの場合は、DEBUGとTRACE
            //   リリースビルドの場合は、TRACE
            // が付与されて、コンパイルされる.
            //
            // Traceクラスと同様にDebugクラスもListenerの考えを持っていて
            // リスナーを追加することでいろいろな出力が行える様になっている.
            //
            // 注意点として、ListenerはTraceとDebugで共有されており
            // どちらかに追加すると両方で有効となる.
            //
            // また、ListenerにはTraceOptionsを指定して細かく情報を出力することが
            // 可能となっているが、DebugクラスのWrite, WriteLineなどのメソッドで出力しても
            // TraceOptionsの設定が効かない. (Trace.WriteInformation
            //
            Debug.IndentLevel = 2;
            Debug.IndentSize = 2;
            Debug.AutoFlush = true;

            var options = TraceOptions.DateTime |
                          TraceOptions.LogicalOperationStack |
                          TraceOptions.Callstack |
                          TraceOptions.ThreadId |
                          TraceOptions.Timestamp;

            Debug.Listeners.Clear();
            Debug.Listeners.Add(new ConsoleTraceListener {TraceOutputOptions = options});

            var prevOutputManager = Output.OutputManager;
            Output.OutputManager = new DebugOutputManager();

            Output.WriteLine("デバッグメッセージ");

            Output.OutputManager = prevOutputManager;
        }
    }

    internal class DebugOutputManager : IOutputManager
    {
        private readonly bool output = true;

        public void Write(object data)
        {
            Debug.Write(data);
            Debug.Write(data, "Log-Category");
            Debug.WriteIf(output, string.Format("[条件付き]{0}", data));
        }

        public void WriteLine(object data)
        {
            Debug.WriteLine(data);
            Debug.WriteLine(data, "Log-Category");
            Debug.WriteLineIf(output, string.Format("[条件付き]{0}", data));
        }

        public Stream OutStream
        {
            get { throw new NotSupportedException(); }
        }
    }

}