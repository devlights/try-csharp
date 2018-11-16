using System.Data;
using TryCSharp.Common;

namespace TryCSharp.Samples.AdoNet
{
    /// <summary>
    ///     DataTableクラスに関するサンプルです。
    /// </summary>
    [Sample]
    public class DataTableSamples01 : IExecutable
    {
        public void Execute()
        {
            var table = new DataTable();

            table.Columns.Add("Val", typeof(decimal));

            for (var i = 0; i < 10; i++)
            {
                table.LoadDataRow(new object[] {i*0.1}, true);
            }

            //
            // 列は[]付きでも無しでも構わないが、付けておいた方が無難.
            //
            var result = table.Compute("SUM([Val])", "[Val] >= 0.5");
            Output.WriteLine("{0}:{1}", result, result.GetType().FullName);
        }
    }
}