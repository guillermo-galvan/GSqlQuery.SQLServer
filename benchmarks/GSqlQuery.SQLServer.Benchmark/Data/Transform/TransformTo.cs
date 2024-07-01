using GSqlQuery.Runner.Transforms;
using Microsoft.Data.SqlClient;
using Microsoft.SqlServer.Types;

namespace GSqlQuery.SQLServer.Benchmark.Data.Transform
{
    internal abstract class TransformTo<T>(int numColumns) : TransformTo<T, SqlDataReader>(numColumns) where T : class
    {
        protected T1 GetValue<T1>(PropertyOptionsInEntity column, SqlDataReader reader)
        {
            if (column == null)
            {
                return default;
            }
            else if (!column.Ordinal.HasValue)
            {
                return (T1)column.DefaultValue;
            }
            else
            {
                var type = typeof(T1);

                if (type == typeof(SqlGeometry))
                {
                    object result = (SqlGeometry)reader.GetValue(column.Ordinal.Value);
                    return (T1)result;
                }

                var value = reader.GetValue(column.Ordinal.Value);
                return (T1)TransformTo.SwitchTypeValue(type, value);
            }
        }
    }
}
