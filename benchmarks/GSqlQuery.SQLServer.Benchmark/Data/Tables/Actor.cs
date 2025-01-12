using System;

namespace GSqlQuery.SQLServer.Benchmark.Data.Tables
{
    [Table("actor")]
    public class Actor : EntityExecute<Actor>
    {
        [Column("actor_id", Size = 5, IsAutoIncrementing = true, IsPrimaryKey = true)]
        public long ActorId { get; set; }

        [Column("first_name", Size = 45)]
        public string FirstName { get; set; }

        [Column("last_name", Size = 45)]
        public string LastName { get; set; }

        [Column("last_update", Size = 19)]
        public DateTime LastUpdate { get; set; }

        public Actor()
        { }

        public Actor(long actorId, string firstName, string lastName, DateTime lastUpdate)
        {
            ActorId = actorId;
            FirstName = firstName;
            LastName = lastName;
            LastUpdate = lastUpdate;
        }
    }
}
