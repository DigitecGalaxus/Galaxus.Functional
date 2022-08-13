using System;

namespace Galaxus.Functional
{
    /// <summary>
    ///     Thrown when an attempt was made to unwrap an <see cref="Option{T}" /> containing <see cref="None" />.
    /// </summary>
    public sealed class TriedToUnwrapNoneException : Exception
    {
        /// <summary>
        ///     Create an <see cref="TriedToUnwrapNoneException" /> object.
        /// </summary>
        /// <param name="message">The message for the exception.</param>
        public TriedToUnwrapNoneException(string message) : base(message)
        {
        }
    }
}
