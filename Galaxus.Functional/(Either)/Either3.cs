using System;

namespace Galaxus.Functional
{
    /// <summary>
    /// The <see cref="Either{A, B, C}"/> is a discriminated union which is a data structure used to hold a value that could take on three different, but fixed, types.
    /// Only one of the types can be in use at any one time, and a discriminant explicitly indicates which one is in use.
    /// It can be thought of as a type that has several "cases", each of which should be handled correctly when that type is accessed.
    /// </summary>
    /// <typeparam name="A">The first type this union can contain.</typeparam>
    /// <typeparam name="B">The second type this union can contain.</typeparam>
    /// <typeparam name="C">The third type this union can contain.</typeparam>
    public class Either<A, B, C> : IEquatable<Either<A, B, C>>, IEither
    {
        #region Type Initializer

        private static readonly bool _tAIsValueType;
        private static readonly bool _tBIsValueType;
        private static readonly bool _tCIsValueType;

        static Either()
        {
            _tAIsValueType = typeof(A).IsValueType;
            _tBIsValueType = typeof(B).IsValueType;
            _tCIsValueType = typeof(C).IsValueType;
        }

        #endregion

        #region Instance Initializer

        /// <summary>
        /// Constructs a union in which field "A" is in use.
        /// </summary>
        public Either(A a)
        {
            if (_tAIsValueType == false && a == null)
                throw new ArgumentNullException(nameof(a));

            Discriminant = Discriminant3.A;
            _a = a;
        }

        /// <summary>
        /// Constructs a union in which field "B" is in use.
        /// </summary>
        public Either(B b)
        {
            if (_tBIsValueType == false && b == null)
                throw new ArgumentNullException(nameof(b));

            Discriminant = Discriminant3.B;
            _b = b;
        }

        /// <summary>
        /// Constructs a union in which field "C" is in use.
        /// </summary>
        public Either(C c)
        {
            if (_tCIsValueType == false && c == null)
                throw new ArgumentNullException(nameof(c));

            Discriminant = Discriminant3.C;
            _c = c;
        }

        // Usages of implicit operators will only compile if A, B and C are different types.

        public static implicit operator Either<A, B, C>(A value)
            => new Either<A, B, C>(value);

        public static implicit operator Either<A, B, C>(B value)
            => new Either<A, B, C>(value);

        public static implicit operator Either<A, B, C>(C value)
            => new Either<A, B, C>(value);

        #endregion

        #region State

        protected readonly A _a;
        protected readonly B _b;
        protected readonly C _c;

        /// <summary>
        /// Indicates which field is in use.
        /// </summary>
        public Discriminant3 Discriminant { get; }

        #endregion

        #region Match

        /// <summary>
        /// Provides access to <b>self</b>'s content by calling either <paramref name="onA"/>, <paramref name="onB"/> or <paramref name="onC"/> and passing in <b>self</b>'s content.
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
                        throw new ArgumentNullException(nameof(onA));

                    onA(_a);
                    break;

                case Discriminant3.B:
                    if (onB == null)
                        throw new ArgumentNullException(nameof(onB));

                    onB(_b);
                    break;

                case Discriminant3.C:
                    if (onC == null)
                        throw new ArgumentNullException(nameof(onC));

                    onC(_c);
                    break;

                default:
                    throw new InvalidOperationException($"{GetType()} has an invalid discriminant. This is an implementation bug.");
            }
        }

        /// <summary>
        /// Provides access to <b>self</b>'s content by calling either <paramref name="onA"/>, <paramref name="onB"/> or <paramref name="onC"/> and passing in <b>self</b>'s content.
        /// </summary>
        /// <param name="onA">Called when field "A" is in use. The argument to this action is never the <b>null</b> reference.</param>
        /// <param name="onB">Called when field "B" is in use. The argument to this action is never the <b>null</b> reference.</param>
        /// <param name="onC">Called when field "C" is in use. The argument to this action is never the <b>null</b> reference.</param>
        public T Match<T>(Func<A, T> onA, Func<B, T> onB, Func<C, T> onC)
        {
            switch (Discriminant)
            {
                case Discriminant3.A:
                    if(onA == null)
                        throw new ArgumentNullException(nameof(onA));

                    return onA(_a);

                case Discriminant3.B:
                    if (onB == null)
                        throw new ArgumentNullException(nameof(onB));

                    return onB(_b);

                case Discriminant3.C:
                    if (onC == null)
                        throw new ArgumentNullException(nameof(onC));

                    return onC(_c);

                default:
                    throw new InvalidOperationException($"{GetType()} has an invalid discriminant. This is an implementation bug.");
            }
        }

        #endregion

        object IEither.ToObject()
            => Match<object>(a => a, b => b, c => c);

        #region Equals, GetHashCode & ToString

        public sealed override bool Equals(object other)
            => Equals(other as Either<A, B, C>);

        public bool Equals(Either<A, B, C> other)
        {
            if (other is null)
                return false;

            if (ReferenceEquals(this, other))
                return true;

            if (Discriminant != other.Discriminant)
                return false;

            switch (Discriminant)
            {
                case Discriminant3.A:
                    return _a.Equals(other._a);
                case Discriminant3.B:
                    return _b.Equals(other._b);
                case Discriminant3.C:
                    return _c.Equals(other._c);
                default:
                    throw new InvalidOperationException($"{GetType()} has an invalid discriminant. This is an implementation bug.");
            }
        }

        public static bool operator ==(Either<A, B, C> lhs, Either<A, B, C> rhs)
        {
            if (lhs is null)
                return rhs is null;

            return lhs.Equals(rhs);
        }

        public static bool operator !=(Either<A, B, C> lhs, Either<A, B, C> rhs)
            => (lhs == rhs) == false;

        public override int GetHashCode()
            => (Discriminant, _a, _b, _c).GetHashCode();

        public override string ToString()
            => Match(a => a.ToString(), b => b.ToString(), c => c.ToString());

        #endregion
    }
}
