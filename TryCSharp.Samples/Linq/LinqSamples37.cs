using System.Linq;
using TryCSharp.Common;

namespace TryCSharp.Samples.Linq
{
    /// <summary>
    ///     Linqのサンプルです。
    /// </summary>
    [Sample]
    public class LinqSamples37 : IExecutable
    {
        public void Execute()
        {
            //
            // Aggregate拡張メソッド.
            //
            // Aggregate拡張メソッドは、指定されたseedを起点値としてfunc関数を繰り返し呼び出し
            // 結果をアキュムレーターに保持してくれるメソッド。
            //
            // pythonのmap関数みたいなものである。
            //
            // Aggregateは、独自の集計処理を行う場合に利用する。
            // 尚、Sum, Min, Max, Average拡張メソッドなどはAggregateの特殊なパターンといえる。
            // (つまり、全部Aggregateで同じように処理できる。）
            //
            // 利用する際の注意点としては、seedを明示的に指定しない場合、暗黙的に
            // ソースシーケンスの最初の要素をseedとして利用して処理してくれることである。
            // 通常の場合はこれで良いが、出来るだけseedは明示的に渡した方がよい。
            //

            //
            // Sum拡張メソッドの動きをAggregateで行う.
            //
            var query = Enumerable.Range(1, 10).Aggregate
            (
                0, // seed
                (a, s) => a + s // func
            );

            Output.WriteLine("========= Sum拡張メソッドの動作 ==========");
            Output.WriteLine("SUM = [{0}]", query);
            Output.WriteLine("======================================");

            //
            // 独自の集計処理を行ってみる.
            //   以下は各オーダーの発注最高額とその月を求める。
            //
            Output.WriteLine("========= 独自の集計処理実行 ==========");

            //
            // ソースシーケンス.
            //
            var orders = new[]
            {
                new Order {Name = "gsf_zero1", Amount = 1000, Month = 3},
                new Order {Name = "gsf_zero1", Amount = 600, Month = 4},
                new Order {Name = "gsf_zero1", Amount = 100, Month = 5},
                new Order {Name = "gsf_zero2", Amount = 100, Month = 3},
                new Order {Name = "gsf_zero2", Amount = 1000, Month = 4},
                new Order {Name = "gsf_zero2", Amount = 1200, Month = 5},
                new Order {Name = "gsf_zero3", Amount = 1000, Month = 3},
                new Order {Name = "gsf_zero3", Amount = 1200, Month = 4},
                new Order {Name = "gsf_zero3", Amount = 900, Month = 5}
            };

            //
            // 集計する前に、オーダー名単位でグループ化.
            //
            var orderGroupingQuery = from theOrder in orders
                    group theOrder by theOrder.Name;

            //
            // 最高発注額を求める。
            //
            var maxOrderQuery = from orderGroup in orderGroupingQuery
                    select
                    new
                    {
                        Name = orderGroup.Key,
                        MaxOrder = orderGroup.Aggregate
                        (
                            new {MaxAmount = 0, Month = 0}, // seed
                            (a, s) => a.MaxAmount > s.Amount // func
                                ? a
                                : new {MaxAmount = s.Amount, s.Month}
                        )
                    };

            foreach (var item in maxOrderQuery)
            {
                Output.WriteLine(item);
            }
            Output.WriteLine("======================================");
        }

        private class Order
        {
            public string Name { get; set; }
            public int Amount { get; set; }
            public int Month { get; set; }
        }
    }
}