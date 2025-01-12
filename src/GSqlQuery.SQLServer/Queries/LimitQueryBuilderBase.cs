﻿using GSqlQuery.Cache;
using System.Collections.Generic;

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

        protected LimitQueryBuilderBase(IBuilder<TQuery> queryBuilder, IQueryOptions<TQueryOptions> queryOptions, uint start, uint? length) : base(queryOptions.QueryOptions)
        {
            _query = queryBuilder.Build();
            _start = start;
            _length = length;
        }

        internal string CreateQueryText()
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

        public override TReturn Build()
        {
            string text = CreateQueryText();
            TReturn result = GetQuery(text, _query.Columns, _query.Criteria, QueryOptions);
            return result;
        }

        public abstract TReturn GetQuery(string text, PropertyOptionsCollection columns, IEnumerable<CriteriaDetailCollection> criteria, TQueryOptions queryOptions);
    }

    internal class LimitQueryBuilder<T, TQuery>(IBuilder<TQuery> queryBuilder, IQueryOptions<QueryOptions> queryOptions, uint start, uint? length) : LimitQueryBuilderBase<T, LimitQuery<T>, QueryOptions, TQuery>(queryBuilder, queryOptions, start, length)
        where T : class
        where TQuery : IQuery<T, QueryOptions>
    {
        public override LimitQuery<T> GetQuery(string text, PropertyOptionsCollection columns, IEnumerable<CriteriaDetailCollection> criteria, QueryOptions queryOptions)
        {
            return new LimitQuery<T>(ref text, _classOptions.FormatTableName.Table, columns, criteria, queryOptions);
        }
    }

    internal class LimitQueryBuilder<T, TQuery, TDbConnection>(IBuilder<TQuery> queryBuilder, IQueryOptions<ConnectionOptions<TDbConnection>> queryOptions, uint start, uint? length) : LimitQueryBuilderBase<T, LimitQuery<T, TDbConnection>, ConnectionOptions<TDbConnection>, TQuery>(queryBuilder, queryOptions, start, length),
        IQueryBuilder<LimitQuery<T, TDbConnection>, ConnectionOptions<TDbConnection>>
        where T : class
        where TQuery : IQuery<T, ConnectionOptions<TDbConnection>>
    {
      

        public override LimitQuery<T, TDbConnection> GetQuery(string text, PropertyOptionsCollection columns, IEnumerable<CriteriaDetailCollection> criteria, ConnectionOptions<TDbConnection> queryOptions)
        {
            return new LimitQuery<T, TDbConnection>(ref text, _classOptions.FormatTableName.Table, columns, criteria, queryOptions);
        }
    }
}
