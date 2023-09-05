using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace GSqlQuery.SQLServer
{
    internal class BulkInsertExecute : ISqlBulkCopyExecute
    {
        private readonly Queue<DataTable> _tables;
        private readonly string _connectionString;

        public IDatabaseManagement<SqlServerDatabaseConnection> DatabaseManagement { get; }

        IDatabaseManagement<SqlConnection> IExecute<int, SqlConnection>.DatabaseManagement => throw new NotImplementedException();

        public BulkInsertExecute(string connectionString)
        {
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new ArgumentNullException(nameof(connectionString));
            }

            _connectionString = connectionString;
            _tables = new Queue<DataTable>();
            DatabaseManagement = new SqlServerDatabaseManagement(_connectionString);
        }

        public int Execute()
        {
            WriteToServerBulkCopy(new SqlBulkCopy(_connectionString));
            return _tables.Sum(x => x.Rows.Count);
        }

        public int Execute(SqlConnection dbConnection)
        {
            ValidConnection(dbConnection);
            WriteToServerBulkCopy(new SqlBulkCopy(dbConnection));
            return _tables.Sum(x => x.Rows.Count);
        }

        public int Execute(SqlBulkCopyOptions sqlBulkCopyOptions)
        {
            WriteToServerBulkCopy(new SqlBulkCopy(_connectionString, sqlBulkCopyOptions));
            return _tables.Sum(x => x.Rows.Count);
        }

        public int Execute(SqlBulkCopyOptions sqlBulkCopyOptions, SqlTransaction sqlTransaction)
        {
            ValidTransaction(sqlTransaction);
            WriteToServerBulkCopy(new SqlBulkCopy(sqlTransaction.Connection, sqlBulkCopyOptions, sqlTransaction));
            return _tables.Sum(x => x.Rows.Count);
        }

        public async Task<int> ExecuteAsync(CancellationToken cancellationToken = default)
        {
            await WriteToServerBulkCopyAsync(new SqlBulkCopy(_connectionString), cancellationToken);
            return _tables.Sum(x => x.Rows.Count);
        }

        public async Task<int> ExecuteAsync(SqlConnection dbConnection, CancellationToken cancellationToken = default)
        {
            ValidConnection(dbConnection);
            await WriteToServerBulkCopyAsync(new SqlBulkCopy(dbConnection), cancellationToken);
            return _tables.Sum(x => x.Rows.Count);
        }

        public async Task<int> ExecuteAsync(SqlBulkCopyOptions sqlBulkCopyOptions, CancellationToken cancellationToken = default)
        {
            await WriteToServerBulkCopyAsync(new SqlBulkCopy(_connectionString, sqlBulkCopyOptions), cancellationToken);
            return _tables.Sum(x => x.Rows.Count);
        }

        public async Task<int> ExecuteAsync(SqlBulkCopyOptions sqlBulkCopyOptions, SqlTransaction sqlTransaction, CancellationToken cancellationToken = default)
        {
            ValidTransaction(sqlTransaction);
            await WriteToServerBulkCopyAsync(new SqlBulkCopy(sqlTransaction.Connection, sqlBulkCopyOptions, sqlTransaction), cancellationToken);
            return _tables.Sum(x => x.Rows.Count);
        }

        public ISqlBulkCopyExecute Copy<T>(IEnumerable<T> values)
        {
            _tables.Enqueue(BulkCopyExtension.FillTable(values));
            return this;
        }

        private void WriteToServerBulkCopy(SqlBulkCopy bulkCopy)
        {
            using (bulkCopy)
            {
                foreach (var item in _tables)
                {
                    bulkCopy.DestinationTableName = item.TableName;
                    bulkCopy.WriteToServer(item);
                }
            }
        }

        private async Task WriteToServerBulkCopyAsync(SqlBulkCopy bulkCopy, CancellationToken cancellationToken)
        {
            using (bulkCopy)
            {
                cancellationToken.ThrowIfCancellationRequested();
                foreach (var item in _tables)
                {
                    bulkCopy.DestinationTableName = item.TableName;
                    await bulkCopy.WriteToServerAsync(item, cancellationToken);
                }
            }
        }

        IBulkCopyExecute IBulkCopy.Copy<T>(IEnumerable<T> values) => Copy(values);

        private void ValidConnection(SqlConnection sqlConnection)
        {

            if (sqlConnection == null)
            {
                throw new ArgumentNullException(nameof(sqlConnection));
            }

            if (sqlConnection.State != ConnectionState.Open)
            {
                throw new InvalidOperationException("The connection is not open");
            }

        }

        private void ValidTransaction(SqlTransaction sqlTransaction)
        {
            if (sqlTransaction == null)
            {
                throw new ArgumentNullException(nameof(sqlTransaction));
            }

            ValidConnection(sqlTransaction.Connection);
        }
    }
}