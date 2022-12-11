using System;

namespace Galaxus.Functional
{
    public sealed partial class Result<TOk, TErr>
    {
        /// <summary>
        ///     Returns <paramref name="fallback" /> if <b>self</b> contains <b>Err</b>, otherwise returns the <b>Ok</b> value
        ///     contained in <b>self</b>.
        /// </summary>
        /// <param name="fallback">
        ///     The result to return if <b>self</b> contains <b>Err</b>.
        ///     This argument is eagerly evaluated; if you are passing the result of a function call,
        ///     it is recommended to use <see cref="OrElse{TContinuationErr}" />, which is lazily evaluated.
        /// </param>
        public Result<TOk, TContinuationErr> Or<TContinuationErr>(Result<TOk, TContinuationErr> fallback)
        {
            return IsOk ? _ok : fallback;
        }

        /// <summary>
        ///     Calls <paramref name="continuation" /> if <b>self</b> contains <b>Err</b>, otherwise returns the <b>Ok</b> value
        ///     contained in <b>self</b>.
        ///     This function can be used for control flow based on <see cref="Result{TOk, TErr}" />s.
        /// </summary>
        /// <param name="continuation">The function to call if <b>self</b> contains <b>Err</b>.</param>
        public Result<TOk, TContinuationErr> OrElse<TContinuationErr>(
            Func<TErr, Result<TOk, TContinuationErr>> continuation)
        {
            if (IsOk)
            {
                return _ok;
            }

            if (continuation is null)
            {
                throw new ArgumentNullException(nameof(continuation));
            }

            return continuation(_err);
        }
    }
}
