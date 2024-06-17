namespace GSqlQuery.SQLServer
{
    public static class BulkCopyFactory
    {
        public static ISqlBulkCopy Create(string connectionString)
        {
            return new BulkExecute(connectionString);
        }
    }
}