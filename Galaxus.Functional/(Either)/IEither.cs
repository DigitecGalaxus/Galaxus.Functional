namespace Galaxus.Functional
{
    /// <summary>
    /// General interface for objects that roughly represent a discriminated union.
    /// </summary>
    public interface IEither
    {
        /// <summary>
        /// Returns the contained value as an <see cref="object"/>.
        /// </summary>
        object ToObject();
    }
}
