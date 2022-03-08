using System.Data;
using System.Data.Common;
using TryCSharp.Common;
// ReSharper disable PossibleNullReferenceException

namespace TryCSharp.Samples.AdoNet
{
    /// <summary>
    ///     ADO.NETを利用してテキストファイルに接続するサンプルです。
    /// </summary>
    /// <remarks>
    ///     CSVファイルに接続し、各クエリ文を発行します。
    /// </remarks>
    [Sample]
    public class TextConnectSamples01 : IExecutable
    {
        public void Execute()
        {
            //
            // OleDbプロバイダを利用してテキストファイル(CSV)に接続する.
            //
            var factory = DbProviderFactories.GetFactory("System.Data.OleDb");
            using (var conn = factory.CreateConnection()!)
            {
                //
                // テキストファイルに接続する為の接続文字列を構築.
                //
                // 基本的にExcelに接続する場合とほぼ同じ要領となる。
                // Extended Properties内のISAMドライバがExcel 12.0からtextになる。
                // また、フォーマット方式を指定する必要がある。
                //
                // Data Sourceに指定するのは、該当ファイルが存在するディレクトリを指定する。
                // 尚、該当ファイルの構造については別途schema.iniファイルを同じディレクトリに
                // 用意する必要がある。
                //
                var builder = factory.CreateConnectionStringBuilder()!;

                builder["Provider"] = "Microsoft.ACE.OLEDB.12.0";
                builder["Data Source"] = @".";
                builder["Extended Properties"] = "text;HDR=YES;FMT=Delimited";

                conn.ConnectionString = builder.ToString();
                conn.Open();

                //
                // SELECT.
                // FROM句の中に読み込む対象のファイル名を指定する。
                // データが取得される際にschema.iniファイルが参照され、列定義が行われる。
                //
                using (var command = conn.CreateCommand())
                {
                    command.CommandText = "SELECT * FROM [resources\\Persons.txt]";

                    var table = new DataTable();
                    using (var reader = command.ExecuteReader())
                    {
                        table.Load(reader);
                    }

                    foreach (DataRow row in table.Rows)
                    {
                        Output.WriteLine("{0},{1},{2}", row["ID"], row["NAME"], row["AGE"]);
                    }
                }

                //
                // INSERT.
                // Accessの場合と同じくテキストファイルは追加・参照しか出来ない。
                // （つまり、更新・削除は出来ない）
                //
                using (var command = conn.CreateCommand())
                {
                    var query = string.Empty;

                    query += " INSERT INTO [resources\\Persons.txt] ";
                    query += "   (ID, NAME, AGE) ";
                    query += "   VALUES ";
                    query += "   (7, 'gsf_zero7', 37) ";

                    command.CommandText = query;
                    command.ExecuteNonQuery();

                    using (var command2 = conn.CreateCommand())
                    {
                        command2.CommandText = "SELECT * FROM [resources\\Persons.txt]";

                        var table = new DataTable();
                        using (var reader = command2.ExecuteReader())
                        {
                            table.Load(reader);
                        }

                        foreach (DataRow row in table.Rows)
                        {
                            Output.WriteLine("{0},{1},{2}", row["ID"], row["NAME"], row["AGE"]);
                        }
                    }
                }

                //
                // UPDATE
                // この ISAM では、リンク テーブル内のデータを更新することはできません。
                //

                //
                // DELETE.
                // この ISAM では、リンク テーブル内のデータを削除することはできません。
                //
            }
        }
    }
}