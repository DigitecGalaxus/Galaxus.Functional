using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Galaxus.Functional
{
    /// <summary>
    ///     <see cref="Result{TOk, TErr}"/>s are used for returning and propagating errors.
    ///     They're either <b>Ok</b>, representing success and containing a value,
    ///     or <b>Err</b>, representing failure and containing an error value.
    ///     Functions return <see cref="Result{TOk, TErr}"/>s whenever errors are expected and recoverable.
    /// </summary>
    public class Result<TOk, TErr> : IEquatable<Result<TOk, TErr>>
    {
        #region Type Initializer

        private static readonly bool _tOkIsValueType;
        private static readonly bool _tErrIsValueType;

        static Result()
        {
            _tOkIsValueType = typeof(TOk).IsValueType;
            _tErrIsValueType = typeof(TErr).IsValueType;
        }

        #endregion

        #region Instance Initializer

        /// <summary>
        /// Constructs a <see cref="Result{TOk, TErr}"/> containing <b>Ok</b>.
        /// </summary>
        /// <param name="ok">The <b>Ok</b> value.</param>
        public static Result<TOk, TErr> FromOk(TOk ok)
            => new Result<TOk, TErr>(ok);

        /// <summary>
        /// Constructs a <see cref="Result{TOk, TErr}"/> containing <b>Err</b>.
        /// </summary>
        /// <param name="err">The <b>Err</b> value.</param>
        public static Result<TOk, TErr> FromErr(TErr err)
            => new Result<TOk, TErr>(err);

        // Note:
        // We use private ctors here to allow TOk and TErr to be of the same type.
        // If we would use public ctors the compiler wouldn't know which one to invoke
        // when TOk and TErr refer to the same type. See FromOk and FromErr.

        // Note:
        // We allow the "null" value for nullable value types
        // because accessing a "null" value of a value type does not cause a NullReferenceException.

        private Result(TOk ok)
        {
            if (_tOkIsValueType == false && ok == null)
                throw new ArgumentNullException(nameof(ok));

            _ok = ok;
            IsOk = true;
        }

        private Result(TErr err)
        {
            if (_tErrIsValueType == false && err == null)
                throw new ArgumentNullException(nameof(err));

            _err = err;
        }

        public static implicit operator Result<TOk, TErr>(TOk ok)
            => new Result<TOk, TErr>(ok);

        public static implicit operator Result<TOk, TErr>(TErr err)
            => new Result<TOk, TErr>(err);

        #endregion

        #region State

        private readonly TOk _ok;
        private readonly TErr _err;

        /// <summary>
        /// Returns <b>true</b> if <b>self</b> contains <b>Ok</b>.
        /// </summary>
        public bool IsOk { get; }

        /// <summary>
        /// Returns <b>true</b> if <b>self</b> contains <b>Err</b>.
        /// </summary>
        public bool IsErr
            => IsOk == false;

        /// <summary>
        /// Discards <b>Err</b> and returns <b>Ok</b>.
        /// </summary>
        public Option<TOk> Ok
            => IsOk ? _ok.ToOption() : None.Value;

        /// <summary>
        /// Discards <b>Ok</b> and returns <b>Err</b>.
        /// </summary>
        public Option<TErr> Err
            => IsOk ? None.Value : _err.ToOption();

        #endregion

        #region Match

        /// <summary>
        /// Provides access to <b>self</b>'s content by calling <paramref name="onOk"/> and passing in <b>Ok</b>
        /// or calling <paramref name="onErr"/> and passing in <b>Err</b>.
        /// </summary>
        /// <param name="onOk">Called when <b>self</b> contains <b>Ok</b>. The argument to this action is never the <b>null</b> reference.</param>
        /// <param name="onErr">Called when <b>self</b> contains <b>Err</b>. The argument to this action is never the <b>null</b> reference.</param>
        public void Match(Action<TOk> onOk, Action<TErr> onErr)
        {
            if (IsOk)
            {
                if (onOk is null)
                    throw new ArgumentNullException(nameof(onOk));

                onOk(_ok);
            }
            else
            {
                if (onErr is null)
                    throw new ArgumentNullException(nameof(onErr));

                onErr(_err);
            }
        }

        /// <summary>
        /// Provides access to <b>self</b>'s content by calling <paramref name="onOk"/> and passing in <b>Ok</b>
        /// or calling <paramref name="onErr"/> and passing in <b>Err</b>.
        /// </summary>
        /// <param name="onOk">Called when <b>self</b> contains <b>Ok</b>. The argument to this function is never the <b>null</b> reference.</param>
        /// <param name="onErr">Called when <b>self</b> contains <b>Err</b>. The argument to this function is never the <b>null</b> reference.</param>
        public T Match<T>(Func<TOk, T> onOk, Func<TErr, T> onErr)
        {
            if (IsOk)
            {
                if (onOk is null)
                    throw new ArgumentNullException(nameof(onOk));

                return onOk(_ok);
            }

            if (onErr is null)
                throw new ArgumentNullException(nameof(onErr));

            return onErr(_err);
        }

        public Task<TResult> MatchAsync<TResult>(Func<TOk, Task<TResult>> onOk, Func<TErr, Task<TResult>> onErr)
        {
            return Match(
                async ok => await onOk(ok),
                async err => await onErr(err));
        }

        public Task<TResult> MatchAsync<TResult>(Func<TOk, Task<TResult>> onOk, Func<TErr, TResult> onErr)
        {
            return Match(
                async ok => await onOk(ok),
                err => Task.FromResult(onErr(err)));
        }

        public Task<TResult> MatchAsync<TResult>(Func<TOk, TResult> onOk, Func<TErr, Task<TResult>> onErr)
        {
            return Match(
                ok => Task.FromResult(onOk(ok)),
                async err => await onErr(err));
        }

        #endregion

        #region If

        /// <summary>
        /// Provides access to <b>self</b>'s <b>Ok</b> value by calling <paramref name="onOk"/> if <b>self</b> contains <b>Ok</b>.
        /// </summary>
        /// <param name="onOk">Called when <b>self</b> contains <b>Ok</b>. The argument to this action is never the <b>null</b> reference.</param>
        public void IfOk(Action<TOk> onOk)
        {
            if (IsOk)
            {
                if (onOk is null)
                    throw new ArgumentNullException(nameof(onOk));

                onOk(_ok);
            }
        }

        /// <summary>
        /// Provides access to <b>self</b>'s <b>Ok</b> value by calling <paramref name="onOk"/> if <b>self</b> contains <b>Ok</b>.
        /// </summary>
        /// <param name="onOk">Called when <b>self</b> contains <b>Ok</b>. The argument to this function is never the <b>null</b> reference.</param>
        public Task IfOkAsync(Func<TOk, Task> onOk)
        {
            if (IsOk)
            {
                if (onOk is null)
                    throw new ArgumentNullException(nameof(onOk));

                return onOk(_ok);
            }

            return Task.CompletedTask;
        }

        /// <summary>
        /// Provides access to <b>self</b>'s <b>Err</b> value by calling <paramref name="onErr"/> if <b>self</b> contains <b>Err</b>.
        /// </summary>
        /// <param name="onErr">Called when <b>self</b> contains <b>Err</b>. The argument to this action is never the <b>null</b> reference.</param>
        public void IfErr(Action<TErr> onErr)
        {
            if (IsErr)
            {
                if (onErr is null)
                    throw new ArgumentNullException(nameof(onErr));

                onErr(_err);
            }
        }

        /// <summary>
        /// Provides access to <b>self</b>'s <b>Err</b> value by calling <paramref name="onErr"/> if <b>self</b> contains <b>Err</b>.
        /// </summary>
        /// <param name="onErr">Called when <b>self</b> contains <b>Err</b>. The argument to this function is never the <b>null</b> reference.</param>
        public Task IfErrAsync(Func<TErr, Task> onErr)
        {
            if (IsErr)
            {
                if (onErr is null)
                    throw new ArgumentNullException(nameof(onErr));

                return onErr(_err);
            }

            return Task.CompletedTask;
        }

        #endregion

        #region And

        /// <summary>
        /// Returns <paramref name="result"/> if <b>self</b> contains <b>Ok</b>, otherwise returns the <b>Err</b> value contained in <b>self</b>.
        /// </summary>
        /// <typeparam name="TNewOk">The <b>Ok</b> type of <paramref name="result"/>.</typeparam>
        /// <param name="result">The result to return if <b>self</b> is <b>Ok</b>.</param>
        public Result<TNewOk, TErr> And<TNewOk>(Result<TNewOk, TErr> result)
        {
            return Match(
                ok =>
                {
                    if (result is null)
                        throw new ArgumentNullException(nameof(result));

                    return result;
                },
                err => err
            );
        }

        /// <summary>
        /// Calls <paramref name="continuation"/> if <b>self</b> contains <b>Ok</b>, otherwise returns the <b>Err</b> value contained in <b>self</b>.
        /// This function can be used for control flow based on <see cref="Result{TOk, TErr}"/>s.
        /// </summary>
        /// <typeparam name="TContinuationOk">The <b>Ok</b> type of the <paramref name="continuation"/>'s <see cref="Result{TOk, TErr}"/>.</typeparam>
        /// <param name="continuation">The function to call if <b>self</b> contains <b>Ok</b>.</param>
        public Result<TContinuationOk, TErr> AndThen<TContinuationOk>(Func<TOk, Result<TContinuationOk, TErr>> continuation)
        {
            return Match(
                ok =>
                {
                    if (continuation is null)
                        throw new ArgumentNullException(nameof(continuation));

                    return continuation(ok);
                },
                err => err
            );
        }

        /// <summary>
        /// Calls <paramref name="continuation"/> if <b>self</b> contains <b>Ok</b>, otherwise returns the <b>Err</b> value contained in <b>self</b>.
        /// This function can be used for control flow based on <see cref="Result{TOk, TErr}"/>s.
        /// </summary>
        /// <param name="continuation">The function to call if <b>self</b> is <b>Ok</b>.</param>
        public Result<TOk, TErr> AndThen(Action<TOk> continuation)
        {
            IfOk(continuation);
            return this;
        }

        /// <summary>
        /// Calls <paramref name="continuation"/> if <b>self</b> contains <b>Ok</b>, otherwise returns the <b>Err</b> value contained in <b>self</b>.
        /// This function can be used for control flow based on <see cref="Result{TOk, TErr}"/>s.
        /// </summary>
        /// <typeparam name="TContinuationOk">The <b>Ok</b> type of the <paramref name="continuation"/>'s <see cref="Result{TOk, TErr}"/>.</typeparam>
        /// <param name="continuation">The function to call if <b>self</b> contains <b>Ok</b>.</param>
        public Task<Result<TContinuationOk, TErr>> AndThenAsync<TContinuationOk>(Func<TOk, Task<Result<TContinuationOk, TErr>>> continuation)
        {
            return Match(
                continuation,
                err => Task.FromResult(err.ToErr<TContinuationOk, TErr>()));
        }

        #endregion

        #region Or

        /// <summary>
        /// Returns <paramref name="fallback"/> if <b>self</b> contains <b>Err</b>, otherwise returns the <b>Ok</b> value contained in <b>self</b>.
        /// </summary>
        /// <param name="fallback">
        /// The result to return if <b>self</b> contains <b>Err</b>.
        /// This argument is eagerly evaluated; if you are passing the result of a function call,
        /// it is recommended to use <see cref="OrElse{TContinuationErr}(Func{TErr, Result{TOk, TContinuationErr}})"/>, which is lazily evaluated.
        /// </param>
        public Result<TOk, TContinuationErr> Or<TContinuationErr>(Result<TOk, TContinuationErr> fallback)
            => IsOk ? _ok : fallback;

        /// <summary>
        /// Calls <paramref name="continuation"/> if <b>self</b> contains <b>Err</b>, otherwise returns the <b>Ok</b> value contained in <b>self</b>.
        /// This function can be used for control flow based on <see cref="Result{TOk, TErr}"/>s.
        /// </summary>
        /// <param name="continuation">The function to call if <b>self</b> contains <b>Err</b>.</param>
        public Result<TOk, TContinuationErr> OrElse<TContinuationErr>(Func<TErr, Result<TOk, TContinuationErr>> continuation)
        {
            if (IsOk)
                return _ok;

            if (continuation is null)
                throw new ArgumentNullException(nameof(continuation));

            return continuation(_err);
        }


        /// <summary>
        /// Calls <paramref name="continuation"/> if <b>self</b> contains <b>Err</b>, otherwise returns the <b>Ok</b> value contained in <b>self</b>.
        /// This function can be used for control flow based on <see cref="Result{TOk, TErr}"/>s.
        /// </summary>
        /// <typeparam name="TContinuationErr">The <b>Error</b> type of the <paramref name="continuation"/>'s <see cref="Result{TOk, TErr}"/>.</typeparam>
        /// <param name="continuation">The function to call if <b>self</b> contains <b>Err</b>.</param>
        public Task<Result<TOk, TContinuationErr>> OrElseAsync<TContinuationErr>(Func<TErr, Task<Result<TOk, TContinuationErr>>> continuation)
        {
            if (IsOk)
                return Task.FromResult(_ok.ToOk<TOk, TContinuationErr>());

            if (continuation is null)
                throw new ArgumentNullException(nameof(continuation));

            return continuation(_err);
        }

        #endregion

        #region Unwrap

        /// <summary>
        /// Unwraps <b>self</b> and returns <b>Ok</b>.
        /// <i>Throws if <b>self</b> contains <b>Err</b>!</i>
        /// </summary>
        public TOk Unwrap()
        {
            return Unwrap(err =>
            {
                switch (err)
                {
                    case IError error:
                        return error.Description;
                    case IEnumerable<IError> errors:
                        return string.Join("; ", errors);
                    case string str:
                        return str;
                    case IEnumerable<string> strs:
                        return string.Join("; ", strs);
                    default:
                        return $"Cannot unwrap \"Ok\" when the result is \"Err\": {_err.ToString()}.";
                }
            });
        }

        /// <summary>
        /// Unwraps <b>self</b> and returns <b>Ok</b>.
        /// <i>Throws if <b>self</b> contains <b>Err</b>!</i>
        /// </summary>
        /// <param name="error">
        /// A custom error to use as the exception message.
        /// This argument is eagerly evaluated; if you are passing the result of a function call,
        /// it is recommended to use <see cref="Unwrap(Func{TErr, string})"/>, which is lazily evaluated.
        /// </param>
        public TOk Unwrap(string error)
        {
            if (IsErr)
                throw new AttemptToUnwrapErrWhenResultWasOkException(error);

            return _ok;
        }

        /// <summary>
        /// Unwraps <b>self</b> and returns <b>Ok</b>.
        /// <i>Throws if <b>self</b> contains <b>Err</b>!</i>
        /// </summary>
        /// <param name="error">A function that returns a custom error to use as the exception message.</param>
        public TOk Unwrap(Func<TErr, string> error)
        {
            if (IsErr)
            {
                if (error is null)
                    throw new ArgumentNullException(nameof(error));

                throw new AttemptToUnwrapErrWhenResultWasOkException(error(_err));
            }

            return _ok;
        }

        /// <summary>
        /// Unwraps <b>self</b> and returns <b>Ok</b> if <b>self</b> contains <b>Ok</b>. Returns <paramref name="fallback"/> otherwise.
        /// </summary>
        /// <param name="fallback">
        /// The value to return if <b>self</b> contains <b>Err</b>.
        /// This argument is eagerly evaluated; if you are passing the result of a function call,
        /// it is recommended to use <see cref="UnwrapOrElse(Func{TOk})"/>, which is lazily evaluated.
        /// </param>
        public TOk UnwrapOr(TOk fallback)
            => IsOk ? _ok : fallback;

        /// <summary>
        /// Unwraps <b>self</b> and returns <b>Ok</b> if <b>self</b> contains <b>Ok</b>. Returns the result of <paramref name="fallback"/> otherwise.
        /// </summary>
        /// <param name="fallback">The function to call if <b>self</b> contains <b>Err</b>.</param>
        public TOk UnwrapOrElse(Func<TOk> fallback)
        {
            if (IsOk)
                return _ok;

            if (fallback is null)
                throw new ArgumentNullException(nameof(fallback));

            return fallback();
        }

        /// <summary>
        /// Unwraps <b>self</b> and returns <b>Ok</b> if <b>self</b> contains <b>Ok</b>. Returns the default value of <typeparamref name="TOk"/> otherwise.
        /// </summary>
        public TOk UnwrapOrDefault()
            => IsOk ? _ok : default;

        #endregion

        #region Map

        /// <summary>
        /// Maps a <see cref="Result{TOk, TErr}"/> to <see cref="Result{TOkTo, TErr}"/> by applying a function to a contained <b>Ok</b> value,
        /// leaving an <b>Err</b> value untouched. This function can be used to compose the results of two functions.
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

                    return map(ok);
                },
                err => err.ToErr<TOkTo, TErr>()
            );
        }

        /// <summary>
        /// Maps a <see cref="Result{TOk, TErr}"/> to <see cref="Result{TOkTo, TErr}"/> by applying a function to a contained <b>Ok</b> value,
        /// leaving an <b>Err</b> value untouched. This function can be used to compose the results of two functions.
        /// </summary>
        /// <typeparam name="TOkTo">The type to map <b>Ok</b> to.</typeparam>
        /// <param name="continuation">The mapping function.</param>
        public Task<Result<TOkTo, TErr>> MapAsync<TOkTo>(Func<TOk, Task<TOkTo>> continuation)
        {
            return MatchAsync(
                async ok => new Result<TOkTo, TErr>(await continuation(ok)),
                err => err.ToErr<TOkTo, TErr>());
        }

        /// <summary>
        /// Maps a <see cref="Result{TOk, TErr}"/> to <see cref="Result{TOk, TErrTo}"/> by applying a function to a contained <b>Err</b> value,
        /// leaving an <b>Ok</b> value untouched. This function can be used to compose the results of two functions.
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

                    return map(err);
                }
            );
        }

        /// <summary>
        /// Maps a <see cref="Result{TOk, TErr}"/> to <see cref="Result{TOk, TErrTo}"/> by applying a function to a contained <b>Err</b> value,
        /// leaving an <b>Ok</b> value untouched. This function can be used to compose the results of two functions.
        /// </summary>
        /// <typeparam name="TErrTo">The type to map "Err" to.</typeparam>
        /// <param name="continuation">The mapping function.</param>
        public Task<Result<TOk, TErrTo>> MapErrAsync<TErrTo>(Func<TErr, Task<TErrTo>> continuation)
        {
            return MatchAsync(
                ok => ok.ToOk<TOk, TErrTo>(),
                async err => new Result<TOk, TErrTo>(await continuation(err)));
        }

        #endregion

        #region Contains

        /// <summary>
        /// Returns true if the the result is <b>Ok</b> and contains the given value.
        /// </summary>
        /// <param name="value">The value to check against.</param>
        public bool Contains(TOk value)
        {
            return Match(ok => ok.Equals(value), err => false);
        }

        /// <summary>
        /// Returns true if the the result is <b>Err</b> and contains the given value.
        /// </summary>
        /// <param name="value">The value to check against.</param>
        public bool ContainsErr(TErr value)
        {
            return Match(ok => false, err => err.Equals(value));
        }

        #endregion

        #region Equals, GetHashCode & ToString

        public sealed override bool Equals(object other)
            => Equals(other as Result<TOk, TErr>);

        public bool Equals(Result<TOk, TErr> other)
        {
            if (other is null)
                return false;

            if (ReferenceEquals(this, other))
                return true;

            if (IsOk)
            {
                return
                    other.IsOk
                    ? _ok.Equals(other._ok)
                    : false;
            }

            return
                other.IsOk
                ? false
                : _err.Equals(other._err);
        }

        public static bool operator ==(Result<TOk, TErr> lhs, Result<TOk, TErr> rhs)
        {
            if (lhs is null)
                return rhs is null;

            return lhs.Equals(rhs);
        }

        public static bool operator !=(Result<TOk, TErr> lhs, Result<TOk, TErr> rhs)
            => (lhs == rhs) == false;

        public override int GetHashCode()
            => (IsOk, _ok, _err).GetHashCode();

        public override string ToString()
            => Match(ok => $"Ok: {ok.ToString()}", err => $"Err: {err.ToString()}");

        #endregion
    }
}
