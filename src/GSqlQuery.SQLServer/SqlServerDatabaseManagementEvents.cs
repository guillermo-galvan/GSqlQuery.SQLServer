using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace GSqlQuery.SQLServer
{
    public class SqlServerDatabaseManagementEvents : DatabaseManagementEvents
    {
        public override IEnumerable<IDataParameter> GetParameter<T>(IEnumerable<ParameterDetail> parameters)
        {
            return parameters.Select(x => new SqlParameter(x.Name, x.Value));
        }
    }
}