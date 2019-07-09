using TryCSharp.Common;

namespace TryCSharp.Samples.CSharp7
{
    [Sample]
    public class Deconstructors : IExecutable
    {
        class Data
        {
            private readonly int _x;
            private readonly int _y;

            public Data(int x, int y)
            {
                this._x = x;
                this._y = y;
            }

            public void Deconstruct(out int x, out int y)
            {
                x = this._x;
                y = this._y;
            }
        }
        
        public void Execute()
        {
            // C# 7.0 で deconstructor パターンが導入された
            // deconstructor とは コンストラクタの逆の事を示す
            // つまり、オブジェクト自身を右辺に配置した場合に自身の内容を
            // 「分解」して返却するための機能
            // deconstructor は 決まった書式で以下の様に記載する
            //   public void Deconstruct(out xxx a, out xxx b)
            var d = new Data(1, 2);
            
            // ここで deconstructor が呼ばれる
            var (x, y) = d;
            
            Output.WriteLine($"{x}-{y}");
        }
    }
}