using System;
using TryCSharp.Common;

namespace TryCSharp.Samples.Basic
{
    /// <summary>
    ///     時刻に関する処理(10進数形式からXX時間XX分形式に変換)についてのサンプルです。
    /// </summary>
    [Sample]
    public class TimeConvertSample02 : IExecutable
    {
        public void Execute()
        {
            // 元の値. 7.67時間とする.
            var original = 111.12M;

            //
            // 時間の部分は既に確定済みなので、そのまま利用.
            //
            var hour = decimal.ToInt32(original);

            //
            // 時間部分の分数を算出.
            //
            var hourMinutes = hour*60;

            //
            // 元の値の分数を算出.
            //
            var originalMinutes = original*60;

            //
            // 求めた元の値の分数を四捨五入.
            //
            var roundedOriginalMinutes = decimal.ToInt32(Math.Round(originalMinutes, 0, MidpointRounding.AwayFromZero));

            //
            // 元の値の分数から時間部分の分数を引く.
            // これが結果の分数となる。
            //
            var minutes = roundedOriginalMinutes - hourMinutes;

            //
            // 結果を構築.
            //
            var result = decimal.Parse(string.Format("{0}.{1}", hour, minutes));

            Output.WriteLine("結果={0}, {1}時間{2}分", result, hour, minutes);
        }
    }
}