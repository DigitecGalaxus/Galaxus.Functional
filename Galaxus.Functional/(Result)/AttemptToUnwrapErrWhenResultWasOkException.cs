using System;

namespace Galaxus.Functional
{
    /// <summary>
    ///     Thrown when an attempt was made to unwrap a <see cref="Result{TOk, TErr}" /> containing <b>Err</b>.
    /// </summary>
    public class AttemptToUnwrapErrWhenResultWasOkException : Exception
    {
        /// <summary>
        ///     Create an <see cref="AttemptToUnwrapErrWhenResultWasOkException" /> object.
        /// </summary>
        /// <param name="message">The message for the exception.</param>
        public AttemptToUnwrapErrWhenResultWasOkException(string message) : base(message: message)
        {
        }
    }
}
