using System;

namespace Galaxus.Functional
{
    /// <summary>
    ///     Thrown when an attempt was made to unwrap a <see cref="Result{TOk, TErr}" /> containing <b>Err</b>.
    /// </summary>
    public class TriedToUnwrapErrException : Exception
    {
        /// <summary>
        ///     Create an <see cref="TriedToUnwrapErrException" /> object.
        /// </summary>
        /// <param name="message">The message for the exception.</param>
        public TriedToUnwrapErrException(string message) : base(message)
        {
        }
    }
}
