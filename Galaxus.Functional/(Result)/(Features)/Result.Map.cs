using System;
using System.Threading.Tasks;

namespace Galaxus.Functional
{
    public sealed partial class Result<TOk, TErr>
    {
        /// <summary>
        ///     Maps a <see cref="Result{TOk, TErr}" /> to <see cref="Result{TOkTo, TErr}" /> by applying a function to a contained
        ///     <b>Ok</b> value,
        ///     leaving an <b>Err</b> value untouched. This function can be used to compose the results of two functions.
        /// </summary>
        /// <typeparam name="TOkTo">The type to map <b>Ok</b> to.</typeparam>
        /// <param name="map">The mapping function.</param>
        public Result<TOkTo, TErr> Map<TOkTo>(Func<TOk, TOkTo> map)
        {
            return Match(
                ok =>
                {
                    if (map == null)
                        throw new ArgumentNullException(nameof(map));

                    return map(arg: ok);
                },
                err => err.ToErr<TOkTo, TErr>()
            );
        }

        /// <summary>
        ///     Maps a <see cref="Result{TOk, TErr}" /> to <see cref="Result{TOkTo, TErr}" /> by applying a function to a contained
        ///     <b>Ok</b> value,
        ///     leaving an <b>Err</b> value untouched. This function can be used to compose the results of two functions.
        /// </summary>
        /// <typeparam name="TOkTo">The type to map <b>Ok</b> to.</typeparam>
        /// <param name="continuation">The mapping function.</param>
        public Task<Result<TOkTo, TErr>> MapAsync<TOkTo>(Func<TOk, Task<TOkTo>> continuation)
        {
            return MatchAsync(
                async ok => new Result<TOkTo, TErr>(await continuation(arg: ok)),
                err => err.ToErr<TOkTo, TErr>());
        }

        /// <summary>
        ///     Maps a <see cref="Result{TOk, TErr}" /> to <see cref="Result{TOk, TErrTo}" /> by applying a function to a contained
        ///     <b>Err</b> value,
        ///     leaving an <b>Ok</b> value untouched. This function can be used to compose the results of two functions.
        /// </summary>
        /// <typeparam name="TErrTo">The type to map "Err" to.</typeparam>
        /// <param name="map">The mapping function.</param>
        public Result<TOk, TErrTo> MapErr<TErrTo>(Func<TErr, TErrTo> map)
        {
            return Match(
                ok => ok.ToOk<TOk, TErrTo>(),
                err =>
                {
                    if (map == null)
                        throw new ArgumentNullException(nameof(map));

                    return map(arg: err);
                }
            );
        }

        /// <summary>
        ///     Maps a <see cref="Result{TOk, TErr}" /> to <see cref="Result{TOk, TErrTo}" /> by applying a function to a contained
        ///     <b>Err</b> value,
        ///     leaving an <b>Ok</b> value untouched. This function can be used to compose the results of two functions.
        /// </summary>
        /// <typeparam name="TErrTo">The type to map "Err" to.</typeparam>
        /// <param name="continuation">The mapping function.</param>
        public Task<Result<TOk, TErrTo>> MapErrAsync<TErrTo>(Func<TErr, Task<TErrTo>> continuation)
        {
            return MatchAsync(
                ok => ok.ToOk<TOk, TErrTo>(),
                async err => new Result<TOk, TErrTo>(await continuation(arg: err)));
        }
    }
}
