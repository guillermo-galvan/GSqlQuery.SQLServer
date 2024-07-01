using System.Collections.Generic;
using System.Data;

namespace GSqlQuery.SQLServer.Benchmark.Data.Parameters
{
    internal interface IGetParameterTypes
    {
        IDictionary<string, SqlDbType> Types { get; }
    }

    internal interface IGetParameterTypes<T> : IGetParameterTypes
    {

    }
}
