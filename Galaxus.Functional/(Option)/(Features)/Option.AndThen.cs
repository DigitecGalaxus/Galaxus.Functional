using System;

namespace Galaxus.Functional
{
    public readonly partial struct Option<T>
    {
        /// <summary>
        ///     Returns <b>None</b> if <b>self</b> contains <b>None</b>, otherwise returns <paramref name="fallback" />.
        /// </summary>
        /// <param name="fallback">The value to return if <b>self</b> contains <b>Some</b>.</param>
        public Option<U> And<U>(Option<U> fallback)
        {
            return IsNone ? Option<U>.None : fallback;
        }

        /// <summary>
        ///     Returns <b>None</b> if <b>self</b> contains <b>None</b>, otherwise calls <paramref name="continuation" /> with the
        ///     wrapped value and returns the result.
        ///     Some languages call this operation flatmap.
        /// </summary>
        /// <param name="continuation">The function to call if <b>self</b> contains <b>Some</b>.</param>
        public Option<U> AndThen<U>(Func<T, Option<U>> continuation)
        {
            if (IsNone)
                return Option<U>.None;

            if (continuation is null)
                throw new ArgumentNullException(nameof(continuation));

            return continuation(arg: _some);
        }

        /// <summary>
        ///     Returns <b>None</b> if <b>self</b> contains <b>None</b>, otherwise calls <paramref name="continuation" /> with the
        ///     wrapped value.
        ///     Some languages call this operation flatmap.
        /// </summary>
        /// <param name="continuation">The function to call if <b>self</b> contains <b>Some</b>.</param>
        public Option<T> AndThen(Action<T> continuation)
        {
            IfSome(onSome: continuation);
            return this;
        }
    }
}
