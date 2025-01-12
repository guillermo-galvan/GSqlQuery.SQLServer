using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using GSqlQuery.Runner;
using GSqlQuery.SQLServer.Benchmark.Data.Parameters;
using GSqlQuery.SQLServer.Benchmark.Data.Tables;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.DependencyInjection;

namespace GSqlQuery.SQLServer.Benchmark.Query
{
    [SimpleJob(RuntimeMoniker.Net90, baseline: true)]
    [SimpleJob(RuntimeMoniker.Net80)]
    [SimpleJob(RuntimeMoniker.Net462)]
    [MinColumn, MaxColumn, MeanColumn, MedianColumn]
    public abstract class BenchmarkBase
    {
        protected readonly SqlServerConnectionOptions _connectionOptions;
        protected readonly ServiceProvider _serviceCollection;
        protected readonly SqlServerConnectionOptions _connectionOptionsServicesProvider;

        public BenchmarkBase()
        {
            _serviceCollection = new ServiceCollection()
            .AddScoped<ITransformTo<Actor, SqlDataReader>, Data.Transform.Actors>()
            .AddScoped<IGetParameterTypes<Actor>, Actors>()
           .BuildServiceProvider();
            _connectionOptions = Helper.GetConnectionOptions();
            _connectionOptionsServicesProvider = Helper.GetConnectionOptions(_serviceCollection);
        }

        [Params(true, false)]
        public bool Async { get; set; }
    }
}
