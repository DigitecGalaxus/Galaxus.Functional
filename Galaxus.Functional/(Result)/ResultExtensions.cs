using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Galaxus.Functional
{
    /// <summary>
    ///     Extensions for the result type, providing some quality of life-shorthands.
    /// </summary>
    public static class ResultExtensions
    {
        /// <summary>
        ///     Transposes a <see cref="Result{TOk, TErr}" /> of an <see cref="Option{T}" /> into an <see cref="Option{T}" /> of a
        ///     <see cref="Result{TOk, TErr}" />.
        ///     <b>Ok(None)</b> will be mapped to <b>None</b>.
        ///     <b>Ok(Some(TOk))</b> will be mapped to <b>Some(Ok(TOk))</b>.
        ///     <b>Err(TErr)</b> will be mapped to <b>Some(Err(TErr))</b>.
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

        #region Instance Initializer

        /// <summary>
        ///     Wraps <paramref name="self" /> into a <see cref="Result{TOk, TErr}" /> containing <b>Ok</b>.
        /// </summary>
        public static Result<TOk, TErr> ToOk<TOk, TErr>(this TOk self)
        {
            return Result<TOk, TErr>.FromOk(self);
        }

        /// <summary>
        ///     Wraps <paramref name="self" /> into a <see cref="Result{TOk, TErr}" /> containing <b>Err</b>.
        /// </summary>
        public static Result<TOk, TErr> ToErr<TOk, TErr>(this TErr self)
        {
            return Result<TOk, TErr>.FromErr(self);
        }

        #endregion

        #region Auto Map

        /// <summary>
        ///     Automatically maps <b>Ok</b> to a different type.
        /// </summary>
        /// <typeparam name="TOkFrom">The current type of <b>Ok</b>. This type must derive from <typeparamref name="TOkTo" />.</typeparam>
        /// <typeparam name="TOkTo">The type to map <b>Ok</b> to.</typeparam>
        /// <typeparam name="TErr">The type of <b>Err</b> which will remain untouched.</typeparam>
        public static Result<TOkTo, TErr> Map<TOkFrom, TOkTo, TErr>(this Result<TOkFrom, TErr> self)
            where TOkFrom : TOkTo
        {
            return self.Match(
                ok => ok,
                err => err.ToErr<TOkTo, TErr>()
            );
        }

        /// <summary>
        ///     Automatically maps <b>Err</b> to a different type.
        /// </summary>
        /// <typeparam name="TOk">The type of <b>Ok</b> which will remain untouched.</typeparam>
        /// <typeparam name="TErrFrom">The current type of <b>Err</b>. This type must derive from <typeparamref name="TErrTo" />.</typeparam>
        /// <typeparam name="TErrTo">The type to map <b>Err</b> to.</typeparam>
        public static Result<TOk, TErrTo> MapErr<TOk, TErrFrom, TErrTo>(this Result<TOk, TErrFrom> self)
            where TErrFrom : TErrTo
        {
            return self.Match(
                ok => ok.ToOk<TOk, TErrTo>(),
                err => err
            );
        }

        #endregion

        #region Enumerations

        /// <summary>
        ///     Returns a subset of <paramref name="self" /> which contains all <b>Ok</b> values in <paramref name="self" />.
        /// </summary>
        public static IEnumerable<TOk> SelectOk<TOk, TErr>(this IEnumerable<Result<TOk, TErr>> self)
        {
            return self.Where(v => v.IsOk).Select(v => v.Unwrap());
        }

        /// <summary>
        ///     Returns a subset of <paramref name="self" /> which contains all <b>Ok</b> values in <paramref name="self" />.
        ///     Then runs it through the <paramref name="selector" />.
        /// </summary>
        public static IEnumerable<TSelection> SelectOk<TOk, TErr, TSelection>(
            this IEnumerable<Result<TOk, TErr>> self,
            Func<TOk, TSelection> selector)
        {
            return self.SelectOk().Select(selector);
        }

        /// <summary>
        ///     Returns a subset of <paramref name="self" /> which contains all <b>Err</b> values in <paramref name="self" />.
        /// </summary>
        public static IEnumerable<TErr> SelectErr<TOk, TErr>(this IEnumerable<Result<TOk, TErr>> self)
        {
            return self.Where(v => v.IsErr).Select(v => v.Err.Unwrap());
        }

        /// <summary>
        ///     Returns a subset of <paramref name="self" /> which contains all <b>Err</b> values in <paramref name="self" />.
        ///     Then runs it through the <paramref name="selector" />.
        /// </summary>
        public static IEnumerable<TSelection> SelectErr<TOk, TErr, TSelection>(
            this IEnumerable<Result<TOk, TErr>> self,
            Func<TErr, TSelection> selector)
        {
            return self.SelectErr().Select(selector);
        }

        #endregion

        #region Match

        /// <summary>
        ///     Async overload for <see cref="Result{TOk,TErr}.Match"/>
        /// </summary>
        /// <inheritdoc cref="Result{TOk,TErr}.Match"/>
        public static async Task<TResult> MatchAsync<TOk, TErr, TResult>(
            this Task<Result<TOk, TErr>> self,
            Func<TOk, Task<TResult>> onOk,
            Func<TErr, Task<TResult>> onErr)
        {
            return await (await self).MatchAsync(onOk, onErr);
        }

        /// <summary>
        ///     Async overload for <see cref="Result{TOk,TErr}.Match"/>
        /// </summary>
        /// <inheritdoc cref="Result{TOk,TErr}.Match"/>
        public static async Task<TResult> MatchAsync<TOk, TErr, TResult>(
            this Task<Result<TOk, TErr>> self,
            Func<TOk, TResult> onOk,
            Func<TErr, TResult> onErr)
        {
            return (await self).Match(onOk, onErr);
        }

        /// <summary>
        ///     Async overload for <see cref="Result{TOk,TErr}.Match"/>
        /// </summary>
        /// <inheritdoc cref="Result{TOk,TErr}.Match"/>
        public static async Task<TResult> MatchAsync<TOk, TErr, TResult>(
            this Task<Result<TOk, TErr>> self,
            Func<TOk, Task<TResult>> onOk,
            Func<TErr, TResult> onErr)
        {
            return await (await self).MatchAsync(onOk, onErr);
        }

        /// <summary>
        ///     Async overload for <see cref="Result{TOk,TErr}.Match"/>
        /// </summary>
        /// <inheritdoc cref="Result{TOk,TErr}.Match"/>
        public static async Task<TResult> MatchAsync<TOk, TErr, TResult>(
            this Task<Result<TOk, TErr>> self,
            Func<TOk, TResult> onOk,
            Func<TErr, Task<TResult>> onErr)
        {
            return await (await self).MatchAsync(onOk, onErr);
        }

        /// <summary>
        ///     Async overload for <see cref="Result{TOk,TErr}.Match"/>, producing another result.
        /// </summary>
        /// <inheritdoc cref="Result{TOk,TErr}.Match"/>
        public static async Task<Result<TOkResult, TErrResult>> MatchResultAsync<TOk, TErr, TOkResult, TErrResult>(
            this Task<Result<TOk, TErr>> self,
            Func<TOk, Task<TOkResult>> onOk,
            Func<TErr, Task<TErrResult>> onErr)
        {
            return await (await self).Match(
                async ok => Result<TOkResult, TErrResult>.FromOk(await onOk(ok)),
                async err => Result<TOkResult, TErrResult>.FromErr(await onErr(err)));
        }

        /// <summary>
        ///     Async overload for <see cref="Result{TOk,TErr}.Match"/>, producing another result.
        /// </summary>
        /// <inheritdoc cref="Result{TOk,TErr}.Match"/>
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

        #region UnwrapAsync

        /// <summary>
        ///     Async overload for <see cref="Result{TOk,TErr}.Unwrap()"/>
        /// </summary>
        /// <inheritdoc cref="Result{TOk,TErr}.Unwrap()"/>
        public static async Task<TOk> UnwrapAsync<TOk, TErr>(this Task<Result<TOk, TErr>> self)
        {
            return (await self).Unwrap();
        }

        /// <summary>
        ///     Async overload for <see cref="Result{TOk,TErr}.Unwrap(string)"/>
        /// </summary>
        /// <inheritdoc cref="Result{TOk,TErr}.Unwrap(string)"/>
        public static async Task<TOk> UnwrapAsync<TOk, TErr>(this Task<Result<TOk, TErr>> self, string error)
        {
            return (await self).Unwrap(error);
        }

        /// <summary>
        ///     Async overload for <see cref="Result{TOk,TErr}.Unwrap(Func{TErr, string})"/>
        /// </summary>
        /// <inheritdoc cref="Result{TOk,TErr}.Unwrap(Func{TErr, string})"/>
        public static async Task<TOk> UnwrapAsync<TOk, TErr>(this Task<Result<TOk, TErr>> self, Func<TErr, string> error)
        {
            return (await self).Unwrap(error);
        }

        /// <summary>
        ///     Async overload for <see cref="Result{TOk,TErr}.Unwrap(Func{TErr, string})"/>
        /// </summary>
        /// <inheritdoc cref="Result{TOk,TErr}.Unwrap(Func{TErr, string})"/>
        public static async Task<TOk> UnwrapAsync<TOk, TErr>(this Task<Result<TOk, TErr>> self, Func<TErr, Task<string>> error)
        {
            var res = await self;
            if (res.IsErr)
            {
                if (error is null)
                {
                    throw new ArgumentNullException(nameof(error));
                }

                throw new TriedToUnwrapErrException(await error(res.Err.Unwrap()));
            }

            return res.Unwrap();
        }

        /// <summary>
        ///     Async overload for <see cref="Result{TOk,TErr}.UnwrapOr"/>
        /// </summary>
        /// <inheritdoc cref="Result{TOk,TErr}.UnwrapOr"/>
        public static async Task<TOk> UnwrapOrAsync<TOk, TErr>(this Task<Result<TOk, TErr>> self, TOk fallback)
        {
            return (await self).UnwrapOr(fallback);
        }

        /// <summary>
        ///     Async overload for <see cref="Result{TOk,TErr}.UnwrapOr"/>
        /// </summary>
        /// <inheritdoc cref="Result{TOk,TErr}.UnwrapOr"/>
        public static async Task<TOk> UnwrapOrAsync<TOk, TErr>(this Task<Result<TOk, TErr>> self, Task<TOk> fallback)
        {
            return (await self).UnwrapOr(await fallback);
        }

        /// <summary>
        ///     Async overload for <see cref="Result{TOk,TErr}.UnwrapOrElse"/>
        /// </summary>
        /// <inheritdoc cref="Result{TOk,TErr}.UnwrapOrElse"/>
        public static async Task<TOk> UnwrapOrElseAsync<TOk, TErr>(this Task<Result<TOk, TErr>> self, Func<TOk> fallback)
        {
            return (await self).UnwrapOrElse(fallback);
        }

        /// <summary>
        ///     Async overload for <see cref="Result{TOk,TErr}.UnwrapOrElse"/>
        /// </summary>
        /// <inheritdoc cref="Result{TOk,TErr}.UnwrapOrElse"/>
        public static async Task<TOk> UnwrapOrElseAsync<TOk, TErr>(this Task<Result<TOk, TErr>> self, Func<Task<TOk>> fallback)
        {
            var res = await self;
            if (res.IsOk)
            {
                return res.Unwrap();
            }

            if (fallback is null)
            {
                throw new ArgumentNullException(nameof(fallback));
            }

            return await fallback();
        }

        /// <summary>
        ///     Async overload for <see cref="Result{TOk,TErr}.UnwrapOrDefault"/>
        /// </summary>
        /// <inheritdoc cref="Result{TOk,TErr}.UnwrapOrDefault"/>
        public static async Task<TOk> UnwrapOrDefaultAsync<TOk, TErr>(this Task<Result<TOk, TErr>> self)
        {
            return (await self).UnwrapOrDefault();
        }

        #endregion
    }
}
