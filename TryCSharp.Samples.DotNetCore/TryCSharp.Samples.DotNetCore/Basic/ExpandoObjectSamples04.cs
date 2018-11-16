using System.Collections.Generic;
using System.ComponentModel;
using System.Dynamic;
using TryCSharp.Common;

namespace TryCSharp.Samples.Basic
{
    /// <summary>
    ///     ExpandoObjectクラスについてのサンプルです。
    /// </summary>
    /// <remarks>
    ///     .NET 4.0から追加されたクラスです。
    /// </remarks>
    [Sample]
    public class ExpandoObjectSamples04 : IExecutable
    {
        public void Execute()
        {
            ///////////////////////////////////////////////////////////////////////
            //
            // ExpandoObjectをINotifyPropertyChangedとして扱う. (プロパティの変更をハンドル)
            //
            dynamic obj = new ExpandoObject();

            //
            // イベントハンドラ設定.
            //
            (obj as INotifyPropertyChanged).PropertyChanged += (sender, e) => { Output.WriteLine("Property Changed:{0}", e.PropertyName); };

            //
            // メンバー定義.
            //
            obj.Name = "gsf_zero1";
            obj.Age = 30;

            //
            // メンバー削除.
            //
            (obj as IDictionary<string, object>).Remove("Age");

            //
            // 値変更.
            //
            obj.Name = "gsf_zero2";

            //
            // 実行結果：
            //     Property Changed:Name
            //     Property Changed:Age
            //     Property Changed:Age
            //     Property Changed:Name
            //
        }
    }
}