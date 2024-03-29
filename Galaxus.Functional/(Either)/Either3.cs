using System;

namespace Galaxus.Functional
{
    /// <summary>
    ///     The <see cref="Either{A, B, C}" /> is a discriminated union which is a data structure used to hold a value that
    ///     could take on three different, but fixed, types.
    ///     Only one of the types can be in use at any one time, and a discriminant explicitly indicates which one is in use.
    ///     It can be thought of as a type that has several "cases", each of which should be handled correctly when that type
    ///     is accessed.
    /// </summary>
    /// <typeparam name="A">The first type this union can contain.</typeparam>
    /// <typeparam name="B">The second type this union can contain.</typeparam>
    /// <typeparam name="C">The third type this union can contain.</typeparam>
    public class Either<A, B, C> : IEquatable<Either<A, B, C>>, IEither
    {
        object IEither.ToObject()
        {
            return Match<object>(a => a, b => b, c => c);
        }

        #region Instance Initializer

        /// <summary>
        ///     Constructs a union in which field "A" is in use.
        /// </summary>
        public Either(A a)
        {
            if (typeof(A).IsValueType == false && a == null)
            {
                throw new ArgumentNullException(nameof(a));
            }

            Discriminant = Discriminant3.A;
            _a = a;
        }

        /// <summary>
        ///     Constructs a union in which field "B" is in use.
        /// </summary>
        public Either(B b)
        {
            if (typeof(B).IsValueType == false && b == null)
            {
                throw new ArgumentNullException(nameof(b));
            }

            Discriminant = Discriminant3.B;
            _b = b;
        }

        /// <summary>
        ///     Constructs a union in which field "C" is in use.
        /// </summary>
        public Either(C c)
        {
            if (typeof(C).IsValueType == false && c == null)
            {
                throw new ArgumentNullException(nameof(c));
            }

            Discriminant = Discriminant3.C;
            _c = c;
        }

        // Usages of implicit operators will only compile if A, B and C are different types.

        /// <summary>
        ///     Implicitly cast a value of type <typeparamref name="A"/> to an <see cref="Either{A,B,C}" /> containing A.
        /// </summary>
        /// <param name="value">The value to cast.</param>
        /// <returns>An either containing that value as A.</returns>
        public static implicit operator Either<A, B, C>(A value)
        {
            return new Either<A, B, C>(value);
        }

        /// <summary>
        ///     Implicitly cast a value of type <typeparamref name="B"/> to an <see cref="Either{A,B,C}" /> containing B.
        /// </summary>
        /// <param name="value">The value to cast.</param>
        /// <returns>An either containing that value as B.</returns>
        public static implicit operator Either<A, B, C>(B value)
        {
            return new Either<A, B, C>(value);
        }

        /// <summary>
        ///     Implicitly cast a value of type <typeparamref name="C"/> to an <see cref="Either{A,B,C}" /> containing C.
        /// </summary>
        /// <param name="value">The value to cast.</param>
        /// <returns>An either containing that value as C.</returns>
        public static implicit operator Either<A, B, C>(C value)
        {
            return new Either<A, B, C>(value);
        }

        #endregion

        #region State

        /// <summary>
        ///     The value of the first type that may be contained.
        /// </summary>
        /// <remarks>
        ///     When this either <see cref="IsB"/> or <see cref="IsC"/>, this value is <c>default</c>.
        /// </remarks>
        protected readonly A _a;

        /// <summary>
        ///     The value of the second type that may be contained.
        /// </summary>
        /// <remarks>
        ///     When this either <see cref="IsA"/> or <see cref="IsC"/>, this value is <c>default</c>.
        /// </remarks>
        protected readonly B _b;

        /// <summary>
        ///     The value of the third type that may be contained.
        /// </summary>
        /// <remarks>
        ///     When this either <see cref="IsA"/> or <see cref="IsB"/>, this value is <c>default</c>.
        /// </remarks>
        protected readonly C _c;

        /// <summary>
        ///     Indicates which field is in use.
        /// </summary>
        public Discriminant3 Discriminant { get; }

        /// <summary>
        ///     Indicates that field A is in use.
        /// </summary>
        public bool IsA => Discriminant == Discriminant3.A;

        /// <summary>
        ///     Indicates that field B is in use.
        /// </summary>
        public bool IsB => Discriminant == Discriminant3.B;

        /// <summary>
        ///     Indicates that field C is in use.
        /// </summary>
        public bool IsC => Discriminant == Discriminant3.C;

        #endregion

        #region Match

