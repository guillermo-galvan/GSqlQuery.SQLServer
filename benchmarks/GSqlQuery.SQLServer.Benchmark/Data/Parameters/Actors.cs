using GSqlQuery.SQLServer.Benchmark.Data.Tables;
using System.Data;

namespace GSqlQuery.SQLServer.Benchmark.Data.Parameters
{
    internal class Actors : GetParameterTypes<Actor>
    {
        public Actors() : base()
        {
            _valuePairs.Add(nameof(Actor.ActorId), SqlDbType.BigInt);
            _valuePairs.Add(nameof(Actor.FirstName), SqlDbType.VarChar);
            _valuePairs.Add(nameof(Actor.LastName), SqlDbType.VarChar);
            _valuePairs.Add(nameof(Actor.LastUpdate), SqlDbType.DateTime);
        }
    }
}
