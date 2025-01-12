using GSqlQuery.SQLServer.Test.Data.Tables;

namespace GSqlQuery.SQLServer.Test.Extensions
{
    public class LimitQueryExtensionTest
    {
        private readonly SqlServerConnectionOptions _connectionOptions;

        public LimitQueryExtensionTest()
        {
            Helper.CreateDataTable();
            _connectionOptions = new SqlServerConnectionOptions(Helper.GetConnectionString(), new SqlServerDatabaseManagementEventsCustom());
        }

        [Fact]
        public void Limit_start_length()
        {
            var text = "SELECT [actor].[actor_id],[actor].[first_name],[actor].[last_name],[actor].[last_update] FROM [actor] ORDER BY [actor].[actor_id] ASC OFFSET 0 ROWS FETCH NEXT 5 ROWS ONLY;";

            var result = Actor.Select(new QueryOptions(_connectionOptions.Formats)).OrderBy(x => new { x.ActorId }, OrderBy.ASC).Limit(0, 5).Build();
            Assert.NotNull(result);
            Assert.Equal(text, result.Text);
        }

        [Fact]
        public void Limit_start_length_and_where()
        {
            var text = "SELECT [actor].[actor_id],[actor].[first_name],[actor].[last_name],[actor].[last_update] FROM [actor] WHERE [actor].[last_name] IS NOT NULL ORDER BY [actor].[actor_id] ASC OFFSET 0 ROWS FETCH NEXT 5 ROWS ONLY;";
            var result = Actor.Select(new QueryOptions(_connectionOptions.Formats)).Where().IsNotNull(x => x.LastName).OrderBy(x => new { x.ActorId }, OrderBy.ASC).Limit(0, 5).Build();
            Assert.NotNull(result);
            Assert.Equal(text, result.Text);
        }

        [Fact]
        public void Limit_start()
        {
            var text = "SELECT [actor].[actor_id],[actor].[first_name],[actor].[last_name],[actor].[last_update] FROM [actor] ORDER BY [actor].[actor_id] ASC OFFSET 0 ROWS;";

            var result = Actor.Select(new QueryOptions(_connectionOptions.Formats)).OrderBy(x => new { x.ActorId }, OrderBy.ASC).Limit(0).Build();
            Assert.NotNull(result);
            Assert.Equal(text, result.Text);
        }

        [Fact]
        public void Limit_start_and_where()
        {
            var text = "SELECT [actor].[actor_id],[actor].[first_name],[actor].[last_name],[actor].[last_update] FROM [actor] WHERE [actor].[last_name] IS NOT NULL ORDER BY [actor].[actor_id] ASC OFFSET 0 ROWS;";
            var result = Actor.Select(new QueryOptions(_connectionOptions.Formats)).Where().IsNotNull(x => x.LastName).OrderBy(x => new { x.ActorId }, OrderBy.ASC).Limit(0).Build();
            Assert.NotNull(result);
            Assert.Equal(text, result.Text);
        }

        [Fact]
        public void Limit_start_length_Execute()
        {
            var result = Address.Select(_connectionOptions).OrderBy(x => new { x.AddressId }, OrderBy.ASC).Limit(0, 5).Build().Execute();
            Assert.NotNull(result);
            Assert.Equal(5, result.Count());
        }

        [Fact]
        public void Limit_start_Execute()
        {
            var result = Address.Select(_connectionOptions).OrderBy(x => new { x.AddressId }, OrderBy.ASC).Limit(0).Build().Execute();
            Assert.NotNull(result);
            Assert.True(result.Count() > 0);
        }

        [Fact]
        public void Limit_start_length_Execute_with_connection()
        {
            using (var connection = _connectionOptions.DatabaseManagement.GetConnection())
            {
                var result = Address.Select(_connectionOptions).OrderBy(x => new { x.AddressId }, OrderBy.ASC).Limit(0, 5).Build().Execute(connection);
                Assert.NotNull(result);
                Assert.Equal(5, result.Count());
            }
        }

        [Fact]
        public void Limit_start_Execute_with_connection()
        {
            using (var connection = _connectionOptions.DatabaseManagement.GetConnection())
            {
                var result = Address.Select(_connectionOptions).OrderBy(x => new { x.AddressId }, OrderBy.ASC).Limit(0).Build().Execute(connection);
                Assert.NotNull(result);
                Assert.True(result.Count() > 0);
            }
        }

        [Fact]
        public void Throw_exeception_when_connection_is_null()
        {
            using (var connection = _connectionOptions.DatabaseManagement.GetConnection())
            {
                Assert.Throws<ArgumentNullException>(() => Address.Select(_connectionOptions).OrderBy(x => new { x.AddressId }, OrderBy.ASC).Limit(0, 5).Build().Execute(null));
            }
        }

        [Fact]
        public async Task Limit_start_length_ExecuteAsync()
        {
            var result = await Address.Select(_connectionOptions).OrderBy(x => new { x.AddressId }, OrderBy.ASC).Limit(0, 5).Build().ExecuteAsync();
            Assert.NotNull(result);
            Assert.Equal(5, result.Count());
        }

        [Fact]
        public async Task Limit_start_ExecuteAsync()
        {
            var result = await Address.Select(_connectionOptions).OrderBy(x => new { x.AddressId }, OrderBy.ASC).Limit(0).Build().ExecuteAsync();
            Assert.NotNull(result);
            Assert.True(result.Count() > 0);
        }

