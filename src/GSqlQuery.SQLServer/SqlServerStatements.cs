namespace GSqlQuery.SQLServer
{
    public class SqlServerStatements : DefaultFormats
    {
        public override string Format => "[{0}]";

        public override string ValueAutoIncrementingQuery => "SELECT SCOPE_IDENTITY();";
    }
}