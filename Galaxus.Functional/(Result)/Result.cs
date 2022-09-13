using System;
using System.Collections.Generic;

namespace Galaxus.Functional
{
    /// <summary>
    ///     <see cref="Result{TOk, TErr}" />s are used for returning and propagating errors.
    ///     They're either <b>Ok</b>, representing success and containing a value,
    ///     or <b>Err</b>, representing failure and containing an error value.
    ///     Functions return <see cref="Result{TOk, TErr}" />s whenever errors are expected and recoverable.
    /// </summary>
    public sealed partial class Result<TOk, TErr> : IEquatable<Result<TOk, TErr>>
    {
        #region Instance Initializer

        /// <summary>
        ///     Constructs a <see cref="Result{TOk, TErr}" /> containing <b>Ok</b>.
        /// </summary>
        /// <param name="ok">The <b>Ok</b> value.</param>
        public static Result<TOk, TErr> FromOk(TOk ok)
        {
            return new Result<TOk, TErr>(ok);
        }

        /// <summary>
        ///     Constructs a <see cref="Result{TOk, TErr}" /> containing <b>Err</b>.
        /// </summary>
        /// <param name="err">The <b>Err</b> value.</param>
        public static Result<TOk, TErr> FromErr(TErr err)
        {
            return new Result<TOk, TErr>(err);
        }

        // Note:
        // We use private ctors here to allow TOk and TErr to be of the same type.
        // If we would use public ctors the compiler wouldn't know which one to invoke
        // when TOk and TErr refer to the same type. See FromOk and FromErr.

        // Note:
        // We allow the "null" value for nullable value types
        // because accessing a "null" value of a value type does not cause a NullReferenceException.

        private Result(TOk ok)
        {
            if (typeof(TOk).IsValueType == false && ok == null)
            {
                throw new ArgumentNullException(nameof(ok));
            }

            _ok = ok;
            IsOk = true;
        }

        private Result(TErr err)
        {
            if (typeof(TErr).IsValueType == false && err == null)
            {
                throw new ArgumentNullException(nameof(err));
            }

            _err = err;
        }

        /// <summary>
        ///     Cast a value to a <see cref="Result{TOk,TErr}" /> containing Ok.
        /// </summary>
        /// <param name="ok">The value to be contained.</param>
        /// <returns>The result.</returns>
        public static implicit operator Result<TOk, TErr>(TOk ok)
        {
            return new Result<TOk, TErr>(ok);
        }

        /// <summary>
        ///     Cast a value to a <see cref="Result{TOk,TErr}" /> containing Err.
        /// </summary>
        /// <param name="err">The value to be contained.</param>
        /// <returns>The result.</returns>
        public static implicit operator Result<TOk, TErr>(TErr err)
        {
            return new Result<TOk, TErr>(err);
        }

        #endregion

        #region State

        private readonly TOk _ok;
        private readonly TErr _err;

        /// <summary>
        ///     Returns <b>true</b> if <b>self</b> contains <b>Ok</b>.
        /// </summary>
        public bool IsOk { get; }

        /// <summary>
        ///     Returns <b>true</b> if <b>self</b> contains <b>Err</b>.
        /// </summary>
        public bool IsErr
            => IsOk == false;

        /// <summary>
        ///     Discards <b>Err</b> and returns <b>Ok</b>.
        /// </summary>
        public Option<TOk> Ok
            => IsOk ? _ok.ToOption() : None.Value;

        /// <summary>
        ///     Discards <b>Ok</b> and returns <b>Err</b>.
        /// </summary>
        public Option<TErr> Err
            => IsOk ? None.Value : _err.ToOption();

        #endregion

        #region Unwrap

        /// <summary>
        ///     Unwraps <b>self</b> and returns <b>Ok</b>.
        ///     <i>Throws if <b>self</b> contains <b>Err</b>!</i>
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
        ///     Unwraps <b>self</b> and returns <b>Ok</b>.
        ///     <i>Throws if <b>self</b> contains <b>Err</b>!</i>
        /// </summary>
        /// <param name="error">
        ///     A custom error to use as the exception message.
        ///     This argument is eagerly evaluated; if you are passing the result of a function call,
        ///     it is recommended to use <see cref="Unwrap(Func{TErr, string})" />, which is lazily evaluated.
        /// </param>
        public TOk Unwrap(string error)
        {
            if (IsErr)
            {
                throw new TriedToUnwrapErrException(error);
            }

            return _ok;
        }

