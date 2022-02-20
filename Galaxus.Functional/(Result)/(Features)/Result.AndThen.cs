using System;
using System.Threading.Tasks;

namespace Galaxus.Functional
{
    public sealed partial class Result<TOk, TErr>
    {
        /// <summary>
        ///     Returns <paramref name="result" /> if <b>self</b> contains <b>Ok</b>, otherwise returns the <b>Err</b> value
        ///     contained in <b>self</b>.
        /// </summary>
        /// <typeparam name="TNewOk">The <b>Ok</b> type of <paramref name="result" />.</typeparam>
        /// <param name="result">The result to return if <b>self</b> is <b>Ok</b>.</param>
        public Result<TNewOk, TErr> And<TNewOk>(Result<TNewOk, TErr> result)
        {
            return Match(
                ok =>
                {
                    if (result is null)
                    {
                        throw new ArgumentNullException(nameof(result));
                    }

                    return result;
                },
                err => err
            );
        }

        /// <summary>
        ///     Calls <paramref name="continuation" /> if <b>self</b> contains <b>Ok</b>, otherwise returns the <b>Err</b> value
        ///     contained in <b>self</b>.
        ///     This function can be used for control flow based on <see cref="Result{TOk, TErr}" />s.
        /// </summary>
        /// <typeparam name="TContinuationOk">
        ///     The <b>Ok</b> type of the <paramref name="continuation" />'s
        ///     <see cref="Result{TOk, TErr}" />.
        /// </typeparam>
        /// <param name="continuation">The function to call if <b>self</b> contains <b>Ok</b>.</param>
        public Result<TContinuationOk, TErr> AndThen<TContinuationOk>(
            Func<TOk, Result<TContinuationOk, TErr>> continuation)
        {
            return Match(
                ok =>
                {
                    if (continuation is null)
                    {
                        throw new ArgumentNullException(nameof(continuation));
                    }

                    return continuation(arg: ok);
                },
                err => err
            );
        }

        /// <summary>
        ///     Calls <paramref name="continuation" /> if <b>self</b> contains <b>Ok</b>, otherwise returns the <b>Err</b> value
        ///     contained in <b>self</b>.
        ///     This function can be used for control flow based on <see cref="Result{TOk, TErr}" />s.
        /// </summary>
        /// <param name="continuation">The function to call if <b>self</b> is <b>Ok</b>.</param>
        public Result<TOk, TErr> AndThen(Action<TOk> continuation)
        {
            IfOk(onOk: continuation);
            return this;
        }

        /// <summary>
        ///     Calls <paramref name="continuation" /> if <b>self</b> contains <b>Ok</b>, otherwise returns the <b>Err</b> value
        ///     contained in <b>self</b>.
        ///     This function can be used for control flow based on <see cref="Result{TOk, TErr}" />s.
        /// </summary>
        /// <typeparam name="TContinuationOk">
        ///     The <b>Ok</b> type of the <paramref name="continuation" />'s
        ///     <see cref="Result{TOk, TErr}" />.
        /// </typeparam>
        /// <param name="continuation">The function to call if <b>self</b> contains <b>Ok</b>.</param>
        public Task<Result<TContinuationOk, TErr>> AndThenAsync<TContinuationOk>(
            Func<TOk, Task<Result<TContinuationOk, TErr>>> continuation)
        {
            return Match(
                onOk: continuation,
                err => Task.FromResult(err.ToErr<TContinuationOk, TErr>()));
        }
    }
}
