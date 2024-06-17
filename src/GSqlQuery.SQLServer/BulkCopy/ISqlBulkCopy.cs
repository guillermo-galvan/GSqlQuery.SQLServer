using System.Collections.Generic;

namespace GSqlQuery.SQLServer
{
    public interface ISqlBulkCopy
    {
        ISqlBulkCopyExecute Copy<T>(IEnumerable<T> values);
    }
}