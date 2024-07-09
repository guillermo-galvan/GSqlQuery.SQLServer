using GSqlQuery.Runner;
using GSqlQuery.SQLServer.Benchmark.Data.Parameters;
using GSqlQuery.SQLServer.Benchmark.Data.Tables;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Data;

namespace GSqlQuery.SQLServer.Benchmark
{
    public class SqlServerDatabaseManagementEventsCustom : SqlServerDatabaseManagementEvents
    {
        private readonly ServiceProvider _serviceProvider;

        public SqlServerDatabaseManagementEventsCustom(ServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public override IEnumerable<IDataParameter> GetParameter<T>(IEnumerable<ParameterDetail> parameters)
        {
            Queue<SqlParameter> sqlParameters = new();
            IGetParameterTypes<T> getParameters;
            try
            {
                getParameters = typeof(Actor) == typeof(T) ? (IGetParameterTypes<T>)new Actors() : _serviceProvider.GetService<IGetParameterTypes<T>>();
            }
            catch
            {
                getParameters = null;
            }

            if (getParameters == null)
            {
                throw new InvalidProgramException($"Interface to IGetParameters not found for type {typeof(T)}");
            }

            foreach (var param in parameters)
            {
                SqlDbType sqlDbType = getParameters.Types[param.PropertyOptions.PropertyInfo.Name];

                sqlParameters.Enqueue(new SqlParameter(param.Name, sqlDbType) { Value = param.Value });
            }

            return sqlParameters;
        }

        public override ITransformTo<T, TDbDataReader> GetTransformTo<T, TDbDataReader>(ClassOptions classOptions)
        {
            return typeof(Actor) == typeof(T) ? (ITransformTo<T, TDbDataReader>)new Data.Transform.Actors() : _serviceProvider.GetService<ITransformTo<T, TDbDataReader>>();
        }
    }
}
