using GSqlQuery.Runner;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Threading;
using System.Threading.Tasks;

namespace GSqlQuery.SQLServer
{
    public sealed class SqlServerDatabaseConnection(string connectionString) : 
        Connection<SqlServerDatabaseTransaction, SqlConnection, SqlTransaction, SqlCommand>(new SqlConnection(connectionString)), 
        IConnection<SqlServerDatabaseTransaction, SqlCommand>
    {
        public override Task CloseAsync(CancellationToken cancellationToken = default)
        {
#if NET5_0_OR_GREATER
            cancellationToken.ThrowIfCancellationRequested();
            return _connection.CloseAsync();
#else
            return base.CloseAsync(cancellationToken);
#endif
        }

        public override SqlServerDatabaseTransaction BeginTransaction()
        {
            return SetTransaction(new SqlServerDatabaseTransaction(this, _connection.BeginTransaction()));
        }

        public override SqlServerDatabaseTransaction BeginTransaction(IsolationLevel isolationLevel)
        {
            return SetTransaction(new SqlServerDatabaseTransaction(this, _connection.BeginTransaction(isolationLevel)));
        }

        public override Task<SqlServerDatabaseTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            return Task.FromResult(SetTransaction(new SqlServerDatabaseTransaction(this, _connection.BeginTransaction())));
        }

        public override Task<SqlServerDatabaseTransaction> BeginTransactionAsync(IsolationLevel isolationLevel, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            return Task.FromResult(SetTransaction(new SqlServerDatabaseTransaction(this, _connection.BeginTransaction(isolationLevel))));
        }

        ~SqlServerDatabaseConnection()
        {
            Dispose(disposing: false);
        }
    }
}