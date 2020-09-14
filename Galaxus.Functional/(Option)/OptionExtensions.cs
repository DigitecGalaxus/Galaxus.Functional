using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
        public static IEnumerable<TSelection> SelectSome<T, TSelection>(this IEnumerable<Option<T>> self,
            Func<T, TSelection> selector)
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

        #region UnwrapAsync

        /// <summary>
        /// Unwraps asynchronous <b>self</b> and returns <b>Some</b>.
        /// <i>Throws if <b>self</b> contains <b>None</b>!</i>
        /// </summary>
        /// <param name="self"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns><b>some</b></returns>
        public static async Task<T> UnwrapAsync<T>(this Task<Option<T>> self) => (await self).Unwrap();

        /// <summary>
        /// Unwraps asynchronous <b>self</b> and returns <b>Some</b>.
        /// <i>Throws if <b>self</b> contains <b>None</b>!</i>
        /// </summary>
        /// <param name="self"></param>
        /// <param name="error">
        /// A custom error to use as the exception message.
        /// This argument is eagerly evaluated; if you are passing the result of a function call,
        /// it is recommended to use <see cref="UnwrapAsync{T}(System.Threading.Tasks.Task{Galaxus.Functional.Option{T}})"/>, which is lazily evaluated.</param>
        /// <typeparam name="T"></typeparam>
        /// <returns><b>some</b></returns>
        public static async Task<T> UnwrapAsync<T>(this Task<Option<T>> self, string error) =>
            (await self).Unwrap(error);

        /// <summary>
        /// Unwraps asynchronous <b>self</b> and returns <b>Ok</b>.
        /// <i>Throws if <b>self</b> contains <b>Err</b>!</i>
        /// </summary>
        /// <param name="self"></param>
        /// <param name="error"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns><b>some</b></returns>
        public static async Task<T> UnwrapAsync<T>(this Task<Option<T>> self, Func<string> error) =>
            (await self).Unwrap(error);

        /// <summary>
        /// Unwraps <b>self</b> and returns <b>Some</b> if <b>self</b> contains <b>Some</b>. Returns <paramref name="fallback"/> otherwise.
        /// </summary>
        /// <param name="self"></param>
        /// <param name="fallback">
        /// The value to return if <b>self</b> is <b>Some</b>.
        /// This argument is eagerly evaluated; if you are passing the result of a function call,
        /// it is recommended to use <see cref="UnwrapOrElseAsync{T}(System.Threading.Tasks.Task{Galaxus.Functional.Option{T}},System.Func{T})"/>, which is lazily evaluated.</param>
        public static async Task<T> UnwrapOrAsync<T>(this Task<Option<T>> self, T fallback) =>
            (await self).UnwrapOr(fallback);
        
        /// <summary>
        /// Unwraps <b>self</b> and returns <b>Some</b> if <b>self</b> contains <b>Some</b>. Returns <paramref name="fallback"/> otherwise.
        /// </summary>
        /// <param name="self"></param>
        /// <param name="fallback">
        /// The value to return if <b>self</b> is <b>Some</b>.
        /// This argument is eagerly evaluated; if you are passing the result of a function call,
        /// it is recommended to use <see cref="UnwrapOrElseAsync{T}(System.Threading.Tasks.Task{Galaxus.Functional.Option{T}},System.Func{System.Threading.Tasks.Task{T}})"/>, which is lazily evaluated.</param>
        public static async Task<T> UnwrapOrAsync<T>(this Task<Option<T>> self, Task<T> fallback) =>
            (await self).UnwrapOr(await fallback);

        /// <summary>
        /// Unwraps <b>self</b> and returns <b>Some</b> if <b>self</b> contains <b>Some</b>. Returns the result of <paramref name="fallback"/> otherwise.
        /// </summary>
        /// <param name="self"></param>
        /// <param name="fallback">The function to call if <b>self</b> contains <b>None</b>.</param>
        public static async Task<T> UnwrapOrElseAsync<T>(this Task<Option<T>> self, Func<T> fallback) =>
            (await self).UnwrapOrElse(fallback);

        /// <summary>
        /// Unwraps <b>self</b> and returns <b>Some</b> if <b>self</b> contains <b>Some</b>. Returns the result of <paramref name="fallback"/> otherwise.
        /// </summary>
        /// <param name="self"></param>
        /// <param name="fallback">The function to call if <b>self</b> contains <b>None</b>.</param>
        public static async Task<T> UnwrapOrElseAsync<T>(this Task<Option<T>> self, Func<Task<T>> fallback)
        {
            var opt = await self;
            if (opt.IsSome)
            {
                return opt.Unwrap();
            }
            if (fallback is null)
                throw new ArgumentNullException(nameof(fallback));
            
            return await fallback();
        }
        
        /// <summary>
        /// Unwraps <b>self</b> and returns <b>Some</b> if <b>self</b> contains <b>Some</b>. Returns the default value of <typeparamref name="T"/> otherwise.
        /// </summary>
        /// <param name="self"></param>
        /// <typeparam name="T"></typeparam>
        public static async Task<T> UnwrapOrDefault<T>(this Task<Option<T>> self) => (await self).UnwrapOrDefault();
        
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