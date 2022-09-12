using System;
using System.Collections.Generic;
using System.Linq;

namespace Galaxus.Functional
{
    /// <summary>
    ///     Extensions for the option type, providing some quality of life-shorthands.
    /// </summary>
    public static class OptionExtensions
    {
        #region C# type system compatibility

        /// <summary>
        ///     Transforms an option of a value type into a nullable of the same value type.
        /// </summary>
        public static T? ToNullable<T>(this Option<T> self) where T : struct
        {
            return self.Match(v => v, () => default(T?));
        }

        #endregion

        #region Static Map

        /// <summary>
        ///     Automatically maps the contained value to another type.
        /// </summary>
        /// <typeparam name="TFrom">
        ///     The current type of the options' content. This type must derive from
        ///     <typeparamref name="TTo" />.
        /// </typeparam>
        /// <typeparam name="TTo">The type to map the option's content to.</typeparam>
        public static Option<TTo> Map<TFrom, TTo>(this Option<TFrom> self) where TFrom : TTo
        {
            return self.Match(v => v.ToOption<TTo>(), () => Option<TTo>.None);
        }

        #endregion

        /// <summary>
        ///     Transposes an <see cref="Option{T}" /> containing a <see cref="Result{TOk, TErr}" /> into a
        ///     <see cref="Result{TOk, TErr}" /> containing an <see cref="Option{T}" /> as its <c>Ok</c> value.
        ///     <c>None</c> will be mapped to <c>Ok(None)</c>.
        ///     <c>Some(Ok(TOk))</c> will be mapped to <c>Ok(Some(TOk))</c>.
        ///     <c>Some(Err(TErr))</c> will be mapped to <c>Err(TErr)</c>.
        /// </summary>
        /// <typeparam name="TOk">Ok-type of the resulting <see cref="Result{TOk,TErr}"/></typeparam>
        /// <typeparam name="TErr">Err-type of the resulting <see cref="Result{TOk,TErr}"/></typeparam>
        /// <param name="self">The <see cref="Option{T}"/> to transpose.</param>
        /// <returns>
        ///     <c>None</c>, if <paramref name="self"/> is <c>Ok(None)</c>.<br/>
        ///     <c>Some(Ok)</c>, if <paramref name="self"/> is <c>Ok(Some)</c>.<br/>
        ///     <c>Some(Err)</c>, if <paramref name="self"/> is <c>Err</c>.<br/>
        /// </returns>
        public static Result<Option<TOk>, TErr> Transpose<TOk, TErr>(this Option<Result<TOk, TErr>> self)
        {
            return self.Match(
                result => result.Match(
                    ok => ok.ToOption().ToOk<Option<TOk>, TErr>(),
                    err => err
                ),
                () => Option<TOk>.None.ToOk<Option<TOk>, TErr>()
            );
        }

        #region Enumeration

        /// <summary>
        ///     Returns a subset of <paramref name="self" /> which contains all values in <paramref name="self" /> that contained
        ///     "Some".
        /// </summary>
        public static IEnumerable<T> SelectSome<T>(this IEnumerable<Option<T>> self)
        {
            return self.Where(v => v.IsSome).Select(v => v.Unwrap());
        }

        /// <summary>
        ///     Returns a subset of <paramref name="self" /> which contains all values in <paramref name="self" /> that contained
        ///     "Some" and runs it through the <paramref name="selector" />.
        /// </summary>
        public static IEnumerable<TSelection> SelectSome<T, TSelection>(this IEnumerable<Option<T>> self,
            Func<T, TSelection> selector)
        {
            return self.SelectSome().Select(selector);
        }

        #endregion

        #region Flatten

        /// <summary>
        ///     Flatten an option containing an option into a single option.
        /// </summary>
        /// <param name="option">The option to flatten.</param>
        /// <typeparam name="T">Type contained in the option.</typeparam>
        /// <returns>
        ///     <see cref="Option{T}" /> containing <c>Some</c> if both the outer as well as the inner option are <c>Some</c>;
        ///     otherwise, returns <see cref="None" />
        /// </returns>
        public static Option<T> Flatten<T>(this Option<Option<T>> option)
        {
            return option.UnwrapOr(Option<T>.None);
        }

        /// <summary>
        ///     Flatten an option containing an option within an option into a single option.
        /// </summary>
        /// <param name="option">The option to flatten.</param>
        /// <typeparam name="T">Type contained in the option.</typeparam>
        /// <returns>
        ///     <see cref="Option{T}" /> containing <c>Some</c> if all layers in the given option are <c>Some</c>;
        ///     otherwise, returns <see cref="None" />
        /// </returns>
        public static Option<T> Flatten<T>(this Option<Option<Option<T>>> option)
        {
            return option.UnwrapOr(Option<Option<T>>.None).UnwrapOr(Option<T>.None);
        }

        #endregion

        #region ToOption

        /// <summary>
        ///     Wraps a value into an option. Non-null values will result in Option.Some and null values in Option.None. Therefore
        ///     it is safe to invoke this method
        ///     on a "null" reference.
        /// </summary>
        public static Option<T> ToOption<T>(this T self)
        {
            return Option<T>.Some(self);
        }

        /// <summary>
        ///     Transforms a nullable value type into an option of the same value type. Non-null values will result in Option.Some
        ///     and null values in Option.None.
        /// </summary>
        public static Option<T> ToOption<T>(this T? self) where T : struct
        {
            return self?.ToOption() ?? Option<T>.None;
        }

        #endregion
    }
}
