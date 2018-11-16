namespace TryCSharp.Samples.Helpers
{
    /// <summary>
    ///     比較メソッドの結果値を変換するためのヘルパークラス.
    /// </summary>
    public static class CompareResultHelper
    {
        private static readonly string[] CompResults = {"小さい", "等しい", "大きい"};

        // 比較結果の数値を文字列に変換.
        public static string ToStringResult(this int self)
        {
            return CompResults[self + 1];
        }
    }
}