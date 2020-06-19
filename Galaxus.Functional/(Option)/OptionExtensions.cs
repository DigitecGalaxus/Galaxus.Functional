using System;
using System.Collections.Generic;
using System.Linq;

namespace Galaxus.Functional
{
    public static class OptionExtensions
    {
        #region Enumeration

        /// <summary>
        /// Returns a subset of <paramref name="self"/> which contains all values in <paramref name="self"/> that contained "Some".
        /// </summary>
        public static IEnumerable<T> SelectSome<T>(this IEnumerable<Option<T>> self)
            => self.Where(v => v.IsSome).Select(v => v.Unwrap());

        /// <summary>
        /// Returns a subset of <paramref name="self"/> which contains all values in <paramref name="self"/> that contained "Some" and runs it through the <paramref name="selector"/>.
        /// </summary>
        public static IEnumerable<TSelection> SelectSome<T, TSelection>(this IEnumerable<Option<T>> self, Func<T, TSelection> selector)
            => self.SelectSome().Select(selector);

        #endregion

        #region C# type system compatibility

        /// <summary>
        /// Transforms an option of a value type into a nullable of the same value type.
        /// </summary>
        public static T? ToNullable<T>(this Option<T> self) where T : struct
            => self.Match(v => v, () => default(T?));

        #endregion

        #region Static Map

        /// <summary>
        /// Automatically maps the contained value to another type.
        /// </summary>
        /// <typeparam name="TFrom">The current type of the options' content. This type must derive from <typeparamref name="TTo"/>.</typeparam>
        /// <typeparam name="TTo">The type to map the option's content to.</typeparam>
        public static Option<TTo> Map<TFrom, TTo>(this Option<TFrom> self) where TFrom : TTo
            => self.Match(v => v.ToOption<TTo>(), () => Option<TTo>.None);

        #endregion

        #region Flatten

        public static Option<T> Flatten<T>(this Option<Option<T>> option)
        {
            return option.UnwrapOr(Option<T>.None);
        }

        public static Option<T> Flatten<T>(this Option<Option<Option<T>>> option)
        {
            return option.UnwrapOr(Option<Option<T>>.None).UnwrapOr(Option<T>.None);
        }

        #endregion

        #region ToOption

        /// <summary>
        /// Wraps a value into an option. Non-null values will result in Option.Some and null values in Option.None. Therefore it is safe to invoke this method
        /// on a "null" reference.
        /// </summary>
        public static Option<T> ToOption<T>(this T self)
            => Option<T>.Some(self);

        /// <summary>
        /// Transforms a nullable value type into an option of the same value type. Non-null values will result in Option.Some and null values in Option.None.
        /// </summary>
        public static Option<T> ToOption<T>(this T? self) where T : struct
            => self?.ToOption() ?? Option<T>.None;

        #endregion

        /// <summary>
        /// Transposes an <see cref="Option{T}"/> containing a <see cref="Result{TOk, TErr}"/> into a <see cref="Result{TOk, TErr}"/> containing an <see cref="Option{T}"/> as its <b>Ok</b> value.
        /// <b>None</b> will be mapped to <b>Ok(None)</b>.
        /// <b>Some(Ok(TOk))</b> will be mapped to <b>Ok(Some(TOk))</b>.
        /// <b>Some(Err(TErr))</b> will be mapped to <b>Err(TErr)</b>.
        /// </summary>
        /// <typeparam name="TOk"></typeparam>
        /// <typeparam name="TErr"></typeparam>
        /// <param name="self"></param>
        /// <returns></returns>
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
    }
}
