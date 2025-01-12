namespace GSqlQuery.SQLServer.Test
{
    public class SqlServerConnectionOptionsTest
    {
        [Fact]
        public void Create_SqlServerConnectionOptions_With_ConnectionString()
        {
            var sqlServerConnectionOptions = new SqlServerConnectionOptions(Helper.GetConnectionString());
            Assert.NotNull(sqlServerConnectionOptions);
        }

        [Fact]
        public void Create_SqlServerConnectionOptions_With_ConnectionString_and_events()
        {
            var sqlServerConnectionOptions = new SqlServerConnectionOptions(Helper.GetConnectionString(), new SqlServerDatabaseManagementEventsCustom());
            Assert.NotNull(sqlServerConnectionOptions);
        }

        [Fact]
        public void Create_SqlServerConnectionOptions_With_formats_and_SqlServerDatabaseManagement()
        {
            var sqlServerConnectionOptions = new SqlServerConnectionOptions(new SqlServerFormats(), new SqlServerDatabaseManagement(Helper.GetConnectionString()));
            Assert.NotNull(sqlServerConnectionOptions);
        }
    }
}
