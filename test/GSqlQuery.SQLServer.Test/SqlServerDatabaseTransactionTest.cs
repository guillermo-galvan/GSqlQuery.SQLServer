﻿using GSqlQuery.SQLServer.Test.Data.Tables;
using Microsoft.SqlServer.Types;

namespace GSqlQuery.SQLServer.Test
{
    public class SqlServerDatabaseTransactionTest
    {
        private readonly SqlServerConnectionOptions _connectionOptions;

        public SqlServerDatabaseTransactionTest()
        {
            Helper.CreateDataTable();
            _connectionOptions = new SqlServerConnectionOptions(Helper.GetConnectionString(), new SqlServerDatabaseManagementEventsCustom());
        }


        [Fact]
        public void Commit()
        {
            Actor actor = new Actor(0, "PENELOPE", "PENELOPE", DateTime.Now);
            using (var connection = _connectionOptions.DatabaseManagement.GetConnection())
            {
                using (var transaction = connection.BeginTransaction())
                {
                    var result = actor.Insert(_connectionOptions).Build().Execute(transaction.Connection);
                    transaction.Commit();
                    connection.Close();
                    var isExists = Actor.Select(_connectionOptions).Where().Equal(x => x.ActorId, result.ActorId).Build().Execute().Any();
                    Assert.True(isExists);
                }
            }
        }

        [Fact]
        public async Task CommitAsync()
        {
            Address address = new Address(0, "47 MySakila Drive", null, "Alberta", 300, string.Empty, string.Empty, SqlGeometry.Point(153.1408538, -27.6333361, 0), DateTime.Now);
            using (var connection = await _connectionOptions.DatabaseManagement.GetConnectionAsync())
            {
                using (var transaction = await connection.BeginTransactionAsync())
                {
                    var result = await address.Insert(_connectionOptions).Build().ExecuteAsync(transaction.Connection);
                    await transaction.CommitAsync();
                    await connection.CloseAsync();
                    var isExists = Address.Select(_connectionOptions).Where().Equal(x => x.AddressId, result.AddressId).Build().Execute().Any();
                    Assert.True(isExists);
                }
            }
        }

        [Fact]
        public async Task CommitAsync_with_cancellationtoken()
        {
            CancellationTokenSource source = new CancellationTokenSource();
            CancellationToken token = source.Token;

            Actor actor = new Actor(0, "PENELOPE", "PENELOPE", DateTime.Now);
            using (var connection = await _connectionOptions.DatabaseManagement.GetConnectionAsync(token))
            {
                using (var transaction = await connection.BeginTransactionAsync(token))
                {
                    var result = await actor.Insert(_connectionOptions).Build().ExecuteAsync(transaction.Connection, token);
                    await transaction.CommitAsync(token);
                    await connection.CloseAsync(token);
                    var isExists = Actor.Select(_connectionOptions).Where().Equal(x => x.ActorId, result.ActorId).Build().Execute().Any();
                    Assert.True(isExists);
                }
            }
        }

        [Fact]
        public async Task Throw_exception_if_Cancel_token_on_CommitAsync()
        {
            CancellationTokenSource source = new CancellationTokenSource();
            CancellationToken token = source.Token;

            Address address = new Address(0, "47 MySakila Drive", null, "Alberta", 300, string.Empty, string.Empty, SqlGeometry.Point(153.1408538, -27.6333361, 0), DateTime.Now);
            using (var connection = await _connectionOptions.DatabaseManagement.GetConnectionAsync(token))
            {
                using (var transaction = await connection.BeginTransactionAsync(token))
                {
                    var result = await address.Insert(_connectionOptions).Build().ExecuteAsync(transaction.Connection, token);
                    source.Cancel();
                    await Assert.ThrowsAsync<OperationCanceledException>(async () => await transaction.CommitAsync(token));
                }
            }
        }

        [Fact]
        public void Rollback()
        {
            Actor actor = new Actor(0, "PENELOPE", "PENELOPE", DateTime.Now);
            using (var connection = _connectionOptions.DatabaseManagement.GetConnection())
            {
                using (var transaction = connection.BeginTransaction())
                {
                    var result = actor.Insert(_connectionOptions).Build().Execute(transaction.Connection);
                    transaction.Rollback();
                    connection.Close();

                    var isExists = Actor.Select(_connectionOptions).Where().Equal(x => x.ActorId, result.ActorId).Build().Execute().Any();
                    Assert.False(isExists);
                }
            }
        }

        [Fact]
        public async Task RollbackAsync()
        {
            Address address = new Address(0, "47 MySakila Drive", null, "Alberta", 300, string.Empty, string.Empty, SqlGeometry.Point(153.1408538, -27.6333361, 0), DateTime.Now);
            using (var connection = await _connectionOptions.DatabaseManagement.GetConnectionAsync())
            {
                using (var transaction = await connection.BeginTransactionAsync())
                {
                    var result = await address.Insert(_connectionOptions).Build().ExecuteAsync(transaction.Connection);
                    await transaction.RollbackAsync();
                    await connection.CloseAsync();
                    var isExists = Address.Select(_connectionOptions).Where().Equal(x => x.AddressId, result.AddressId).AndEqual(x => x.CityId, address.CityId).Build().Execute().Any();
                    Assert.False(isExists);
                }
            }
        }

        [Fact]
        public async Task RollbackAsync_with_cancellationtoken()
        {
            CancellationTokenSource source = new CancellationTokenSource();
            CancellationToken token = source.Token;

            Actor actor = new Actor(0, "PENELOPE", "PENELOPE", DateTime.Now);
            using (var connection = await _connectionOptions.DatabaseManagement.GetConnectionAsync(token))
            {
                using (var transaction = await connection.BeginTransactionAsync(token))
                {
                    var result = await actor.Insert(_connectionOptions).Build().ExecuteAsync(transaction.Connection, token);
                    await transaction.RollbackAsync(token);
                    var isExists = Actor.Select(_connectionOptions).Where().Equal(x => x.ActorId, result.ActorId).AndEqual(x => x.LastName, actor.LastName).Build().Execute(connection).Any();
                    await connection.CloseAsync(token);
                    Assert.False(isExists);
                }
            }
        }

        [Fact]
        public async Task Throw_exception_if_Cancel_token_on_RollbackAsync()
        {
            CancellationTokenSource source = new CancellationTokenSource();
            CancellationToken token = source.Token;

            Address address = new Address(0, "47 MySakila Drive", null, "Alberta", 300, string.Empty, string.Empty, SqlGeometry.Point(153.1408538, -27.6333361, 0), DateTime.Now);
            using (var connection = await _connectionOptions.DatabaseManagement.GetConnectionAsync(token))
            {
                using (var transaction = await connection.BeginTransactionAsync(token))
                {
                    var result = await address.Insert(_connectionOptions).Build().ExecuteAsync(transaction.Connection, token);
                    source.Cancel();
                    await Assert.ThrowsAsync<OperationCanceledException>(async () => await transaction.RollbackAsync(token));
                }
            }
        }
    }
}
