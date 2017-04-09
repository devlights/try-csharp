using System.Data;
using TryCSharp.Common;

namespace TryCSharp.Samples.AdoNet
{
    /// <summary>
    ///     DataTableクラスに関するサンプルです。
    /// </summary>
    [Sample]
    public class DataTableSamples03 : IExecutable
    {
        public void Execute()
        {
            var tableA = new DataTable();

            tableA.Columns.Add("Val", typeof(int));

            for (var i = 0; i < 10; i++)
            {
                tableA.LoadDataRow(new object[] {i}, true);
            }

            Output.WriteLine("[TableA]ColumnCount = {0}", tableA.Columns.Count);
            Output.WriteLine("[TableA]RowCount = {0}", tableA.Rows.Count);

            //
            // tableAのスキーマをtableBにコピー.
            // (データはコピーしない。)
            //
            var tableB = tableA.Clone();
            Output.WriteLine("[TableB]ColumnCount = {0}", tableB.Columns.Count);
            Output.WriteLine("[TableB]RowCount = {0}", tableB.Rows.Count);

            //
            // tableAのスキーマとデータをtableCにコピー.
            //
            var tableC = tableA.Copy();
            Output.WriteLine("[TableC]ColumnCount = {0}", tableC.Columns.Count);
            Output.WriteLine("[TableC]RowCount = {0}", tableC.Rows.Count);
        }
    }
}