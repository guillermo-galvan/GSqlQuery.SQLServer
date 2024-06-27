using GSqlQuery.Runner;
using GSqlQuery.SQLServer.Test.Data.Tables;
using GSqlQuery.SQLServer.Test.Transform;
using Microsoft.Data.SqlClient;
using System.Data;

namespace GSqlQuery.SQLServer.Test
{
    public class SqlServerDatabaseManagementEventsCustom : SqlServerDatabaseManagementEvents
    {
        private SqlParameter GetAddressParam(ParameterDetail parameterDetail)
        {
            if (parameterDetail.PropertyOptions.PropertyInfo.Name == nameof(Address.Location))
            {
                return new SqlParameter(parameterDetail.Name, parameterDetail.Value)
                {
                    SqlDbType = SqlDbType.Udt,
                    UdtTypeName = "geometry"
                };
            }

            return new SqlParameter(parameterDetail.Name, parameterDetail.Value);
        }

        public override IEnumerable<IDataParameter> GetParameter<T>(IEnumerable<ParameterDetail> parameters)
        {
            if (typeof(T) == typeof(Address))
            {
                return parameters.Select(GetAddressParam);
            }

            return parameters.Select(x => new SqlParameter(x.Name, x.Value));
        }

        public override ITransformTo<T, TDbDataReader> GetTransformTo<T, TDbDataReader>(ClassOptions classOptions)
        {
            if (typeof(T) == typeof(Address))
            {
                return (ITransformTo<T, TDbDataReader>)new AddressTransform(classOptions.PropertyOptions.Count());
            }

            return base.GetTransformTo<T, TDbDataReader>(classOptions);
        }
    }
}
