using System;

namespace Galaxus.Functional
{
    /// <summary>
    /// Thrown when an attempt was made to unwrap a <see cref="Result{TOk, TErr}"/> containing <b>Err</b>.
    /// </summary>
    public class AttemptToUnwrapErrWhenResultWasOkException : Exception
    {
        public AttemptToUnwrapErrWhenResultWasOkException(string message) : base(message)
        {
        }
    }
}
