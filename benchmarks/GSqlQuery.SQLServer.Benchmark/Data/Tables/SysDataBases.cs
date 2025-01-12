namespace GSqlQuery.SQLServer.Benchmark.Data.Tables
{
    [Table("sys", "databases")]
    public class SysDataBases : EntityExecute<SysDataBases>
    {
        [Column("name")]
        public string Name { get; set; }

        public SysDataBases(string name)
        {
            Name = name;
        }
    }
}
