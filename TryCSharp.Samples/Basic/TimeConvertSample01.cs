using System;
using TryCSharp.Common;

namespace TryCSharp.Samples.Basic
{
    /// <summary>
    ///     時刻に関する処理(XX時間XX分形式から10進数形式に変換)についてのサンプルです。
    /// </summary>
    [Sample]
    public class TimeConvertSample01 : IExecutable
    {
        public void Execute()
        {
            // 元の値。7時間40分とする.
            var original = 111.07M;

            //
            // 時間の部分は既に確定済みなので、そのまま利用.
            //
            var hour = decimal.ToInt32(original);

            //
            // 元の値より、時間の部分を差し引く.
            // 上記の元値の場合は、0.4となる。
            //
            var minutes = original - hour;

            //
            // 0.4に対して、100を掛けて分数を確定.
            //
            minutes *= 100;

            //
            // 最後に60（一時間の分数）で割る.
            //
            minutes /= 60;

            //
            // 計算結果によっては、端数が生じるので四捨五入.
            // (小数点第3位四捨五入)
            //
            minutes = Math.Round(minutes, 2, MidpointRounding.AwayFromZero);

            //
            // 結果を構築.
            //
            // 上記の分を求める式は、以下のようにも出来る。
            // minutes = Math.Round(((original % 1) * 100 / 60), 2, MidpointRounding.AwayFromZero);
            //
            var result = hour + minutes;

            Output.WriteLine("{0}時間", result);
        }
    }
}