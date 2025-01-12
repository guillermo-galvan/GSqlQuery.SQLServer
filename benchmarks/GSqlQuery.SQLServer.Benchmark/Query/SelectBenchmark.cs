﻿using BenchmarkDotNet.Attributes;
using GSqlQuery.SQLServer.Benchmark.Data.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GSqlQuery.SQLServer.Benchmark.Query
{
    public abstract class SelectBenchmark : BenchmarkBase
    {
        public SelectBenchmark()
        {
            Helper.CreateDataTable();
            int count = Actor.Select(_connectionOptions, x => new { x.ActorId }).Count().Build().Execute();
            Console.WriteLine("Init Initialize {1} 2 {0}", count, typeof(Actor));
        }
    }

    public class Select : SelectBenchmark
    {
        private readonly IEnumerable<long> _ids;
        public Select()
        {
            _ids = Enumerable.Range(0, 250).Select(x => (long)x);
        }

        [Params(true, false)]
        public bool IsServicesProvider { get; set; }

        new public bool Async { get; set; }


        [Benchmark]
        public int Select_All()
        {
            var query = IsServicesProvider ? Actor.Select(_connectionOptionsServicesProvider).Build() : Actor.Select(_connectionOptions).Build();
            var result = query.Execute();
            return result.Count();
        }

        [Benchmark]
        public async Task<int> Select_AllAsync()
        {
            var query = IsServicesProvider ? Actor.Select(_connectionOptionsServicesProvider).Build() : Actor.Select(_connectionOptions).Build();
            var result = await query.ExecuteAsync();
            return result.Count();
        }

        [Benchmark]
        public int Select_Many_Columns_true()
        {
            var query = IsServicesProvider ? Actor.Select(_connectionOptionsServicesProvider, x => new { x.ActorId, x.FirstName }).Build() :
                                             Actor.Select(_connectionOptions, x => new { x.ActorId, x.FirstName }).Build();
            var result = query.Execute();
            return result.Count();
        }

        [Benchmark]
        public async Task<int> Select_Many_Columns_trueAsync()
        {
            var query = IsServicesProvider ? Actor.Select(_connectionOptionsServicesProvider, x => new { x.ActorId, x.FirstName }).Build() :
                                             Actor.Select(_connectionOptions, x => new { x.ActorId, x.FirstName }).Build();
            var result = await query.ExecuteAsync();
            return result.Count();
        }

        [Benchmark]
        public int Select_All_Columns_With_Where()
        {
            var query = IsServicesProvider ? Actor.Select(_connectionOptionsServicesProvider).Where().In(x => x.ActorId, _ids).Build() :
                                             Actor.Select(_connectionOptions).Where().In(x => x.ActorId, _ids).Build();
            var result = query.Execute();
            return result.Count();
        }

        [Benchmark]
        public async Task<int> Select_All_Columns_With_WhereAsync()
        {
            var query = IsServicesProvider ? Actor.Select(_connectionOptionsServicesProvider).Where().In(x => x.ActorId, _ids).Build() :
                                             Actor.Select(_connectionOptions).Where().In(x => x.ActorId, _ids).Build();
            var result = await query.ExecuteAsync();
            return result.Count();
        }

        [Benchmark]
        public int Select_Many_Columns_With_Where()
        {
            var query = IsServicesProvider ? Actor.Select(_connectionOptionsServicesProvider, x => new { x.ActorId, x.FirstName }).Where().In(x => x.ActorId, _ids).Build() :
                                             Actor.Select(_connectionOptions, x => new { x.ActorId, x.FirstName }).Where().In(x => x.ActorId, _ids).Build();
            var result = query.Execute();
            return result.Count();
        }

        [Benchmark]
        public async Task<int> Select_Many_Columns_With_WhereAsync()
        {
            var query = IsServicesProvider ? Actor.Select(_connectionOptionsServicesProvider, x => new { x.ActorId, x.FirstName }).Where().In(x => x.ActorId, _ids).Build() :
                                             Actor.Select(_connectionOptions, x => new { x.ActorId, x.FirstName }).Where().In(x => x.ActorId, _ids).Build();
            var result = await query.ExecuteAsync();
            return result.Count();
        }
    }
}
