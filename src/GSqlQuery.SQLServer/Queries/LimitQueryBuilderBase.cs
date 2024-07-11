namespace GSqlQuery.SQLServer
{
    internal abstract class LimitQueryBuilderBase<T, TReturn, TQueryOptions, TQuery> : QueryBuilderBase<T, TReturn, TQueryOptions>
        where T : class
        where TReturn : IQuery<T, TQueryOptions>
        where TQuery : IQuery<T, TQueryOptions>
        where TQueryOptions : QueryOptions
    {
        protected readonly TQuery _query;
        protected readonly uint _start;
        protected readonly uint? _length;

        protected LimitQueryBuilderBase(IQueryBuilder<TQuery, TQueryOptions> queryBuilder, uint start, uint? length) : base(queryBuilder.QueryOptions)
        {
            _query = queryBuilder.Build();
            _start = start;
            _length = length;
            Columns = [];
        }

        internal string GenerateQuery()
        {
            string result = _query.Text.Replace(";", "");

            if (_length.HasValue)
            {
                return "{0} OFFSET {1} ROWS FETCH NEXT {2} ROWS ONLY;".Replace("{0}", result).Replace("{1}", _start.ToString()).Replace("{2}", _length.ToString());
            }
            else
            {
                return "{0} OFFSET {1} ROWS;".Replace("{0}", result).Replace("{1}", _start.ToString());
            }
        }
    }

    internal class LimitQueryBuilder<T, TQuery>(IQueryBuilder<TQuery, QueryOptions> queryBuilder, uint start, uint? length) : LimitQueryBuilderBase<T, LimitQuery<T>, QueryOptions, TQuery>(queryBuilder, start, length)
        where T : class
        where TQuery : IQuery<T, QueryOptions>
    {
        public override LimitQuery<T> Build()
        {
            string query = GenerateQuery();
            return new LimitQuery<T>(ref query, _query.Columns, _query.Criteria, QueryOptions);
        }
    }

    internal class LimitQueryBuilder<T, TQuery, TDbConnection>(IQueryBuilder<TQuery, ConnectionOptions<TDbConnection>> queryBuilder, uint start, uint? length) : LimitQueryBuilderBase<T, LimitQuery<T, TDbConnection>, ConnectionOptions<TDbConnection>, TQuery>(queryBuilder, start, length),
        IQueryBuilder<LimitQuery<T, TDbConnection>, ConnectionOptions<TDbConnection>>
        where T : class
        where TQuery : IQuery<T, ConnectionOptions<TDbConnection>>
    {
        public override LimitQuery<T, TDbConnection> Build()
        {
            string query = GenerateQuery();
            return new LimitQuery<T, TDbConnection>(query, _query.Columns, _query.Criteria, QueryOptions);
        }
    }
}
