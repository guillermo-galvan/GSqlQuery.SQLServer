using System.Collections.Generic;
using System.Data;

namespace GSqlQuery.SQLServer.Benchmark.Data.Parameters
{
    internal abstract class GetParameterTypes<T> : IGetParameterTypes<T>
    {
        protected readonly Dictionary<string, SqlDbType> _valuePairs;

        public GetParameterTypes()
        {
            _valuePairs = new Dictionary<string, SqlDbType>();
        }

        public IDictionary<string, SqlDbType> Types => _valuePairs;
    }
}
