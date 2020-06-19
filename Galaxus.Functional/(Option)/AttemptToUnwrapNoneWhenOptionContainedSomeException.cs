using System;

namespace Galaxus.Functional
{
    /// <summary>
    /// Thrown when an attempt was made to unwrap an <see cref="Option{T}"/> containing <see cref="None"/>.
    /// </summary>
    public class AttemptToUnwrapNoneWhenOptionContainedSomeException : Exception
    {
        public AttemptToUnwrapNoneWhenOptionContainedSomeException(string message) : base(message)
        {
        }
    }
}
