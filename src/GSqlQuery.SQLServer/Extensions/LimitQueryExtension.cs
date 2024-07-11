using System;

namespace GSqlQuery.SQLServer
{
    public static class LimitQueryExtension
    {
        public static IQueryBuilder<LimitQuery<T>, QueryOptions> Limit<T>(this IQueryBuilder<OrderByQuery<T>, QueryOptions> queryBuilder, uint start, uint? length = null)
            where T : class
        {
            if (queryBuilder == null)
            {
                throw new ArgumentNullException(nameof(queryBuilder));
            }
            return new LimitQueryBuilder<T, OrderByQuery<T>>(queryBuilder, start, length);
        }

        public static IQueryBuilder<LimitQuery<T, TDbConnection>, ConnectionOptions<TDbConnection>> Limit<T, TDbConnection>(this IQueryBuilder<OrderByQuery<T, TDbConnection>, ConnectionOptions<TDbConnection>> queryBuilder, uint start, uint? length = null) where T : class
        {
            if (queryBuilder == null)
            {
                throw new ArgumentNullException(nameof(queryBuilder));
            }

            return new LimitQueryBuilder<T, OrderByQuery<T, TDbConnection>, TDbConnection>(queryBuilder, start, length);
        }
    }
}
