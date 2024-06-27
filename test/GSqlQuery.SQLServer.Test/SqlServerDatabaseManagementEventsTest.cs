using GSqlQuery.SQLServer.Test.Data.Tables;

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
        public void GetParameter()
        {
            var query = Address.Select(_connectionOptions).Build();

            Queue<ParameterDetail> parameters = new Queue<ParameterDetail>();
            if (query.Criteria != null)
            {
                foreach (var item in query.Criteria.Where(x => x.ParameterDetails != null))
                {
                    foreach (var item2 in item.ParameterDetails)
                    {
                        parameters.Enqueue(item2);
                    }
                }
            }

            var result = _connectionOptions.DatabaseManagement.Events.GetParameter<Address>(parameters);
            Assert.NotNull(result);
            Assert.Equal(parameters.Count, result.Count());
        }

        [Fact]
        public void OnGetParameter()
        {
            var query = Film.Select(_connectionOptions).Build();

            Queue<ParameterDetail> parameters = new Queue<ParameterDetail>();
            if (query.Criteria != null)
            {
                foreach (var item in query.Criteria.Where(x => x.ParameterDetails != null))
                {
                    foreach (var item2 in item.ParameterDetails)
                    {
                        parameters.Enqueue(item2);
                    }
                }
            }

            var result = _connectionOptions.DatabaseManagement.Events.GetParameter<Film>(parameters);
            Assert.NotNull(result);
            Assert.Empty(result);
        }
    }
}
