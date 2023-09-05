using System;
using System.Collections.Generic;
using System.Text;

namespace GSqlQuery.SQLServer
{
    public interface ISqlBulkCopy : IBulkCopy
    {
        new ISqlBulkCopyExecute Copy<T>(IEnumerable<T> values);
    }
}