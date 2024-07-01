using BenchmarkDotNet.Attributes;
using GSqlQuery.SQLServer.Benchmark.Data.Tables;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GSqlQuery.SQLServer.Benchmark.Query
{
    public abstract class InsertBenchmark : BenchmarkBase
    {
        [GlobalSetup]
        public virtual void GlobalSetup()
        {
            Helper.CreateDataTable();
        }

        [IterationSetup]
        public virtual void InitializeTest()
        {
            int count = Actor.Select(_connectionOptions, x => x.ActorId).Count().Build().Execute();
            Console.WriteLine("Init Initialize Actor {0}", count);
        }
    }

    public class Single_Insert : InsertBenchmark
    {
        [Benchmark]
        public async Task<Actor> GenerateQuery_Actor()
        {
            Actor test = new Actor() { ActorId = 0, FirstName = "PENELOPE", LastName = "GUINESS", LastUpdate = DateTime.Now.ToUniversalTime() };
            return Async ? await test.Insert(_connectionOptions).Build().ExecuteAsync() : test.Insert(_connectionOptions).Build().Execute();
        }

        [Benchmark]
        public async Task<Actor> GenerateQueryTransaction_Actor()
        {
            using (var connection = await _connectionOptions.DatabaseManagement.GetConnectionAsync())
            {
                using (var transaction = await connection.BeginTransactionAsync())
                {
                    Actor test = new Actor() { ActorId = 0, FirstName = "PENELOPE", LastName = "GUINESS", LastUpdate = DateTime.Now.ToUniversalTime() };
                    var result = Async ? await test.Insert(_connectionOptions).Build().ExecuteAsync(transaction.Connection) : test.Insert(_connectionOptions).Build().Execute(transaction.Connection);
                    await transaction.CommitAsync();
                    await connection.CloseAsync();

                    return result;
                }
            }
        }

        [Benchmark]
        public async Task<Customer> GenerateQuery_Customer()
        {
            Customer test = new Customer() { CustomerId = 0, StoreId = 1, FirstName = "MARY", LastName = "SMITH", Email = "MARY.SMITH@sakilacustomer.org", AddressId = 5, Active = 1, CreateDate = DateTime.Now.ToUniversalTime(), LastUpdate = DateTime.Now.ToUniversalTime() };
            return Async ? await test.Insert(_connectionOptions).Build().ExecuteAsync() : test.Insert(_connectionOptions).Build().Execute();
        }

        [Benchmark]
        public async Task<Customer> GenerateQueryTransaction_Customer()
        {
            using (var connection = await _connectionOptions.DatabaseManagement.GetConnectionAsync())
            {
                using (var transaction = await connection.BeginTransactionAsync())
                {
                    Customer test = new Customer() { CustomerId = 0, StoreId = 1, FirstName = "MARY", LastName = "SMITH", Email = "MARY.SMITH@sakilacustomer.org", AddressId = 5, Active = 1, CreateDate = DateTime.Now.ToUniversalTime(), LastUpdate = DateTime.Now.ToUniversalTime() };
                    var result = Async ? await test.Insert(_connectionOptions).Build().ExecuteAsync(transaction.Connection)
                                       : test.Insert(_connectionOptions).Build().Execute(transaction.Connection);
                    await transaction.CommitAsync();
                    await connection.CloseAsync();
                    return result;
                }
            }
        }
    }

    public class Many_Insert : InsertBenchmark
    {
        [Params(10, 100, 1000)]
        public int Rows { get; set; }

        [Benchmark]
        public async Task<List<Actor>> GenerateQuery_Actor()
        {
            List<Actor> result = new List<Actor>();
            Actor test = new Actor() { ActorId = 0, FirstName = "PENELOPE", LastName = "GUINESS", LastUpdate = DateTime.Now.ToUniversalTime() };
            for (int i = 0; i < Rows; i++)
            {
                result.Add(Async ? await test.Insert(_connectionOptions).Build().ExecuteAsync() : test.Insert(_connectionOptions).Build().Execute());
            }

            return result;
        }

        [Benchmark]
        public async Task<List<Actor>> GenerateQueryTransaction_Actor()
        {
            List<Actor> result = new List<Actor>();

            using (var connection = await _connectionOptions.DatabaseManagement.GetConnectionAsync())
            {
                using (var transaction = await connection.BeginTransactionAsync())
                {
                    Actor test = new Actor() { ActorId = 0, FirstName = "PENELOPE", LastName = "GUINESS", LastUpdate = DateTime.Now.ToUniversalTime() };
                    for (int i = 0; i < Rows; i++)
                    {
                        result.Add(Async ? await test.Insert(_connectionOptions).Build().ExecuteAsync(transaction.Connection) : test.Insert(_connectionOptions).Build().Execute(transaction.Connection));
                    }

                    await transaction.CommitAsync();
                    await connection.CloseAsync();

                    return result;
                }
            }
        }

        [Benchmark]
        public async Task<List<Customer>> GenerateQuery_Customer()
        {
            List<Customer> result = new List<Customer>();
            Customer test = new Customer() { CustomerId = 0, StoreId = 1, FirstName = "MARY", LastName = "SMITH", Email = "MARY.SMITH@sakilacustomer.org", AddressId = 5, Active = 1, CreateDate = DateTime.Now.ToUniversalTime(), LastUpdate = DateTime.Now.ToUniversalTime() };

            for (int i = 0; i < Rows; i++)
            {
                result.Add(Async ? await test.Insert(_connectionOptions).Build().ExecuteAsync() : test.Insert(_connectionOptions).Build().Execute());
            }

            return result;
        }

        [Benchmark]
        public async Task<List<Customer>> GenerateQueryTransaction_Customer()
        {
            List<Customer> result = new List<Customer>();
            using (var connection = await _connectionOptions.DatabaseManagement.GetConnectionAsync())
            {
                using (var transaction = await connection.BeginTransactionAsync())
                {
                    Customer test = new Customer() { CustomerId = 0, StoreId = 1, FirstName = "MARY", LastName = "SMITH", Email = "MARY.SMITH@sakilacustomer.org", AddressId = 5, Active = 1, CreateDate = DateTime.Now.ToUniversalTime(), LastUpdate = DateTime.Now.ToUniversalTime() };
                    for (int i = 0; i < Rows; i++)
                    {
                        result.Add(Async ? await test.Insert(_connectionOptions).Build().ExecuteAsync(transaction.Connection)
                                         : test.Insert(_connectionOptions).Build().Execute(transaction.Connection));
                    }

                    await transaction.CommitAsync();
                    await connection.CloseAsync();
                    return result;
                }
            }
        }
    }

    public class Many_Insert_Batch : InsertBenchmark
    {
        [Params(10, 100, 200)]
        public int Rows { get; set; }

        [Benchmark]
        public async Task<int> Batch_GenerateQuery_Actor()
        {
            Console.WriteLine("Batch_GenerateQuery_Actor");
            Actor test = new Actor() { ActorId = 0, FirstName = "PENELOPE", LastName = "GUINESS", LastUpdate = DateTime.Now.ToUniversalTime() };
            var batch = Execute.BatchExecuteFactory(_connectionOptions);
            for (int i = 0; i < Rows; i++)
            {
                batch.Add(sb => test.Insert(sb).Build());
            }

            return Async ? await batch.ExecuteAsync() : batch.Execute();
        }

        [Benchmark]
        public async Task<int> Batch_GenerateQueryTransaction_Actor()
        {
            Console.WriteLine("Batch_GenerateQueryTransaction_Actor");
            using (var connection = await _connectionOptions.DatabaseManagement.GetConnectionAsync())
            {
                using (var transaction = await connection.BeginTransactionAsync())
                {
                    var batch = Execute.BatchExecuteFactory(_connectionOptions);

                    Actor test = new Actor() { ActorId = 0, FirstName = "PENELOPE", LastName = "GUINESS", LastUpdate = DateTime.Now.ToUniversalTime() };
                    for (int i = 0; i < Rows; i++)
                    {
                        batch.Add(sb => test.Insert(sb).Build());
                    }

                    int result = Async ? await batch.ExecuteAsync(transaction.Connection) : batch.Execute(transaction.Connection);

                    await transaction.CommitAsync();
                    await connection.CloseAsync();

                    return result;
                }
            }
        }

        [Benchmark]
        public async Task<int> Batch_GenerateQuery_Customer()
        {
            Console.WriteLine("Batch_GenerateQuery_Customer");
            Customer test = new Customer() { CustomerId = 0, StoreId = 1, FirstName = "MARY", LastName = "SMITH", Email = "MARY.SMITH@sakilacustomer.org", AddressId = 5, Active = 1, CreateDate = DateTime.Now.ToUniversalTime(), LastUpdate = DateTime.Now.ToUniversalTime() };
            var batch = Execute.BatchExecuteFactory(_connectionOptions);
            for (int i = 0; i < Rows; i++)
            {
                batch.Add(sb => test.Insert(sb).Build());
            }

            return Async ? await batch.ExecuteAsync() : batch.Execute(); ;
        }

        [Benchmark]
        public async Task<int> Batch_GenerateQueryTransaction_Customer()
        {
            Console.WriteLine("Batch_GenerateQueryTransaction_Customer");
            using (var connection = await _connectionOptions.DatabaseManagement.GetConnectionAsync())
            {
                using (var transaction = await connection.BeginTransactionAsync())
                {
                    Customer test = new Customer() { CustomerId = 0, StoreId = 1, FirstName = "MARY", LastName = "SMITH", Email = "MARY.SMITH@sakilacustomer.org", AddressId = 5, Active = 1, CreateDate = DateTime.Now.ToUniversalTime(), LastUpdate = DateTime.Now.ToUniversalTime() };
                    var batch = Execute.BatchExecuteFactory(_connectionOptions);
                    for (int i = 0; i < Rows; i++)
                    {
                        batch.Add(sb => test.Insert(sb).Build());
                    }

                    int result = Async ? await batch.ExecuteAsync(transaction.Connection) : batch.Execute(transaction.Connection);

                    await transaction.CommitAsync();
                    await connection.CloseAsync();

                    return result;
                }
            }
        }
    }
}
