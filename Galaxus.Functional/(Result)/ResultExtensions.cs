using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Galaxus.Functional
{
    public static class ResultExtensions
    {
        #region Instance Initializer

        /// <summary>
        /// Wraps <paramref name="self"/> into a <see cref="Result{TOk, TErr}"/> containing <b>Ok</b>.
        /// </summary>
        public static Result<TOk, TErr> ToOk<TOk, TErr>(this TOk self)
            => Result<TOk, TErr>.FromOk(self);

        /// <summary>
        /// Wraps <paramref name="self"/> into a <see cref="Result{TOk, TErr}"/> containing <b>Err</b>.
        /// </summary>
        public static Result<TOk, TErr> ToErr<TOk, TErr>(this TErr self)
            => Result<TOk, TErr>.FromErr(self);

        #endregion

        #region Auto Map

        /// <summary>
        /// Automatically maps <b>Ok</b> to a different type.
        /// </summary>
        /// <typeparam name="TOkFrom">The current type of <b>Ok</b>. This type must derive from <typeparamref name="TOkTo"/>.</typeparam>
        /// <typeparam name="TOkTo">The type to map <b>Ok</b> to.</typeparam>
        /// <typeparam name="TErr">The type of <b>Err</b> which will remain untouched.</typeparam>
        public static Result<TOkTo, TErr> Map<TOkFrom, TOkTo, TErr>(this Result<TOkFrom, TErr> self) where TOkFrom : TOkTo
        {
            return self.Match(
                ok => ok,
                err => err.ToErr<TOkTo, TErr>()
            );
        }

        /// <summary>
        /// Automatically maps <b>Err</b> to a different type.
        /// </summary>
        /// <typeparam name="TOk">The type of <b>Ok</b> which will remain untouched.</typeparam>
        /// <typeparam name="TErrFrom">The current type of <b>Err</b>. This type must derive from <typeparamref name="TErrTo"/>.</typeparam>
        /// <typeparam name="TErrTo">The type to map <b>Err</b> to.</typeparam>
        public static Result<TOk, TErrTo> MapErr<TOk, TErrFrom, TErrTo>(this Result<TOk, TErrFrom> self) where TErrFrom : TErrTo
        {
            return self.Match(
                ok => ok.ToOk<TOk, TErrTo>(),
                err => err
            );
        }

        #endregion

        #region Map

        /// <summary>
        /// Maps a <see cref="Result{TOk, TErr}"/> to <see cref="Result{TOkTo, TErr}"/> by applying a function to a contained <b>Ok</b> value,
        /// leaving an <b>Err</b> value untouched. This function can be used to compose the results of two functions.
        /// </summary>
        /// <typeparam name="TOkTo">The type to map <b>Ok</b> to.</typeparam>
        /// <param name="continuation">The mapping function.</param>
        public static async Task<Result<TOkTo, TErr>> MapAsync<TOk, TErr, TOkTo>(
            this Task<Result<TOk, TErr>> self,
            Func<TOk, Task<TOkTo>> continuation)
        {
            return await (await self).MapAsync(continuation);
        }

        /// <summary>
        /// Maps a <see cref="Result{TOk, TErr}"/> to <see cref="Result{TOkTo, TErr}"/> by applying a function to a contained <b>Ok</b> value,
        /// leaving an <b>Err</b> value untouched. This function can be used to compose the results of two functions.
        /// </summary>
        /// <typeparam name="TOkTo">The type to map <b>Ok</b> to.</typeparam>
        /// <param name="continuation">The mapping function.</param>
        public static Task<Result<TOkTo, TErr>> MapAsync<TOk, TErr, TOkTo>(
            this Task<Result<TOk, TErr>> self,
            Func<TOk, TOkTo> continuation)
        {
            return self.MapAsync(ok => Task.FromResult(continuation(ok)));
        }

        /// <summary>
        /// Maps a <see cref="Result{TOk, TErr}"/> to <see cref="Result{TOk, TErrTo}"/> by applying a function to a contained <b>Err</b> value,
        /// leaving an <b>Ok</b> value untouched. This function can be used to compose the results of two functions.
        /// </summary>
        /// <typeparam name="TErrTo">The type to map "Err" to.</typeparam>
        /// <param name="continuation">The mapping function.</param>
        public static async Task<Result<TOk, TErrTo>> MapErrAsync<TOk, TErr, TErrTo>(
            this Task<Result<TOk, TErr>> self,
            Func<TErr, Task<TErrTo>> continuation)
        {
            return await (await self).MapErrAsync(continuation);
        }

        /// <summary>
        /// Maps a <see cref="Result{TOk, TErr}"/> to <see cref="Result{TOk, TErrTo}"/> by applying a function to a contained <b>Err</b> value,
        /// leaving an <b>Ok</b> value untouched. This function can be used to compose the results of two functions.
        /// </summary>
        /// <typeparam name="TErrTo">The type to map "Err" to.</typeparam>
        /// <param name="continuation">The mapping function.</param>
        public static Task<Result<TOk, TErrTo>> MapErrAsync<TOk, TErr, TErrTo>(
            this Task<Result<TOk, TErr>> self,
            Func<TErr, TErrTo> continuation)
        {
            return self.MapErrAsync(err => Task.FromResult(continuation(err)));
        }

        #endregion

        #region Enumerations

        /// <summary>
        /// Returns a subset of <paramref name="self"/> which contains all <b>Ok</b> values in <paramref name="self"/>.
        /// </summary>
        public static IEnumerable<TOk> SelectOk<TOk, TErr>(this IEnumerable<Result<TOk, TErr>> self)
            => self.Where(v => v.IsOk).Select(v => v.Unwrap());

        /// <summary>
        /// Returns a subset of <paramref name="self"/> which contains all <b>Ok</b> values in <paramref name="self"/>.
        /// Then runs it through the <paramref name="selector"/>.
        /// </summary>
        public static IEnumerable<TSelection> SelectOk<TOk, TErr, TSelection>(this IEnumerable<Result<TOk, TErr>> self, Func<TOk, TSelection> selector)
            => self.SelectOk().Select(selector);

        /// <summary>
        /// Returns a subset of <paramref name="self"/> which contains all <b>Err</b> values in <paramref name="self"/>.
        /// </summary>
        public static IEnumerable<TErr> SelectErr<TOk, TErr>(this IEnumerable<Result<TOk, TErr>> self)
            => self.Where(v => v.IsErr).Select(v => v.Err.Unwrap());

        /// <summary>
        /// Returns a subset of <paramref name="self"/> which contains all <b>Err</b> values in <paramref name="self"/>.
        /// Then runs it through the <paramref name="selector"/>.
        /// </summary>
        public static IEnumerable<TSelection> SelectErr<TOk, TErr, TSelection>(this IEnumerable<Result<TOk, TErr>> self, Func<TErr, TSelection> selector)
            => self.SelectErr().Select(selector);

        #endregion

        #region Match

        public static async Task<TResult> MatchAsync<TOk, TErr, TResult>(
            this Task<Result<TOk, TErr>> self,
            Func<TOk, Task<TResult>> onOk,
            Func<TErr, Task<TResult>> onErr)
        {
            return await (await self).MatchAsync(onOk, onErr);
        }

        public static async Task<TResult> MatchAsync<TOk, TErr, TResult>(
            this Task<Result<TOk, TErr>> self,
            Func<TOk, TResult> onOk,
            Func<TErr, TResult> onErr)
        {
            return (await self).Match(onOk, onErr);
        }

        public static async Task<TResult> MatchAsync<TOk, TErr, TResult>(
            this Task<Result<TOk, TErr>> self,
            Func<TOk, Task<TResult>> onOk,
            Func<TErr, TResult> onErr)
        {
            return await (await self).MatchAsync(onOk, onErr);
        }

        public static async Task<TResult> MatchAsync<TOk, TErr, TResult>(
            this Task<Result<TOk, TErr>> self,
            Func<TOk, TResult> onOk,
            Func<TErr, Task<TResult>> onErr)
        {
            return await (await self).MatchAsync(onOk, onErr);
        }

        public static async Task<Result<TOkResult, TErrResult>> MatchResultAsync<TOk, TErr, TOkResult, TErrResult>(
            this Task<Result<TOk, TErr>> self,
            Func<TOk, Task<TOkResult>> onOk,
            Func<TErr, Task<TErrResult>> onErr)
        {
            return await (await self).Match(
                async ok => Result<TOkResult, TErrResult>.FromOk(await onOk(ok)),
                async err => Result<TOkResult, TErrResult>.FromErr(await onErr(err)));
        }

        public static async Task<Result<TOkResult, TErrResult>> MatchResultAsync<TOk, TErr, TOkResult, TErrResult>(
            this Task<Result<TOk, TErr>> self,
            Func<TOk, Task<Result<TOkResult, TErrResult>>> onOk,
            Func<TErr, TErrResult> onErr)
        {
            return await (await self).Match(
                async ok => await onOk(ok),
                err => Task.FromResult(Result<TOkResult, TErrResult>.FromErr(onErr(err))));
        }

        #endregion

        #region AndThen

        /// <summary>
        /// Calls <paramref name="continuation"/> if <b>self</b> contains <b>Ok</b>, otherwise returns the <b>Err</b> value contained in <b>self</b>.
        /// This function can be used for control flow based on <see cref="Result{TOk, TErr}"/>s.
        /// </summary>
        /// <typeparam name="TContinuationOk">The <b>Ok</b> type of the <paramref name="continuation"/>'s <see cref="Result{TOk, TErr}"/>.</typeparam>
        /// <param name="continuation">The function to call if <b>self</b> contains <b>Ok</b>.</param>
        public static async Task<Result<TContinuationOk, TErr>> AndThenAsync<TOk, TErr, TContinuationOk>(
            this Task<Result<TOk, TErr>> self,
            Func<TOk, Task<Result<TContinuationOk, TErr>>> continuation)
        {
            return await (await self).AndThenAsync(continuation);
        }

        /// <summary>
        /// Calls <paramref name="continuation"/> if <b>self</b> contains <b>Ok</b>, otherwise returns the <b>Err</b> value contained in <b>self</b>.
        /// This function can be used for control flow based on <see cref="Result{TOk, TErr}"/>s.
        /// </summary>
        /// <typeparam name="TContinuationOk">The <b>Ok</b> type of the <paramref name="continuation"/>'s <see cref="Result{TOk, TErr}"/>.</typeparam>
        /// <param name="continuation">The function to call if <b>self</b> contains <b>Ok</b>.</param>
        public static Task<Result<TContinuationOk, TErr>> AndThenAsync<TOk, TErr, TContinuationOk>(
            this Task<Result<TOk, TErr>> self,
            Func<TOk, Result<TContinuationOk, TErr>> continuation)
        {
            return self.AndThenAsync(ok => Task.FromResult(continuation(ok)));
        }

        #endregion

        #region Or

        /// <summary>
        /// Calls <paramref name="continuation"/> if <b>self</b> contains <b>Err</b>, otherwise returns the <b>Ok</b> value contained in <b>self</b>.
        /// This function can be used for control flow based on <see cref="Result{TOk, TErr}"/>s.
        /// </summary>
        /// <typeparam name="TContinuationErr">The <b>Error</b> type of the <paramref name="continuation"/>'s <see cref="Result{TOk, TErr}"/>.</typeparam>
        /// <param name="continuation">The function to call if <b>self</b> contains <b>Err</b>.</param>
        public static async Task<Result<TOk, TContinuationErr>> OrElseAsync<TOk, TContinuationErr>(
            this Task<Result<TOk, TContinuationErr>> self,
            Func<TContinuationErr, Task<Result<TOk, TContinuationErr>>> continuation)
        {
            return await (await self).OrElseAsync(continuation);
        }

        /// <summary>
        /// Calls <paramref name="continuation"/> if <b>self</b> contains <b>Err</b>, otherwise returns the <b>Ok</b> value contained in <b>self</b>.
        /// This function can be used for control flow based on <see cref="Result{TOk, TErr}"/>s.
        /// </summary>
        /// <typeparam name="TContinuationErr">The <b>Error</b> type of the <paramref name="continuation"/>'s <see cref="Result{TOk, TErr}"/>.</typeparam>
        /// <param name="continuation">The function to call if <b>self</b> contains <b>Err</b>.</param>
        public static Task<Result<TOk, TContinuationErr>> OrElseAsync<TOk, TContinuationErr>(
            this Task<Result<TOk, TContinuationErr>> self,
            Func<TContinuationErr, Result<TOk, TContinuationErr>> continuation)
        {
            return self.OrElseAsync(err => Task.FromResult(continuation(err)));
        }


        #endregion

        /// <summary>
        /// Transposes a <see cref="Result{TOk, TErr}"/> of an <see cref="Option{T}"/> into an <see cref="Option{T}"/> of a <see cref="Result{TOk, TErr}"/>.
        /// <b>Ok(None)</b> will be mapped to <b>None</b>.
        /// <b>Ok(Some(TOk))</b> will be mapped to <b>Some(Ok(TOk))</b>.
        /// <b>Err(TErr)</b> will be mapped to <b>Some(Err(TErr))</b>.
        /// </summary>
        public static Option<Result<TOk, TErr>> Transpose<TOk, TErr>(this Result<Option<TOk>, TErr> self)
        {
            return self.Match(
                ok => ok.Match(
                    some => some.ToOk<TOk, TErr>().ToOption(),
                    () => None.Value
                ),
                err => err.ToErr<TOk, TErr>().ToOption()
            );
        }
    }
}
