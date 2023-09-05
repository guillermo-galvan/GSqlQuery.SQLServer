using GSqlQuery.Runner;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Threading;
using System.Threading.Tasks;

namespace GSqlQuery.SQLServer
{
    public sealed class SqlServerDatabaseConnection : Connection, IConnection
    {
        public SqlServerDatabaseConnection(string connectionString) : base(new SqlConnection(connectionString))
        { }

        public SqlServerDatabaseTransaction BeginTransaction()
        {
            return (SqlServerDatabaseTransaction)SetTransaction(new SqlServerDatabaseTransaction(this, ((SqlConnection)_connection).BeginTransaction()));
        }

        public SqlServerDatabaseTransaction BeginTransaction(IsolationLevel isolationLevel)
        {
            return (SqlServerDatabaseTransaction)SetTransaction(new SqlServerDatabaseTransaction(this, ((SqlConnection)_connection).BeginTransaction(isolationLevel)));
        }


#if NET5_0_OR_GREATER
        public async Task<SqlServerDatabaseTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            return (SqlServerDatabaseTransaction)SetTransaction(new SqlServerDatabaseTransaction(this, await ((SqlConnection)_connection).BeginTransactionAsync(cancellationToken)));
        }
#else
        public Task<SqlServerDatabaseTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            return Task.FromResult((SqlServerDatabaseTransaction)SetTransaction(new SqlServerDatabaseTransaction(this, ((SqlConnection)_connection).BeginTransaction())));
        }
#endif

#if NET5_0_OR_GREATER
        public async Task<SqlServerDatabaseTransaction> BeginTransactionAsync(IsolationLevel isolationLevel, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            return
                (SqlServerDatabaseTransaction)SetTransaction(new SqlServerDatabaseTransaction(this,
                await ((SqlConnection)_connection).BeginTransactionAsync(isolationLevel, cancellationToken)));
        }
#else
        public Task<SqlServerDatabaseTransaction> BeginTransactionAsync(IsolationLevel isolationLevel, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            return Task.FromResult((SqlServerDatabaseTransaction)SetTransaction(new SqlServerDatabaseTransaction(this, ((SqlConnection)_connection).BeginTransaction())));
        }
#endif
        ITransaction IConnection.BeginTransaction() => BeginTransaction();

        ITransaction IConnection.BeginTransaction(IsolationLevel isolationLevel) => BeginTransaction(isolationLevel);

        async Task<ITransaction> IConnection.BeginTransactionAsync(CancellationToken cancellationToken = default) => await BeginTransactionAsync(cancellationToken);

        async Task<ITransaction> IConnection.BeginTransactionAsync(IsolationLevel isolationLevel, CancellationToken cancellationToken = default) =>
            await BeginTransactionAsync(isolationLevel, cancellationToken);

        ~SqlServerDatabaseConnection()
        {
            Dispose(disposing: false);
        }
    }
}