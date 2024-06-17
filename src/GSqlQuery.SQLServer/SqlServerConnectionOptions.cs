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

        public SqlServerConnectionOptions(IFormats formats, SqlServerDatabaseManagement sqlServerDatabaseManagement) :
            base(formats, sqlServerDatabaseManagement)
        {

        }
    }
}