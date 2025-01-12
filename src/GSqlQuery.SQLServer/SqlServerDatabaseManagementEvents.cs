using GSqlQuery.Runner;
using GSqlQuery.Runner.TypeHandles;
using Microsoft.Data.SqlClient;
using System;

namespace GSqlQuery.SQLServer
{
    public class SqlServerDatabaseManagementEvents : DatabaseManagementEvents
    {
        public static readonly TypeHandleCollection<SqlDataReader> TypeHandleCollection = TypeHandleCollection<SqlDataReader>.Instance;

        public SqlServerDatabaseManagementEvents()
        {}

        protected override ITypeHandler<TDbDataReader> GetTypeHandler<TDbDataReader>(Type property)
        {
            return (ITypeHandler<TDbDataReader>)TypeHandleCollection.GetTypeHandler(property);
        }
    }
}