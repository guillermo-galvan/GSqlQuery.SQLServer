using GSqlQuery.SQLServer.Test.Data.Tables;
using Microsoft.Data.SqlClient;

namespace GSqlQuery.SQLServer.Test.BulkCopy
{
    public class BulkCopyFactoryTest
    {
        private readonly string CONNECTIONSTRING = Helper.GetConnectionString();

        private readonly IEnumerable<Actor> _actors;
        private readonly IEnumerable<Customer> _customers;
        private readonly SqlServerConnectionOptions _connection;

        public BulkCopyFactoryTest()
        {
            Helper.CreateDataTable();
            _connection = new SqlServerConnectionOptions(Helper.GetConnectionString(), new SqlServerDatabaseManagementEventsCustom());
            _actors = Actor.Select(_connection).Build().Execute();
            _customers = Customer.Select(_connection).Build().Execute();
        }

        [Fact]
        public void Throw_exeception()
        {
            string connectionString = null;
            Assert.Throws<ArgumentNullException>(() => BulkCopyFactory.Create(connectionString));
            Assert.Throws<ArgumentNullException>(() => BulkCopyFactory.Create(string.Empty));
        }

        [Fact]
        public void Execute()
        {
            var beforeActorTotal = Actor.Select(_connection, x => new { x.ActorId }).Count().Build().Execute();
            var beforeCustomersTotal = Customer.Select(_connection, x => new { x.CustomerId }).Count().Build().Execute();

            var bulkcopy = BulkCopyFactory.Create(CONNECTIONSTRING).Copy(_customers).Copy(_actors).Execute();

            var actorTotal = Actor.Select(_connection, x => new { x.ActorId }).Count().Build().Execute();
            var customersTotal = Customer.Select(_connection, x => new { x.CustomerId }).Count().Build().Execute();

            Assert.True(bulkcopy > 0);
            Assert.True(actorTotal > beforeActorTotal);
            Assert.True(customersTotal > beforeCustomersTotal);
        }

        [Fact]
        public void Execute_with_connection()
        {
            using (SqlConnection connection = new SqlConnection(CONNECTIONSTRING))
            {
                connection.Open();
                var beforeActorTotal = Actor.Select(_connection, x => new { x.ActorId }).Count().Build().Execute();
                var beforeCustomersTotal = Customer.Select(_connection, x => new { x.CustomerId }).Count().Build().Execute();

                var bulkcopy = BulkCopyFactory.Create(CONNECTIONSTRING).Copy(_customers).Copy(_actors).Execute(connection);

                var actorTotal = Actor.Select(_connection, x => new { x.ActorId }).Count().Build().Execute();
                var customersTotal = Customer.Select(_connection, x => new { x.CustomerId }).Count().Build().Execute();
                connection.Close();

                Assert.True(bulkcopy > 0);
                Assert.True(actorTotal > beforeActorTotal);
                Assert.True(customersTotal > beforeCustomersTotal);
            }
        }

        [Fact]
        public void Execute_with_connection_Throw_exeception()
        {
            Assert.Throws<ArgumentNullException>(() => BulkCopyFactory.Create(CONNECTIONSTRING).Copy(_actors).Execute(null));
        }

        [Fact]
        public async Task ExecuteAsync()
        {
            var beforeActorTotal = await Actor.Select(_connection, x => new { x.ActorId }).Count().Build().ExecuteAsync();
            var beforeCustomersTotal = await Customer.Select(_connection, x => new { x.CustomerId }).Count().Build().ExecuteAsync();

            var bulkcopy = await BulkCopyFactory.Create(CONNECTIONSTRING).Copy(_customers).Copy(_actors).ExecuteAsync();

            var actorTotal = await Actor.Select(_connection, x => new { x.ActorId }).Count().Build().ExecuteAsync();
            var customersTotal = await Customer.Select(_connection, x => new { x.CustomerId }).Count().Build().ExecuteAsync();

            Assert.True(bulkcopy > 0);
            Assert.True(actorTotal > beforeActorTotal);
            Assert.True(customersTotal > beforeCustomersTotal);
        }

        [Fact]
        public async Task ExecuteAsync_with_connection()
        {
            using (SqlConnection connection = new SqlConnection(CONNECTIONSTRING))
            {
                await connection.OpenAsync();

                var beforeActorTotal = await Actor.Select(_connection, x => new { x.ActorId }).Count().Build().ExecuteAsync();
                var beforeCustomersTotal = await Customer.Select(_connection, x => new { x.CustomerId }).Count().Build().ExecuteAsync();

                var bulkcopy = await BulkCopyFactory.Create(CONNECTIONSTRING).Copy(_customers).Copy(_actors).ExecuteAsync(connection);

                var actorTotal = await Actor.Select(_connection, x => new { x.ActorId }).Count().Build().ExecuteAsync();
                var customersTotal = await Customer.Select(_connection, x => new { x.CustomerId }).Count().Build().ExecuteAsync();
                connection.Close();

                Assert.True(bulkcopy > 0);
                Assert.True(actorTotal > beforeActorTotal);
                Assert.True(customersTotal > beforeCustomersTotal);

            }
        }

