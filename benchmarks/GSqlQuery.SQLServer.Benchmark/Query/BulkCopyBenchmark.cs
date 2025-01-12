using BenchmarkDotNet.Attributes;
using GSqlQuery.SQLServer.Benchmark.Data.Tables;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GSqlQuery.SQLServer.Benchmark.Query
{
    public class BulkCopyBenchmark : BenchmarkBase
    {
        private readonly string _connectionString;
        public IEnumerable<Actor> _actors;

        protected BulkCopyBenchmark()
        {
            _connectionString = Helper.GetConnectionString();
        }

        [GlobalSetup]
        public virtual void GlobalSetup()
        {
            Helper.CreateDataTable();
            _actors = Actor.Select(_connectionOptions).Build().Execute();
            foreach (var item in _actors)
            {
                item.ActorId = 0;
            }
        }

        [IterationSetup]
        public virtual void InitializeTest()
        {
            int count = Actor.Select(_connectionOptions, x => x.ActorId).Count().Build().Execute();
            Console.WriteLine("Init Initialize Actor {0}", count);
        }

        [Benchmark]
        public async Task<int> BulkCopyExecute()
        {
            ISqlBulkCopyExecute query = BulkCopyFactory.Create(_connectionString).Copy(_actors);
            return Async ? await query.ExecuteAsync() : query.Execute();
        }

        [Benchmark]
        public async Task<int> BulkCopyExecute_with_connection()
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                if (Async)
                {
                    await connection.OpenAsync();
                }
                else
                {
                    connection.Open();
                }

                ISqlBulkCopyExecute query = BulkCopyFactory.Create(Helper.GetConnectionString()).Copy(_actors);
                return Async ? await query.ExecuteAsync(connection) : query.Execute(connection);
            }
            
        }
    }
}
