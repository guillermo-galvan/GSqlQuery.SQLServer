using BenchmarkDotNet.Columns;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Diagnosers;
using BenchmarkDotNet.Running;
using GSqlQuery.Runner;
using GSqlQuery.SQLServer.Benchmark.Data.Parameters;
using GSqlQuery.SQLServer.Benchmark.Data.Tables;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.DependencyInjection;
using System;


namespace GSqlQuery.SQLServer.Benchmark
{
    internal class Program
    {
        //protected static SqlServerConnectionOptions _connectionOptions;
        //protected static ServiceProvider _serviceCollection;
        //protected static SqlServerConnectionOptions _connectionOptionsServicesProvider;


        public static void Main(string[] args)
        {
            // _serviceCollection = new ServiceCollection()
            // .AddScoped<ITransformTo<Actor, SqlDataReader>, Data.Transform.Actors>()
            // .AddScoped<IGetParameterTypes<Actor>, Actors>()
            //.BuildServiceProvider();

            // _connectionOptions = Helper.GetConnectionOptions();
            // _connectionOptionsServicesProvider = Helper.GetConnectionOptions(_serviceCollection);
            // Helper.CreateDataTable();

            // var v = Actor.Select(_connectionOptionsServicesProvider).Build().Execute();
            // Console.WriteLine(v);

            IConfig config = DefaultConfig.Instance;

            config = config
                .AddDiagnoser(MemoryDiagnoser.Default)
                .AddColumn(StatisticColumn.OperationsPerSecond);

            var summary = BenchmarkSwitcher.FromAssembly(typeof(Program).Assembly).Run(args, config);

            Console.WriteLine(summary);
        }
    }
}
