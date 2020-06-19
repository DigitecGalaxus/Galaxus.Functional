namespace Galaxus.Functional
{
    /// <summary>
    /// <see cref="IError"/> is an interface representing the basic expectations for error values, i.e., values of type <b>TErr</b> in <see cref="Result{TOk, TErr}"/>.
    /// </summary>
    public interface IError
    {
        /// <summary>
        /// A description of the error.
        /// </summary>
        string Description { get; }

        /// <summary>
        /// The lower-level cause of this error, if any.
        /// </summary>
        Option<IError> Cause { get; }
    }
}
