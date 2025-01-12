using GSqlQuery.SQLServer.Test.Data.Tables;
using Microsoft.Data.SqlClient;

namespace GSqlQuery.SQLServer.Test
{
    public class SqlServerDatabaseManagementEventsTest
    {
        private readonly SqlServerConnectionOptions _connectionOptions;

        public SqlServerDatabaseManagementEventsTest()
        {
            _connectionOptions = new SqlServerConnectionOptions(Helper.GetConnectionString(), new SqlServerDatabaseManagementEventsCustom());
        }

        [Fact]
        public void GetTransformTo_ReturnsNotNull_ForAddressType()
        {
            var classOptions = ClassOptionsFactory.GetClassOptions(typeof(Address));
            var result = _connectionOptions.DatabaseManagement.Events.GetTransformTo<Address, SqlDataReader>(classOptions);
            Assert.NotNull(result);
        }

        [Fact]
        public void GetTransformTo_ReturnsNotNull_ForFilmType()
        {
            var classOptions = ClassOptionsFactory.GetClassOptions(typeof(Film));
            var result = _connectionOptions.DatabaseManagement.Events.GetTransformTo<Film, SqlDataReader>(classOptions);
            Assert.NotNull(result);
        }
    }
}
