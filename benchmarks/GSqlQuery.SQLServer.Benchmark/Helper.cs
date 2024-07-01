using GSqlQuery.SQLServer.Benchmark.Data.Tables;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Smo;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;


namespace GSqlQuery.SQLServer.Benchmark
{
    internal class Helper
    {
        private static string connection = null;
        private readonly static Mutex mut = new Mutex();
        private static bool IsFirtsExecute = true;
        private static string connectionSakila = null;

        internal static void  GetConnectionsString()
        {
            if (string.IsNullOrWhiteSpace(connection))
            {
                var builder = new ConfigurationBuilder().AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                                                        .AddEnvironmentVariables().AddUserSecrets(typeof(Helper).GetTypeInfo().Assembly);

                var tmp = builder.Build();
                connection = tmp.GetConnectionString("TEST");
                connectionSakila = tmp.GetConnectionString("TESTSakila");
            }
        }

        internal static string GetConnectionString()
        {
            GetConnectionsString();
            return connectionSakila!;
        }

        internal static string GetConnectionStringMaster()
        {
            GetConnectionsString();
            return connection!;
        }

        internal static void CreateDataTable()
        {
            mut.WaitOne();

            if (IsFirtsExecute)
            {
                bool deleteTable = false;

                SqlServerConnectionOptions connectionOptions = new SqlServerConnectionOptions(GetConnectionStringMaster());

                IEnumerable<SysDataBases> tables = SysDataBases.Select(connectionOptions).Build().Execute();

                if (tables.Any(x => x.Name == "sakila"))
                {
                    deleteTable = true;
                }

                using (SqlConnection connection = new SqlConnection(GetConnectionStringMaster()))
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

                IsFirtsExecute = false;
            }

            mut.ReleaseMutex();
        }

        internal static SqlServerConnectionOptions GetConnectionOptions()
        {
            return new SqlServerConnectionOptions(GetConnectionString());
        }

        internal static SqlServerConnectionOptions GetConnectionOptions(ServiceProvider serviceProvider)
        {
            return new SqlServerConnectionOptions(GetConnectionString(), new SqlServerDatabaseManagementEventsCustom(serviceProvider));
        }
    }
}