        [Fact]
        public async Task ExecuteAsync_with_connection_Throw_exeception()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(async () => await BulkCopyFactory.Create(CONNECTIONSTRING).Copy(_customers).ExecuteAsync(null));
            using (SqlConnection connection = new SqlConnection(CONNECTIONSTRING))
            {
                var beforeActorTotal = Actor.Select(_connection, x => new { x.ActorId }).Count().Build().Execute();
                var beforeCustomersTotal = Customer.Select(_connection, x => new { x.CustomerId }).Count().Build().Execute();

                await Assert.ThrowsAsync<InvalidOperationException>(async () => await BulkCopyFactory.Create(CONNECTIONSTRING).Copy(_customers).ExecuteAsync(connection));

                SqlTransaction sqlTransaction = null;
                await Assert.ThrowsAsync<ArgumentNullException>(async () => await BulkCopyFactory.Create(CONNECTIONSTRING).Copy(_customers).ExecuteAsync(SqlBulkCopyOptions.Default, sqlTransaction));
            }


        }

        [Fact]
        public void Execute_with_SqlBulkCopyOptions()
        {
            var beforeActorTotal = Actor.Select(_connection, x => new { x.ActorId }).Count().Build().Execute();
            var beforeCustomersTotal = Customer.Select(_connection, x => new { x.CustomerId }).Count().Build().Execute();

            var bulkcopy = BulkCopyFactory.Create(CONNECTIONSTRING).Copy(_customers).Copy(_actors).Execute(SqlBulkCopyOptions.Default);

            var actorTotal = Actor.Select(_connection, x => new { x.ActorId }).Count().Build().Execute();
            var customersTotal = Customer.Select(_connection, x => new { x.CustomerId }).Count().Build().Execute();

            Assert.True(bulkcopy > 0);
            Assert.True(actorTotal > beforeActorTotal);
            Assert.True(customersTotal > beforeCustomersTotal);
        }

        [Fact]
        public void Execute_with_SqlBulkCopyOptions_and_SqlTransaction()
        {
            using (SqlConnection connection = new SqlConnection(CONNECTIONSTRING))
            {
                connection.Open();
                var beforeActorTotal = Actor.Select(_connection, x => new { x.ActorId }).Count().Build().Execute();
                var beforeCustomersTotal = Customer.Select(_connection, x => new { x.CustomerId }).Count().Build().Execute();
                int bulkcopy = 0;
                using (SqlTransaction sqlTransaction = connection.BeginTransaction())
                {
                    bulkcopy = BulkCopyFactory.Create(CONNECTIONSTRING).Copy(_customers).Copy(_actors).Execute(SqlBulkCopyOptions.Default, sqlTransaction);
                    sqlTransaction.Commit();
                }


                var actorTotal = Actor.Select(_connection, x => new { x.ActorId }).Count().Build().Execute();
                var customersTotal = Customer.Select(_connection, x => new { x.CustomerId }).Count().Build().Execute();

                Assert.True(bulkcopy > 0);
                Assert.True(actorTotal > beforeActorTotal);
                Assert.True(customersTotal > beforeCustomersTotal);
            }
        }

        [Fact]
        public async Task ExecuteAsync_with_SqlBulkCopyOptions()
        {
            var beforeActorTotal = await Actor.Select(_connection, x => new { x.ActorId }).Count().Build().ExecuteAsync();
            var beforeCustomersTotal = await Customer.Select(_connection, x => new { x.CustomerId }).Count().Build().ExecuteAsync();

            var bulkcopy = await BulkCopyFactory.Create(CONNECTIONSTRING).Copy(_customers).Copy(_actors).ExecuteAsync(SqlBulkCopyOptions.Default);

            var actorTotal = await Actor.Select(_connection, x => new { x.ActorId }).Count().Build().ExecuteAsync();
            var customersTotal = await Customer.Select(_connection, x => new { x.CustomerId }).Count().Build().ExecuteAsync();

            Assert.True(bulkcopy > 0);
            Assert.True(actorTotal > beforeActorTotal);
            Assert.True(customersTotal > beforeCustomersTotal);
        }

        [Fact]
        public async Task ExecuteAsync_with_SqlBulkCopyOptions_and_SqlTransaction()
        {
            using (SqlConnection connection = new SqlConnection(CONNECTIONSTRING))
            {
                await connection.OpenAsync();
                var beforeActorTotal = await Actor.Select(_connection, x => new { x.ActorId }).Count().Build().ExecuteAsync();
                var beforeCustomersTotal = await Customer.Select(_connection, x => new { x.CustomerId }).Count().Build().ExecuteAsync();
                int bulkcopy = 0;
                using (SqlTransaction sqlTransaction = connection.BeginTransaction())
                {
                    bulkcopy = await BulkCopyFactory.Create(CONNECTIONSTRING).Copy(_customers).Copy(_actors).ExecuteAsync(SqlBulkCopyOptions.Default, sqlTransaction);
                    sqlTransaction.Commit();
                }


                var actorTotal = await Actor.Select(_connection, x => new { x.ActorId }).Count().Build().ExecuteAsync();
                var customersTotal = await Customer.Select(_connection, x => new { x.CustomerId }).Count().Build().ExecuteAsync();

                Assert.True(bulkcopy > 0);
                Assert.True(actorTotal > beforeActorTotal);
                Assert.True(customersTotal > beforeCustomersTotal);
            }
        }
    }
}