        /// <summary>
        ///     Provides access to <b>self</b>'s content by calling either <paramref name="onA" />, <paramref name="onB" /> or
        ///     <paramref name="onC" /> and passing in <b>self</b>'s content.
        /// </summary>
        /// <param name="onA">Called when field "A" is in use. The argument to this action is never the <b>null</b> reference.</param>
        /// <param name="onB">Called when field "B" is in use. The argument to this action is never the <b>null</b> reference.</param>
        /// <param name="onC">Called when field "C" is in use. The argument to this action is never the <b>null</b> reference.</param>
        public void Match(Action<A> onA, Action<B> onB, Action<C> onC)
        {
            switch (Discriminant)
            {
                case Discriminant3.A:
                    if (onA == null)
                    {
                        throw new ArgumentNullException(nameof(onA));
                    }

                    onA(_a);
                    break;

                case Discriminant3.B:
                    if (onB == null)
                    {
                        throw new ArgumentNullException(nameof(onB));
                    }

                    onB(_b);
                    break;

                case Discriminant3.C:
                    if (onC == null)
                    {
                        throw new ArgumentNullException(nameof(onC));
                    }

                    onC(_c);
                    break;

                default:
                    throw new InvalidOperationException(
                        $"{GetType()} has an invalid discriminant. This is an implementation bug.");
            }
        }

        /// <summary>
        ///     Provides access to <b>self</b>'s content by calling either <paramref name="onA" />, <paramref name="onB" /> or
        ///     <paramref name="onC" /> and passing in <b>self</b>'s content.
        /// </summary>
        /// <param name="onA">Called when field "A" is in use. The argument to this action is never the <b>null</b> reference.</param>
        /// <param name="onB">Called when field "B" is in use. The argument to this action is never the <b>null</b> reference.</param>
        /// <param name="onC">Called when field "C" is in use. The argument to this action is never the <b>null</b> reference.</param>
        public T Match<T>(Func<A, T> onA, Func<B, T> onB, Func<C, T> onC)
        {
            switch (Discriminant)
            {
                case Discriminant3.A:
                    if (onA == null)
                    {
                        throw new ArgumentNullException(nameof(onA));
                    }

                    return onA(_a);

                case Discriminant3.B:
                    if (onB == null)
                    {
                        throw new ArgumentNullException(nameof(onB));
                    }

                    return onB(_b);

                case Discriminant3.C:
                    if (onC == null)
                    {
                        throw new ArgumentNullException(nameof(onC));
                    }

                    return onC(_c);

                default:
                    throw new InvalidOperationException(
                        $"{GetType()} has an invalid discriminant. This is an implementation bug.");
            }
        }

        #endregion

        #region Equals, GetHashCode & ToString

        /// <inheritdoc />
        public sealed override bool Equals(object other)
        {
            return Equals(other as Either<A, B, C>);
        }

        /// <inheritdoc />
        public bool Equals(Either<A, B, C> other)
        {
            if (other is null)
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            if (Discriminant != other.Discriminant)
            {
                return false;
            }

            switch (Discriminant)
            {
                case Discriminant3.A:
                    return _a.Equals(other._a);
                case Discriminant3.B:
                    return _b.Equals(other._b);
                case Discriminant3.C:
                    return _c.Equals(other._c);
                default:
                    throw new InvalidOperationException(
                        $"{GetType()} has an invalid discriminant. This is an implementation bug.");
            }
        }

        /// <summary>
        ///     Checks whether two eithers are equal.
        /// </summary>
        /// <param name="lhs">The either to check against.</param>
        /// <param name="rhs">The either to check.</param>
        /// <returns><c>True</c> if the eithers are equal, <c>false</c> otherwise.</returns>
        public static bool operator ==(Either<A, B, C> lhs, Either<A, B, C> rhs)
        {
            if (lhs is null)
            {
                return rhs is null;
            }

            return lhs.Equals(rhs);
        }

        /// <summary>
        ///     Checks whether two eithers are not equal.
        /// </summary>
        /// <param name="lhs">The either to check against.</param>
        /// <param name="rhs">The either to check.</param>
        /// <returns><c>False</c> if the eithers are equal, <c>true</c> otherwise.</returns>
        public static bool operator !=(Either<A, B, C> lhs, Either<A, B, C> rhs)
        {
            return lhs == rhs == false;
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            return (Discriminant, _a, _b, _c).GetHashCode();
        }

        /// <inheritdoc />
        public override string ToString()
        {
            return Match(a => a.ToString(), b => b.ToString(), c => c.ToString());
        }

        #endregion
    }
}
