using GSqlQuery.SQLServer.Test.Data.Tables;
using Microsoft.Data.SqlClient;
using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Smo;
using Testcontainers.MsSql;

namespace GSqlQuery.SQLServer.Test
{
    public class GlobalFixture : IAsyncLifetime
    {
        public const string CONNECTIONSTRING = "Server=127.0.0.1,9000;Database=sakila;User Id=sa;Password=sadmin@!123;TrustServerCertificate=True";

        private readonly MsSqlContainer _msSqlContainer = new MsSqlBuilder()
            .WithImage("mcr.microsoft.com/mssql/server:2022-latest")
            .WithPortBinding(9000, 1433)
            .WithName("GSqlQuery.SQLServer-db-test")
            .WithPassword("sadmin@!123")
            .WithCleanUp(true)
            .Build();

        public async Task InitializeAsync()
        {
            await _msSqlContainer.StartAsync();

            CreateDataTable();
        }

        public async Task DisposeAsync()
        {
            await _msSqlContainer.StopAsync();
        }

        internal void CreateDataTable()
        {
            bool deleteTable = false;

            SqlServerConnectionOptions connectionOptions = new SqlServerConnectionOptions(_msSqlContainer.GetConnectionString());

            IEnumerable<SysDataBases> tables = SysDataBases.Select(connectionOptions).Build().Execute();

            if (tables.Any(x => x.Name == "sakila"))
            {
                deleteTable = true;
            }

            using (SqlConnection connection = new SqlConnection(_msSqlContainer.GetConnectionString()))
            {
                connection.Open();
                string path = string.Empty;
                Server server = null;

                if (deleteTable)
                {
                    path = Path.Combine(AppDomain.CurrentDomain.SetupInformation.ApplicationBase!, "Data", "Scripts", "DeleteDataBase.sql");
                    server = new Server(new ServerConnection(connection));

                    server.ConnectionContext.ExecuteNonQuery(File.ReadAllText(path));
                    server = null;
                }

                path = Path.Combine(AppDomain.CurrentDomain.SetupInformation.ApplicationBase!, "Data", "Scripts", "CreateDataBase.sql");

                server = new Server(new ServerConnection(connection));

                string script = File.ReadAllText(path);
                server.ConnectionContext.ExecuteNonQuery(script);
                server = null;


                path = Path.Combine(AppDomain.CurrentDomain.SetupInformation.ApplicationBase!, "Data", "Scripts", "CreateTables.sql");
                server = new Server(new ServerConnection(connection));
                script = File.ReadAllText(path);
                server.ConnectionContext.ExecuteNonQuery(script);
                server = null;
            }
        }
    }
}
