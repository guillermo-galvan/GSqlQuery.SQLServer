namespace GSqlQuery.SQLServer.Test
{
    [Collection("GlobalTestServer")]
    public class SqlServerConnectionOptionsTest
    {
        [Fact]
        public void Create_SqlServerConnectionOptions_With_ConnectionString()
        {
            var sqlServerConnectionOptions = new SqlServerConnectionOptions(GlobalFixture.CONNECTIONSTRING);
            Assert.NotNull(sqlServerConnectionOptions);
        }

        [Fact]
        public void Create_SqlServerConnectionOptions_With_ConnectionString_and_events()
        {
            var sqlServerConnectionOptions = new SqlServerConnectionOptions(GlobalFixture.CONNECTIONSTRING, new SqlServerDatabaseManagementEventsCustom());
            Assert.NotNull(sqlServerConnectionOptions);
        }

        [Fact]
        public void Create_SqlServerConnectionOptions_With_formats_and_SqlServerDatabaseManagement()
        {
            var sqlServerConnectionOptions = new SqlServerConnectionOptions(new SqlServerFormats(), new SqlServerDatabaseManagement(GlobalFixture.CONNECTIONSTRING));
            Assert.NotNull(sqlServerConnectionOptions);
        }
    }
}
