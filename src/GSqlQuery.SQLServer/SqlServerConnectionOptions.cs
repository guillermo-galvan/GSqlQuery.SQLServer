using GSqlQuery.Runner;

namespace GSqlQuery.SQLServer
{
    public class SqlServerConnectionOptions : ConnectionOptions<SqlServerDatabaseConnection>
    {
        public SqlServerConnectionOptions(string connectionString) : base(new SqlServerStatements(),
            new SqlServerDatabaseManagement(connectionString))
        { }

        public SqlServerConnectionOptions(string connectionString, DatabaseManagementEvents events) :
            base(new SqlServerStatements(), new SqlServerDatabaseManagement(connectionString, events))
        { }

        public SqlServerConnectionOptions(IStatements statements, SqlServerDatabaseManagement sqlServerDatabaseManagement) :
            base(statements, sqlServerDatabaseManagement)
        {

        }
    }
}