        /// <summary>
        ///     Unwraps <b>self</b> and returns <b>Ok</b>.
        ///     <i>Throws if <b>self</b> contains <b>Err</b>!</i>
        /// </summary>
        /// <param name="error">A function that returns a custom error to use as the exception message.</param>
        public TOk Unwrap(Func<TErr, string> error)
        {
            if (IsErr)
            {
                if (error is null)
                {
                    throw new ArgumentNullException(nameof(error));
                }

                throw new TriedToUnwrapErrException(error(_err));
            }

            return _ok;
        }

        /// <summary>
        ///     Unwraps <b>self</b> and returns <b>Ok</b> if <b>self</b> contains <b>Ok</b>. Returns <paramref name="fallback" />
        ///     otherwise.
        /// </summary>
        /// <param name="fallback">
        ///     The value to return if <b>self</b> contains <b>Err</b>.
        ///     This argument is eagerly evaluated; if you are passing the result of a function call,
        ///     it is recommended to use <see cref="UnwrapOrElse(Func{TOk})" />, which is lazily evaluated.
        /// </param>
        public TOk UnwrapOr(TOk fallback)
        {
            return IsOk ? _ok : fallback;
        }

        /// <summary>
        ///     Unwraps <b>self</b> and returns <b>Ok</b> if <b>self</b> contains <b>Ok</b>. Returns the result of
        ///     <paramref name="fallback" /> otherwise.
        /// </summary>
        /// <param name="fallback">The function to call if <b>self</b> contains <b>Err</b>.</param>
        public TOk UnwrapOrElse(Func<TOk> fallback)
        {
            if (IsOk)
            {
                return _ok;
            }

            if (fallback is null)
            {
                throw new ArgumentNullException(nameof(fallback));
            }

            return fallback();
        }

        #endregion

        #region Contains

        /// <summary>
        ///     Returns true if the the result is <b>Ok</b> and contains the given value.
        /// </summary>
        /// <param name="value">The value to check against.</param>
        public bool Contains(TOk value)
        {
            return Match(ok => ok.Equals(value), err => false);
        }

        /// <summary>
        ///     Returns true if the the result is <b>Err</b> and contains the given value.
        /// </summary>
        /// <param name="value">The value to check against.</param>
        public bool ContainsErr(TErr value)
        {
            return Match(ok => false, err => err.Equals(value));
        }

        #endregion

        #region Equals, GetHashCode & ToString

        /// <inheritdoc />
        public override bool Equals(object other)
        {
            return Equals(other as Result<TOk, TErr>);
        }

        /// <inheritdoc />
        public bool Equals(Result<TOk, TErr> other)
        {
            if (other is null)
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            if (IsOk)
            {
                return other.IsOk && _ok.Equals(other._ok);
            }

            return !other.IsOk && _err.Equals(other._err);
        }

        /// <summary>
        ///     Check that two results are equal.
        /// </summary>
        /// <param name="lhs">Result to check against.</param>
        /// <param name="rhs">Result to check.</param>
        /// <returns><c>True</c> if the results are equal, <c>true</c> otherwise.</returns>
        public static bool operator ==(Result<TOk, TErr> lhs, Result<TOk, TErr> rhs)
        {
            if (lhs is null)
            {
                return rhs is null;
            }

            return lhs.Equals(rhs);
        }

        /// <summary>
        ///     Check that two results are not equal.
        /// </summary>
        /// <param name="lhs">Result to check against.</param>
        /// <param name="rhs">Result to check.</param>
        /// <returns><c>False</c> if the results are equal, <c>true</c> otherwise.</returns>
        public static bool operator !=(Result<TOk, TErr> lhs, Result<TOk, TErr> rhs)
        {
            return lhs == rhs == false;
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            return (IsOk, _ok, _err).GetHashCode();
        }

        /// <inheritdoc />
        public override string ToString()
        {
            return Match(ok => $"Ok: {ok.ToString()}", err => $"Err: {err.ToString()}");
        }

        #endregion
    }
}
