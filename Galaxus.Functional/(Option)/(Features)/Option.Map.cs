using System;

namespace Galaxus.Functional
{
    public readonly partial struct Option<T>
    {
        /// <summary>
        ///     Maps a <see cref="Option{T}" /> to <see cref="Option{U}" /> by applying a function to a contained <b>Some</b>
        ///     value.
        /// </summary>
        /// <typeparam name="TTo">The type to map to.</typeparam>
        /// <param name="map">The mapping function.</param>
        public Option<TTo> Map<TTo>(Func<T, TTo> map)
        {
            return Match(
                some =>
                {
                    if (map == null)
                    {
                        throw new ArgumentNullException(nameof(map));
                    }

                    return map(some).ToOption();
                },
                () => Option<TTo>.None
            );
        }

        /// <summary>
        ///     Applies a function to the contained <b>Some</b> value (if any). Returns <paramref name="fallback" /> otherwise.
        /// </summary>
        /// <typeparam name="TTo">The type to map to.</typeparam>
        /// <param name="fallback">The value to return if <b>self</b> contains <b>None</b>.</param>
        /// <param name="map">The mapping function.</param>
        public TTo MapOr<TTo>(Func<T, TTo> map, TTo fallback)
        {
            return Match(
                some =>
                {
                    if (map == null)
                    {
                        throw new ArgumentNullException(nameof(map));
                    }

                    return map(some);
                },
                () => fallback
            );
        }

        /// <summary>
        ///     Applies a function to the contained <b>Some</b> value (if any), otherwise calls <paramref name="fallback" /> and returns the result.
        /// </summary>
        /// <typeparam name="TTo">The type to map to.</typeparam>
        /// <param name="fallback">The function to call if <b>self</b> contains <b>None</b>.</param>
        /// <param name="map">The mapping function.</param>
        public TTo MapOrElse<TTo>(Func<T, TTo> map, Func<TTo> fallback)
        {
            return Match(
                some =>
                {
                    if (map == null)
                    {
                        throw new ArgumentNullException(nameof(map));
                    }

                    return map(some);
                },
                () =>
                {
                    if (fallback == null)
                    {
                        throw new ArgumentNullException(nameof(fallback));
                    }

                    return fallback();
                }
            );
        }
    }
}
