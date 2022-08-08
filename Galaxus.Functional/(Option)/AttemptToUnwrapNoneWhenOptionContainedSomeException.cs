using System;

namespace Galaxus.Functional
{
    /// <summary>
    ///     Thrown when an attempt was made to unwrap an <see cref="Option{T}" /> containing <see cref="None" />.
    /// </summary>
    public class AttemptToUnwrapNoneWhenOptionContainedSomeException : Exception
    {
        /// <summary>
        ///     Create an <see cref="AttemptToUnwrapNoneWhenOptionContainedSomeException" /> object.
        /// </summary>
        /// <param name="message">The message for the exception.</param>
        public AttemptToUnwrapNoneWhenOptionContainedSomeException(string message) : base(message)
        {
        }
    }
}
