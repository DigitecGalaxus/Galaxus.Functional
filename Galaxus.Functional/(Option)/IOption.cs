namespace Galaxus.Functional
{
    /// <summary>
    ///     General interface for objects that represent an optional value.
    /// </summary>
    public interface IOption
    {
        /// <summary>
        ///     Returns <b>Some</b> as an <see cref="object" /> if <b>self</b> contains <b>Some</b>.
        ///     Returns <b>null</b> otherwise.
        /// </summary>
        object ToObject();
    }
}
