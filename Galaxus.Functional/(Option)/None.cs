using System;

namespace Galaxus.Functional
{
    /// <summary>
    /// Denotes the absence of <b>Some</b>.
    /// This type is most often used to implicitly convert from its value to an <see cref="Option{T}"/>.
    /// </summary>
    public struct None : IEquatable<None>
    {
        /// <summary>
        /// The <b>None</b> value.
        /// </summary>
        public static readonly None Value = default;

        /// <inheritdoc />
        public override bool Equals(object other)
            => other is None;

        /// <inheritdoc />
        public bool Equals(None obj)
            => true;

        /// <inheritdoc />
        public override int GetHashCode()
            => -1;

        /// <inheritdoc />
        public override string ToString()
            => "None";
    }
}
