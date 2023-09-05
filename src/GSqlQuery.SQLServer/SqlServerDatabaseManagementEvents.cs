using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace GSqlQuery.SQLServer
{
    public class SqlServerDatabaseManagementEvents : DatabaseManagementEvents
    {
        public override Func<Type, IEnumerable<ParameterDetail>, IEnumerable<IDataParameter>> OnGetParameter { get; set; } = (type, parametersDetail) =>
        {
            return parametersDetail.Select(x => new SqlParameter(x.Name, x.Value));
        };
    }
}