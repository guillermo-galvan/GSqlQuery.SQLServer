using GSqlQuery.Runner;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Threading;
using System.Threading.Tasks;

namespace GSqlQuery.SQLServer
{
    public sealed class SqlServerDatabaseManagement : DatabaseManagement<SqlServerDatabaseConnection, SqlServerDatabaseTransaction, SqlCommand, SqlTransaction, SqlDataReader>, IDatabaseManagement<SqlServerDatabaseConnection>
    {
        public SqlServerDatabaseManagement(string connectionString) :
            base(connectionString, new SqlServerDatabaseManagementEvents())
        { }

        public SqlServerDatabaseManagement(string connectionString, SqlServerDatabaseManagementEvents events) : base(connectionString, events)
        { }

        public override SqlServerDatabaseConnection GetConnection()
        {
            SqlServerDatabaseConnection sqlConnection = new SqlServerDatabaseConnection(_connectionString);

            if (sqlConnection.State != ConnectionState.Open)
            {
                sqlConnection.Open();
            }

            return sqlConnection;
        }

        public override async Task<SqlServerDatabaseConnection> GetConnectionAsync(CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            SqlServerDatabaseConnection sqlConnection = new SqlServerDatabaseConnection(_connectionString);

            if (sqlConnection.State != ConnectionState.Open)
            {
                await sqlConnection.OpenAsync(cancellationToken).ConfigureAwait(false);
            }

            return sqlConnection;
        }
        public override async Task<int> ExecuteNonQueryAsync(SqlServerDatabaseConnection connection, IQuery query, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            using (SqlCommand command = CreateCommand(connection, query))
            {
                if (Events.IsTraceActive)
                {
                    Events.WriteTrace("ExecuteNonQueryAsync Query: {@Text} Parameters: {@parameters}", [query.Text, command.Parameters]);
                }

                return await command.ExecuteNonQueryAsync(cancellationToken).ConfigureAwait(false);
            }
        }
    }
}