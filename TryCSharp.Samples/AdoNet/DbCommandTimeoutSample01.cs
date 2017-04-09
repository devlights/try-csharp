using System.Data.Common;
using System.Diagnostics;
using TryCSharp.Common;

namespace TryCSharp.Samples.AdoNet
{
    /// <summary>
    ///     DbCommandのタイムアウト機能についてのサンプルです。
    /// </summary>
    [Sample]
    public class DbCommandTimeoutSample01 : IExecutable
    {
        public void Execute()
        {
            var factory = DbProviderFactories.GetFactory("System.Data.SqlClient");
            using (var conn = factory.CreateConnection())
            {
                conn.ConnectionString = @"User Id=medal;Password=medal;Initial Catalog=Medal;Data Source=.\SQLEXPRESS";
                conn.Open();

                using (var command = conn.CreateCommand())
                {
                    command.CommandText =
                        @"SELECT a.*, b.* FROM MST_ZIP a FULL OUTER JOIN MST_ZIP b ON a.[publicCode] = b.[publicCode] WHERE a.[cmp] LIKE '%あ%' AND b.[cmpK] LIKE '%ｱ%'";
                    command.CommandTimeout = 1;

                    try
                    {
                        var watch = Stopwatch.StartNew();
                        using (var reader = command.ExecuteReader())
                        {
                            watch.Stop();

                            var count = 0;
                            for (; reader.Read(); count++)
                            {
                            }

                            Output.WriteLine("COUNT={0}, Elapsed={1}", count, watch.Elapsed);
                        }
                    }
                    catch (DbException ex)
                    {
                        Output.WriteLine(ex);
                    }
                }
            }
        }
    }
}