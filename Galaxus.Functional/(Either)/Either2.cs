using System;

namespace Galaxus.Functional
{
    /// <summary>
    /// The <see cref="Either{A, B}"/> is a discriminated union which is a data structure used to hold a value that could take on two different, but fixed, types.
    /// Only one of the types can be in use at any one time, and a discriminant explicitly indicates which one is in use.
    /// It can be thought of as a type that has several "cases", each of which should be handled correctly when that type is accessed.
    /// </summary>
    /// <typeparam name="A">The first type this union can contain.</typeparam>
    /// <typeparam name="B">The second type this union can contain.</typeparam>
    public class Either<A, B> : IEquatable<Either<A, B>>, IEither
    {
        #region Type Initializer

        private static readonly bool _tAIsValueType;
        private static readonly bool _tBIsValueType;

        static Either()
        {
            _tAIsValueType = typeof(A).IsValueType;
            _tBIsValueType = typeof(B).IsValueType;
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

            Discriminant = Discriminant2.A;
            _a = a;
        }

        /// <summary>
        /// Constructs a union in which field "B" is in use.
        /// </summary>
        public Either(B b)
        {
            if (_tBIsValueType == false && b == null)
                throw new ArgumentNullException(nameof(b));

            Discriminant = Discriminant2.B;
            _b = b;
        }

        // Usages of implicit operators will only compile if A and B are different types.

        public static implicit operator Either<A, B>(A a)
            => new Either<A, B>(a);

        public static implicit operator Either<A, B>(B b)
            => new Either<A, B>(b);

        #endregion

        #region State

        protected readonly A _a;
        protected readonly B _b;

        /// <summary>
        /// Indicates which field is in use.
        /// </summary>
        public Discriminant2 Discriminant { get; }

        #endregion

        #region Match

        /// <summary>
        /// Provides access to <b>self</b>'s content by calling either <paramref name="onA"/> or <paramref name="onB"/> and passing in <b>self</b>'s content.
        /// </summary>
        /// <param name="onA">Called when field "A" is in use. The argument to this action is never the <b>null</b> reference.</param>
        /// <param name="onB">Called when field "B" is in use. The argument to this action is never the <b>null</b> reference.</param>
        public void Match(Action<A> onA, Action<B> onB)
        {
            switch (Discriminant)
            {
                case Discriminant2.A:
                    if (onA == null)
                        throw new ArgumentNullException(nameof(onA));

                    onA(_a);
                    break;
                case Discriminant2.B:
                    if (onB == null)
                        throw new ArgumentNullException(nameof(onB));

                    onB(_b);
                    break;
                default:
                    throw new InvalidOperationException($"{GetType()} has an invalid discriminant. This is an implementation bug.");
            }
        }

        /// <summary>
        /// Provides access to <b>self</b>'s content by calling either <paramref name="onA"/> or <paramref name="onB"/> and passing in <b>self</b>'s content.
        /// </summary>
        /// <param name="onA">Called when field "A" is in use. The argument to this action is never the <b>null</b> reference.</param>
        /// <param name="onB">Called when field "B" is in use. The argument to this action is never the <b>null</b> reference.</param>
        public T Match<T>(Func<A, T> onA, Func<B, T> onB)
        {
            switch (Discriminant)
            {
                case Discriminant2.A:
                    if (onA == null)
                        throw new ArgumentNullException(nameof(onA));

                    return onA(_a);
                case Discriminant2.B:
                    if (onB == null)
                        throw new ArgumentNullException(nameof(onB));

                    return onB(_b);
                default:
                    throw new InvalidOperationException($"{GetType()} has an invalid discriminant. This is an implementation bug.");
            }
        }

        #endregion

        object IEither.ToObject()
            => Match<object>(a => a, b => b);

        #region Equals, GetHashCode & ToString

        public sealed override bool Equals(object other)
            => Equals(other as Either<A, B>);

        public bool Equals(Either<A, B> other)
        {
            if (other is null)
                return false;

            if (ReferenceEquals(this, other))
                return true;

            if (Discriminant != other.Discriminant)
                return false;

            switch (Discriminant)
            {
                case Discriminant2.A:
                    return _a.Equals(other._a);
                case Discriminant2.B:
                    return _b.Equals(other._b);
                default:
                    throw new InvalidOperationException($"{GetType()} has an invalid discriminant. This is an implementation bug.");
            }
        }

        public static bool operator ==(Either<A, B> lhs, Either<A, B> rhs)
        {
            if (lhs is null)
                return rhs is null;

            return lhs.Equals(rhs);
        }

        public static bool operator !=(Either<A, B> lhs, Either<A, B> rhs)
            => (lhs == rhs) == false;

        public override int GetHashCode()
            => (Discriminant, _a, _b).GetHashCode();

        public override string ToString()
            => Match(a => a.ToString(), b => b.ToString());

        #endregion
    }
}
