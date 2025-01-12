using GSqlQuery.Runner.TypeHandles;
using Microsoft.Data.SqlClient;
using Microsoft.SqlServer.Types;
using System.Data;

namespace GSqlQuery.SQLServer.Test.TypeHandles
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
            if (dataParameter is SqlParameter sqlParameter)
            {
                sqlParameter.SqlDbType = SqlDbType.Udt;
                sqlParameter.UdtTypeName = "Geometry";
            }
            else
            {
                dataParameter.DbType = DbType.Object;
            }
        }
    }
}
