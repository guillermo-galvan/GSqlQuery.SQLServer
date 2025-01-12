using System;

namespace GSqlQuery.SQLServer
{
    public static class LimitQueryExtension
    {
        public static IQueryBuilder<LimitQuery<T>, QueryOptions> Limit<T>(this IOrderByQueryBuilder<T, OrderByQuery<T>, QueryOptions> queryBuilder, uint start, uint? length = null)
            where T : class
        {
            return queryBuilder == null ? throw new ArgumentNullException(nameof(queryBuilder)) : new LimitQueryBuilder<T, OrderByQuery<T>>(queryBuilder, queryBuilder, start, length);
        }

        public static IQueryBuilder<LimitQuery<T, TDbConnection>, ConnectionOptions<TDbConnection>> Limit<T, TDbConnection>(this IOrderByQueryBuilder<T, OrderByQuery<T, TDbConnection>, ConnectionOptions<TDbConnection>> queryBuilder, uint start, uint? length = null)
            where T : class
        {
            return queryBuilder == null ? throw new ArgumentNullException(nameof(queryBuilder)) : new LimitQueryBuilder<T, OrderByQuery<T, TDbConnection>, TDbConnection>(queryBuilder, queryBuilder, start, length);
        }

        public static IQueryBuilder<LimitQuery<T>, QueryOptions> Limit<T>(this IJoinOrderByQueryBuilder<T, OrderByQuery<T>, QueryOptions> queryBuilder, uint start, uint? length = null)
            where T : class
        {
            return queryBuilder == null ? throw new ArgumentNullException(nameof(queryBuilder)) : new LimitQueryBuilder<T, OrderByQuery<T>>(queryBuilder, queryBuilder, start, length);
        }

        public static IQueryBuilder<LimitQuery<T, TDbConnection>, ConnectionOptions<TDbConnection>> Limit<T, TDbConnection>(this IJoinOrderByQueryBuilder<T, OrderByQuery<T, TDbConnection>, ConnectionOptions<TDbConnection>> queryBuilder, uint start, uint? length = null)
            where T : class
        {
            return queryBuilder == null ? throw new ArgumentNullException(nameof(queryBuilder)) : new LimitQueryBuilder<T, OrderByQuery<T, TDbConnection>, TDbConnection>(queryBuilder, queryBuilder, start, length);
        }
    }
}
