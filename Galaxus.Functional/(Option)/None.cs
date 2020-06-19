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
        public static readonly None Value;

        public override bool Equals(object other)
            => other is None;

        public bool Equals(None obj)
            => true;

        public override int GetHashCode()
            => -1;

        public override string ToString()
            => "None";
    }
}
