using Microsoft.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;

namespace GSqlQuery.SQLServer
{
    public interface ISqlBulkCopyExecute : ISqlBulkCopy, IExecute<int>
    {
        int Execute(SqlConnection dbConnection);

        int Execute(SqlBulkCopyOptions sqlBulkCopyOptions);

        int Execute(SqlBulkCopyOptions sqlBulkCopyOptions, SqlTransaction sqlTransaction);

        Task<int> ExecuteAsync(SqlConnection dbConnection, CancellationToken cancellationToken = default);

        Task<int> ExecuteAsync(SqlBulkCopyOptions sqlBulkCopyOptions, CancellationToken cancellationToken = default);

        Task<int> ExecuteAsync(SqlBulkCopyOptions sqlBulkCopyOptions, SqlTransaction sqlTransaction, CancellationToken cancellationToken = default);

    }
}