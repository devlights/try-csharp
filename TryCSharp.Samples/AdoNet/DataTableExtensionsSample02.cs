using System.Data;
using TryCSharp.Common;

namespace TryCSharp.Samples.AdoNet
{
    /// <summary>
    ///     System.Data.Extensionsのサンプル2です。
    /// </summary>
    [Sample]
    public class DataTableExtensionsSample02 : IExecutable
    {
        public void Execute()
        {
            var table = BuildSampleTable();

            //
            // 1列目の情報をint型で取得.
            // 2列目の情報をstring型で取得.
            //
            PrintTable("Before:", table);

            //
            // 1列目の情報を変更.
            //
            table.Rows[0].SetField("COL-1", 100);
            PrintTable("After:", table);
        }

        private DataTable BuildSampleTable()
        {
            var table = new DataTable();

            table.BeginInit();
            table.Columns.Add("COL-1", typeof(int));
            table.Columns.Add("COL-2");
            table.EndInit();

            table.BeginLoadData();
            for (var i = 0; i < 5; i++)
            {
                table.LoadDataRow(new object[] {i, (i + 1).ToString()}, true);
            }
            table.EndLoadData();

            return table;
        }

        private void PrintTable(string title, DataTable table)
        {
            Output.WriteLine(title);

            foreach (DataRow row in table.Rows)
            {
                Output.WriteLine("\t{0}, {1}", row[0], row[1]);
            }

            Output.WriteLine(string.Empty);
        }
    }
}