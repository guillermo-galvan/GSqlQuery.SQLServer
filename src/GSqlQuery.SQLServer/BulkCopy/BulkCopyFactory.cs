namespace GSqlQuery.SQLServer
{
    public static class BulkCopyFactory
    {
        public static ISqlBulkCopyExecute Create(string connectionString)
        {
            return new BulkExecute(connectionString);
        }
    }
}