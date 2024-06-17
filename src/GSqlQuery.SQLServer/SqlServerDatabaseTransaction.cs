using GSqlQuery.Runner;
using Microsoft.Data.SqlClient;

namespace GSqlQuery.SQLServer
{
    public sealed class SqlServerDatabaseTransaction(SqlServerDatabaseConnection connection, SqlTransaction transaction) : 
        Transaction<SqlServerDatabaseConnection, SqlCommand, SqlTransaction, SqlConnection>(connection, transaction),
        ITransaction<SqlServerDatabaseConnection, SqlTransaction>
    {
        ~SqlServerDatabaseTransaction()
        {
            Dispose(disposing: false);
        }
    }
}