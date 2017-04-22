namespace MediaLibrary.Utilities.Linq
{
    using System.Linq;
    using System.Linq.Expressions;

    /// <summary>
    /// Provides extension methods for LINQ to SQL queries.
    /// </summary>
    public static class IQueryableExtensions
    {
        /// <summary>
        /// Sorts the elements of a sequence in ascending order according to a key.
        /// </summary>
        /// <typeparam name="T">The declaring type of the source object.</typeparam>
        /// <param name="source">A sequence of values to order.</param>
        /// <param name="propertyName">The string name of the property within the source.</param>
        public static IOrderedQueryable<T> OrderBy<T>(this IQueryable<T> source, string propertyName)
        {
            return OrderingHelper(source, propertyName, false, false);
        }

        /// <summary>
        /// Sorts the elements of a sequence in ascending order according to a key.
        /// </summary>
        /// <typeparam name="T">The declaring type of the source object.</typeparam>
        /// <param name="source">A sequence of values to order.</param>
        /// <param name="propertyName">The string name of the property within the source.</param>
        public static IOrderedQueryable<T> OrderByDescending<T>(this IQueryable<T> source, string propertyName)
        {
            return OrderingHelper(source, propertyName, true, false);
        }

        /// <summary>
        /// Performs a subsequent ordering of the elements in a sequence in ascending order according to a key.
        /// </summary>
        /// <typeparam name="T">The declaring type of the source object.</typeparam>
        /// <param name="source">A sequence of values to order.</param>
        /// <param name="propertyName">The string name of the property within the source.</param>
        public static IOrderedQueryable<T> ThenBy<T>(this IOrderedQueryable<T> source, string propertyName)
        {
            return OrderingHelper(source, propertyName, false, true);
        }

        /// <summary>
        /// Performs a subsequent ordering of the elements in a sequence in ascending order according to a key.
        /// </summary>
        /// <typeparam name="T">The declaring type of the source object.</typeparam>
        /// <param name="source">A sequence of values to order.</param>
        /// <param name="propertyName">The string name of the property within the source.</param>
        public static IOrderedQueryable<T> ThenByDescending<T>(this IOrderedQueryable<T> source, string propertyName)
        {
            return OrderingHelper(source, propertyName, true, true);
        }

        private static IOrderedQueryable<T> OrderingHelper<T>(IQueryable<T> source, string propertyName, bool descending, bool anotherLevel)
        {
            var param = Expression.Parameter(typeof(T), "p");
            var property = Expression.PropertyOrField(param, propertyName);
            var sort = Expression.Lambda(property, param);

            var call = Expression.Call(
                typeof(Queryable),
                (!anotherLevel ? "OrderBy" : "ThenBy") + (descending ? "Descending" : string.Empty),
                new[] { typeof(T), property.Type },
                source.Expression,
                Expression.Quote(sort));

            return (IOrderedQueryable<T>)source.Provider.CreateQuery<T>(call);
        }
    }
}