        [Fact]
        public async Task Limit_start_length_ExecuteAsync_with_token()
        {
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
            CancellationToken token = cancellationTokenSource.Token;
            var result = await Address.Select(_connectionOptions).OrderBy(x => new { x.AddressId }, OrderBy.ASC).Limit(0, 5).Build().ExecuteAsync(token);
            Assert.NotNull(result);
            Assert.Equal(5, result.Count());
        }

        [Fact]
        public async Task Limit_start_ExecuteAsync_with_token()
        {
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
            CancellationToken token = cancellationTokenSource.Token;
            var result = await Address.Select(_connectionOptions).OrderBy(x => new { x.AddressId }, OrderBy.ASC).Limit(0).Build().ExecuteAsync(token);
            Assert.NotNull(result);
            Assert.True(result.Count() > 0);
        }

        [Fact]
        public async Task Throw_exeception_when_cancel_token()
        {
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
            CancellationToken token = cancellationTokenSource.Token;
            cancellationTokenSource.Cancel();
            await Assert.ThrowsAsync<OperationCanceledException>(async () => await Actor.Select(_connectionOptions).OrderBy(x => new { x.ActorId }, OrderBy.ASC).Limit(0, 5).Build().ExecuteAsync(token));
        }

        [Fact]
        public async Task Limit_start_length_ExecuteAsync_with_connection()
        {
            using (var connection = await _connectionOptions.DatabaseManagement.GetConnectionAsync())
            {
                var result = await Address.Select(_connectionOptions).OrderBy(x => new { x.AddressId }, OrderBy.ASC).Limit(0, 5).Build().ExecuteAsync(connection);
                Assert.NotNull(result);
                Assert.Equal(5, result.Count());
            }
        }

        [Fact]
        public async Task Limit_start_ExecuteAsync_with_connection()
        {
            using (var connection = await _connectionOptions.DatabaseManagement.GetConnectionAsync())
            {
                var result = await Address.Select(_connectionOptions).OrderBy(x => new { x.AddressId }, OrderBy.ASC).Limit(0).Build().ExecuteAsync(connection);
                Assert.NotNull(result);
                Assert.True(result.Count() > 0);
            }
        }

        [Fact]
        public async Task Throw_exeception_when_connection_is_null_executeasync()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(async () => await Actor.Select(_connectionOptions).OrderBy(x => new { x.ActorId }, OrderBy.ASC).Limit(0, 5).Build().ExecuteAsync(null));
        }

        [Fact]
        public async Task Limit_start_length_ExecuteAsync_with_connection_and_token()
        {
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
            CancellationToken token = cancellationTokenSource.Token;
            using (var connection = await _connectionOptions.DatabaseManagement.GetConnectionAsync())
            {
                var result = await Address.Select(_connectionOptions).OrderBy(x => new { x.AddressId }, OrderBy.ASC).Limit(0, 5).Build().ExecuteAsync(connection, token);
                Assert.NotNull(result);
                Assert.Equal(5, result.Count());
            }
        }

        [Fact]
        public async Task Limit_start_ExecuteAsync_with_connection_and_token()
        {
            using (var connection = await _connectionOptions.DatabaseManagement.GetConnectionAsync())
            {
                CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
                CancellationToken token = cancellationTokenSource.Token;
                var result = await Address.Select(_connectionOptions).OrderBy(x => new { x.AddressId }, OrderBy.ASC).Limit(0).Build().ExecuteAsync(connection, token);
                Assert.NotNull(result);
                Assert.True(result.Count() > 0);
            }
        }


        [Fact]
        public async Task Throw_exeception_with_connection_and_cancel_token()
        {
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
            CancellationToken token = cancellationTokenSource.Token;
            using (var connection = await _connectionOptions.DatabaseManagement.GetConnectionAsync())
            {
                cancellationTokenSource.Cancel();
                await Assert.ThrowsAsync<OperationCanceledException>(async () => await Actor.Select(_connectionOptions).OrderBy(x => new { x.ActorId }, OrderBy.ASC).Limit(0, 5).Build().ExecuteAsync(connection, token));
                await Assert.ThrowsAsync<ArgumentNullException>(async () => await Actor.Select(_connectionOptions).OrderBy(x => new { x.ActorId }, OrderBy.ASC).Limit(0, 5).Build().ExecuteAsync(null, token));
            }
        }

        [Fact]
        public async Task Limit_executeasync_in_two_Join_with_whereAsync()
        {
            var result = await Actor.Select(_connectionOptions)
                                      .InnerJoin<Film_Actor>()
                                      .Equal(x => x.Table1.ActorId, x => x.Table2.ActorId)
                                      .Where()
                                      .Equal(x => x.Table1.ActorId, 1)
                                      .OrderBy(x => x.Table1.ActorId, OrderBy.ASC)
                                      .Limit(0, 5)
                                      .Build().ExecuteAsync();

            Assert.NotNull(result);
            Assert.NotEmpty(result);
        }

        [Fact]
        public async Task Limit_executeasync_in_three_Join_with_where()
        {
            var result = await Actor.Select(_connectionOptions)
                                      .InnerJoin<Film_Actor>()
                                      .Equal(x => x.Table1.ActorId, x => x.Table2.ActorId)
                                      .InnerJoin<Film>()
                                      .Equal(x => x.Table2.FilmId, x => x.Table3.FilmId)
                                      .Where()
                                      .Equal(x => x.Table1.ActorId, 1)
                                      .OrderBy(x => x.Table1.ActorId, OrderBy.ASC)
                                      .Limit(0, 5)
                                      .Build().ExecuteAsync();

            Assert.NotNull(result);
            Assert.NotEmpty(result);
        }
    }
}
