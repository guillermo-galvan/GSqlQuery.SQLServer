namespace GSqlQuery.SQLServer.Test.Data.Tables
{
    [Table("film_actor")]
    public class Film_Actor : EntityExecute<Film_Actor>
    {
        [Column("actor_id", Size = 5, IsPrimaryKey = true)]
        public int ActorId { get; set; }

        [Column("film_id", Size = 5, IsPrimaryKey = true)]
        public int FilmId { get; set; }

        [Column("last_update", Size = 19)]
        public DateTime LastUpdate { get; set; }

        public Film_Actor()
        { }

        public Film_Actor(int actorId, int filmId, DateTime lastUpdate)
        {
            ActorId = actorId;
            FilmId = filmId;
            LastUpdate = lastUpdate;
        }
    }
}
