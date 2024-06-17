using GSqlQuery.Runner;
using Microsoft.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;

namespace GSqlQuery.SQLServer
{
    public sealed class SqlServerDatabaseManagement : 
        DatabaseManagement<SqlServerDatabaseConnection, SqlServerDatabaseTransaction, SqlCommand, SqlTransaction, SqlDataReader>, IDatabaseManagement<SqlServerDatabaseConnection>
    {
        public SqlServerDatabaseManagement(string connectionString) :
            base(connectionString, new SqlServerDatabaseManagementEvents())
        { }

        public SqlServerDatabaseManagement(string connectionString, DatabaseManagementEvents events) : base(connectionString, events)
        { }

        public override SqlServerDatabaseConnection GetConnection()
        {
            SqlServerDatabaseConnection sqlConnection = new SqlServerDatabaseConnection(_connectionString);

            if (sqlConnection.State != System.Data.ConnectionState.Open)
            {
                sqlConnection.Open();
            }

            return sqlConnection;
        }

        public override async Task<SqlServerDatabaseConnection> GetConnectionAsync(CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            SqlServerDatabaseConnection sqlConnection = new SqlServerDatabaseConnection(_connectionString);

            if (sqlConnection.State != System.Data.ConnectionState.Open)
            {
                await sqlConnection.OpenAsync(cancellationToken);
            }

            return sqlConnection;
        }
    }
}