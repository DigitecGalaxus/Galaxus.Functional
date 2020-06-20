using System;

namespace Galaxus.Functional
{
    /// <summary>
    /// Type <see cref="Option{T}"/> represents an optional value: every <see cref="Option{T}"/> is either <b>Some</b> and contains a value, or <b>None</b>, and does not.
    ///
    /// Option types are very common, as they have a number of uses:
    /// 1) Initial values,
    /// 2) Return values for functions that are not defined over their entire input range (partial functions),
    /// 3) Return value for otherwise reporting simple errors, where <b>None</b> is returned on error,
    /// 4) Optional fields,
    /// 5) Optional function arguments,
    /// 6) Nullable References.
    /// </summary>
    public readonly partial struct Option<T> : IOption, IEquatable<Option<T>>
    {
        #region Type Initializer

        private static readonly bool _tIsValueType;

        static Option()
        {
            _tIsValueType = typeof(T).IsValueType;
        }

        #endregion

        #region Instance Initializer

        // Note:
        // We allow "null" instances for nullable value types.
        // Because accessing a "null" instance of a value type does not cause a NullReferenceException.
        // Using an Option<bool?> and not being able to use "null" would be weird.
        // Although using an Option<T?> should probably not be done anyway.
        // (But it cannot be prevented at compile time)

        private Option(T some)
        {
            _some = some;
            IsSome = _tIsValueType || some != null;
        }

        /// <summary>
        /// Constructs an <see cref="Option{T}"/> containing <b>None</b>.
        /// </summary>
        public static Option<T> None
            => default;

        /// <summary>
        /// Constructs an <see cref="Option{T}"/> containing <b>Some</b>.
        /// Except if <paramref name="some"/> is the <b>null</b> reference, then the <see cref="Option{T}"/> will contain <b>None</b>.
        /// </summary>
        /// <param name="some">The value to use as <b>Some</b>.</param>
        public static Option<T> Some(T some)
            => new Option<T>(some);

        public static implicit operator Option<T>(None none)
            => None;

        // Note on implicit conversion:
        // An implicit operator from T to Option<T> is not implemented because we tried that and it caused unexpected runtime issues.
        // The next line (which uses implicit conversion) would create a runtime type of Option<Option<bool>> which in almost all cases is not the intended behaviour:
        // Option<object> obj = true.ToOption(); // The variable obj has a runtime type of Option<Option<bool>>

        #endregion

        #region State

        private readonly T _some;

        /// <summary>
        /// True if the option contains "Some".
        /// </summary>
        public bool IsSome { get; }

        /// <summary>
        /// True if the option contains "None".
        /// </summary>
        public bool IsNone
            => IsSome == false;

        #endregion

        #region Ok

        /// <summary>
        /// Transforms <b>self</b> into a <see cref="Result{T,TErr}"/>, mapping <b>Some(v)</b> to <b>Ok(v)</b> and <b>None</b> to <b>Err(err)</b>.
        /// </summary>
        /// <param name="err">
        /// The value to return as <b>Err</b> if <b>self</b> contains <b>None</b>.
        /// This argument is eagerly evaluated; if you are passing the result of a function call,
        /// it is recommended to use <see cref="OkOrElse{TErr}(Func{TErr})"/>, which is lazily evaluated.
        /// </param>
        public Result<T, TErr> OkOr<TErr>(TErr err)
            => IsSome ? Result<T, TErr>.FromOk(_some) : Result<T, TErr>.FromErr(err);

        /// <summary>
        /// Transforms <b>self</b> into a <see cref="Result{TOk, TErr}"/>, mapping <b>Some(v)</b> to <b>Ok(v)</b> and <b>None</b> to <b>Err(err)</b>.
        /// </summary>
        /// <param name="fallback">The value to return as <b>Err</b> if <b>self</b> contains <b>None</b>.</param>
        public Result<T, TErr> OkOrElse<TErr>(Func<TErr> fallback)
        {
            if (IsSome)
                return Result<T, TErr>.FromOk(_some);

            if (fallback is null)
                throw new ArgumentNullException(nameof(fallback));

            return Result<T, TErr>.FromErr(fallback());
        }

        #endregion

        #region Unwrap

        /// <summary>
        /// Unwraps <b>self</b> and returns <b>Some</b>.
        /// <i>Throws if <b>self</b> contains <b>None</b>!</i>
        /// </summary>
        public T Unwrap()
            => Unwrap(() => $"Cannot unwrap \"None\" of type {typeof(T)}.");

        /// <summary>
        /// Unwraps <b>self</b> and returns <b>Some</b>.
        /// <i>Throws if <b>self</b> contains <b>None</b>!</i>
        /// </summary>
        /// <param name="error">
        /// A custom error to use as the exception message.
        /// This argument is eagerly evaluated; if you are passing the result of a function call,
        /// it is recommended to use <see cref="Unwrap(Func{string})"/>, which is lazily evaluated.</param>
        public T Unwrap(string error)
        {
            if (IsSome == false)
                throw new AttemptToUnwrapNoneWhenOptionContainedSomeException(error);

            return _some;
        }

        /// <summary>
        /// Unwraps <b>self</b> and returns <b>Ok</b>.
        /// <i>Throws if <b>self</b> contains <b>Err</b>!</i>
        /// </summary>
        /// <param name="error">A function that returns a custom error to use as the exception message.</param>
        public T Unwrap(Func<string> error)
        {
            if (IsSome == false)
            {
                if (error is null)
                    throw new ArgumentNullException(nameof(error));

                throw new AttemptToUnwrapNoneWhenOptionContainedSomeException(error());
            }

            return _some;
        }

        /// <summary>
        /// Unwraps <b>self</b> and returns <b>Some</b> if <b>self</b> contains <b>Some</b>. Returns <paramref name="fallback"/> otherwise.
        /// </summary>
        /// <param name="fallback">
        /// The value to return if <b>self</b> is <b>Some</b>.
        /// This argument is eagerly evaluated; if you are passing the result of a function call,
        /// it is recommended to use <see cref="UnwrapOrElse(Func{T})"/>, which is lazily evaluated.</param>
        public T UnwrapOr(T fallback)
            => IsSome ? _some : fallback;

        /// <summary>
        /// Unwraps <b>self</b> and returns <b>Some</b> if <b>self</b> contains <b>Some</b>. Returns the result of <paramref name="fallback"/> otherwise.
        /// </summary>
        /// <param name="fallback">The function to call if <b>self</b> contains <b>None</b>.</param>
        public T UnwrapOrElse(Func<T> fallback)
        {
            if (IsSome)
                return _some;

            if (fallback is null)
                throw new ArgumentNullException(nameof(fallback));

            return fallback();
        }

        /// <summary>
        /// Unwraps <b>self</b> and returns <b>Some</b> if <b>self</b> contains <b>Some</b>. Returns the default value of <typeparamref name="T"/> otherwise.
        /// </summary>
        public T UnwrapOrDefault()
            => IsSome ? _some : default;

        #endregion

        /// <summary>
        /// Returns <b>None</b> if the option is <b>None</b>. Otherwise calls <paramref name="filter"/> with the wrapped value and returns:
        /// <b>Some(t)</b> if <paramref name="filter"/> returns <b>true</b> (where t is the wrapped value), and
        /// <b>None</b> if <paramref name="filter"/> returns <b>false</b>.
        /// You can imagine the <see cref="Option{T}"/> being an iterator over one or zero elements. <see cref="Filter(Func{T, bool})"/> lets you decide which elements to keep.
        /// </summary>
        /// <param name="filter">The filter to apply.</param>
        public Option<T> Filter(Func<T, bool> filter)
        {
            var this_ = this;

            return Match(
                v => filter(v) ? this_ : None,
                () => None
            );
        }

        /// <summary>
        /// Returns true if the option contains the given value.
        /// </summary>
        /// <param name="value">The value to check against.</param>
        public bool Contains(T value)
        {
            return Match(v => v.Equals(value), () => false);
        }

        /// <summary>
        /// Returns the contained value as an <see cref="object"/> if the option contains <b>Some</b>.
        /// Returns the <b>null</b> reference otherwise.
        /// </summary>
        object IOption.ToObject()
            => IsSome ? _some : default(object);

        #region Equals, GetHashCode & ToString

        public override bool Equals(object other)
            => other is Option<T> option && Equals(option);

        public bool Equals(Option<T> other)
        {
            return
                IsSome
                    ? other.IsSome && _some.Equals(other._some)
                    : other.IsSome == false
                ;
        }

        public static bool operator ==(Option<T> a, Option<T> b)
            => a.Equals(b);

        public static bool operator !=(Option<T> a, Option<T> b)
            => a.Equals(b) == false;

        public override int GetHashCode()
            => (IsSome, _some).GetHashCode();

        public override string ToString()
            => Match(some => some.ToString(), () => Functional.None.Value.ToString());

        #endregion
    }
}
