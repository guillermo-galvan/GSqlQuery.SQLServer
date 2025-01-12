using GSqlQuery.Runner;
using GSqlQuery.Runner.TypeHandles;
using GSqlQuery.SQLServer.Benchmark.Data.Tables;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.SqlServer.Types;
using System.Data;
using System.Threading;
using System.Threading.Tasks;

namespace GSqlQuery.SQLServer.Benchmark
{
    internal class MySqlGeometryNullableTypeHandler : TypeHandler<SqlDataReader>
    {
        public override object GetValue(SqlDataReader reader, int ordinal)
        {
            return reader.IsDBNull(ordinal) ? null : (SqlGeometry)reader.GetValue(ordinal);
        }

        public override async Task<object> GetValueAsync(SqlDataReader reader, int ordinal, CancellationToken cancellationToken)
        {
            return await reader.IsDBNullAsync(ordinal, cancellationToken).ConfigureAwait(false) ? null : (SqlGeometry)reader.GetValue(ordinal);
        }

        protected override void SetDataType(IDataParameter dataParameter)
        {
            if (dataParameter is SqlParameter mySqlParameter)
            {
                mySqlParameter.SqlDbType = SqlDbType.Udt;
            }
            else
            {
                dataParameter.DbType = DbType.Object;
            }
        }
    }

    public class SqlServerDatabaseManagementEventsCustom : SqlServerDatabaseManagementEvents
    {
        private readonly ServiceProvider _serviceProvider;

        public SqlServerDatabaseManagementEventsCustom(ServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;

            if (!TypeHandleCollection.ContainsKey(typeof(SqlGeometry)))
            {
                TypeHandleCollection.Add(typeof(SqlGeometry), new MySqlGeometryNullableTypeHandler());
            }
        }

        public override ITransformTo<T, TDbDataReader> GetTransformTo<T, TDbDataReader>(ClassOptions classOptions)
        {
            return typeof(Actor) == typeof(T) ? (ITransformTo<T, TDbDataReader>)new Data.Transform.Actors() : _serviceProvider.GetService<ITransformTo<T, TDbDataReader>>();
        }
    }
}
