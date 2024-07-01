using BenchmarkDotNet.Attributes;
using GSqlQuery.SQLServer.Benchmark.Data.Tables;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace GSqlQuery.SQLServer.Benchmark.Query
{
    public abstract class UpdateBenchmark : BenchmarkBase
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
            Console.WriteLine("Init Initialize test 2 {0}", count);
        }
    }

    public class Update : UpdateBenchmark
    {
        [Benchmark]
        public async Task<int> Update_Actor()
        {
            Actor test = new Actor() { ActorId = 1, FirstName = "PENELOPE", LastName = "GUINESS", LastUpdate = DateTime.Now.ToUniversalTime() };
            var query = test.Update(_connectionOptions, x => x.LastUpdate).Where().Equal(x => x.ActorId, 1).Build();
            return Async ? await query.ExecuteAsync() : query.Execute();
        }

        [Benchmark]
        public async Task<int> Update_DateTime()
        {
            Actor test = new Actor() { ActorId = 1, FirstName = "PENELOPE", LastName = "GUINESS", LastUpdate = DateTime.Now.ToUniversalTime() };
            test.LastUpdate = DateTime.UtcNow;
            var query = test.Update(_connectionOptions, x => x.LastUpdate).Build();
            return Async ? await query.ExecuteAsync() : query.Execute();
        }

        [Benchmark]
        public async Task<int> Update_Decimal()
        {
            Actor test = new Actor() { ActorId = 1, FirstName = "PENELOPE", LastName = "GUINESS", LastUpdate = DateTime.Now.ToUniversalTime() };
            var query = test.Update(_connectionOptions, x => x.FirstName).Build();
            return Async ? await query.ExecuteAsync() : query.Execute();
        }

        [Benchmark]
        public async Task<int> Update_AllColumns()
        {
            Actor test = new Actor() { ActorId = 1, FirstName = "PENELOPE", LastName = "GUINESS", LastUpdate = DateTime.Now.ToUniversalTime() };
            var query = test.Update(_connectionOptions, x => new { x.FirstName, x.LastName, x.LastUpdate }).Where().In(x => x.ActorId, Enumerable.Range(1, 1000).Select(x => (long)x)).Build();
            return Async ? await query.ExecuteAsync() : query.Execute();
        }

        [Benchmark]
        public async Task<int> Update_AllColumns_by_Id()
        {
            Actor test = new Actor() { ActorId = 1, FirstName = "PENELOPE", LastName = "GUINESS", LastUpdate = DateTime.Now.ToUniversalTime() };
            var query = test.Update(_connectionOptions, x => new { x.FirstName, x.LastName, x.LastUpdate }).Where().Equal(x => x.ActorId, 1).Build();
            return Async ? await query.ExecuteAsync() : query.Execute();
        }
    }
}
