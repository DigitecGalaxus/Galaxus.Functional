using System;
using System.Collections.Generic;
using System.Linq;

namespace Galaxus.Functional.Linq
{
    /// <summary>
    ///     Extensions on top of LINQ using <see cref="Option{T}" />.
    /// </summary>
    public static class OptionLinqExtensions
    {
        /// <summary>
        ///     Returns the element at a specified index in a sequence or <see cref="Option{T}.None" /> if the index is out of
        ///     range.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of <c>source</c>.</typeparam>
        /// <param name="source">An <see cref="IEnumerable{T}" /> to return an element from.</param>
        /// <param name="index">The zero-based index of the element to retrieve.</param>
        /// <returns>
        ///     <c><see cref="Option{T}" />.None</c> if the index is outside the bounds of the source sequence; otherwise, the
        ///     element at the specified position in the source sequence.
        /// </returns>
        /// <exception cref="ArgumentNullException"><c>source</c> is <c>null</c>.</exception>
        public static Option<TSource> ElementAtOrNone<TSource>(this IEnumerable<TSource> source, int index)
            => source
                .Select(Option<TSource>.Some)
                .ElementAtOrDefault(index);

        /// <summary>
        ///     Returns the first element of the sequence that satisfies a condition or <see cref="Option{T}.None" /> if the
        ///     sequence contains no elements.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of <c>source</c>.</typeparam>
        /// <param name="source">An <see cref="IEnumerable{T}" /> to return an element from.</param>
        /// <returns>
        ///     <c><see cref="Option{T}" />.None</c> if <c>source</c> is empty; otherwise, the first element in <c>source</c>.
        /// </returns>
        public static Option<TSource> FirstOrNone<TSource>(this IEnumerable<TSource> source)
            => source.FirstOrNone(True);

        /// <summary>
        ///     Returns the first element of the sequence that satisfies a condition or <see cref="Option{T}.None" /> if the
        ///     sequence contains no elements.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of <c>source</c>.</typeparam>
        /// <param name="source">An <see cref="IEnumerable{T}" /> to return an element from.</param>
        /// <param name="predicate">A function to test each element for a condition.</param>
        /// <returns>
        ///     <c><see cref="Option{T}" />.None</c> if <c>source</c> is empty or if no element passes the test specified by
        ///     <c>predicate</c>; otherwise, the first element in <c>source</c> that passes the test specified by <c>predicate</c>.
        /// </returns>
        /// <exception cref="ArgumentNullException"><c>source</c> or <c>predicate</c> is <c>null</c>.</exception>
        public static Option<TSource> FirstOrNone<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate)
            => source
                .Where(predicate)
                .Select(Option<TSource>.Some)
                .FirstOrDefault();

        /// <summary>
        ///     Returns the last element of a sequence, or <see cref="Option{T}.None" /> if the sequence contains no elements.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of <c>source</c>.</typeparam>
        /// <param name="source">An <see cref="IEnumerable{T}" /> to return the last element of.</param>
        /// <returns>
        ///     <c><see cref="Option{T}" />.None</c> if the source sequence is empty; otherwise, the last element in the
        ///     <see cref="IEnumerable{T}" />.
        /// </returns>
        /// <exception cref="ArgumentNullException"><c>source</c> is <c>null</c>.</exception>
        public static Option<TSource> LastOrNone<TSource>(this IEnumerable<TSource> source)
            => source.LastOrNone(True);

        /// <summary>
        ///     Returns the last element of a sequence that satisfies a condition or <see cref="Option{T}.None" /> if no such
        ///     element is found.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of <c>source</c>.</typeparam>
        /// <param name="source">An <see cref="IEnumerable{T}" /> to return an element from.</param>
        /// <param name="predicate">A function to test each element for a condition.</param>
        /// <returns>
        ///     <c><see cref="Option{T}" />.None</c> if the sequence is empty or if no elements pass the test in the predicate
        ///     function; otherwise, the last element that passes the test in the predicate function.
        /// </returns>
        /// <exception cref="ArgumentNullException"><c>source</c> or <c>predicate</c> is <c>null</c>.</exception>
        public static Option<TSource> LastOrNone<TSource>(this IEnumerable<TSource> source,
            Func<TSource, bool> predicate)
            => source
                .Where(predicate)
                .Select(Option<TSource>.Some)
                .LastOrDefault();

        /// <summary>
        ///     Returns the only element of a sequence, or <see cref="Option{T}.None" /> if the sequence is empty; this method
        ///     throws an exception if there is more than one element in the sequence.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of <c>source</c>.</typeparam>
        /// <param name="source">An <see cref="IEnumerable{T}" /> to return the single element of.</param>
        /// <returns>
        ///     The single element of the input sequence, or
        ///     <c>
        ///         <see cref="Option{T}.None" />
        ///     </c>
        ///     if the sequence contains no elements.
        /// </returns>
        /// <exception cref="ArgumentNullException"><c>source</c> is <c>null</c>.</exception>
        /// <exception cref="InvalidOperationException">The input sequence contains more than one element.</exception>
        public static Option<TSource> SingleOrNone<TSource>(this IEnumerable<TSource> source)
            => source.SingleOrNone(True);

        /// <summary>
        ///     Returns the only element of a sequence that satisfies a specified condition or <see cref="Option{T}.None" /> if no
        ///     such element exists; this method throws an exception if more than one element satisfies the condition.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of <c>source</c>.</typeparam>
        /// <param name="source">An <see cref="IEnumerable{T}" /> to return a single element from.</param>
        /// <param name="predicate">A function to test an element for a condition.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"><c>source</c> or <c>predicate</c> is <c>null</c>.</exception>
        /// <exception cref="InvalidOperationException">More than one element satisfies the condition in <c>source</c>.</exception>
        public static Option<TSource> SingleOrNone<TSource>(this IEnumerable<TSource> source,
            Func<TSource, bool> predicate)
            => source
                .Where(predicate)
                .Select(Option<TSource>.Some)
                .SingleOrDefault();

        /// <summary>
        /// Checks a dictionary if it contains a value for a given key. If so this value is wrapped in an Option.
        /// Else <see cref="Option{T}.None"/> is returned.
        /// </summary>
        /// <param name="source">The dictionary in question</param>
        /// <param name="key">The key to check if a value exists</param>
        /// <returns>The value of the dictionary or None</returns>
        public static Option<TSome> GetValueOrNone<TKey, TSome>(
            this IEnumerable<KeyValuePair<TKey, TSome>> source,
            TKey key)
        {
            switch (source)
            {
                case IDictionary<TKey, TSome> dictionary:
                    return dictionary.ContainsKey(key) ? Option<TSome>.Some(dictionary[key]) : Option<TSome>.None;
                case IReadOnlyDictionary<TKey, TSome> readOnlyDictionary:
                    return readOnlyDictionary.ContainsKey(key)
                        ? Option<TSome>.Some(readOnlyDictionary[key])
                        : Option<TSome>.None;
                default:
                    return source.SingleOrNone(kvp => kvp.Key.Equals(key)).Map(kvp => kvp.Value);
            }
        }

        // This probably should be defined publicly together with False and Identity
        private static bool True<TSource>(TSource _)
            => true;
    }
}