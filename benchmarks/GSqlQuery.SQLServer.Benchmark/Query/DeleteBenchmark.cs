using BenchmarkDotNet.Attributes;
using GSqlQuery.SQLServer.Benchmark.Data.Tables;
using System;
using System.Threading.Tasks;

namespace GSqlQuery.SQLServer.Benchmark.Query
{
    public abstract class DeleteBenchmark : BenchmarkBase
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

    public class Delete : DeleteBenchmark
    {
        [Benchmark]
        public async Task<int> Delete_Bool()
        {
            var query = Actor.Delete(_connectionOptions).Where().Equal(x => x.ActorId, 5000).Build();
            return Async ? await query.ExecuteAsync() : query.Execute();
        }
    }
}
