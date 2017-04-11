namespace TryCSharp.Samples.Commons.Extensions
{
    /// <summary>
    ///     共通で利用しているString拡張クラスです。
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        ///     数値に変換します。
        /// </summary>
        /// <param name="self">自分自身</param>
        /// <return>自分自身を数値に変換した値.</return>
        public static int ToInt(this string self)
        {
            int i;
            if (!int.TryParse(self, out i))
            {
                return int.MinValue;
            }

            return i;
        }
    }
}