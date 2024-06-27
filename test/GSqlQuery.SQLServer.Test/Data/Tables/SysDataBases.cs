namespace GSqlQuery.SQLServer.Test.Data.Tables
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
