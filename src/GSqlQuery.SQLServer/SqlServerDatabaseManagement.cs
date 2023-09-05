using GSqlQuery.Runner;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Data;
using System.Threading;
using System.Threading.Tasks;

namespace GSqlQuery.SQLServer
{
    public sealed class SqlServerDatabaseManagement : DatabaseManagement, IDatabaseManagement<SqlServerDatabaseConnection>
    {
        public SqlServerDatabaseManagement(string connectionString) :
            base(connectionString, new SqlServerDatabaseManagementEvents())
        { }

        public SqlServerDatabaseManagement(string connectionString, DatabaseManagementEvents events) : base(connectionString, events)
        { }

        public SqlServerDatabaseManagement(string connectionString, DatabaseManagementEvents events, ILogger logger) : base(connectionString, events, logger)
        { }

        public int ExecuteNonQuery(SqlServerDatabaseConnection connection, IQuery query, IEnumerable<IDataParameter> parameters)
        {
            return base.ExecuteNonQuery(connection, query, parameters);
        }

        public Task<int> ExecuteNonQueryAsync(SqlServerDatabaseConnection connection, IQuery query, IEnumerable<IDataParameter> parameters, CancellationToken cancellationToken = default)
        {
            return base.ExecuteNonQueryAsync(connection, query, parameters, cancellationToken);
        }

        public IEnumerable<T> ExecuteReader<T>(SqlServerDatabaseConnection connection, IQuery<T> query, IEnumerable<PropertyOptions> propertyOptions, IEnumerable<IDataParameter> parameters) 
            where T : class, new()
        {
            return base.ExecuteReader<T>(connection, query, propertyOptions, parameters);
        }

        public Task<IEnumerable<T>> ExecuteReaderAsync<T>(SqlServerDatabaseConnection connection, IQuery<T> query, IEnumerable<PropertyOptions> propertyOptions, IEnumerable<IDataParameter> parameters, CancellationToken cancellationToken = default) 
            where T : class, new()
        {
            return base.ExecuteReaderAsync<T>(connection, query, propertyOptions, parameters, cancellationToken);
        }

        public T ExecuteScalar<T>(SqlServerDatabaseConnection connection, IQuery query, IEnumerable<IDataParameter> parameters)
        {
            return base.ExecuteScalar<T>(connection, query, parameters);
        }

        public Task<T> ExecuteScalarAsync<T>(SqlServerDatabaseConnection connection, IQuery query, IEnumerable<IDataParameter> parameters, CancellationToken cancellationToken = default)
        {
            return base.ExecuteScalarAsync<T>(connection, query, parameters, cancellationToken);
        }

        public override IConnection GetConnection()
        {
            SqlServerDatabaseConnection databaseConnection = new SqlServerDatabaseConnection(_connectionString);

            if (databaseConnection.State != ConnectionState.Open)
            {
                databaseConnection.Open();
            }

            return databaseConnection;
        }

        SqlServerDatabaseConnection IDatabaseManagement<SqlServerDatabaseConnection>.GetConnection()
        {
            return (SqlServerDatabaseConnection)GetConnection();
        }

        public async override Task<IConnection> GetConnectionAsync(CancellationToken cancellationToken = default)
        {
            SqlServerDatabaseConnection databaseConnection = new SqlServerDatabaseConnection(_connectionString);

            if (databaseConnection.State != ConnectionState.Open)
            {
                await databaseConnection.OpenAsync(cancellationToken);
            }

            return databaseConnection;
        }

        async Task<SqlServerDatabaseConnection> IDatabaseManagement<SqlServerDatabaseConnection>.GetConnectionAsync(CancellationToken cancellationToken)
        {
            return (SqlServerDatabaseConnection)await GetConnectionAsync(cancellationToken);
        }
    }
}