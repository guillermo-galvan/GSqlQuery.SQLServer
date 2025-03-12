namespace GSqlQuery.SQLServer.Test
{
    [CollectionDefinition("GlobalTestServer", DisableParallelization = true)]
    public class GlobalTestServer : ICollectionFixture<GlobalFixture>
    {
    }
}
