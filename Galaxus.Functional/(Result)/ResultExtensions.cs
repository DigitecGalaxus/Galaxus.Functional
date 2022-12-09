using System;
using System.Collections.Generic;
using System.Linq;

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
    }
}
