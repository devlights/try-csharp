using TryCSharp.Common;

namespace TryCSharp.Samples.CSharp7
{
    [Sample]
    public class MoreExpressionBodiedMembers : IExecutable
    {
        class Data
        {
            private string _value;

            public Data(string val) => this.Value = val;
            
            public string UpperCaseValue => this.Value.ToUpper();

            // ReSharper disable once ConvertToAutoProperty
            public string Value
            {
                get => this._value;
                private set => this._value = value;
            }

            public string ToLowerValue() => this.Value.ToLower();
        }
        
        public void Execute()
        {
            // C# 6.0 にて Expression Body が導入されたが
            // C# 7.0 にて、新たに
            //   - コンストラクタ
            //   - read/write プロパティ
            //   - ファイナライザ
            // でも利用できるようなった
            var data = new Data("hello world");
            Output.WriteLine($"{data.Value}-{data.UpperCaseValue}-{data.ToLowerValue()}");
        }
    }
}