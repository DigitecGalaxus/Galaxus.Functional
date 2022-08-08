using System;

namespace Galaxus.Functional
{
    public readonly partial struct Option<T>
    {
        /// <summary>
        ///     Returns <b>self</b> if it contains <b>Some</b>, otherwise returns <paramref name="fallback" />.
        /// </summary>
        /// <param name="fallback">
        ///     The value to return if <b>self</b> contains <b>None</b>.
        ///     This argument is eagerly evaluated; if you are passing the result of a function call,
        ///     it is recommended to use <see cref="OrElse(System.Func{Galaxus.Functional.Option{T}})" />, which is lazily
        ///     evaluated.
        /// </param>
        public Option<T> Or(Option<T> fallback)
        {
            return IsSome ? this : fallback;
        }

        /// <summary>
        ///     Returns <b>self</b> if it contains <b>Some</b>, otherwise calls <paramref name="fallback" /> and returns the
        ///     result.
        /// </summary>
        /// <param name="fallback">The function to call if <b>self</b> contains <b>None</b>.</param>
        public Option<T> OrElse(Func<Option<T>> fallback)
        {
            if (IsSome)
            {
                return this;
            }

            if (fallback is null)
            {
                throw new ArgumentNullException(nameof(fallback));
            }

            return fallback();
        }

        /// <summary>
        ///     Returns <b>Some</b> if exactly one of <b>self</b> and <paramref name="fallback" /> is <b>Some</b>, otherwise
        ///     returns <b>None</b>.
        /// </summary>
        /// <param name="fallback">The <see cref="Option{T}" /> to compare against.</param>
        public Option<T> Xor(Option<T> fallback)
        {
            if (IsSome == fallback.IsSome)
            {
                return None;
            }

            return IsSome ? this : fallback;
        }
    }
}
