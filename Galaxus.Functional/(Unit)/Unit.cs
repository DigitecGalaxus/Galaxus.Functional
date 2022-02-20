using System;

namespace Galaxus.Functional
{
    /// <summary>
    ///     The <see cref="Unit" /> type has exactly one value, and is used when there is no other meaningful value that could
    ///     be used.
    ///     It is most commonly seen in combination with <see cref="Result{TOk, TErr}" />s which indicate failure but do not
    ///     return a meaningful error.
    /// </summary>
    public struct Unit : IEquatable<Unit>
    {
        /// <summary>
        ///     The unit value.
        /// </summary>
        public static readonly Unit Value = default;

        /// <inheritdoc />
        public override bool Equals(object other)
        {
            return other is Unit;
        }

        /// <inheritdoc />
        public bool Equals(Unit obj)
        {
            return true;
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            return -1;
        }

        /// <inheritdoc />
        public override string ToString()
        {
            return "()";
        }
    }
}
