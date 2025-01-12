using GSqlQuery.Runner;
using GSqlQuery.SQLServer.Test.Data.Tables;
using GSqlQuery.SQLServer.Test.Transform;
using GSqlQuery.SQLServer.Test.TypeHandles;
using Microsoft.SqlServer.Types;

namespace GSqlQuery.SQLServer.Test
{
    public class SqlServerDatabaseManagementEventsCustom : SqlServerDatabaseManagementEvents
    {
        public SqlServerDatabaseManagementEventsCustom()
        {
            if (!TypeHandleCollection.ContainsKey(typeof(SqlGeometry)))
            {
                TypeHandleCollection.Add(typeof(SqlGeometry), new MySqlGeometryNullableTypeHandler());
            }
        }

        public override ITransformTo<T, TDbDataReader> GetTransformTo<T, TDbDataReader>(ClassOptions classOptions)
        {
            if (typeof(T) == typeof(Address))
            {
                return (ITransformTo<T, TDbDataReader>)new AddressTransform();
            }

            return base.GetTransformTo<T, TDbDataReader>(classOptions);
        }
    }
}
